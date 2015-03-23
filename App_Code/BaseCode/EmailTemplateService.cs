using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Routing;

namespace BaseCode
{
	public static class EmailTemplates
	{
		private const string Directory = "~/EmailTemplates/";

		/// <summary>
		/// [[PageName]]
		/// [[FormFields]]
		/// </summary>
		public const string CMSFormPost = Directory + "ContentManager/formPost.htm";

		/// <summary>
		/// [[Tracking]]
		/// [[BodyText]]
		/// [[BeginPerOrderBodyText]]
		/// [[PerOrderBodyText]]
		/// [[EndPerOrderBodyText]]
		/// [[OrderNumber]]
		/// [[OrderInfoView]]
		/// </summary>
		public const string EcommerceOrder = Directory + "Ecommerce/default.htm";

		/// <summary>
		/// [[Message]]
		/// [[Title]]
		/// [[Date]]
		/// [[Time]]
		/// [[Location]]
		/// [[Description]]
		/// [[LinkToEvent]]
		/// </summary>
		public const string EventsTellAFriend = Directory + "Events/default.htm";

		/// <summary>
		/// [[Body]]
		/// </summary>
		public const string MasterEmailTemplate = Directory + "email-template.html";

		/// <summary>
		/// [[Body]]
		/// [[BeginRequired]]
		/// [[UserName]]
		/// [[GUID]]
		/// [[EndRequired]]
		/// </summary>
		public const string MembershipPasswordRecovery = Directory + "Membership/LostPasswordMail.htm";

		/// <summary>
		/// [[Email Only]]
		/// [[NewsletterLink]]
		/// [[NewsletterSender]]
		/// [[End Email Only]]
		/// [[NewsletterTitle]]
		/// [[NewsletterIssue]]
		/// [[NewsletterDate]]
		/// [[NewsletterBody]]
		/// [[FullCompanyName]]
		/// [[FullPhysicalMailAddress]]
		/// [[UnsubscribeLink]]
		/// [[ForwardLink]]
		/// </summary>
		public const string NewsletterDefaultTemplate = Directory + "Newsletter/default.htm";

		/// <summary>
		/// [[BodyText]]
		/// [[FirstName]]
		/// [[LastName]]
		/// [[Address1]]
		/// [[Address2]]
		/// [[City]]
		/// [[StateAbb]]
		/// [[Zipcode]]
		/// [[Country]]
		/// [[Email]]
		/// [[Phone]]
		/// [[Amount]]
		/// </summary>
		public const string OpenPaymentClient = Directory + "OpenPayment/default.htm";

		/// <summary>
		/// [[BodyText]]
		/// [[FirstName]]
		/// [[LastName]]
		/// [[Address1]]
		/// [[Address2]]
		/// [[City]]
		/// [[StateAbb]]
		/// [[Zipcode]]
		/// [[Country]]
		/// [[Email]]
		/// [[Phone]]
		/// [[Amount]]
		/// [[AdditionalInfo]]
		/// </summary>
		public const string OpenPaymentAdmin = Directory + "OpenPayment/admin.htm";

		/// <summary>
		/// [[FirstName]]
		/// [[LastName]]
		/// [[Message]]
		/// [[ContactMethod]]
		/// [[PhoneOrEmail]]
		/// [[ContactTime]]
		/// </summary>
		public const string ContactSubmission = Directory + "Contacts/contact.htm";

		/// <summary>
		/// [[FirstName]]
		/// [[LastName]]
		/// [[Message]]
		/// [[ContactMethod]]
		/// [[PhoneOrEmail]]
		/// [[ContactTime]]
		/// </summary>
		public const string ContactAgentSubmission = Directory + "Contacts/contact-agent.htm";		

		/// <summary>
		/// [[FirstName]]
		/// [[LastName]]
		/// [[Message]]
		/// [[ContactMethod]]
		/// [[PhoneOrEmail]]
		/// [[ContactTime]]
		/// [[Address1]]
		/// [[Address2]]
		/// [[City]]
		/// [[State]]
		/// [[Zip]]
		/// </summary>
		public const string HomeValuation = Directory + "Contacts/home-valuation-request.htm";

		/// <summary>
		/// [[FirstName]]
		/// [[LastName]]
		/// [[Message]]
		/// [[ContactMethod]]
		/// [[PhoneOrEmail]]
		/// [[ContactTime]]
		/// [[Address1]]
		/// [[Address2]]
		/// [[City]]
		/// [[State]]
		/// [[Zip]]
		/// </summary>
		public const string MaintenanceRequest = Directory + "Contacts/maintenance-request.htm";

