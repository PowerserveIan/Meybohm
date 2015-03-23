using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using BaseCode;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public class BaseMasterPage : MasterPage
{
	protected const string m_BottomBarToggleCookieName = "AdminBottomBar_Show";
	protected const string m_BottomBarPinCookieName = "AdminBottomBar_Pin";
	/// <summary>
	/// Display Name of the Component for use with the Bottom Bar
	/// </summary>
	public string ComponentName { get; set; }
	/// <summary>
	/// Landing page for the Component Return to Admin button in the Bottom Bar ("~/admin/" will be prepended)
	/// </summary>
	public string ComponentAdminPage { get; set; }
	/// <summary>
	/// Can be used to pass any other URL to the bottom bar that will open up in a fancybox
	/// </summary>
	public string ComponentAdditionalLink { get; set; }
	/// <summary>
	/// Based on value of cookie
	/// </summary>
	public bool HideBottomBar { get; set; }
	/// <summary>
	/// Primarily used by the Dynamic Header to show/hide the content placeholder
	/// </summary>
	public bool PageUsesBaseCMPage { get; set; }

	public bool? NewHomePage { get; set; }
	/// <summary>
	/// Based on value of cookie
	/// </summary>
	public bool UnpinBottomBar { get; set; }

	protected PlaceHolder m_BottomBarPlaceHolder;

	protected LinkButton m_ClearSiteCacheLink;

	protected PlaceHolder m_CMSPlaceHolder;

	protected HyperLink m_ComponentAdminLink;

	protected HtmlLink m_CssFiles;

	protected HtmlLink m_CssPrintFiles;

	protected HyperLink m_EditPagePropertiesLink;

	protected HtmlElement m_HtmlEntity;

	protected Literal m_JavaScriptFiles;

	protected HyperLink m_LogoutLink;

	protected Repeater m_PageRegionsRepeater;

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		if (m_LogoutLink != null)
			m_LogoutLink.NavigateUrl = "~/logout.aspx?ReturnUrl=" + HttpContext.Current.Request.RawUrl;

		if (Globals.Settings.FacebookEnableLikeButton && m_HtmlEntity != null)
		{
			m_HtmlEntity.Attributes["xmlns:fb"] = "http://www.facebook.com/2008/fbml";
			m_HtmlEntity.Attributes["xmlns:og"] = "http://opengraphprotocol.org/schema";
		}

		decimal temp;
		if (m_HtmlEntity != null && Request.Browser.Browser == "IE" && Decimal.TryParse(Request.Browser.Version, out temp))
			m_HtmlEntity.Attributes["class"] = "ie" + temp.ToString().Split('.')[0] + (temp < 9 ? " ltIE9" : "");
		else if (m_HtmlEntity != null && ((!String.IsNullOrEmpty(Request.UserAgent) && Request.UserAgent.ToLower().Contains("macintosh")) || Request.Browser.MobileDeviceManufacturer == "Apple" || Request.Browser.Browser == "Safari"))
			m_HtmlEntity.Attributes["class"] = "mac";
		else if (m_HtmlEntity != null && Request.Browser.Browser == "Chrome")
			m_HtmlEntity.Attributes["class"] = "chrome";
		if (m_HtmlEntity != null)
		{
			if (!String.IsNullOrEmpty(Request.UserAgent) && Request.UserAgent.ToLower().Contains("ipad"))
				m_HtmlEntity.Attributes["class"] += " ipad";
			if (!String.IsNullOrEmpty(Request.UserAgent) && (Request.UserAgent.ToLower().Contains("blackberry") || Request.UserAgent.ToLower().Contains("iphone") || Request.UserAgent.ToLower().Contains("android")))
				m_HtmlEntity.Attributes["class"] += " mobile";
			m_HtmlEntity.Attributes["class"] += " tft-" + Helpers.GetFileName().ToLower().Replace(".aspx", "");
			m_HtmlEntity.Attributes["class"] = m_HtmlEntity.Attributes["class"].Trim();
		}
		m_ClearSiteCacheLink.Click += m_ClearSiteCacheLink_Click;
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		if (m_BottomBarPlaceHolder != null)
			m_BottomBarPlaceHolder.Visible = Helpers.CanAccessAdmin();
		if (m_BottomBarPlaceHolder != null && m_BottomBarPlaceHolder.Visible)
			BindBottomBar();
		if (m_CssFiles != null)
			Helpers.GetCSSCode(m_CssFiles);
		if (m_CssPrintFiles != null)
			Helpers.GetCSSCode(m_CssPrintFiles);
		if (m_JavaScriptFiles != null)
			Helpers.GetJSCode(m_JavaScriptFiles);
	}

	void m_ClearSiteCacheLink_Click(object sender, EventArgs e)
	{
		Helpers.PurgeCacheItems(null);
		Response.Redirect(Request.UrlReferrer.PathAndQuery);
	}

	protected virtual void BindBottomBar()
	{
		if (Page.User.IsInRole("Admin"))
		{
			if (!String.IsNullOrEmpty(ComponentName) && !String.IsNullOrEmpty(ComponentAdminPage))
			{
				m_ComponentAdminLink.Text = "<span>" + ComponentName + " Manager</span>";
				m_ComponentAdminLink.NavigateUrl = "~/admin/" + ComponentAdminPage;
				m_ComponentAdminLink.CssClass += (String.IsNullOrEmpty(m_ComponentAdminLink.CssClass) ? "" : " ") + ComponentName.ToLower().Replace(" ", "") + "Icon";
			}
			else
				m_ComponentAdminLink.Visible = false;

			if (!String.IsNullOrEmpty(ComponentAdditionalLink))
				m_EditPagePropertiesLink.NavigateUrl = ComponentAdditionalLink;
			else
				m_EditPagePropertiesLink.Visible = false;

			if (m_CMSPlaceHolder != null)
			{
				Dictionary<string, string> regionNames = Classes.ContentManager.CMSHelpers.HasFullCMSPermission() ? GetRegionNames(this, new Dictionary<string, string>()) : new Dictionary<string, string>();
				m_PageRegionsRepeater.DataSource = regionNames;
				m_PageRegionsRepeater.DataBind();
				if (regionNames.Count == 0)
					m_CMSPlaceHolder.Visible = false;
			}
		}
		else
			m_ClearSiteCacheLink.Visible = m_EditPagePropertiesLink.Visible = m_ComponentAdminLink.Visible = m_CMSPlaceHolder.Visible = false;
		HideBottomBar = Request.Cookies[m_BottomBarToggleCookieName] != null && !Convert.ToBoolean(Request.Cookies[m_BottomBarToggleCookieName].Value);
		UnpinBottomBar = Request.Cookies[m_BottomBarPinCookieName] != null && !Convert.ToBoolean(Request.Cookies[m_BottomBarPinCookieName].Value);
	}

	protected Dictionary<string, string> GetRegionNames(Control c, Dictionary<string, string> regionNames)
	{
		foreach (Control cont in c.Controls)
		{
			if (cont is BaseContentRegion && cont.Visible)
				regionNames.Add(((BaseContentRegion)cont).RegionName, cont.ClientID);
			else if (cont.HasControls())
				GetRegionNames(cont, regionNames);
		}
		return regionNames;
	}
}