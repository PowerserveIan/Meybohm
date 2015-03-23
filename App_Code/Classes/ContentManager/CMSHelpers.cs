using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Caching;
using BaseCode;
using Classes.Media352_MembershipProvider;
using Classes.SiteLanguages;

namespace Classes.ContentManager
{
	public static class CMSHelpers
	{
		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		/// <summary>
		/// Is User in the "Admin" or "CMS Admin" role
		/// </summary>
		public static bool HasFullCMSPermission()
		{
			return (HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("CMS Admin"));
		}

		public static CMPage GetCurrentRequestCMSPage(HttpContext requestContext = null)
		{
			if (requestContext == null)
				requestContext = HttpContext.Current;
			if (requestContext == null || requestContext.Items["CMSPage"] == "null")
				return null;
			if (requestContext.Items["CMSPage"] == null)
			{
				string fileName = Helpers.GetFileName(requestContext);
				CMMicrosite micrositeEntity = GetCurrentRequestCMSMicrosite();
				CMPage cmPage = CMPage.CMPageGetByFileName(fileName).FirstOrDefault(c => micrositeEntity == null || c.CMMicrositeID == micrositeEntity.CMMicroSiteID);
				if (cmPage == null && fileName.ToLower().EndsWith(".aspx"))
					cmPage = CMPage.CMPageGetByFileName(fileName.ToLower().Replace(".aspx", "")).FirstOrDefault(c => micrositeEntity == null || c.CMMicrositeID == micrositeEntity.CMMicroSiteID);
				//TODO: Showcase fix, would love to make this not so stupid
				if (fileName.Equals("showcase.aspx", StringComparison.OrdinalIgnoreCase))
				{
					SEOComponent.SEOData seoEntity = SEOComponent.SEOData.SEODataGetByPageURL("~/" + fileName + "?" + requestContext.Request.QueryString.ToString().Replace("?&", "?").TrimEnd('?').Split('&')[0]).FirstOrDefault();
					if (seoEntity != null && !String.IsNullOrEmpty(seoEntity.FriendlyFilename) && seoEntity.FriendlyFilename.Split('/').Length > 1)
						cmPage = CMPage.CMPageGetByFileName(seoEntity.FriendlyFilename.Split('/')[1]).FirstOrDefault(c => micrositeEntity == null || c.CMMicrositeID == micrositeEntity.CMMicroSiteID);
				}
				if (cmPage != null)
					CMSHelpers.SetCurrentRequestCMSPage(cmPage);
				else
					requestContext.Items["CMSPage"] = "null";
				return cmPage;
			}
			return (CMPage)requestContext.Items["CMSPage"];
		}

		public static void SetCurrentRequestCMSPage(CMPage cmsPage, HttpContext requestContext = null)
		{
			if (requestContext == null)
				requestContext = HttpContext.Current;
			if (requestContext == null)
				return;
			requestContext.Items["CMSPage"] = cmsPage;
		}

		public static CMMicrosite GetCurrentRequestCMSMicrosite(HttpContext requestContext = null)
		{
			if (requestContext == null)
				requestContext = HttpContext.Current;
			if (requestContext == null || requestContext.Items["CMSMicrosite"] == "null")
				return null;
			if (requestContext.Items["CMSMicrosite"] == null)
			{
				string fileName = Helpers.GetFileName(requestContext);
				int? micrositeID = GetCMMicrositeID(fileName, out fileName);
				CMMicrosite microsite = micrositeID.HasValue ? CMMicrosite.GetByID(micrositeID.Value) : null;
				if (microsite != null)
					CMSHelpers.SetCurrentRequestCMSMicrosite(microsite);
				else
					requestContext.Items["CMSMicrosite"] = "null";
				return microsite;
			}
			return (CMMicrosite)requestContext.Items["CMSMicrosite"];
		}

		public static void SetCurrentRequestCMSMicrosite(CMMicrosite micrositeEntity, HttpContext requestContext = null)
		{
			if (requestContext == null)
				requestContext = HttpContext.Current;
			if (requestContext == null)
				return;
			requestContext.Items["CMSMicrosite"] = micrositeEntity;
		}

		public static string GetCurrentRequestCMSPagePathWithMicrosite(HttpContext requestContext = null)
		{
			CMMicrosite microsite = GetCurrentRequestCMSMicrosite(requestContext);
			CMPage cmPage = GetCurrentRequestCMSPage(requestContext);
			if (cmPage == null)
				return string.Empty;
			return (microsite != null ? microsite.Name.ToLower().Replace(" ", "-") + "/" : string.Empty) + cmPage.FileName;
		}

