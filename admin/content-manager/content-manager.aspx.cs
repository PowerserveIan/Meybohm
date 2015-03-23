using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.ContentManager;
using Classes.Media352_MembershipProvider;
using Classes.SiteLanguages;
using Settings = Classes.ContentManager.Settings;

namespace ContentManager2.Admin
{
	public partial class ContentManager : System.Web.UI.Page
	{
		protected int NumberOfPagesViewable;
		protected int NumberOfTemplatesViewable;
		protected List<CMPageRegion> UnapprovedRegions = new List<CMPageRegion>();

		#region properties

		protected bool ShowDeleted
		{
			get
			{
				if (ViewState["ContentManagerShowDeleted"] == null)
					return false;
				return (bool)ViewState["ContentManagerShowDeleted"];
			}
			set { ViewState["ContentManagerShowDeleted"] = value; }
		}

		protected bool IsDeveloper
		{
			get { return User.IsInRole("Admin") || User.IsInRole("CMS Admin") || User.IsInRole("CMS Content Integrator"); }
		}

		protected int MicroSiteID
		{
			get { return Session["MicroSiteID"] != null ? Convert.ToInt32(Session["MicroSiteID"]) : 0; }
			set { Session["MicroSiteID"] = value; }
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

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			uxTemplatesRepeater.ItemDataBound += uxTemplatesRepeater_ItemDataBound;
			uxTemplatesRepeater.DataBinding += uxTemplatesRepeater_DataBinding;
			uxMicrositeList.SelectedIndexChanged += uxMicrositeList_SelectedIndexChanged;
			uxPublish.Click += uxPublish_Click;
			uxUnpublish.Click += uxUnpublish_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			UnapprovedRegions = CMPageRegion.CMPageRegionGetByNeedsApproval(true);
			if (!IsPostBack)
			{
				if (Settings.EnableMicrosites)
				{
					uxMicrositePlaceHolder.Visible = true;
					uxMicrositeList.Visible = false;
					if (IsDeveloper || User.IsInRole("Microsite Admin"))
					{
						List<CMMicrosite> tempMicrositeList = IsDeveloper ? CMMicrosite.CMMicrositeGetByActive(true) : CMMicrosite.GetMicrositesByUserID(Helpers.GetCurrentUserID()).Where(c => c.Active).ToList();

						if (IsDeveloper || User.IsInRole("CMS Page Manager"))
						{
							uxMicrositeList.Items.Add(new ListItem("Main Site"));
							uxMicrositeList.AppendDataBoundItems = true;
						}
						if (IsDeveloper)
						{
							uxMicrositeList.Items.Add(new ListItem("Microsite Default"));
							uxMicrositeList.Visible = true;
						}

						if (tempMicrositeList.Count > 1)
						{
							uxMicrositeList.Visible = true;

							uxMicrositeList.DataSource = tempMicrositeList;
							uxMicrositeList.DataTextField = "Name";
							uxMicrositeList.DataValueField = "CMMicroSiteID";
							if (!IsDeveloper && MicroSiteID == 0)
								MicroSiteID = tempMicrositeList[0].CMMicroSiteID;
						}
						else if (tempMicrositeList.Count == 1)
						{
							if (IsDeveloper || User.IsInRole("CMS Page Manager"))
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
							uxTemplatesRepeater.Visible = false;
							return;
						}
					}

					CMMicrosite micrositeEntity = MicroSiteID > 0 ? CMMicrosite.GetByID(MicroSiteID) : null;

					if (MicroSiteID > 0 && (micrositeEntity == null || !micrositeEntity.Active))
						MicroSiteID = 0;

					if (micrositeEntity != null && micrositeEntity.Active)
					{
						uxPublish.Visible = !micrositeEntity.Published;
						uxUnpublish.Visible = micrositeEntity.Published;
						publishPopupLink.Visible = true;
					}
				}
				else if (!IsDeveloper && User.IsInRole("Microsite Admin"))
					Response.Redirect("~/admin");

				DataBind();
				if (Settings.EnableMicrosites && uxMicrositeList.Visible)
				{
					if (MicroSiteID > 0)
						uxMicrositeList.SelectedValue = MicroSiteID.ToString();
					else if (MicroSiteID == -1)
						uxMicrositeList.SelectedValue = uxMicrositeList.Items.FindByText("Microsite Default").Value;
					uxMicroSiteName.Text = uxMicrositeList.SelectedItem.Text;
				}
				uxLanguageToggle.Visible = Settings.EnableMultipleLanguages;
				uxMicrositeLanguagePH.Visible = uxLanguageToggle.Visible || uxMicrositePlaceHolder.Visible;
			}
		}

		public void ShowDeleted_OnClick(object s, EventArgs e)
		{
			ShowDeleted = true;
			uxTemplatesRepeater.DataBind();
		}

