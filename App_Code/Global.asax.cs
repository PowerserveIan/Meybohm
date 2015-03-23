using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Elmah;

namespace BaseCode
{
	public class Global : HttpApplication
	{
		protected void Application_Start(Object sender, EventArgs e)
		{
			AuthConfig.RegisterOpenAuth();
			System.Web.Optimization.BundleTable.Bundles.IgnoreList.Clear();
			RegisterRoutes(RouteTable.Routes);
			if (System.IO.Directory.Exists(Server.MapPath("~/" + Globals.Settings.UploadFolder + "temp/")))
				System.IO.Directory.Delete(Server.MapPath("~/" + Globals.Settings.UploadFolder + "temp/"), true);
			ImageRewriter_Application_Start();
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.Ignore("{*tinymce}", new { tinymce = @"(.*/)?TinyMCEHandler\.ashx.*" });
			routes.Ignore("{*facebook}", new { facebook = @"(.*/)?FacebookLogin\.ashx.*" });
			routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
			routes.Add(new Route("admin/{path}", new StopRoutingHandler()));
			routes.Add(new Route("css/{path}", new StopRoutingHandler()));
			routes.Add(new Route("img/{path}", new StopRoutingHandler()));
			routes.Add(new Route("images/{path}", new StopRoutingHandler()));
			routes.Add(new Route("tft-js/{path}", new StopRoutingHandler()));
			routes.Add(new Route("uploads/{path}", new StopRoutingHandler()));
			//Blog virtual pathing fix
			if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["BlogEngine.VirtualPath"]))
				routes.Add(new Route(System.Configuration.ConfigurationManager.AppSettings["BlogEngine.VirtualPath"].Replace("~/", "") + "{*path}", new StopRoutingHandler()));
			routes.Add(new Route("forum/{*path}", new StopRoutingHandler()));
			routes.MapPageRoute("HTCResources", "{resource}.htc", "~/{resource}.htc");
			routes.MapPageRoute("HomeValues", "{Microsite}/home-value", "~/home-valuation.aspx");
			routes.MapPageRoute("ShowcaseDetails", "{Microsite}/home-details", "~/showcase-item.aspx");
			routes.Add(new Route("{Microsite}/{CMPage}", new RoutingHandler()));
			routes.Add(new Route("{MicrositeOrCMPage}", new RoutingHandler()));
		}

		protected void Application_Error(Object sender, EventArgs e)
		{
			var f = Response.Filter;
			((HttpApplication)sender).Response.Filter = null;
			try
			{
				((HttpApplication)sender).Response.Headers.Remove("Content-encoding");
			}
			catch (Exception)
			{
				((HttpApplication)sender).Response.ClearHeaders();
			}
		}

		public void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
		{
		}

		protected void Session_Start(Object sender, EventArgs e)
		{
			//Jon requested the mobile redirect be turned off for now
			//if (!String.IsNullOrEmpty(Request.QueryString["stopMobileRedirect"]))
			//	Session["MobileUser"] = "1";
			//if (Session["MobileUser"] == null && Helpers.IsMobileBrowser())
			//	Response.Redirect("http://m.meybohm.com");
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			if (Request.Cookies["352 Client Login"] == null && !Request.AppRelativeCurrentExecutionFilePath.Equals("~/client-login.aspx", StringComparison.OrdinalIgnoreCase) && !Request.AppRelativeCurrentExecutionFilePath.ToLower().Contains("~/emailtemplates/") && !Request.IsLocal && System.IO.File.Exists(Server.MapPath("~/client-login.aspx")))
				Response.Redirect("~/client-login.aspx" + (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ReturnURL"]) ? "?" + HttpContext.Current.Request.QueryString.ToString() : "?ReturnURL=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.RawUrl)));

			Classes.Redirects.Redirect permaRedirect = Classes.Redirects.Redirect.GetAll().OrderByDescending(r => r.OldUrl.Length).FirstOrDefault(r => Request.Url.ToString().ToLower().StartsWith(Helpers.ReplaceRootWithAbsolutePath(r.OldUrl).ToLower()));
			if (permaRedirect != null)
				Helpers.PermanentRedirect(Helpers.ReplaceRootWithAbsolutePath(permaRedirect.NewUrl), HttpContext.Current);
			string urlHost = string.Empty;

			if (!String.IsNullOrEmpty(Globals.Settings.UrlHost) && Globals.Settings.UrlHost != "TO BE SET ON LAUNCH")
				urlHost = Globals.Settings.UrlHost;

			if (!String.IsNullOrWhiteSpace(urlHost))
			{
				string pathInfo = Helpers.GetFileName();
				
				List<string> sslPaths = Globals.Settings.SSLPaths;

				List<string> protocolInheritedPaths = Globals.Settings.ProtocolInheritedPaths;

				bool useSecure = sslPaths.Any(s => pathInfo.Contains(s.ToLower()));
				string requestUrlHost = HttpContext.Current.Request.Url.Host;

				bool redirectForURLHost = !requestUrlHost.Equals(urlHost.Split('/')[0], StringComparison.OrdinalIgnoreCase) && (!Globals.Settings.EnableParallelization || (!requestUrlHost.Equals(Globals.Settings.CSSSubdomain.TrimStart('/').TrimEnd('/'), StringComparison.OrdinalIgnoreCase) && !requestUrlHost.Equals(Globals.Settings.UploadsSubdomain.TrimStart('/').TrimEnd('/'), StringComparison.OrdinalIgnoreCase) && !requestUrlHost.Equals(Globals.Settings.ResizerSubdomain.TrimStart('/').TrimEnd('/'), StringComparison.OrdinalIgnoreCase)));
				bool redirectForHTTPS = !HttpContext.Current.Request.IsSecureConnection && useSecure;
                bool redirectForHTTP = HttpContext.Current.Request.IsSecureConnection && !useSecure && !protocolInheritedPaths.Any(s => pathInfo.Contains(s.ToLower()));
                var isPowerserveHost = HttpContext.Current.Request.IsLocal || HttpContext.Current.Request.Url.Host.Contains("powerserve");

                if (isPowerserveHost || (!redirectForURLHost && !redirectForHTTP && !redirectForHTTPS))
					return;

				string urlRedirect = "http" + (useSecure ? "s" : "") + "://" + urlHost.TrimEnd('/') + "/" + pathInfo + (!String.IsNullOrWhiteSpace(HttpContext.Current.Request.QueryString.ToString()) ? "?" + HttpContext.Current.Request.QueryString.ToString() : "");
				Helpers.PermanentRedirect(urlRedirect, HttpContext.Current);
			}
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{
		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
		}

		protected void Session_End(Object sender, EventArgs e)
		{
		}

		protected void Application_End(Object sender, EventArgs e)
		{
		}

		void ImageRewriter_Application_Start()
		{
			ImageResizer.Configuration.Config.Current.Pipeline.RewriteDefaults += delegate(IHttpModule m, HttpContext c, ImageResizer.Configuration.IUrlEventArgs args)
			{
				if (args.VirtualPath.IndexOf("/uploads/", StringComparison.OrdinalIgnoreCase) > -1)
					args.QueryString["404"] = "~/" + Globals.Settings.MissingImagePath;
			};
		}

		void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs args)
		{
			Filter(args);
		}

		void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs args)
		{
			Filter(args);
		}

		void Filter(ExceptionFilterEventArgs args)
		{
			string userAgent = ((HttpContext)args.Context).Request.ServerVariables["HTTP_USER_AGENT"];
			if (args.Exception.Message.Contains("Request format is unrecognized for URL unexpectedly ending in '") ||
				args.Exception.Message.Contains("This is an invalid webresource request") ||
				args.Exception.Message.Contains("Invalid viewstate") ||
				args.Exception.Message.Contains("A potentially dangerous Request.Path value was detected from") ||
				(args.Exception.InnerException != null && (args.Exception.InnerException.Message.Contains("Invalid viewstate"))) ||
				((HttpContext)args.Context).Request.AppRelativeCurrentExecutionFilePath.ToLower().Contains("foaf.axd") ||
				(!string.IsNullOrWhiteSpace(userAgent) && (
						userAgent.Contains("Sosospider")
						|| userAgent.Contains("Baiduspider")
						|| userAgent.Contains("Sogou web spider")
						|| userAgent.Contains("bingbot")
						|| userAgent.Contains("AhrefsBot")
						|| userAgent.Contains("Ezooms")
						|| userAgent.ToLower().Contains("spam")
						|| userAgent.ToLower().Contains("bot"))))
				args.Dismiss();
		}
	}
}
