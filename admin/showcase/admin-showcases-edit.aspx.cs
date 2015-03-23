using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Media352_MembershipProvider;
using Classes.Showcase;

public partial class Admin_AdminShowcasesEdit : BaseEditPage
{
	public Showcases ShowcasesEntity { get; set; }

	protected List<ShowcaseUser> ShowcaseManagers { get; set; }

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
		m_LinkToListingPage = "admin-showcases.aspx";
		m_ClassName = "Showcase";
		base.OnInit(e);
		uxEditShowcase.Click += uxEditShowcase_Click;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (!ShowcaseHelpers.UserCanManageOtherShowcases())
				Response.Redirect("~/admin/");

			if (EntityId > 0)
			{
				ShowcasesEntity = Showcases.GetByID(EntityId);
				if (ShowcasesEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				if (!ShowcaseHelpers.IsShowcaseAdmin() && !ShowcaseUser.ShowcaseUserGetByShowcaseID(EntityId).Exists(s => s.UserID == Helpers.GetCurrentUserID()))
					Response.Redirect("~/admin/");
				LoadData();
				uxShowcasePlaceHolder.Visible = true;
			}
			else
			{
				if (!ShowcaseHelpers.IsShowcaseAdmin())
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				NewRecord = true;
				uxManagedByPlaceHolder.Visible = uxShowcasePlaceHolder.Visible = false;
			}

			uxAdminOnlyPlaceHolder.Visible = ShowcaseHelpers.IsShowcaseAdmin();
			uxManagedByPlaceHolder.Visible = uxManagedByPlaceHolder.Visible && uxAdminOnlyPlaceHolder.Visible;

			//SEO code
			if (EntityId > 0)
			{
				uxSEOData.PageLinkFormatterElements.Add(Convert.ToString(ShowcasesEntity.ShowcaseID));
				uxSEOData.LoadControlData();
			}
			else
				uxSEOData.LoadControlData(true);
		}
	}

	protected override void Save()
	{
		if (IsValid)
		{
			ShowcasesEntity = EntityId > 0 ? Showcases.GetByID(EntityId) : new Showcases();
			ShowcasesEntity.Active = uxActive.Checked;
			ShowcasesEntity.MLSData = uxMLSData.Checked;
			ShowcasesEntity.Title = uxTitle.Text;
			ShowcasesEntity.Save();
			EntityId = ShowcasesEntity.ShowcaseID;

			//SEO saving should not be done until the new product has been created
			if (ShowcasesEntity.ShowcaseID > 0)
			{
				uxSEOData.PageLinkFormatterElements.Clear();
				uxSEOData.PageLinkFormatterElements.Add(Convert.ToString(ShowcasesEntity.ShowcaseID));
				if (String.IsNullOrEmpty(uxSEOData.Title))
					uxSEOData.Title = uxTitle.Text;
				uxSEOData.SaveControlData();

				if (ShowcasesEntity.ShowcaseID == ShowcaseHelpers.GetDefaultShowcaseID())
				{
					//Save as Default SEO item so that when a user goes to showcase.aspx, they still get SEO
					uxSEOData.PageLinkFormatter = "~/showcase.aspx";
					uxSEOData.PageLinkFormatterElements.Clear();
					uxSEOData.SEODataEntity = Classes.SEOComponent.SEOData.SEODataGetByPageURL(uxSEOData.PageLinkFormatter).FirstOrDefault();
					uxSEOData.SaveControlData();
				}
			}

			if (NewRecord)
			{
				uxManagedByPlaceHolder.Visible = uxShowcasePlaceHolder.Visible = true;
				CopySettings();
			}
			m_ClassTitle = ShowcasesEntity.Title;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = ShowcasesEntity.Active;
		uxMLSData.Checked = ShowcasesEntity.MLSData;
		uxTitle.Text = ShowcasesEntity.Title;
		LoadManagers();		
	}

	void LoadManagers()
	{
		ShowcaseManagers = ShowcaseUser.GetShowcaseUserForAdmin(EntityId);
		uxMicrositeUsers.DataSource = ShowcaseManagers;
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

		ShowcasesEntity = Showcases.GetByID(EntityId);
		ShowcaseManagers = ShowcaseUser.GetShowcaseUserForAdmin(ShowcasesEntity.ShowcaseID);
		allUsers = allUsers.Where(u => !ShowcaseManagers.Exists(m => m.UserID == u.UserID)).ToList();

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
		List<UserRole> allShowcaseUsers = UserRole.UserRoleGetWithUserByRoleName("Showcase Manager");

		if (e.CommandName == "Add")
		{
			if (String.IsNullOrEmpty(Request.Form["uxUserList"].ToString()))
				uxManagerAddReqVal.IsValid = false;
			else
			{
				int userID = Convert.ToInt32(Request.Form["uxUserList"].ToString());
				int showcaseManagerRoleID = Role.RoleGetByName("Showcase Manager").FirstOrDefault().RoleID;

				ShowcaseUser newShowcaseUser = new ShowcaseUser();
				newShowcaseUser.UserID = userID;
				newShowcaseUser.ShowcaseID = EntityId;
				newShowcaseUser.Save();

				//Add user to Showcase Manager role if they aren't already one
				if (!allShowcaseUsers.Exists(r => r.UserID == newShowcaseUser.UserID))
				{
					UserRole newShowcasesAdmin = new UserRole();
					newShowcasesAdmin.UserID = newShowcaseUser.UserID;
					newShowcasesAdmin.RoleID = showcaseManagerRoleID;
					newShowcasesAdmin.Save();
				}
				ShowcaseHelpers.BreakUserCache(userID);
			}
		}
		else if (e.CommandName == "Delete")
		{
			int userID = Convert.ToInt32(e.CommandArgument.ToString());
			List<ShowcaseUser> showcaseUsers = ShowcaseUser.ShowcaseUserGetByShowcaseID(EntityId);

			showcaseUsers.Find(u => u.UserID == userID).Delete();

			//If the user isn't managing any microsites, remove them from Microsite Admin role
			if (ShowcaseUser.ShowcaseUserGetByUserID(userID).Count == 0 && allShowcaseUsers.Exists(u => u.UserID == userID))
				allShowcaseUsers.Find(u => u.UserID == userID).Delete();
			ShowcaseHelpers.BreakUserCache(userID);
		}
		ShowcaseUser.ClearCache();
		LoadManagers();
	}

	private void uxEditShowcase_Click(object sender, EventArgs e)
	{
		Session["ShowcaseID"] = EntityId;
		Response.Redirect("~/admin/showcase/admin-showcase-item.aspx");
	}

	private void CopySettings()
	{
		List<Classes.ConfigurationSettings.SiteSettings> siteSettings = Classes.ConfigurationSettings.SiteSettings.GetByComponentName("Showcase");
		foreach (Classes.ConfigurationSettings.SiteSettings setting in siteSettings)
		{
			ShowcaseSiteSettings showcaseSetting = new ShowcaseSiteSettings();
			showcaseSetting.ShowcaseID = ShowcasesEntity.ShowcaseID;
			showcaseSetting.SiteSettingsID = setting.SiteSettingsID;
			showcaseSetting.Value = setting.Value;
			showcaseSetting.Save();
		}
	}
}