		public void HideDeleted_OnClick(object s, EventArgs e)
		{
			ShowDeleted = false;
			uxTemplatesRepeater.DataBind();
		}

		public void uxTemplatesRepeater_OnItemCommand(object s, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Add")
			{
				string microsite = MicroSiteID != 0 ? "&MicrositeId=" + MicroSiteID : "";
				Response.Redirect(string.Format("~/admin/content-manager/content-manager-page.aspx?TemplateId={0}{1}", e.CommandArgument as string, microsite));
			}
			else if (e.CommandName == "Edit")
			{
				Response.Redirect("~/admin/content-manager/ContentManagerTemplate.aspx?id=" + e.CommandArgument);
			}
		}

		public void uxPagesRepeater_OnItemCommand(object s, RepeaterCommandEventArgs e)
		{
			int id = int.Parse((string)e.CommandArgument);
			switch (e.CommandName)
			{
				case "Restore":
					DeleteOrRestore(id, false);
					uxTemplatesRepeater.DataBind();
					break;
				case "Delete":
					DeleteOrRestore(id, true);
					uxTemplatesRepeater.DataBind();
					break;
				case "PermaDelete":
					CMPage.GetByID(id).Delete();
					uxTemplatesRepeater.DataBind();
					break;
			}
		}

		protected string GetLinkForPageByCMPageID(int id)
		{
			return string.Format("~/{0}{1}", MicroSiteID > 0 ? CMMicrosite.GetByID(MicroSiteID).Name.ToLower().Replace(" ", "-") + "/" : "", GetFileName(id));
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
			DataBind();
			CMMicrosite micrositeEntity = CMMicrosite.GetByID(MicroSiteID);

			if (micrositeEntity != null && micrositeEntity.Active)
			{
				uxPublish.Visible = !micrositeEntity.Published;
				uxUnpublish.Visible = micrositeEntity.Published;
			}
			else
				uxPublish.Visible = uxUnpublish.Visible = false;
			publishPopupLink.Visible = MicroSiteID > 0;
		}

