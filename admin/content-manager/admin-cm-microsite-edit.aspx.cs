using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.ContentManager;
using Classes.Media352_MembershipProvider;

public partial class Admin_AdminCMMicrositeEdit : BaseEditPage
{
	public CMMicrosite CMMicrositeEntity { get; set; }

	protected List<CMMicrositeUser> MicrositeAdmins { get; set; }

	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	protected override void OnInit(EventArgs e)
	{
		if (!String.IsNullOrEmpty(Request.QueryString["contentType"]))
			GetUserList();
		if (!Classes.ContentManager.Settings.EnableMicrosites)
			Response.Redirect("~/admin/");
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-cm-microsite.aspx";
		m_ClassName = "Microsite";
		base.OnInit(e);
		uxNameUniqueCV.ServerValidate += uxNameUniqueCV_ServerValidate;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		uxImage.UploadToLocation = Globals.Settings.UploadFolder + "images";
		if (!IsPostBack)
		{
			if (EntityId > 0)
			{
				CMMicrositeEntity = CMMicrosite.GetByID(EntityId);
				if (CMMicrositeEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
			}
			else
			{
				NewRecord = true;
				uxManagedByPlaceHolder.Visible = false;
			}
		}
	}

	protected override void Save()
	{
		uxImage.CommitChanges();
		if (IsValid)
		{
			CMMicrositeEntity = EntityId > 0 ? CMMicrosite.GetByID(EntityId) : new CMMicrosite();
			CMMicrositeEntity.Active = uxActive.Checked;
			CMMicrositeEntity.Name = uxName.Text.Trim();
			CMMicrositeEntity.Description = uxDescription.Text;
			CMMicrositeEntity.Location = uxLocation.Text;
			CMMicrositeEntity.Image = uxImage.FileName;
			CMMicrositeEntity.Phone = uxPhone.Text;
			CMMicrositeEntity.Save();
			EntityId = CMMicrositeEntity.CMMicroSiteID;

			if (NewRecord)
			{
				CMMicrosite.PopulateNewMicrosite(CMMicrositeEntity.CMMicroSiteID);
				uxManagedByPlaceHolder.Visible = true;
				//create upload folders
				Directory.CreateDirectory(Server.MapPath("~/" + Globals.Settings.UploadFolder + "images/" + CMMicrositeEntity.CMMicroSiteID));
				Directory.CreateDirectory(Server.MapPath("~/" + Globals.Settings.UploadFolder + "docs/" + CMMicrositeEntity.CMMicroSiteID));
				Directory.CreateDirectory(Server.MapPath("~/" + Globals.Settings.UploadFolder + "videos/" + CMMicrositeEntity.CMMicroSiteID));
				Directory.CreateDirectory(Server.MapPath("~/" + Globals.Settings.UploadFolder + "flash/" + CMMicrositeEntity.CMMicroSiteID));
			}
			m_ClassTitle = CMMicrositeEntity.Name;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = CMMicrositeEntity.Active;
		uxName.Text = CMMicrositeEntity.Name;
		uxDescription.Text = CMMicrositeEntity.Description;
		uxLocation.Text = CMMicrositeEntity.Location;
		uxImage.FileName = CMMicrositeEntity.Image;
		uxPhone.Text = CMMicrositeEntity.Phone;

		MicrositeAdmins = CMMicrositeUser.GetMicrositeAdminsForMicrosite(EntityId);
		uxMicrositeUsers.DataSource = MicrositeAdmins;
		uxMicrositeUsers.DataBind();
	}

	protected void GetUserList()
	{
		int totalCount;
		User.Filters filterList = new User.Filters();
		filterList.FilterUserDeleted = false.ToString();
		filterList.FilterUserIsApproved = true.ToString();
		int startIndex = (Convert.ToInt32(Request.QueryString["p"]) - 1) * Convert.ToInt32(Request.QueryString["s"]);
		List<User> allUsers = Classes.Media352_MembershipProvider.User.UserPageWithTotalCount(startIndex, Convert.ToInt32(Request.QueryString["s"]), Request.QueryString["q"], "Name", true, out totalCount, filterList);

		MicrositeAdmins = CMMicrositeUser.GetMicrositeAdminsForMicrosite(EntityId);
		allUsers = allUsers.Where(u => !MicrositeAdmins.Exists(m => m.UserID == u.UserID)).ToList();

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

	protected void ManagersItem_Command(object sender, CommandEventArgs e)
	{
		List<UserRole> allMicrositeAdmins = UserRole.UserRoleGetWithUserByRoleName("Microsite Admin");
		CMMicrositeEntity = CMMicrosite.GetByID(EntityId);
		if (e.CommandName == "Add")
		{
			if (String.IsNullOrEmpty(Request.Form["uxUserList"].ToString()))
				uxManagerAddReqVal.IsValid = false;
			else
			{
				int userID = Convert.ToInt32(Request.Form["uxUserList"].ToString());
				int micrositeAdminRoleID = Role.RoleGetByName("Microsite Admin").Single().RoleID;

				CMMicrositeUser newMicrositeUser = new CMMicrositeUser();
				newMicrositeUser.UserID = userID;
				newMicrositeUser.CMMicrositeID = CMMicrositeEntity.CMMicroSiteID;
				newMicrositeUser.Save();

				//Add user to Microsite Admin role if they aren't already one
				if (!allMicrositeAdmins.Exists(r => r.UserID == newMicrositeUser.UserID))
				{
					UserRole newMSAdmin = new UserRole();
					newMSAdmin.UserID = newMicrositeUser.UserID;
					newMSAdmin.RoleID = micrositeAdminRoleID;
					newMSAdmin.Save();
				}
			}
		}
		else if (e.CommandName == "Delete")
		{
			int userID = Convert.ToInt32(e.CommandArgument.ToString());
			List<CMMicrositeUser> micrositeUsers = CMMicrositeUser.CMMicrositeUserGetByCMMicrositeID(CMMicrositeEntity.CMMicroSiteID);

			micrositeUsers.Find(u => u.UserID == userID).Delete();

			//If the user isn't managing any microsites, remove them from Microsite Admin role
			if (CMMicrositeUser.CMMicrositeUserGetByUserID(userID).Count == 0 && allMicrositeAdmins.Exists(u => u.UserID == userID))
				allMicrositeAdmins.Find(u => u.UserID == userID).Delete();
		}
		CMMicrositeUser.ClearCache();
		LoadData();
	}

	private void uxNameUniqueCV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = CMMicrosite.CMMicrositeGetByName(uxName.Text).Where(c => c.CMMicroSiteID != EntityId).ToList().Count == 0;
	}
}