		public static void ClearCaches()
		{
			Helpers.PurgeCacheItems("ContentManager");
		}

		public static List<CMPage> GetCachedCMPages()
		{
			int languageID = Helpers.GetCurrentLanguage().LanguageID;
			string cacheKeyCMPages = "ContentManager_CMPage_AllCMPages_" + languageID;
			List<CMPage> allCMPages = Cache[cacheKeyCMPages] as List<CMPage>;
			if (allCMPages == null)
			{
				allCMPages = Settings.EnableMultipleLanguages ? CMPage.CMPagesGetAllWithCMPageTitle(languageID) : CMPage.GetAll();
				Cache.Store(cacheKeyCMPages, allCMPages);
			}
			return allCMPages;
		}

		public static List<SMItem> GetCachedSMItems(int? micrositeID)
		{
			return GetCachedSMItems(micrositeID, null);
		}

		public static List<SMItem> GetCachedSMItems(int? micrositeID, int? languageID)
		{
			string cacheKeySMItems = "ContentManager_SMItem_" + (micrositeID.HasValue ? "_" + micrositeID.Value : "") + (languageID.HasValue ? "_" + languageID.Value : "");
			List<SMItem> allSMItems = Cache[cacheKeySMItems] as List<SMItem>;

			if (allSMItems == null)
			{
				//taking advantage of paging filters to allow language filtering. 
				SMItem.Filters filterList = new SMItem.Filters();
				if (languageID.HasValue)
					filterList.FilterSMItemLanguageID = languageID.Value.ToString();

				if (micrositeID.HasValue && micrositeID.Value == -1)
					filterList.FilterSMItemMicrositeDefault = true.ToString();
				else if (micrositeID.HasValue && micrositeID.Value > 0)
				{
					filterList.FilterSMItemCMMicrositeID = micrositeID.Value.ToString();
					filterList.FilterSMItemMicrositeDefault = false.ToString();
				}
				else
					filterList.FilterSMItemMicrositeDefault = false.ToString();

				allSMItems = SMItem.SMItemPage(0, 0, "", "", true, filterList);

				if (!micrositeID.HasValue)
					allSMItems = allSMItems.Where(s => s.CMMicrositeID == null).ToList();
				Cache.Store(cacheKeySMItems, allSMItems);
			}
			return allSMItems;
		}

		/// <summary>
		/// Sets the SMParentItemID of children of the passed in SMItemID to be the parent of the SMItemID (NULL if it had no parent)
		/// </summary>
		/// <param name="smItemID"></param>
		/// <param name="editorDelete">Deleted by a user without full CMS permission</param>
		public static void UpdateSMItemsOnParentItemDelete(int smItemID, bool editorDelete)
		{
			using (Entities entity = new Entities())
			{
				entity.CMS_UpdateSMItemsOnParentItemDelete(smItemID, editorDelete);
			}
		}

		public static int? GetCMMicrositeID(string inFilename, out string filename)
		{
			if (Cache[inFilename + "_outfilename"] != null)
			{
				filename = (string)Cache[inFilename + "_outfilename"];
				if (Cache[inFilename + "_micrositeID"] == null)
					return null;
				return Convert.ToInt32(Cache[inFilename + "_micrositeID"]);
			}
			int? micrositeID = null;
			filename = inFilename;
			if (inFilename.Contains("/"))
			{
				List<CMMicrosite> microsites = CMMicrosite.CMMicrositeGetByName(inFilename.Split('/')[0].Replace("-", " "));
				if (microsites.Count == 1)
					micrositeID = microsites[0].CMMicroSiteID;
				filename = inFilename.Split('/')[1];
			}
			else
			{
				if (Settings.EnableMicrosites && !GetCachedCMPages().Any(c => c.FileName.Equals(inFilename, StringComparison.OrdinalIgnoreCase) && (c.CMMicrositeID == null || c.CMMicrositeID == -1)))
				{
					CMMicrosite micrositeEntity = CMMicrosite.CMMicrositeGetByName(inFilename.Replace("-", " ")).FirstOrDefault();
					if (micrositeEntity != null)
					{
						micrositeID = micrositeEntity.CMMicroSiteID;
						filename = "Home.aspx";
					}
				}
			}
			if (micrositeID.HasValue && String.IsNullOrEmpty(filename))
				filename = "Home.aspx";
			Cache[inFilename + "_outfilename"] = filename;
			if (micrositeID != null)
				Cache[inFilename + "_micrositeID"] = micrositeID;
			return micrositeID;
		}

