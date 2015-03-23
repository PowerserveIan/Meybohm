using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.ContentManager;
using Classes.SEOComponent;
using Classes.SiteLanguages;
using Settings = Classes.ContentManager.Settings;

public partial class Admin_ContentManager_ApprovalAlerts : Page
{
	private List<CMPage> pageContentNeedingApproval;
	private List<CMPage> pagePropertiesNeedingApproval;
	private List<SMItem> sitemapItemsNeedingApproval;
	private List<int> sitemapSectionsNeedingApproval;

	protected override void OnInit(EventArgs e)
	{
		if (!Settings.EnableApprovals)
			Response.Redirect("~/admin/content-manager/content-manager.aspx");
		base.OnInit(e);
		uxSiteSectionRepeater.ItemDataBound += uxSiteSectionRepeater_ItemDataBound;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
			BindData();
	}

	private void BindData()
	{
		List<CMPageRegion> unapprovedRegions = CMPageRegion.CMPageRegionGetByNeedsApproval(true);
		List<CMPageRegion> unapprovedGlobalRegions = unapprovedRegions.Where(c => c.GlobalAreaCMPageID.HasValue).ToList();
		unapprovedRegions = unapprovedRegions.Where(c => !c.GlobalAreaCMPageID.HasValue).ToList();
		if (unapprovedGlobalRegions.Count > 0)
		{
			uxUnapprovedGlobalAreas.DataSource = unapprovedGlobalRegions;
			uxUnapprovedGlobalAreas.DataBind();
		}
		else
			uxUnapprovedGlobalAreas.Visible = false;

		List<int> sectionsNeedingApproval = new List<int>();
		pageContentNeedingApproval = new List<CMPage>();
		pagePropertiesNeedingApproval = CMPage.GetAllPagesNeedingApproval();

		List<Language> allLanguages = null;
		if (Settings.EnableMultipleLanguages)
			allLanguages = Language.GetAll();

		unapprovedRegions.ForEach(r =>
									{
										CMPage pageToAdd;
										if (Settings.EnableMultipleLanguages && r.LanguageID.HasValue)
											pageToAdd = CMPage.GetByCMPageIDAndLanguageID(r.CMPageID, r.LanguageID.Value);
										else
											pageToAdd = CMPage.GetByID(r.CMPageID);

										if (pageToAdd == null) return;
										if (Settings.EnableMultipleLanguages && allLanguages != null && allLanguages.Count > 0)
										{
											Language pageLanguage = allLanguages.Find(l => l.LanguageID == r.LanguageID);
											if (pageLanguage != null)
												pageToAdd.CultureName = pageLanguage.CultureName;
										}
										pageContentNeedingApproval.Add(pageToAdd);
									});

		foreach (CMPage p in pageContentNeedingApproval)
		{
			sectionsNeedingApproval = AddToSections(sectionsNeedingApproval, p);
		}

		foreach (CMPage p in pagePropertiesNeedingApproval)
		{
			sectionsNeedingApproval = AddToSections(sectionsNeedingApproval, p);
		}

		sitemapItemsNeedingApproval = SMItem.GetAllSMItemsNeedingApproval(null);
		sitemapSectionsNeedingApproval = new List<int>();
		foreach (SMItem s in sitemapItemsNeedingApproval)
		{
			int sectionID = 0;
			if (s.MicrositeDefault)
				sectionID = -1;
			else if (s.CMMicrositeID.HasValue)
				sectionID = s.CMMicrositeID.Value;
			if (!sitemapSectionsNeedingApproval.Contains(sectionID))
				sitemapSectionsNeedingApproval.Add(sectionID);
			if (!sectionsNeedingApproval.Contains(sectionID))
				sectionsNeedingApproval.Add(sectionID);
		}

		if (sectionsNeedingApproval.Count == 0)
		{
			uxNothingNeedingApproval.Visible = true;
			uxApprove.Visible = uxDeny.Visible = uxSiteSectionRepeater.Visible = uxSelectAll.Visible = false;
		}
		else
		{
			sectionsNeedingApproval.Sort();
			uxSiteSectionRepeater.DataSource = sectionsNeedingApproval;
			uxSiteSectionRepeater.DataBind();
		}
	}

