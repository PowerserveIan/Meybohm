using System;
using System.Configuration;
using BaseCode;
using Classes.ConfigurationSettings;

namespace Classes.Media352_MembershipProvider
{
	public partial class Settings
	{
		/// <summary>
		/// Version number of the component
		/// </summary>
		public static string VersionNumber
		{
			get { return ConfigurationManager.AppSettings["Media352_MembershipProvider_componentVersion"]; }
		}

		/// <summary>
		/// Simple or Complex, Simple disables Roles and the Profile control in the Admin
		/// </summary>
		public static UserManagerType UserManager
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["Media352_MembershipProvider_UserManager"]) || ConfigurationManager.AppSettings["Media352_MembershipProvider_UserManager"].Equals("Simple", StringComparison.OrdinalIgnoreCase))
					return UserManagerType.Simple;
				return UserManagerType.Complex;
			}
		}

		/// <summary>
		/// Subject of email that gets sent out to a user requesting his password
		/// </summary>
		public static string LostPasswordSubject
		{
			get
			{
				if (String.IsNullOrEmpty(SiteSettings.GetSettingKeyValuePair()["Media352_MembershipProvider_lostPasswordSubject"]))
					return "Here is your password";
				return SiteSettings.GetSettingKeyValuePair()["Media352_MembershipProvider_lostPasswordSubject"];
			}
		}

		/// <summary>
		/// The address that the lost password email will be sent from
		/// </summary>
		public static string LostPasswordEmailFrom
		{
			get
			{
				if (String.IsNullOrEmpty(SiteSettings.GetSettingKeyValuePair()["Media352_MembershipProvider_lostPasswordEmailFrom"]))
					return Globals.Settings.FromEmail;
				return SiteSettings.GetSettingKeyValuePair()["Media352_MembershipProvider_lostPasswordEmailFrom"];
			}
		}

		/// <summary>
		/// The body of the email that will be sent out when a user requests his password
		/// </summary>
		public static string LostPasswordText
		{
			get
			{
				if (String.IsNullOrEmpty(SiteSettings.GetSettingKeyValuePair()["Media352_MembershipProvider_lostPasswordText"]))
					return "Here is your password";
				return Helpers.ReplaceRootWithAbsolutePath(SiteSettings.GetSettingKeyValuePair()["Media352_MembershipProvider_lostPasswordText"]);
			}
		}

		/// <summary>
		/// Removes the user name field and makes the user's email address their user name
		/// </summary>
		public static bool UserNameIsEmail
		{
			get { return Convert.ToBoolean(ConfigurationManager.AppSettings["Media352_MembershipProvider_userNameIsEmail"]); }
		}

		/// <summary>
		/// Enables security questions
		/// </summary>
		public static bool SecurityQuestionRequired
		{
			get { return Convert.ToBoolean(ConfigurationManager.AppSettings["Media352_MembershipProvider_securityQuestionRequired"]); }
		}

		/// <summary>
		/// Whether or not users are approved as members by default upon registering
		/// </summary>
		public static bool UsersApprovedByDefault
		{
			get
			{
				if (String.IsNullOrEmpty(SiteSettings.GetSettingKeyValuePair()["Media352_MembershipProvider_usersApprovedByDefault"]))
					return true;
				return Convert.ToBoolean(SiteSettings.GetSettingKeyValuePair()["Media352_MembershipProvider_usersApprovedByDefault"]);
			}
		}

		public static string ActiveDirectoryPath
		{
			get { return ConfigurationManager.AppSettings["ActiveDirectory_Server"]; }
		}

		public static string LostPasswordEmailAddressForStaff
		{
			get
			{
				if (String.IsNullOrEmpty(SiteSettings.GetSettingKeyValuePair()["Media352_MembershipProvider_lostPasswordEmailAddressForStaff"]))
					return Globals.Settings.FromEmail;
				return SiteSettings.GetSettingKeyValuePair()["Media352_MembershipProvider_lostPasswordEmailAddressForStaff"];
			}
		}
	}
}