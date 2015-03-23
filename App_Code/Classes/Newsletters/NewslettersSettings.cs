using System;
using System.Configuration;
using System.Net.Mail;
using BaseCode;
using Classes.ConfigurationSettings;

namespace Classes.Newsletters
{
	public partial class Settings
	{
		public static SmtpClient SMTP;

		/// <summary>
		/// Version number of the component
		/// </summary>
		public static string VersionNumber
		{
			get { return ConfigurationManager.AppSettings["Newsletters_componentVersion"]; }
		}

		/// <summary>
		/// Turn this on to limit the number of mailing lists and subscribers per mailing list that can be added
		/// (use this to prevent spam or malicious users)
		/// </summary>
		public static bool EnableMailingListLimitations
		{
			get
			{
				string enabled = ConfigurationManager.AppSettings["Newsletters_enableMailingListLimitations"];
				if (String.IsNullOrEmpty(enabled))
					return false;
				return Convert.ToBoolean(enabled);
			}
		}

		/// <summary>
		/// If EnableMailingListLimitations is true, this will be the max number of mailing lists to allow the Admin to create
		/// </summary>
		public static int MaxNumberMailingLists
		{
			get { return Convert.ToInt32(ConfigurationManager.AppSettings["Newsletters_maxNumberMailingLists"]); }
		}

		/// <summary>
		/// If EnableMailingListLimitations is true, this will be the max number of subscribers that can be added to a single
		/// mailing list
		/// </summary>
		public static int MaxNumberSubscribers
		{
			get { return Convert.ToInt32(ConfigurationManager.AppSettings["Newsletters_maxNumberSubscribers"]); }
		}

		/// <summary>
		/// If EnableMailingListLimitations is true, this error message will appear if the Admin tries to add more 
		/// subscribers than the current max
		/// </summary>
		public static string MaxNumberSubscribersErrorMessage
		{
			get { return ConfigurationManager.AppSettings["Newsletters_maxNumberSubscribersErrorMessage"].Replace("[maxNumberSubscribers]", MaxNumberSubscribers.ToString()); }
		}

		/// <summary>
		/// If EnableMailingListLimitations is true, this error message will appear if the Admin tries to add more 
		/// mailing lists than the current max
		/// </summary>
		public static string MaxNumberMailingListsErrorMessage
		{
			get { return ConfigurationManager.AppSettings["Newsletters_maxNumberMailingListsErrorMessage"].Replace("[maxNumberMailingLists]", MaxNumberMailingLists.ToString()); }
		}

		public static string CustomWelcomeMessage
		{
			get { return Helpers.ReplaceRootWithAbsolutePath(SiteSettings.GetSettingKeyValuePair()["Newsletters_welcomeMessage"]); }
		}

		/// <summary>
		/// The physical mailing address of the client's company
		/// </summary>
		public static string CustomPhysicalMailAddress
		{
			get { return SiteSettings.GetSettingKeyValuePair()["Newsletters_physicalMailAddress"]; }
		}

		/// <summary>
		/// The mail server responsible for sending out the Newsletter
		/// </summary>
		public static string CustomMailServer
		{
			get { return ConfigurationManager.AppSettings["Newsletters_mailServer"]; }
		}

		/// <summary>
		/// The email address where Newsletters will come from
		/// </summary>
		public static string CustomSenderEmail
		{
			get { return SiteSettings.GetSettingKeyValuePair()["Newsletters_senderEmail"]; }
		}

		public static string TextOnlyDomains
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["Newsletters_textOnlyDomains"]))
					return "";
				return ConfigurationManager.AppSettings["Newsletters_textOnlyDomains"];
			}
		}

		/// <summary>
		/// HTMLOnly, TextOnly, Multipart.
		/// </summary>
		public static NewsletterSendingTypeFormat NewsletterSendingType
		{
			get
			{
				string sendingType = ConfigurationManager.AppSettings["Newsletters_newsletterSendingType"];

				if (sendingType.ToLower().Equals("htmlonly"))
					return NewsletterSendingTypeFormat.HtmlOnly;
				if (sendingType.ToLower().Equals("textonly"))
					return NewsletterSendingTypeFormat.TextOnly;
				if (sendingType.ToLower().Equals("multipart"))
					return NewsletterSendingTypeFormat.Multipart;
				return NewsletterSendingTypeFormat.HtmlAndText;
			}
		}

		/// <summary>
		/// Set to 0 to turn off.  If set, admin will not be able to send more than the number of emails specified per month through the Newsletter component.
		/// </summary>
		public static int MaxEmailsPerMonth
		{
			get { return Convert.ToInt32(ConfigurationManager.AppSettings["Newsletters_maxEmailsPerMonth"]); }
		}
		

		/// <summary>
		/// Default MailServer used to send out Newsletters (overwritten by CustomMailServer)
		/// </summary>
		public static string MailServer
		{
			get
			{
				// Return the base class' MailServer property.
				// The name of the MailServer to use is retrieved from the site's 
				// custom config section
				// If no MailServer is defined for the <Mail> element, the
				// site's default smtp setting will be used.
				if (SMTP == null)
					SMTP = new SmtpClient();

				return (string.IsNullOrEmpty(CustomMailServer) ?
				                                               	SMTP.Host : CustomMailServer);
			}
		}

		public static string SenderEmail
		{
			get
			{
				// Return the base class' SenderEmail property.
				// The name of the SenderEmail to use is retrieved from the site's 
				// custom config section
				// If no SenderEmail is defined for the <Mail> element, the
				// site's default smtp setting will be used.

				return (string.IsNullOrEmpty(CustomSenderEmail) ?
				                                                	Globals.Settings.FromEmail : CustomSenderEmail);
			}
		}
	}
}