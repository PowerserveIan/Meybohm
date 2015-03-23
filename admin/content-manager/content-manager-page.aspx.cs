using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.ContentManager;
using Classes.Media352_MembershipProvider;
using Classes.SEOComponent;
using Classes.SiteLanguages;
using Settings = Classes.ContentManager.Settings;

namespace ContentManager2.Admin
{
	public partial class ContentManagerPage : BaseEditPage
	{
		protected int PageId
		{
			get
			{
				if (ViewState["PageId"] == null)
					return 0;
				return (int)ViewState["PageId"];
			}
			set { ViewState["PageId"] = value; }
		}

		public int TemplateId
		{
			get
			{
				if (ViewState["TemplateId"] == null)
					return 0;
				return (int)ViewState["TemplateId"];
			}
			set { ViewState["TemplateId"] = value; }
		}

		public int MicroSiteId
		{
			get
			{
				if (ViewState["MicroSiteId"] == null)
					return 0;
				return (int)ViewState["MicroSiteId"];
			}
			set { ViewState["MicroSiteId"] = value; }
		}

		public string MicroSiteName
		{
			get { return ViewState["MicroSiteName"] != null ? ViewState["MicroSiteName"].ToString() : string.Empty; }
			set { ViewState["MicroSiteName"] = value; }
		}

		private int LanguageID
		{
			get
			{
				if (ViewState["LanguageID"] == null)
				{
					Language currLanguage = Helpers.GetCurrentLanguage();
					ViewState["LanguageID"] = currLanguage.LanguageID;
				}
				return (int)ViewState["LanguageID"];
			}
		}

		private int DefaultLanguageID
		{
			get
			{
				if (ViewState["DefaultLanguageID"] == null)
				{
					ViewState["DefaultLanguageID"] = Helpers.GetDefaultLanguageID();
				}
				return (int)ViewState["DefaultLanguageID"];
			}
		}

		protected bool CanEditPage
		{
			get { return CMSHelpers.HasFullCMSPermission() || Page.User.IsInRole("CMS Content Integrator"); }
		}

		private bool? LiveContent
		{
			get
			{
				if (ViewState["LiveContent"] != null)
					return Convert.ToBoolean(ViewState["LiveContent"]);
				return null;
			}
			set { ViewState["LiveContent"] = value; }
		}

		protected Data ds { get; set; }

		private bool m_DontRedirectOnNewPage;

		private void ResponsePage_DataBound(object sender, EventArgs e)
		{
			ResponsePage.Items.Insert(0, new ListItem(" -- Same Page --", ""));
			ResponsePage.SelectedValue = ds.Page.ResponsePageID.HasValue ? ds.Page.ResponsePageID.Value.ToString() : "";
		}

		public override void DataBind()
		{
			base.DataBind();
			if (Settings.EnableCMPageRoles)
			{
				foreach (CMPageRole cmPR in CMPageRole.CMPageRoleGetByCMPageID(PageId))
				{
					if (!cmPR.Editor && uxRolesList.Items.FindByValue(cmPR.RoleID.ToString()) != null)
						uxRolesList.Items.FindByValue(cmPR.RoleID.ToString()).Selected = true;
					if (cmPR.Editor && uxRolesEditList.Items.FindByValue(cmPR.RoleID.ToString()) != null)
						uxRolesEditList.Items.FindByValue(cmPR.RoleID.ToString()).Selected = true;

					//Disable View checkboxes whose Edit counterparts are already checked
					if (cmPR.Editor && !CMSHelpers.HasFullCMSPermission() && Settings.EnableApprovals)
						uxRolesList.Items.FindByValue(cmPR.RoleID.ToString()).Enabled = false;
				}
			}
			if (ds.Page.DynamicCollectionID.HasValue && uxDynamicCollection.Items.FindByValue(ds.Page.DynamicCollectionID.ToString()) != null)
				uxDynamicCollection.Items.FindByValue(ds.Page.DynamicCollectionID.ToString()).Selected = true;
		}

