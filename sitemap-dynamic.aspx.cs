using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using BaseCode;

public partial class sitemap_dynamic : System.Web.UI.Page
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		List<DynamicSitemap_URLAndTitle> siteMapItems;
		using (Entities entity = new Entities())
		{
			siteMapItems = entity.SiteWide_GetDynamicSitemap().ToList();
		}
		XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
		XElement urlset = new XElement(ns + "urlset",
										from dr in siteMapItems
										where !dr.URL.Contains("://")
										select new XElement(ns + "url",
															new XElement(ns + "loc", Helpers.RootPath + (dr.URL.Contains("[BLOGROOT]") ? dr.URL.Replace("[BLOGROOT]", ConfigurationManager.AppSettings["BlogEngine.VirtualPath"].Replace("~/", "")) : dr.URL) + Server.UrlEncode(dr.Title))
										 ));

		string xml = Classes.SEOComponent.SEOData.ReplaceHtmlWithFriendlyFilenames(urlset.ToString());
		try
		{
			Response.ContentType = "text/xml";
			Response.Write(@"<?xml version=""1.0"" encoding=""utf-8""?>" + Environment.NewLine);
			Response.Write(xml);
		}
		catch (Exception ex)
		{
			Helpers.LogException(ex);
		}
	}
}