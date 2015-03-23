using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web.Security;
using BaseCode;
using Classes.Media352_MembershipProvider;
using Classes.MLS;
using Classes.Rets;
using Classes.Showcase;
using Classes.StateAndCountry;
using Classes.WhatsNearBy;
using Settings = Classes.Showcase.Settings;



public partial class RetsConnectorUpdatefromXml : System.Web.UI.Page
{
	#region Properties

	protected string[] ArrayOfActiveStatusCodes = { "Active", "Contingent", "" };
	protected Dictionary<string, string> PropertyTypes = new Dictionary<string, string>
		{
			{"RES", "Homes"},
			{"MUL", "Condos & Apartments"},
			{"LND", "Land"}
		};
	protected List<string> PropertyClasses = new List<string>(new[] { "All" });
	protected List<int> ShowcaseIDs = new List<int>()
		{
			(int)MeybohmShowcases.AugustaExistingHomes,
			(int)MeybohmShowcases.AugustaLand,
			(int)MeybohmShowcases.AikenExistingHomes,
			(int)MeybohmShowcases.AikenLand,
			    };
	protected List<string> Cities = new List<string>(new[] { 
	"Augusta",
	 "Aiken" 
	 });
	protected List<RetsMapping> PropertyMap = new List<RetsMapping>();
	protected List<RetsMapping> OfficeMap = new List<RetsMapping>();
	protected List<RetsMapping> AgentMap = new List<RetsMapping>();
	protected List<WhatsNearByLocation> Locations;
	protected List<Neighborhood> AllNeighborhoods;
	protected List<AgentException> AgentExceptionsList;
	protected bool CurrentCSVAiken;
	protected List<Int32> allMLSIDs = new List<int>();
	protected bool RetsImportInProgress
	{
		get
		{
			if (Request.QueryString["resetLock"] != null && Request.QueryString["resetLock"].Equals("1")) return false;
			return RetsUpdatePageTracker.RetsUpdatePageTrackerGetByRunCompleted(false).Any();
		}
	}
	private TextInfo textInfo;
	private int m_ElementarySchoolCategoryID = WhatsNearByCategory.WhatsNearByCategoryGetByName("Elementary Schools").FirstOrDefault().WhatsNearByCategoryID;
	private int m_MiddleSchoolCategoryID = WhatsNearByCategory.WhatsNearByCategoryGetByName("Middle Schools").FirstOrDefault().WhatsNearByCategoryID;
	private int m_HighSchoolCategoryID = WhatsNearByCategory.WhatsNearByCategoryGetByName("High Schools").FirstOrDefault().WhatsNearByCategoryID;

	private List<string> m_AttributesToCommaSeparate = new List<string>(new[] { "Neighborhood Amenities", "Exterior Features", "Interior Features", "Garage/Carport", "Garage" });
	private List<PropertyChangeLog> m_ChangedProperties = new List<PropertyChangeLog>();
	#endregion

	#region Events

	protected void Page_Load(object sender, EventArgs e)
	{
		Helpers.PurgeCacheItems("Rets_RetsUpdatePageTracker");
		RetsUpdatePageTracker currentTracker = RetsUpdatePageTracker.GetAll().OrderByDescending(t => t.CurrentRunStartTime).FirstOrDefault();
		if (currentTracker == null) currentTracker = new RetsUpdatePageTracker();
		string version = Request.QueryString["version"] != null && Request.QueryString["version"].ToLower() == "full" ? "full" : string.Empty;
		if (!RetsImportInProgress)
		{
			if (version.Equals("full"))
                SendErrorEmail("", version + " property update process started @ " + DateTime.Now, version + "Property Update Process Started @" + DateTime.Now);
			currentTracker.CurrentRunStartTime = DateTime.UtcNow;
			currentTracker.RunCompleted = false;
			currentTracker.Save();
			textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
			AgentExceptionsList = SetAgentExceptionList();
			Locations = WhatsNearByLocation.GetAll();
			AllNeighborhoods = Neighborhood.GetAll();
			PropertyMap = SetPropertyColumnMap();
			OfficeMap = SetOfficeColumnMap();
			AgentMap = SetAgentColumnMap();
			ImportOffices();
			ImportAgents();
			ImportAllProperties(version);
			PurgeOldShowcaseItems(version);
			try
			{
				SendSavedSearchAlerts();
				UpdateAllGeolocation();
			}
			catch (Exception ex)
			{
				Helpers.LogException(ex);
			}
			Helpers.PurgeCacheItems("Showcase");
			currentTracker.RunCompleted = true;
			currentTracker.Save();
			if (version.Equals("full"))
				SendErrorEmail("", version + " property update process has completed successfully @ " + DateTime.Now, version + "Property Update Process Has Completed @" + DateTime.Now);
			Response.Write(version + " property update process has completed successfully @ " + DateTime.Now);
		}
		else if (currentTracker.RetsUpdatePageTrackerID > 0 && currentTracker.CurrentRunStartTime < DateTime.UtcNow.AddDays(-1))
		{
			SendErrorEmail("", version +
						   " property update has not completed in the last 24 hours.  This could be caused by system restart or code updates being pushed while there was an update running. The single run lock has been reset and updates should continue as scheduled. If this does not occur, please contact support.");
			currentTracker.RunCompleted = true;
			currentTracker.Save();
			Response.Write(version + " property update has not completed in the last 24 hours. Please contact support.");
		}
		else
		{
			SendErrorEmail("",
							  version + " property update stopped to prevent conflict with import in process  @ " + DateTime.Now + ".  If you continue to get this message please run the reset url and contact support.");
			Response.Write("property update stopped to prevent conflict with import in process  @ " + DateTime.Now);
		}
	}

	#endregion

	#region Data Handling

	private void ImportAllProperties(string version)
	{
		Cities.ForEach(city => PropertyClasses.ForEach(className =>
		{
			CurrentCSVAiken = city.Equals("Aiken");
			var ds = new DataSet();
			if (File.Exists(Server.MapPath("~/RetsConnector/RetsCSVData/Meybohm-" + city + "-" + className + version + ".csv")))
			{
				DataTable table = null;
				using (StreamReader reader =
					new StreamReader(
						Server.MapPath("../RetsConnector/RetsCSVData/Meybohm-" + city + "-" + className + version + ".csv")))
				{
					table = CsvParser.Parse(reader);
				}
				if (table != null&&table.Rows.Count>0)
				{
					ds.Tables.Add(table);
					ds.Tables[0].TableName = "Properties";
					List<string> newNames = SetDataTableHeaders(ds, out ds);
					List<string> attributes = new List<string>(newNames);
					// remove any columns that aren't attributes
					foreach (RetsMapping retsMapping in PropertyMap.Where(r => !r.Attribute))
						attributes.Remove(retsMapping.RetsName);
					AddNewAttributes(ds, attributes, PropertyMap, city);
					ImportCityProperties(city, newNames, ds);
				}
			}
		}));
	}

	private void PurgeOldShowcaseItems(string version)
	{
		if (version == "full")
		{
			List<ShowcaseItem> allMLSFromDB =
				ShowcaseItem.ShowcaseItemGetByActive(true).Where(s => ShowcaseIDs.Contains(s.ShowcaseID)).ToList();
			foreach (
				var showcaseItem in
					allMLSFromDB.Where(item => item.MlsID.HasValue && !allMLSIDs.Contains(item.MlsID.Value)))
			{
				showcaseItem.Active = false;
				try
				{
					showcaseItem.Save();
				}
				catch(Exception ex)
				{
					Helpers.LogException(ex);
				}
			}
		}
	}