		protected override void OnInit(EventArgs e)
		{
			m_Header = uxHeader;
			m_SavePanel = uxPanel;
			m_ButtonContainer = uxButtonContainer;
			m_LinkToListingPage = "content-manager.aspx";
			DataBinding += ContentManagerPage_DataBinding;
			m_ClassName = "Page";
			base.OnInit(e);
			FilenameCustomValidator.ServerValidate += FilenameCustomValidator_ServerValidate;
			uxTitleUniqueValidator.ServerValidate += uxTitleUniqueValidator_ServerValidate;
			uxUnapprovedContent.Click += uxUnapprovedContent_Click;
			uxLiveContent.Click += uxLiveContent_Click;
			ResponsePage.DataBound += ResponsePage_DataBound;
			uxNext.Command += NextPrevious_Command;
			uxPrevious.Command += NextPrevious_Command;
		}

		void NextPrevious_Command(object sender, CommandEventArgs e)
		{
			Response.Redirect("~/admin/content-manager/content-manager-page.aspx?id=" + e.CommandArgument);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				FilenameASPXExtensionREV.Enabled = Globals.Settings.RequireASPXExtensions;
				FilenameNoExtensionREV.Enabled = !Globals.Settings.RequireASPXExtensions;
				int tempPageId;
				if (Int32.TryParse(Request.QueryString["id"], out tempPageId) && tempPageId > 0)
				{
					PageId = tempPageId;
					CMPage currentPage = CMPage.GetByID(PageId);
					if (currentPage == null || !CanEditPage && (!currentPage.CMMicrositeID.HasValue || (!CMMicrositeUser.CMMicrositeUserGetByUserID(Helpers.GetCurrentUserID()).Exists(p => p.CMMicrositeID == currentPage.CMMicrositeID))) && !(Page.User.IsInRole("CMS Page Manager") && CMPageRole.CMPageRoleGetByCMPageID(currentPage.OriginalCMPageID.HasValue ? currentPage.OriginalCMPageID.Value : currentPage.CMPageID).Exists(r => r.Editor && UserRole.UserRoleGetByUserID(Helpers.GetCurrentUserID()).Exists(ur => ur.RoleID == r.RoleID))))
						Response.Redirect("~/admin/content-manager/content-manager.aspx");
					else
					{
						uxNewRecordPageName.Visible = false;
						uxEditPageLit.Visible = true;
					}
				}
				else
				{
					if (!CanEditPage && Page.User.IsInRole("CMS Page Manager") && (MicroSiteId <= 0 || !Page.User.IsInRole("Microsite Admin")))
						Response.Redirect("~/admin/content-manager/content-manager.aspx");
					uxNewRecordPageName.Visible = true;
					uxEditPageLit.Visible = false;
					uxPrevious.Visible = uxNext.Visible = false;
				}
				int tempTemplateId;
				if (Int32.TryParse(Request.QueryString["TemplateId"], out tempTemplateId) && tempTemplateId > 0)
				{
					TemplateId = tempTemplateId;
					if (CMTemplate.GetByID(TemplateId) == null)
						Response.Redirect("~/admin/content-manager/content-manager.aspx");
				}

				if (TemplateId == 0 && PageId == 0)
					Response.Redirect("~/admin/content-manager/content-manager.aspx");

				//Uncomment the following lines if you have the Dynamic Header installed
				uxDynamicCollection.DataSource = Classes.DynamicHeader.DynamicCollection.DynamicCollectionGetByActive(true).OrderBy(c => c.Name);
				uxDynamicCollection.DataTextField = "Name";
				uxDynamicCollection.DataValueField = "DynamicCollectionID";
				DataBind();
				uxDynamicCollectionPH.Visible = uxDynamicCollection.Items.Count > 1;

				if (PageId == 0 && Globals.Settings.RequireASPXExtensions)
					Filename.Text = @".aspx";

				//modify page title source if multiple languages are enabled
				uxLanguageTogglePH.Visible = Settings.EnableMultipleLanguages;
				uxMicrositeNamePH.Visible = Settings.EnableMicrosites;

				if (!CanEditPage && User.IsInRole("Microsite Admin") && !Settings.AllowMicrositeAdminToEditSitemap)
					uxTitle.Enabled = false;
				uxEditPH.Visible = !FrontendView;
			}
			if (IsPostBack)
				LoadData();
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			m_AddNewButton.Visible = !String.IsNullOrEmpty(Request.QueryString["new"]);
			if (!IsPostBack)
			{
				if (!String.IsNullOrEmpty(Request.QueryString["new"]) && Request.QueryString["new"].Equals("1"))
				{
					m_SuccessMessagePlaceholder.Visible = true;
					m_ClassTitle = uxTitle.Text;
					m_SuccessMessageLiteral.Text = @"The " + (NewRecord ? @"new " : "") + m_ClassName + (!String.IsNullOrEmpty(m_ClassTitle) ? @" """ + m_ClassTitle + @"""" : "") + @" has been successfully <u>added</u>.";
				}
			}
		}

		protected override void Save()
		{
			if (IsValid)
			{
				int? nonEditedPageID = ds.Page.OriginalCMPageID;
				bool insert = ds.Page.CMPageID == 0;

				if (!CMSHelpers.HasFullCMSPermission() && !ds.Page.OriginalCMPageID.HasValue && !ds.Page.NeedsApproval && !insert)
					nonEditedPageID = ds.Page.CMPageID;

				bool duplicateInsert = Settings.EnableApprovals && nonEditedPageID.HasValue && !ds.Page.OriginalCMPageID.HasValue && !ds.Page.NeedsApproval;

				if (!Settings.EnableMultipleLanguages || DefaultLanguageID == LanguageID || insert)
					ds.Page.Title = uxTitle.Text;
				ds.Page.FormRecipient = FormRecipient.Text;
				ds.Page.ResponsePageID = null;
				if (!String.IsNullOrEmpty(uxDynamicCollection.SelectedValue))
					ds.Page.DynamicCollectionID = Convert.ToInt32(uxDynamicCollection.SelectedValue);
				else
					ds.Page.DynamicCollectionID = null;
				if (insert)
				{
					ds.Page.CanDelete = true;
					ds.Page.CMTemplateID = TemplateId;
					if (MicroSiteId == -1)
						ds.Page.MicrositeDefault = true;
					else if (MicroSiteId != 0)
						ds.Page.CMMicrositeID = MicroSiteId;
					if (Settings.EnableApprovals)
						ds.Page.NeedsApproval = !CMSHelpers.HasFullCMSPermission();
				}
				if (!String.IsNullOrEmpty(ResponsePage.SelectedValue))
				{
					CMPage cmPage = ds.Pages.Where(p => p.CMPageID.ToString() == ResponsePage.SelectedValue).SingleOrDefault();
					if (cmPage != null)
						ds.Page.ResponsePageID = cmPage.CMPageID;
				}

				if (Filename.Enabled)
					ds.Page.FileName = Filename.Text;
				ds.Page.FeaturedPage = uxFeaturedPage.Checked;
				ds.Page.UserID = Helpers.GetCurrentUserID();
				if (!CMSHelpers.HasFullCMSPermission() && !CMSHelpers.PageHasBeenEditedByUserBefore(ds.Page.EditorUserIDs, ds.Page.UserID))
					ds.Page.EditorUserIDs = (ds.Page.EditorUserIDs + "," + ds.Page.UserID).TrimStart(',');
				if (duplicateInsert)
				{
					//Check to see if an unapproved page exists for a different language
					CMPage unapprovedPage = null;
					if (Settings.EnableMultipleLanguages)
						unapprovedPage = CMPage.CMPageGetByOriginalCMPageID(ds.Page.CMPageID).FirstOrDefault();

					ds.Page.OriginalCMPageID = nonEditedPageID;

					if (unapprovedPage != null)
					{
						ds.Page.CMPageID = unapprovedPage.CMPageID;
						ds.Page.Save();
					}
					else
						ds.Page.Save();
				}
				else
				{
					ds.Page.OriginalCMPageID = nonEditedPageID;
					ds.Page.Save();
				}

				//SEO saving should not be done until the new product has been created
				if (ds.Page.CMPageID > 0)
				{
					//also save CMPageTitle if necessary here
					if (Settings.EnableMultipleLanguages)
					{
						//Title has changed and needs approval and no unapproved title exists
						if (Settings.EnableApprovals && ds.PageTitle.Title != uxTitle.Text && !CMSHelpers.HasFullCMSPermission() && ds.PageTitle.CMPageID != ds.Page.CMPageID)
						{
							ds.PageTitle = new CMPageTitle();
							ds.PageTitle.CMPageID = ds.Page.CMPageID;
						}
						else
							ds.PageTitle.CMPageID = ds.Page.OriginalCMPageID.HasValue ? ds.Page.OriginalCMPageID.Value : ds.Page.CMPageID;
						ds.PageTitle.LanguageID = LanguageID;
						ds.PageTitle.Title = uxTitle.Text;
						ds.PageTitle.Save();
					}
					if (string.IsNullOrEmpty(uxSEOData.Title))
						uxSEOData.Title = uxTitle.Text;
					uxSEOData.PageUrl = (MicroSiteId > 0 && CMMicrosite.GetByID(MicroSiteId) != null ? CMMicrosite.GetByID(MicroSiteId).Name + "/" : "") + Filename.Text;
					uxSEOData.LanguageID = LanguageID;
					//Sets main page alias as product title
					//uxSEOData.MainAlias = Filename.Text;
					uxSEOData.Approved = !ds.Page.OriginalCMPageID.HasValue && !ds.Page.NeedsApproval;
					if (duplicateInsert)
						uxSEOData.SEODataEntity = new SEOData();
					uxSEOData.SaveControlData();
				}

				if (Settings.EnableCMPageRoles)
					UpdatePermissions();
				CMSHelpers.ClearCaches();

				if (ds.Page.NeedsApproval || ds.Page.OriginalCMPageID.HasValue)
				{
					int? languageID = null;
					if (Settings.EnableMultipleLanguages)
						languageID = LanguageID;
					CMSHelpers.SendApprovalEmailAlerts(ds.Page, null, ds.Page.UserID, false, User.IsInRole("Admin") || User.IsInRole("CMS Admin"), null, languageID);
				}
				if (insert || duplicateInsert) //New Page
				{
					if (Settings.MicrositeDefaultChangesAffectExistingMicrosites && ds.Page.MicrositeDefault)
						CMMicrosite.CreatePageForMicrosites(ds.Page.CMPageID, false);
					if (!m_DontRedirectOnNewPage)
						Response.Redirect(String.Format("content-manager-page.aspx?id={0}&new=1{1}{2}#center", ds.Page.CMPageID, MicroSiteId > 0 ? "&MicrositeId=" + MicroSiteId : "", TemplateId > 0 ? "&TemplateId=" + TemplateId : ""));
				}
				else if (Settings.MicrositeDefaultChangesAffectExistingMicrosites && ds.Page.MicrositeDefault)
					CMMicrosite.CreatePageForMicrosites(ds.Page.CMPageID, true);
				uxEditPageLit.Visible = true;
				uxNewRecordPageName.Visible = false;
				m_ClassTitle = uxTitle.Text;
			}
		}

		protected override void SaveAndAddNewButton_Click(object sender, EventArgs e)
		{
			m_DontRedirectOnNewPage = true;
			Save();
			if (Page.IsValid)
				Response.Redirect("~/admin/content-manager/content-manager-page.aspx?id=0&TemplateId=" + TemplateId + (MicroSiteId > 0 ? "&MicrositeId=" + MicroSiteId : ""));
		}

		private void ContentManagerPage_DataBinding(object sender, EventArgs e)
		{
			LoadData();
			if (Settings.EnableCMPageRoles)
			{
				List<Role> nonSystemRoles = Role.RoleGetBySystemRole(false, "Name");
				uxRolesEditPlaceHolder.Visible = Settings.EnableCMPageRoles && CMSHelpers.HasFullCMSPermission() && nonSystemRoles.Count > 0 && MicroSiteId == 0 && (ds.Page == null || !ds.Page.MicrositeDefault);
				uxRolesPlaceHolder.Visible = Settings.EnableCMPageRoles && CanEditPage && nonSystemRoles.Count > 0 && (ds.Page == null || !ds.Page.MicrositeDefault);
				uxRolesList.DataSource = nonSystemRoles;
				uxRolesList.DataTextField = "Name";
				uxRolesList.DataValueField = "RoleID";
				uxRolesEditList.DataSource = nonSystemRoles;
				uxRolesEditList.DataTextField = "Name";
				uxRolesEditList.DataValueField = "RoleID";
			}
		}

		protected override void LoadData()
		{
			//load name of microsite if exists
			List<CMTemplate> cmTemplates = CMTemplate.CMTemplatePage(0, 0, "", "Name", true);

			CMPage cmPage = CMPage.GetByID(PageId);

			int microSiteId = 0;
			if (!string.IsNullOrEmpty(Request.QueryString["MicroSiteId"]))
				int.TryParse(Request.QueryString["MicroSiteId"], out microSiteId);
			else if (cmPage != null && cmPage.CMPageID > 0 && cmPage.CMMicrositeID.HasValue)
				microSiteId = cmPage.CMMicrositeID.Value;
			else if (cmPage != null && cmPage.MicrositeDefault)
				microSiteId = -1;
			MicroSiteId = microSiteId;

			CMMicrosite micrositeEntity = null;
			if (MicroSiteId > 0)
			{
				micrositeEntity = CMMicrosite.GetByID(MicroSiteId);
				if (micrositeEntity == null)
					Response.Redirect("~/admin/content-manager/content-manager.aspx");
				else
					uxMicrositeName.Text = micrositeEntity.Name.Replace("-", " ");
			}

			List<CMPage> cmPages = (from p in CMPage.CMPageGetByDeleted(false) orderby p.FileName select p).ToList();

			if (ds == null)
			{
				ds = new Data
						{
							Page = cmPage ?? new CMPage { UserID = Helpers.GetCurrentUserID(), CMTemplateID = TemplateId, Created = DateTime.UtcNow, Deleted = false },
							Templates = cmTemplates,
							Pages = cmPages
						};
				if (Settings.EnableMultipleLanguages)
				{
					if (cmPage != null)
					{
						ds.PageTitle = CMPageTitle.CMPageTitleGetByCMPageIDAndLanguageID(cmPage.CMPageID, LanguageID).FirstOrDefault();
						if (ds.PageTitle == null && cmPage.OriginalCMPageID.HasValue)
							ds.PageTitle = CMPageTitle.CMPageTitleGetByCMPageIDAndLanguageID(cmPage.OriginalCMPageID.Value, LanguageID).FirstOrDefault();
					}
					if (ds.PageTitle == null)
						ds.PageTitle = new CMPageTitle();
				}
				if (ds.Templates.Exists(t => t.CMTemplateID == TemplateId && t.Addable == false))
					Response.Redirect("~/admin/content-manager/content-manager.aspx");
			}

			uxTemplatePlaceHolder.Visible = cmTemplates.Count > 1;

			bool isLiveContent = !LiveContent.HasValue || (LiveContent.HasValue && LiveContent.Value) || (!ds.Page.NeedsApproval && CMPage.CMPageGetByOriginalCMPageID(ds.Page.CMPageID).Count > 0);

			int? languageID = null;
			if (Settings.EnableMultipleLanguages)
			{
				languageID = LanguageID;
				uxHideForNonDefaultLanguage.Visible = uxHideForNonDefaultLanguage2.Visible = DefaultLanguageID == LanguageID;
			}
			uxUnapprovedPlaceHolder.Visible = CMPage.PageNeedsApproval(ds.Page.CMPageID, languageID);
			if (uxUnapprovedPlaceHolder.Visible && isLiveContent) //Verify that the unapproved link can go somewhere
			{
				CMPage unapprovedPage = CMPage.CMPageGetByOriginalCMPageID(ds.Page.CMPageID).FirstOrDefault();
				uxUnapprovedPlaceHolder.Visible = unapprovedPage != null && CMPage.PageNeedsApproval(unapprovedPage.CMPageID, languageID);
			}
			if (!uxUnapprovedPlaceHolder.Visible && !isLiveContent && ds.Page.OriginalCMPageID.HasValue)
				Response.Redirect(Request.AppRelativeCurrentExecutionFilePath + "?" + Request.QueryString.ToString().Replace("id=" + Request.QueryString["id"], "id=" + ds.Page.OriginalCMPageID.Value));
			uxMessage.Text = isLiveContent ? "You are viewing the live page." : "You are viewing the unapproved page.";
			uxFlaggedForDeletion.Visible = ds.Page.EditorDeleted.HasValue;
			uxShowApprovalDetails.Visible = !isLiveContent && !ds.Page.NeedsApproval;
			uxLiveContent.Visible = ds.Page.OriginalCMPageID.HasValue;
			uxUnapprovedContent.Visible = isLiveContent;
			m_SaveButton.Visible = m_SaveAndAddNewButton.Visible = !uxUnapprovedPlaceHolder.Visible || !isLiveContent;
			uxSaveDisabled.Visible = !m_SaveButton.Visible;

			if (ds.Page.EditorDeleted.HasValue)
			{
				uxFlaggedForDeletion.Text = @"<br />" + (ds.Page.EditorDeleted.Value ? "An Editor has requested that this page be deleted." : "An Editor has requested that this page be restored.");
				uxFlaggedForDeletion.ForeColor = ds.Page.EditorDeleted.Value ? Color.Red : Color.Green;
			}

			uxLastEditedDate.Text = ds.Page.CreatedClientTime.ToString("MMMM d, yyyy h:mm tt");
			if (!String.IsNullOrEmpty(ds.Page.EditorUserIDs))
			{
				foreach (string s in ds.Page.EditorUserIDs.Split(','))
				{
					User userInfo = Classes.Media352_MembershipProvider.User.GetByID(Convert.ToInt32(s));
					if (userInfo != null)
						uxAllEditors.Text += @"<a href=""mailto:" + userInfo.Email + @""">" + userInfo.Email + @"</a>, ";
				}
			}

			uxAllEditors.Text = uxAllEditors.Text.Trim().TrimEnd(',');

			if (!IsPostBack)
			{
				//SEO code
				if (ds.Page.CMPageID > 0)
				{
					uxSEOData.PageUrl = (micrositeEntity == null ? "" : micrositeEntity.Name + "/") + ds.Page.FileName;
					if (Settings.EnableMultipleLanguages)
						uxSEOData.LanguageID = LanguageID;
					uxSEOData.Approved = isLiveContent;
					uxSEOData.LoadControlData();
				}
				else
				{
					uxSEOData.Approved = isLiveContent;
					uxSEOData.LoadControlData(true);
				}

				if (!ds.Page.IsNewRecord)
				{
					//Bind Next/Previous Buttons
					List<CMPage> allPagesInTemplate = CMPage.CMPageGetByCMTemplateID(ds.Page.CMTemplateID, "Title");
					int currentIndex = allPagesInTemplate.IndexOf(allPagesInTemplate.Find(c => c.CMPageID == ds.Page.CMPageID));
					if (currentIndex != 0)
						uxPrevious.CommandArgument = allPagesInTemplate[currentIndex - 1].CMPageID.ToString();
					else
						uxPrevious.Visible = false;
					if (currentIndex < allPagesInTemplate.Count - 1)
						uxNext.CommandArgument = allPagesInTemplate[currentIndex + 1].CMPageID.ToString();
					else
						uxNext.Visible = false;
				}
			}

			if (ds.Page.CMTemplateID.HasValue)
				TemplateId = ds.Page.CMTemplateID.Value;
			if (FrontendView)
				uxPrevious.Visible = uxNext.Visible = m_SaveAndAddNewButton.Visible = false;
		}

