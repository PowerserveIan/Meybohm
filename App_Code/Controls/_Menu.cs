using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using BaseCode;

namespace Classes.ContentManager
{
	public class Menu : UserControl
	{
		protected List<CMPage> allCMPages;
		protected List<SMItem> allSMItems;
		private int m_CurrentLanguageID;

		public enum MenuMode
		{
			FullSiteMap,
			CurrentSection,
			TopLevelOnly
		}

		/// <summary>
		/// Allows the simulating of another page's menu by passing it in here
		/// </summary>
		public string FileName = string.Empty;

		/// <summary>
		/// Text will be appended to links if this item has sub items
		/// </summary>
		public string SubitemToken = string.Empty;

		/// <summary>
		/// Class will be applied to the top level menu item for the current page
		/// </summary>
		public string CurrentSectionClass { get; set; }

		/// <summary>
		/// Class will be applied to the first LI in the main navigation
		/// </summary>
		public string FirstLIClass { get; set; }

		/// <summary>
		/// Class will be applied to anchor tags at the top level of navigation
		/// </summary>
		public string MainAnchorClass { get; set; }

		/// <summary>
		/// Class will be applied to anchor tags at all sublevels of navigation
		/// </summary>
		public string SubAnchorClass { get; set; }

		/// <summary>
		/// Class will be applied to list tags at the top level of navigation
		/// </summary>
		public string MainListClass { get; set; }

		/// <summary>
		/// Class will be applied to list tags at all sublevels of navigation
		/// </summary>
		public string SubListClass { get; set; }

		/// <summary>
		/// Class will be applied to each unordered list surrounding subitems
		/// </summary>
		public string UnorderedListClass { get; set; }

		/// <summary>
		/// Clearing this will append a class of lastLi to each of the final list items
		/// </summary>
		public bool SuppressLast { get; set; }

		/// <summary>
		/// Show the entire menu structure for the current page
		/// </summary>
		public bool BreakoutCurrentPage { get; set; }

		/// <summary>
		/// Set this to have the menu only pull from microsites
		/// </summary>
		public bool MicrositeMenu { get; set; }

		/// <summary>
		/// The menu will start with the current page and show only the descendants of the current page
		/// </summary>
		public bool CurrentSectionShowOnlyCurrentPageAndBelow { get; set; }

		/// <summary>
		/// Specify which filenames should be use as roots of the menu structure
		/// </summary>
		public String RootFilenames { get; set; }

		/// <summary>
		/// FullSiteMap will show all pages that are entered into the sitemap
		/// CurrentSection will show the parent menu item and all of its children
		/// TopLevelOnly will show only the top level menu items
		/// </summary>
		public MenuMode Mode { get; set; }

		public bool ShowSubPagesOfCurrentPage { get; set; }

		public bool AlwaysShowSecondLevelNav { get; set; }