		public static bool CanUserManagePage()
		{
			CMPage currentPage = GetCurrentRequestCMSPage();
			CMMicrosite micrositeEntity = GetCurrentRequestCMSMicrosite();
			bool canManage = (HttpContext.Current.User.IsInRole("Microsite Admin") && (micrositeEntity != null && CMMicrositeUser.CMMicrositeUserGetByCMMicrositeID(micrositeEntity.CMMicroSiteID).Exists(m => m.UserID == Helpers.GetCurrentUserID())));
			if (HttpContext.Current.User.IsInRole("CMS Page Manager") && !canManage)
			{
				if (currentPage != null)
				{
					List<CMPageRole> pageRoles = CMPageRole.CMPageRolePage(0, 0, "", "", true, new CMPageRole.Filters { FilterCMPageRoleCMPageID = currentPage.CMPageID.ToString(), FilterCMPageRoleEditor = true.ToString() });
					if (pageRoles.Any(role => HttpContext.Current.User.IsInRole(Role.GetByID(role.RoleID).Name)))
						canManage = true;
				}
			}
			return canManage;
		}

		/// <summary>
		/// Takes in the ID of the current CMPage and returns whether or not 
		/// the currently logged in user may access that page (determined by entries in CMPageRole
		/// </summary>
		/// <param name="cmPageID"></param>
		/// <returns></returns>
		public static bool CanUserAccessPage(int cmPageID)
		{
			bool authorized = true;
			if (HasFullCMSPermission())
				return true;

			CMPageRole.Filters filterList = new CMPageRole.Filters();
			filterList.FilterCMPageRoleCMPageID = cmPageID.ToString();
			filterList.FilterCMPageRoleEditor = false.ToString();
			List<CMPageRole> pageRoles = CMPageRole.CMPageRolePage(0, 0, "", "", true, filterList);

			CMPage thePage = CMPage.GetByID(cmPageID);
			if (thePage.NeedsApproval && pageRoles.Count == 0)
				return false;
			if (pageRoles.Count > 0)
			{
				authorized = false;
				if (HttpContext.Current.User.Identity.IsAuthenticated)
				{
					List<UserRole> userRoles = UserRole.UserRoleGetByUserID(Helpers.GetCurrentUserID());
					if (pageRoles.Any(pageRole => userRoles.Exists(r => r.RoleID == pageRole.RoleID && (!thePage.NeedsApproval || (thePage.NeedsApproval && pageRole.Editor)))))
						authorized = true;
				}
			}
			return authorized;
		}

		public static bool IsMenuItemParentByPageRoles(int smItemId)
		{
			int? userID = Helpers.GetCurrentUserID();
			if (userID == 0)
				userID = null;
			using (Entities entity = new Entities())
			{
				return entity.CMS_IsMenuItemParentByPageRoles(smItemId, userID).FirstOrDefault().Value;
			}
		}

		/// <summary>
		/// Sets the currentversion = false for all microsite pages that inherit from the default page.
		/// Called whenever a microsite default page gets changed.
		/// </summary>
		/// <param name="FileName"></param>
		public static void UpdateMicrositesWithDefaultContent(string FileName)
		{
			using (Entities entity = new Entities())
			{
				entity.CMS_UpdateMicrositesWithDefaultContent(FileName);
			}
		}

		public static bool PageHasBeenEditedByUserBefore(string editorUserIDs, int userID)
		{
			if (String.IsNullOrEmpty(editorUserIDs))
				return false;
			if (editorUserIDs.Contains(userID + ","))
				return true;
			if (!editorUserIDs.Contains(",") && editorUserIDs.Equals(userID.ToString()))
				return true;
			if (editorUserIDs.EndsWith("," + userID))
				return true;
			return false;
		}

		public static void SendApprovalEmailAlerts(CMPage editedPage, CMPageRegion region, int userID, bool content, bool isAdmin)
		{
			SendApprovalEmailAlerts(editedPage, region, userID, content, isAdmin, null, null);
		}

