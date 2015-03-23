using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Media352_MembershipProvider;

public partial class Controls_BaseControls_ProfileControl : UserControl
{
	private bool m_UseCurrentLoggedInUser = true;
	private bool m_UseProfileStandalone = true;

	public bool HidePhoneNumber { get; set; }

	public bool UseProfileStandalone
	{
		get { return m_UseProfileStandalone; }
		set { m_UseProfileStandalone = value; }
	}

	public bool UseCurrentLoggedInUser
	{
		get { return m_UseCurrentLoggedInUser; }
		set { m_UseCurrentLoggedInUser = value; }
	}

	public string ProfileValidationGroup { get; set; }

	public int UserID
	{
		get
		{
			if (ViewState["ProfileUserID"] == null)
				ViewState["ProfileUserID"] = 0;
			return (int)ViewState["ProfileUserID"];
		}
		set { ViewState["ProfileUserID"] = value; }
	}

	public UserInfo UserInfoEntity { get; set; }

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxSaveButton.Command += uxSaveButton_Command;
		if (!IsPostBack)
		{
			uxPreferredCMMicrositeID.DataSource = Classes.ContentManager.CMMicrosite.CMMicrositePage(0, 0, "", "Name", true, new Classes.ContentManager.CMMicrosite.Filters { FilterCMMicrositeActive = true.ToString(), FilterCMMicrositePublished = true.ToString() });
			uxPreferredCMMicrositeID.DataTextField = "Name";
			uxPreferredCMMicrositeID.DataValueField = "CMMicrositeID";
			uxPreferredCMMicrositeID.DataBind();

			uxPreferredLanguageID.DataSource = Classes.SiteLanguages.Language.GetAll().OrderBy(l => l.Culture);
			uxPreferredLanguageID.DataTextField = "Culture";
			uxPreferredLanguageID.DataValueField = "LanguageID";
			uxPreferredLanguageID.DataBind();
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!String.IsNullOrEmpty(ProfileValidationGroup))
		{
			uxFirstNameRFV.ValidationGroup =
			uxLastNameRFV.ValidationGroup =
			uxAddress.ValidationGroup =
			uxProfileErrorSummary.ValidationGroup =
			uxSaveButton.ValidationGroup = ProfileValidationGroup;
		}

		uxSaveButton.Visible =
		uxProfileErrorSummary.Visible = UseProfileStandalone;
		uxPhonePH.Visible = !HidePhoneNumber;
	}

	private void uxSaveButton_Command(object sender, CommandEventArgs e)
	{
		SaveProfile();
	}

	public void LoadProfile()
	{
		// Get accessor for profile...
		GetUserInfoToUse();

		if (!UserInfoEntity.IsNewRecord)
		{
			uxAddress.AddressID = UserInfoEntity.AddressID;
			uxAddress.Load();
			uxFirstName.Text = UserInfoEntity.FirstName;
			uxLastName.Text = UserInfoEntity.LastName;
			if (!HidePhoneNumber)
				uxPhone.Text = UserInfoEntity.HomePhone;
			if (uxPreferredCMMicrositeID.Items.FindByValue(UserInfoEntity.PreferredCMMicrositeID.ToString()) != null)
				uxPreferredCMMicrositeID.Items.FindByValue(UserInfoEntity.PreferredCMMicrositeID.ToString()).Selected = true;
			if (uxPreferredLanguageID.Items.FindByValue(UserInfoEntity.PreferredLanguageID.ToString()) != null)
				uxPreferredLanguageID.Items.FindByValue(UserInfoEntity.PreferredLanguageID.ToString()).Selected = true;
		}
	}

	public void SaveProfile()
	{
		if (Page.IsValid)
		{
			if (UserInfoEntity == null)
				GetUserInfoToUse();

			if (UserInfoEntity != null)
			{
				uxAddress.Save();
				UserInfoEntity.AddressID = uxAddress.AddressID;
				UserInfoEntity.FirstName = uxFirstName.Text;
				UserInfoEntity.LastName = uxLastName.Text;
				if (!HidePhoneNumber)
					UserInfoEntity.HomePhone = uxPhone.Text;
				UserInfoEntity.PreferredCMMicrositeID = Convert.ToInt32(uxPreferredCMMicrositeID.SelectedValue);
				UserInfoEntity.PreferredLanguageID = Convert.ToInt32(uxPreferredLanguageID.SelectedValue);
				UserInfoEntity.UserID = UserID;
				UserInfoEntity.Save();
			}

			uxSuccessMessage.Visible = UseProfileStandalone;
		}
	}

	public void GetUserInfoToUse()
	{
		if (UseCurrentLoggedInUser)
			UserID = Helpers.GetCurrentUserID();

		if (UserID > 0)
			UserInfoEntity = UserInfo.UserInfoGetByUserID(UserID).FirstOrDefault();
		if (UserInfoEntity == null)
			UserInfoEntity = new UserInfo();
	}
}