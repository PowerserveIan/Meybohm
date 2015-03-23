using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Classes.Media352_MembershipProvider;

public partial class Admin_AdminCMRoleEdit : BaseEditPage
{
	public Role RoleEntity { get; set; }

	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	protected override void OnInit(EventArgs e)
	{
		if (!String.IsNullOrEmpty(Request.QueryString["contentType"]))
			GetUserList();
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-cm-role.aspx";
		m_ClassName = "CM Role";
		base.OnInit(e);
		uxNameUniqueCV.ServerValidate += uxNameUniqueCV_ServerValidate;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (EntityId > 0)
			{
				RoleEntity = Role.GetByID(EntityId);
				if (RoleEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
			}
			else
			{
				NewRecord = true;
				uxUsersInRolePlaceHolder.Visible = false;
			}
		}
	}

	protected override void Save()
	{
		if (IsValid)
		{
			RoleEntity = EntityId > 0 ? Role.GetByID(EntityId) : new Role();
			RoleEntity.Name = uxName.Text;
			RoleEntity.SystemRole = false;
			RoleEntity.Save();
			EntityId = RoleEntity.RoleID;

			if (NewRecord)
				uxUsersInRolePlaceHolder.Visible = true;
			m_ClassTitle = RoleEntity.Name;
		}
	}

	protected override void LoadData()
	{
		uxName.Text = RoleEntity.Name;
		uxUsersInRole.DataSource = UserRole.UserRoleGetWithUserByRoleName(RoleEntity.Name);
		uxUsersInRole.DataBind();
	}

	protected void GetUserList()
	{
		int totalCount;
		User.Filters filterList = new User.Filters();
		filterList.FilterUserDeleted = false.ToString();
		filterList.FilterUserIsApproved = true.ToString();
		int startIndex = (Convert.ToInt32(Request.QueryString["p"]) - 1) * Convert.ToInt32(Request.QueryString["s"]);
		List<User> allUsers = Classes.Media352_MembershipProvider.User.UserPageWithTotalCount(startIndex, Convert.ToInt32(Request.QueryString["s"]), Request.QueryString["q"], "Name", true, out totalCount, filterList);

		RoleEntity = Role.GetByID(EntityId);
		List<UserRole> usersInRole = UserRole.UserRoleGetWithUserByRoleName(RoleEntity.Name);
		allUsers = allUsers.Where(u => !usersInRole.Exists(m => m.UserID == u.UserID)).ToList();

		string json = "{\"results\":[";
		foreach (Classes.Media352_MembershipProvider.User u in allUsers)
		{
			json += "{\"id\":" + u.UserID + ",\"name\":\"" + u.Name + "\",\"email\":\"" + u.Email + "\"},";
		}
		json = json.TrimEnd(',');
		json += "],\"total\":\"" + totalCount + "\"}";
		Response.ContentType = "application/json";
		Response.Write(json);
		Response.End();
	}

	protected void UserRoleItem_Command(object sender, CommandEventArgs e)
	{
		RoleEntity = EntityId > 0 ? Role.GetByID(EntityId) : new Role();
		if (e.CommandName == "Add")
		{
			int userID;
			if (String.IsNullOrEmpty(Request.Form["uxUserList"].ToString()) || !Int32.TryParse(Request.Form["uxUserList"].ToString(), out userID))
				uxAddUserToRoleReqVal.IsValid = false;
			else
			{				
				UserRole newUserRole = new UserRole();
				newUserRole.UserID = userID;
				newUserRole.RoleID = RoleEntity.RoleID;
				newUserRole.Save();

				if (!UserRole.UserRoleGetWithUserByRoleName("CMS Page Manager").Exists(u => u.UserID == userID))
				{
					int pageManagerRoleID = Role.RoleGetByName("CMS Page Manager").Single().RoleID;
					UserRole managerRole = new UserRole();
					managerRole.UserID = userID;
					managerRole.RoleID = pageManagerRoleID;
					managerRole.Save();
				}
			}
		}
		else if (e.CommandName == "Delete")
		{
			int userID = Convert.ToInt32(e.CommandArgument.ToString());
			UserRole.UserRoleGetWithUserByRoleName(RoleEntity.Name).ForEach(u => { if (u.UserID == userID) u.Delete(); });

			if (!UserRole.UserRoleGetBySystemRole(false).Any(u => u.UserID == userID))
			{
				int pageManagerRoleID = Role.RoleGetByName("CMS Page Manager").Single().RoleID;
				UserRole cmsPageManagerRole = UserRole.UserRoleGetByUserID(userID).Find(u => u.RoleID == pageManagerRoleID);
				if (cmsPageManagerRole != null)
					cmsPageManagerRole.Delete();
			}
		}
		UserRole.ClearCache();
		LoadData();
	}

	private void uxNameUniqueCV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = Role.RoleGetByName(uxName.Text).Where(c => c.RoleID != EntityId).ToList().Count == 0;
	}
}