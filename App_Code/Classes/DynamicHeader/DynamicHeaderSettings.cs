using System;
using System.Configuration;
using Classes.ConfigurationSettings;

namespace Classes.DynamicHeader
{
	public partial class Settings
	{
		/// <summary>
		/// Version number of the component
		/// </summary>
		public static string VersionNumber
		{
			get { return ConfigurationManager.AppSettings["DynamicHeader_componentVersion"]; }
		}

		/// <summary>
		/// This should be used by all paging methods done on the frontend to determine page size
		/// </summary>
		public static int DefaultCycleSpeed
		{
			get
			{
				if (SiteSettings.GetSettingKeyValuePair().ContainsKey("DynamicHeader_speed"))
					return Convert.ToInt32(SiteSettings.GetSettingKeyValuePair()["DynamicHeader_speed"]);
				return 10;
			}
		}

		/// <summary>
		/// Enabling this will add a rich text editor to the edit page that will display in a caption div on the frontend.
		/// </summary>
		public static bool EnableCaptions
		{
			get
			{
				return Convert.ToBoolean(ConfigurationManager.AppSettings["DynamicHeader_enableCaptions"]);
			}
		}
	}
}