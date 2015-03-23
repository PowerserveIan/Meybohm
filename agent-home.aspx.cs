using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Services;
using BaseCode;
using Classes.Contacts;
using Classes.Media352_MembershipProvider;
using Classes.Showcase;
using Contact = Classes.Contacts.Contact;

public partial class MembersHome : BasePage
{
	protected override void SetCssAndJs()
	{
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Members Area";
		ComponentAdminPage = "media352-membership-provider/admin-user.aspx";
	}

	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		if (!User.Identity.IsAuthenticated)
			Response.Redirect("~/");
		if (Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite() == null)
		{
			UserInfo userInfoEntity = UserInfo.UserInfoGetByUserID(Helpers.GetCurrentUserID(), includeList: new string[] { "CMMicrosite" }).FirstOrDefault();
			if (userInfoEntity != null && userInfoEntity.CMMicrosite != null)
				Response.Redirect(userInfoEntity.CMMicrosite.Name.ToLower().Replace(" ", "-") + "/agent-home");
			Response.Redirect("~/augusta/agent-home");
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			int userID = Helpers.GetCurrentUserID();
			UserOffice userOfficeEntity = UserOffice.UserOfficeGetByUserID(userID, includeList: new string[] { "Office", "Office.Address", "Office.Address.State" }).FirstOrDefault();
			if (userOfficeEntity != null)
			{
				uxOffice.Text = userOfficeEntity.Office.Name;
				uxOfficeAddress.Text = userOfficeEntity.Office.Address.Address1 + "<br />" + userOfficeEntity.Office.Address.City + ", " + userOfficeEntity.Office.Address.State.Abb + " " + userOfficeEntity.Office.Address.Zip;
				uxOfficeFax.Text = userOfficeEntity.Office.Fax;
				uxOfficePhone.Text = userOfficeEntity.Office.Phone;
			}

			uxNewsletters.DataSource = Classes.Newsletters.Newsletter.NewsletterPage(0, 3, "", "DisplayDate", false, new Classes.Newsletters.Newsletter.Filters { FilterNewsletterActive = true.ToString(), FilterNewsletterCMMicrositeID = "", FilterNewsletterDeleted = false.ToString() });
			uxNewsletters.DataBind();

			UserInfo userInfoEntity = UserInfo.UserInfoGetByUserID(userID).FirstOrDefault();
			uxPropertyContactsPH.Visible = userInfoEntity.StaffTypeID == (int)StaffTypes.AgentAssistant || userInfoEntity.StaffTypeID == (int)StaffTypes.REALTOR || userInfoEntity.StaffTypeID == (int)StaffTypes.RentalRealtor;

			uxContactStatusID.DataSource = ContactStatus.GetAll().Where(c => c.ContactStatusID != (int)ContactStatuses.Unread);
			uxContactStatusID.DataTextField = "Name";
			uxContactStatusID.DataValueField = "ContactStatusID";
			uxContactStatusID.DataBind();

			List<ShowcaseItem> agentProperties = ShowcaseItem.GetPropertiesForAgent(99999, "NumberOfVisits", false, userID);
			uxTotalListings.Text = agentProperties.Count.ToString();
			Dictionary<string, int> propertyTypes = new Dictionary<string, int>();
			foreach (ShowcaseItem property in agentProperties)
			{
				if (!String.IsNullOrWhiteSpace(property.PropertyType))
				{
					if (!propertyTypes.ContainsKey(property.PropertyType))
						propertyTypes.Add(property.PropertyType, 0);
					propertyTypes[property.PropertyType]++;
				}
			}

			uxListingTypes.DataSource = propertyTypes;
			uxListingTypes.DataBind();

			Classes.Videos.Video featuredVideo = Classes.Videos.Video.VideoGetByFeatured(true, "DisplayOrder").FirstOrDefault();
			if (featuredVideo != null)
			{
				uxVideoTitle.Text = featuredVideo.Title;
				uxVideoUrl.Text = featuredVideo.Url.ToLower().Contains("iframe") ? featuredVideo.Url : string.Format(@"<iframe width=""250"" height=""190"" src=""{0}"" frameborder=""0"" allowfullscreen></iframe>", featuredVideo.Url);
			}

			uxHelpDesk.NavigateUrl = Globals.Settings.HelpDeskUrl;
		}
	}

	[WebMethod]
	public static ListingItemWithCount<Contact> GetPropertyRequests(int pageSize, string sortField, bool sortDirection)
	{
		int totalCount;
		int userID = Helpers.GetCurrentUserID();
		List<Contact> listItems = Contact.ContactPageWithTotalCount(0, pageSize, "", sortField, sortDirection, out totalCount, new Contact.Filters { FilterContactAgentID = userID.ToString(), FilterContactContactTypeID = ((int)ContactTypes.PropertyInformation).ToString() }, new string[] { "ContactMethod", "ContactTime", "ShowcaseItem.Address.State" });
		return new ListingItemWithCount<Contact> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static ListingItemWithCount<Contact> GetAgentRequests(int pageSize, string sortField, bool sortDirection)
	{
		int totalCount;
		int userID = Helpers.GetCurrentUserID();
		List<Contact> listItems = Contact.ContactPageWithTotalCount(0, pageSize, "", sortField, sortDirection, out totalCount, new Contact.Filters { FilterContactAgentID = userID.ToString(), FilterContactContactTypeID = ((int)ContactTypes.Agent).ToString() }, new string[] { "ContactMethod", "ContactTime" });
		return new ListingItemWithCount<Contact> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void UpdateContactRequest(int contactID, int statusID)
	{
		Contact entity = Contact.GetByID(contactID);
		if (entity != null)
		{
			entity.ContactStatusID = statusID;
			entity.Save();
		}
	}

	[WebMethod]
	public static ListingItemWithCount<ShowcaseItem> GetProperties(int pageSize, string sortField, bool sortDirection)
	{
		int totalCount;
		int userID = Helpers.GetCurrentUserID();
		List<ShowcaseItem> listItems = ShowcaseItem.GetPropertiesForAgentWithTotalCount(pageSize, sortField, sortDirection, userID, out totalCount);
		return new ListingItemWithCount<ShowcaseItem> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static ListingItemWithCount<ExchangeEmailMessage> GetEmails(int pageSize)
	{
		int userID = Helpers.GetCurrentUserID();
		User userEntity = Classes.Media352_MembershipProvider.User.GetByID(userID);
		int totalCount;
		List<ExchangeEmailMessage> items = ExchangeEmailMessage.GetEmailsFromExchangeServer(userEntity.Email, pageSize, out totalCount);
		return new ListingItemWithCount<ExchangeEmailMessage> { Items = items, TotalCount = totalCount };
	}

	[WebMethod]
	public static void ShareListings(string emails, string subject, string message, List<ShowcaseItem> listingItems)
	{
		int userID = Helpers.GetCurrentUserID();
		Classes.Media352_MembershipProvider.User userEntity = Classes.Media352_MembershipProvider.User.GetByID(userID);
		UserInfo userInfo = UserInfo.UserInfoGetByUserID(userID, includeList: new string[] { "CMMicrosite" }).FirstOrDefault();
		MailMessage email = new MailMessage();
		email.IsBodyHtml = true;
		email.From = new MailAddress(userEntity.Email);
		foreach (string emailAddress in emails.Split(';'))
		{
			if (Regex.IsMatch(emailAddress, Helpers.EmailValidationExpression))
				email.To.Add(emailAddress);
		}
		email.Subject = Globals.Settings.SiteTitle + " - " + subject;
		string basePropertyHtml = EmailTemplateService.HtmlMessageBody(EmailTemplates.PropertyItemForStatistics, null, false);
		string propertyHtml = string.Empty;
		foreach (ShowcaseItem property in listingItems)
		{
			string msLink = property.ShowcaseID == (int)MeybohmShowcases.AikenExistingHomes ||  property.ShowcaseID == (int)MeybohmShowcases.AikenRentalHomes
									? "aiken/" : "augusta/";
			string searchType = property.ShowcaseID == (int)MeybohmShowcases.AikenExistingHomes || property.ShowcaseID == (int)MeybohmShowcases.AugustaExistingHomes ?(property.NewHome?"new-":"")+ "search" :"rentals";
			
			string thisPropertyHtml = basePropertyHtml.Replace("[[Title]]", property.Title)
												.Replace("[[Image]]", !String.IsNullOrWhiteSpace(property.Image) && !property.Image.ToLower().StartsWith("http") ? Helpers.RootPath + Globals.Settings.UploadFolder + "images/" + property.Image : property.Image)
												.Replace("[[Address]]", property.Address.Address1 + "<br />" + property.Address.City + ", " + property.Address.State.Abb + " " + property.Address.Zip)
												.Replace("[[DateListed]]", property.DateListed.HasValue ? property.DateListed.Value.ToShortDateString() : string.Empty)
												.Replace("[[NumberOfPhotos]]", Media.GetNumberOfPhotos(property.ShowcaseItemID).ToString())
												.Replace("[[StatsFromShowcaseItemMetric]]", string.Empty)
												.Replace("[[PropertyLink]]", Helpers.RootPath + msLink + searchType + "?id=" + property.ShowcaseItemID);

			if (thisPropertyHtml.IndexOf("[[SavedSearchBegin]]", StringComparison.Ordinal) > 0 && thisPropertyHtml.IndexOf("[[SavedSearchEnd]]", StringComparison.Ordinal) > 0)
				thisPropertyHtml = thisPropertyHtml.Remove(thisPropertyHtml.IndexOf("[[SavedSearchBegin]]", StringComparison.Ordinal), thisPropertyHtml.IndexOf("[[SavedSearchEnd]]", StringComparison.Ordinal) - thisPropertyHtml.IndexOf("[[SavedSearchBegin]]", StringComparison.Ordinal) + "[[SavedSearchEnd]]".Length);
			propertyHtml += thisPropertyHtml;
		}
		email.Body = EmailTemplateService.HtmlMessageBody(EmailTemplates.ShareListings, new
		{
			PersonalMessage = Helpers.MaintainLineBreaks(message),
			AgentName = userInfo.FirstAndLast,
			AgentImage = !String.IsNullOrWhiteSpace(userInfo.Photo) && !userInfo.Photo.ToLower().StartsWith("http") ? Helpers.RootPath + Globals.Settings.UploadFolder + "agents/" + userInfo.Photo : userInfo.Photo,
			OfficePhone = userInfo.OfficePhone,
			Fax = userInfo.Fax,
			CellPhone = userInfo.CellPhone,
			AgentEmail = userEntity.Email,
			Properties = propertyHtml
		}).Replace("[[PersonalMessageBegin]]", "").Replace("[[PersonalMessageEnd]]", "");

		SmtpClient smtp = new SmtpClient();
		smtp.Send(email);
	}

	[WebMethod]
	public static void SendStats(string emails, string subject, string message, int showcaseItemID)
	{
		ShowcaseItemMetric.SendPropertyStatisticEmail(ShowcaseItem.GetByID(showcaseItemID, new string[] { "Address", "Address.State" }), emails, subject, message);
	}
}