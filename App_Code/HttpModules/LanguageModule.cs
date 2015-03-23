using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using Classes.SiteLanguages;

namespace BaseCode.HttpModules
{
	/// <summary>
	/// Initializes culture for multilingual support
	/// </summary>
	public class LanguageModule : IHttpModule
	{
		#region IHttpModule Members

		public void Dispose()
		{
		}

		public void Init(HttpApplication context)
		{
			context.PreRequestHandlerExecute += context_PreRequestHandlerExecute;
		}

		#endregion

		private void context_PreRequestHandlerExecute(object sender, EventArgs e)
		{
			Page page = HttpContext.Current.CurrentHandler as Page;
			if (page != null) page.PreInit += Page_PreInit;
		}

		private void Page_PreInit(object sender, EventArgs e)
		{
			if (HttpContext.Current != null)
			{
				if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["language"]) && Language.LanguageGetByCultureName(HttpContext.Current.Request.QueryString["language"].Split(';')[0]).Count > 0)
				{
					Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Request.QueryString["language"].Split(';')[0]);
					Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Request.QueryString["language"].Split(';')[0]);
					if (HttpContext.Current.Session != null)
						HttpContext.Current.Session["Language"] = Thread.CurrentThread.CurrentCulture.Name;
				}
				else if (HttpContext.Current.Session != null && HttpContext.Current.Session["Language"] != null)
				{
					Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Session["Language"].ToString().Split(';')[0]);
					Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Session["Language"].ToString().Split(';')[0]);
				}
				else
				{
					if (HttpContext.Current.Request.UserLanguages != null && Language.LanguageGetByCultureName(HttpContext.Current.Request.UserLanguages[0].Split(';')[0]).Count > 0)
					{
						Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Request.UserLanguages[0].Split(';')[0]);
						Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Request.UserLanguages[0].Split(';')[0]);
					}
					if (HttpContext.Current.Session != null)
						HttpContext.Current.Session["Language"] = Thread.CurrentThread.CurrentCulture.Name;
				}
			}
		}
	}
}