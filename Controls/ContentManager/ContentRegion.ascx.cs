using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.ContentManager;
using Classes.Media352_MembershipProvider;
using Classes.SiteLanguages;
using Settings = Classes.ContentManager.Settings;

public partial class Controls_ContentManager_ContentRegion : BaseContentRegion
{
	protected string DiscardMessage = "You have made changes to the content without clicking the Update button, your changes will be lost.";
	private CMPage m_CMSPage;
	private string m_GlobalAreaName = string.Empty;
	private bool m_Hover = true;
	private CMMicrosite m_Microsite;
	private bool m_ViewByDate = true;
	private DateTime m_Viewdate;
	private int m_Width = 533;

	/// <summary>
	/// Sets the Width of the Editor, if Hover is set, the minimum Width is 630
	/// </summary>
	public int Width
	{
		get { return m_Width; }
		set { m_Width = value; }
	}

	/// <summary>
	/// Sets the Height of the Editor, if Hover is set, the minimum Height is 400
	/// </summary>
	public double Height
	{
		get { return uxEditor.Height.Value; }
		set { uxEditor.Height = (Unit)value; }
	}

	/// <summary>
	/// Allows the Editor to hover over other elements on the page
	/// </summary>
	public bool Hover
	{
		get { return m_Hover; }
		set { m_Hover = value; }
	}

	/// <summary>
	/// Must set this if you wish for the content region to be used as a Global Area.  Should be the same name as the RegionName property in most cases.
	/// </summary>
	public string GlobalAreaName
	{
		get { return m_GlobalAreaName; }
		set { m_GlobalAreaName = value; }
	}

	/// <summary>
	/// Enables a checkbox that will let the user make the change for this page only
	/// Only enabled if a GlobalAreaName is set.
	/// </summary>
	public bool AllowSinglePageEditing { get; set; }

	/// <summary>
	/// Whether or not this Global area should only be used for the current Microsite
	/// </summary>
	public bool MicrositeGlobalArea { get; set; }

	/// <summary>
	/// When Hover is set to true, offsets the left position of the Editor
	/// </summary>
	public int LeftAlign { get; set; }

	/// <summary>
	/// When Hover is set to true, offsets the top position of the Editor
	/// </summary>
	public int TopAlign { get; set; }

	/// <summary>
	/// Setting this will remove the rich text editor and replace it with a simple textarea
	/// </summary>
	public bool SimpleEditor { get; set; }

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

	protected bool Editable
	{
		get
		{
			HttpContext context = HttpContext.Current;
			if (context.User.Identity.IsAuthenticated)
				if (CMSHelpers.HasFullCMSPermission() || context.User.IsInRole("CMS Content Integrator") || (CMSHelpers.CanUserManagePage() && ((Settings.EnableMicrosites && (MicrositeGlobalArea || String.IsNullOrEmpty(GlobalAreaName))) || (context.User.IsInRole("CMS Page Editor")))))
					return true;
			return false;
		}
	}

	private bool SingleEditedPage
	{
		get
		{
			if (ViewState["SingleEditedPage"] == null)
				return false;
			return Convert.ToBoolean(ViewState["SingleEditedPage"]);
		}
		set { ViewState["SingleEditedPage"] = value; }
	}

	private bool ViewLiveContent
	{
		get
		{
			bool temp;
			if (!String.IsNullOrEmpty(Request.QueryString[RegionName + "LiveContent"]) && Boolean.TryParse(Request.QueryString[RegionName + "LiveContent"], out temp))
				return temp;
			return false;
		}
	}

	protected int? m_CMPageRegionIDToPullFrom;

