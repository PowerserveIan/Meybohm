using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.ContentManager;
using Classes.Media352_MembershipProvider;
using Classes.SiteLanguages;
using Settings = Classes.ContentManager.Settings;

public partial class Admin_ContentManager_sitemap : Page
{
	public bool ShowAll { get { return (Request.QueryString["showall"] != null); } }
	protected int DepthLimit = Settings.DepthLimit - 1;
	protected bool ForceOneToOne = Settings.ForceOneToOne;

	protected bool IsDeveloper
	{
		get { return User.IsInRole("Admin") || User.IsInRole("CMS Admin"); }
	}

	protected bool CanEdit
	{
		get { return IsDeveloper || User.IsInRole("CMS Content Integrator"); }
	}

	protected int MicroSiteID
	{
		get { return Session["MicroSiteID"] != null ? Convert.ToInt32(Session["MicroSiteID"]) : 0; }
		set { Session["MicroSiteID"] = value; }
	}

	protected int LanguageID
	{
		get
		{
			if (ViewState["LanguageID"] == null)
			{
				if (Settings.MultilingualManageSiteMapsIndividually)
				{
					Language currLanguage = Helpers.GetCurrentLanguage();
					ViewState["LanguageID"] = currLanguage.LanguageID;
				}
				else
					ViewState["LanguageID"] = Helpers.GetDefaultLanguageID();
			}
			return (int)ViewState["LanguageID"];
		}
		set { ViewState["LanguageID"] = value; }
	}

	protected bool LiveContent
	{
		get
		{
			if (ViewState["LiveContent"] == null)
				ViewState["LiveContent"] = true;
			return Convert.ToBoolean(ViewState["LiveContent"]);
		}
		set { ViewState["LiveContent"] = value; }
	}