		public static void SendApprovalEmailAlerts(CMPage editedPage, CMPageRegion region, int userID, bool content, bool isAdmin, bool? approval, int? languageID)
		{
			if (Settings.EnableApprovals && Settings.SendApprovalEmails)
			{
				MailMessage email = new MailMessage();
				SmtpClient client = new SmtpClient();
				CMPage originalPage = editedPage.OriginalCMPageID.HasValue ? CMPage.GetByID(editedPage.OriginalCMPageID.Value) : null;
				string pageName = string.Empty;
				if (languageID.HasValue)
				{
					CMPageTitle titleEntity = null;
					//If Denied, take the original page title
					if (originalPage != null && approval.HasValue && !approval.Value)
						titleEntity = CMPageTitle.CMPageTitleGetByCMPageIDAndLanguageID(originalPage.CMPageID, languageID.Value).FirstOrDefault();
					//If not approve/deny, take the current displayed unapproved page title
					if (titleEntity == null)
						titleEntity = CMPageTitle.CMPageTitleGetByCMPageIDAndLanguageID(editedPage.CMPageID, languageID.Value).FirstOrDefault();
					if (titleEntity != null)
						pageName = titleEntity.Title;
				}
				if (String.IsNullOrEmpty(pageName))
				{
					if (originalPage != null && approval.HasValue && !approval.Value)
						pageName = originalPage.Title;
					else
						pageName = editedPage.Title;
				}

				Language languageEntity = null;
				if (languageID.HasValue)
					languageEntity = Language.GetByID(languageID.Value);

				if (!approval.HasValue)
				{
					User userEntity = User.GetByID(userID);
					//Don't send Admin Email if Admin is the one who edited
					if (!isAdmin)
					{
						//Send Admin Email
						email.From = new MailAddress(Globals.Settings.FromEmail);
						if (!String.IsNullOrEmpty(Settings.ApprovalAdminEmailAddresses))
						{
							foreach (string s in Settings.ApprovalAdminEmailAddresses.Split(';'))
							{
								email.To.Add(new MailAddress(s));
							}
						}
						else //Send to all Admins
						{
							foreach (UserRole admin in UserRole.UserRoleGetWithUserByRoleName("Admin"))
							{
								email.To.Add(new MailAddress(admin.User.Email, admin.User.Name));
							}
						}

						email.IsBodyHtml = true;
						email.Body = userEntity.Name + " has updated the " + (languageEntity != null ? languageEntity.Culture + " " : "") + (content ? "content" : "properties") + " of <a href=\"" + Helpers.RootPath + (content ? (editedPage.CMMicrositeID.HasValue ? CMMicrosite.GetByID(editedPage.CMMicrositeID.Value).Name + "/" : "") + editedPage.FileName + (languageEntity != null ? "?language=" + languageEntity.CultureName : "") : "admin/content-manager/content-manager-page.aspx?id=" + editedPage.CMPageID + (languageEntity != null ? "&language=" + languageEntity.CultureName : "")) + "\">" + pageName + "</a>";
						email.Subject = Globals.Settings.SiteTitle + " - " + (content ? "Content" : "Page Properties") + " Approval Required";

						client.Send(email);
					}

					//Send Editor Email
					email = new MailMessage();
					email.From = new MailAddress(Globals.Settings.FromEmail);
					if (content && region != null && !String.IsNullOrEmpty(region.EditorUserIDs))
					{
						foreach (string id in region.EditorUserIDs.Split(','))
						{
							if (!id.Equals(userID.ToString()))
							{
								User editor = User.GetByID(Convert.ToInt32(id));
								email.To.Add(new MailAddress(editor.Email, editor.Name));
							}
						}
					}
					else if (!String.IsNullOrEmpty(editedPage.EditorUserIDs))
					{
						foreach (string id in editedPage.EditorUserIDs.Split(','))
						{
							if (!id.Equals(userID.ToString()))
							{
								User editor = User.GetByID(Convert.ToInt32(id));
								email.To.Add(new MailAddress(editor.Email, editor.Name));
							}
						}
					}

					if (email.To.Count > 0)
					{
						email.IsBodyHtml = true;
						email.Body = userEntity.Name + " has updated the " + (languageEntity != null ? languageEntity.Culture + " " : "") + (content ? "content" : "properties") + " of <a href=\"" + Helpers.RootPath + (content ? (editedPage.CMMicrositeID.HasValue ? CMMicrosite.GetByID(editedPage.CMMicrositeID.Value).Name + "/" : "") + editedPage.FileName + (languageEntity != null ? "?language=" + languageEntity.CultureName : "") : "admin/content-manager/content-manager-page.aspx?id=" + editedPage.CMPageID + (languageEntity != null ? "&language=" + languageEntity.CultureName : "")) + "\">" + pageName + "</a>, which you have also edited.  The page is still awaiting approval from an Admin.";
						email.Subject = Globals.Settings.SiteTitle + " - " + (content ? "Content" : "Page Properties") + " Edited";

						client = new SmtpClient();
						client.Send(email);
					}
				}
				else //Approve/Denied
				{
					//Send Editors Email
					email = new MailMessage();
					email.From = new MailAddress(Globals.Settings.FromEmail);
					if (content && region != null && !String.IsNullOrEmpty(region.EditorUserIDs))
					{
						foreach (string id in region.EditorUserIDs.Split(','))
						{
							if (!id.Equals(userID.ToString()))
							{
								User editor = User.GetByID(Convert.ToInt32(id));
								email.To.Add(new MailAddress(editor.Email, editor.Name));
							}
						}
					}
					else if (!String.IsNullOrEmpty(editedPage.EditorUserIDs))
					{
						foreach (string id in editedPage.EditorUserIDs.Split(','))
						{
							if (!id.Equals(userID.ToString()))
							{
								User editor = User.GetByID(Convert.ToInt32(id));
								email.To.Add(new MailAddress(editor.Email, editor.Name));
							}
						}
					}

					if (email.To.Count > 0)
					{
						email.IsBodyHtml = true;
						email.Body = "An Admin has " + (approval.Value ? "approved" : "denied") + " the " + (languageEntity != null ? languageEntity.Culture + " " : "") + (content ? "content" : "properties") + " changes to <a href=\"" + Helpers.RootPath + (content ? (editedPage.CMMicrositeID.HasValue ? CMMicrosite.GetByID(editedPage.CMMicrositeID.Value).Name + "/" : "") + editedPage.FileName + (languageEntity != null ? "?language=" + languageEntity.CultureName : "") : "admin/content-manager/content-manager-page.aspx?id=" + editedPage.CMPageID + (languageEntity != null ? "&language=" + languageEntity.CultureName : "")) + "\">" + pageName + "</a> that you made.";
						email.Subject = Globals.Settings.SiteTitle + " - " + (content ? "Content" : "Page Properties") + " " + (approval.Value ? "Approved" : "Denied");

						client = new SmtpClient();
						client.Send(email);
					}
				}
			}
		}

