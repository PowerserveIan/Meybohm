using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Showcase;

public partial class ShowcaseItemPage : BasePage
{
	protected const int numberOfStaticTabs = 7;
	protected int numberOfCollections;
	protected int? m_ShowcaseID;
	protected string m_MapLocation;

	protected decimal? m_ShowcaseItemLatitude;
	protected decimal? m_ShowcaseItemLongitude;

	protected int ShowcaseItemId
	{
		get
		{
			int tempID;
			if (Request.QueryString["Id"] != null)
				if (Int32.TryParse(Request.QueryString["Id"], out tempID))
					return tempID;

			return 0;
		}
	}

	protected bool IsAiken { get { return m_ShowcaseID.HasValue && (m_ShowcaseID.Value == (int)MeybohmShowcases.AikenExistingHomes || m_ShowcaseID.Value == (int)MeybohmShowcases.AikenLand || m_ShowcaseID.Value == (int)MeybohmShowcases.AikenRentalHomes); } }

	protected bool m_IsOwnProperty;

	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
		m_AdditionalJavaScriptFiles = uxJavaScripts;
		EnableWhiteSpaceCompression = false;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Showcase";
		ComponentAdminPage = "showcase/admin-showcase-item.aspx";
		ComponentAdditionalLink = "~/admin/showcase/admin-showcase-item-edit.aspx?id=" + Request.QueryString["id"] + "&frontendView=true";
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		ShowcaseItem showcaseItemEntity = ShowcaseItem.GetByID(ShowcaseItemId, new string[] { "Address", "Address.State", "Office" });
		if (showcaseItemEntity != null && showcaseItemEntity.Active)
		{
			if (ShowcaseHelpers.GetDefaultShowcaseID() != showcaseItemEntity.ShowcaseID)
				m_ShowcaseID = showcaseItemEntity.ShowcaseID;
			#region MainDesign
			uxAddSavedSearch.ShowcaseItemID = ShowcaseItemId;
			uxAddSavedSearch.ShowcaseID = showcaseItemEntity.ShowcaseID;
			uxSaveSearchPH.Visible = !User.Identity.IsAuthenticated || !SavedSearch.SavedSearchPage(0, 1, "", "", true, new SavedSearch.Filters { FilterSavedSearchShowcaseItemID = ShowcaseItemId.ToString(), FilterSavedSearchUserID = Helpers.GetCurrentUserID().ToString() }).Any();
			if (showcaseItemEntity.Address != null)
			{
				uxAddress.Text = uxAddressInMapTab.Text = showcaseItemEntity.Address.Address1 + "<br />" + showcaseItemEntity.Address.City + ", " + showcaseItemEntity.Address.State.Abb + " " + showcaseItemEntity.Address.Zip;
				m_ShowcaseItemLatitude = showcaseItemEntity.Address.Latitude;
				m_ShowcaseItemLongitude = showcaseItemEntity.Address.Longitude;
			}
			uxItemSummary.Text = showcaseItemEntity.Summary;
			uxMainImageTopRight.AlternateText =uxImage.AlternateText = showcaseItemEntity.Title;
			uxMainImageTopRight.ImageUrl = uxImage.ImageUrl = Helpers.ResizedImageUrl(showcaseItemEntity.Image, Globals.Settings.UploadFolder + "images/", uxImage.ResizerWidth, uxImage.ResizerHeight, true, false);
			uxTopArea.Visible = !String.IsNullOrEmpty(showcaseItemEntity.Image);
			uxLinkToPage.Text = Helpers.RootPath + "showcase.aspx?" + (Settings.MultipleShowcases ? "ShowcaseID=" + showcaseItemEntity.ShowcaseID + "&amp;" : "") + "id=" + ShowcaseItemId;
			if (!uxTopArea.Visible)
				uxLinkWrapper.Attributes["class"] += " noTopImage";
			uxPrice.Text = showcaseItemEntity.ListPrice.HasValue ? showcaseItemEntity.ListPrice.Value.ToString("C").Replace(".00", "") : "";
			uxMLSNumberPH.Visible = showcaseItemEntity.MlsID.HasValue;
			uxMLSNumber.Text = showcaseItemEntity.MlsID.ToString();
			
			uxWebsite.Visible = !String.IsNullOrWhiteSpace(showcaseItemEntity.Website);
			if (uxWebsite.Visible)
			{
				uxWebsite.NavigateUrl = showcaseItemEntity.Website;
				uxWebsite.Attributes["onclick"] = "TrackClick(" + (int)ClickTypes.Website + ");";
			}
			uxVirtualTour.Visible = !String.IsNullOrWhiteSpace(showcaseItemEntity.VirtualTourURL);
			if (uxVirtualTour.Visible)
			{
				uxVirtualTour.NavigateUrl = showcaseItemEntity.VirtualTourURL;
				uxVirtualTour.Attributes["onclick"] = "TrackClick(" + (int)ClickTypes.VirtualTour + ");";
			}
			uxAvailableDate.Visible = showcaseItemEntity.AvailabilityDate.HasValue;
			if (showcaseItemEntity.AvailabilityDate.HasValue)
				uxAvailableDate.Text = "Available: " + (showcaseItemEntity.AvailabilityDate <= DateTime.UtcNow ? "Now" : showcaseItemEntity.AvailabilityDateClientTime.Value.ToString("MMMM d, yyyy"));
			#endregion
			#region Overview Tab
			if (Settings.AttributeDisplayStyle == AttributeDisplays.Tabs)
			{
				uxTabsAttributes.ShowcaseAttributes = ShowcaseAttribute.GetAttributesAndValuesByShowcaseItemID(ShowcaseItemId, null, false);
				uxTabsAttributes.DataBind();
			}
			uxBrokerReciprocityPH.Visible = showcaseItemEntity.Office != null && !showcaseItemEntity.Office.IsMeybohm;
			if (uxBrokerReciprocityPH.Visible)
				uxBrokerOffice.Text = showcaseItemEntity.Office.Name;
			#endregion
			#region Map Tab
			uxMapLI.Visible = uxMapPH.Visible = uxMapsJSPH.Visible = m_ShowcaseItemLatitude.HasValue && m_ShowcaseItemLongitude.HasValue;
			if (m_ShowcaseItemLatitude.HasValue && m_ShowcaseItemLongitude.HasValue)
			{
				m_MapLocation = "new markerItem(" + m_ShowcaseItemLatitude.Value + ", " + m_ShowcaseItemLongitude.Value + ", \"\")";
				uxMapDirections.NavigateUrl = "http://maps.google.com/maps?f=d&source=s_d&daddr=" + m_ShowcaseItemLatitude.Value + "," + m_ShowcaseItemLongitude.Value + "&hl=en";
				uxMapBirdsEye.NavigateUrl = string.Format("http://www.bing.com/maps/?v=2&lvl=20&dir=0&sty=b&where1={0}&form=LMLTCC", (showcaseItemEntity.Address.Address1 + " " + showcaseItemEntity.Address.City + ", " + showcaseItemEntity.Address.State.Abb + " " + showcaseItemEntity.Address.Zip).Trim());
			}
			uxNoMapsJSPH.Visible = !uxMapsJSPH.Visible;
			uxDirections.Text = showcaseItemEntity.Directions;
			#endregion
			#region Neighborhood Tab
			uxNeighborhoodLI.Visible = uxNeighborhoodPH.Visible = showcaseItemEntity.NeighborhoodID.HasValue;
			#endregion
			#region Builder Tab
			uxBuilderLI.Visible = uxBuilderPH.Visible = showcaseItemEntity.BuilderID.HasValue;
			#endregion
			List<NearbyLocations> nearByLocations = m_ShowcaseItemLatitude.HasValue && m_ShowcaseItemLongitude.HasValue ? Classes.WhatsNearBy.WhatsNearByLocation.GetLocationsNearCoordinates(m_ShowcaseItemLatitude.Value, m_ShowcaseItemLongitude.Value) : new List<NearbyLocations>();
			#region School Tab
			uxSchoolsLI.Visible = uxSchoolsPH.Visible = showcaseItemEntity.ElementarySchoolID.HasValue || showcaseItemEntity.MiddleSchoolID.HasValue || showcaseItemEntity.HighSchoolID.HasValue;
			BindSchool(nearByLocations, showcaseItemEntity.ElementarySchoolID, "Elementary");
			BindSchool(nearByLocations, showcaseItemEntity.MiddleSchoolID, "Middle");
			BindSchool(nearByLocations, showcaseItemEntity.HighSchoolID, "High");
			#endregion
			#region Whats Near By Tab
			uxWhatsNearByRepeater.DataSource = nearByLocations.Where(n => (int?)n.WhatsNearByLocationID != showcaseItemEntity.ElementarySchoolID && (int?)n.WhatsNearByLocationID != showcaseItemEntity.MiddleSchoolID && (int?)n.WhatsNearByLocationID != showcaseItemEntity.HighSchoolID);
			uxWhatsNearByRepeater.DataBind();

			uxWhatsNearByLI.Visible = uxWhatsNearByRepeater.Visible = uxWhatsNearByRepeater.Items.Count > 0;
			#endregion
			#region Contact Tab
			if (showcaseItemEntity.AgentID.HasValue)
			{
				Classes.Media352_MembershipProvider.UserInfo agentInfo = Classes.Media352_MembershipProvider.UserInfo.UserInfoGetByUserID(showcaseItemEntity.AgentID.Value).FirstOrDefault();
				uxAgentImage.Visible  = !String.IsNullOrWhiteSpace(agentInfo.Photo);
				if (uxAgentImage.Visible)
				{
					uxAgentImage.ImageUrl =  uxMainImage3.ImageUrl = Helpers.ResizedImageUrl(agentInfo.Photo, Globals.Settings.UploadFolder + "agents/", uxAgentImage.ResizerWidth, uxAgentImage.ResizerHeight, true);
					uxAgentImage.AlternateText = uxMainImage3.AlternateText = agentInfo.FirstAndLast;
				}
				
				uxAgentName.Text  = uxMainAgentName.Text = agentInfo.FirstAndLast;
				uxAgentPhone.Text = uxMainAgentPhone.Text = (agentInfo.PrimaryPhone == "Office Phone" ? agentInfo.OfficePhone : agentInfo.PrimaryPhone == "Home Phone" ? agentInfo.HomePhone : agentInfo.CellPhone);
			}
			else if (showcaseItemEntity.TeamID.HasValue)
			{
				Classes.Media352_MembershipProvider.Team teamInfo = Classes.Media352_MembershipProvider.Team.GetByID(showcaseItemEntity.TeamID.Value);
				Dictionary<string, string> phoneNumbers = new Dictionary<string, string>();
				if (teamInfo != null)
				{
					uxAgentImage.Visible =  !String.IsNullOrWhiteSpace(teamInfo.Photo);
					if (uxAgentImage.Visible)
					{
						uxAgentImage.ImageUrl =  Helpers.ResizedImageUrl(teamInfo.Photo, Globals.Settings.UploadFolder + "agents/", uxAgentImage.ResizerWidth, uxAgentImage.ResizerHeight, true);
						uxAgentImage.AlternateText =  teamInfo.Name;
					}
					uxAgentName.Text =  uxMainAgentName.Text = teamInfo.Name;
					if (!String.IsNullOrWhiteSpace(teamInfo.Phone))
						phoneNumbers.Add("Main", teamInfo.Phone);
				}
				uxAgentPhone.Visible =  false;

				List<Classes.Media352_MembershipProvider.UserTeam> usersOnTeam = Classes.Media352_MembershipProvider.UserTeam.UserTeamGetByTeamID(showcaseItemEntity.TeamID.Value, includeList: new[] { "User.UserInfo" });

				foreach (var agent in usersOnTeam)
				{
					if (agent.User.UserInfo != null && agent.User.UserInfo.Any())
					{
						Classes.Media352_MembershipProvider.UserInfo agentInfo = agent.User.UserInfo.First();
						if (!String.IsNullOrWhiteSpace(agentInfo.OfficePhone) && !phoneNumbers.ContainsKey("Office"))
							phoneNumbers.Add("Office", agentInfo.OfficePhone);
						if (!String.IsNullOrWhiteSpace(agentInfo.CellPhone))
						{
							string key = agentInfo.FirstName + " Cell";
							if (phoneNumbers.ContainsKey(key) && !String.IsNullOrWhiteSpace(agentInfo.LastName))
								key = agentInfo.FirstName + " " + agentInfo.LastName.Substring(0, 1) + "." + " Cell";
							if (!phoneNumbers.ContainsKey(key))
								phoneNumbers.Add(key, agentInfo.CellPhone);
						}
					}
				}
				if (phoneNumbers.Any())
				{
					uxMainTeamPhones.Visible = uxTeamPhones.Visible  = true;
					uxMainTeamPhones.DataSource = uxTeamPhones.DataSource = phoneNumbers;
					uxMainTeamPhones.DataBind();
					uxTeamPhones.DataBind();
				}
			}
			else
				uxAgentPH.Visible =  uxMainAgentPH.Visible = false;
			uxContactForm.ShowcaseItemID = ShowcaseItemId;
			#endregion
			#region Open House Tab
			List<OpenHouse> openHouses = OpenHouse.GetFutureOpenHouses(ShowcaseItemId, DateTime.UtcNow.AddMonths(1));
			uxOpenHouseLI.Visible = uxOpenHousePH.Visible = openHouses.Any();
			if (uxOpenHouseLI.Visible)
			{
				uxOpenHouseTimes.DataSource = openHouses;
				uxOpenHouseTimes.DataBind();

				if (showcaseItemEntity.OpenHouseAgentID.HasValue)
				{
					Classes.Media352_MembershipProvider.UserInfo agentInfo = Classes.Media352_MembershipProvider.UserInfo.UserInfoGetByUserID(showcaseItemEntity.OpenHouseAgentID.Value).FirstOrDefault();
					uxOpenHouseAgentImage.Visible = !String.IsNullOrWhiteSpace(agentInfo.Photo);
					if (uxOpenHouseAgentImage.Visible)
					{
						uxOpenHouseAgentImage.ImageUrl = Helpers.ResizedImageUrl(agentInfo.Photo, Globals.Settings.UploadFolder + "agents/", uxAgentImage.ResizerWidth, uxAgentImage.ResizerHeight, true);
						uxOpenHouseAgentImage.AlternateText = agentInfo.FirstAndLast;
					}
					uxOpenHouseAgentName.Text = agentInfo.FirstAndLast;
					uxOpenHouseAgentPhone.Text = agentInfo.CellPhone;
				}
				else
					uxOpenHouseAgentPH.Visible = false;
				if (!String.IsNullOrWhiteSpace(Request.QueryString["OpenHouse"]) && Request.QueryString["OpenHouse"] == "true")
					ClientScript.RegisterStartupScript(GetType(), "viewtarget", "$(document).ready(function(){setTimeout(function(){$('a[href=\"#tabs-7\"]').trigger('click');}, 1);});", true);
			}
			#endregion
			List<MediaCollection> listOfCollections = MediaCollection.GetAllActiveByShowcaseItemID(ShowcaseItemId);
			numberOfCollections = listOfCollections.Count;

			uxCollectionsRepeater.DataSource = listOfCollections;
			uxCollectionsRepeater.DataBind();

			uxTabsRepeater.DataSource = listOfCollections;
			uxTabsRepeater.DataBind();

			//Statistics Tracking
			int? userID = Page.User.Identity.IsAuthenticated ? (int?)Helpers.GetCurrentUserID() : null;
			m_IsOwnProperty = showcaseItemEntity.AgentID == userID;
			Dictionary<int, List<int>> sessionHomeVisits = Session["HomeVisits"] as Dictionary<int, List<int>> ?? new Dictionary<int, List<int>>();
			if (Settings.EnableStatisticsTracking && !sessionHomeVisits.Any(v => v.Key == ShowcaseItemId && v.Value.Any(val => val == (int)ClickTypes.Visit)))
			{
				if (!m_IsOwnProperty)
				{
					ShowcaseItemMetric userClick = new ShowcaseItemMetric { ClickTypeID = (int)ClickTypes.Visit, Date = DateTime.Now, SessionID = HttpContext.Current.Session.SessionID, ShowcaseItemID = ShowcaseItemId, UserID = userID };
					userClick.Save();
				}
				if (!sessionHomeVisits.ContainsKey(ShowcaseItemId))
					sessionHomeVisits.Add(ShowcaseItemId, new List<int>());
				sessionHomeVisits[ShowcaseItemId].Add((int)ClickTypes.Visit);
				Session["HomeVisits"] = sessionHomeVisits;
			}

			if (Globals.Settings.FacebookEnableLikeButton)
				FacebookLike.AddMetaData(Page, showcaseItemEntity.Title, FacebookLike.FBType.Product, Request.Url.ToString(),
										 "~/" + Globals.Settings.UploadFolder + "images/" + showcaseItemEntity.Image,
										 Globals.Settings.CompanyName, showcaseItemEntity.Summary);

			CanonicalLink = Helpers.RootPath + Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite().Name.ToLower().Replace(" ", "-") + "/home-details?id=" + showcaseItemEntity.ShowcaseItemID + "&title=" + Server.UrlEncode(showcaseItemEntity.Title);
		}
		else
			Response.Redirect("search");

