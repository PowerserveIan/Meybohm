using System;
using System.Collections.Generic;
using System.Linq;
using BaseCode;
using Classes.MLS;

public partial class neighborhoods_details : BasePage
{
	protected List<NearbyLocations> m_NearByLocations;

	protected override void SetCssAndJs()
	{
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Neighborhood";
		ComponentAdminPage = "m-l-s/admin-neighborhood.aspx";
		ComponentAdditionalLink = "~/admin/m-l-s/admin-neighborhood-edit.aspx?id=" + NeighborhoodID + "&frontendView=true";
		NewHomePage = true;
	}

	protected int NeighborhoodID
	{
		get
		{
			int tempID;
			if (Request.QueryString["id"] != null)
				if (Int32.TryParse(Request.QueryString["id"], out tempID))
					return tempID;

			return 0;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			Neighborhood neighborhoodEntity = Neighborhood.GetByID(NeighborhoodID, new string[] { "Address", "Address.State" }.ToList());
			if (neighborhoodEntity != null && neighborhoodEntity.Active)
			{
				uxName.Text = neighborhoodEntity.Name;
				uxImage.ImageUrl = "~/" + Globals.Settings.UploadFolder + "neighborhoods/" + neighborhoodEntity.Image + "?width=232&height=162&mode=crop&anchor=middlecenter";
				uxAddress.Text = neighborhoodEntity.Address.Address1;
				uxCity.Text = neighborhoodEntity.Address.City;
				uxState.Text = neighborhoodEntity.Address.State.Name;
				uxZip.Text = neighborhoodEntity.Address.Zip;
				uxPriceRange.Text = neighborhoodEntity.PriceRange;
				uxPriceRangePH.Visible = !String.IsNullOrWhiteSpace(neighborhoodEntity.PriceRange);
				uxDirections.Text = Helpers.ReplaceRootWithAbsolutePath(neighborhoodEntity.Directions);
				uxDirectionsPH.Visible = !String.IsNullOrWhiteSpace(neighborhoodEntity.Directions);
				uxOverview.Text = Helpers.ReplaceRootWithAbsolutePath(neighborhoodEntity.Overview);
				uxAmenities.Text = Helpers.ReplaceRootWithAbsolutePath(neighborhoodEntity.Amenities);
				uxAmenitiesPH.Visible = uxAmenitiesLI.Visible = !String.IsNullOrWhiteSpace(neighborhoodEntity.Amenities);			
				uxWebsite.Text = uxWebsite.NavigateUrl = neighborhoodEntity.Website;
				uxWebsite.Visible = !String.IsNullOrEmpty(neighborhoodEntity.Website);

				uxBuilders.DataSource = Builder.GetByNeighborhoodID(NeighborhoodID);
				uxBuilders.DataBind();

				uxBuildersLI.Visible = uxBuilders.Visible = uxBuilders.Items.Count > 0;

				m_NearByLocations = neighborhoodEntity.Address.Latitude.HasValue && neighborhoodEntity.Address.Longitude.HasValue ? Classes.WhatsNearBy.WhatsNearByLocation.GetLocationsNearCoordinates(neighborhoodEntity.Address.Latitude.Value, neighborhoodEntity.Address.Longitude.Value) : new List<NearbyLocations>();
				uxWhatsNearbyLI.Visible = uxNearbyCategories.Visible = m_NearByLocations.Any();
				if (uxNearbyCategories.Visible)
				{
					List<string> categoryNames = new List<string>();
					categoryNames.Add(null);
					foreach (NearbyLocations n in m_NearByLocations.Where(c=>!String.IsNullOrWhiteSpace(c.CategoryNames)))
					{
						foreach (string c in n.CategoryNames.Split(','))
						{
							if (!categoryNames.Any(cn => cn == c) && !String.IsNullOrWhiteSpace(c))
								categoryNames.Add(c);
						}
					}
					uxNearbyCategories.DataSource = categoryNames;
					uxNearbyCategories.DataBind();
				}

				List<ShowcaseItemForJSON> homes = Classes.Showcase.ShowcaseItem.GetPagedFilteredShowcaseItems(0, 0, "", "ListPrice", true, new Classes.Showcase.ShowcaseItem.Filters { FilterShowcaseItemActive = true.ToString(), FilterShowcaseItemNeighborhoodID = NeighborhoodID.ToString(), NewHomesOnly = (neighborhoodEntity.ShowLotsLand ? null : (bool?)true), ShowLotsLand = neighborhoodEntity.ShowLotsLand });
				uxHomes.Visible = uxHomesForSaleLI.Visible = uxHomesAvailablePH.Visible = homes.Any();
				uxHomes.DataSource = homes;
				uxHomes.DataBind();

				uxNumberHomesAvailable.Text = homes.Count.ToString();
				uxHomesAvailableS.Visible = homes.Count != 1;

				uxMapDirections.NavigateUrl = "http://maps.google.com/maps?f=d&source=s_d&daddr=" + (neighborhoodEntity.Address.Latitude.HasValue ? neighborhoodEntity.Address.Latitude.Value + "," + neighborhoodEntity.Address.Longitude.Value : neighborhoodEntity.Address.Address1 + " " + neighborhoodEntity.Address.Address2 + " " + neighborhoodEntity.Address.City + ", " + neighborhoodEntity.Address.Zip + " " + neighborhoodEntity.Address.State.Abb).Trim() + "&hl=en";
				uxMapBirdsEye.NavigateUrl = "http://www.bing.com/maps/?v=2&lvl=17&dir=0&sty=b&where1=" + (neighborhoodEntity.Address.Latitude.HasValue ? neighborhoodEntity.Address.Latitude.Value + "," + neighborhoodEntity.Address.Longitude.Value : neighborhoodEntity.Address.Address1 + " " + neighborhoodEntity.Address.Address2 + " " + neighborhoodEntity.Address.City + ", " + neighborhoodEntity.Address.Zip + " " + neighborhoodEntity.Address.State.Abb).Trim() + "&form=LMLTCC";
			}
			else
				Response.Redirect("neighborhoods");
		}
	}
}