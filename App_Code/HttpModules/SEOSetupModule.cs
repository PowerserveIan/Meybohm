using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using Classes.SEOComponent;

namespace BaseCode.HttpModules
{
	/// <summary>
	/// Sets up the page SEO data such as page title and metatags.  Uses the SEOComponent
	/// </summary>
	public class SEOSetupModule : IHttpModule
	{
		public void Dispose()
		{
		}

		public void Init(HttpApplication context)
		{
			context.PreRequestHandlerExecute += SEOSetupModule_PreRequestHandlerExecute;
			context.PreSendRequestHeaders += RemoveEtag_PreSendRequestHeaders;
		}

		void RemoveEtag_PreSendRequestHeaders(object sender, EventArgs e)
		{
			if (HttpContext.Current != null && HttpContext.Current.Response != null && HttpRuntime.UsingIntegratedPipeline && HttpContext.Current.Response.Headers != null)
				HttpContext.Current.Response.Headers.Remove("ETag");
		}

		private void SEOSetupModule_PreRequestHandlerExecute(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			if ((app.Context.CurrentHandler is Page))
			{
				Page pg = app.Context.CurrentHandler as Page;
				pg.Load += InitialPage_Load;
				pg.PreInit += Page_PreInit;
			}
		}

		/// <summary>
		/// Called when the page is being initially loaded (before the page load itself happpens.  Sets up the page title and meta data
		/// </summary>
		private void InitialPage_Load(object sender, EventArgs e)
		{
			if (HttpContext.Current != null)
				LoadSEOData(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath, HttpContext.Current.Request.QueryString.ToString());
		}

		public static void LoadSEOData(string pageURL, string queryString)
		{
			Page page = HttpContext.Current.CurrentHandler as Page;
			StringBuilder titleBuilder = new StringBuilder();
			string oldPageTitle = string.Empty;
			if (page != null && page.Header != null)
			{
				oldPageTitle = page.Title;
				titleBuilder.Append(Globals.Settings.SiteTitle);
				if (!String.IsNullOrEmpty(page.Title))
					page.Title = page.Title + " - " + titleBuilder;
				else
					page.Title = titleBuilder.ToString();

				if (HttpContext.Current != null)
					page.Title = HttpContext.Current.Server.HtmlEncode(page.Title);
				else
					page.Title = page.Title.Replace("& ", "&amp; ");
			}
			//Check if SEO output is desired
			//Don't hit database for Admin pages
			if (!Config.Settings.DisableDynamicSEOOutput && !pageURL.ToLower().Contains("admin/") && !pageURL.ToLower().Contains("blog/") && !pageURL.ToLower().Contains("forum/") && !pageURL.ToLower().Contains("tft-js/core/") && page != null && !pageURL.ToLower().Contains("session-keep-alive.aspx"))
			{
				if (!String.IsNullOrEmpty(page.Request.QueryString["filename"]))
				{
					pageURL = "~/" + Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSPagePathWithMicrosite();
					queryString = queryString.Replace("filename=" + page.Request.QueryString["filename"], string.Empty);
				}
				
				//Get the data from the database for the possible page - URL
				SEOData currentSEO = SEOData.GetSEOForSpecificPath(pageURL, queryString);
				if (currentSEO != null)
				{
					//Using this in case it is decided to setup somethig for the title besides what is typed in
					if (!string.IsNullOrEmpty(currentSEO.Title) && !currentSEO.Title.Equals("Default Title"))
						titleBuilder.Insert(0, currentSEO.Title + " - ");
					else
						titleBuilder.Insert(0, String.IsNullOrEmpty(oldPageTitle) ? "" : oldPageTitle + " - ");

					if (page.Header != null)
					{
						if (HttpContext.Current != null)
							page.Title = HttpContext.Current.Server.HtmlEncode(titleBuilder.ToString());
						else
							page.Title = titleBuilder.ToString().Replace("& ", "&amp; ");

						page.MetaDescription = currentSEO.Description + (!String.IsNullOrEmpty(page.MetaDescription) ? ", " + page.MetaDescription : "");
						page.MetaKeywords = currentSEO.Keywords + (!String.IsNullOrEmpty(page.MetaKeywords) ? ", " + page.MetaKeywords : "");
					}
				}
			}
		}

		private void Page_PreInit(object sender, EventArgs e)
		{
		}
	}
}