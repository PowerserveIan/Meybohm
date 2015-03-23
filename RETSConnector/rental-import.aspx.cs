using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using BaseCode;
using Classes.Media352_MembershipProvider;
using Classes.MLS;
using Classes.Showcase;
using Classes.StateAndCountry;
using Classes.WhatsNearBy;
using Settings = Classes.Showcase.Settings;

public partial class RentalImport : System.Web.UI.Page
{
	protected string[] ArrayOfActiveStatusCodes = { "Active" };
	protected List<string> Cities = new List<string>(new[] { "Augusta", "Aiken" });
	protected List<RetsMapping> PropertyMap = new List<RetsMapping>();
	protected List<RetsMapping> OfficeMap = new List<RetsMapping>();
	protected List<RetsMapping> AgentMap = new List<RetsMapping>();
	protected List<WhatsNearByLocation> Locations;
	protected List<Neighborhood> AllNeighborhoods;

	private int m_ElementarySchoolCategoryID = WhatsNearByCategory.WhatsNearByCategoryGetByName("Elementary Schools").FirstOrDefault().WhatsNearByCategoryID;
	private int m_MiddleSchoolCategoryID = WhatsNearByCategory.WhatsNearByCategoryGetByName("Middle Schools").FirstOrDefault().WhatsNearByCategoryID;
	private int m_HighSchoolCategoryID = WhatsNearByCategory.WhatsNearByCategoryGetByName("High Schools").FirstOrDefault().WhatsNearByCategoryID;

	private TextInfo textInfo;

	protected void Page_Load(object sender, EventArgs e)
	{
		//FixVirtualTourUrls();
		//return;
		textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
		Locations = WhatsNearByLocation.GetAll();
		AllNeighborhoods = Neighborhood.GetAll();
		PropertyMap = SetPropertyColumnMap();
		Cities.ForEach(city =>
			{
				var ds = new DataSet();
				var reader = new StreamReader(Server.MapPath("../RetsConnector/RentalData/Rentals-" + city + ".csv"));
				DataTable table = CsvParser.Parse(reader);
				if (table != null)
				{
					ds.Tables.Add(table);
					ds.Tables[0].TableName = "Properties";
					List<string> newNames = SetDataTableHeaders(ds, out ds);
					List<string> attributes = newNames;
					// remove any columns that aren't attributes
					foreach (RetsMapping retsMapping in PropertyMap.Where(r => !r.Attribute))
					{
						attributes.Remove(retsMapping.RetsName);
					}

					AddNewAttributes(ds, attributes, PropertyMap, city);
					ImportProperties(newNames, ds);
				}
			});
		//UpdateAllGeolocation();
		Helpers.PurgeCacheItems("Showcase");
	}

	void FixVirtualTourUrls()
	{
		List<ShowcaseItem> allItems = ShowcaseItem.GetAll();
		foreach (ShowcaseItem item in allItems)
		{
			if (item.VirtualTourURL == string.Empty)
			{
				item.VirtualTourURL = null;
				item.Save();
			}
			else if (!String.IsNullOrWhiteSpace(item.VirtualTourURL) && !item.VirtualTourURL.ToLower().StartsWith("http"))
			{
				item.VirtualTourURL = "http://" + item.VirtualTourURL;
				item.Save();
			}
		}
	}

