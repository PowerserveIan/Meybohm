using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;
using System.Web.UI;
using Classes.ContentManager;
using Classes.SEOComponent;

namespace BaseCode
{
	public class RoutingHandler : IRouteHandler
	{
		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			string micrositeName = requestContext.RouteData.Values.ContainsKey("Microsite") ? requestContext.RouteData.Values["Microsite"].ToString() : string.Empty;
			string pageName = requestContext.RouteData.Values.ContainsKey("CMPage") ? requestContext.RouteData.Values["CMPage"].ToString() : requestContext.RouteData.Values["MicrositeOrCMPage"].ToString();
			string virtualPath = string.Empty;
			string templateName = string.Empty;
			if (File.Exists(requestContext.HttpContext.Server.MapPath("~/") + pageName + (pageName.EndsWith(".aspx") ? "" : ".aspx")))
			{
				virtualPath = "~/" + pageName + (pageName.EndsWith(".aspx") ? "" : ".aspx");
				if (!String.IsNullOrEmpty(micrositeName))
					CMSHelpers.SetCurrentRequestCMSMicrosite(CMMicrosite.CMMicrositeGetByName(micrositeName.Replace("-", " ")).FirstOrDefault(), requestContext.HttpContext.ApplicationInstance.Context);
			}
			else
			{
				SEOData seo = SEOData.SEODataGetByFriendlyFilename(pageName).Find(s => s.Approved && (!s.LanguageID.HasValue || s.LanguageID == Helpers.GetCurrentLanguage().LanguageID));
				if (!String.IsNullOrEmpty(micrositeName) && seo == null)
					seo = SEOData.SEODataGetByFriendlyFilename(micrositeName + "/" + pageName).Find(s => s.Approved && (!s.LanguageID.HasValue || s.LanguageID == Helpers.GetCurrentLanguage().LanguageID));
				if (seo != null)
				{
					virtualPath = seo.PageURL;
					if (!String.IsNullOrEmpty(micrositeName))
						CMSHelpers.SetCurrentRequestCMSMicrosite(CMMicrosite.CMMicrositeGetByName(micrositeName.Replace("-", " ")).FirstOrDefault(), requestContext.HttpContext.ApplicationInstance.Context);
				}
				else
				{

					CMPage cmsPage = null;
					CMMicrosite micrositeEntity = null;
					if (Classes.ContentManager.Settings.EnableMicrosites)
					{
						micrositeEntity = CMMicrosite.CMMicrositeGetByName((!String.IsNullOrEmpty(micrositeName) ? micrositeName : pageName).Replace("-", " ")).FirstOrDefault();
						if (micrositeEntity != null && String.IsNullOrEmpty(micrositeName))
							cmsPage = CMPage.CMPagePage(0, 0, "", "", true, new CMPage.Filters { FilterCMPageFileName = "Home" + (Globals.Settings.RequireASPXExtensions ? ".aspx" : string.Empty), FilterCMPageDeleted = false.ToString(), FilterCMPageCMMicrositeID = micrositeEntity.CMMicroSiteID.ToString() }).FirstOrDefault(c => !c.OriginalCMPageID.HasValue);
					}
					if (cmsPage == null)
						cmsPage = CMPage.CMPagePage(0, 0, "", "", true, new CMPage.Filters { FilterCMPageDeleted = false.ToString(), FilterCMPageFileName = pageName, FilterCMPageCMMicrositeID = (micrositeEntity != null ? micrositeEntity.CMMicroSiteID.ToString() : null) }).FirstOrDefault(c => !c.OriginalCMPageID.HasValue);

					if (cmsPage == null || !cmsPage.CMTemplateID.HasValue) //No templateID means its a global region since programmed pages wouldn't be hitting this
						requestContext.HttpContext.Response.Redirect("~/" + (!String.IsNullOrEmpty(micrositeName) ? micrositeName + "/" : string.Empty) + "404.aspx?page=" + pageName);
					else
					{
						CMTemplate cmTemplate = CMTemplate.GetByID(cmsPage.CMTemplateID.Value);
						CMSHelpers.SetCurrentRequestCMSPage(cmsPage, requestContext.HttpContext.ApplicationInstance.Context);
						if (micrositeEntity != null)
							CMSHelpers.SetCurrentRequestCMSMicrosite(micrositeEntity, requestContext.HttpContext.ApplicationInstance.Context);
						templateName = cmTemplate.FileName;
						virtualPath = "~/" + templateName + "?filename=" + pageName;
					}
				}
			}
			//Showcase hardcoding fix
			virtualPath = virtualPath.Replace("~/home-details", "~/showcase-item.aspx");
			string queryString = ((virtualPath.Contains("?") ? "&" : "?") + requestContext.HttpContext.Request.QueryString).TrimEnd('?').TrimEnd('&');
			HttpContext.Current.RewritePath(virtualPath + queryString, false);
			try
			{
				return (Page)BuildManager.CreateInstanceFromVirtualPath(virtualPath.Split('?')[0], typeof(Page));
			}
			catch (Exception ex)
			{
				if (!String.IsNullOrEmpty(templateName) && ex.Message.Contains(templateName))
					throw new Exception("Could not load the page.  Does the CMS Template file '" + virtualPath.Split('?')[0] + "' exist?");
				else
					throw ex;
			}
		}
	}
}