		private void UpdatePermissions(bool approval = false) //updates the page permissions
		{
			List<CMPageRole> existingPageRoles = CMPageRole.CMPageRoleGetByCMPageID(approval && ds.Page.OriginalCMPageID.HasValue ? ds.Page.OriginalCMPageID.Value : ds.Page.CMPageID);
			foreach (ListItem i in uxRolesList.Items)
			{
				CMPageRole existing = existingPageRoles.Where(r => r.RoleID == Convert.ToInt32(i.Value) && !r.Editor).SingleOrDefault();
				if (existing == null && i.Selected)
					new CMPageRole { CMPageID = (approval && ds.Page.OriginalCMPageID.HasValue ? ds.Page.OriginalCMPageID.Value : ds.Page.CMPageID), RoleID = Convert.ToInt32(i.Value) }.Save();
				else if (existing != null && !i.Selected)
					existing.Delete();
			}

			foreach (ListItem i in uxRolesEditList.Items)
			{
				CMPageRole existing = existingPageRoles.Where(r => r.RoleID == Convert.ToInt32(i.Value) && r.Editor).SingleOrDefault();
				if (existing == null && i.Selected)
					new CMPageRole { CMPageID = (approval && ds.Page.OriginalCMPageID.HasValue ? ds.Page.OriginalCMPageID.Value : ds.Page.CMPageID), RoleID = Convert.ToInt32(i.Value), Editor = true }.Save();
				else if (existing != null && !i.Selected)
					existing.Delete();
			}
		}

