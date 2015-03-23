using System;
using System.Configuration;
using System.Web.Configuration;
using BaseCode;
using Classes.ConfigurationSettings;

/******************************************************************************/
/*   DO NOT MAKE CHANGES TO THIS FILE.  IT WAS GENERATED BY A CODE GENERATOR. */
/*   ANY CHANGES WILL BE OVERWRITTEN.                                         */
/******************************************************************************/

namespace Classes.Media352_MembershipProvider
{
	public partial class Settings
	{
		public static string ConnectionStringName
		{
			get { return ConfigurationManager.AppSettings["Media352_MembershipProvider_connectionStringName"]; }
		}

		public static string ConnectionString
		{
			get
			{
				// Unless a custom connection string is specified, this method will return the connection
				// string from _Settings_ConnectionString.config.
				string connStringName = (string.IsNullOrEmpty(Settings.ConnectionStringName) ? Globals.Settings.DefaultConnectionStringName : Settings.ConnectionStringName);
				return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
			}
		}

		/// <summary>
		/// This should be used by all paging methods done on the frontend to determine page size
		/// </summary>
		public static int FrontEndPageSize
		{
			get
			{   
				if (SiteSettings.GetSettingKeyValuePair().ContainsKey("Media352_MembershipProvider_frontEndPageSize"))
					return Convert.ToInt32(SiteSettings.GetSettingKeyValuePair()["Media352_MembershipProvider_frontEndPageSize"]);
				return Globals.Settings.FrontEndPageSize;
			}
		}

		/// <summary>
		/// Turn caching on or off
		/// </summary>
		public static bool EnableCaching
		{
			get 
			{ 
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["Media352_MembershipProvider_enableCaching"]))
					return Globals.Settings.EnableCaching;
				return Convert.ToBoolean(ConfigurationManager.AppSettings["Media352_MembershipProvider_enableCaching"]); 
			}
		}

		/// <summary>
		/// All database calls will have their results put into cache for this duration
		/// </summary>
		public static int CacheDuration
		{
			get
			{   
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["Media352_MembershipProvider_cacheDuration"]))
					return Globals.Settings.DefaultCacheDuration;
				int duration = Convert.ToInt32(ConfigurationManager.AppSettings["Media352_MembershipProvider_cacheDuration"]);
				return (duration > 0 ? duration : Globals.Settings.DefaultCacheDuration);
			}
		}
	}
}