	protected override void OnPreInit(EventArgs e)
	{
		if (Request.Form["languageID"] != null)
			LanguageID = Convert.ToInt32(Request.Form["languageID"]);
		if (Request.Form["operation"] == "hide_smitem")
			ShowSMItemInMenu(false);
		else if (Request.Form["operation"] == "display_smitem")
			ShowSMItemInMenu(true);
		else if (Request.Form["operation"] == "remove_page" || Request.Form["operation"] == "restore_page")
			DeleteOrRestorePageNode();
		else if (Request.Form["operation"] == "remove_smitem")
			DeleteSMItem();
		else if (Request.Form["operation"] == "rename_page")
			RenameNode();
		else if (Request.Form["operation"] == "move_node")
			MoveNode();
		else if (Request.Form["operation"] == "restore_smitem")
			RestoreItem();
		base.OnPreInit(e);
		string masterPageScript = ((Literal)Master.FindControl("uxJavaScripts")).Text;
		((Literal)Master.FindControl("uxJavaScripts")).Text = masterPageScript + (String.IsNullOrEmpty(masterPageScript) ? "" : ",") + uxJavaScripts.Text;
		uxJavaScripts.Visible = false;
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		int tempID;
		if (!String.IsNullOrEmpty(Request.QueryString["MicrositeID"]) && Int32.TryParse(Request.QueryString["MicrositeID"], out tempID))
		{
			MicroSiteID = tempID;
			Response.Redirect("~/admin/content-manager/sitemap.aspx");
		}
		uxMicrositeList.SelectedIndexChanged += uxMicrositeList_SelectedIndexChanged;
		uxNewHomes.SelectedIndexChanged += uxNewHomes_SelectedIndexChanged;
		uxAddLink.Click += uxAddLink_Click;
		uxUnapprovedContent.Click += uxUnapprovedContent_Click;
		uxLiveContent.Click += uxLiveContent_Click;

		uxOneToOne.Visible = ForceOneToOne;
		uxDepthLimit.Text = (DepthLimit + 1).ToString("d");
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			if (!CanEdit && !Settings.AllowMicrositeAdminToEditSitemap)
				Response.Redirect("~/admin");

			if (Settings.EnableMicrosites)
			{
				uxMicrositePlaceHolder.Visible = true;
				uxMicrositeList.Visible = false;
				if (CanEdit || User.IsInRole("Microsite Admin"))
				{
					List<CMMicrosite> tempMicrositeList = CanEdit ? CMMicrosite.CMMicrositeGetByActive(true) : CMMicrosite.GetMicrositesByUserID(Helpers.GetCurrentUserID()).Where(c => c.Active).ToList();

					if (CanEdit)
					{
						uxMicrositeList.Items.Add(new ListItem("Main Site"));
						uxMicrositeList.Items.Add(new ListItem("Microsite Default"));
						uxMicrositeList.AppendDataBoundItems = true;
						uxMicrositeList.Visible = true;
					}

					if (tempMicrositeList.Count > 1)
					{
						uxMicrositeList.Visible = true;

						uxMicrositeList.DataSource = tempMicrositeList;
						uxMicrositeList.DataTextField = "Name";
						uxMicrositeList.DataValueField = "CMMicroSiteID";
						if (!CanEdit && MicroSiteID == 0)
							MicroSiteID = tempMicrositeList[0].CMMicroSiteID;
					}
					else if (tempMicrositeList.Count == 1)
					{
						if (CanEdit)
						{
							uxMicrositeList.Visible = true;

							uxMicrositeList.DataSource = tempMicrositeList;
							uxMicrositeList.DataTextField = "Name";
							uxMicrositeList.DataValueField = "CMMicroSiteID";
						}
						else
						{
							uxMicrositeList.Visible = false;
							uxMicroSiteName.Text = tempMicrositeList[0].Name;
							MicroSiteID = tempMicrositeList[0].CMMicroSiteID;
						}
					}
					else if (tempMicrositeList.Count == 0 && !IsDeveloper)
					{
						uxMicrositeInactive.Visible = true;
						uxHideThisIfMSAdmin.Visible = false;
						return;
					}
				}
				if (MicroSiteID > 0 && (CMMicrosite.GetByID(MicroSiteID) == null || !CMMicrosite.GetByID(MicroSiteID).Active))
					MicroSiteID = 0;
				if (uxMicrositeList.Visible)
				{
					uxMicrositeList.DataBind();

					if (MicroSiteID > 0)
						uxMicrositeList.SelectedValue = MicroSiteID.ToString();
					else if (MicroSiteID == -1)
						uxMicrositeList.SelectedValue = uxMicrositeList.Items.FindByText("Microsite Default").Value;
					uxMicroSiteName.Text = uxMicrositeList.SelectedItem.Text;
					uxNewHomes.Visible = MicroSiteID != 0;
				}
			}
			else if (!CanEdit && User.IsInRole("Microsite Admin"))
				Response.Redirect("~/admin");
			uxLanguageToggle.Visible = Settings.EnableMultipleLanguages && Settings.MultilingualManageSiteMapsIndividually;
			uxMicrositeLanguagePH.Visible = uxLanguageToggle.Visible || uxMicrositePlaceHolder.Visible;
			Populate();
		}
	}

	private void uxAddLink_Click(object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			//Check to see if the node is in the sitemap tree but not set to show in menu
			List<CMPage> listCMPages;
			if (Settings.EnableMultipleLanguages)
				listCMPages = CMPage.CMPageGetByFileNameAndLanguageID(uxLinkToPage.Text, LanguageID).Where(p => (MicroSiteID <= 0 ? p.CMMicrositeID == null : p.CMMicrositeID == MicroSiteID)).ToList();
			else
				listCMPages = CMPage.CMPageGetByFileName(uxLinkToPage.Text).Where(p => (MicroSiteID <= 0 ? p.CMMicrositeID == null : p.CMMicrositeID == MicroSiteID)).ToList();

			if (listCMPages.Count > 0)
			{
				List<SMItem> listSMItems;
				if (Settings.EnableMultipleLanguages)
					listSMItems = SMItem.SMItemGetByCMPageID(listCMPages[0].CMPageID).Where(smi => smi.LanguageID.HasValue && smi.LanguageID.Value == LanguageID).ToList();
				else
					listSMItems = SMItem.SMItemGetByCMPageID(listCMPages[0].CMPageID);

				if (listSMItems.Count > 0)
				{
					if (!listCMPages[0].Title.Equals(uxPageTitle.Text, StringComparison.OrdinalIgnoreCase) || (Settings.EnableMultipleLanguages && !listCMPages[0].CMPageTitleTitle.Equals(uxPageTitle.Text, StringComparison.OrdinalIgnoreCase)))
					{
						listCMPages[0].Title = uxPageTitle.Text;
						listCMPages[0].UserID = Helpers.GetCurrentUserID();
						if (Settings.EnableApprovals && !CMSHelpers.HasFullCMSPermission())
						{
							listCMPages[0].EditorUserIDs = listCMPages[0].UserID.ToString();
							listCMPages[0].NeedsApproval = true;
							CMSHelpers.SendApprovalEmailAlerts(listCMPages[0], null, listCMPages[0].UserID, false, false);
						}
						listCMPages[0].Save();
						listSMItems[0].Name = uxPageTitle.Text;
					}

					listSMItems[0].ShowInMenu = true;
					if (Settings.EnableApprovals && (!CMSHelpers.HasFullCMSPermission() || !LiveContent) && !listSMItems[0].NeedsApproval && !listSMItems[0].OriginalSMItemID.HasValue)
					{
						listSMItems[0].OriginalSMItemID = listSMItems[0].SMItemID;
						listSMItems[0].Save();
					}
					else
						listSMItems[0].Save();
				}
				TrackEditors();
			}
			else
			{
				CMPage newPage = new CMPage();
				newPage.CanDelete = false;
				newPage.FileName = uxLinkToPage.Text;
				newPage.Title = uxPageTitle.Text;
				newPage.UserID = Helpers.GetCurrentUserID();
				newPage.Created = DateTime.UtcNow;
				if (MicroSiteID > 0)
					newPage.CMMicrositeID = MicroSiteID;
				else if (MicroSiteID == -1)
					newPage.MicrositeDefault = true;
				if (Settings.EnableApprovals && !CMSHelpers.HasFullCMSPermission())
				{
					newPage.NeedsApproval = true;
					CMSHelpers.SendApprovalEmailAlerts(newPage, null, newPage.UserID, false, false);
				}
				newPage.Save();
			}
			uxLinkToPage.Text = string.Empty;
			uxPageTitle.Text = string.Empty;
			Populate();
		}
	}

	private void uxMicrositeList_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (uxMicrositeList.SelectedItem.Text == @"Main Site")
			MicroSiteID = 0;
		else if (uxMicrositeList.SelectedItem.Text == @"Microsite Default")
			MicroSiteID = -1;
		else
			MicroSiteID = Convert.ToInt32(uxMicrositeList.SelectedValue);
		uxMicroSiteName.Text = uxMicrositeList.SelectedItem.Text;
		if (Settings.EnableApprovals)
			LiveContent = false;
		uxNewHomes.Visible = MicroSiteID != 0;
		Populate();
	}

	void uxNewHomes_SelectedIndexChanged(object sender, EventArgs e)
	{
		Populate();
	}

	private void Populate()
	{
		if (Settings.EnableMicrosites && MicroSiteID != 0)
		{
			PopulateSiteMapTree(MicroSiteID);
			PopulateSourceTree(MicroSiteID);
		}
		else
		{
			PopulateSiteMapTree(null);
			PopulateSourceTree(null);
		}
	}

	private void PopulateSiteMapTree(int? cmMicrositeID)
	{
		StringBuilder siteMapText = new StringBuilder();
		siteMapText.AppendLine("<ul>");
		siteMapText.AppendLine("<li rel='folder' id='root'><a href='#' class=" + (ShowAll ? "showall" : "") + ">" + (cmMicrositeID == null ? "Site Map Root" : "Microsite Root") + "</a>");
		List<SMItem> allSiteMaps = CMSHelpers.GetCachedSMItems(cmMicrositeID, Settings.EnableMultipleLanguages ? (int?)LanguageID : null).Where(s => !cmMicrositeID.HasValue || s.NewHomes.Equals(Convert.ToBoolean(uxNewHomes.SelectedValue))).ToList();

		if (!IsPostBack)
			LiveContent = allSiteMaps.Count(s => s.NeedsApproval || s.OriginalSMItemID.HasValue || s.EditorDeleted) == 0;

		if (!LiveContent && allSiteMaps.Count(s => s.NeedsApproval || s.OriginalSMItemID.HasValue || s.EditorDeleted) == 0)
			LiveContent = true;

		uxUnapprovedPlaceHolder.Visible = !LiveContent || allSiteMaps.Count(s => s.NeedsApproval || s.OriginalSMItemID.HasValue || s.EditorDeleted) != 0;
		uxMessage.Text = LiveContent ? "You are viewing live content." : "You are viewing unapproved content.";
		uxLiveContent.Visible = uxShowApprovalDetails.Visible = !LiveContent;
		uxUnapprovedContent.Visible = LiveContent;
		uxModifySiteMapPlaceholder.Visible = uxPagesPH.Visible =
			//SiteMap.Enabled = 
			(uxUnapprovedPlaceHolder.Visible && !LiveContent) || (!uxUnapprovedPlaceHolder.Visible && LiveContent);
		uxAdminMessage.Visible = CMSHelpers.HasFullCMSPermission() && !LiveContent;

		List<SMItemUser> allEditors;
		if (MicroSiteID > 0)
			allEditors = SMItemUser.SMItemUserGetByCMMicrositeID(MicroSiteID);
		else if (MicroSiteID == -1)
			allEditors = SMItemUser.SMItemUserGetByMicrositeDefault(true);
		else
			allEditors = SMItemUser.GetAll().Where(s => s.CMMicrositeID == null && !s.MicrositeDefault).ToList();

		if (Settings.EnableMultipleLanguages)
			allEditors = allEditors.Where(s => s.LanguageID == LanguageID).ToList();

		foreach (SMItemUser id in allEditors)
		{
			User userInfo = Classes.Media352_MembershipProvider.User.GetByID(Convert.ToInt32(id.UserID));
			if (userInfo != null)
				uxAllEditors.Text += @"<a href=""mailto:" + userInfo.Email + @""">" + userInfo.Email + @"</a>, ";
		}
		uxAllEditors.Text = uxAllEditors.Text.Trim().TrimEnd(',');
		if (LiveContent)
			allSiteMaps = allSiteMaps.Where(s => !s.NeedsApproval && !s.OriginalSMItemID.HasValue).ToList();
		else
		{
			//Remove original items in favor of their edited items
			List<SMItem> tempItems = new List<SMItem>();
			tempItems.AddRange(allSiteMaps);
			foreach (SMItem itemEntity in tempItems)
			{
				if (itemEntity.OriginalSMItemID.HasValue)
					allSiteMaps.RemoveAll(p => p.SMItemID == itemEntity.OriginalSMItemID.Value);
			}
		}

		AddSiteMapLayer(allSiteMaps, siteMapText, null);
		siteMapText.AppendLine("</li></ul>");
		uxSitemapList.Text = siteMapText.ToString();
	}

	private void AddSiteMapLayer(List<SMItem> allSiteMaps, StringBuilder siteMapText, int? parentID, bool parentShowInMenu = true)
	{
		List<CMPage> allCMPages = CMSHelpers.GetCachedCMPages();
		siteMapText.AppendLine("<ul>");
		foreach (SMItem sm in (from s in allSiteMaps
							   orderby s.Rank
							   where s.SMItemParentID == parentID && (s.ShowInMenu || ShowAll)
							   select s).ToArray())
		{
			string cssClass = string.Empty;
			string title = allCMPages.Find(c => c.CMPageID == sm.CMPageID).FileName;
			string rel = "page";
			if (!LiveContent)
			{
				if (sm.EditorDeleted)
				{
					cssClass = rel = "deleted";
					title += " - Deleted by Editor";
				}
				else if (sm.NeedsApproval || sm.OriginalSMItemID.HasValue)
				{
					cssClass = rel = "changed";
					title += " - Unapproved Change";
				}
			}

			if (!sm.ShowInMenu || !parentShowInMenu) cssClass = (cssClass + " hidden").Trim();

			bool childrenShowInMenu = parentShowInMenu ? sm.ShowInMenu : true;

			siteMapText.AppendLine("<li id='s" + sm.SMItemID + "' rel='" + rel + "'><a href='#'" + (!String.IsNullOrEmpty(title) ? "title='" + title + "'" : "") + (!String.IsNullOrEmpty(cssClass) ? "class='" + cssClass + "' " : "") + ">" + sm.Name + "</a>");
			AddSiteMapLayer(allSiteMaps, siteMapText, sm.SMItemID, childrenShowInMenu);
			siteMapText.AppendLine("</li>");
		}
		siteMapText.AppendLine("</ul>");
	}

	private void PopulateSourceTree(int? cmMicrositeID)
	{
		StringBuilder sourceText = new StringBuilder();
		List<CMTemplate> templates = new List<CMTemplate>();
		templates.AddRange(CMTemplate.GetAll());
		if (!Settings.EnableMicrosites && templates.Any(t => t.Name.Equals("Microsite Template", StringComparison.OrdinalIgnoreCase)) || MicroSiteID == 0)
			templates.RemoveAll(t => t.Name.Equals("Microsite Template", StringComparison.OrdinalIgnoreCase));
		if (MicroSiteID != 0)
			templates.RemoveAll(t => t.Name.Equals("Default Template", StringComparison.OrdinalIgnoreCase));
		sourceText.AppendLine("<ul>");
		foreach (CMTemplate template in templates)
		{
			sourceText.AppendLine("<li rel='folder' id='t" + template.CMTemplateID + "'><a href='#'>" + template.Name + "</a>");
			List<CMPage> cmpages = CMPage.CMPageGetByCMTemplateIDAndLanguageID(template.CMTemplateID, (Settings.EnableMultipleLanguages ? (int?)LanguageID : null))
				.OrderBy(p => (Settings.EnableMultipleLanguages && !String.IsNullOrEmpty(p.CMPageTitleTitle) ? p.CMPageTitleTitle : p.Title))
				.Where(p => (!p.Deleted || (!LiveContent && p.Deleted && p.OriginalCMPageID.HasValue)) && (cmMicrositeID == -1 ? p.MicrositeDefault : p.CMMicrositeID == cmMicrositeID && !p.MicrositeDefault)).ToList();

			List<CMPage> tempPageList = new List<CMPage>();
			tempPageList.AddRange(cmpages);
			foreach (CMPage pageEntity in tempPageList)
			{
				if (pageEntity.OriginalCMPageID.HasValue)
					cmpages.RemoveAll(p => p.CMPageID == pageEntity.OriginalCMPageID.Value);
			}

			if (cmpages.Count > 0)
				sourceText.AppendLine("<ul>");
			foreach (CMPage page in cmpages)
			{
				string cssClass = "contextDisabled";
				string title = page.FileName;
				string rel = "page";
				if (page.Deleted || (page.EditorDeleted.HasValue && page.EditorDeleted.Value))
				{
					cssClass += " deleted";
					title += " - Deleted by Editor";
					rel = "deleted";
				}
				else if (page.NeedsApproval || page.OriginalCMPageID.HasValue)
				{
					cssClass += " changed";
					title += " - Unapproved Change";
					rel = "changed";
				}
				sourceText.AppendLine("<li id='p" + page.CMPageID + "' rel='" + rel + "'><a href='#'" + (!String.IsNullOrEmpty(title) ? "title='" + title + "' " : "") + (!String.IsNullOrEmpty(cssClass) ? "class='" + cssClass + "' " : "") + ">" + (Settings.EnableMultipleLanguages && !String.IsNullOrEmpty(page.CMPageTitleTitle) ? page.CMPageTitleTitle : page.Title) + "</a></li>");
			}
			if (cmpages.Count > 0)
				sourceText.AppendLine("</ul>");
			sourceText.AppendLine("</li>");
		}
		//The following section of code is for use with Existing Pages and External Links
		//This populates the source tree folder.
		sourceText.AppendLine("<li rel='folder' id='ExistingPagesandExternalLinks'><a href='#'>Existing Pages and External Links</a>");
		List<CMPage> externalPages = CMPage.GetAll().Where(p => (!p.Deleted || (!LiveContent && p.Deleted && p.OriginalCMPageID.HasValue)) && p.CMTemplateID == null && (cmMicrositeID == -1 ? p.MicrositeDefault : p.CMMicrositeID == cmMicrositeID && !p.MicrositeDefault)).OrderBy(p => p.Title).ToList();

		List<CMPage> tempExternalPageList = new List<CMPage>();
		tempExternalPageList.AddRange(externalPages);
		foreach (CMPage pageEntity in tempExternalPageList)
		{
			if (pageEntity.OriginalCMPageID.HasValue)
				externalPages.RemoveAll(p => p.CMPageID == pageEntity.OriginalCMPageID.Value);
		}

		if (externalPages.Count > 0)
			sourceText.AppendLine("<ul>");
		foreach (CMPage page in externalPages)
		{
			if (!SMItem.SMItemGetByShowInMenu(false).Exists(p => p.CMPageID == page.CMPageID))
			{
				string cssClass = string.Empty;
				string title = page.FileName;
				string rel = "page";
				if (page.Deleted || (page.EditorDeleted.HasValue && page.EditorDeleted.Value))
				{
					cssClass = rel = "deleted";
					title += " - Deleted by Editor";
				}
				else if (page.NeedsApproval || page.OriginalCMPageID.HasValue)
				{
					cssClass = rel = "changed";
					title += " - Unapproved Change";
				}
				sourceText.AppendLine("<li id='p" + page.CMPageID + "' rel='" + rel + "'><a href='#'" + (!String.IsNullOrEmpty(title) ? "title='" + title + "' " : "") + (!String.IsNullOrEmpty(cssClass) ? "class='" + cssClass + "' " : "") + ">" + page.Title + "</a></li>");
			}
		}
		if (externalPages.Count > 0)
			sourceText.AppendLine("</ul>");
		sourceText.AppendLine("</li></ul>");
		uxPagesList.Text = sourceText.ToString();
	}

	private static bool CheckChild(int dID, int sID, List<SMItem> allSMs)
	{
		foreach (SMItem item in allSMs.Where(s => s.SMItemParentID == sID).ToList())
		{
			if (item.SMItemID == dID)
				return true;
			if (CheckChild(dID, item.SMItemID, allSMs))
				return true;
		}
		return false;
	}

	private short GetRank(int? destID, List<SMItem> allSiteMaps)
	{
		return (short)((from s in allSiteMaps
						where s.SMItemParentID == destID && (s.LanguageID == LanguageID || !s.LanguageID.HasValue)
						select s.Rank).Count() > 0 ? (from s in allSiteMaps
													  where s.SMItemParentID == destID && (s.LanguageID == LanguageID || !s.LanguageID.HasValue)
													  select s.Rank).Max() + 1 : 0);
	}

	void DeleteOrRestorePageNode()
	{
		int cmPageID = Convert.ToInt32(Request.Form["id"]);
		List<SMItem> deleteSMPage = SMItem.SMItemGetByCMPageID(cmPageID);
		foreach (SMItem l in deleteSMPage)
		{
			CMSHelpers.UpdateSMItemsOnParentItemDelete(l.SMItemID, !CMSHelpers.HasFullCMSPermission());
			if (Settings.EnableApprovals && (!CMSHelpers.HasFullCMSPermission() || !LiveContent) && !l.NeedsApproval && !l.OriginalSMItemID.HasValue)
			{
				l.EditorDeleted = true;
				l.Save();
			}
			else
				l.Delete();
		}
		CMPage deletedPage = CMPage.GetByID(cmPageID);
		if (deletedPage != null)
		{
			deletedPage.Deleted = true;
			if (Settings.EnableApprovals && (!CMSHelpers.HasFullCMSPermission() || !LiveContent) && !deletedPage.NeedsApproval && !deletedPage.OriginalCMPageID.HasValue)
			{
				deletedPage.OriginalCMPageID = deletedPage.CMPageID;
				deletedPage.UserID = Helpers.GetCurrentUserID();
				deletedPage.Save();
			}
			else if (deletedPage.CMTemplateID.HasValue)
				deletedPage.Save();
			else
				deletedPage.Delete();
		}

		TrackEditors();
		SendJSONResponse("reload");
	}


	/// <summary>
	/// Changes whether the SM Item shows in the Menu or not
	/// </summary>
	void ShowSMItemInMenu(bool showInMenu)
	{
		int smItemID = Convert.ToInt32(Request.Form["id"]);
		SMItem smItemEntity = SMItem.GetByID(smItemID);
		smItemEntity.ShowInMenu = showInMenu;
		smItemEntity.Save();

		SendJSONResponse();
	}

	void DeleteSMItem()
	{
		int smItemID = Convert.ToInt32(Request.Form["id"]);
		List<SMItem> allSiteMaps = SMItem.GetAll();
		SMItem sm = SMItem.GetByID(smItemID);
		Action<List<SMItem>, SMItem> del = null;
		del = (smColl, smItem) =>
		{
			smColl.Where(s => s.SMItemParentID == smItem.SMItemID).ToList().ForEach(s =>
			{
				del(smColl, s);
			});

			if (Settings.EnableApprovals && (!CMSHelpers.HasFullCMSPermission() || !LiveContent) && !smItem.NeedsApproval && !smItem.OriginalSMItemID.HasValue)
			{
				smItem.EditorDeleted = true;
				smItem.Save();
			}
			else
			{
				foreach (SMItem sibling in smColl.Where(s => s.Rank > smItem.Rank && s.SMItemParentID == smItem.SMItemParentID))
				{
					sibling.Rank--;
					sibling.Save();
				}
				smItem.Delete();
			}
		};
		del(allSiteMaps, sm); // delete with children
		SendJSONResponse();
	}

	void RenameNode()
	{
		int smID = Convert.ToInt32(Request.Form["id"]);
		SMItem sm = SMItem.GetByID(smID);
		sm.Name = Request.Form["title"];
		if (Settings.EnableApprovals && (!CMSHelpers.HasFullCMSPermission() || !LiveContent) && !sm.NeedsApproval && !sm.OriginalSMItemID.HasValue)
		{
			sm.OriginalSMItemID = sm.SMItemID;
			sm.Save();
			foreach (SMItem subItem in SMItem.SMItemGetBySMItemParentID(sm.OriginalSMItemID))
			{
				subItem.SMItemParentID = sm.SMItemID;
				if (!subItem.NeedsApproval && !subItem.OriginalSMItemID.HasValue)
				{
					subItem.OriginalSMItemID = subItem.SMItemID;
					subItem.Save();
				}
				else
					subItem.Save();
			}
		}
		else
			sm.Save();

		TrackEditors();

		SendJSONResponse();
	}

	void MoveNode()
	{
		int sID = Convert.ToInt32(Request.Form["sourceID"].Substring(1));
		int dID = Request.Form["destinationID"] == "root" ? 0 : Convert.ToInt32(Request.Form["destinationID"].Substring(1));
		short position = Convert.ToInt16(Request.Form["position"]);
		string sourceType = Request.Form["sourceID"].Substring(0, 1);
		string destType = Request.Form["destinationID"].Substring(0, 1);
		bool newHome = Convert.ToBoolean(Request.Form["newHome"]);

		// can drop to root or a site map
		if (destType == "r" || destType == "s")
		{
			int? destID = dID;
			if (destID == 0)
				destID = null;

			List<SMItem> allSiteMaps;

			if (MicroSiteID == 0)
				allSiteMaps = SMItem.SMItemGetByCMMicrositeID(null).Where(s => !s.MicrositeDefault).ToList();
			else if (MicroSiteID == -1)
				allSiteMaps = SMItem.SMItemGetByMicrositeDefault(true);
			else
				allSiteMaps = SMItem.SMItemGetByCMMicrositeID(MicroSiteID);

			SMItem smParent = (from a in allSiteMaps where a.SMItemID == destID select a).SingleOrDefault();
			int depthCount = 0;
			while (smParent != null)
			{
				smParent = (from s in allSiteMaps where s.SMItemID == smParent.SMItemParentID select s).SingleOrDefault();
				depthCount++;
			}

			if (depthCount <= DepthLimit)
			{
				switch (sourceType)
				{
					//Folders
					case "E":
					case "t":
						{
							short rank = GetRank(destID, allSiteMaps);
							int? templateID = sID;
							if (templateID == 0)
								templateID = null;
							List<CMPage> pages;
							if (Settings.EnableMultipleLanguages)
								pages = CMPage.CMPageGetByCMTemplateIDAndLanguageID(templateID, LanguageID).Where(p => !p.Deleted && (MicroSiteID == -1 ? p.MicrositeDefault : (MicroSiteID == 0 ? p.CMMicrositeID == null : p.CMMicrositeID == MicroSiteID))).ToList();
							else
								pages = CMPage.CMPageGetByCMTemplateID(templateID).Where(p => !p.Deleted && (MicroSiteID == -1 ? p.MicrositeDefault : (MicroSiteID == 0 ? p.CMMicrositeID == null : p.CMMicrositeID == MicroSiteID))).ToList();
							foreach (CMPage page in pages)
							{
								if (allSiteMaps.Where(s => s.CMPageID == page.CMPageID).Count() == 0 || (Settings.EnableMultipleLanguages && allSiteMaps.Where(s => s.CMPageID == page.CMPageID && s.LanguageID == LanguageID).Count() == 0))
								{
									SMItem smNew = new SMItem();
									smNew.CMPageID = page.OriginalCMPageID.HasValue ? page.OriginalCMPageID.Value : page.CMPageID;
									smNew.Name = Settings.EnableMultipleLanguages ? page.CMPageTitleTitle ?? page.Title : page.Title;
									smNew.SMItemParentID = destID;
									smNew.Rank = rank;
									smNew.ShowInMenu = true;
									if (MicroSiteID > 0)
										smNew.CMMicrositeID = MicroSiteID;
									else if (MicroSiteID == -1)
										smNew.MicrositeDefault = true;
									if (Settings.EnableMultipleLanguages)
										smNew.LanguageID = LanguageID;
									if (Settings.EnableApprovals && (!CMSHelpers.HasFullCMSPermission() || !LiveContent))
										smNew.NeedsApproval = true;
									smNew.Save();
									allSiteMaps.Add(smNew);
									rank++;
								}
							}
							TrackEditors();
							SendJSONResponse("reload");
						}
						break;
					//In sitemap already
					case "s":
						{
							if (!CheckChild(dID, sID, allSiteMaps))
							{
								short rank = (short)(position + 1);
								SMItem smOld = allSiteMaps.Where(s => s.SMItemID == sID).FirstOrDefault();
								smOld.SMItemParentID = destID;
								smOld.Rank = rank;
								smOld.SMItemParent = null;
								List<SMItem> temp = allSiteMaps.Where(s => s.SMItemParentID == destID && s.SMItemID != smOld.SMItemID).OrderBy(s => s.Rank).ToList();
								for (int i = 0; i < rank - 1; i++)
								{
									SMItem adjustedItem = temp[i];
									if (adjustedItem.Rank != (short)(i + 1))
									{
										adjustedItem.Rank = (short)(i + 1);
										if (Settings.EnableApprovals && (!CMSHelpers.HasFullCMSPermission() || !LiveContent) && !adjustedItem.NeedsApproval && !adjustedItem.OriginalSMItemID.HasValue)
										{
											adjustedItem.OriginalSMItemID = adjustedItem.SMItemID;
											adjustedItem.Save();
										}
										else
											adjustedItem.Save();
									}
								}
								for (int i = rank - 1; i < temp.Count; i++)
								{
									SMItem adjustedItem = temp[i];
									if (adjustedItem.Rank != (short)(i + 2))
									{
										adjustedItem.Rank = (short)(i + 2);
										if (Settings.EnableApprovals && (!CMSHelpers.HasFullCMSPermission() || !LiveContent) && !adjustedItem.NeedsApproval && !adjustedItem.OriginalSMItemID.HasValue)
										{
											adjustedItem.OriginalSMItemID = adjustedItem.SMItemID;
											adjustedItem.Save();
										}
										else
											adjustedItem.Save();
									}
								}
								if (Settings.EnableApprovals && (!CMSHelpers.HasFullCMSPermission() || !LiveContent) && !smOld.NeedsApproval && !smOld.OriginalSMItemID.HasValue)
								{
									smOld.OriginalSMItemID = smOld.SMItemID;
									smOld.Save();
								}
								else
									smOld.Save();
							}
						}
						break;
					//Page from left tree
					case "p":
						{
							short rank = (short)(position + 1);
							CMPage page = null;
							if (Settings.EnableMultipleLanguages)
								page = CMPage.GetByCMPageIDAndLanguageID(sID, LanguageID);
							if (page == null)
								page = CMPage.GetByID(sID); //even if in multilingual setup, external links have no associated language id.
							if (!ForceOneToOne || (!allSiteMaps.Any(s => s.CMPageID == page.CMPageID) || (Settings.EnableMultipleLanguages && !allSiteMaps.Any(s => s.CMPageID == page.CMPageID && s.LanguageID == LanguageID && (MicroSiteID == 0 || s.NewHomes.Equals(newHome))))))
							{
								SMItem smNew = new SMItem();
								smNew.CMPageID = page.OriginalCMPageID.HasValue ? page.OriginalCMPageID.Value : page.CMPageID;
								smNew.Name = Settings.EnableMultipleLanguages ? page.CMPageTitleTitle ?? page.Title : page.Title;								
								smNew.SMItemParentID = destID;
								smNew.Rank = rank;
								smNew.ShowInMenu = true;
								if (MicroSiteID > 0)
									smNew.CMMicrositeID = MicroSiteID;
								else if (MicroSiteID == -1)
									smNew.MicrositeDefault = true;
								if (Settings.EnableMultipleLanguages)
									smNew.LanguageID = LanguageID;
								if (Settings.EnableApprovals && (!CMSHelpers.HasFullCMSPermission() || !LiveContent))
									smNew.NeedsApproval = true;
								smNew.NewHomes = smNew.CMMicrositeID.HasValue ? (bool?)newHome : null;
								List<SMItem> temp = allSiteMaps.Where(s => s.SMItemParentID == destID).OrderBy(s => s.Rank).ToList();
								for (int i = 0; i < rank - 1; i++)
								{
									SMItem adjustedItem = temp[i];
									if (adjustedItem.Rank != (short)(i + 1))
									{
										adjustedItem.Rank = (short)(i + 1);
										if (Settings.EnableApprovals && (!CMSHelpers.HasFullCMSPermission() || !LiveContent) && !adjustedItem.NeedsApproval && !adjustedItem.OriginalSMItemID.HasValue)
										{
											adjustedItem.OriginalSMItemID = adjustedItem.SMItemID;
											adjustedItem.Save();
										}
										else
											adjustedItem.Save();
									}
								}
								for (int i = rank - 1; i < temp.Count; i++)
								{
									SMItem adjustedItem = temp[i];
									if (adjustedItem.Rank != (short)(i + 2))
									{
										adjustedItem.Rank = (short)(i + 2);
										if (Settings.EnableApprovals && (!CMSHelpers.HasFullCMSPermission() || !LiveContent) && !adjustedItem.NeedsApproval && !adjustedItem.OriginalSMItemID.HasValue)
										{
											adjustedItem.OriginalSMItemID = adjustedItem.SMItemID;
											adjustedItem.Save();
										}
										else
											adjustedItem.Save();
									}
								}
								smNew.Save();
								allSiteMaps.Add(smNew);
								TrackEditors();
								SendJSONResponse("s" + smNew.SMItemID);
							}
							else
								SendJSONResponse("That page already exists in the sitemap");
						}
						break;
				}
				TrackEditors();
				SendJSONResponse();
			}
			else
				SendJSONResponse("Your page was not added, because the sitemap may only be " + Settings.DepthLimit + " pages deep.");
		}
	}

	void RestoreItem()
	{
		int smItemID = Convert.ToInt32(Request.Form["id"]);
		SMItem sm = SMItem.GetByID(smItemID);
		sm.EditorDeleted = false;
		sm.Save();
		SendJSONResponse();
	}

	void SendJSONResponse(string message = null)
	{
		CMSHelpers.ClearCaches();
		Response.ContentType = "application/json";
		if (!CMSHelpers.HasFullCMSPermission() && LiveContent)
			Response.Write("{ \"status\" : \"reload\" }");
		else if (!String.IsNullOrEmpty(message))
			Response.Write("{ \"status\" : \"" + message + "\" }");
		else
			Response.Write("{ \"status\" : 1 }");
		Response.End();
	}

	private void uxLiveContent_Click(object sender, EventArgs e)
	{
		LiveContent = true;
		Populate();
	}

	private void uxUnapprovedContent_Click(object sender, EventArgs e)
	{
		LiveContent = false;
		Populate();
	}

	private void TrackEditors()
	{
		if (Settings.EnableApprovals && !CMSHelpers.HasFullCMSPermission())
		{
			int userID = Helpers.GetCurrentUserID();
			int? microsite = null;
			if (MicroSiteID != 0)
				microsite = MicroSiteID;
			int? languageID = null;
			if (Settings.EnableMultipleLanguages)
				languageID = LanguageID;
			if (SMItemUser.SMItemUserGetByUserID(userID).Count(s => (MicroSiteID > 0 ? s.CMMicrositeID == MicroSiteID : MicroSiteID == -1 ? s.MicrositeDefault : s.CMMicrositeID == null) && s.LanguageID == languageID) == 0)
			{
				SMItemUser smEditor = new SMItemUser { UserID = userID, MicrositeDefault = MicroSiteID == -1, CMMicrositeID = microsite, LanguageID = languageID };
				smEditor.Save();
			}

			CMSHelpers.SendSiteMapApprovalEmailAlerts(userID, microsite, languageID, false, null);
		}
	}

	#region Validators

	protected void uxPageTitleCustomVal_ServerValidate(object source, ServerValidateEventArgs args)
	{
		List<CMPage> listPages = CMPage.CMPageGetByTitle(uxPageTitle.Text).Where(p => (MicroSiteID <= 0 ? p.CMMicrositeID == null : p.CMMicrositeID == MicroSiteID)).ToList();
		args.IsValid = listPages.Count == 0;
	}

	protected void uxLinkToPageExistsCustomVal_ServerValidate(object source, ServerValidateEventArgs args)
	{
		List<CMPage> listPages = CMPage.CMPageGetByFileName(uxLinkToPage.Text).Where(p => (MicroSiteID <= 0 ? p.CMMicrositeID == null : p.CMMicrositeID == MicroSiteID)).ToList();
		args.IsValid = listPages.Count == 0;
	}

	protected void uxLinkToPageCustomVal2_ServerValidate(object source, ServerValidateEventArgs args)
	{
		if (uxLinkToPage.Text.StartsWith("http://") || uxLinkToPage.Text.StartsWith("https://")) // || uxLinkToPage.Text.StartsWith("www.") || uxLinkToPage.Text.EndsWith(".com") || uxLinkToPage.Text.EndsWith(".net"))
			args.IsValid = true;
		else
		{
			try
			{
				FileInfo f = new FileInfo(Server.MapPath("~\\") + uxLinkToPage.Text.Split('?')[0].Split('#')[0]);
				args.IsValid = f.Exists;
			}
			catch (Exception)
			{
				args.IsValid = false;
			}
			if (!args.IsValid && (uxLinkToPage.Text.Contains("?") || uxLinkToPage.Text.Contains("#")))
				args.IsValid = CMPage.CMPageGetByFileName(uxLinkToPage.Text.Split('?')[0].Split('#')[0]).Any();

		}
	}

	#endregion
}