		protected override string GetAddUrl()
		{
			return "~/admin/content-manager/content-manager-page.aspx?id=0&TemplateId=" + TemplateId + (MicroSiteId > 0 ? "&MicrositeId=" + MicroSiteId : "");
		}

		public string GetPath(string filename)
		{
			Regex reg = new Regex(@"^(\w+/){0,}", RegexOptions.IgnoreCase);
			return (reg.Match(filename)).Value;
		}

		#region Approvals

		private void uxLiveContent_Click(object sender, EventArgs e)
		{
			if (ds.Page.OriginalCMPageID.HasValue)
				Response.Redirect("content-manager-page.aspx?id=" + ds.Page.OriginalCMPageID.Value);
			else
			{
				LiveContent = true;
				LoadData();
			}
		}

		private void uxUnapprovedContent_Click(object sender, EventArgs e)
		{
			CMPage unapprovedPage = CMPage.CMPageGetByOriginalCMPageID(ds.Page.CMPageID).FirstOrDefault();
			if (unapprovedPage != null)
				Response.Redirect("~/admin/content-manager/content-manager-page.aspx?id=" + unapprovedPage.CMPageID);
			else
			{
				LiveContent = false;
				LoadData();
			}
		}

		#endregion

		#region Validators

		private void FilenameCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
		{
			//check the filename doesnt exist
			//iff its a new page
			//also check the db for the filename, incase its a deleted file

			if (ds != null)
				if (ds.Page.FileName != null)
					if (Filename.Text.Equals(ds.Page.FileName, StringComparison.OrdinalIgnoreCase))
					{
						args.IsValid = true;
						return;
					}

			if (File.Exists("~/" + Filename.Text))
			{
				args.IsValid = false;
				return;
			}

			args.IsValid = !CMPage.CMPageGetByFileName(Filename.Text).Exists(p => p.CMPageID != EntityId && (MicroSiteId > 0 ? p.CMMicrositeID == MicroSiteId : MicroSiteId == 0 ? p.CMMicrositeID == null : p.MicrositeDefault));
		}

