using System;
using System.Configuration;
using Classes.ConfigurationSettings;

namespace Classes.ContentManager
{
	public partial class Settings
	{
		/// <summary>
		/// Version number of the component
		/// </summary>
		public static string VersionNumber
		{
			get { return ConfigurationManager.AppSettings["ContentManager_componentVersion"]; }
		}

		/// <summary>
		/// Sets the level of access for the client.  0 gives limited access, 1 gives full access
		/// </summary>
		public static int ClientCMLevel
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ContentManager_clientCMLevel"]))
					return 1;
				return Convert.ToInt32(ConfigurationManager.AppSettings["ContentManager_clientCMLevel"]);
			}
		}

		/// <summary>
		/// Sets the number of levels deep a tree may go in the sitemap.  Currently supported max is 3.
		/// </summary>
		public static int DepthLimit
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ContentManager_depthLimit"]))
					return 3;
				return Convert.ToInt32(ConfigurationManager.AppSettings["ContentManager_depthLimit"]);
			}
		}

		/// <summary>
		/// Forcing One to One is required to have fully functional filename to sitemap item mappings.
		/// Once disabled, with many to one sitemaps to pages, menus may start from the wrong location.  Please
		/// fully consider this before changing.
		/// </summary>
		public static bool ForceOneToOne
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ContentManager_forceOneToOne"]))
					return true;
				return Convert.ToBoolean(ConfigurationManager.AppSettings["ContentManager_forceOneToOne"]);
			}
		}

		/// <summary>
		/// Microsite Admins will only be able to access the sitemap if this is set
		/// </summary>
		public static bool AllowMicrositeAdminToEditSitemap
		{
			get
			{
				if (SiteSettings.GetSettingKeyValuePair().ContainsKey("ContentManager_allowMicrositeAdminSitemap"))
					return Convert.ToBoolean(SiteSettings.GetSettingKeyValuePair()["ContentManager_allowMicrositeAdminSitemap"]);
				return false;
			}
		}

		/// <summary>
		/// Changes to the Microsite Default will only affect existing microsites if this is checked
		/// </summary>
		public static bool MicrositeDefaultChangesAffectExistingMicrosites
		{
			get
			{
				if (SiteSettings.GetSettingKeyValuePair().ContainsKey("ContentManager_micrositeDefaultChangesAffectExisting"))
					return Convert.ToBoolean(SiteSettings.GetSettingKeyValuePair()["ContentManager_micrositeDefaultChangesAffectExisting"]);
				return false;
			}
		}

		/// <summary>
		/// Turns microsites on/off
		/// </summary>
		public static bool EnableMicrosites
		{
			get { return Convert.ToBoolean(ConfigurationManager.AppSettings["ContentManager_enableMicrosites"]); }
		}

		/// <summary>
		/// Toggles support for multiple languages
		/// </summary>
		public static bool EnableMultipleLanguages
		{
			get { return Convert.ToBoolean(ConfigurationManager.AppSettings["ContentManager_enableMultipleLanguages"]); }
		}

		/// <summary>
		/// Limit access to CMS pages to users in specific roles
		/// </summary>
		public static bool EnableCMPageRoles
		{
			get
			{
				if (SiteSettings.GetSettingKeyValuePair().ContainsKey("ContentManager_enableCMPageRoles"))
					return Convert.ToBoolean(SiteSettings.GetSettingKeyValuePair()["ContentManager_enableCMPageRoles"]);
				return false;
			}
		}

		/// <summary>
		/// Setting this will only show pages in the menu that a user may access
		/// </summary>
		public static bool HideMembersAreaPagesInMenu
		{
			get
			{
				if (SiteSettings.GetSettingKeyValuePair().ContainsKey("ContentManager_hideMembersAreaPagesInMenu"))
					return Convert.ToBoolean(SiteSettings.GetSettingKeyValuePair()["ContentManager_hideMembersAreaPagesInMenu"]);
				return false;
			}
		}

		/// <summary>
		/// Makes all changes to the CMS done by CMS Content Integrators or CMS Page Managers require approval before going live
		/// Turning this setting off will only allow any future changes to go straight to live,
		/// an Admin will still have to approve all existing unapproved content
		/// </summary>
		public static bool EnableApprovals
		{
			get { return Convert.ToBoolean(ConfigurationManager.AppSettings["ContentManager_enableApprovals"]); }
		}

		/// <summary>
		/// Admins will receive emails when an editor makes a change
		/// Editors will receive an email when another Editor changes their content or the Admin approves/denies their content
		/// </summary>
		public static bool SendApprovalEmails
		{
			get
			{
				if (SiteSettings.GetSettingKeyValuePair().ContainsKey("ContentManager_approvalEmailAlerts"))
					return Convert.ToBoolean(SiteSettings.GetSettingKeyValuePair()["ContentManager_approvalEmailAlerts"]);
				return false;
			}
		}

		/// <summary>
		/// Semi-colon separated list of Admins who will receive Approval Alert emails (if enabled)
		/// </summary>
		public static string ApprovalAdminEmailAddresses
		{
			get
			{
				if (SiteSettings.GetSettingKeyValuePair().ContainsKey("ContentManager_approvalAdminEmailAddresses"))
					return SiteSettings.GetSettingKeyValuePair()["ContentManager_approvalAdminEmailAddresses"];
				return "";
			}
		}

		/// <summary>
		/// If set each language will have its own sitemap, otherwise, all will use the same sitemap.
		/// </summary>
		public static bool MultilingualManageSiteMapsIndividually
		{
			get
			{
				if (SiteSettings.GetSettingKeyValuePair().ContainsKey("ContentManager_manageSiteMapsIndividually"))
					return Convert.ToBoolean(SiteSettings.GetSettingKeyValuePair()["ContentManager_manageSiteMapsIndividually"]);
				return false;
			}
		}

		/// <summary>
		/// Email address that will receive submissions via global contact forms
		/// </summary>
		public static string GlobalContactEmailAddress
		{
			get
			{
				if (SiteSettings.GetSettingKeyValuePair().ContainsKey("ContentManager_globalContactFormsEmailTo"))
					return SiteSettings.GetSettingKeyValuePair()["ContentManager_globalContactFormsEmailTo"];
				return "";
			}
		}

		/// <summary>
		/// Used to determine whether or not any database settings exist for the CMS
		/// </summary>
		public static bool CMSConfigSettingsExist
		{
			get { return SiteSettings.GetByComponentName("ContentManager").Count > 0; }
		}
	}
}