		private void uxTemplatesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Repeater uxPagesRepeater = (Repeater)e.Item.FindControl("uxPagesRepeater");
				uxPagesRepeater.DataBinding += uxPagesRepeater_DataBinding;
				uxPagesRepeater.DataBind();
			}
		}

		private void uxTemplatesRepeater_DataBinding(object sender, EventArgs e)
		{
			List<CMTemplate> templateList;
			if (MicroSiteID > 0 || MicroSiteID == -1)
				templateList = CMTemplate.CMTemplateGetByMicrositeEnabled(true).OrderBy(t => t.Name).ToList();
			else
				templateList = CMTemplate.CMTemplateGetByMicrositeEnabled(false).OrderBy(t => t.Name).ToList();
			uxTemplatesRepeater.DataSource = templateList;
			NumberOfTemplatesViewable = templateList.Count;
		}

		private void uxPagesRepeater_DataBinding(object sender, EventArgs e)
		{
			Repeater uxPagesRepeater = (Repeater)sender;
			int templateid = int.Parse(((Label)uxPagesRepeater.Parent.FindControl("templateid")).Text);
			List<CMPage> pageList = Settings.EnableMultipleLanguages ? CMPage.CMPageGetByCMTemplateIDAndLanguageID(templateid, LanguageID) : CMPage.CMPageGetByCMTemplateIDAndLanguageID(templateid);

			//Role Filter
			if (Settings.EnableCMPageRoles && !IsDeveloper && (!Page.User.IsInRole("Microsite Admin") || MicroSiteID <= 0))
			{
				pageList = pageList.Where(p => (from r in CMPageRole.GetAll()
												join ur in UserRole.UserRoleGetByUserID(Helpers.GetCurrentUserID()) on r.RoleID equals ur.RoleID
												where r.Editor
												select r.CMPageID).Contains(p.OriginalCMPageID.HasValue ? p.OriginalCMPageID.Value : p.CMPageID)).ToList();
			}

			//Microsite Filter
			pageList = pageList.Where(p => ((MicroSiteID == 0 && !p.CMMicrositeID.HasValue && !p.MicrositeDefault) || (p.CMMicrositeID.HasValue && MicroSiteID > 0 && p.CMMicrositeID.Value == MicroSiteID) || (p.MicrositeDefault && MicroSiteID == -1))).OrderBy(p => p.Title).ToList();

			//Remove duplicates
			List<CMPage> tempPageList = new List<CMPage>();
			tempPageList.AddRange(pageList);
			foreach (CMPage pageEntity in tempPageList)
			{
				if (pageEntity.OriginalCMPageID.HasValue)
					pageList.RemoveAll(p => p.CMPageID == pageEntity.OriginalCMPageID.Value);
			}

			uxPagesRepeater.DataSource = pageList.Where(p => (p.Deleted == ShowDeleted && !p.EditorDeleted.HasValue) || (p.EditorDeleted.HasValue && p.EditorDeleted.Value == ShowDeleted));

			Image uxNeedApprovalIcon = (Image)uxTemplatesRepeater.Controls[0].Controls[0].FindControl("uxNeedApprovalIcon");
			uxNeedApprovalIcon.Visible = pageList.Count(p => p.Deleted && (p.NeedsApproval || p.OriginalCMPageID.HasValue) && (!p.EditorDeleted.HasValue || p.EditorDeleted.Value) || (p.EditorDeleted.HasValue && p.EditorDeleted.Value)) > 0;

			NumberOfPagesViewable = pageList.Count(p => (p.Deleted == ShowDeleted && !p.EditorDeleted.HasValue) || (p.EditorDeleted.HasValue && p.EditorDeleted.Value == ShowDeleted));
			uxPagesRepeater.Parent.FindControl("uxNoPagesToEdit").Visible = User.IsInRole("CMS Page Manager") && pageList.Count == 0;
		}

		private void DeleteOrRestore(int id, bool deleted)
		{
			CMPage thePage = CMPage.GetByID(id);
			if (Settings.MicrositeDefaultChangesAffectExistingMicrosites && MicroSiteID == -1)
				CMPage.CMPageGetByFileName(thePage.FileName).ForEach(p =>
				{
					int userID = Helpers.GetCurrentUserID();
					p.UserID = userID;
					if (!CMSHelpers.HasFullCMSPermission())
					{
						p.EditorDeleted = deleted;
						if (!CMSHelpers.PageHasBeenEditedByUserBefore(thePage.EditorUserIDs, userID))
						{
							p.EditorUserIDs = (p.EditorUserIDs + "," + userID.ToString()).TrimStart(',');
							CMSHelpers.SendApprovalEmailAlerts(p, null, userID, false, User.IsInRole("Admin") || User.IsInRole("CMS Admin"));
						}
					}
					else
					{
						p.Deleted = deleted;
						p.EditorDeleted = null;
					}

					if (p.Deleted == p.EditorDeleted)
						p.EditorDeleted = null;

					p.Save();
				});
			else
			{
				int userID = Helpers.GetCurrentUserID();

				thePage.UserID = userID;
				if (!CMSHelpers.HasFullCMSPermission())
				{
					thePage.EditorDeleted = deleted;
					if (!CMSHelpers.PageHasBeenEditedByUserBefore(thePage.EditorUserIDs, userID))
					{
						thePage.EditorUserIDs = (thePage.EditorUserIDs + "," + userID).TrimStart(',');
						CMSHelpers.SendApprovalEmailAlerts(thePage, null, userID, false, User.IsInRole("Admin") || User.IsInRole("CMS Admin"));
					}
				}
				else
				{
					thePage.Deleted = deleted;
					thePage.EditorDeleted = null;
				}

				if (thePage.Deleted == thePage.EditorDeleted)
					thePage.EditorDeleted = null;

				thePage.Save();
			}
			CMSHelpers.ClearCaches();

			if (deleted)
			{
				List<SMItem> allSMs = SMItem.GetAll();
				Action<List<SMItem>, SMItem> del = null;
				del = (smColl, smItem) =>
						{
							smColl.Where(s => s.SMItemParentID == smItem.SMItemID).ToList().ForEach(s =>
							{
								del(smColl, s);
								if (Settings.EnableApprovals && !CMSHelpers.HasFullCMSPermission())
								{
									s.EditorDeleted = true;
									s.Save();
								}
								else
									s.Delete();
							});
							if (Settings.EnableApprovals && !CMSHelpers.HasFullCMSPermission())
							{
								smItem.EditorDeleted = true;
								smItem.Save();
							}
							else
								smItem.Delete();
						};
				(from s in allSMs where s.CMPageID == thePage.CMPageID || s.CMPageID == thePage.OriginalCMPageID select s).ToList().ForEach(sm => del(allSMs, sm));
			}
		}

		private void uxUnpublish_Click(object sender, EventArgs e)
		{
			CMMicrosite micrositeEntity = CMMicrosite.GetByID(MicroSiteID);
			micrositeEntity.Published = false;
			micrositeEntity.Save();
			uxUnpublish.Visible = false;
			uxPublish.Visible = true;
			CMSHelpers.ClearCaches();
		}

		private void uxPublish_Click(object sender, EventArgs e)
		{
			CMMicrosite micrositeEntity = CMMicrosite.GetByID(MicroSiteID);
			micrositeEntity.Published = true;
			micrositeEntity.Save();
			uxUnpublish.Visible = true;
			uxPublish.Visible = false;
			CMSHelpers.ClearCaches();
		}

		#region GetFileName()

		private static string GetFileName(int id)
		{
			return CMPage.GetByID(id).FileName;
		}

		#endregion
	}
}