	#region Data Handling
	private static void AddNewAttributes(DataSet ds, List<string> attributes, List<RetsMapping> propertyMap, string city)
	{
		Helpers.PurgeCacheItems("Showcase");
		// Get all existing attributes
		List<Showcases> showcases = Showcases.GetAll().Where(s => s.Title.ToLower().Contains(city.ToLower()) && !s.MLSData).ToList();
		var allAttrs = ShowcaseAttribute.GetAll();
		var currentAttrs = new List<ShowcaseAttribute>();
		foreach (Showcases showcase in showcases)
		{
			currentAttrs.AddRange(allAttrs.Where(a => a.ShowcaseID == showcase.ShowcaseID));
		}

		int maxDisplayOrder = currentAttrs.Any() ? ((currentAttrs.Max(a => a.DisplayOrder)) ?? 1) : 1;
		// Check if each column is an attribute
		attributes.ForEach(i =>
		{
			string attributeTitle = (propertyMap.Any(r => r.RetsName == i) ? propertyMap.Find(r => r.RetsName == i).FieldName : i).TrimEnd();
			// If not add the attribute
			if (!currentAttrs.Any(j => attributeTitle.Equals(j.Title)) && !(attributeTitle.Equals("Photo location")))
			{
				bool firstrow = true;
				bool numeric = false;
				foreach (DataRow drCurrent in ds.Tables["Properties"].Rows)
				{
					if (firstrow)
					{
						firstrow = false;// skip column header
						continue;
					}
					decimal number;
					numeric = (!string.IsNullOrEmpty(drCurrent[i.TrimEnd()].ToString()) &&
							   decimal.TryParse(drCurrent[i.TrimEnd()].ToString(), out number));
					break;
				}

				foreach (Showcases showcase in showcases)
				{
					var attr = new ShowcaseAttribute
					{
						Title = attributeTitle,
						DisplayOrder = (short?)maxDisplayOrder,
						Active = true,
						ShowcaseFilterID = numeric ? (int)FilterTypes.RangeSlider : (int)FilterTypes.DropDown,
						Numeric = numeric,
						ShowcaseID = showcase.ShowcaseID,
						ImportItemAttribute = true
					};
					attr.Save();
					//attrs.Add(attr);
				}
				++maxDisplayOrder;
			}

		});
	}