		private void uxTitleUniqueValidator_ServerValidate(object source, ServerValidateEventArgs args)
		{
			if (Settings.EnableMultipleLanguages)
				args.IsValid = !CMPageTitle.CMPageTitleGetByTitle(uxTitle.Text).Any(t => EntityId != t.CMPageID && (ds.Page != null ? (!ds.Page.OriginalCMPageID.HasValue || ds.Page.OriginalCMPageID.Value != t.CMPageID) : true) && CMPage.GetByID(t.CMPageID).OriginalCMPageID != EntityId && (MicroSiteId > 0 ? CMPage.GetByID(t.CMPageID).CMMicrositeID == MicroSiteId : MicroSiteId == 0 ? CMPage.GetByID(t.CMPageID).CMMicrositeID == null : CMPage.GetByID(t.CMPageID).MicrositeDefault));
			else
				args.IsValid = !CMPage.CMPageGetByTitle(uxTitle.Text).Any(t => EntityId != t.CMPageID && (ds.Page != null ? (!ds.Page.OriginalCMPageID.HasValue || ds.Page.OriginalCMPageID.Value != t.CMPageID) : true) && (MicroSiteId > 0 ? t.CMMicrositeID == MicroSiteId : MicroSiteId == 0 ? t.CMMicrositeID == null : t.MicrositeDefault));
		}

		#endregion

		#region Nested type: Data

		[Serializable]
		protected class Data
		{
			public CMPage Page { get; set; }
			public List<CMTemplate> Templates { get; set; }
			public List<CMPage> Pages { get; set; }
			public CMPageTitle PageTitle { get; set; }
		}

		#endregion
	}
}
