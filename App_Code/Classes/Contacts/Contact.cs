using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using BaseCode;
using Classes.Media352_MembershipProvider;
using Classes.Showcase;

namespace Classes.Contacts
{
	public partial class Contact
	{
		public string ContactStatusName
		{
			get { return this.ContactStatus != null ? this.ContactStatus.Name : string.Empty; }
		}

		public static List<UserInfo> GetAllAgentsWithContactRequests()
		{
			List<UserInfo> objects;
			string key = cacheKeyPrefix + "GetAllAgentsWithContactRequests";

			List<UserInfo> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<UserInfo>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.Contact.Where(c => c.AgentID.HasValue).Select(c => c.Agent.UserInfo.FirstOrDefault()).ToList();
				}

				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<ShowcaseItem> GetAllPropertiesWithContactRequests()
		{
			List<ShowcaseItem> objects;
			string key = cacheKeyPrefix + "GetAllPropertiesWithContactRequests";

			List<ShowcaseItem> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<ShowcaseItem>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.ShowcaseItem.Where(s => s.Contact.Any()).OrderBy(s => s.Title).ToList();
				}

				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Contact> PropertyInformationPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Contact> objects = PropertyInformationPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Contact> PropertyInformationPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ContactID");

			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText, includeList);

			List<Contact> objects;
			string baseKey = cacheKeyPrefix + "PropertyInformationPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Contact> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Contact>();
				tmpList = Cache[key] as List<Contact>;
				tmpInt = Cache[countKey] as int?;
			}

			if (tmpList != null && tmpInt.HasValue)
			{
				objects = tmpList;
				m_ItemCount = tmpInt.Value;
			}
			else
			{
				int pageNumber = maximumRows > 0 ? 1 + startRowIndex / maximumRows : 1;

				using (Entities entity = new Entities())
				{
					IQueryable<Contact> itemQuery = SetupQuery(entity.Contact, "Contact", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);
					if (!String.IsNullOrWhiteSpace(filterList.FilterShowcaseID))
					{
						int showcaseID = Convert.ToInt32(filterList.FilterShowcaseID);
						itemQuery = itemQuery.Where(c => c.ShowcaseItem.ShowcaseID == showcaseID);
					}
					objects = maximumRows <= 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : (maximumRows <= 0 || (pageNumber == 1 && objects.Count < maximumRows) ? objects.Count : itemQuery.Count());
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		public static void SendSubmissionEmail(Contact contactEntity, ContactTypes contactFormType)
		{
			string toAddress = contactFormType == ContactTypes.ContactUs ? Settings.ContactSubmissionEmailAddress : contactFormType == ContactTypes.HomeValuationRequest ? Settings.HomeValuationEmailAddress : Settings.MaintenanceRequestEmailAddress;
			string templatePath = contactFormType == ContactTypes.ContactUs ? EmailTemplates.ContactSubmission : contactFormType == ContactTypes.HomeValuationRequest ? EmailTemplates.HomeValuation : EmailTemplates.MaintenanceRequest;
			string propertyInfo = string.Empty;
			string microsite = string.Empty;
            ShowcaseItem showcaseItem = null;

			if (contactEntity.ShowcaseItemID.HasValue)
			{
				showcaseItem = Classes.Showcase.ShowcaseItem.GetByID(contactEntity.ShowcaseItemID.Value, new string[] { "Agent", "Showcase.CMMicrosite", "Team", "Address" });
				if (showcaseItem.AgentID.HasValue)
					toAddress = showcaseItem.Agent.Email;
				else if (showcaseItem.TeamID.HasValue)
					toAddress = showcaseItem.Team.Email;
				else
					toAddress = Classes.Contacts.Settings.DefaultAgentContactEmail;
				templatePath = EmailTemplates.PropertyInfoRequest;
				propertyInfo = showcaseItem.Address.FormattedAddress + (showcaseItem.MlsID.HasValue ? "<br />MLS ID #" + showcaseItem.MlsID : "");
				microsite = showcaseItem.Showcase.CMMicrosite.Name.ToLower().Replace(" ", "-");
			}
			else if (contactEntity.AgentID.HasValue)
			{
				Classes.Media352_MembershipProvider.User agent = Classes.Media352_MembershipProvider.User.GetByID(contactEntity.AgentID.Value, new[] { "UserOffice.Office" });
				if (agent.UserOffice != null && agent.UserOffice.Any(o => o.Office != null && o.Office.Active))
					toAddress = agent.Email;
				else
					toAddress = Classes.Contacts.Settings.DefaultAgentContactEmail;
				templatePath = EmailTemplates.ContactAgentSubmission;
			}
			else if (contactEntity.TeamID.HasValue)
			{
				Classes.Media352_MembershipProvider.Team team = Classes.Media352_MembershipProvider.Team.GetByID(contactEntity.TeamID.Value);
				toAddress = team.Email;
				templatePath = EmailTemplates.ContactAgentSubmission;
			}

			if (string.IsNullOrWhiteSpace(toAddress))
				return;
			string contactMethodName = ContactMethod.GetByID(contactEntity.ContactMethodID).Name;
			MailMessage email = new MailMessage();
			email.IsBodyHtml = true;
			email.From = new MailAddress(Globals.Settings.FromEmail);
			//TODO: Remove this when done testing.  Put in place to prevent spamming of meybohm agents
			if (System.Web.HttpContext.Current.IsDebuggingEnabled && toAddress.ToLower().EndsWith("@meybohm.com"))
				toAddress = "ian.nielson@powerserve.net";
			email.To.Add(toAddress);
			if (contactFormType == ContactTypes.PropertyInformation && toAddress != Settings.PropertyInfoCCEmailAddress)
				email.CC.Add(Settings.PropertyInfoCCEmailAddress);
			else if (contactFormType == ContactTypes.Agent && toAddress != Settings.AgentContactCCEmailAddress)
				email.CC.Add(Settings.AgentContactCCEmailAddress);

			Classes.StateAndCountry.Address addressEntity = null;
			if (contactEntity.AddressID.HasValue)
				addressEntity = Classes.StateAndCountry.Address.GetByID(contactEntity.AddressID.Value, new string[] { "State" });
			email.Body = EmailTemplateService.HtmlMessageBody(templatePath, new
			{
				FirstName = contactEntity.FirstName,
				LastName = contactEntity.LastName,
				Address1 = (addressEntity != null ? addressEntity.Address1 : string.Empty),
				Address2 = (addressEntity != null ? addressEntity.Address2 : string.Empty),
				City = (addressEntity != null ? addressEntity.City : string.Empty),
				State = (addressEntity != null ? addressEntity.State.Name : string.Empty),
				Zip = (addressEntity != null ? addressEntity.Zip : string.Empty),
				Message = contactEntity.Message,
				ContactMethod = contactMethodName,
				Email = contactEntity.Email,
				PhoneOrEmail = (contactEntity.ContactMethodID == (int)ContactMethods.Email ?"": "Phone: " + contactEntity.Phone),
				ContactTime = ContactTime.GetByID(contactEntity.ContactTimeID).Name,
				PropertyInfo = propertyInfo,
				Microsite = microsite
			});
			string subjectText = contactEntity.ShowcaseItemID.HasValue ? "Property Information Request" : contactFormType == ContactTypes.ContactUs ? "Contact Form Submission" : contactFormType == ContactTypes.HomeValuationRequest ? "Home Valuation Request" : contactFormType == ContactTypes.MaintenanceRequest ? "Maintenance Request" : "Contact Agent Submission";
			if(showcaseItem != null)
            {
                if (showcaseItem.ShowcaseID == (int)MeybohmShowcases.AugustaRentalHomes || showcaseItem.ShowcaseID == (int)MeybohmShowcases.AikenRentalHomes)
                {
                    subjectText += " - Lease";
                }
            }
            email.Subject = Globals.Settings.SiteTitle + " - " + subjectText;

            // Apply MLS ID and address information to the subject line if this is a property information
            // request.
            if (showcaseItem != null && (showcaseItem.MlsID != null || showcaseItem.Address != null))
            {
                string subjectDetails = "[";

                if (showcaseItem.MlsID != null)
                {
                    subjectDetails += showcaseItem.MlsID;
                }

                if (showcaseItem.MlsID != null && showcaseItem.Address != null)
                {
                    subjectDetails += " - ";
                }

                if (showcaseItem.Address != null)
                {
                    subjectDetails += showcaseItem.Address.Address1;
                }

                subjectDetails += "] Property Information Request";

                if (showcaseItem.ShowcaseID == (int)MeybohmShowcases.AugustaRentalHomes || showcaseItem.ShowcaseID == (int)MeybohmShowcases.AikenRentalHomes)
                {
                    subjectDetails += " - Lease";
                }

                email.Subject = subjectDetails;
            }

			SmtpClient smtp = new SmtpClient();
			smtp.Send(email);
		}

		public partial struct Filters
		{
			public string FilterShowcaseID { get; set; }

			public Dictionary<string, object> GetCustomFilterList()
			{
				filterList = GetFilterList();
				if (FilterShowcaseID != null)
				{
					if (FilterShowcaseID == string.Empty)
						filterList.Add("@FilterShowcaseID", string.Empty);
					else
						filterList.Add("@FilterShowcaseID", Convert.ToInt32(FilterShowcaseID));
				}
				return filterList;
			}
		}
	}
}