		if (!IsPostBack)
		{
			uxBackToShowcase.Visible = Request.UrlReferrer == null ||
									   !Server.UrlDecode(Request.UrlReferrer.AbsoluteUri.Split('?')[0]).ToLower().Contains(
										Helpers.RootPath.ToLower());
			if (!uxBackToShowcase.Visible)
			{
				//Loaded as an iFrame
				ContentPlaceHolder EntireBody = (ContentPlaceHolder)Master.FindControl("EntireBody");
				EntireBody.Controls.Clear();
				HtmlForm formCtrl = new HtmlForm();
				formCtrl.Controls.Add(uxEntireBodyPH);
				EntireBody.Controls.Add(formCtrl);
			}
			else
				uxBackToShowcase.NavigateUrl = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite().Name.ToLower().Replace(" ", "-") + "/search";
		}
	}

	void BindSchool(List<NearbyLocations> nearByLocations, int? schoolID, string schoolType)
	{
		if (!schoolID.HasValue)
		{
			HideSchool(schoolType);
			return;
		}
		NearbyLocations nearBySchool = nearByLocations.Find(n => (int?)n.WhatsNearByLocationID == schoolID);
		if (nearBySchool == null)
		{
			nearBySchool = new NearbyLocations();
			Classes.WhatsNearBy.WhatsNearByLocation school = Classes.WhatsNearBy.WhatsNearByLocation.GetByID(schoolID.Value, new string[] { "Address" }.ToList());
			if (school == null)
			{
				HideSchool(schoolType);
				return;
			}
			nearBySchool.Image = school.ImageOrPlaceHolderSrc;
			nearBySchool.Name = school.Name;
			nearBySchool.DistanceAway = Helpers.DistanceBetweenPoints(m_ShowcaseItemLatitude, m_ShowcaseItemLongitude, school.Address.Latitude, school.Address.Longitude);
		}
		if (nearBySchool != null)
		{
			switch (schoolType)
			{
				case "Elementary":
					uxElementaryImage.Visible = !String.IsNullOrWhiteSpace(nearBySchool.Image);
					if (uxElementaryImage.Visible)
						uxElementaryImage.ImageUrl = Helpers.ResizedImageUrl(nearBySchool.Image, Globals.Settings.UploadFolder + "nearByLocations/", uxElementaryImage.ResizerWidth, uxElementaryImage.ResizerHeight, true);
					uxElementaryImage.AlternateText = uxElementaryName.Text = nearBySchool.Name;
					uxElementaryDistance.Text = nearBySchool.DistanceAway.HasValue ? Math.Round(nearBySchool.DistanceAway.Value, 2).ToString() : "";
					break;
				case "Middle":
					uxMiddleImage.Visible = !String.IsNullOrWhiteSpace(nearBySchool.Image);
					if (uxMiddleImage.Visible)
						uxMiddleImage.ImageUrl = Helpers.ResizedImageUrl(nearBySchool.Image, Globals.Settings.UploadFolder + "nearByLocations/", uxMiddleImage.ResizerWidth, uxMiddleImage.ResizerHeight, true);
					uxMiddleImage.AlternateText = uxMiddleName.Text = nearBySchool.Name;
					uxMiddleDistance.Text = nearBySchool.DistanceAway.HasValue ? Math.Round(nearBySchool.DistanceAway.Value, 2).ToString() : "";
					break;
				case "High":
					uxHighImage.Visible = !String.IsNullOrWhiteSpace(nearBySchool.Image);
					if (uxHighImage.Visible)
						uxHighImage.ImageUrl = Helpers.ResizedImageUrl(nearBySchool.Image, Globals.Settings.UploadFolder + "nearByLocations/", uxHighImage.ResizerWidth, uxHighImage.ResizerHeight, true);
					uxHighImage.AlternateText = uxHighName.Text = nearBySchool.Name;
					uxHighDistance.Text = nearBySchool.DistanceAway.HasValue ? Math.Round(nearBySchool.DistanceAway.Value, 2).ToString() : "";
					break;
			}
		}
	}

	void HideSchool(string schoolType)
	{
		switch (schoolType)
		{
			case "Elementary":
				uxElementarySchoolLI.Visible = false;
				break;
			case "Middle":
				uxMiddleSchoolLI.Visible = false;
				break;
			case "High":
				uxHighSchoolLI.Visible = false;
				break;
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		//Register Showcase Web Service
		ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
		if (scriptManager.Services.Count(s => s.Path == "~/tft-services/Showcase/ShowcaseWebMethods.asmx") == 0)
			scriptManager.Services.Add(new ServiceReference { InlineScript = true, Path = "~/tft-services/Showcase/ShowcaseWebMethods.asmx" });
	}

	[WebMethod]
	public static Classes.MLS.Builder GetBuilder(int showcaseItemID)
	{
		ShowcaseItem showcaseItemEntity = ShowcaseItem.GetByID(showcaseItemID);
		return Classes.MLS.Builder.GetByID(showcaseItemEntity.BuilderID.Value);
	}

	[WebMethod]
	public static Classes.MLS.Neighborhood GetNeighborhood(int showcaseItemID)
	{
		ShowcaseItem showcaseItemEntity = ShowcaseItem.GetByID(showcaseItemID);
		Classes.MLS.Neighborhood neighborhoodEntity = Classes.MLS.Neighborhood.GetByID(showcaseItemEntity.NeighborhoodID.Value, new string[] { "Address", "Address.State" });
		neighborhoodEntity.Amenities = Helpers.ReplaceRootWithAbsolutePath(neighborhoodEntity.Amenities);
		return neighborhoodEntity;
	}
}