	private void ImportProperties(List<string> colNames, DataSet ds)
	{
		int counter = 0;
		foreach (DataRow dr in ds.Tables["Properties"].Rows)
		{
			if (counter++ == 0) continue;// skips header row in csv- cannot remove header row needed to associate filed names
			//try
			//{
			RetsMapping stateMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("StateOrProvince"));
			bool stateIsGA = stateMap != null && dr[stateMap.RetsName].ToString().Equals("GA");
			int showcaseForCurrentItem = stateIsGA ? (int)MeybohmShowcases.AugustaRentalHomes : (int)MeybohmShowcases.AikenRentalHomes;
			// get the existing item or a new one based on MLS number
			ShowcaseItem item = new ShowcaseItem();
			item = SaveShowcaseItem(item, ds, dr, showcaseForCurrentItem);

			// add the attribute values
			List<string> cols = colNames;
			// remove any columns that aren't attributes
			foreach (RetsMapping retsMapping in PropertyMap.Where(r => !r.Attribute))
			{
				cols.Remove(retsMapping.RetsName);
			}
			List<ShowcaseAttribute> attributesToImport = ShowcaseAttribute.ShowcaseAttributePage(0, 0, "", "", true, new ShowcaseAttribute.Filters { FilterShowcaseAttributeShowcaseID = item.ShowcaseID.ToString(CultureInfo.InvariantCulture), FilterShowcaseAttributeImportItemAttribute = true.ToString() });
			cols.ForEach(i =>
				{
					string attributeTitle = (PropertyMap.Any(r => r.RetsName == i) ? PropertyMap.Find(r => r.RetsName == i).FieldName : i).TrimEnd();
					// Using the generated objects here would be a huge bottle neck, so
					// a custom procedure was written to remove the overhead.
					// remove any columns that are not marked as an attribute to import in the admin
					string tempVal = dr[i].ToString();
					if (attributesToImport.Any(s => s.Title == attributeTitle) && tempVal != "NULL")
					{
						List<SqlParameter> sqlParams = new List<SqlParameter>();
						sqlParams.Add(new SqlParameter("@Value", tempVal));
						sqlParams.Add(new SqlParameter("@MLSAttributeName", attributeTitle));
						sqlParams.Add(new SqlParameter("@ShowcaseItemId", item.ShowcaseItemID));
						sqlParams.Add(new SqlParameter("@ShowcaseID", item.ShowcaseID));
						if (i.Equals("Features"))
							sqlParams.Add(new SqlParameter("@SplitDelimiter", ','));
						SqlHelper.ExecuteNonQuery(Settings.ConnectionString, CommandType.StoredProcedure, "CUSTOM_RETS_ShowcaseItemAttributeAssign", sqlParams.ToArray());
					}
				});
			GenerateMediaCollection(item);
			//}
			//catch (Exception ex)
			//{
			//	Helpers.LogException(ex);
			//}
		}
		Helpers.PurgeCacheItems("Showcase");
	}

	private ShowcaseItem SaveShowcaseItem(ShowcaseItem item, DataSet ds, DataRow dr, int showcaseForCurrentItem)
	{
		RetsMapping summary = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Note"));
		RetsMapping bedroomsMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Bedrooms"));
		RetsMapping listPriceMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Rental Price"));
		RetsMapping fullBathMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Full Baths"));
		RetsMapping halfBathMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Half Baths"));
		RetsMapping officeIdMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("OfficeName"));
		//RetsMapping agentMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("AgentID"));
		RetsMapping imageMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Filename"));
		RetsMapping dateAvailableMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("DateAvailable"));
		RetsMapping statusMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Status"));
		RetsMapping statsSentToWhoMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("OPTStats"));
		RetsMapping statsSentToEmailsMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("OPTEmail"));
		RetsMapping tempOldIDMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("RentalPK"));
		RetsMapping agentFirstMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("AgentFirstName"));
		RetsMapping agentLastMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("AgentLastName"));

		Address itemAddress = new Address();
		itemAddress = SetAddressFields(itemAddress, dr, null, PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Address1")), PropertyMap.FirstOrDefault(r => r.FieldName.Equals("City")), PropertyMap.FirstOrDefault(r => r.FieldName.Equals("StateOrProvince")), PropertyMap.FirstOrDefault(r => r.FieldName.Equals("ZipOrPostalCode")));
		int ms = itemAddress.StateID == (int)States.GA ? 2 : 3;
		Neighborhood itemNeighborhood = SetItemNeighborhood(ds, dr, itemAddress, ms);
		WhatsNearByLocation esLocation;
		WhatsNearByLocation msLocation;
		WhatsNearByLocation hsLocation = WhatsNearBySchools(dr, itemAddress, out esLocation, out msLocation);

		#region save ShowcaseItem information

		int bedrooms = bedroomsMap != null && ds.Tables[0].Columns.Contains(bedroomsMap.RetsName) && dr[bedroomsMap.RetsName].ToString().Trim() != "NULL" ? Convert.ToInt32(dr[bedroomsMap.RetsName].ToString().Trim()) : 0;
		int listPrice = listPriceMap != null && dr[listPriceMap.RetsName].ToString().Trim() != "NULL" ? Convert.ToInt32(dr[listPriceMap.RetsName].ToString().Trim()) : 0;
		int totalBaths = (fullBathMap != null && ds.Tables[0].Columns.Contains(fullBathMap.RetsName) && dr[fullBathMap.RetsName].ToString().Trim() != "NULL" ? Convert.ToInt32(dr[fullBathMap.RetsName].ToString()) : 0) + (halfBathMap != null && ds.Tables[0].Columns.Contains(halfBathMap.RetsName) && dr[halfBathMap.RetsName].ToString().Trim() != "NULL" ? Convert.ToInt32(dr[halfBathMap.RetsName].ToString().Trim()) : 0);
		if (summary != null && ds.Tables[0].Columns.Contains(summary.RetsName) && dr[summary.RetsName].ToString().Trim() != "NULL")
			item.Summary = dr[summary.RetsName].ToString().Trim();
		else
			item.Summary = string.Empty;

		if (officeIdMap != null && dr[officeIdMap.RetsName].ToString().Trim() != "NULL")
		{
			string officeName = dr[officeIdMap.RetsName].ToString().Trim().Replace("Prop Mgmt - ", "");
			Office tempOffice = Office.OfficePage(0, 1, officeName, "OfficeID", true).FirstOrDefault();
			item.OfficeID = tempOffice != null ? (int?)tempOffice.OfficeID : null;
		}

		if (dr[agentLastMap.RetsName].ToString().Trim() != "NULL")
		{
			UserInfo agentInfo = UserInfo.UserInfoPage(0, 1, "", "", true, new UserInfo.Filters { FilterUserInfoFirstName = dr[agentFirstMap.RetsName].ToString().Trim() != "NULL" ? dr[agentFirstMap.RetsName].ToString().Trim() : null, FilterUserInfoLastName = dr[agentLastMap.RetsName].ToString().Trim() }).FirstOrDefault();
			if (agentInfo != null)
				item.AgentID = agentInfo.UserID;
		}

		item.Active = dr[statusMap.RetsName].ToString().Trim() != "Inactive";
		item.NeighborhoodID = itemNeighborhood != null ? (int?)itemNeighborhood.NeighborhoodID : null;
		item.Featured = false; //TODO set featured for Meybohm properties
		item.ShowcaseID = showcaseForCurrentItem;
		item.AddressID = itemAddress.AddressID;
		item.Title = "Price: $" + listPrice + " Bedrooms: " + bedrooms + " Bathrooms: " + totalBaths;
		item.Image = dr[imageMap.RetsName].ToString().Trim() != "NULL" ? dr[imageMap.RetsName].ToString().Trim() : "";
		item.ListPrice = listPrice;
		item.HighSchoolID = hsLocation != null ? (int?)hsLocation.WhatsNearByLocationID : null;
		item.MiddleSchoolID = msLocation != null ? (int?)msLocation.WhatsNearByLocationID : null;
		item.ElementarySchoolID = esLocation != null ? (int?)esLocation.WhatsNearByLocationID : null;
		//UserOffice agentUser = UserOffice.UserOfficeGetByMlsID(dr[agentMap.RetsName].ToString().Trim()).FirstOrDefault();
		//Team team = Team.TeamGetByMlsID(dr[agentMap.RetsName].ToString().Trim()).FirstOrDefault();
		//item.AgentID = agentUser != null ? (int?)agentUser.UserID : null;
		//item.TeamID = team != null ? team.TeamID : (int?)null;
		DateTime availabilityDate;
		if (!string.IsNullOrEmpty(dr[dateAvailableMap.RetsName].ToString().Trim()) && DateTime.TryParse(dr[dateAvailableMap.RetsName].ToString().Trim(), out availabilityDate))
			item.AvailabilityDate = availabilityDate;
		else if (dr[dateAvailableMap.RetsName].ToString().Trim() == "Now")
			item.AvailabilityDate = DateTime.Now.Date;
		item.Rented = dr[statusMap.RetsName].ToString().Trim() == "Rented";
		item.StatsSentToAgent = dr[statsSentToWhoMap.RetsName].ToString().Trim() == "B" || dr[statsSentToWhoMap.RetsName].ToString().Trim() == "A";
		item.StatsSentToOwner = dr[statsSentToWhoMap.RetsName].ToString().Trim() == "B" || dr[statsSentToWhoMap.RetsName].ToString().Trim() == "S";
		item.EmailAddresses = dr[statsSentToEmailsMap.RetsName].ToString().Trim() != "NULL" ? dr[statsSentToEmailsMap.RetsName].ToString().Trim() : string.Empty;
		item.tempOldID = Convert.ToInt32(dr[tempOldIDMap.RetsName].ToString().Trim());
		item.Save();


		RetsMapping leaseBeginMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("LeaseBegins"));
		RetsMapping ownerFirstMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("OwnerFirstName"));
		RetsMapping ownerLastMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("OwnerLastName"));
		RetsMapping contactFirstMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("ContactFirstName"));
		RetsMapping contactLastMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("ContactLastName"));
		RetsMapping contactPhoneMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("ContactWorkPhone"));
		RetsMapping contactPhoneExtMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("ContactWorkPhoneExt"));

		ShowcaseItemRental rentalEntity = new ShowcaseItemRental();
		if (dr[contactFirstMap.RetsName].ToString().Trim() != "NULL" || dr[contactLastMap.RetsName].ToString().Trim() != "NULL")
			rentalEntity.ContactName = ((dr[contactFirstMap.RetsName].ToString().Trim() != "NULL" ? dr[contactFirstMap.RetsName].ToString().Trim() + " " : string.Empty) + (dr[contactLastMap.RetsName].ToString().Trim() != "NULL" ? dr[contactLastMap.RetsName].ToString().Trim() : string.Empty)).Trim();
		if (dr[contactPhoneMap.RetsName].ToString().Trim() != "NULL")
		{
			int ext;
			rentalEntity.ContactPhone = Helpers.FormatPhoneNumber(dr[contactPhoneMap.RetsName].ToString().Trim()) + (Int32.TryParse(dr[contactPhoneExtMap.RetsName].ToString().Trim(), out ext) ? " x " + ext : string.Empty);
		}
		DateTime leaseBeginDate;
		if (dr[leaseBeginMap.RetsName].ToString().Trim() != "NULL" && DateTime.TryParse(dr[leaseBeginMap.RetsName].ToString().Trim(), out leaseBeginDate))
			rentalEntity.LeaseBeginDate = leaseBeginDate;
		if (dr[ownerFirstMap.RetsName].ToString().Trim() != "NULL" || dr[ownerLastMap.RetsName].ToString().Trim() != "NULL")
			rentalEntity.OwnerName = ((dr[ownerFirstMap.RetsName].ToString().Trim() != "NULL" ? dr[ownerFirstMap.RetsName].ToString().Trim() + " " : string.Empty) + (dr[ownerLastMap.RetsName].ToString().Trim() != "NULL" ? dr[ownerLastMap.RetsName].ToString().Trim() : string.Empty)).Trim();
		rentalEntity.ShowcaseItemID = item.ShowcaseItemID;
		rentalEntity.Save();
		#endregion
		return item;
	}

	private WhatsNearByLocation WhatsNearBySchools(DataRow dr, Address itemAddress, out WhatsNearByLocation esLocation, out WhatsNearByLocation msLocation)
	{
		esLocation = msLocation = null;
		RetsMapping mSMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Middle School"));
		RetsMapping hSMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("High School"));
		RetsMapping eSMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Elementary School"));
		WhatsNearByLocation hsLocation = null;
		if (eSMap != null && dr[eSMap.RetsName].ToString().Trim() != "NULL")
		{
			esLocation = Locations.FirstOrDefault(l => l.Name.Equals(dr[eSMap.RetsName].ToString().Trim(), StringComparison.OrdinalIgnoreCase));
			if (esLocation == null)
			{
				Address esAddress = new Address
					{
						Address1 = textInfo.ToTitleCase(dr[eSMap.RetsName].ToString().ToLower()),
						City = itemAddress.City,
						StateID = itemAddress.StateID//,
						//Zip = itemAddress.Zip  //I don't think we can assume they have the same zipcode.
					};
				esAddress.Save();
				esLocation = new WhatsNearByLocation { Name = CleanUpSchoolName(textInfo.ToTitleCase(dr[eSMap.RetsName].ToString().Trim().ToLower())), AddressID = esAddress.AddressID, Active = false };
				esLocation.Save();

				new WhatsNearByLocationCategory { WhatsNearByCategoryID = m_ElementarySchoolCategoryID, WhatsNearByLocationID = esLocation.WhatsNearByLocationID }.Save();
				Locations.Add(esLocation);
			}
			if (esLocation.Name != CleanUpSchoolName(esLocation.Name)) { esLocation.Name = CleanUpSchoolName(esLocation.Name); esLocation.Save(); }
		}
		if (mSMap != null && dr[mSMap.RetsName].ToString().Trim() != "NULL")
		{
			msLocation = Locations.FirstOrDefault(l => l.Name.Equals(dr[mSMap.RetsName].ToString().Trim(), StringComparison.OrdinalIgnoreCase));
			if (msLocation == null)
			{
				Address msAddress = new Address
					{
						Address1 = textInfo.ToTitleCase(dr[mSMap.RetsName].ToString().Trim().ToLower()),
						City = itemAddress.City,
						StateID = itemAddress.StateID//,
						//Zip = itemAddress.Zip  //I don't think we can assume they have the same zipcode.
					};
				msAddress.Save();
				msLocation = new WhatsNearByLocation { Name = CleanUpSchoolName(textInfo.ToTitleCase(dr[mSMap.RetsName].ToString().Trim().ToLower())), AddressID = msAddress.AddressID, Active = false };
				msLocation.Save();

				new WhatsNearByLocationCategory { WhatsNearByCategoryID = m_MiddleSchoolCategoryID, WhatsNearByLocationID = msLocation.WhatsNearByLocationID }.Save();
				Locations.Add(msLocation);
			}
			if (msLocation.Name != CleanUpSchoolName(msLocation.Name)) { msLocation.Name = CleanUpSchoolName(msLocation.Name); msLocation.Save(); }
		}
		if (hSMap != null && dr[hSMap.RetsName].ToString().Trim() != "NULL")
		{
			hsLocation = Locations.FirstOrDefault(l => l.Name.Equals(dr[hSMap.RetsName].ToString().Trim(), StringComparison.OrdinalIgnoreCase));
			if (hsLocation == null)
			{
				Address hsAddress = new Address
					{
						Address1 = textInfo.ToTitleCase(dr[hSMap.RetsName].ToString().Trim().ToLower()),
						City = itemAddress.City,
						StateID = itemAddress.StateID//,
						//Zip = itemAddress.Zip  //I don't think we can assume they have the same zipcode.
					};
				hsAddress.Save();
				hsLocation = new WhatsNearByLocation { Name = CleanUpSchoolName(textInfo.ToTitleCase(dr[hSMap.RetsName].ToString().Trim().ToLower())), AddressID = hsAddress.AddressID, Active = false };
				hsLocation.Save();

				new WhatsNearByLocationCategory { WhatsNearByCategoryID = m_HighSchoolCategoryID, WhatsNearByLocationID = hsLocation.WhatsNearByLocationID }.Save();
				Locations.Add(hsLocation);
			}
			if (hsLocation.Name != CleanUpSchoolName(hsLocation.Name)) { hsLocation.Name = CleanUpSchoolName(hsLocation.Name); hsLocation.Save(); }
		}
		return hsLocation;
	}

	private string CleanUpSchoolName(string name)
	{
		CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
		TextInfo textInfo = cultureInfo.TextInfo;
		string newName = string.Empty;
		string[] temp = name.Split(' ');
		newName = temp.Aggregate(newName, (current, s) => current + (textInfo.ToTitleCase(s) + " "));
		return newName.Trim();
	}

	private Neighborhood SetItemNeighborhood(DataSet ds, DataRow dr, Address itemAddress, int ms)
	{
		RetsMapping neighborhoodMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Neighborhood"));
		if (neighborhoodMap != null && !string.IsNullOrEmpty(dr[neighborhoodMap.RetsName].ToString().Trim()) && dr[neighborhoodMap.RetsName].ToString().Trim() != "NULL")
		{
			Neighborhood itemNeighborhood = AllNeighborhoods.FirstOrDefault(n => n.Name.Equals(dr[neighborhoodMap.RetsName].ToString().Trim(), StringComparison.OrdinalIgnoreCase)) ??
							   new Neighborhood();
			Address neighborhoodAddress = itemNeighborhood.NeighborhoodID > 0 ? Address.GetByID(itemNeighborhood.AddressID) : new Address();
			if (neighborhoodAddress.AddressID < 1)
			{
				neighborhoodAddress.City = itemAddress.City;
				neighborhoodAddress.StateID = itemAddress.StateID;
				neighborhoodAddress.Zip = itemAddress.Zip;
				neighborhoodAddress.Save();
			}
			if (itemNeighborhood.NeighborhoodID <= 0)
			{
				itemNeighborhood.Active =
				itemNeighborhood.Featured = false;
			}
			itemNeighborhood.AddressID = neighborhoodAddress.AddressID;
			itemNeighborhood.Name = textInfo.ToTitleCase(dr[neighborhoodMap.RetsName].ToString().Trim().ToLower());
			itemNeighborhood.CMMicrositeID = ms;
			itemNeighborhood.Save();
			AllNeighborhoods.Add(itemNeighborhood);
			return itemNeighborhood;
		}
		return null;
	}

	private void GenerateMediaCollection(ShowcaseItem item)
	{
		if (item.ShowcaseItemID > 0)
		{
			// add the images
			MediaCollection collection = new MediaCollection
			{
				DisplayOrder = 1,
				Active = true,
				ShowcaseMediaTypeID = (int)MediaTypes.Image,
				TextBlock = "",
				Title = "Photos",
				ShowcaseItemID = item.ShowcaseItemID
			};
			collection.Save();
		}
	}

	private Address SetAddressFields(Address addressEntity, DataRow dr, RetsMapping streetNumberMap, RetsMapping addressMap, RetsMapping cityMap, RetsMapping stateMap, RetsMapping zipMap)
	{
		string zip = zipMap != null ? dr[zipMap.RetsName].ToString().Trim().Replace(".", "").Replace(",", "") : "";
		if (zip.Length > 5)
			zip = zip.ToString(CultureInfo.InvariantCulture).Substring(0, 5) + "-" +
				  zip.ToString(CultureInfo.InvariantCulture).Substring(5, zip.Length - 5);

		bool addressChanged = (addressMap != null && addressEntity.Address1 != (streetNumberMap != null ? dr[streetNumberMap.RetsName].ToString().Trim() + " " : "") + dr[addressMap.RetsName].ToString().Trim()) ||
								(cityMap != null && addressEntity.City != dr[cityMap.RetsName].ToString().Trim()) ||
								(stateMap != null && addressEntity.StateID != (dr[stateMap.RetsName].ToString().Trim() == "GA" ? (int)States.GA : (int)States.SC)) ||
								(zipMap != null && addressEntity.Zip != zip);
		if (addressMap != null)
			addressEntity.Address1 = textInfo.ToTitleCase((streetNumberMap != null ? dr[streetNumberMap.RetsName].ToString().Trim() + " " : "") + dr[addressMap.RetsName].ToString().Trim());
		if (cityMap != null)
			addressEntity.City = textInfo.ToTitleCase(dr[cityMap.RetsName].ToString().Trim());
		if (stateMap != null)
			addressEntity.StateID = dr[stateMap.RetsName].ToString().Trim() == "GA" ? (int)States.GA : (int)States.SC;
		if (zipMap != null)
			addressEntity.Zip = zip == "00000" || zip == "00000-0000" ? "" : zip;
		if (addressChanged)
			addressEntity.Latitude = addressEntity.Longitude = null;
		addressEntity.Save();

		return addressEntity;
	}
	#endregion

	#region Helpers

	private static void UpdateAllGeolocation()
	{
		int failCount = 0;
		List<Address> allAddresses = Address.GetAll();
		List<Address> allAddressesWithoutLatLong = Address.GetAll().Where(a => !a.Latitude.HasValue || !a.Longitude.HasValue).ToList();
		foreach (Address address in allAddressesWithoutLatLong)
		{
			Address equivelantAddress =
				allAddresses.FirstOrDefault(a =>
				(
					(a.Address1 == null && address.Address1 == null)
					|| ((a.Address1 != null && address.Address1 != null) && a.Address1.Trim().ToLower().Equals(address.Address1.Trim().ToLower()))
				)
				&& a.City.ToLower().Equals(address.City.ToLower())
				&& a.StateID == address.StateID
				&& address.Zip != null
				&& a.Zip.Trim().ToLower().Equals(address.Zip.Trim().ToLower())
				&& a.Latitude != null
				&& a.Longitude != null);
			if (equivelantAddress != null)
			{
				address.Latitude = equivelantAddress.Latitude;
				address.Longitude = equivelantAddress.Longitude;
				address.Save();
			}
			else if (address.StateID != null && (!String.IsNullOrWhiteSpace(address.Address1) || !String.IsNullOrWhiteSpace(address.Zip)))
			{
				string tempaddress = address.Address1 + " " + address.City + " " + (address.StateID == (int)States.GA ? "GA" : "SC") + " " + address.Zip;
				decimal? outLat;
				decimal? outLong;
				failCount = Helpers.GetLatLong(tempaddress.Trim(), out outLat, out outLong);
				address.Latitude = outLat;
				address.Longitude = outLong;
				address.Save();
			}
			
			if (failCount > 50) break;
		}

	}

	private static List<string> SetDataTableHeaders(DataSet ds, out DataSet dsOut)
	{
		string tablename = ds.Tables[0].TableName;
		List<string> names = new List<string>();
		List<string> newNames = new List<string>();
		foreach (DataColumn column in ds.Tables[tablename].Columns)
		{
			names.Add(column.ColumnName.Replace("#", "Number"));
		}
		bool firstrow = true;
		foreach (DataRow drCurrent in ds.Tables[tablename].Rows)
		{
			if (!firstrow) break;
			foreach (string name in names)
			{
				if (!newNames.Contains(drCurrent[name].ToString().TrimEnd()))
					newNames.Add(drCurrent[name].ToString().TrimEnd());
				else
					newNames.Add(drCurrent[name].ToString().TrimEnd() + 2);
			}
			firstrow = false;
		}
		for (int n = 0; n < names.Count(); n++)
		{
			if (!String.IsNullOrWhiteSpace(newNames[n]))
				ds.Tables[tablename].Columns[n].ColumnName = newNames[n].Replace("#", "Number");
		}
		dsOut = ds;
		return newNames;
	}

	private static List<RetsMapping> SetPropertyColumnMap()
	{
		List<RetsMapping> tempmap = new List<RetsMapping>
			{
				new RetsMapping("Address1", "Address1", false),
				new RetsMapping("Address2", "Address2", false),
				new RetsMapping("City", "City", false),
				new RetsMapping("StateOrProvince", "StateOrProvince", false),
				new RetsMapping("ZipOrPostalCode", "ZipOrPostalCode", false),
				new RetsMapping("Neighborhood", "Subdivision", true),
				new RetsMapping("RentalPK", "RentalPK", false),
				new RetsMapping("OfficeName", "OfficeName", false),
				new RetsMapping("Property Type", "Type", true),
				new RetsMapping("Status", "Status", false),				
				new RetsMapping("Rental Price", "RentalPrice", true),
				new RetsMapping("ListingDate", "Listing Date", false),
				new RetsMapping("LeaseBegins", "LeaseBegins", false),
				new RetsMapping("LeaseExpires", "LeaseExpires", false),
				new RetsMapping("DateAvailable", "DateAvailable", false),
				new RetsMapping("Bedrooms", "Bedrooms", true),
				new RetsMapping("Full Baths", "FullBaths", true),
				new RetsMapping("Half Baths", "HalfBaths", true),
				new RetsMapping("Garage", "Garage", true),
				new RetsMapping("Elementary School", "ElemSchool", true),
				new RetsMapping("High School", "HighSchool", true),
				new RetsMapping("Middle School", "MidSchool", true),
				new RetsMapping("Features", "Features", true),
				new RetsMapping("OPTEmail", "OPTEmail", false),
				new RetsMapping("OPTStats", "OPTStats", false),
				new RetsMapping("Filename", "Filename", false),
				new RetsMapping("Note", "Note", false),
				new RetsMapping("OwnerFirstName", "OwnerFirstName", false),
				new RetsMapping("OwnerLastName", "OwnerLastName", false),
				new RetsMapping("ContactFirstName", "ContactFirstName", false),
				new RetsMapping("ContactLastName", "ContactLastName", false),
				new RetsMapping("ContactWorkPhone", "ContactWorkPhone", false),
				new RetsMapping("ContactWorkPhoneExt", "ContactWorkPhoneExt", false),
				new RetsMapping("AgentFirstName", "AgentFirstName", false),
				new RetsMapping("AgentLastName", "AgentLastName", false)
			};


		return tempmap;
	}

	#endregion

	#region Classes
	public class RetsMapping
	{
		public string RetsName { get; set; }
		public string FieldName { get; set; }
		public bool Attribute { get; set; }
		public RetsMapping(string fieldName, string retsName, bool attribute)
		{
			RetsName = retsName;
			FieldName = fieldName;
			Attribute = attribute;
		}
	}
	#endregion
}
