using System;
using System.Configuration;
using Classes.ConfigurationSettings;

namespace Classes.Media352_NewsPress
{
	public partial class Settings
	{
		/// <summary>
		/// Version number of the component
		/// </summary>
		public static string VersionNumber
		{
			get { return ConfigurationManager.AppSettings["Media352_NewsPress_componentVersion"]; }
		}

		/// <summary>
		/// Enable category sorting for News & Press Releases
		/// </summary>
		public static bool EnableCategories
		{
			get { return Convert.ToBoolean(SiteSettings.GetSettingKeyValuePair()["Media352_NewsPress_enableCategories"]); }
		}

		public static ArchiveTypes ArchiveType
		{
			get
			{
				string archiveType = SiteSettings.GetSettingKeyValuePair()["Media352_NewsPress_archiveType"];
				if (archiveType.Equals("NumCurrentArticles", StringComparison.OrdinalIgnoreCase))
					return ArchiveTypes.NumCurrentArticles;
				if (archiveType.Equals("ArchiveAfterNumDays", StringComparison.OrdinalIgnoreCase))
					return ArchiveTypes.ArchiveAfterNumDays;
				return ArchiveTypes.ManualArchiving;
			}
		}

		/// <summary>
		/// The number of articles to show as non-archived
		/// </summary>
		public static int NumCurrentArticles
		{
			get { return Convert.ToInt32(SiteSettings.GetSettingKeyValuePair()["Media352_NewsPress_numCurrentArticles"]); }
		}

		/// <summary>
		/// The number of days old an article can be before it is considered archived
		/// </summary>
		public static int NumDaysToKeepCurrent
		{
			get { return Convert.ToInt32(SiteSettings.GetSettingKeyValuePair()["Media352_NewsPress_archiveAfterNumDays"]); }
		}
	}
}