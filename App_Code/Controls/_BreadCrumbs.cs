using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;

namespace Classes.ContentManager
{
	public class BreadCrumbs : UserControl
	{
		public Literal BreadCrumbsL = new Literal();
		private string m_ULClass = "breadcrumbs clearfix";
		private string m_FirstLIClass = "firstBreadcrumb";
		private string m_CurrentLIClass = "currentBreadcrumb";

		public string PageTitle { get; set; }

		/// <summary>
		/// Class that will be applied to the UL surrounding the breadcrumbs
		/// </summary>
		public string ULClass
		{
			get { return m_ULClass; }
			set { m_ULClass = value; }
		}

		/// <summary>
		/// Class that will be applied to the first LI
		/// </summary>
		public string FirstLIClass
		{
			get { return m_FirstLIClass; }
			set { m_FirstLIClass = value; }
		}

		/// <summary>
		/// Class that will be applied to the LI surrounding the current page
		/// </summary>
		public string CurrentLIClass
		{
			get { return m_CurrentLIClass; }
			set { m_CurrentLIClass = value; }
		}

		private int m_CurrentLanguageID;

		public bool? NewHomes { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			m_CurrentLanguageID = Settings.EnableMultipleLanguages ? Helpers.GetCurrentLanguage().LanguageID : Helpers.GetDefaultLanguageID();
			BreadCrumbsL.Text = string.Empty;

			CMPage page = CMSHelpers.GetCurrentRequestCMSPage();
			CMMicrosite micrositeEntity = CMSHelpers.GetCurrentRequestCMSMicrosite();
			bool currentPageAdded = false;
			int? micrositeID = micrositeEntity != null ? (int?)micrositeEntity.CMMicroSiteID : null;
			if (page != null)
			{
				if (page.FileName.Equals("default.aspx", StringComparison.OrdinalIgnoreCase) || page.FileName.Equals("Home.aspx", StringComparison.OrdinalIgnoreCase))
				{
				}
				else
				{
					List<SMItem> sms;
					if (Settings.EnableMultipleLanguages && Settings.MultilingualManageSiteMapsIndividually)
						sms = CMSHelpers.GetCachedSMItems(micrositeID, Helpers.GetCurrentLanguage().LanguageID).Where(s => s.CMPageID == page.CMPageID && !s.OriginalSMItemID.HasValue && !s.NeedsApproval).ToList();
					else
						sms = CMSHelpers.GetCachedSMItems(micrositeID).Where(s => s.CMPageID == page.CMPageID && !s.OriginalSMItemID.HasValue && !s.NeedsApproval && (s.LanguageID == null || s.LanguageID == Helpers.GetDefaultLanguageID())).ToList();
					sms = sms.Where(s => !s.NewHomes.HasValue || s.NewHomes.Value == NewHomes).ToList();
					if (sms.Count > 0)
					{
						int mID;
						SMItem smi = (!String.IsNullOrEmpty(Request.QueryString["mID"]) && Int32.TryParse(Request.QueryString["mID"], out mID) ? sms.Find(s1 => s1.SMItemID == mID) : sms.Find(s1 => s1.CMPageID == page.CMPageID)) ?? sms[0];
						int count = 0;

						Action<SMItem> addBreadCrumb = null;
						addBreadCrumb = smItem =>
						{
							CMPage cmPage = CMSHelpers.GetCachedCMPages().Where(c => c.CMPageID == smItem.CMPageID).FirstOrDefault();
							string itemDisplayName = smItem.Name;
							if (Settings.EnableMultipleLanguages && !Settings.MultilingualManageSiteMapsIndividually && smItem.LanguageID != m_CurrentLanguageID)
							{
								List<CMPageTitle> titles = CMPageTitle.CMPageTitleGetByCMPageIDAndLanguageID(smItem.CMPageID, m_CurrentLanguageID);
								if (titles.Count > 0)
									itemDisplayName = titles.LastOrDefault().Title;
							}
							if (cmPage != null && count != 0)
							{
								if (!cmPage.FileName.Equals("default.aspx", StringComparison.OrdinalIgnoreCase) && !cmPage.FileName.Equals("Home.aspx", StringComparison.OrdinalIgnoreCase))
									BreadCrumbsL.Text = @"<li><a title=""" + Server.HtmlEncode(itemDisplayName.Replace("<br />", "")) + @""" href=""" + Server.HtmlEncode(cmPage.FileName) + @""">" + Server.HtmlEncode(itemDisplayName.Replace("<br />", "")) + @"</a></li>" + BreadCrumbsL.Text;
							}
							else
							{
								currentPageAdded = true;
								//This is where the current page gets added
								BreadCrumbsL.Text = @"<li" + (!String.IsNullOrEmpty(m_CurrentLIClass) ? " class='" + m_CurrentLIClass + "'" : "") + @">" + Server.HtmlEncode(itemDisplayName.Replace("<br />", "")) + @"</li>" + BreadCrumbsL.Text;
							}
							count++;
							if (smItem.SMItemParentID.HasValue)
								addBreadCrumb(CMSHelpers.GetCachedSMItems(micrositeID).Where(s => s.SMItemID == smItem.SMItemParentID.Value).Single());
						};
						addBreadCrumb(smi);
					}
				}
			}

			if (micrositeEntity != null)
				BreadCrumbsL.Text = @"<li><a title=""" + Server.HtmlEncode(micrositeEntity.Name) + @""" href=""" + Helpers.RootPath + Server.HtmlEncode(micrositeEntity.Name.ToLower().Replace(" ", "-")) + (Globals.Settings.RequireASPXExtensions ? "/Home.aspx" : "/") + @""">" + Server.HtmlEncode(micrositeEntity.Name) + @"</a></li>" + BreadCrumbsL.Text;


			BreadCrumbsL.Text = @"<ul class='" + m_ULClass + @"'><li" + (!String.IsNullOrEmpty(m_FirstLIClass) ? " class='" + m_FirstLIClass + "'" : "") + @"><a title=""Home"" href=""" + Helpers.RootPath + @""">Home</a></li>" + BreadCrumbsL.Text;

			if (!currentPageAdded && (!String.IsNullOrEmpty(PageTitle) || Page.Title != Globals.Settings.SiteTitle))
				BreadCrumbsL.Text += @"<li" + (!String.IsNullOrEmpty(m_CurrentLIClass) ? " class='" + m_CurrentLIClass + "'" : "") + @">" + (!String.IsNullOrEmpty(PageTitle) ? PageTitle : Page.Title.Replace(" - " + Globals.Settings.SiteTitle, "")) + @"</li>";
			BreadCrumbsL.Text += @"</ul>";
			BreadCrumbsL.EnableViewState = false;
			Controls.Add(BreadCrumbsL);
		}
	}
}