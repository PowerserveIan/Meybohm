using System;
using System.Collections;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Media352_MembershipProvider;

public partial class Admin_AdminUserEdit : BaseEditPage
{
	public User UserEntity { get; set; }

	protected bool? Staff
	{
		get
		{
			bool temp;
			if (!String.IsNullOrEmpty(Request.QueryString["FilterUserHasRole"]) && Boolean.TryParse(Request.QueryString["FilterUserHasRole"], out temp))
				return temp;
			return null;
		}
	}

	protected string RoleName
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["FilterUserRoleName"]))
				return Request.QueryString["FilterUserRoleName"];
			return null;
		}
	}

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-user.aspx";
		m_ClassName = Staff.HasValue && !Staff.Value ? "Customer" : !String.IsNullOrWhiteSpace(RoleName) ? "Agent" : "Staff";
		base.OnInit(e);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			Media352_RoleProvider role = new Media352_RoleProvider();

			//Don't show Microsite Admin role because unchecking it will mess up the user's ability to edit microsites
			//Also don't show Showcase Manager...same reasoning
			uxRoles.DataSource = role.GetAllRoles().Where(r => !r.Equals("Microsite Admin") && !r.Equals("CMS Page Manager") && !r.Equals("Showcase Manager")).OrderBy(r => r);
			uxRoles.DataBind();

			uxCMSRoles.DataSource = Role.RoleGetBySystemRole(false, "Name");
			uxCMSRoles.DataTextField = "Name";
			uxCMSRoles.DataValueField = "RoleID";
			uxCMSRoles.DataBind();

			uxCMSRolesPlaceHolder.Visible = uxCMSRoles.Items.Count > 0;

			if (EntityId > 0)
			{
				UserEntity = Classes.Media352_MembershipProvider.User.GetByID(EntityId);
				if (UserEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
			}
			else
				UserEntity = new User();

			uxUserLoginInformation.EntityId = EntityId;
			uxUserLoginInformation.UserEntity = UserEntity;
			uxUserLoginInformation.SetupPasswordDisplayItems();

			uxUserProfile.Visible = Settings.UserManager == UserManagerType.Complex;
			uxRolesPlaceHolder.Visible = UserEntity.Name != User.Identity.Name;
			uxStaffPH.Visible = uxUserProfile.HidePhoneNumber = Staff.HasValue && Staff.Value;

			if (uxStaffPH.Visible)
			{
				//SEO code
				if (EntityId > 0)
				{
					uxSEOData.PageLinkFormatterElements.Add(Convert.ToString(EntityId));
					uxSEOData.LoadControlData();
				}
				else
					uxSEOData.LoadControlData(true);
			}
		}
	}

	protected override void Save()
	{
		if (IsValid)
		{
			//Use the applications instantiated membership provider
			Media352_MembershipProvider membership = (Media352_MembershipProvider)Membership.Provider;

			MembershipCreateStatus status = MembershipCreateStatus.Success;

			string userNameToUse = Settings.UserNameIsEmail ? uxUserLoginInformation.Email : uxUserLoginInformation.UserName;

			UserEntity = EntityId > 0 ? Classes.Media352_MembershipProvider.User.GetByID(EntityId) : new User();
			if (UserEntity.UserID < 1)
			{
				MembershipUser user = membership.CreateUser(userNameToUse, uxUserLoginInformation.Password, uxUserLoginInformation.Email, uxUserLoginInformation.PasswordQuestion, uxUserLoginInformation.PasswordAnswer, uxIsApproved.Checked, 1, out status);
				if (user != null)
				{
					UserEntity = Classes.Media352_MembershipProvider.User.GetByID((int)user.ProviderUserKey);

					if (Settings.UserManager == UserManagerType.Simple)
					{
						UserRole userrole = new UserRole();
						userrole.RoleID = Role.RoleGetByName("Admin")[0].RoleID;
						userrole.UserID = UserEntity.UserID;
						userrole.Save();
					}
				}
			}
			else
			{
				UserEntity.IsApproved = uxIsApproved.Checked;
				uxUserLoginInformation.UserEntity = UserEntity;
				uxUserLoginInformation.SaveData();
			}
			EntityId = UserEntity.UserID;

			if (status == MembershipCreateStatus.Success)
			{
				ArrayList add = new ArrayList();
				ArrayList remove = new ArrayList();
				foreach (ListItem li in uxRoles.Items)
				{
					if (li.Selected)
						add.Add(li.Text);
					else
						remove.Add(li.Text);
				}

				foreach (ListItem li in uxCMSRoles.Items)
				{
					if (li.Selected)
						add.Add(li.Text);
					else
						remove.Add(li.Text);
				}

				Media352_RoleProvider role = new Media352_RoleProvider();
				if (add.Count > 0)
					role.AddUsersToRoles(new[] { userNameToUse }, (string[])add.ToArray(typeof(string)));
				if (remove.Count > 0)
					role.RemoveUsersFromRoles(new[] { userNameToUse }, (string[])remove.ToArray(typeof(string)));

				//Setup for profile information
				uxUserProfile.UserID = UserEntity.UserID;
				uxUserProfile.UseCurrentLoggedInUser = false;
				uxUserProfile.SaveProfile();

				if (Staff.HasValue && Staff.Value)
				{
					uxStaffProfile.EntityId = EntityId;
					uxStaffProfile.UserInfoEntity = uxUserProfile.UserInfoEntity;
					uxStaffProfile.SaveData();

					uxSEOData.PageLinkFormatterElements.Clear();
					uxSEOData.PageLinkFormatterElements.Add(EntityId.ToString());
					if (String.IsNullOrEmpty(uxSEOData.Title))
						uxSEOData.Title = uxStaffProfile.UserInfoEntity.FirstAndLast;
					uxSEOData.SaveControlData();
				}

				m_ClassTitle = UserEntity.Name;

				m_SuccessMessagePlaceholder.Visible = true;

				if (NewRecord)
				{
					NewRecord = false;
					m_AddNewButton.Visible = true;
				}

				Helpers.PageView.Anchor(Page, Helpers.PageView.PageAnchors.center);
				m_SuccessMessageLiteral.Text = @"The " + (NewRecord ? @"new " : "") + m_ClassName + (!String.IsNullOrEmpty(m_ClassTitle) ? @" """ + m_ClassTitle + @"""" : "") + @" has been successfully <u>" + (NewRecord ? @"added" : @"updated") + @"</u>.";
			}
			else
			{
				if (status == MembershipCreateStatus.DuplicateEmail)
					uxUserErrorCV.ErrorMessage = @"A user with that email address already exists, please enter a new email.";
				else
					uxUserErrorCV.ErrorMessage = status.ToString();
				uxUserErrorCV.IsValid = false;
			}
			uxUserLoginInformation.EntityId = EntityId;
			uxUserLoginInformation.SetupPasswordDisplayItems();
		}
	}

	protected override void SaveButton_Click(object sender, EventArgs e)
	{
		Save();
	}

	protected override void LoadData()
	{
		uxIsApproved.Checked = UserEntity.IsApproved;
		uxUserLoginInformation.UserEntity = UserEntity;
		uxUserLoginInformation.LoadData();
		Media352_RoleProvider role = new Media352_RoleProvider();
		string[] roles = role.GetRolesForUser(UserEntity.Name);
		foreach (string r in roles)
		{
			ListItem li = uxRoles.Items.FindByText(r);
			if (li != null)
				li.Selected = true;
			else
			{
				li = uxCMSRoles.Items.FindByText(r);
				if (li != null)
					li.Selected = true;
			}
		}

		if (Settings.UserManager == UserManagerType.Complex)
		{
			//Load the profile for the user
			uxUserProfile.UserID = UserEntity.UserID;
			uxUserProfile.UseCurrentLoggedInUser = false;
			uxUserProfile.LoadProfile();
			if (Staff.HasValue && Staff.Value)
			{
				uxStaffProfile.EntityId = EntityId;
				uxStaffProfile.UserInfoEntity = uxUserProfile.UserInfoEntity;
				uxStaffProfile.LoadData();
			}
		}
	}
}