	private void uxSiteSectionRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			int sectionID = Convert.ToInt32(e.Item.DataItem);
			Literal uxSectionLabel = (Literal)e.Item.FindControl("uxSectionLabel");
			Repeater uxContentRepeater = (Repeater)e.Item.FindControl("uxContentRepeater");
			Repeater uxPropertiesRepeater = (Repeater)e.Item.FindControl("uxPropertiesRepeater");
			Repeater uxSiteMapRepeater = (Repeater)e.Item.FindControl("uxSiteMapRepeater");
			List<SMItem> siteMapItemsInSectionNeedingApproval;
			switch (sectionID)
			{
				case -1:
					uxContentRepeater.DataSource = pageContentNeedingApproval.Where(c => c.MicrositeDefault);
					uxContentRepeater.Visible = pageContentNeedingApproval.Count(c => c.MicrositeDefault) > 0;
					uxPropertiesRepeater.DataSource = pagePropertiesNeedingApproval.Where(c => c.MicrositeDefault);
					uxPropertiesRepeater.Visible = pagePropertiesNeedingApproval.Count(c => c.MicrositeDefault) > 0;
					uxSectionLabel.Text = @"Microsite Default";
					siteMapItemsInSectionNeedingApproval = sitemapItemsNeedingApproval.Where(s => s.MicrositeDefault).ToList();
					break;
				case 0:
					uxContentRepeater.DataSource = pageContentNeedingApproval.Where(c => c.CMMicrositeID == null && !c.MicrositeDefault);
					uxContentRepeater.Visible = pageContentNeedingApproval.Count(c => c.CMMicrositeID == null && !c.MicrositeDefault) > 0;
					uxPropertiesRepeater.DataSource = pagePropertiesNeedingApproval.Where(c => c.CMMicrositeID == null && !c.MicrositeDefault);
					uxPropertiesRepeater.Visible = pagePropertiesNeedingApproval.Count(c => c.CMMicrositeID == null && !c.MicrositeDefault) > 0;
					uxSectionLabel.Text = @"Main Site";
					siteMapItemsInSectionNeedingApproval = sitemapItemsNeedingApproval.Where(s => s.CMMicrositeID == null && !s.MicrositeDefault).ToList();
					break;
				default:
					uxContentRepeater.DataSource = pageContentNeedingApproval.Where(c => c.CMMicrositeID == sectionID);
					uxContentRepeater.Visible = pageContentNeedingApproval.Count(c => c.CMMicrositeID == sectionID) > 0;
					uxPropertiesRepeater.DataSource = pagePropertiesNeedingApproval.Where(c => c.CMMicrositeID == sectionID);
					uxPropertiesRepeater.Visible = pagePropertiesNeedingApproval.Count(c => c.CMMicrositeID == sectionID) > 0;
					uxSectionLabel.Text = CMMicrosite.GetByID(sectionID).Name;
					siteMapItemsInSectionNeedingApproval = sitemapItemsNeedingApproval.Where(s => s.CMMicrositeID == sectionID).ToList();
					break;
			}

			if (sitemapSectionsNeedingApproval.Contains(sectionID))
			{
				List<SMItem> culturesNeedingApproval = new List<SMItem>();
				if (!Settings.EnableMultipleLanguages && siteMapItemsInSectionNeedingApproval.Exists(s => String.IsNullOrEmpty(s.CultureName)))
					culturesNeedingApproval.Add(siteMapItemsInSectionNeedingApproval.Find(s => String.IsNullOrEmpty(s.CultureName)));
				foreach (SMItem s in siteMapItemsInSectionNeedingApproval)
				{
					if (!String.IsNullOrEmpty(s.CultureName) && !culturesNeedingApproval.Exists(c => c.CultureName == s.CultureName))
						culturesNeedingApproval.Add(s);
				}
				uxSiteMapRepeater.DataSource = culturesNeedingApproval;
				uxSiteMapRepeater.DataBind();
			}

