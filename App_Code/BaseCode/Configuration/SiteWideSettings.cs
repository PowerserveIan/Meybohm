using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using Classes.ConfigurationSettings;

namespace BaseCode.Configuration
{
	public class SiteWideSettings
	{
		/// <summary>
		/// Used by a Helpers method, these tags are comma separated
		/// </summary>
		public readonly string AllowedHtmlTags = ConfigurationManager.AppSettings["SiteWide_AllowableCommentHtml"];

		/// <summary>
		/// The title of the site that will appear at the end of page titles
		/// </summary>
		public string SiteTitle
		{
			get { return SiteSettings.GetSettingKeyValuePair()["SiteWide_siteTitle"]; }
		}

		/// <summary>
		/// The client's company name, this will be used in the outgoing Newsletter
		/// </summary>
		public string CompanyName
		{
			get { return SiteSettings.GetSettingKeyValuePair()["SiteWide_companyName"]; }
		}

		/// <summary>
		/// This is used by all pagers in the admin section to determine the number of results returned on the listing page
		/// </summary>
		public int AdminPageSize
		{
			get { return Convert.ToInt32(SiteSettings.GetSettingKeyValuePair()["SiteWide_adminPageSize"]); }
		}

		/// <summary>
		/// This should be used by all paging methods done on the frontend to determine page size
		/// </summary>
		public int FrontEndPageSize
		{
			get { return Convert.ToInt32(SiteSettings.GetSettingKeyValuePair()["SiteWide_frontEndPageSize"]); }
		}

