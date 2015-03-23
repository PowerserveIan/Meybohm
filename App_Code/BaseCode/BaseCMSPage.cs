using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.ContentManager;

public abstract class BaseCMSPage : BasePage
{
	protected bool IsDeveloper
	{
		get { return (User.IsInRole("Admin") || User.IsInRole("CMS Admin") || User.IsInRole("CMS Content Integrator")); }
	}

	private string m_MicrositeName = string.Empty;

	public override void SetComponentInformation()
	{
		ComponentName = "Content";
		ComponentAdminPage = "content-manager/content-manager.aspx";
		PageUsesBaseCMPage = true;
	}

	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		string masterPageCss = ((HtmlLink)Page.Master.FindControl("uxCSSFiles")).Href;
		string cmsCss = "~/css/contentManager.css";
		if (!masterPageCss.Contains(cmsCss))
			((HtmlLink)Page.Master.FindControl("uxCSSFiles")).Href = masterPageCss + (String.IsNullOrEmpty(masterPageCss) ? "" : ",") + cmsCss;
		if (!Page.ClientScript.IsStartupScriptRegistered("Validate"))
			Page.ClientScript.RegisterStartupScript(typeof(string), "Validate", @"<script language=""javascript"" type=""text/javascript"" src=""//ajax.aspnetcdn.com/ajax/jquery.validate/1.9/jquery.validate.min.js""></script>");

		CMPage cmsPage = CMSHelpers.GetCurrentRequestCMSPage();
		CMMicrosite micrositeEntity = CMSHelpers.GetCurrentRequestCMSMicrosite();
		string fileName = string.Empty;
		if (cmsPage == null)
		{
			fileName = Helpers.GetFileName();
			cmsPage = CMPage.CMPageGetByFileName(fileName).FirstOrDefault();
			if (cmsPage != null)
				CMSHelpers.SetCurrentRequestCMSPage(cmsPage);
		}
		if (cmsPage != null && Settings.EnableCMPageRoles && !IsDeveloper && !CMSHelpers.CanUserManagePage() && !CMSHelpers.CanUserAccessPage(cmsPage.CMPageID))
			Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + (micrositeEntity == null ? "" : micrositeEntity.Name.Replace(" ", "-") + "/") + cmsPage.FileName.Replace(" ", "-"));
		if (cmsPage == null || (micrositeEntity != null && (!micrositeEntity.Active && !(CMSHelpers.CanUserManagePage() || micrositeEntity.Published || User.IsInRole("Admin") || User.IsInRole("CMS Admin")))))
			Response.Redirect("~/404.aspx?page=" + (micrositeEntity == null ? "" : micrositeEntity.Name.Replace(" ", "-") + "/") + fileName);

		if (cmsPage != null)
		{
			if (micrositeEntity != null)
				m_MicrositeName = micrositeEntity.Name;
			ComponentAdditionalLink = "~/admin/content-manager/content-manager-page.aspx?id=" + cmsPage.CMPageID + "&frontendView=true";
			//Uncomment the following lines if you have the Dynamic Header installed
			ContentPlaceHolder contentDynamicHeader = Helpers.FindContentPlaceHolder(Master, "ContentDynamicHeader");
			if (contentDynamicHeader != null)
			{
				contentDynamicHeader.Visible = false;
				if (cmsPage.DynamicCollectionID.HasValue)
				{
					Classes.DynamicHeader.DynamicCollection collection = Classes.DynamicHeader.DynamicCollection.GetByID(cmsPage.DynamicCollectionID.Value);
					if (collection.Active)
					{
						BaseDynamicHeader dynamicHeader = (BaseDynamicHeader)contentDynamicHeader.FindControl("uxDynamicHeaderQV");
						if (dynamicHeader != null)
						{
							dynamicHeader.CollectionName = collection.Name;
							dynamicHeader.Visible = contentDynamicHeader.Visible = true;
						}
					}
				}
			}
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!String.IsNullOrEmpty(m_MicrositeName))
			Page.Title = m_MicrositeName + " - " + Page.Title;
	}
}