using System;
using System.Collections.Generic;
using System.Linq;
using BaseCode;
using Classes.Media352_MembershipProvider;

public partial class staff_details : BasePage
{
	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Staff";
		ComponentAdminPage = "media352-membership-provider/admin-user.aspx?FilterUserHasRole=true";
		ComponentAdditionalLink = "~/admin/media352-membership-provider/admin-user-edit.aspx?id=" + UserID + "&frontendView=true&FilterUserHasRole=true";
	}

	protected int UserID
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

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxContactForm.AgentID = UserID;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			UserInfo userInfoEntity = UserInfo.UserInfoGetByUserID(UserID, includeList: new string[] { "JobTitle" }).FirstOrDefault();
			if (userInfoEntity != null && userInfoEntity.DisplayInDirectory)
			{
				uxImage.Visible = !String.IsNullOrEmpty(userInfoEntity.Photo);
				if (uxImage.Visible)
					uxImage.ImageUrl = Helpers.ResizedImageUrl(userInfoEntity.Photo, Globals.Settings.UploadFolder + "agents/", uxImage.ResizerWidth, uxImage.ResizerHeight, true);
				uxFirstAndLast.Text = userInfoEntity.FirstAndLast;
				if (userInfoEntity.JobTitle != null)
					uxJobTitle.Text = userInfoEntity.JobTitle.Name;
				uxWebsite.Text = uxWebsite.NavigateUrl = userInfoEntity.Website;
				uxWebsite.Visible = !String.IsNullOrEmpty(userInfoEntity.Website);

				uxHomePhone.Visible = !String.IsNullOrEmpty(userInfoEntity.HomePhone);
				uxCellPhone.Visible = !String.IsNullOrEmpty(userInfoEntity.CellPhone);
				uxOfficePhone.Visible = !String.IsNullOrEmpty(userInfoEntity.OfficePhone);
				uxFax.Visible = !String.IsNullOrEmpty(userInfoEntity.Fax);
				if (uxHomePhone.Visible)
					uxHomePhone.Text = "Home: " + userInfoEntity.HomePhone;
				if (uxCellPhone.Visible)
					uxCellPhone.Text = "Mobile: " + userInfoEntity.CellPhone;
				if (uxOfficePhone.Visible)
					uxOfficePhone.Text = "Office: " + userInfoEntity.OfficePhone;
				if (uxFax.Visible)
					uxFax.Text = "Fax: " + userInfoEntity.Fax;

				uxDesignations.DataSource = UserDesignation.UserDesignationGetByUserID(UserID, "Designation.Name", true, new string[] { "Designation" });
				uxDesignations.DataBind();

				uxDesignations.Visible = uxDesignations.Items.Count > 0;

				uxViewListings.Visible = userInfoEntity.ShowListingLink;
				uxViewListings.NavigateUrl = "~/" + GetPreferredMicrosite(userInfoEntity) + "/" + (userInfoEntity.StaffTypeID == (int)StaffTypes.RentalRealtor ? "rentals" : ((microsite)Master).NewHomes ? "new-search" : "search") + "?AgentID=" + UserID;

				uxBioPH.Visible = !String.IsNullOrEmpty(userInfoEntity.Biography);
				uxBiography.Text = userInfoEntity.Biography;

				uxTestimonials.DataSource = UserTestimonial.UserTestimonialGetByUserID(UserID);
				uxTestimonials.DataBind();

				uxTestimonialsPH.Visible = uxTestimonials.Items.Count > 0;

				string micrositePath = userInfoEntity.PreferredCMMicrositeID.HasValue ? Classes.ContentManager.CMMicrosite.GetByID(userInfoEntity.PreferredCMMicrositeID.Value).Name.ToLower().Replace(" ", "-") : MicrositePath;
				CanonicalLink = Helpers.RootPath + micrositePath + "/staff-details?id=" + UserID;

				Dictionary<string, string> unacceptableValues = new Dictionary<string, string>();
				unacceptableValues.Add("Page", "1");
				unacceptableValues.Add("marketID", "");
				unacceptableValues.Add("letter", "");
				unacceptableValues.Add("languageID", "1");
				unacceptableValues.Add("staffTypeID", "8");
				unacceptableValues.Add("searchText", "");

				uxBackToSearchResults.NavigateUrl = "~/" + micrositePath + "/staff" + BaseCode.Helpers.GetQueryStringWithAcceptableValues(unacceptableValues);
			}
			else
				Response.Redirect("staff");
		}
		else
			uxContactMeHeaderPH.Visible = false;
	}

	protected string GetPreferredMicrosite(UserInfo userInfoEntity)
	{
		int micrositeID = userInfoEntity.PreferredCMMicrositeID.HasValue ? userInfoEntity.PreferredCMMicrositeID.Value : 0;
		if (micrositeID == 0)
		{
			UserOffice userOfficeEntity = UserOffice.UserOfficeGetByUserID(userInfoEntity.UserID, includeList: new string[] { "Office" }).FirstOrDefault();
			if (userOfficeEntity != null)
				micrositeID = userOfficeEntity.Office.CMMicrositeID;
		}
		if (micrositeID != 0)
			return micrositeID == 2 ? "augusta" : "aiken";
		return ((microsite)Master).CurrentMicrosite.Name.ToLower().Replace(" ", "-");
	}
}