		/// <summary>
		/// Use this to get the connection string name specified in appSettings
		/// </summary>
		public string DefaultConnectionStringName
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["SiteWide_defaultConnectionStringName"]))
					return "LocalSqlServer";
				return ConfigurationManager.AppSettings["SiteWide_defaultConnectionStringName"];
			}
		}

		/// <summary>
		/// Turn caching on or off
		/// </summary>
		public bool EnableCaching
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["SiteWide_enableCaching"]))
					return true;
				return Convert.ToBoolean(ConfigurationManager.AppSettings["SiteWide_enableCaching"]);
			}
		}

		/// <summary>
		/// Unless overridden by a component, all database calls will have their results put into cache for this duration
		/// </summary>
		public int DefaultCacheDuration
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["SiteWide_defaultCacheDuration"]))
					return 10000;
				return Convert.ToInt32(ConfigurationManager.AppSettings["SiteWide_defaultCacheDuration"]);
			}
		}

		/// <summary>
		/// Live, local, development, etc.
		/// </summary>
		public string SiteStatus
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["SiteWide_siteStatus"]))
					return "Local";
				return ConfigurationManager.AppSettings["SiteWide_siteStatus"];
			}
		}

		/// <summary>
		/// If the site is live, use this to link to the location of the site
		/// </summary>
		public string LinkToSite
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["SiteWide_linkToSite"]))
					return "/";
				return ConfigurationManager.AppSettings["SiteWide_linkToSite"];
			}
		}

		public bool DisableDynamicSEOOutput
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["SiteWide_disableDynamicSEOOutput"]))
					return false;
				return Convert.ToBoolean(ConfigurationManager.AppSettings["SiteWide_disableDynamicSEOOutput"]);
			}
		}

		public string ConnectionString
		{
			get
			{
				string connStringName = DefaultConnectionStringName;
				return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
			}
		}

		/// <summary>
		/// Where emails will come from by default (can be overridden by components)
		/// </summary>
		public string FromEmail
		{
			get { return ConfigurationManager.AppSettings["SiteWide_fromEmail"]; }
		}

		/// <summary>
		/// List of paths specified as needing an SSL redirect
		/// </summary>
		public List<string> SSLPaths
		{
			get
			{
				string[] pageArray = ConfigurationManager.AppSettings["SiteWide_SSLPaths"].Split(',');
				return pageArray.Where(s => !String.IsNullOrWhiteSpace(s)).ToList();
			}
		}

		/// <summary>
		/// List of pages that should only be loaded insecurely (because of external content)
		/// </summary>
		public List<string> ProtocolInheritedPaths
		{
			get
			{
				string[] pageArray = ConfigurationManager.AppSettings["SiteWide_ProtocolInheritedPaths"].Split(',');
				return pageArray.Where(s=>!String.IsNullOrWhiteSpace(s)).ToList();
			}
		}

		/// <summary>
		/// Must be specified for SSL redirects, this is the address of the dev folder on a live site
		/// </summary>
		public string UrlHost
		{
			get { return ConfigurationManager.AppSettings["SiteWide_UrlHost"]; }
		}

		/// <summary>
		/// Returns whether web.config has been setup to allow every page on the site to use http compression.  Works with CompressionModule.cs
		/// </summary>
		public bool EnableHttpCompression
		{
			get { return Convert.ToBoolean(ConfigurationManager.AppSettings["SiteWide_EnableHttpCompression"]); }
		}

		/// <summary>
		/// Use this path to link to the default missing image path
		/// </summary>
		public string MissingImagePath
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["SiteWide_missingImagePath"]))
					return "img/missingFile.jpg";
				return ConfigurationManager.AppSettings["SiteWide_missingImagePath"];
			}
		}

		/// <summary>
		/// The base location all uploads should go to
		/// </summary>
		public string UploadFolder
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["SiteWide_UploadFolder"]))
					return "uploads/";
				return ConfigurationManager.AppSettings["SiteWide_UploadFolder"];
			}
		}

		/// <summary>
		/// News & Press feed used on Admin pages
		/// </summary>
		public string NewsFeed352Media
		{
			get { return ConfigurationManager.AppSettings["SiteWide_352MediaNewsFeed"]; }
		}

		/// <summary>
		/// Blog feed used on Admin pages
		/// </summary>
		public string BlogFeed352Media
		{
			get { return ConfigurationManager.AppSettings["SiteWide_352MediaBlogFeed"]; }
		}

		/// <summary>
		/// Component upgrade feed used on Admin pages
		/// </summary>
		public string ComponentFeed352Media
		{
			get { return ConfigurationManager.AppSettings["SiteWide_352MediaComponentFeed"]; }
		}

		/// <summary>
		/// Twitter feed used on Admin pages
		/// </summary>
		public string TwitterFeed352Media
		{
			get { return ConfigurationManager.AppSettings["SiteWide_352MediaTwitterFeed"]; }
		}

		/// <summary>
		/// Url to post 352 feedback to
		/// </summary>
		public string FeedbackPostUrl352Media
		{
			get { return ConfigurationManager.AppSettings["SiteWide_352MediaFeedbackUrl"]; }
		}

		/// <summary>
		/// Turns on/off the Like button on the site
		/// </summary>
		public bool FacebookEnableLikeButton
		{
			get { return Convert.ToBoolean(ConfigurationManager.AppSettings["Facebook_EnableLikeButton"]); }
		}

		/// <summary>
		/// Comma separated list of Facebook Admin IDs
		/// </summary>
		public string FacebookAdminIDs
		{
			get { return ConfigurationManager.AppSettings["Facebook_AdminIDs"]; }
		}

		public bool EnableParallelization { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["SiteWide_enableParallelization"]); } }

		/// <summary>
		/// The url of the css subdomain
		/// </summary>
		public string CSSSubdomain { get { return ConfigurationManager.AppSettings["SiteWide_CSSSubdomain"]; } }

		/// <summary>
		/// The url of the uploads subdomain
		/// </summary>
		public string UploadsSubdomain { get { return ConfigurationManager.AppSettings["SiteWide_UploadsSubdomain"]; } }

		/// <summary>
		/// The url of the resizer subdomain
		/// </summary>
		public string ResizerSubdomain { get { return ConfigurationManager.AppSettings["SiteWide_ResizerSubdomain"]; } }

		/// <summary>
		/// Some hosting companies will not allow routing through IIS.  ASPX extensions will ensure that ASP.NET handles the routing.
		/// </summary>
		public bool RequireASPXExtensions
		{
			get { return Convert.ToBoolean(ConfigurationManager.AppSettings["SiteWide_requiredASPXExtensions"]); }
		}

		/// <summary>
		/// The default timezone used for displaying times to users
		/// </summary>
		public string DefaultTimeZone
		{
			get { return SiteSettings.GetSettingKeyValuePair()["SiteWide_defaultDisplayTimeZone"]; }
		}

		/// <summary>
		/// Enable Search as You Type on Admin Listing Pages
		/// </summary>
		public bool AdminSearchAsYouType
		{
			get { return Convert.ToBoolean(SiteSettings.GetSettingKeyValuePair()["SiteWide_searchAsYouType"]); }
		}

		public string HashAlgorithm { get { return ConfigurationManager.AppSettings["SiteWide_hashAlgorithm"]; } }

		/// <summary>
		/// The 352 copyright that should appear on every frontend page (except the homepage)
		/// </summary>
		public string CompanyCopyrightMaster { get { return @"Designed by web design company 352 Media"; } }

		/// <summary>
		/// The 352 copyright that should appear on the homepage
		/// </summary>
		public string CompanyCopyrightHomepage
		{
			get
			{
				return @"Designed by <a href=""http://www.352media.com/"" target=""_blank"">web design company</a> 352 Media";
			}
		}

		public string HelpDeskUrl
		{
			get { return ConfigurationManager.AppSettings["HelpDesk_Url"]; }
		}
	}
}