	private string GetCacheKey()
	{
		string cacheKey = String.Format("ContentManager{0}{1}{2}", (m_Microsite != null ? m_Microsite.CMMicroSiteID + "/" : "") + (m_CMSPage != null ? m_CMSPage.FileName : Helpers.GetFileName()), RegionName, (Settings.EnableMultipleLanguages ? LanguageID.ToString() : ""));
		return cacheKey;
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		MasterPage master = Page.Master;
		if (master.Master != null)
			master = master.Master;
		string masterPageCss = ((HtmlLink)master.FindControl("uxCSSFiles")).Href;
		const string cmsCss = "~/css/contentManager.css";
		if (!masterPageCss.Contains(cmsCss))
			((HtmlLink)master.FindControl("uxCSSFiles")).Href = masterPageCss + (String.IsNullOrEmpty(masterPageCss) ? "" : ",") + cmsCss;

		if (!Page.ClientScript.IsStartupScriptRegistered("Validate"))
			Page.ClientScript.RegisterStartupScript(typeof(string), "Validate", @"<script language=""javascript"" type=""text/javascript"" src=""//ajax.aspnetcdn.com/ajax/jquery.validate/1.9/jquery.validate.min.js""></script>");
		if (!SimpleEditor)
			Helpers.GetJSCode(uxScripts);
		else
			uxScripts.Visible = false;

		uxSubmit.Click += uxSubmit_Click;
		uxUnapprovedContent.Click += uxUnapprovedContent_Click;
		uxLiveContent.Click += uxLiveContent_Click;
		uxSaveAsDraft.Click += uxSaveAsDraft_Click;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		uxEditor.Visible =
		uxToggleRadEditor.Visible =
		uxSaveAsDraft.Visible =
		uxCancel.Visible =
		uxSubmit.Visible = Editable;

		try
		{
			//the date format passed from the admin is yyyyMMddHHmmssfff; this of course just appears to be a large number
			m_Viewdate = DateTime.ParseExact(Request.QueryString["viewdate"], "yyyyMMddHHmmssfff", null);
		}
		catch (FormatException)
		{
			m_ViewByDate = false;
		}
		catch (ArgumentNullException)
		{
			m_ViewByDate = false;
		}

		m_CMSPage = CMSHelpers.GetCurrentRequestCMSPage();
		//m_CMSPage == null: Physical file that has not had content saved yet

		if (Editable)
		{
			HttpContext.Current.Session["imagemanager.rootpath"] =
			HttpContext.Current.Session["imagemanager.filesystem.rootpath"] = "~/" + Globals.Settings.UploadFolder + "images/";
			HttpContext.Current.Session["filemanager.rootpath"] =
			HttpContext.Current.Session["filemanager.filesystem.rootpath"] = "~/" + Globals.Settings.UploadFolder + "docs/";
		}
		if (Settings.EnableMicrosites)
		{
			m_Microsite = CMSHelpers.GetCurrentRequestCMSMicrosite();

			if (m_Microsite != null)
			{
				if (Editable)
				{
					// set the upload path to the director belonging to the microsite
					string docPath = "~/" + Globals.Settings.UploadFolder + "docs/" + m_Microsite.CMMicroSiteID;
					string imagePath = "~/" + Globals.Settings.UploadFolder + "images/" + m_Microsite.CMMicroSiteID;

					if (!Directory.Exists(Server.MapPath(docPath)))
						Directory.CreateDirectory(Server.MapPath(docPath));
					if (!Directory.Exists(Server.MapPath(imagePath)))
						Directory.CreateDirectory(Server.MapPath(imagePath));

					HttpContext.Current.Session["imagemanager.rootpath"] =
					HttpContext.Current.Session["imagemanager.filesystem.rootpath"] = imagePath;
					HttpContext.Current.Session["filemanager.rootpath"] =
					HttpContext.Current.Session["filemanager.filesystem.rootpath"] = docPath;
				}
			}
		}

		if (RegionName == null)
			throw new Exception("The region requires a name");

		if (IsPostBack)
			DynamicForm.ParseRequestForFormFields(!String.IsNullOrEmpty(GlobalAreaName) ? GlobalAreaName : RegionName);
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxContent.Text = Helpers.ReplaceRootWithRelativePath(GetContent(), (m_Microsite != null ? 1 : 0));
		if (!Page.ClientScript.IsStartupScriptRegistered("ValidateOnlyOnce"))
			Page.ClientScript.RegisterStartupScript(typeof(string), "ValidateOnlyOnce", @"
        <script type=""text/javascript"">
			//<![CDATA[
            $(document).ready(function() {
				if ($(""input[name=dynamicsubmit]"").length > 0)
					$(""input[type=submit][name!=dynamicsubmit]"").addClass('cancel');
            });
			//]]>
        </script>", false);
		Page.ClientScript.RegisterStartupScript(typeof(string), "ValidateForm_" + ClientID, @"
        <script type=""text/javascript"">
			//<![CDATA[
            $(document).ready(function() {
                $(""#" + editableContent.ClientID + @" input[name=dynamicsubmit]"").click(function(){
					if ($(""#" + editableContent.ClientID + @" input[name=dynamicsubmit]"").length == 0)
						$(""form"").validate({
						   ignore: "":not(.fancybox-inner input, .fancybox-inner textarea, .fancybox-inner select)""
						});
					else{
						$(""form"").validate({
						   ignore: "":not(#" + editableContent.ClientID + @" input, #" + editableContent.ClientID + @" textarea, #" + editableContent.ClientID + @" select)""
						});
					}
				});
            });
			//]]>
        </script>", false);

		//For File Upload
		Page.ClientScript.RegisterStartupScript(Page.GetType(), "SetFormEncType", "$(document).ready(function(){if ($(\"input[type=file]\").length > 0)$(\"form\").attr(\"enctype\", \"multipart/form-data\");});", true);

		uxHoverOffsetOpenPH.Visible = uxHoverOffsetClosePH.Visible = Hover && Editable;
		uxHoverDivStyle.Text = @"<div class=""CMBackground"" style=""position: relative; left: " + LeftAlign + @"px; top: " + TopAlign + @"px"">";

		if (Editable)
		{
			uxEditor.Width = Width;
			if (Hover && Height < 400 && !SimpleEditor)
				Height = 400;
			if (!System.Diagnostics.Debugger.IsAttached)
			{
				Helpers.GetJSCode(new Literal { Text = "~/tft-js/core/KeepAlive.js" });
				Page.ClientScript.RegisterStartupScript(typeof(string), "KeepAliveStartup", @"window.setInterval( ""sessionKeepAlive(" + (m_Microsite != null ? 1 : 0) + @")"", 15 * 60 * 1000);", true);
			}
			// Confirm discard JS
			Page.ClientScript.RegisterStartupScript(typeof(string), "ConfirmDiscard", @"
			var suppressConfirm = false;
			$(function(){
				$(""#" + uxSubmit.ClientID + @""").each(function (i) { $(this).mousedown(function () { return suppressConfirm = true; }); });
				$(""#" + uxCancel.ClientID + @""").each(function (i) { $(this).mousedown(function () { return suppressConfirm = true; }); });
			});
			
			window.onbeforeunload = confirmExit;
			function confirmExit(){
				if(!suppressConfirm && editorLoaded) {
					if (tinyMCE.activeEditor.isDirty())
                        return '" + DiscardMessage + @"';
                }
            }", true);

			uxToggleRadEditor.Attributes["onclick"] = "ContentManagerWebMethods.LoadCMSContent(" + (m_CMPageRegionIDToPullFrom.HasValue ? m_CMPageRegionIDToPullFrom.ToString() : "null") + ", LoadCMSContentSuccess_" + ClientID + ");$(this).remove();return false;";
			Page.ClientScript.RegisterClientScriptBlock(typeof(string), "LoadCMSContent_" + ClientID, @"function LoadCMSContentSuccess_" + ClientID + @"(results,context,data){
$('#" + uxEditor.ClientID + "').val(results);" + (SimpleEditor ? "$('#" + uxEditor.ClientID + "').removeClass('hidden');" : "InitializeTinyMCE('" + uxEditor.ClientID + "', " + (m_Microsite != null ? 1 : 0) + ");") + "$('#" + uxEditor.ClientID + "').parents('.CMWrapper').addClass('editing');$('#" + uxEditor.ClientID + "').parents('.CMWrapper').find('.CMTop').show();$('#" + uxCancel.ClientID + "').show();$('#" + uxSubmit.ClientID + "').show();$('#" + uxSaveAsDraft.ClientID + "').show();$('#" + editableContent.ClientID + "').remove();}", true);

			//Register Web Service
			ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
			if (scriptManager.Services.Count(s => s.Path == "~/tft-services/ContentManager/ContentManagerWebMethods.asmx") == 0)
				scriptManager.Services.Add(new ServiceReference { InlineScript = true, Path = "~/tft-services/ContentManager/ContentManagerWebMethods.asmx" });
		}
		else
		{
			uxEditor.Height = new Unit();
			uxSubmit.Visible = uxCancel.Visible = false;
		}
		uxTablePlaceHolder.Visible = true;
		uxGlobalChangeCheckBox.Visible = Editable && !String.IsNullOrEmpty(GlobalAreaName) && AllowSinglePageEditing;
		uxGlobalChangeCheckBox.Checked = !SingleEditedPage;
		uxGlobalChangeCheckBox.Enabled = CMSHelpers.HasFullCMSPermission();
		uxTablePlaceHolder.Visible = uxGlobalChangeCheckBox.Visible;
		uxUnapprovedPlaceHolder.Visible = uxUnapprovedPlaceHolder.Visible || ViewLiveContent;
		uxMessage.Text = ViewLiveContent ? "You are viewing live content.&nbsp;" : "You are viewing unapproved content.&nbsp;";
		uxLiveContent.Visible = !ViewLiveContent;
		uxUnapprovedContent.Visible = ViewLiveContent;
		uxShowApprovalDetails.Visible = !ViewLiveContent;
	}

	private string GetContent()
	{
		CMPageRegion cmPR = null;
		string cacheKey = GetCacheKey() + "_" + (!Editable || ViewLiveContent) + "_" + LanguageID + (Editable ? "_" + Helpers.GetCurrentUserID() : string.Empty);
		const bool offline = false;
		CMRegion cmRegion = CMRegion.CMRegionGetByName(RegionName).FirstOrDefault();
		if (cmRegion == null)
		{
			cmRegion = new CMRegion { Name = RegionName };
			cmRegion.Save();
		}

		if (!m_ViewByDate && !offline)
		{
			CMPageRegion cacheValue = HttpContext.Current.Cache[cacheKey] as CMPageRegion;
			if (cacheValue == null)
			{
				CMPage cmPage = m_CMSPage;
				if (cmPage != null && cmPage.OriginalCMPageID.HasValue)
					cmPage = CMPage.GetByID(cmPage.OriginalCMPageID.Value);

				if (cmPage != null)
				{
					cmPR = GetRegionAfterFiltering(cmPage, cmRegion.CMRegionID, !(!Editable || ViewLiveContent));
					uxDraftPH.Visible = cmPR != null && cmPR.Draft;

					SingleEditedPage = cmPR != null;

					//Check if this is a microsite page, and if it is, pull down the content from the default
					if (cmPR == null && m_Microsite != null)
					{
						CMPage micrositeDefault = CMPage.CMPageGetByFileName(cmPage.FileName).Find(p => p.MicrositeDefault);
						cmPR = GetRegionAfterFiltering(micrositeDefault, cmRegion.CMRegionID);
					}

					//There is no content for this page, now check if it is a microsite global region and pull down that content
					if (cmPR == null && !String.IsNullOrEmpty(GlobalAreaName) && MicrositeGlobalArea && m_Microsite != null)
					{
						CMPage globalPageName = CMPage.CMPageGetByFileName(GlobalAreaName).Find(c => c.CMMicrositeID == m_Microsite.CMMicroSiteID);
						cmPR = GetRegionAfterFiltering(globalPageName, cmRegion.CMRegionID);
					}

					//There is no content for this page, now check if its a global region and pull down that content
					if (cmPR == null && !String.IsNullOrEmpty(GlobalAreaName))
					{
						CMPage globalPageName = CMPage.CMPageGetByFileName(GlobalAreaName).Find(c => c.CMMicrositeID == null);
						cmPR = GetRegionAfterFiltering(globalPageName, cmRegion.CMRegionID);
					}
				}
				else if (!String.IsNullOrEmpty(GlobalAreaName))
				{
					CMPage globalPageName = CMPage.CMPageGetByFileName(GlobalAreaName).Find(c => c.CMMicrositeID == m_Microsite.CMMicroSiteID);
					cmPR = GetRegionAfterFiltering(globalPageName, cmRegion.CMRegionID);
				}
				if (cmPR != null)
				{
					if (cmPR.NeedsApproval || ViewLiveContent)
						ShowApprovalInfo(cmPR);
					m_CMPageRegionIDToPullFrom = cmPR.CMPageRegionID;

					HttpContext.Current.Cache.Insert(cacheKey, cmPR, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(Settings.CacheDuration));
				}
			}
			else
			{
				cmPR = cacheValue;

				if (cmPR != null && cmPR.NeedsApproval || ViewLiveContent)
					ShowApprovalInfo(cmPR);
				if (cmPR != null)
					m_CMPageRegionIDToPullFrom = cmPR.CMPageRegionID;
				if (!String.IsNullOrEmpty(GlobalAreaName) && AllowSinglePageEditing)
					SingleEditedPage = cmPR != null && CMPage.GetByID(cmPR.CMPageID).CMTemplateID.HasValue;
				uxDraftPH.Visible = cmPR != null && cmPR.Draft;
			}
		}
		else if (!offline) //Viewing by date 
		{
			CMPage cmPage = m_CMSPage;
			cmPR = GetRegionAfterFiltering(cmPage, cmRegion.CMRegionID, m_Viewdate, false);

			if (cmPR == null)
				return "No content available at " + m_Viewdate.ToString("d");
			m_CMPageRegionIDToPullFrom = cmPR.CMPageRegionID;
		}
		return cmPR != null ? cmPR.Content : string.Empty;
	}

	private CMPageRegion GetRegionAfterFiltering(CMPage cmPage, int regionID)
	{
		return GetRegionAfterFiltering(cmPage, regionID, null, true);
	}

	private CMPageRegion GetRegionAfterFiltering(CMPage cmPage, int regionID, bool filterApproval = true)
	{
		return GetRegionAfterFiltering(cmPage, regionID, null, filterApproval);
	}

	private CMPageRegion GetRegionAfterFiltering(CMPage cmPage, int regionID, DateTime? date = null, bool filterApproval = true)
	{
		CMPageRegion cmPR = null;
		if (cmPage != null)
		{
			CMPageRegion.Filters filterList = new CMPageRegion.Filters();
			filterList.FilterCMPageRegionCMPageID = cmPage.CMPageID.ToString();
			filterList.FilterCMPageRegionCMRegionID = regionID.ToString();
			if (Settings.EnableMultipleLanguages)
				filterList.FilterCMPageRegionLanguageID = Helpers.GetCurrentLanguage().LanguageID.ToString();
			if (date.HasValue)
				filterList.FilterCMPageRegionCreated = date.Value.ToString();
			filterList.FilterCMPageRegionNeedsApproval = filterApproval.ToString();
			if (Editable)
				filterList.FilterCMPageRegionUserID = Helpers.GetCurrentUserID().ToString();

			cmPR = CMPageRegion.LoadContentRegion(filterList);
		}
		return cmPR;
	}

	void SaveContent(bool draft)
	{
		string cacheKey = GetCacheKey();
		if (HttpContext.Current.Cache[cacheKey + "_True"] != null)
			HttpContext.Current.Cache.Remove(cacheKey + "_True");
		if (HttpContext.Current.Cache[cacheKey + "_False"] != null)
			HttpContext.Current.Cache.Remove(cacheKey + "_False");

		uxEditor.Text = uxContent.Text = Helpers.ReplaceAbsolutePathWithRoot(HttpUtility.HtmlDecode(uxEditor.Text));
		List<CMPage> cachedPages = new List<CMPage>();
		m_CMSPage = CMSHelpers.GetCurrentRequestCMSPage();
		if (String.IsNullOrEmpty(GlobalAreaName) || (!String.IsNullOrEmpty(GlobalAreaName) && !uxGlobalChangeCheckBox.Checked))
		{
			cachedPages.Add(m_CMSPage);
			//TODO: I dont know what this code is intended to do, but its running the same check twice which would seemingly never fire the second if block
			//Page might exist just not for language
			//if (cachedPages.Count == 0 && Settings.EnableMultipleLanguages)
			//{
			//	cachedPages = CMPage.CMPageGetByFileName(m_CMSPage.FileName).Where(c => c.CMMicrositeID == m_Microsite.CMMicroSiteID).ToList();
			//	if (cachedPages.Count > 0)
			//	{
			//		CMPage pageToUse = cachedPages.FirstOrDefault(c => !c.NeedsApproval && !c.OriginalCMPageID.HasValue) ?? cachedPages.FirstOrDefault();
			//		CMPageTitle newTitle = CMPageTitle.CMPageTitleGetByCMPageIDAndLanguageID((pageToUse.OriginalCMPageID.HasValue ? pageToUse.OriginalCMPageID.Value : pageToUse.CMPageID), Helpers.GetDefaultLanguageID()).FirstOrDefault();
			//		if (newTitle != null)
			//		{
			//			newTitle.LanguageID = LanguageID;
			//			newTitle.Save();
			//		}
			//	}
			//}
		}
		else //Global Region
			cachedPages = CMSHelpers.GetCachedCMPages().Where(c => c.FileName.Equals(GlobalAreaName, StringComparison.OrdinalIgnoreCase) && c.CMMicrositeID == m_Microsite.CMMicroSiteID).ToList();

		int userId = Helpers.GetCurrentUserID();
		CMPage cmPage;
		if (cachedPages.Any(results => results != null))
		{
			cmPage = cachedPages.FirstOrDefault();
			if (cmPage != null && cmPage.OriginalCMPageID.HasValue)
				cmPage = CMPage.GetByID(cmPage.OriginalCMPageID.Value);
		}
		else
		{
			cmPage = new CMPage();
			cmPage.CanDelete = false;
			cmPage.Created = DateTime.UtcNow;
			cmPage.CMTemplateID = null;
			if (m_Microsite != null)
				cmPage.CMMicrositeID = m_Microsite.CMMicroSiteID;
			cmPage.UserID = userId;
			if (!String.IsNullOrEmpty(GlobalAreaName) && uxGlobalChangeCheckBox.Checked)
			{
				cmPage.FileName = GlobalAreaName;
				cmPage.Title = GlobalAreaName;
				cmPage.Deleted = true;
			}
			else
			{
				cmPage.FileName =
				cmPage.Title = Helpers.GetFileName();
				cmPage.Deleted = false;
				if (Settings.EnableApprovals && !CMSHelpers.HasFullCMSPermission())
				{
					cmPage.NeedsApproval = true;
					cmPage.EditorUserIDs = cmPage.UserID.ToString();
				}
			}
			cmPage.Save();

			m_CMSPage = cmPage;
		}

		CMRegion cmRegion = CMRegion.CMRegionGetByName(RegionName).FirstOrDefault();
		if (!String.IsNullOrEmpty(GlobalAreaName) && uxGlobalChangeCheckBox.Checked && !draft)
		{
			//Set the current version to false for the current page because it now needs to pull from the Global Area
			CMPage actualPage = m_CMSPage;
			if (actualPage != null)
			{
				CMPageRegion.Filters filterList = new CMPageRegion.Filters();
				filterList.FilterCMPageRegionCMPageID = actualPage.CMPageID.ToString();
				filterList.FilterCMPageRegionCMRegionID = cmRegion.CMRegionID.ToString();
				filterList.FilterCMPageRegionCurrentVersion = true.ToString();
				if (Settings.EnableMultipleLanguages)
					filterList.FilterCMPageRegionLanguageID = Helpers.GetCurrentLanguage().LanguageID.ToString();

				List<CMPageRegion> oldRegions = CMPageRegion.CMPageRegionPage(0, 0, "", "CMPageRegionID", false, filterList);
				oldRegions.ForEach(pr => { pr.CurrentVersion = false; pr.Save(); });
			}
		}
		
		int? languageID = null;
		if (Settings.EnableMultipleLanguages)
			languageID = LanguageID;

		CMPageRegion cmPRlast;
		if (draft)
		{
			CMPageRegion.Filters filterList = new CMPageRegion.Filters();
			filterList.FilterCMPageRegionCMPageID = cmPage.CMPageID.ToString();
			filterList.FilterCMPageRegionCMRegionID = cmRegion.CMRegionID.ToString();
			filterList.FilterCMPageRegionCurrentVersion = true.ToString();
			if (languageID.HasValue)
				filterList.FilterCMPageRegionLanguageID = languageID.ToString();
			filterList.FilterCMPageRegionDraft = true.ToString();
			filterList.FilterCMPageRegionUserID = userId.ToString();

			cmPRlast = CMPageRegion.CMPageRegionPage(0, 0, "", "CMPageRegionID", false, filterList).FirstOrDefault();
			if (cmPRlast == null)
				cmPRlast = new CMPageRegion { CMPageID = cmPage.CMPageID, Created = DateTime.UtcNow, CMRegionID = cmRegion.CMRegionID, Draft = true, LanguageID = languageID, UserID = userId };
			cmPRlast.Content = DynamicForm.PrepareDynamicForm(Helpers.ReplaceAbsolutePathWithRoot(uxEditor.Text));
			cmPRlast.ContentClean = Regex.Replace(uxEditor.Text, @"<(.|\n)*?>", string.Empty);
			cmPRlast.Save();
			CMSHelpers.ClearCaches();
			uxDraftPH.Visible = true;
		}
		else
		{
			//Admin is editing the page			
			CMPageRegion cmPRnew = new CMPageRegion();
			cmPRnew.CMPageID = cmPage.CMPageID;
			cmPRnew.CMRegionID = cmRegion.CMRegionID;

			if (CMSHelpers.HasFullCMSPermission())
			{
				CMPageRegion.Filters filterList = new CMPageRegion.Filters();
				filterList.FilterCMPageRegionCMPageID = cmPage.CMPageID.ToString();
				filterList.FilterCMPageRegionCMRegionID = cmRegion.CMRegionID.ToString();
				filterList.FilterCMPageRegionCurrentVersion = true.ToString();
				if (languageID.HasValue)
					filterList.FilterCMPageRegionLanguageID = languageID.ToString();

				List<CMPageRegion> pageRegionsLast = CMPageRegion.CMPageRegionPage(0, 0, "", "CMPageRegionID", true, filterList);
				pageRegionsLast.Where(p => !p.Draft).ToList().ForEach(pr => { pr.CurrentVersion = false; pr.Save(); });
				cmPRlast = pageRegionsLast.LastOrDefault();
				if (pageRegionsLast.Any(p => p.Draft && p.UserID == userId))
				{
					cmPRnew = pageRegionsLast.Find(p => p.Draft && p.UserID == userId);
					cmPRnew.Draft = false;
				}
				cmPRnew.CurrentVersion = true;
				if (cmPRlast != null)
					CMSHelpers.SendApprovalEmailAlerts(cmPage, cmPRlast, userId, true, true, true, languageID);

				pageRegionsLast.ForEach(p => { if (p.NeedsApproval) p.Delete(); });
			}
			else
			{
				CMPageRegion.Filters filterList = new CMPageRegion.Filters();
				filterList.FilterCMPageRegionCMPageID = cmPage.CMPageID.ToString();
				filterList.FilterCMPageRegionCMRegionID = cmRegion.CMRegionID.ToString();
				filterList.FilterCMPageRegionNeedsApproval = true.ToString();
				if (languageID.HasValue)
					filterList.FilterCMPageRegionLanguageID = languageID.ToString();

				CMPageRegion pageRegionLast = CMPageRegion.LoadContentRegion(filterList);
				cmPRlast = pageRegionLast;
				if (pageRegionLast != null && pageRegionLast.Draft && pageRegionLast.UserID == userId)
					cmPRlast.Draft = false;
				if (cmPRlast != null)
					cmPRnew = cmPRlast;

				if (Settings.EnableApprovals)
					cmPRnew.NeedsApproval = true;

				if (!String.IsNullOrEmpty(GlobalAreaName) && uxGlobalChangeCheckBox.Checked)
				{
					CMPage actualCMPage = m_CMSPage;
					if (actualCMPage != null)
						cmPRnew.GlobalAreaCMPageID = actualCMPage.CMPageID;
				}

				if (!CMSHelpers.PageHasBeenEditedByUserBefore(cmPRnew.EditorUserIDs, userId))
					cmPRnew.EditorUserIDs = (cmPRnew.EditorUserIDs + "," + userId).TrimStart(',');
				CMSHelpers.SendApprovalEmailAlerts(cmPage, cmPRnew, userId, true, false, null, languageID);
			}

			cmPRnew.Content = DynamicForm.PrepareDynamicForm(Helpers.ReplaceAbsolutePathWithRoot(uxEditor.Text));
			cmPRnew.ContentClean = Regex.Replace(uxEditor.Text, @"<(.|\n)*?>", string.Empty);
			cmPRnew.Created = DateTime.UtcNow;

			if (userId != 0)
				cmPRnew.UserID = userId;
			if (Settings.EnableMultipleLanguages)
				cmPRnew.LanguageID = Helpers.GetCurrentLanguage().LanguageID;

			cmPRnew.Save();

			CMSHelpers.ClearCaches();

			if (Settings.EnableMicrosites && Settings.MicrositeDefaultChangesAffectExistingMicrosites && cmPage.MicrositeDefault)
				CMSHelpers.UpdateMicrositesWithDefaultContent(cmPage.FileName);

//			if (!(!String.IsNullOrEmpty(GlobalAreaName) && uxGlobalChangeCheckBox.Checked || cmPRnew.NeedsApproval))
//			{
//				SearchPlugin.SendAllFileContent();
//				SearchPlugin searchPlugin = new SearchPlugin { Name = RegionName };
//
//				searchPlugin.SendSearchContent((m_Microsite != null ? (int?)m_Microsite.CMMicroSiteID : null), m_CMSPage.FileName, uxEditor.Text, cmPRnew.ContentClean);
//			}
			uxDraftPH.Visible = false;
		}
		uxToggleRadEditor.Visible = true;
		Cache.Remove(cacheKey);

		uxEditor.Text = string.Empty;

		if (!String.IsNullOrEmpty(Request.QueryString["viewdate"]))
			Response.Redirect(Request.Url.ToString().Replace("viewdate=" + Request.QueryString["viewdate"], "").Replace("?&", "?").TrimEnd('?'));
	}

	private void ShowApprovalInfo(CMPageRegion cmPR)
	{
		uxUnapprovedPlaceHolder.Visible = Editable;
		if (!String.IsNullOrEmpty(cmPR.EditorUserIDs))
		{
			foreach (string s in cmPR.EditorUserIDs.Split(','))
			{
				User userInfo = User.GetByID(Convert.ToInt32(s));
				if (userInfo != null)
					uxAllEditors.Text += @"<a href=""mailto:" + userInfo.Email + @""">" + userInfo.Email + @"</a>";
			}
		}
		uxLastEditedDate.Text = cmPR.CreatedClientTime.ToString("MMMM d, yyyy h:mm tt");
	}

	private void uxLiveContent_Click(object sender, EventArgs e)
	{
		string redirectURL = Request.RawUrl.Replace("?" + RegionName + "LiveContent=" + Request.QueryString[RegionName + "LiveContent"], "").Replace("&" + RegionName + "LiveContent=" + Request.QueryString[RegionName + "LiveContent"], "");
		redirectURL = redirectURL + (redirectURL.Contains("?") ? "&" : "?") + RegionName + "LiveContent=true";
		Response.Redirect(redirectURL);
	}

	private void uxUnapprovedContent_Click(object sender, EventArgs e)
	{
		string redirectURL = Request.RawUrl.Replace("?" + RegionName + "LiveContent=" + Request.QueryString[RegionName + "LiveContent"], "").Replace("&" + RegionName + "LiveContent=" + Request.QueryString[RegionName + "LiveContent"], "");
		Response.Redirect(redirectURL);
	}

	private void uxSubmit_Click(object sender, EventArgs e)
	{
		SaveContent(false);
	}

	void uxSaveAsDraft_Click(object sender, EventArgs e)
	{
		SaveContent(true);
	}
}