	private void ImportCityProperties(string city, List<string> colNames, DataSet ds)
	{
		int mlsNumber = 0;
		string step = string.Empty;
		RetsMapping newConstructionMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("New Construction"));
		RetsMapping agentMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("AgentID"));
		RetsMapping officeIdMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("OfficeID"));

		int counter = 0;
		List<UserOffice> allAgents = UserOffice.GetAll();
		List<string> allAgentIDS = allAgents.Select(u => u.MlsID).ToList();
		List<string> allTeamIDS = Team.GetAll().Select(u => u.MlsID).ToList();

		List<int> allagentOfficeIDs = allAgents.Select(u => u.OfficeID).ToList();
		var dataWithNonMeybohmNewConstructionRemoved = (from myRows in ds.Tables["Properties"].AsEnumerable()
														where
															!myRows.Field<string>(newConstructionMap.RetsName).Equals("Y") ||
															(allAgentIDS.Contains(myRows.Field<string>(agentMap.RetsName).Replace("-", "_")) || allTeamIDS.Contains(myRows.Field<string>(agentMap.RetsName).Replace("-", "_")))
														select myRows).ToList();
		foreach (DataRow dr in dataWithNonMeybohmNewConstructionRemoved)
		{
			try
			{
				step = "read the datarow(property)";
				if (counter++ == 0) continue; // skips header row in csv- cannot remove header row needed to associate filed names
				{
					RetsMapping retsStatus = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Status"));
					RetsMapping photoUrl = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("PhotoURL"));

					RetsMapping mlsNumberMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("MLS Number"));
					RetsMapping propertyTypeMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("PropertyType"));
					step = "step2 - map data rows";
					bool newConstruction = newConstructionMap != null &&
										   (colNames.Contains(newConstructionMap.RetsName) &&
											(dr[newConstructionMap.RetsName].ToString().Contains("Y") ||
											 dr[newConstructionMap.RetsName].ToString().Contains("1")));
					step = "step 3 - determine if property is newconstruction";
					List<int> allShowcasesForProperty = new List<int>();
					allShowcasesForProperty = new List<int>() { ((dr[mlsNumberMap.RetsName]).ToString().Length == 6 ? 4 : 2) };
					if ((dr[propertyTypeMap.RetsName]).ToString().Equals("Lots/Land"))
						allShowcasesForProperty.Add(city.Equals("Augusta")
														? MeybohmShowcases.AugustaLand.GetHashCode()
														: MeybohmShowcases.AikenLand.GetHashCode());
					step = "step 4 - determine all showcases for property";
					List<string> imagePaths = null;
					if (photoUrl != null) imagePaths = dr[photoUrl.RetsName].ToString().Split(',').ToList();

					foreach (var currentShowcaseID in allShowcasesForProperty)
					{
						step = "step 5 - start cycling through showcases for property";
						if (mlsNumberMap == null || dr[mlsNumberMap.RetsName] == null)
							continue;

						// get the existing item or a new one based on MLS number
						ShowcaseItem item = ShowcaseItem.ShowcaseItemGetByMlsID(Convert.ToInt32(dr[mlsNumberMap.RetsName])).FirstOrDefault(s => s.ShowcaseID == currentShowcaseID)
											?? new ShowcaseItem();

						step = "step 6- determine if property exists in db";
						mlsNumber = Convert.ToInt32(dr[mlsNumberMap.RetsName].ToString());
						if (!allMLSIDs.Contains(mlsNumber))
							allMLSIDs.Add(mlsNumber);

						// Delete showcase items for mls items that have deletion status codes
						step = "step 7- save mlsID to use later for deactivating properties not in import";
						if (retsStatus != null && !(ArrayOfActiveStatusCodes.Contains(dr[retsStatus.RetsName])))
						{
							if (item.ShowcaseItemID > 0)
							{
								item.Active = false;
								item.Save();
							}
							step = "step 8 - deactivate properties if they don't have the correct status";
						}
						else
						{
							string agentID = dr[agentMap.RetsName].ToString().Replace("-", "_").Trim();

							bool logNewHome = false;
							if (item != null && item.ShowcaseItemID > 0)
								CheckforUpdate(item, ds, dr, colNames);
							else
								logNewHome = true;

							item = SaveShowcaseItem(item, ds, dr, currentShowcaseID, mlsNumber, imagePaths,newConstruction);
							if (logNewHome && item.ShowcaseItemID > 0)
							{
								PropertyChangeLog log = new PropertyChangeLog
								{
									Attribute = "Home Added",
									DateStamp = DateTime.UtcNow,
									OldValue = string.Empty,
									NewValue = string.Empty,
									ShowcaseItemID = item.ShowcaseItemID
								};
								log.Save();
								m_ChangedProperties.Add(log);
							}


							step = "step 9 - save or update property";
						}

						List<PropertyChangeLog> changesForProperty = m_ChangedProperties.Where(p => p.ShowcaseItemID == item.ShowcaseItemID).ToList();
						if (item.ShowcaseItemID > 0 && changesForProperty.Any(s => s.Attribute != "Summary" && s.Attribute != "Status" && s.Attribute != "Address"))
						{
							// add the attribute values
							List<string> cols = colNames;
							// remove any columns that aren't attributes
							foreach (RetsMapping retsMapping in PropertyMap.Where(r => !r.Attribute))
								cols.Remove(retsMapping.RetsName);
							List<ShowcaseAttribute> attributesToImport = ShowcaseAttribute.ShowcaseAttributePage(0, 0, "", "", true, new ShowcaseAttribute.Filters { FilterShowcaseAttributeShowcaseID = item.ShowcaseID.ToString(), FilterShowcaseAttributeImportItemAttribute = true.ToString() });
							cols.Where(i => attributesToImport.Any(s => s.MLSAttributeName == i) && changesForProperty.Any(p => p.Attribute == i)).ToList().ForEach(i =>
								{
									// Using the generated objects here would be a huge bottle neck, so
									// a custom procedure was written to remove the overhead.
									// remove any columns that are not marked as an attribute to import in the admin
									string tempVal = (i == "Apx Year Built") ?
														(!string.IsNullOrEmpty(dr[i].ToString()) && Convert.ToInt32(dr[i]) < ((DateTime.UtcNow.Year) + 10) ?
															dr[i].ToString() : "0")
														: dr[i].ToString();
									List<SqlParameter> sqlParams = new List<SqlParameter>();
									sqlParams.Add(new SqlParameter("@Value", textInfo.ToTitleCase(tempVal.ToLower().Trim())));
									sqlParams.Add(new SqlParameter("@MLSAttributeName", i));
									sqlParams.Add(new SqlParameter("@ShowcaseItemId", item.ShowcaseItemID));
									sqlParams.Add(new SqlParameter("@ShowcaseID", item.ShowcaseID));
									if (m_AttributesToCommaSeparate.Contains(i))
										sqlParams.Add(new SqlParameter("@SplitDelimiter", ','));
									SqlHelper.ExecuteNonQuery(Settings.ConnectionString, CommandType.StoredProcedure, "CUSTOM_RETS_ShowcaseItemAttributeAssign", sqlParams.ToArray());
								});
						}
						step = "step 10 - save all attributes for property";
						GenerateMediaCollection(item, imagePaths);
						step = "step 11 -delete old and save new media items";
					}
				}
			}
			catch (Exception ex)
			{
				SendErrorEmail("ImportProperties");
				Helpers.LogException(new Exception("CustomRetsError|ImportProperties|City :" + city + "|Mls ID:" + mlsNumber + "|step of method: " + step, ex));
			}
		}

		Helpers.PurgeCacheItems("Showcase");
	}

	private ShowcaseItem SaveShowcaseItem(ShowcaseItem item, DataSet ds, DataRow dr, int showcaseForCurrentItem, int mlsNumber, List<string> imagePaths,bool newConstruction)
	{
		string step = string.Empty;
		try
		{
			step = "pre-step1";
			RetsMapping summary = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Summary"));
			RetsMapping bedroomsMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Bedrooms"));
			RetsMapping listPriceMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("List Price"));
			RetsMapping fullBathMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Full Baths"));
			RetsMapping halfBathMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Half Baths"));
			RetsMapping totalSqftMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("TotalSqFt"));
			RetsMapping officeIdMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("OfficeID"));
			RetsMapping agentMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("AgentID"));
			RetsMapping listingDateMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("ListingDate"));
			RetsMapping directionsMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Directions"));
			RetsMapping virtualTourMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Virtual Tour"));
			RetsMapping latMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Latitude"));
			RetsMapping lonMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Longitude"));
			RetsMapping propertyTypeMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("PropertyType"));
			RetsMapping totalAcresMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Total Acres"));
			step = "step1-datamapping";

			Address itemAddress = item.AddressID > 0 ? Address.GetByID(item.AddressID) : new Address();
			itemAddress = SetAddressFields(itemAddress, dr, PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Street Number")),
										   PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Address")),
										   PropertyMap.FirstOrDefault(r => r.FieldName.Equals("City")),
										   PropertyMap.FirstOrDefault(r => r.FieldName.Equals("State")),
										   PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Zip Code")),
											latMap, lonMap);
			step = "step2-get address";

			Builder itemBuilder = SetItemBuilder(ds, dr, itemAddress);

			Neighborhood itemNeighborhood = SetItemNeighborhood(ds, dr, itemAddress);
			step = "step3-create neighborhood";

			WhatsNearByLocation esLocation;
			WhatsNearByLocation msLocation;
			WhatsNearByLocation hsLocation = WhatsNearBySchools(dr, itemAddress, out esLocation, out msLocation);
			step = "step4-set schools near by";

			#region save ShowcaseItem information


			string virtualTourURL = virtualTourMap != null && ds.Tables[0].Columns.Contains(virtualTourMap.RetsName)
										? dr[virtualTourMap.RetsName].ToString().Trim()
										: null;
			int bedrooms = 0;
			if (bedroomsMap != null && ds.Tables[0].Columns.Contains(bedroomsMap.RetsName))
				Int32.TryParse((dr[bedroomsMap.RetsName].ToString().Trim()), out bedrooms);
			int listPrice = 0;
			if (listPriceMap != null)
				Int32.TryParse((dr[listPriceMap.RetsName].ToString().Trim()), out listPrice);
			int halfBath = 0;
			int fullBath = 0;
			if (fullBathMap != null && ds.Tables[0].Columns.Contains(fullBathMap.RetsName))
				Int32.TryParse(dr[fullBathMap.RetsName].ToString(), out fullBath);
			if (halfBathMap != null && ds.Tables[0].Columns.Contains(halfBathMap.RetsName))
				Int32.TryParse(dr[halfBathMap.RetsName].ToString().Trim(), out halfBath);
			if (totalSqftMap != null && !(ds.Tables[0].Columns.Contains(totalSqftMap.RetsName)))
				totalSqftMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("TotalSqFt-Alternate"));
			// Aiken has different name for field
			int totalSqft = 0;
			if (totalSqftMap != null && ds.Tables[0].Columns.Contains(totalSqftMap.RetsName))
				Int32.TryParse(dr[totalSqftMap.RetsName].ToString().Trim(), out totalSqft);
			if (summary != null && ds.Tables[0].Columns.Contains(summary.RetsName))
				item.Summary = CleanUpDescriptionText(dr[summary.RetsName].ToString().Trim());
			else if (summary != null && ds.Tables[0].Columns.Contains("Remarks"))
				item.Summary = CleanUpDescriptionText(dr["Remarks"].ToString().Trim());
			else item.Summary = string.Empty;
			step = "step5 - set quick access attributes";
			Office tempOffice = null;
			string officeid = (string)(officeIdMap != null ? dr[officeIdMap.RetsName] : null);
			if (officeIdMap != null && officeid != null)
			{
				tempOffice = Office.OfficeGetByMlsID(Convert.ToInt32(officeid)).FirstOrDefault(o => o.Active && o.CMMicrositeID == (CurrentCSVAiken ? 3 : 2));
				item.OfficeID = tempOffice != null ? (int?)tempOffice.OfficeID : null;
			}
			step = "step6 - set office";
			if (!String.IsNullOrWhiteSpace(virtualTourURL))
				item.VirtualTourURL = (!virtualTourURL.ToLower().StartsWith("http") ? "http://" : "") + virtualTourURL;
			item.Active = true;
			item.BuilderID = itemBuilder != null ? (int?)itemBuilder.BuilderID : null;
			item.NeighborhoodID = itemNeighborhood != null && (int?)itemNeighborhood.NeighborhoodID>0 ? (int?)itemNeighborhood.NeighborhoodID : null;
			item.Featured = false; //TODO set featured for Meybohm properties
			item.ShowcaseID = showcaseForCurrentItem;
			item.NewHome = ((showcaseForCurrentItem == (int) MeybohmShowcases.AugustaExistingHomes ||
				                 showcaseForCurrentItem == (int) MeybohmShowcases.AikenExistingHomes) && newConstruction);
			item.AddressID = itemAddress.AddressID;
			if (dr.Table.Columns.Contains(propertyTypeMap.RetsName) && (dr[propertyTypeMap.RetsName]).ToString().Equals("Lots/Land", StringComparison.OrdinalIgnoreCase))
			{
				decimal acreage;
				string acreageString = "0";
				if (dr.Table.Columns.Contains(totalAcresMap.RetsName) && Decimal.TryParse(dr[totalAcresMap.RetsName].ToString(), out acreage))
					acreageString = acreage > 0 ? Math.Round(acreage, 2).ToString().TrimEnd('.') : "N/A";

				item.Title = "Price: " + listPrice.ToString("C").Replace(".00", "") + " Acreage: " + acreageString;
			}
			else
				item.Title = "Price: " + listPrice.ToString("C").Replace(".00", "") + " Bedrooms: " + bedrooms + " Bathrooms: " +
							 fullBath + (halfBath > 0 ? "/" + halfBath : "") + " SqFt: " + totalSqft.ToString("C").Replace(".00", "").TrimStart('$');
			item.MlsID = mlsNumber;
			item.Image = imagePaths.Any() ? imagePaths.FirstOrDefault() : "";
			item.ListPrice = listPrice;
			DateTime? listDate = item.DateListed
								 ??
								 (dr.Table.Columns.Contains(listingDateMap.RetsName) &&
								  !string.IsNullOrEmpty(dr[listingDateMap.RetsName].ToString())
									  ? (Convert.ToDateTime(dr[listingDateMap.RetsName]))
									  : DateTime.UtcNow);
			item.DateListed = listDate;
			item.HighSchoolID = hsLocation != null ? (int?)hsLocation.WhatsNearByLocationID : null;
			item.MiddleSchoolID = msLocation != null ? (int?)msLocation.WhatsNearByLocationID : null;
			item.ElementarySchoolID = esLocation != null ? (int?)esLocation.WhatsNearByLocationID : null;
			UserOffice agentUser = dr.Table.Columns.Contains(agentMap.RetsName) ? UserOffice.UserOfficeGetByMlsID(dr[agentMap.RetsName].ToString().Replace("-", "_").Trim()).FirstOrDefault(u => tempOffice != null && u.OfficeID == tempOffice.OfficeID) : null;
			Team team = dr.Table.Columns.Contains(agentMap.RetsName) ? Team.TeamGetByMlsID(dr[agentMap.RetsName].ToString().Replace("-", "_").Trim()).FirstOrDefault() : null;
			item.AgentID = agentUser != null ? (int?)agentUser.UserID : null;
			item.TeamID = team != null ? team.TeamID : (int?)null;
			item.Directions = dr.Table.Columns.Contains(directionsMap.RetsName) ? CleanUpDescriptionText(dr[directionsMap.RetsName].ToString().Trim()) : "";
			step = "step7 - set final showcaseitem attributes";

			item.Save();
		}
		catch (Exception ex)
		{
			SendErrorEmail("SaveShowcaseItem");
			Helpers.LogException(new Exception("CustomRetsError|SaveShowcaseItem|Mls ID :" + mlsNumber + "|Showcase:" + Showcases.GetByID(item.ShowcaseID).Title + "|step of method: " + step, ex));
		}

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
		if (eSMap != null && !string.IsNullOrEmpty(dr[eSMap.RetsName].ToString().Trim()))
		{
			esLocation = Locations.FirstOrDefault(l => l.Name.Equals(dr[eSMap.RetsName].ToString().Trim(), StringComparison.OrdinalIgnoreCase));
			if (esLocation == null)
			{
				Address esAddress = new Address
					{
						Address1 = textInfo.ToTitleCase(dr[eSMap.RetsName].ToString().ToLower()),
						City = itemAddress.City,
						StateID = itemAddress.StateID
					};
				esAddress.Save();
				esLocation = new WhatsNearByLocation { Name = CleanUpSchoolName(textInfo.ToTitleCase(dr[eSMap.RetsName].ToString().Trim().ToLower())), AddressID = esAddress.AddressID, Active = false };
				esLocation.Save();

				new WhatsNearByLocationCategory { WhatsNearByCategoryID = m_ElementarySchoolCategoryID, WhatsNearByLocationID = esLocation.WhatsNearByLocationID }.Save();
				Locations.Add(esLocation);
			}
			if (esLocation.Name != CleanUpSchoolName(esLocation.Name)) { esLocation.Name = CleanUpSchoolName(esLocation.Name); esLocation.Save(); }
		}
		if (mSMap != null && !string.IsNullOrEmpty(dr[mSMap.RetsName].ToString().Trim()))
		{
			msLocation = Locations.FirstOrDefault(l => l.Name.Equals(dr[mSMap.RetsName].ToString().Trim(), StringComparison.OrdinalIgnoreCase));
			if (msLocation == null)
			{
				Address msAddress = new Address
					{
						Address1 = textInfo.ToTitleCase(dr[mSMap.RetsName].ToString().Trim().ToLower()),
						City = itemAddress.City,
						StateID = itemAddress.StateID
					};
				msAddress.Save();
				msLocation = new WhatsNearByLocation { Name = CleanUpSchoolName(textInfo.ToTitleCase(dr[mSMap.RetsName].ToString().Trim().ToLower())), AddressID = msAddress.AddressID, Active = false };
				msLocation.Save();

				new WhatsNearByLocationCategory { WhatsNearByCategoryID = m_MiddleSchoolCategoryID, WhatsNearByLocationID = msLocation.WhatsNearByLocationID }.Save();
				Locations.Add(msLocation);
			}
			if (msLocation.Name != CleanUpSchoolName(msLocation.Name)) { msLocation.Name = CleanUpSchoolName(msLocation.Name); msLocation.Save(); }
		}
		if (hSMap != null && !string.IsNullOrEmpty(dr[hSMap.RetsName].ToString().Trim()))
		{
			hsLocation = Locations.FirstOrDefault(l => l.Name.Equals(dr[hSMap.RetsName].ToString().Trim(), StringComparison.OrdinalIgnoreCase));
			if (hsLocation == null)
			{
				Address hsAddress = new Address
					{
						Address1 = textInfo.ToTitleCase(dr[hSMap.RetsName].ToString().Trim().ToLower()),
						City = itemAddress.City,
						StateID = itemAddress.StateID
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

	private Neighborhood SetItemNeighborhood(DataSet ds, DataRow dr, Address itemAddress)
	{
		RetsMapping neighborhoodMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Neighborhood"));
		if (neighborhoodMap != null && !(ds.Tables[0].Columns.Contains(neighborhoodMap.RetsName)))
			neighborhoodMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Town"));
		if (neighborhoodMap != null && !string.IsNullOrEmpty(dr[neighborhoodMap.RetsName].ToString().Trim()) && !dr[neighborhoodMap.RetsName].ToString().Trim().ToLower().StartsWith("none"))
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
			itemNeighborhood.CMMicrositeID = CurrentCSVAiken ? 3 : 2;
			itemNeighborhood.Save();
			AllNeighborhoods.Add(itemNeighborhood);
			return itemNeighborhood;
		}
		return null;
	}

	private Builder SetItemBuilder(DataSet ds, DataRow dr, Address itemAddress)
	{
		RetsMapping builderMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Builder"));
		Builder itemBuilder = null;
		if (builderMap != null && ds.Tables[0].Columns.Contains(builderMap.RetsName) && !string.IsNullOrEmpty(dr[builderMap.RetsName].ToString().Trim()))
		{
			itemBuilder = Builder.BuilderGetByName(dr[builderMap.RetsName].ToString().Trim()).FirstOrDefault() ?? new Builder();
			itemBuilder.Name = textInfo.ToTitleCase(dr[builderMap.RetsName].ToString().Trim().ToLower());
			itemBuilder.Save();
			int ms = itemAddress.StateID == (int)States.GA ? 2 : 3;
			BuilderMicrosite bmsEntity = BuilderMicrosite.BuilderMicrositePage(0, 1, "", "", true, new BuilderMicrosite.Filters { FilterBuilderMicrositeBuilderID = itemBuilder.BuilderID.ToString(CultureInfo.InvariantCulture), FilterBuilderMicrositeCMMicrositeID = ms.ToString(CultureInfo.InvariantCulture) }).FirstOrDefault();
			if (bmsEntity == null)
			{
				bmsEntity = new BuilderMicrosite { BuilderID = itemBuilder.BuilderID, CMMicrositeID = ms };
				bmsEntity.Save();
			}
		}
		return itemBuilder;
	}

	private void GenerateMediaCollection(ShowcaseItem item, List<string> paths)
	{

		if (item.ShowcaseItemID > 0)
		{
			MediaCollection collection = MediaCollection.MediaCollectionPage(0, 1, "", "", true, new MediaCollection.Filters { FilterMediaCollectionShowcaseItemID = item.ShowcaseItemID.ToString(CultureInfo.InvariantCulture), FilterMediaCollectionTitle = "Photos" }).FirstOrDefault();
			if (collection == null)
			{
				collection = new MediaCollection
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
			List<Media> medias = Media.MediaGetByShowcaseMediaCollectionID(collection.ShowcaseMediaCollectionID);
			bool changeLogged = false;
			if (paths.Count > 0)
			{
				for (short i = 1; i <= paths.Count(); i++)
				{
					if (i == 1 && !item.Image.Equals(paths[i - 1], StringComparison.OrdinalIgnoreCase))
                    {
						item.Image = paths[i - 1];
						item.Save();
					}
					if (medias.Any(m => m.DisplayOrder == i && m.URL.Equals(paths[i - 1], StringComparison.OrdinalIgnoreCase)))
						continue;
					Media mediaItem = medias.Find(m => m.DisplayOrder == i);
					if (mediaItem == null)
					{
						mediaItem = new Media { Active = true, ShowcaseMediaCollectionID = collection.ShowcaseMediaCollectionID, DisplayOrder = i };
						medias.Add(mediaItem);
					}
					mediaItem.URL = paths[i - 1];
					mediaItem.Save();
					changeLogged = true;
				}
			}
			if (medias.Count > paths.Count())
			{
				medias.ForEach(m => { if (m.DisplayOrder > paths.Count()) m.Delete(); });
				changeLogged = true;
			}
			//if (changeLogged)
			//	HandleLogCreated(item.ShowcaseItemID);
		}
	}

	private void ImportAgents()
	{
		RetsMapping agentIdMap = AgentMap.FirstOrDefault(r => r.FieldName.Equals("AgentID"));
		RetsMapping homePhoneMap = AgentMap.FirstOrDefault(r => r.FieldName.Equals("Home Phone"));
		RetsMapping addressMap = AgentMap.FirstOrDefault(r => r.FieldName.Equals("Address"));
		RetsMapping stateMap = AgentMap.FirstOrDefault(r => r.FieldName.Equals("State"));
		RetsMapping cityMap = AgentMap.FirstOrDefault(r => r.FieldName.Equals("City"));
		RetsMapping zipMap = AgentMap.FirstOrDefault(r => r.FieldName.Equals("Zip"));
		RetsMapping phoneMap = AgentMap.FirstOrDefault(r => r.FieldName.Equals("Phone"));
		RetsMapping firstnameMap = AgentMap.FirstOrDefault(r => r.FieldName.Equals("First Name"));
		RetsMapping lastnameMap = AgentMap.FirstOrDefault(r => r.FieldName.Equals("Last Name"));
		RetsMapping officeIdMap = AgentMap.FirstOrDefault(r => r.FieldName.Equals("OfficeID"));
		RetsMapping emailMap = AgentMap.FirstOrDefault(r => r.FieldName.Equals("Email"));
		RetsMapping webMap = AgentMap.FirstOrDefault(r => r.FieldName.Equals("Web Address"));

		Cities.ForEach(city =>
			{
				string firstName = string.Empty;
				string lastName = string.Empty;

				try
				{
					var ds = new DataSet();
					if (File.Exists(Server.MapPath("../RetsConnector/RetsCSVData/Meybohm-" + city + "-Agents.csv")))
					{
						using (StreamReader reader = new StreamReader(Server.MapPath("../RetsConnector/RetsCSVData/Meybohm-" + city + "-Agents.csv")))
						{
							DataTable tempTable = CsvParser.Parse(reader);
							if (tempTable != null)
								ds.Tables.Add(tempTable);
						}
						if (ds.Tables.Count > 0)
						{
							ds.Tables[0].TableName = "Agents";
							SetDataTableHeaders(ds, out ds);
							int counter = 0;
							foreach (DataRow dr in ds.Tables["Agents"].Rows)
							{
								firstName = string.Empty;
								lastName = string.Empty;
								string email = string.Empty;
								string mlsID = string.Empty;
								if (counter++ == 0) continue; //Header row
								if (agentIdMap != null)
								{
									Office tempOffice = Office.OfficeGetByMlsID(Convert.ToInt32(dr[officeIdMap.RetsName].ToString().Trim())).FirstOrDefault(
											o => o.Active && o.IsMeybohm && o.CMMicrositeID == (city.Equals("Aiken") ? 3 : 2));
									if (firstnameMap != null && lastnameMap != null && tempOffice != null)
									{
										firstName = dr[firstnameMap.RetsName].ToString().Trim();
										lastName = dr[lastnameMap.RetsName].ToString().Trim();
										email = dr[emailMap.RetsName].ToString().Trim();
										mlsID = dr[agentIdMap.RetsName].ToString().Trim().Replace("-", "_");
										if (!(firstName.ToLower().Contains("team") || lastName.ToLower().Contains("team")))
										{
											string userName = ((firstName.Length > 0
													  ? firstName.Substring(0, 1)
													  : "") + lastName).Split(',')[0].Replace(".", "");
											AgentException agentException =
												AgentExceptionsList.FirstOrDefault(
													a =>
													a.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
													a.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase) &&
													a.MLSID.Equals(mlsID, StringComparison.OrdinalIgnoreCase));
											if (agentException != null)
											{
												userName = agentException.UserName;
											}

											User userEntity = Classes.Media352_MembershipProvider.User.UserGetByEmail(email).FirstOrDefault();
											if (userEntity == null)
												userEntity = Classes.Media352_MembershipProvider.User.UserGetByName(userName).FirstOrDefault();
											if (userEntity == null)
												userEntity = Classes.Media352_MembershipProvider.User.UserGetByName(userName + "@meybohm.com").FirstOrDefault();
											Address userAddress = new Address();
											UserInfo userInfoEntity = new UserInfo();
											if (userEntity != null)
											{
												userInfoEntity = UserInfo.UserInfoGetByUserID(userEntity.UserID).FirstOrDefault() ?? new UserInfo();
												if (userInfoEntity != null && userInfoEntity.AddressID.HasValue)
													userAddress = Address.GetByID(userInfoEntity.AddressID.Value);
											}
											userAddress = SetAddressFields(userAddress, dr, null, addressMap, cityMap, stateMap, zipMap, null, null);

											// create or update userinfo
											Media352_MembershipProvider membership = (Media352_MembershipProvider)Membership.Provider;
											if (userEntity == null || userEntity.UserID < 1)
											{
												bool duplicateName = false;
												int loopcounter = 0;
												MembershipUser user = null;
												MembershipCreateStatus status;
												do
												{

													string emailAddress = emailMap != null && dr[emailMap.RetsName] != DBNull.Value &&
																		  !String.IsNullOrWhiteSpace(dr[emailMap.RetsName].ToString())
																			  ? dr[emailMap.RetsName].ToString().Trim()
																			  : userName + "@meybohm.com";
													user = membership.CreateUser(userName, "Meybohm", emailAddress,
																				 "You created your account with Active Directory", "Reset this",
																				 Classes.Media352_MembershipProvider.Settings.
																					 UsersApprovedByDefault,
																				 1, out status);
													loopcounter++;
													if (status == MembershipCreateStatus.DuplicateUserName)
													{
														if (loopcounter == 1 && !userName.Contains("@")) userName = userName + "@meybohm.com";

														else
														{
															Helpers.LogException(
																new Exception("CustomRetsError|ImportAgents|Agent :" + firstName + " " + lastName + " (" + userName + ")" +
																			  "|step of method: Import Agent Data-user not created-ln:759"));
															break;
														}
													}
												} while (duplicateName);

												if (status == MembershipCreateStatus.Success)
												{
													userInfoEntity.UserID = (int)user.ProviderUserKey;
													UserRole newRole = new UserRole { RoleID = (int)RolesEnum.Agent, UserID = (int)user.ProviderUserKey };
													newRole.Save();
												}
											}
											if (userInfoEntity.UserID > 0)
											{
												userInfoEntity.AddressID = userAddress.AddressID;

												if (lastnameMap != null && !String.IsNullOrWhiteSpace(dr[lastnameMap.RetsName].ToString().Trim()) && string.IsNullOrEmpty(userInfoEntity.LastName))
													userInfoEntity.LastName = textInfo.ToTitleCase(dr[lastnameMap.RetsName].ToString().Trim().ToLower());
												if (userInfoEntity.LastName.StartsWith("Mc") && userInfoEntity.LastName.Length > 2)
												{
													string subName = textInfo.ToTitleCase(userInfoEntity.LastName.Substring(2, userInfoEntity.LastName.Length - 2));
													userInfoEntity.LastName = "Mc" + subName;
												}
												if (firstnameMap != null && !String.IsNullOrWhiteSpace(dr[firstnameMap.RetsName].ToString().Trim()) && string.IsNullOrEmpty(userInfoEntity.FirstName))
													userInfoEntity.FirstName = textInfo.ToTitleCase(dr[firstnameMap.RetsName].ToString().Trim().ToLower());
												if (userInfoEntity.UserInfoID <= 0)
												{
													if (phoneMap != null && !String.IsNullOrWhiteSpace(dr[phoneMap.RetsName].ToString().Trim()) &&
														dr[phoneMap.RetsName].ToString().Trim() != "0" && string.IsNullOrEmpty(userInfoEntity.CellPhone))
														userInfoEntity.CellPhone = Helpers.FormatPhoneNumber(dr[phoneMap.RetsName].ToString().Trim());
													else if (string.IsNullOrEmpty(userInfoEntity.CellPhone))
														userInfoEntity.CellPhone = tempOffice.Phone;
													if (homePhoneMap != null && !String.IsNullOrWhiteSpace(dr[homePhoneMap.RetsName].ToString().Trim()) &&
														dr[homePhoneMap.RetsName].ToString().Trim() != "0" && string.IsNullOrEmpty(userInfoEntity.HomePhone))
														userInfoEntity.HomePhone = Helpers.FormatPhoneNumber(dr[homePhoneMap.RetsName].ToString().Trim());
												}
												if (webMap != null && !String.IsNullOrWhiteSpace(dr[webMap.RetsName].ToString().Trim()) && string.IsNullOrEmpty(userInfoEntity.Website))
													userInfoEntity.Website = dr[webMap.RetsName].ToString().Trim();
												userInfoEntity.Save();

												UserOffice agentOffice =
													UserOffice.UserOfficeGetByUserID(userInfoEntity.UserID).FirstOrDefault(
														u => u.MlsID == mlsID) ??
													new UserOffice();
												agentOffice.MlsID = mlsID;
												agentOffice.OfficeID = tempOffice.OfficeID;
												agentOffice.UserID = agentOffice.UserID > 0 ? agentOffice.UserID : userInfoEntity.UserID;
												agentOffice.Save();
											}
										}
										else
										{
											string emailAddress = emailMap != null &&
																		  !String.IsNullOrWhiteSpace(dr[emailMap.RetsName].ToString().Trim())
																			  ? dr[emailMap.RetsName].ToString().Trim()
																			  : string.Empty;
											Team team = Team.TeamGetByMlsID(dr[agentIdMap.RetsName].ToString().Replace("-", "_").Trim()).FirstOrDefault() ??
												new Team
												{
													MlsID = dr[agentIdMap.RetsName].ToString().Replace("-", "_").Trim(),
													CMMicrositeID = city.Equals("Aiken") ? 3 : 2
												};
											team.Email = emailAddress;
											team.Name = string.IsNullOrEmpty(team.Name) ? firstName + " " + lastName : team.Name;
											if (string.IsNullOrWhiteSpace(team.Phone))
											{
												if (phoneMap != null && !String.IsNullOrWhiteSpace(dr[phoneMap.RetsName].ToString().Trim()) && dr[phoneMap.RetsName].ToString().Trim() != "0")
													team.Phone = Helpers.FormatPhoneNumber(dr[phoneMap.RetsName].ToString().Trim());
												else if (homePhoneMap != null && !String.IsNullOrWhiteSpace(dr[homePhoneMap.RetsName].ToString().Trim()) && dr[homePhoneMap.RetsName].ToString().Trim() != "0")
													team.Phone = Helpers.FormatPhoneNumber(dr[homePhoneMap.RetsName].ToString().Trim());
											}
											team.Save();
										}
									}
								}
							}
						}
					}
				}
				catch (DbEntityValidationException e)
				{
					SendErrorEmail("ImportAgents");
				}
				catch (Exception e)
				{
					SendErrorEmail("ImportAgents");
					Helpers.LogException(new Exception("CustomRetsError|ImportAgents|Agent :" + firstName + " " + lastName + "|step of method: Import Agent Data", e));
				}
			});
	}

	private void ImportOffices()
	{
		RetsMapping officeIdMap = OfficeMap.FirstOrDefault(r => r.FieldName.Equals("OfficeID"));
		RetsMapping faxMap = OfficeMap.FirstOrDefault(r => r.FieldName.Equals("Fax"));
		RetsMapping addressMap = OfficeMap.FirstOrDefault(r => r.FieldName.Equals("Address"));
		RetsMapping stateMap = OfficeMap.FirstOrDefault(r => r.FieldName.Equals("State"));
		RetsMapping cityMap = OfficeMap.FirstOrDefault(r => r.FieldName.Equals("City"));
		RetsMapping zipMap = OfficeMap.FirstOrDefault(r => r.FieldName.Equals("Zip"));
		RetsMapping phoneMap = OfficeMap.FirstOrDefault(r => r.FieldName.Equals("Phone"));
		RetsMapping nameMap = OfficeMap.FirstOrDefault(r => r.FieldName.Equals("Office Name"));
		Cities.ForEach(city =>
			{
				string officeName = string.Empty;
				try
				{
					var ds = new DataSet();
					if (File.Exists(Server.MapPath("../RetsConnector/RetsCSVData/Meybohm-" + city + "-Offices.csv")))
					{
						using (
							StreamReader reader =
								new StreamReader(Server.MapPath("../RetsConnector/RetsCSVData/Meybohm-" + city + "-Offices.csv")))
						{
							ds.Tables.Add(CsvParser.Parse(reader));
						}
						ds.Tables[0].TableName = "Offices";
						SetDataTableHeaders(ds, out ds);
						int counter = 0;
						foreach (DataRow dr in ds.Tables["Offices"].Rows)
						{
							officeName = string.Empty;
							officeName = textInfo.ToTitleCase(dr[nameMap.RetsName].ToString().Trim().ToLower());
							if (counter++ == 0) continue;
							if (officeIdMap != null)
							{
								Office tempOffice = Office.OfficeGetByMlsID(Convert.ToInt32(dr[officeIdMap.RetsName].ToString().Trim())).FirstOrDefault(o => o.CMMicrositeID == (city.ToLower().Equals("augusta") ? 2 : 3)) ?? new Office();
								if (tempOffice.OfficeID < 1) // only save data if office is new
								{
									Address officeAddress = Address.GetByID(tempOffice.AddressID) ?? new Address();
									if (nameMap != null)
									{
										officeAddress = SetAddressFields(officeAddress, dr, null, addressMap, cityMap, stateMap, zipMap, null, null);
										// if new set default values
										tempOffice.HasNewHomes =
											tempOffice.HasRentals = false;
										tempOffice.Active = true;
										tempOffice.CMMicrositeID = city.ToLower().Equals("augusta") ? 2 : 3;
										tempOffice.AddressID = officeAddress.AddressID;
										if (faxMap != null)
											tempOffice.Fax = Helpers.FormatPhoneNumber(dr[faxMap.RetsName].ToString().Trim());
										if (phoneMap != null)
											tempOffice.Phone = Helpers.FormatPhoneNumber(dr[phoneMap.RetsName].ToString().Trim());
										tempOffice.MlsID = Convert.ToInt32(dr[officeIdMap.RetsName].ToString().Trim());
										tempOffice.Name = officeName;
										tempOffice.IsMeybohm = false;
										tempOffice.Save();
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					SendErrorEmail("ImportOffices");
					Helpers.LogException(new Exception("CustomRetsError|ImportAgents|Office :" + officeName + "|step of method: Import Office Data", ex));
				}
			});
	}

	private Address SetAddressFields(Address addressEntity, DataRow dr, RetsMapping streetNumberMap, RetsMapping addressMap, RetsMapping cityMap, RetsMapping stateMap, RetsMapping zipMap, RetsMapping latMap, RetsMapping lonMap)
	{
		string zip = zipMap != null ? dr[zipMap.RetsName].ToString().Trim().Replace(".", "").Replace(",", "") : "";
		if (zip.Length > 5)
			zip = zip.ToString(CultureInfo.InvariantCulture).Substring(0, 5) + "-" +
				  zip.ToString(CultureInfo.InvariantCulture).Substring(5, zip.Length - 5);

		bool addressChanged = (addressMap != null && addressEntity.Address1 != null && !addressEntity.Address1.Equals((streetNumberMap != null ? dr[streetNumberMap.RetsName].ToString().Trim() + " " : "") + dr[addressMap.RetsName].ToString().Trim(), StringComparison.OrdinalIgnoreCase)) ||
								(cityMap != null && addressEntity.City != null && !addressEntity.City.Equals(dr[cityMap.RetsName].ToString().Trim(), StringComparison.OrdinalIgnoreCase)) ||
								(stateMap != null && addressEntity.StateID != null && addressEntity.StateID != (dr[stateMap.RetsName].ToString().Trim().ToLower().Contains("ga") ? (int)States.GA : (int)States.SC)) ||
								(zipMap != null && addressEntity.Zip != null && addressEntity.Zip != zip);
		if (addressMap != null)
			addressEntity.Address1 = textInfo.ToTitleCase((streetNumberMap != null ? dr[streetNumberMap.RetsName].ToString().Trim() + " " : "") + dr[addressMap.RetsName].ToString().Trim().ToLower());
		if (cityMap != null)
			addressEntity.City = textInfo.ToTitleCase(dr[cityMap.RetsName].ToString().Trim().ToLower());
		if (stateMap != null)
			addressEntity.StateID = dr[stateMap.RetsName].ToString().Trim().ToLower().Contains("ga") ? (int)States.GA : (int)States.SC;
		if (zipMap != null)
			addressEntity.Zip = zip == "00000" || zip == "00000-0000" ? "" : zip;
		if (lonMap != null && dr[lonMap.RetsName] != null && !string.IsNullOrEmpty(dr[lonMap.RetsName].ToString())) addressEntity.Longitude = Convert.ToDecimal(dr[lonMap.RetsName].ToString());
		if (latMap != null && dr[latMap.RetsName] != null && !string.IsNullOrEmpty(dr[latMap.RetsName].ToString())) addressEntity.Latitude = Convert.ToDecimal(dr[latMap.RetsName].ToString());
		addressEntity.Save();

		return addressEntity;
	}
	#endregion

	#region Helpers

	private static void UpdateAllGeolocation()
	{
		int failCount = 0;
		List<Address> allAddresses = Address.GetAll();
		List<Address> allAddressesWithoutLatLong = allAddresses.Where(a => !a.Latitude.HasValue || !a.Longitude.HasValue).ToList();
		foreach (Address address in allAddressesWithoutLatLong)
		{
			Address equivelantAddress =
				allAddresses.FirstOrDefault(a =>
				(
					(a.Address1 == null && address.Address1 == null)
					|| ((a.Address1 != null && address.Address1 != null) && a.Address1.Trim().Equals(address.Address1.Trim(), StringComparison.OrdinalIgnoreCase))
				)
				&& a.City.Equals(address.City, StringComparison.OrdinalIgnoreCase)
				&& a.StateID == address.StateID
				&& address.Zip != null && a.Zip != null && a.Zip.Trim().Equals(address.Zip.Trim(), StringComparison.OrdinalIgnoreCase)
				&& a.Latitude != null
				&& a.Longitude != null);
			if (equivelantAddress != null)
			{
				address.Latitude = equivelantAddress.Latitude;
				address.Longitude = equivelantAddress.Longitude;
				address.Save();
			}
			else if (address.StateID != null && ((!String.IsNullOrWhiteSpace(address.Address1) && !String.IsNullOrWhiteSpace(address.City)) || !String.IsNullOrWhiteSpace(address.Zip)))
			{
				string tempaddress = address.Address1 + " " + address.City + " " + (address.StateID == (int)States.GA ? "GA" : "SC") + " " + address.Zip;
				decimal? outLat;
				decimal? outLong;
				failCount = Helpers.GetLatLong(tempaddress.Trim(), out outLat, out outLong);
				address.Latitude = outLat;
				address.Longitude = outLong;
				address.Save();
			}
			if (failCount > 15) break;
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
			newNames[n] = newNames[n].Replace("#", "Number");
			if (!string.IsNullOrEmpty(newNames[n]))
				ds.Tables[tablename].Columns[n].ColumnName = newNames[n];
			else
			{
				newNames[n] = ds.Tables[tablename].Columns[n].ColumnName;
			}
		}
		dsOut = ds;
		return newNames;
	}

	private static void AddNewAttributes(DataSet ds, List<string> attributes, List<RetsMapping> propertyMap, string city)
	{
		Helpers.PurgeCacheItems("Showcase");
		// Get all existing attributes
		List<Showcases> showcases = Showcases.GetAll().Where(s => s.Title.ToLower().Contains(city.ToLower()) && s.MLSData).ToList();
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
			// If not add the attribute
			// for each showcase check if attribute exists and add it if needed
			foreach (Showcases showcase in showcases)
			{
				if (!currentAttrs.Any(j => j.ShowcaseID == showcase.ShowcaseID && i.Equals(j.MLSAttributeName)) && !(i.Equals("Photo location")))
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
					var attr = new ShowcaseAttribute
						{
							Title = i,
							DisplayOrder = (short?)maxDisplayOrder,
							Active = false,
							ShowcaseFilterID = numeric ? (int)FilterTypes.RangeSlider : (int)FilterTypes.DropDown,
							Numeric = numeric,
							ShowcaseID = showcase.ShowcaseID,
							MLSAttributeName = i,
							ImportItemAttribute = false
						};
					attr.Save();
				}
				++maxDisplayOrder;
			}

		});
	}

	private static List<RetsMapping> SetPropertyColumnMap()
	{
		List<RetsMapping> tempmap = new List<RetsMapping>
			{
				new RetsMapping("City", "City", true),
				new RetsMapping("Street Number", "Street Number", false),
				new RetsMapping("Address", "Address", false),
				new RetsMapping("State", "State", true),
				new RetsMapping("Summary", "Property Description", false),
				new RetsMapping("PropertyType","Property Type",true),
				new RetsMapping("MLS Number", "MLS Number", false),
				new RetsMapping("Zip Code", "Zip Code", true),
				new RetsMapping("Status", "Property Status", false),
				new RetsMapping("PhotoURL", "Photo location", false),
				new RetsMapping("New Construction", "New Construction", true),
				new RetsMapping("List Price", "List Price", true),
				new RetsMapping("ListingDate", "Listing Date", true),
				new RetsMapping("Bedrooms", "Bedrooms", true),
				new RetsMapping("Full Baths", "Full Baths", true),
				new RetsMapping("Half Baths", "Half Baths", true),
				new RetsMapping("TotalSqFt", "Apx Total Heated SqFt", true),
				new RetsMapping("TotalSqFt-Alternate", "Apx Heated SqFt", true),
				new RetsMapping("Neighborhood", "Subdivision", true),
				new RetsMapping("Town", "Town/Subdivision", true),
				new RetsMapping("Builder", "Builder Name", true),
				new RetsMapping("AgentID", "LA ID", true),
				new RetsMapping("OfficeID", "Listing Office", true),
				new RetsMapping("Elementary School", "Elementary School", true),
				new RetsMapping("High School", "High School", true),
				new RetsMapping("Middle School", "Middle School", true),
				new RetsMapping("Directions", "Directions", false),
				new RetsMapping("Virtual Tour", "Virtual Tour", false),
				new RetsMapping("Latitude", "Latitude", false),
				new RetsMapping("Longitude", "Longitude", false),
				new RetsMapping("Total Acres","Total Acres",true) 
			};


		return tempmap;
	}

	private static List<RetsMapping> SetOfficeColumnMap()
	{
		var tempmap = new List<RetsMapping>
			{
				new RetsMapping("Fax", "Fax", true),
				new RetsMapping("Franchise IDX Opt-In", "Franchise IDX Opt-In", false),
				new RetsMapping("Address", "Mail Address 1", false),
				new RetsMapping("State", "Mail State", true),
				new RetsMapping("City", "Mail City", false),
				new RetsMapping("Zip", "Mail Zip Code", false),
				new RetsMapping("Phone", "Main", true),
				new RetsMapping("NRDS Number", "NRDS Number", false),
				new RetsMapping("Email", "Office Email", false),
				new RetsMapping("OfficeID", "Office ID", false),
				new RetsMapping("Date Modified", "Office Modified", true),
				new RetsMapping("Office Name", "Office Name", true),
				new RetsMapping("Web Address", "Web Address", true)
			};
		return tempmap;
	}

	private static List<RetsMapping> SetAgentColumnMap()
	{
		var tempmap = new List<RetsMapping>
			{
				new RetsMapping("Home Phone", "Home", true),
				new RetsMapping("AgentID", "Agent ID", false),
				new RetsMapping("OfficeID", "Office ID", false),
				new RetsMapping("Address", "Mail Address 1", false),
				new RetsMapping("State", "Mail State", true),
				new RetsMapping("City", "Mail City", false),
				new RetsMapping("Zip", "Mail Zip Code", false),
				new RetsMapping("Phone", "Contact Number", true),
				new RetsMapping("NRDS Number", "NRDS Number", false),
				new RetsMapping("Email", "Agent Email", false),
				new RetsMapping("First Name", "First Name", false),
				new RetsMapping("Date Modified", "Agent Modified", true),
				new RetsMapping("Last Name", "Last Name", true),
				new RetsMapping("Web Address", "Web Address", true)
			};
		return tempmap;
	}

	private List<AgentException> SetAgentExceptionList()
	{
		var templist = new List<AgentException>
			{
				new AgentException("Michael", "Sheftic", "MCSheftic@meybohm.com","999-999")
			};
		templist.Add(new AgentException("Scott", "Brantley", "jsBrantley", "37-256", "scottbrantley13@comcast.net"));
		templist.Add(new AgentException("Scott", "Brantley", "jsBrantley", "187-280", "scottbrantley13@comcast.net"));

		return templist;
	}

	private void CheckforUpdate(ShowcaseItem item, DataSet ds, DataRow dr, List<string> colNames)
	{
		bool logCreated = false;
		bool addressChange = false;
		List<ShowcaseAttribute> attributesToImport = ShowcaseAttribute.ShowcaseAttributePage(0, 0, "", "", true, new ShowcaseAttribute.Filters { FilterShowcaseAttributeShowcaseID = item.ShowcaseID.ToString(), FilterShowcaseAttributeImportItemAttribute = true.ToString() });
		foreach (RetsMapping mapping in PropertyMap.Where(p => !p.Attribute))
		{
			if (ds.Tables[0].Columns.Contains(mapping.RetsName))
			{
				switch (mapping.FieldName)
				{
					case "MLS Number":
						break;
					case "Street Number":
					case "Address":
						if (!(Address.GetByID(item.AddressID).Address1.ToLower().Contains(dr[mapping.RetsName].ToString().Trim().ToLower())))
							addressChange = true;
						break;
					case "Summary":
						if (!item.Summary.ToLower().Contains(CleanUpDescriptionText(dr[mapping.RetsName].ToString().Trim().ToLower())))
						{
							PropertyChangeLog log = new PropertyChangeLog
								{
									Attribute = mapping.FieldName,
									DateStamp = DateTime.UtcNow,
									OldValue = item.Summary,
									NewValue = CleanUpDescriptionText(dr[mapping.RetsName].ToString().Trim()),
									ShowcaseItemID = item.ShowcaseItemID
								};
							log.Save();
							m_ChangedProperties.Add(log);
							logCreated = true;
						}
						break;
					case "Status":
						if (!String.IsNullOrWhiteSpace(dr[mapping.RetsName].ToString()) && dr[mapping.RetsName].ToString().Trim() != "Active")
						{
							PropertyChangeLog log = new PropertyChangeLog
							{
								Attribute = mapping.FieldName,
								DateStamp = DateTime.UtcNow,
								OldValue = string.Empty,
								NewValue = dr[mapping.RetsName].ToString().Trim(),
								ShowcaseItemID = item.ShowcaseItemID
							};
							log.Save();
							m_ChangedProperties.Add(log);
							logCreated = true;
						}
						break;
					case "PhotoURL":
						break;
				}
			}
		}
		if (addressChange)
		{
			RetsMapping streetNumberMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Street Number"));
			RetsMapping addressMap = PropertyMap.FirstOrDefault(r => r.FieldName.Equals("Address"));
			PropertyChangeLog log = new PropertyChangeLog
			{
				Attribute = "Address",
				DateStamp = DateTime.UtcNow,
				OldValue = Address.GetByID(item.AddressID).Address1,
				NewValue = textInfo.ToTitleCase(((streetNumberMap != null ? dr[streetNumberMap.RetsName].ToString().Trim() + " " : "") + (addressMap != null ? dr[addressMap.RetsName].ToString().Trim() : "")).ToLower()),
				ShowcaseItemID = item.ShowcaseItemID
			};
			log.Save();
			m_ChangedProperties.Add(log);
			logCreated = true;
		}
		List<ShowcaseItemAttributeValue> itemAttributes = ShowcaseItemAttributeValue.ShowcaseItemAttributeValueGetByShowcaseItemID(item.ShowcaseItemID, "", true, new[] { "ShowcaseAttributeValue.ShowcaseAttribute" }.ToList());
		foreach (string colName in colNames)
		{
			if (!attributesToImport.Any(s => s.MLSAttributeName == colName))
				continue;
			if (m_AttributesToCommaSeparate.Contains(colName))
			{
				List<string> newValues = textInfo.ToTitleCase(dr[colName].ToString().Trim().ToLower()).Split(',').Select(s => s.Trim().TrimStart(',').TrimEnd(',')).Where(s => !String.IsNullOrWhiteSpace(s)).ToList();
				newValues.RemoveAll(s => s.Trim().ToLower().StartsWith("other-s") || s.Trim().Equals("None", StringComparison.OrdinalIgnoreCase));
				List<ShowcaseItemAttributeValue> oldValues = itemAttributes.Where(i => i.ShowcaseAttributeValue.ShowcaseAttribute.MLSAttributeName.Equals(colName)).ToList();
				if ((newValues.Any(s => !String.IsNullOrWhiteSpace(s)) || oldValues.Any()) &&
					(newValues.Any(s => !oldValues.Any(v => v.ShowcaseAttributeValue.Value.Trim().Equals(s.Trim(), StringComparison.OrdinalIgnoreCase))) ||
						oldValues.Any(v => !newValues.Any(s => v.ShowcaseAttributeValue.Value.Trim().Equals(s.Trim(), StringComparison.OrdinalIgnoreCase)))))
				{
					PropertyChangeLog log = new PropertyChangeLog
					{
						Attribute = colName,
						DateStamp = DateTime.UtcNow,
						OldValue = string.Join(",", oldValues.Select(s => s.ShowcaseAttributeValue.Value.Trim())),
						NewValue = string.Join(",", newValues.Select(s => s.Trim())),
						ShowcaseItemID = item.ShowcaseItemID
					};
					log.Save();
					m_ChangedProperties.Add(log);
					logCreated = true;
				}
			}
			else
			{
				string attributeValue = dr[colName].ToString().Trim();
				if (attributeValue.Trim().ToLower().StartsWith("other-s") || attributeValue.Equals("None", StringComparison.OrdinalIgnoreCase) || attributeValue.Equals("Other", StringComparison.OrdinalIgnoreCase))
					continue;
				ShowcaseItemAttributeValue itemAttr = itemAttributes.FirstOrDefault(i => i.ShowcaseAttributeValue.ShowcaseAttribute.MLSAttributeName.Equals(colName));

				if ((itemAttr == null && !String.IsNullOrWhiteSpace(attributeValue) && attributesToImport.Any(a => a.MLSAttributeName.Equals(colName.Trim(), StringComparison.OrdinalIgnoreCase))) ||
					(itemAttr != null && !itemAttr.ShowcaseAttributeValue.Value.Trim().Equals(attributeValue, StringComparison.OrdinalIgnoreCase)))
				{
					PropertyChangeLog log = new PropertyChangeLog
					{
						Attribute = colName,
						DateStamp = DateTime.UtcNow,
						OldValue = itemAttr != null ? itemAttr.ShowcaseAttributeValue.Value : string.Empty,
						NewValue = textInfo.ToTitleCase(attributeValue.ToLower()),
						ShowcaseItemID = item.ShowcaseItemID
					};
					log.Save();
					m_ChangedProperties.Add(log);
					logCreated = true;
				}
			}
		}
		if (logCreated)
			HandleLogCreated(item.ShowcaseItemID);
	}

	void HandleLogCreated(int showcaseItemID)
	{
		List<SavedSearch> savedSearchNotifications = SavedSearch.SavedSearchPage(0, 0, "", "", true, new SavedSearch.Filters { FilterSavedSearchEnableEmailNotifications = true.ToString(), FilterSavedSearchShowcaseItemID = showcaseItemID.ToString(), FilterSavedSearchSeparateEmail = true.ToString() });
		List<int> userIDs = savedSearchNotifications.Select(s => s.UserID).Distinct().ToList();
		foreach (int userID in userIDs)
		{
			List<SavedSearch> savedProperties = savedSearchNotifications.Where(s => s.UserID == userID).ToList();
			foreach (SavedSearch savedProperty in savedProperties)
			{
				savedProperty.LastAlertDate = DateTime.UtcNow;
				savedProperty.Save();
				SavedSearch.SendAlertEmails(savedProperty);
			}
		}
	}

    private void SendErrorEmail(string message, string body, string subject)
    {
        MailMessage email = new MailMessage();
        email.From = new MailAddress(Globals.Settings.FromEmail);


        email.To.Add(Classes.Rets.Settings.RetsFailEmail1);
        if (!string.IsNullOrEmpty(Classes.Rets.Settings.RetsFailEmail2))
            email.To.Add(Classes.Rets.Settings.RetsFailEmail2);
        if (!string.IsNullOrEmpty(Classes.Rets.Settings.RetsFailEmail3))
            email.To.Add(Classes.Rets.Settings.RetsFailEmail2);
        email.IsBodyHtml = true;
        email.Body = body ?? ("There has been an error with the " + (Request.QueryString["version"] != null ? "full download" : "incremental download") + "in the " + message + " at " + Helpers.ConvertUTCToClientTime(DateTime.UtcNow).ToString(CultureInfo.InvariantCulture) + ". Please go to the site admin for more details.");
        email.Subject = subject ?? "There has been an error during MLS Data Import";
        SmtpClient smtp = new SmtpClient();
        smtp.Send(email);
    }

	private void SendErrorEmail(string message, string body = null)
	{
        SendErrorEmail(message, body, null);
	}

	public void SendSavedSearchAlerts()
	{
		if (!m_ChangedProperties.Any())
			return;
		List<SavedSearch> savedSearchNotifications = SavedSearch.SavedSearchPage(0, 0, "", "", true, new SavedSearch.Filters { FilterSavedSearchEnableEmailNotifications = true.ToString(), FilterSavedSearchSeparateEmail = true.ToString() });
		List<int> userIDs = savedSearchNotifications.Select(s => s.UserID).Distinct().ToList();
		foreach (int userID in userIDs)
		{
			//Don't send the same email more than once per day
			List<SavedSearch> savedShowcaseSearch = savedSearchNotifications.Where(s => s.UserID == userID && s.FilterString != null && (!s.LastAlertDate.HasValue || s.LastAlertDateClientTime.Value.Date != DateTime.Now.Date)).ToList();
			foreach (SavedSearch savedSearch in savedShowcaseSearch)
			{
				List<ShowcaseItemForJSON> changedItems = ShowcaseItem.GetItemsFromSearch(savedSearch.FilterString, savedSearch.ShowcaseID, m_ChangedProperties.Select(s => s.ShowcaseItemID).Distinct().ToList());
				if (changedItems.Any())
				{
					savedSearch.LastAlertDate = DateTime.UtcNow;
					savedSearch.LastAlertCount = changedItems.Count;
					savedSearch.Save();
					SavedSearch.SendAlertEmails(savedSearch, new WhatChanged { ChangeLog = m_ChangedProperties.Where(s => changedItems.Any(p => s.ShowcaseItemID == p.ShowcaseItemID)).ToList(), PropertiesThatChanged = changedItems, SavedSearch = savedSearch });
				}
			}
		}
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

	private string CleanUpDescriptionText(string description)
	{
		int numCaps = 0;
		const int numCapsToTransform = 20;
		foreach (char c in description)
		{
			if (Char.IsUpper(c))
				numCaps++;
			else if (Char.IsLetter(c))
				numCaps = 0;
			if (numCaps >= numCapsToTransform)
				break;
		}
		if (numCaps < numCapsToTransform)
			return description;
		description = description.ToLower();
		string[] alphabet = new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
		foreach (string letter in alphabet)
		{
			description = description.Replace(". " + letter, ". " + letter.ToUpper())
									 .Replace(".  " + letter, ".  " + letter.ToUpper())
									 .Replace("! " + letter, "! " + letter.ToUpper())
									 .Replace("!  " + letter, "!  " + letter.ToUpper())
									 .Replace(": " + letter, ": " + letter.ToUpper())
									 .Replace("? " + letter, "? " + letter.ToUpper());
		}
		description = description.Replace(" asu ", " ASU ")
								 .Replace(" mcg ", " MCG ")
								 .Replace(" va ", " VA ");

		string[] pronounList = new[] { "Aiken", "Arbor Landing", "Arbor Springs", "Augusta", "Baldwin Place", "Bath", "Beech Island", "Belvedere", "Berkley Hills", "Berzelia Commons", "Birchfield", "Brownstone's at Rae's Creek", "Canterbury Farms", "Cedar Creek", "Chamblin Ridge", "Champions Retreat", "Clearwater", "Columbia", "Connor Place", "Crawford Creek", "Dogwood Chase", "Dunnington", "Evans", "Forest Place", "Gem Lakes", "Goshen Plantation", "Graniteville", "Graylyn Lakes", "Grove Landing", "Grovetown", "Hardy Station", "Harlem", "Hephzibah", "Heritage Pine", "High Meadows", "Houndslake", "Jackson", "Lakes and Streams", "Lakes at Spirit Creek", "Langley", "Lincolnton", "Longstreet Place", "Magnolia Villas", "Martinez", "Mediterranean", "Modoc", "Mount Vintage Plantation", "North Augusta", "Oakridge Plantation", "Reynolds Pond", "Rhodes Farm", "Richmond", "Ridge at Chukker Creek", "Riverwood Plantation", "Somerset at Williamsburg", "Springstone Villas", "Summerville", "Sumter Landing", "The Enclave", "The Retreat", "Thomson", "Walton Acres", "Walton Farms", "Wando Woodlands", "Warrenville", "Waynesboro", "Whatley Place", "Windmill Plantation", "Woodside", "Woodside Plantation" };

		foreach (string pronoun in pronounList)
		{
			description = description.Replace(pronoun.ToLower(), pronoun);
		}
		return description.Length > 1 ? description.Substring(0, 1).ToUpper() + description.Substring(1) : description;
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

	public class AgentException
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string MLSID { get; set; }

		public AgentException(string firstName, string lastName, string userName, string mlsID, string email = null)
		{
			FirstName = firstName;
			LastName = lastName;
			UserName = userName;
			Email = email;
			MLSID = mlsID;
		}
	}
	#endregion
}