		public bool? NewHomes { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			allSMItems = new List<SMItem>();
			CMPage currentPage = CMSHelpers.GetCurrentRequestCMSPage();
			FileName = String.IsNullOrEmpty(FileName) ? (currentPage != null ? currentPage.FileName : Helpers.GetFileName()) : FileName;
			string micrositeName = string.Empty;
			CMMicrosite micrositeEntity = CMSHelpers.GetCurrentRequestCMSMicrosite();
			if (Settings.EnableMicrosites && MicrositeMenu)
			{
				if (micrositeEntity != null)
					micrositeName = micrositeEntity.Name.ToLower().Replace(" ", "-");
			}

			allCMPages = CMSHelpers.GetCachedCMPages();

			if (currentPage != null && currentPage.MicrositeDefault)
				allSMItems = SMItem.SMItemGetByMicrositeDefault(true);
			else
				//Must AddRange here in order to eliminate updating the cache with "AdditionalSMItems"
				allSMItems.AddRange(CMSHelpers.GetCachedSMItems((micrositeEntity != null ? (int?)micrositeEntity.CMMicroSiteID : null)).Where(s => s.NewHomes == NewHomes));
			allSMItems.RemoveAll(s => s.NeedsApproval || s.OriginalSMItemID.HasValue);
			m_CurrentLanguageID = Settings.EnableMultipleLanguages ? Helpers.GetCurrentLanguage().LanguageID : Helpers.GetDefaultLanguageID();
			if (Settings.EnableMultipleLanguages && Settings.MultilingualManageSiteMapsIndividually)
				allSMItems.RemoveAll(s => s.LanguageID != m_CurrentLanguageID);
			else
				allSMItems.RemoveAll(s => s.LanguageID != null && s.LanguageID != Helpers.GetDefaultLanguageID());
			List<SMItem> additionalSMItems = MenuPlugin.GetAdditionalSMItems(allSMItems);
			allSMItems.AddRange(additionalSMItems);

			List<SMItem> rootItems = new List<SMItem>();

			Func<int?, bool> shouldRenderSubs = null;

			Func<List<SMItem>, int?, List<int>, List<int>> getChildIDs = null;
			getChildIDs = (sms, parentID, ids) =>
			{
				sms.ForEach(s =>
				{
					if (s.ShowInMenu && s.SMItemID == parentID)
					{
						ids.Add(s.SMItemID);
						ids.AddRange(getChildIDs(sms, s.SMItemParentID, new List<int>()));
					}
				});
				return ids;
			};
			Func<List<SMItem>, int?, List<int>, List<int>> getChildByParentIDs = null;
			getChildByParentIDs = (sms, parentID, ids) =>
			{
				sms.ForEach(s =>
				{
					if (s.ShowInMenu && s.SMItemParentID == parentID)
					{
						ids.Add(s.SMItemID);
						ids.AddRange(getChildByParentIDs(sms, s.SMItemID, new List<int>()));
					}
				});
				return ids;
			};

			string fileNameAndQuery = (FileName + "?" + Request.QueryString.ToString().Replace("filename=" + FileName, "")).Replace("?&", "?").TrimEnd('?');
			if (fileNameAndQuery.ToLower().StartsWith("showcase.aspx?showcaseid=2"))
				fileNameAndQuery = fileNameAndQuery.ToLower().Replace("showcase.aspx?showcaseid=2", "search?").Replace("?&", "?").TrimEnd('?');
			else if (fileNameAndQuery.ToLower().StartsWith("showcase.aspx?showcaseid=4"))
				fileNameAndQuery = fileNameAndQuery.ToLower().Replace("showcase.aspx?showcaseid=4", "search?").Replace("?&", "?").TrimEnd('?');
			Func<List<SMItem>, int?, string> renderMenu = null;
			renderMenu = (menus, Parent) =>
			{
				var sub = menus;
				if (Parent.HasValue)
					sub = menus.Where(m => m.ShowInMenu && m.SMItemParentID == Parent).OrderBy(s => s.Rank).ToList();
				if (sub.Count > 0)
				{
					StringBuilder sb = new StringBuilder();
					if (Parent.HasValue)
						sb.Append(String.Format("<ul{0}>", String.IsNullOrEmpty(UnorderedListClass) ? "" : String.Format(" class=\"{0}\"", UnorderedListClass)));
					int index = 0;

					SMItem topLevelParentSMItem = null;
					if (!Parent.HasValue && !String.IsNullOrEmpty(CurrentSectionClass))
					{
						CMPage currentCMPage = allCMPages.Find(c => c.FileName == FileName);

						if (currentCMPage != null)
							topLevelParentSMItem = allSMItems.Find(s => s.CMPageID == currentCMPage.CMPageID);

						//Don't want the top level of the menu to get this special class if its the currently selected page
						if (topLevelParentSMItem != null && menus.Exists(s => s.SMItemID == topLevelParentSMItem.SMItemID))
							topLevelParentSMItem = null;
						while (topLevelParentSMItem != null && topLevelParentSMItem.SMItemParentID != null)
						{
							topLevelParentSMItem = allSMItems.Find(s => s.SMItemID == topLevelParentSMItem.SMItemParentID.Value);
						}
					}
					foreach (SMItem subMenuItem in sub)
					{
						bool hasSubItems;
						bool duplicatedMenuItem = allSMItems.Any(s => s.CMPageID == subMenuItem.CMPageID && s.SMItemID != subMenuItem.SMItemID && subMenuItem.CMPageID != 0);
						if (additionalSMItems.Exists(s => s.SMItemParentID == subMenuItem.SMItemID))
							hasSubItems = true;
						else if (Settings.HideMembersAreaPagesInMenu && Settings.EnableCMPageRoles && !CMSHelpers.HasFullCMSPermission())
							hasSubItems = CMSHelpers.IsMenuItemParentByPageRoles(subMenuItem.SMItemID);
						else
							hasSubItems = (Parent.HasValue ? menus : allSMItems).Any(s => s.SMItemParentID == subMenuItem.SMItemID && s.ShowInMenu);
						index++;

						string cmPageFileName = string.Empty;

						if (!String.IsNullOrEmpty(subMenuItem.LinkToPage))
							cmPageFileName = subMenuItem.LinkToPage;
						else
						{
							CMPage itemCMPage = allCMPages.Where(c => c.CMPageID == subMenuItem.CMPageID).FirstOrDefault();
							if (itemCMPage != null)
							{
								if (Settings.HideMembersAreaPagesInMenu && Settings.EnableCMPageRoles && !CMSHelpers.CanUserAccessPage(itemCMPage.CMPageID))
									continue;
								cmPageFileName = itemCMPage.FileName;
							}
						}

						string itemDisplayName = subMenuItem.Name;
						if (Settings.EnableMultipleLanguages && !Settings.MultilingualManageSiteMapsIndividually && subMenuItem.LanguageID != m_CurrentLanguageID)
						{
							List<CMPageTitle> titles = CMPageTitle.CMPageTitleGetByCMPageIDAndLanguageID(subMenuItem.CMPageID, m_CurrentLanguageID);
							if (titles.Count > 0)
								itemDisplayName = titles.LastOrDefault().Title;
						}

						string className = Parent.HasValue ? SubListClass : MainListClass;
						if (className == null)
							className = string.Empty;
						if (cmPageFileName.Equals(fileNameAndQuery, StringComparison.OrdinalIgnoreCase) || cmPageFileName.Equals(fileNameAndQuery.ToLower().Replace(".aspx", ""), StringComparison.OrdinalIgnoreCase) || ((cmPageFileName.Equals(FileName, StringComparison.OrdinalIgnoreCase) || cmPageFileName.Equals(FileName.Replace("default.aspx", ""), StringComparison.OrdinalIgnoreCase)) && !allCMPages.Any(c => c.FileName.Equals(fileNameAndQuery, StringComparison.OrdinalIgnoreCase))))
						{
							className += " current";
							if (!Parent.HasValue)
							{
								MasterPage master = Page.Master;
								if (master != null)
								{
									while (master.Master != null)
										master = master.Master;
									((System.Web.UI.HtmlControls.HtmlElement)master.FindControl("htmlEntity")).Attributes["class"] += " parent-" + cmPageFileName.ToLower().Replace(".aspx", "");
								}
							}
						}
						if (index == sub.Count && !SuppressLast)
							className += " lastLi";
						if (topLevelParentSMItem != null && subMenuItem.SMItemID == topLevelParentSMItem.SMItemID)
							className += " " + CurrentSectionClass;
						if (hasSubItems)
							className += " parent";
						if (cmPageFileName.Equals("default.aspx", StringComparison.OrdinalIgnoreCase) || (micrositeEntity != null && (cmPageFileName.Equals("home", StringComparison.OrdinalIgnoreCase) || cmPageFileName.Equals("home.aspx", StringComparison.OrdinalIgnoreCase) || cmPageFileName.Equals("new-homes", StringComparison.OrdinalIgnoreCase))))
							className += " home";
						if (index - 1 == 0 && !String.IsNullOrEmpty(FirstLIClass))
							className += " " + FirstLIClass;
						if (itemDisplayName.ToLower().Contains("<br />"))
							className += " twoLines";
						className = className.Trim();

						sb.Append("<li");

						if (!String.IsNullOrEmpty(className))
							sb.AppendFormat(" class=\"{0}\"", className);

						sb.AppendFormat("><a ");
						if (Parent.HasValue && !String.IsNullOrEmpty(SubAnchorClass))
							sb.AppendFormat("class=\"{0}\" ", SubAnchorClass);
						else if (!Parent.HasValue && !String.IsNullOrEmpty(MainAnchorClass))
							sb.AppendFormat("class=\"{0}\" ", MainAnchorClass);
						sb.AppendFormat("href=\"{0}{1}{2}{3}\"", cmPageFileName.Contains("://") ? "" : VirtualPathUtility.ToAbsolute("~/"), String.IsNullOrEmpty(micrositeName) || cmPageFileName.Contains("://") ? "" : Server.HtmlEncode(micrositeName) + "/", cmPageFileName.Equals("default.aspx", StringComparison.OrdinalIgnoreCase) ? "./" : (!Globals.Settings.RequireASPXExtensions && cmPageFileName.Equals("Home.aspx", StringComparison.OrdinalIgnoreCase) && !String.IsNullOrEmpty(micrositeName) ? "" : Server.HtmlEncode(cmPageFileName)), (duplicatedMenuItem ? (cmPageFileName.Contains("?") ? "&" : "?") + "mID=" + subMenuItem.SMItemID : ""));
						if (cmPageFileName.Contains("://"))
							sb.Append(" target=\"_blank\"");
						//end a start tag
						sb.AppendFormat(">{0}", Server.HtmlEncode(itemDisplayName));
						if (Parent.HasValue && hasSubItems)
							sb.Append(SubitemToken);
						sb.Append("</a>");
						if (hasSubItems && shouldRenderSubs(subMenuItem.SMItemID))
							sb.Append(renderMenu((Parent.HasValue ? menus : allSMItems), subMenuItem.SMItemID));
						sb.Append("</li>");
					}
					if (Parent.HasValue)
						sb.Append("</ul>");

					return sb.ToString();
				}
				return string.Empty;
			};

			if (!String.IsNullOrEmpty(RootFilenames))
			{
				// Add items for the pages in the list to the root
				List<string> fileNames = new List<string>(RootFilenames.Split(','));
				List<CMPage> rootPages = allCMPages.Where(c => fileNames.Contains(c.FileName.ToLower(), StringComparer.OrdinalIgnoreCase)).ToList();
				rootItems.AddRange(allSMItems.Where(s => s.ShowInMenu).Where(s => rootPages.Select(p => p.CMPageID).ToList().Contains(s.CMPageID)).OrderBy(s => s.Rank).ToList());
			}

			switch (Mode)
			{
				case MenuMode.CurrentSection:
					{
						// Add the parent to the current page as the root
						SMItem currentSMItem = null;
						if (String.IsNullOrEmpty(RootFilenames) && currentPage != null)
						{
							int mID;
							currentSMItem = (!String.IsNullOrEmpty(Request.QueryString["mID"]) && Int32.TryParse(Request.QueryString["mID"], out mID) ? allSMItems.Find(s1 => s1.SMItemID == mID) : allSMItems.Find(s1 => s1.CMPageID == currentPage.CMPageID)) ?? allSMItems.Find(s1 => s1.CMPageID == currentPage.CMPageID);
							if (currentSMItem != null)
							{
								if (AlwaysShowSecondLevelNav)
								{
									SMItem parentSMItem = allSMItems.Find(s2 => s2.SMItemID == currentSMItem.SMItemParentID);
									if (parentSMItem != null && parentSMItem.SMItemParentID.HasValue)
										rootItems.Insert(0, allSMItems.Find(s3 => s3.SMItemID == parentSMItem.SMItemParentID));
								}
								if (rootItems.Count == 0)
								{
									if (currentSMItem.SMItemParentID != null && !CurrentSectionShowOnlyCurrentPageAndBelow)
										rootItems.AddRange(allSMItems.Where(s2 => currentSMItem.SMItemParentID == s2.SMItemID).OrderBy(s => s.Rank).ToList());
									else
										rootItems.Add(currentSMItem);
								}
							}
						}
						shouldRenderSubs = ID => { return rootItems.Select(s => s.SMItemID).ToList().Contains(ID ?? 0) ? true : (ShowSubPagesOfCurrentPage && currentSMItem != null && currentSMItem.SMItemID == ID ? true : (AlwaysShowSecondLevelNav ? currentSMItem.SMItemParentID == ID : BreakoutCurrentPage)); };
					}
					break;
				case MenuMode.FullSiteMap:
					{
						if (String.IsNullOrEmpty(RootFilenames)) // Add all zero level items to the root
						{
							if (AlwaysShowSecondLevelNav)
							{
								if (currentPage != null)
								{
									int mID;
									SMItem currentSMItem = null;
									if (!String.IsNullOrEmpty(Request.QueryString["mID"]) && Int32.TryParse(Request.QueryString["mID"], out mID))
										currentSMItem = allSMItems.Find(s1 => s1.SMItemID == mID);
									if (currentSMItem == null)
										currentSMItem = allSMItems.Find(s1 => s1.CMPageID == currentPage.CMPageID);
									if (currentSMItem != null)
									{
										SMItem parentSMItem = allSMItems.Find(s2 => s2.SMItemID == currentSMItem.SMItemParentID);
										while (parentSMItem != null && allSMItems.Any(s3 => s3.SMItemID == parentSMItem.SMItemParentID))
										{
											parentSMItem = allSMItems.Find(s3 => s3.SMItemID == parentSMItem.SMItemParentID);
										}
										rootItems.Insert(0, parentSMItem ?? currentSMItem);
									}
								}
							}
							else
								rootItems.AddRange(allSMItems.Where(s => s.ShowInMenu && !s.SMItemParentID.HasValue).OrderBy(s => s.Rank).ToList());
						}

						shouldRenderSubs = renderSubId =>
						{
							if (!BreakoutCurrentPage) return true;
							return currentPage == null ? false : getChildByParentIDs(allSMItems, renderSubId, new List<int>()).Contains(allSMItems.Where(s => s.ShowInMenu && s.CMPageID == currentPage.CMPageID).Select(s => s.SMItemID).FirstOrDefault()) || allSMItems.Where(s => s.ShowInMenu && s.CMPageID == currentPage.CMPageID).Select(s => s.SMItemID).FirstOrDefault() == renderSubId;
						};
					}
					break;
				case MenuMode.TopLevelOnly:
					{
						if (String.IsNullOrEmpty(RootFilenames)) // Add all zero level items to the root
							rootItems.AddRange(allSMItems.Where(s => s.ShowInMenu && !s.SMItemParentID.HasValue).OrderBy(s => s.Rank).ToList());

						shouldRenderSubs = renderSubId => { return false; };
					}
					break;
			}
			Controls.Add(new LiteralControl(renderMenu(rootItems, null)));
		}
	}
}