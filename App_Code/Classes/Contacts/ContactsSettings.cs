using Classes.ConfigurationSettings;

namespace Classes.Contacts
{
	public partial class Settings
	{
		public static string ContactSubmissionEmailAddress
		{
			get { return SiteSettings.GetSettingKeyValuePair()["Contacts_contactEmailAddress"]; }
		}

		public static string HomeValuationEmailAddress
		{
			get { return SiteSettings.GetSettingKeyValuePair()["Contacts_homeValueEmailAddress"]; }
		}

		public static string MaintenanceRequestEmailAddress
		{
			get { return SiteSettings.GetSettingKeyValuePair()["Contacts_maintenanceRequestEmailAddress"]; }
		}

		public static string DefaultAgentContactEmail
		{
			get { return SiteSettings.GetSettingKeyValuePair()["Contacts_defaultAgentContactEmail"]; }
		}

		public static string AgentContactCCEmailAddress
		{
			get { return SiteSettings.GetSettingKeyValuePair()["Contacts_agentContactCCEmailAddress"]; }
		}

		public static string PropertyInfoCCEmailAddress
		{
			get { return SiteSettings.GetSettingKeyValuePair()["Contacts_propertyInfoCCEmailAddress"]; }
		}
	}
}