using Classes.ConfigurationSettings;

namespace Classes.Rets
{
	public partial class Settings
	{
		public static string RetsFailEmail1
		{
			get { return SiteSettings.GetSettingKeyValuePair()["Rets_RetsAlertEmailAddress1"]; }
		}

		public static string RetsFailEmail2
		{
			get { return SiteSettings.GetSettingKeyValuePair()["Rets_RetsAlertEmailAddress2"]; }
		}

		public static string RetsFailEmail3
		{
			get { return SiteSettings.GetSettingKeyValuePair()["Rets_RetsAlertEmailAddress3"]; }
		}


	}
}