		/// <summary>
		/// [[FirstName]]
		/// [[LastName]]
		/// [[Message]]
		/// [[ContactMethod]]
		/// [[PhoneOrEmail]]
		/// [[ContactTime]]
		/// [[PropertyInfo]]
		/// </summary>
		public const string PropertyInfoRequest = Directory + "Contacts/property-info-request.htm";

		/// <summary>
		/// [[GUID]]
		/// </summary>
		public const string NewUserActivation = Directory + "Membership/user-activation.htm";

		/// <summary>
		/// [[Title]]
		/// [[Image]]
		/// [[Address]]
		/// [[DateListed]]
		/// [[NumberOfPhotos]]
		/// [[StatsFromShowcaseItemMetric]]
		/// [[PropertyLink]]
		/// [[WhatChanged]]
		/// </summary>
		public const string PropertyItemForStatistics = Directory + "Showcase/property-item.htm";

		/// <summary>
		/// [[PersonalMessage]]
		/// [[SiteName]]
		/// [[BeginDate]]
		/// [[EndDate]]
		/// [[Properties]]
		/// [[AgentName]]
		/// [[AgentImage]]
		/// [[OfficePhone]]
		/// [[Fax]]
		/// [[CellPhone]]
		/// [[AgentEmail]]
		/// </summary>
		public const string PropertyStatistics = Directory + "Showcase/property-statistics.htm";

		/// <summary>
		/// [[FirstAndLastName]]
		/// [[PropertyLinks]]
		/// [[SearchLinks]]
		/// </summary>
		public const string SavedSearch = Directory + "Showcase/saved-search.htm";

		/// <summary>
		/// [[Url]]
		/// [[Name]]
		/// </summary>
		public const string SavedSearchFilter = Directory + "Showcase/saved-search-filter.htm";

		/// <summary>
		/// [[Address]]
		/// [[DateListed]]
		/// [[Title]]
		/// [[WhatChanged]]
		/// </summary>
		public const string SavedSearchFilterProperty = Directory + "Showcase/saved-search-filter-property.htm";

		/// <summary>
		/// [[Address]]
		/// [[DateListed]]
		/// [[Title]]
		/// [[WhatChanged]]
		/// [[NumberOfPhotos]]
		/// </summary>
		public const string SavedSearchProperty = Directory + "Showcase/saved-search-property.htm";

		/// <summary>
		/// [[PersonalMessage]]
		/// [[Properties]]
		/// [[AgentName]]
		/// [[AgentImage]]
		/// [[OfficePhone]]
		/// [[Fax]]
		/// [[CellPhone]]
		/// [[AgentEmail]]
		/// </summary>
		public const string ShareListings = Directory + "Showcase/share-listings.htm";
	}

	public static class EmailTemplateService
	{
		/// <param name="applicationPath">application path, should start with "~/"</param>
		private static string LoadText(string applicationPath)
		{
			string text = string.Empty;
			string serverPath = HttpContext.Current.Server.MapPath(applicationPath);
			using (StreamReader reader = new StreamReader(serverPath))
			{
				text = reader.ReadToEnd();
				reader.Close();
			}
			return text;
		}

		public static string HtmlMessageBody(string htmlTemplatePath, object contentReplacements, bool useMasterTemplate = true)
		{
			return HtmlMessageBody(htmlTemplatePath, new RouteValueDictionary(contentReplacements), useMasterTemplate);
		}

		private static string HtmlMessageBody(string htmlTemplatePath, IDictionary<string, object> contentReplacements, bool useMasterTemplate = true)
		{
			if (htmlTemplatePath == null) return null;
			string body = useMasterTemplate ? LoadText(EmailTemplates.MasterEmailTemplate) : LoadText(htmlTemplatePath);
			if (useMasterTemplate)
				body = body.Replace("[[Body]]", LoadText(htmlTemplatePath));
			foreach (var replacement in contentReplacements)
				body = body.Replace(replacement.Key.ToReplaceable(), (replacement.Value ?? string.Empty).ToString());
			return body.Replace("[[ROOT]]", Helpers.RootPath);
		}

		private static string ToReplaceable(this string s)
		{
			return string.IsNullOrWhiteSpace(s) || (s.StartsWith("[[") && s.EndsWith("]]")) ? s : string.Format("[[{0}]]", s);
		}
	}
}