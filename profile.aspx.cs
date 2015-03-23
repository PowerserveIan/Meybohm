using System;

public partial class profile : BasePage
{
	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
		EnableWhiteSpaceCompression = false;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Members Area";
		ComponentAdminPage = "media352-membership-provider/admin-user.aspx";
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxSave.Click += uxSave_Click;
		uxStaffProfile.Visible = uxUserProfile.HidePhoneNumber = System.Web.Security.Roles.GetRolesForUser().Length > 0;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			int userID = BaseCode.Helpers.GetCurrentUserID();
			uxUserLoginInformation.EntityId = userID;
			uxUserLoginInformation.UserEntity = Classes.Media352_MembershipProvider.User.GetByID(userID);
			uxUserLoginInformation.LoadData();
			uxUserLoginInformation.SetupPasswordDisplayItems();

			uxUserProfile.LoadProfile();
			
			if (uxStaffProfile.Visible)
			{
				uxStaffProfile.EntityId = userID;
				uxStaffProfile.UserInfoEntity = uxUserProfile.UserInfoEntity;
				uxStaffProfile.LoadData();
			}
		}
	}

	void uxSave_Click(object sender, EventArgs e)
	{
		if (IsValid)
		{
			int userID = BaseCode.Helpers.GetCurrentUserID();
			uxUserLoginInformation.EntityId = userID;
			uxUserLoginInformation.UserEntity = Classes.Media352_MembershipProvider.User.GetByID(userID);
			uxUserLoginInformation.SaveData();
			uxUserLoginInformation.SetupPasswordDisplayItems();

			uxUserProfile.SaveProfile();

			if (uxStaffProfile.Visible)
			{
				uxStaffProfile.EntityId = userID;
				uxStaffProfile.UserInfoEntity = uxUserProfile.UserInfoEntity;
				uxStaffProfile.SaveData();
			}

			uxSuccessMessage.Visible = true;
		}
	}
}