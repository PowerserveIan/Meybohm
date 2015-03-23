using System;
using System.Linq;
using System.Web.UI;
using BaseCode;
using Classes.ContentManager;

public partial class microsite : BaseMasterPage
{
	public bool IsAiken
	{
		get { return CurrentMicrosite.Name == "Aiken"; }
	}

	public bool IsAugusta
	{
		get { return CurrentMicrosite.Name == "Augusta"; }
	}

	public bool NewHomes
	{
		get
		{
			return Session["NewHomes"] != null;
		}
		set
		{
			if (value)
				Session["NewHomes"] = true;
			else
				Session.Remove("NewHomes");
		}
	}

	public CMMicrosite CurrentMicrosite
	{
		get { return CMSHelpers.GetCurrentRequestCMSMicrosite(); }
	}

	public string CurrentMicrositePath
	{
		get { return ResolveClientUrl("~/") + CurrentMicrosite.Name.ToLower().Replace(" ", "-") + "/"; }
	}

	public CMMicrosite OtherMicrosite
	{
		get { return IsAiken ? CMMicrosite.CMMicrositeGetByName("Augusta").FirstOrDefault() : CMMicrosite.CMMicrositeGetByName("Aiken").FirstOrDefault(); }
	}

	public string OtherMicrositePath
	{
		get { return ResolveClientUrl("~/") + OtherMicrosite.Name.ToLower().Replace(" ", "-") + "/"; }
	}

	public string CurrentMicrositeCityAndState
	{
		get { return IsAiken ? "Aiken, SC" : "Augusta, GA"; }
	}

	protected override void OnInit(EventArgs e)
	{
		if (CurrentMicrosite == null)
			Response.Redirect("~/");
		m_HtmlEntity = htmlEntity;
		m_CssFiles = uxCSSFiles;
		m_CssPrintFiles = uxPrintCSS;
		m_JavaScriptFiles = uxJavaScripts;
		m_BottomBarPlaceHolder = uxAdminBar.BottomBarPH;
		m_ClearSiteCacheLink = uxAdminBar.ClearCacheLink;
		m_EditPagePropertiesLink = uxAdminBar.EditPagePropertiesLink;
		m_ComponentAdminLink = uxAdminBar.ComponentAdminLink;
		m_LogoutLink = uxAdminBar.LogoutLink;
		m_CMSPlaceHolder = uxAdminBar.CMSPlaceholder;
		m_PageRegionsRepeater = uxAdminBar.PageRegionsRepeater;
		base.OnInit(e);
		uxLoginLink.Visible = !Page.User.Identity.IsAuthenticated;
		uxLogout.Visible = !uxLoginLink.Visible;
		uxNewExistingToggle.Click += uxNewExistingToggle_Click;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (NewHomePage.HasValue)
			NewHomes = NewHomePage.Value;
		if (ContentDynamicHeader != null)
			ContentDynamicHeader.Visible = ContentDynamicHeader.Visible && PageUsesBaseCMPage;
		if (uxAugustaWeatherPH != null)
		{
			uxAugustaWeatherPH.Visible = IsAugusta;
			uxAikenWeatherPH.Visible = IsAiken;
		}
		if (uxMenu != null)
			uxMenu.NewHomes = NewHomes;
		if (uxBreadCrumbs != null)
			uxBreadCrumbs.NewHomes = NewHomes;
		if (uxSearchWidget != null)
			uxSearchWidget.NewHomes = NewHomes;
		uxNewExistingToggle.Text = (NewHomes ? "Resale" : "New") + " Homes";

		uxJQuery.Text = Helpers.ReplaceRootWithRelativePath(uxJQuery.Text, 1);

		m_CssFiles.Href = m_CssFiles.Href.Replace("[MICROSITECSS]", "~/css/microsite" + CurrentMicrosite.Name.ToLower().Replace(" ", "-") + ".css").Replace("[MICROSITENEWCSS]", (NewHomes ? ",~/css/micrositeNew.css" : ""));

		if (Request.Cookies["Microsite"] == null)
		{
			System.Web.HttpCookie micrositeCookie = new System.Web.HttpCookie("Microsite", CurrentMicrosite.Name.ToLower().Replace(" ", "-"));
			micrositeCookie.Expires = DateTime.Now.AddYears(5);
			Response.Cookies.Add(micrositeCookie);
		}

		uxMyAccount.Text = Page.User.IsInRole("Agent") ? "My Dashboard" : "My Profile";
		uxMyAccount.NavigateUrl = "~/" + CurrentMicrosite.Name.ToLower().Replace(" ", "-") +  "/" + (Page.User.IsInRole("Agent") ? "agent-home" : "profile");

		uxLiveChatJS.Text = @"<script type=""text/javascript"">var __lc = {};__lc.license = " + System.Configuration.ConfigurationManager.AppSettings["LiveChat_LicenseNumber"] + ";</script>";
	}

	void uxNewExistingToggle_Click(object sender, EventArgs e)
	{
		NewHomes = !NewHomes;
		Response.Redirect(CurrentMicrositePath + (NewHomes ? "new-homes" : ""));
	}
}