		public static void SendSiteMapApprovalEmailAlerts(int userID, int? micrositeID, int? languageID, bool isAdmin, bool? approval)
		{
			if (Settings.EnableApprovals && Settings.SendApprovalEmails)
			{
				MailMessage email = new MailMessage();
				SmtpClient client = new SmtpClient();
				if (!approval.HasValue)
				{
					string key = "ContentManager_SiteMapApprovalEmails_" + userID + "_" + micrositeID + "_" + languageID;
					if (!Settings.EnableCaching || HttpContext.Current == null || HttpContext.Current.Cache == null || HttpContext.Current.Cache[key] == null)
					{
						User userEntity = User.GetByID(userID);
						Language languageEntity = null;
						if (languageID.HasValue)
							languageEntity = Language.GetByID(languageID.Value);
						//Don't send Admin Email if Admin is the one who edited
						if (!isAdmin)
						{
							//Send Admin Email
							email.From = new MailAddress(Globals.Settings.FromEmail);
							if (!String.IsNullOrEmpty(Settings.ApprovalAdminEmailAddresses))
							{
								foreach (string s in Settings.ApprovalAdminEmailAddresses.Split(';'))
								{
									email.To.Add(new MailAddress(s));
								}
							}
							else //Send to all Admins
							{
								foreach (UserRole admin in UserRole.UserRoleGetWithUserByRoleName("Admin"))
								{
									email.To.Add(new MailAddress(admin.User.Email, admin.User.Name));
								}
							}

							email.IsBodyHtml = true;
							email.Body = userEntity.Name + " has updated the" + (languageEntity != null ? " " + languageEntity.Culture : "") + (micrositeID.HasValue ? micrositeID == -1 ? " Microsite Default" : " " + CMMicrosite.GetByID(micrositeID.Value).Name : "") + " <a href=\"" + Helpers.RootPath + "admin/content-manager/sitemap.aspx" + (micrositeID.HasValue ? "?micrositeid=" + micrositeID.Value : "") + (languageEntity != null ? (micrositeID.HasValue ? "&" : "?") + "language=" + languageEntity.CultureName : "") + "\">Sitemap</a>";
							email.Subject = Globals.Settings.SiteTitle + " - Sitemap Approval Required";

							client.Send(email);
						}

						//Send Editor Email
						email = new MailMessage();
						email.From = new MailAddress(Globals.Settings.FromEmail);

						List<SMItemUser> allEditors;
						if (micrositeID.HasValue && micrositeID > 0)
							allEditors = SMItemUser.SMItemUserGetByCMMicrositeID(micrositeID);
						else if (micrositeID.HasValue && micrositeID == -1)
							allEditors = SMItemUser.SMItemUserGetByMicrositeDefault(true);
						else
							allEditors = SMItemUser.GetAll().Where(s => s.CMMicrositeID == null && !s.MicrositeDefault).ToList();

						if (languageID.HasValue)
							allEditors = allEditors.Where(s => s.LanguageID == languageID.Value).ToList();

						foreach (SMItemUser id in allEditors)
						{
							if (id.UserID != userID)
							{
								User editor = User.GetByID(id.UserID);
								email.To.Add(new MailAddress(editor.Email, editor.Name));
							}
						}

						if (email.To.Count > 0)
						{
							email.IsBodyHtml = true;
							email.Body = userEntity.Name + " has updated the" + (languageEntity != null ? " " + languageEntity.Culture : "") + (micrositeID.HasValue ? micrositeID == -1 ? " Microsite Default" : " " + CMMicrosite.GetByID(micrositeID.Value).Name : "") + " <a href=\"" + Helpers.RootPath + "admin/content-manager/sitemap.aspx" + (micrositeID.HasValue ? "?micrositeid=" + micrositeID.Value : "") + (languageEntity != null ? (micrositeID.HasValue ? "&" : "?") + "language=" + languageEntity.CultureName : "") + "\">Sitemap</a>, which you have also edited.  The Sitemap is still awaiting approval from an Admin.";
							email.Subject = Globals.Settings.SiteTitle + " - Sitemap Edited";

							client = new SmtpClient();
							client.Send(email);
						}
						//Only send email alerts every 10 minutes per user that changes sitemap
						if (Settings.EnableCaching && HttpContext.Current != null && HttpContext.Current.Cache != null)
							HttpContext.Current.Cache.Insert(key, true, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
					}
				}
				else //Approve/Denied
				{
					//Send Editors Email
					email = new MailMessage();
					email.From = new MailAddress(Globals.Settings.FromEmail);

					List<SMItemUser> allEditors;
					if (micrositeID.HasValue && micrositeID > 0)
						allEditors = SMItemUser.SMItemUserGetByCMMicrositeID(micrositeID);
					else if (micrositeID.HasValue && micrositeID == -1)
						allEditors = SMItemUser.SMItemUserGetByMicrositeDefault(true);
					else
						allEditors = SMItemUser.GetAll().Where(s => s.CMMicrositeID == null && !s.MicrositeDefault).ToList();

					if (languageID.HasValue)
						allEditors = allEditors.Where(s => s.LanguageID == languageID.Value).ToList();

					foreach (SMItemUser id in allEditors)
					{
						if (id.UserID != userID)
						{
							User editor = User.GetByID(id.UserID);
							email.To.Add(new MailAddress(editor.Email, editor.Name));
						}
					}

					SMItemUser.DeleteAll(micrositeID, languageID);

					if (email.To.Count > 0)
					{
						Language languageEntity = null;
						if (languageID.HasValue)
							languageEntity = Language.GetByID(languageID.Value);

						email.IsBodyHtml = true;
						email.Body = "An Admin has " + (approval.Value ? "approved" : "denied") + " the changes to the" + (languageEntity != null ? " " + languageEntity.Culture : "") + (micrositeID.HasValue ? micrositeID == -1 ? " Microsite Default" : " " + CMMicrosite.GetByID(micrositeID.Value).Name : "") + " <a href=\"" + Helpers.RootPath + "admin/content-manager/sitemap.aspx" + (micrositeID.HasValue ? "?micrositeid=" + micrositeID.Value : "") + (languageEntity != null ? (micrositeID.HasValue ? "&" : "?") + "language=" + languageEntity.CultureName : "") + "\">Sitemap</a> that you made.";
						email.Subject = Globals.Settings.SiteTitle + " - Sitemap " + (approval.Value ? "Approved" : "Denied");

						client = new SmtpClient();
						client.Send(email);
					}
				}
			}
		}
	}
}