			uxContentRepeater.DataBind();
			uxPropertiesRepeater.DataBind();
		}
	}

	private static List<int> AddToSections(List<int> sectionsNeedingApproval, CMPage p)
	{
		int sectionID = 0;
		if (p.MicrositeDefault)
			sectionID = -1;
		else if (p.CMMicrositeID.HasValue)
			sectionID = p.CMMicrositeID.Value;
		if (!sectionsNeedingApproval.Contains(sectionID))
			sectionsNeedingApproval.Add(sectionID);
		return sectionsNeedingApproval;
	}

	protected void Approval_Command(object sender, CommandEventArgs e)
	{
		foreach (RepeaterItem rI in uxUnapprovedGlobalAreas.Items)
		{
			CheckBox uxCheckBox = (CheckBox)rI.FindControl("uxCheckBox");
			if (uxCheckBox.Checked)
			{
				HiddenField uxRegionID = (HiddenField)rI.FindControl("uxRegionID");
				CMPageRegion region = CMPageRegion.GetByID(Convert.ToInt32(uxRegionID.Value));
				if (e.CommandName == "Approve")
					ApproveRegion(region);
				else if (e.CommandName == "Deny")
					DenyRegion(region);
			}
		}

		foreach (RepeaterItem rI in uxSiteSectionRepeater.Items)
		{
			Repeater uxContentRepeater = (Repeater)rI.FindControl("uxContentRepeater");
			Repeater uxPropertiesRepeater = (Repeater)rI.FindControl("uxPropertiesRepeater");
			Repeater uxSiteMapRepeater = (Repeater)rI.FindControl("uxSiteMapRepeater");

			foreach (RepeaterItem rI2 in uxContentRepeater.Items)
			{
				CheckBox uxCheckBox = (CheckBox)rI2.FindControl("uxCheckBox");
				if (uxCheckBox.Checked)
				{
					HiddenField uxPageID = (HiddenField)rI2.FindControl("uxPageID");
					HiddenField uxLanguage = (HiddenField)rI2.FindControl("uxLanguage");
					CMPageRegion.Filters regionFilterList = new CMPageRegion.Filters();
					regionFilterList.FilterCMPageRegionCMPageID = uxPageID.Value;
					regionFilterList.FilterCMPageRegionNeedsApproval = true.ToString();
					if (Settings.EnableMultipleLanguages)
					{
						Language languageEntity = Language.LanguageGetByCultureName(uxLanguage.Value).FirstOrDefault();
						if (languageEntity != null)
							regionFilterList.FilterCMPageRegionLanguageID = languageEntity.LanguageID.ToString();
					}

					CMPageRegion region = CMPageRegion.CMPageRegionPage(0, 0, "", "CMPageRegionID", false, regionFilterList).FirstOrDefault();

					if (region != null)
						if (e.CommandName == "Approve")
							ApproveRegion(region);
						else if (e.CommandName == "Deny")
							DenyRegion(region);
				}
			}
			foreach (RepeaterItem rI2 in uxPropertiesRepeater.Items)
			{
				CheckBox uxCheckBox = (CheckBox)rI2.FindControl("uxCheckBox");
				if (uxCheckBox.Checked)
				{
					HiddenField uxPageID = (HiddenField)rI2.FindControl("uxPageID");
					HiddenField uxLanguage = (HiddenField)rI2.FindControl("uxLanguage");
					CMPage cmPageEntity = CMPage.GetByID(Convert.ToInt32(uxPageID.Value));
					int? languageID = null;
					if (Settings.EnableMultipleLanguages && !String.IsNullOrEmpty(uxLanguage.Value))
						languageID = Language.LanguageGetByCultureName(uxLanguage.Value).FirstOrDefault().LanguageID;
					if (e.CommandName == "Approve")
						ApprovePage(cmPageEntity, languageID);
					else if (e.CommandName == "Deny")
						DenyPage(cmPageEntity, languageID);
				}
			}
			foreach (RepeaterItem rI2 in uxSiteMapRepeater.Items)
			{
				CheckBox uxCheckBox = (CheckBox)rI2.FindControl("uxCheckBox");
				if (uxCheckBox.Checked)
				{
					HiddenField uxMicrositeID = (HiddenField)rI2.FindControl("uxMicrositeID");
					HiddenField uxLanguage = (HiddenField)rI2.FindControl("uxLanguage");

					int? micrositeID = null;
					int temp;
					if (!String.IsNullOrEmpty(uxMicrositeID.Value) && Int32.TryParse(uxMicrositeID.Value, out temp))
						micrositeID = temp;
					if (micrositeID == 0)
						micrositeID = null;

					int? languageID = null;
					if (Settings.EnableMultipleLanguages && !String.IsNullOrEmpty(uxLanguage.Value))
						languageID = Language.LanguageGetByCultureName(uxLanguage.Value).FirstOrDefault().LanguageID;

					if (e.CommandName == "Approve")
						ApproveSitemap(micrositeID, languageID);
					else if (e.CommandName == "Deny")
						DenySitemap(micrositeID, languageID);
				}
			}
		}
		CMSHelpers.ClearCaches();
		BindData();
	}

	private static void ApproveRegion(CMPageRegion region)
	{
		CMPageRegion.Filters regionFilterList = new CMPageRegion.Filters();
		regionFilterList.FilterCMPageRegionCMPageID = region.CMPageID.ToString();
		regionFilterList.FilterCMPageRegionCurrentVersion = true.ToString();
		regionFilterList.FilterCMPageRegionCMRegionID = region.CMRegionID.ToString();
		if (Settings.EnableMultipleLanguages && region.LanguageID.HasValue)
			regionFilterList.FilterCMPageRegionLanguageID = region.LanguageID.ToString();

		CMPageRegion cmPRlast = CMPageRegion.CMPageRegionPage(0, 0, "", "CMPageRegionID", true, regionFilterList).FirstOrDefault();

		if (cmPRlast != null)
		{
			cmPRlast.CurrentVersion = false;
			cmPRlast.Save();
		}
		region.NeedsApproval = false;
		region.CurrentVersion = true;
		if (!region.GlobalAreaCMPageID.HasValue)
		{
			/*Don't send search content if global area change*/
			/*send files on approvals*/
//			Classes.ContentManager.SearchPlugin.SendAllFileContent();
//			Classes.ContentManager.SearchPlugin searchPlugin = new Classes.ContentManager.SearchPlugin();
//			CMRegion cmRegion = CMRegion.GetByID(region.CMRegionID);
//			searchPlugin.Name = cmRegion.Name;
//			CMPage pageEntity = CMPage.GetByID(region.CMPageID);
//			searchPlugin.SendSearchContent(pageEntity.CMMicrositeID, pageEntity.FileName, region.Content, region.ContentClean);
		}
		region.GlobalAreaCMPageID = null;

		CMSHelpers.SendApprovalEmailAlerts(CMPage.GetByID(region.CMPageID), region, Helpers.GetCurrentUserID(), true, true, true, region.LanguageID);
		region.EditorUserIDs = null;
		region.Save();
	}

	private static void ApprovePage(CMPage cmPageEntity, int? languageID)
	{
		CMMicrosite micrositeEntity = null;
		if (cmPageEntity.CMMicrositeID.HasValue)
			micrositeEntity = CMMicrosite.GetByID(cmPageEntity.CMMicrositeID.Value);
		//Save SEO first
		List<SEOData> unFilteredSetups = SEOData.SEODataGetByPageURL("~/" + (micrositeEntity != null ? micrositeEntity.Name + "/" : "") + cmPageEntity.FileName);
		List<SEOData> setups = unFilteredSetups;

		if (Settings.EnableMultipleLanguages)
			setups = setups.Where(s => s.LanguageID == languageID).ToList();

		if (setups.Exists(s => !s.Approved))
			setups.ForEach(s => { if (s.Approved) s.Delete(); });

		SEOData approvedSEOSetup = setups.Find(s => !s.Approved);
		if (approvedSEOSetup != null)
		{
			unFilteredSetups.Remove(approvedSEOSetup);
			approvedSEOSetup.Approved = true;
			approvedSEOSetup.Save();
		}

		CMSHelpers.SendApprovalEmailAlerts(cmPageEntity, null, Helpers.GetCurrentUserID(), false, true, true, languageID);

		#region ApprovePagePermissions

		//Update Page Permissions
		if (Settings.EnableCMPageRoles)
		{
			List<CMPageRole> pageRoles = CMPageRole.CMPageRoleGetByCMPageID(cmPageEntity.CMPageID);
			List<CMPageRole> originalPageRoles = new List<CMPageRole>();
			if (cmPageEntity.OriginalCMPageID.HasValue)
				originalPageRoles = CMPageRole.CMPageRoleGetByCMPageID(cmPageEntity.OriginalCMPageID.Value);

			//Remove deleted page roles
			foreach (CMPageRole roleEntity in originalPageRoles)
			{
				if (!pageRoles.Contains(roleEntity))
					roleEntity.Delete();
			}

			//Add new page roles
			foreach (CMPageRole roleEntity in pageRoles)
			{
				if (!originalPageRoles.Contains(roleEntity) && cmPageEntity.OriginalCMPageID.HasValue)
				{
					roleEntity.CMPageID = cmPageEntity.OriginalCMPageID.Value;
					roleEntity.Save();
				}
			}
		}

		#endregion

		if (cmPageEntity.OriginalCMPageID.HasValue)
		{
			CMPage originalPage = CMPage.GetByID(cmPageEntity.OriginalCMPageID.Value);
			originalPage.Deleted = cmPageEntity.EditorDeleted.HasValue ? cmPageEntity.EditorDeleted.Value : cmPageEntity.Deleted;
			originalPage.Title = cmPageEntity.Title;
			originalPage.EditorUserIDs = null;
			originalPage.FeaturedPage = cmPageEntity.FeaturedPage;
			originalPage.FormRecipient = cmPageEntity.FormRecipient;
			originalPage.NeedsApproval = false;
			originalPage.ResponsePageID = cmPageEntity.ResponsePageID;
			originalPage.UserID = cmPageEntity.UserID;
			originalPage.Save();

			if (originalPage.Deleted) //Remove item from site map
				SMItem.SMItemGetByCMPageID(originalPage.CMPageID).ForEach(s => s.Delete());

			List<CMPageTitle> changedTitles = new List<CMPageTitle>();
			if (Settings.EnableMultipleLanguages)
			{
				//Update original titles with new titles
				changedTitles = CMPageTitle.CMPageTitleGetByCMPageID(cmPageEntity.CMPageID);
				CMPageTitle.CMPageTitleGetByCMPageID(cmPageEntity.OriginalCMPageID.Value).ForEach(c =>
																									{
																										if (c.LanguageID == languageID && changedTitles.Exists(t => t.LanguageID == c.LanguageID))
																										{
																											CMPageTitle newTitle = changedTitles.Find(t => t.LanguageID == c.LanguageID);
																											c.Title = newTitle.Title;
																											c.Save();
																											newTitle.Delete();
																											changedTitles.RemoveAll(t => t.CMPageTitleID == newTitle.CMPageTitleID);
																										}
																									});
				changedTitles.ForEach(c =>
										{
											if (c.LanguageID == languageID)
											{
												c.CMPageID = cmPageEntity.OriginalCMPageID.Value;
												c.Save();
												changedTitles.RemoveAll(t => t.CMPageTitleID == c.CMPageTitleID);
											}
										});
			}

			//Only delete Page if there are no other languages needing approval
			if (!Settings.EnableMultipleLanguages || (Settings.EnableMultipleLanguages && changedTitles.Count == 0 && !unFilteredSetups.Exists(s => !s.Approved)))
				cmPageEntity.Delete();
		}
		else if (cmPageEntity.NeedsApproval)
		{
			if (cmPageEntity.EditorDeleted.HasValue)
				cmPageEntity.Deleted = cmPageEntity.EditorDeleted.Value;
			cmPageEntity.EditorDeleted = null;
			cmPageEntity.NeedsApproval = false;
			cmPageEntity.EditorUserIDs = null;
			cmPageEntity.Save();
		}
		else if (cmPageEntity.EditorDeleted.HasValue)
		{
			cmPageEntity.Deleted = cmPageEntity.EditorDeleted.Value;
			cmPageEntity.EditorDeleted = null;
			cmPageEntity.EditorUserIDs = null;
			cmPageEntity.Save();

			if (cmPageEntity.Deleted) //Remove item from site map
				SMItem.SMItemGetByCMPageID(cmPageEntity.CMPageID).ForEach(s => s.Delete());
		}
	}

	private static void ApproveSitemap(int? micrositeID, int? languageID)
	{
		List<SMItem> allUnapprovedItems = SMItem.GetAllSMItemsNeedingApproval(Settings.EnableMultipleLanguages ? languageID : null);

		if (micrositeID == -1)
			allUnapprovedItems = allUnapprovedItems.Where(s => s.MicrositeDefault).ToList();
		else if (micrositeID.HasValue)
			allUnapprovedItems = allUnapprovedItems.Where(s => s.CMMicrositeID == micrositeID).ToList();

		foreach (SMItem item in allUnapprovedItems)
		{
			if (item.NeedsApproval)
				item.NeedsApproval = false;
			else if (item.OriginalSMItemID.HasValue)
			{
				SMItem originalSMItem = SMItem.GetByID(item.OriginalSMItemID.Value);
				if (originalSMItem != null)
					originalSMItem.Delete();
				item.OriginalSMItemID = null;
			}
			if (item.EditorDeleted)
				item.Delete();
			else
				item.Save();
		}

		//Approve all external pages
		CMPage.CMPageGetByNeedsApproval(true).ForEach(c =>
															{
																if (c.CMTemplateID == null)
																{
																	c.NeedsApproval = false;
																	c.Save();
																}
															});

		CMSHelpers.SendSiteMapApprovalEmailAlerts(Helpers.GetCurrentUserID(), micrositeID, languageID, true, true);
	}

	private static void DenyRegion(CMPageRegion region)
	{
		if (region == null) return;
		CMSHelpers.SendApprovalEmailAlerts(CMPage.GetByID(region.CMPageID), region, Helpers.GetCurrentUserID(), true, true, false, region.LanguageID);
		region.Delete();
	}

	private static void DenyPage(CMPage cmPageEntity, int? languageID)
	{
		CMMicrosite micrositeEntity = null;
		if (cmPageEntity.CMMicrositeID.HasValue)
			micrositeEntity = CMMicrosite.GetByID(cmPageEntity.CMMicrositeID.Value);

		//Remove unapproved SEO first
		List<SEOData> unFilteredSetups = SEOData.SEODataGetByPageURL("~/" + (micrositeEntity != null ? micrositeEntity.Name + "/" : "") + cmPageEntity.FileName);
		List<SEOData> setups = unFilteredSetups;

		if (Settings.EnableMultipleLanguages)
			setups = setups.Where(s => s.LanguageID == languageID).ToList();

		setups.ForEach(s =>
						{
							if (!s.Approved)
							{
								s.Delete();
								unFilteredSetups.Remove(s);
							}
						});

		CMSHelpers.SendApprovalEmailAlerts(cmPageEntity, null, Helpers.GetCurrentUserID(), false, true, false, languageID);

		if (cmPageEntity.EditorDeleted.HasValue)
		{
			cmPageEntity.EditorUserIDs = null;
			cmPageEntity.EditorDeleted = null;
			cmPageEntity.Save();
		}

		if (cmPageEntity.OriginalCMPageID.HasValue || cmPageEntity.NeedsApproval)
		{
			//Don't delete title or permissions if this is the original page created by Admin
			List<CMPageTitle> changedTitles = new List<CMPageTitle>();
			if (Settings.EnableMultipleLanguages)
			{
				changedTitles = CMPageTitle.CMPageTitleGetByCMPageID(cmPageEntity.CMPageID);
				//Delete new titles
				changedTitles.ForEach(c => { if (c.LanguageID == languageID) c.Delete(); });
			}

			#region DenyPagePermissions

			//Update Page Permissions
			if (Settings.EnableCMPageRoles)
			{
				List<CMPageRole> pageRoles = CMPageRole.CMPageRoleGetByCMPageID(cmPageEntity.CMPageID);

				//Remove page roles
				foreach (CMPageRole roleEntity in pageRoles)
				{
					roleEntity.Delete();
				}
			}

			#endregion

			if (!Settings.EnableMultipleLanguages || (Settings.EnableMultipleLanguages && changedTitles.Count(t => t.LanguageID != languageID) == 0 && !unFilteredSetups.Exists(s => !s.Approved)))
				cmPageEntity.Delete();
		}
	}

	private static void DenySitemap(int? micrositeID, int? languageID)
	{
		List<SMItem> allUnapprovedItems = SMItem.GetAllSMItemsNeedingApproval(Settings.EnableMultipleLanguages ? languageID : null);

		if (micrositeID == -1)
			allUnapprovedItems = allUnapprovedItems.Where(s => s.MicrositeDefault).ToList();
		else if (micrositeID.HasValue)
			allUnapprovedItems = allUnapprovedItems.Where(s => s.CMMicrositeID == micrositeID).ToList();

		foreach (SMItem item in allUnapprovedItems)
		{
			if (item.EditorDeleted)
			{
				item.EditorDeleted = false;
				item.Save();
			}
			else
				item.Delete();
		}

		//Delete all unapproved external pages
		CMPage.CMPageGetByNeedsApproval(true).ForEach(c =>
															{
																if (c.CMTemplateID == null)
																	c.Delete();
															});
		CMSHelpers.SendSiteMapApprovalEmailAlerts(Helpers.GetCurrentUserID(), micrositeID, languageID, true, false);
	}

	protected void uxSelectOneValidator_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = false;
		foreach (RepeaterItem rI in uxUnapprovedGlobalAreas.Items)
		{
			CheckBox uxCheckBox = (CheckBox)rI.FindControl("uxCheckBox");
			if (uxCheckBox.Checked)
			{
				args.IsValid = true;
				return;
			}
		}
		foreach (RepeaterItem rI in uxSiteSectionRepeater.Items)
		{
			Repeater uxContentRepeater = (Repeater)rI.FindControl("uxContentRepeater");
			Repeater uxPropertiesRepeater = (Repeater)rI.FindControl("uxPropertiesRepeater");
			Repeater uxSiteMapRepeater = (Repeater)rI.FindControl("uxSiteMapRepeater");

			foreach (RepeaterItem rI2 in uxContentRepeater.Items)
			{
				CheckBox uxCheckBox = (CheckBox)rI2.FindControl("uxCheckBox");
				if (uxCheckBox.Checked)
				{
					args.IsValid = true;
					return;
				}
			}
			foreach (RepeaterItem rI2 in uxPropertiesRepeater.Items)
			{
				CheckBox uxCheckBox = (CheckBox)rI2.FindControl("uxCheckBox");
				if (uxCheckBox.Checked)
				{
					args.IsValid = true;
					return;
				}
			}
			foreach (RepeaterItem rI2 in uxSiteMapRepeater.Items)
			{
				CheckBox uxCheckBox = (CheckBox)rI2.FindControl("uxCheckBox");
				if (uxCheckBox.Checked)
				{
					args.IsValid = true;
					return;
				}
			}
		}
	}
}