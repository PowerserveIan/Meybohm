using System;
using System.Web.UI;
using BaseCode;

public partial class frontend : BaseMasterPage
{
	protected override void OnInit(EventArgs e)
	{
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
	}
}