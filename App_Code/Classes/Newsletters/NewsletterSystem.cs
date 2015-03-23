using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web;
using BaseCode;

namespace Classes.Newsletters
{
	public class NewsletterSystem
	{
		#region Members and Properties

		private string m_MailFrom;
		private string m_MailServer;
		public int MailingListID { get; set; }

		public int NewsletterID { get; set; }

		public int? NewsletterDesignID { get; set; }

		public string MailServer
		{
			get { return m_MailServer ?? (m_MailServer = Settings.MailServer); }
			set { m_MailServer = value; }
		}

		public string MailFrom
		{
			get { return m_MailFrom ?? (m_MailFrom = Settings.SenderEmail); }
			set { m_MailFrom = value; }
		}

		#endregion

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		
		public static UnsubscribeUserReturnCode UnsubscribeUser(int mailingListID, string email)
		{
			ClearCache();

			// Find the Subscriber record by Email Address
			List<Subscriber> subs = Subscriber.SubscriberGetByEmail(email);
			if (subs.Count == 0)
				return UnsubscribeUserReturnCode.Never_Subscribed;

			// Find the MailingList (validation purposes)
			MailingList ml = MailingList.GetByID(mailingListID);
			if (ml == null)
				return UnsubscribeUserReturnCode.Missing_MailingList;

			// Find the MailingListSubscriber record (this could be better)
			List<MailingListSubscriber> mls
				= MailingListSubscriber.MailingListSubscriberGetBySubscriberID(subs[0].SubscriberID);

			if (mls.Count == 0)
				return UnsubscribeUserReturnCode.Never_Subscribed;

			bool alreadyUnsubscribed = false;
			foreach (MailingListSubscriber t in mls)
			{
				if (t.MailingListID == mailingListID)
				{
					alreadyUnsubscribed = true;
					if (t.Active)
					{
						// Ok, the MailingListSubscriber is active, deactivate
						t.Active = false;
						t.Save();
						return UnsubscribeUserReturnCode.Success;
					}
				}
			}

			if (alreadyUnsubscribed)
				return UnsubscribeUserReturnCode.Already_Unsubscribed;

			return UnsubscribeUserReturnCode.Already_Unsubscribed;
		}

		public static void SetSubscriberDefaultNewsletterFormatID(string email, int defaultNewsletterFormatID)
		{
			ClearCache();

			// Find the Subscriber record by Email Address
			List<Subscriber> subs = Subscriber.SubscriberGetByEmail(email);
			if (subs.Count == 0)
			{
				// Not yet a Subscriber, create new Subscriber
				Subscriber sub = new Subscriber();
				sub.Email = email;
				sub.DefaultNewsletterFormatID = defaultNewsletterFormatID;
				sub.Save();
			}
			else if (subs.Count == 1)
			{
				// Already a subscriber, modify record
				subs[0].DefaultNewsletterFormatID = defaultNewsletterFormatID;
				subs[0].Save();
			}
			else
				throw new Exception("More than one Subscriber record exists for the email " + email);
		}

		public static SubscribeUserReturnCode SubscribeUser(int mailingListID, string email, int newsletterFormatID, int? userID)
		{
			ClearCache();

			// Find the Subscriber record by Email Address
			List<Subscriber> subs = Subscriber.SubscriberGetByEmail(email);
			if (subs.Count == 0)
			{
				// Not yet a Subscriber, create new Subscriber
				Subscriber sub = new Subscriber();
				sub.Email = email;
				sub.DefaultNewsletterFormatID = newsletterFormatID; //this should be set to the user preference via SetSubscriberDefaultNewsletterFormatID
				sub.UserID = userID;
				sub.Save();
				subs.Add(sub);
			}

			// Find the MailingList (validation purposes)
			MailingList ml = MailingList.GetByID(mailingListID);
			if (ml == null)
				return SubscribeUserReturnCode.Missing_MailingList;

			// Find the MailingListSubscriber record (this could be better)
			List<MailingListSubscriber> mls
				= MailingListSubscriber.MailingListSubscriberGetBySubscriberID(subs[0].SubscriberID);

			foreach (MailingListSubscriber t in mls)
			{
				if (t.MailingListID == mailingListID)
				{
					//if subscriber isn't flagged as deleted...
					if (t.Active)
					{
						// Ok, the MailingListSubscriber is active, return
						t.NewsletterFormatID = newsletterFormatID;
						t.Save();
						return SubscribeUserReturnCode.Already_Subscribed;
					}
					// MailingListSubscriber already exists but was inactive or was deleted from the subscriber list, reactivate and return success
					subs[0].Deleted = false;
					subs[0].Save();
					t.Active = true;
					t.NewsletterFormatID = newsletterFormatID;
					t.Save();
					return SubscribeUserReturnCode.Success;
				}
			}
			if (Settings.EnableMailingListLimitations && MailingListSubscriber.GetCountByMailingListIDActive(mailingListID, true) >= Settings.MaxNumberSubscribers)
				return SubscribeUserReturnCode.MailingList_Full;
			//if MailingListSubscriber does not exist, create a new MailingListSubscriber
			MailingListSubscriber mlsNew = new MailingListSubscriber();
			mlsNew.MailingListID = mailingListID;
			mlsNew.SubscriberID = subs[0].SubscriberID;
			mlsNew.NewsletterFormatID = newsletterFormatID;
			mlsNew.Active = true;
			mlsNew.EntityID = Guid.NewGuid();
			mlsNew.Save();
			return SubscribeUserReturnCode.Success;
		}

		public static void SendNewsletterTest(string subject, string issue, string body, string bodyText, string email, int newsletterFormatID, string mailServer)
		{
			// if your compile fails here with 'SubscriptionType' could not be found, install the bulkmailer
			// - charles
			SubscriptionType type;
			switch (newsletterFormatID)
			{
				case 1:
					type = SubscriptionType.Html;
					break;
				case 2:
					type = SubscriptionType.PlainText;
					body = body.Replace("&amp;", "&");
					bodyText = bodyText.Replace("&amp;", "&");
					break;
				case 3:
					type = SubscriptionType.MultiPart;
					break;
				default:
					throw new Exception("Newsletter Format " + newsletterFormatID + " not defined.");
			}
			Queue<SubscriberInfo> subscriber = new Queue<SubscriberInfo>();
			subscriber.Enqueue(new SubscriberInfo(email, type));

			AttachmentCollection attachments = null;
			EmailSender es = new EmailSender(subject, subscriber, Settings.SenderEmail, mailServer, bodyText, body, attachments);
			EmailSender.SendEmailJobs.Add(es);
			es.Send();
		}

		public static void SendNewsletterTest(int newsletterID, int? newsletterDesignID, int newsletterFormatID, string email, string mailServer, string mailFrom)
		{
			SendNewsletterTest(newsletterID, newsletterDesignID, newsletterFormatID, email, mailServer, mailFrom, null);
		}

		public static void SendNewsletterTest(int newsletterID, int? newsletterDesignID, int newsletterFormatID, string email, string mailServer, string mailFrom, string approvalCode)
		{
			NewsletterDesign newsletterDesign =
				((newsletterDesignID == null) ? null : NewsletterDesign.GetByID((int)newsletterDesignID));

			if (newsletterID <= 0)
				throw new Exception("Newsletter Id must be greater than 0");
			Newsletter newsletter = Newsletter.GetByID(newsletterID);

			string subject = newsletter.Title;
			SubscriptionType type = SubscriptionType.Html; //default
			if (!string.IsNullOrEmpty(approvalCode))
			{
				switch (newsletterFormatID)
				{
					case 1:
						subject = "Html Newsletter Approval: " + subject;
						type = SubscriptionType.Html;
						break;
					case 2:
						subject = "Text Newsletter Approval: " + subject;
						type = SubscriptionType.PlainText;
						break;
					case 3:
						subject = "Multipart Newsletter Approval: " + subject;
						type = SubscriptionType.MultiPart;
						break;
					default:
						throw new Exception("Newsletter Format " + newsletterFormatID + " not defined.");
				}
			}

			Queue<SubscriberInfo> subscriber = new Queue<SubscriberInfo>();
			string[] recipients = email.Split(',');
			foreach (string i in recipients)
				subscriber.Enqueue(new SubscriberInfo(i, type));

			Mailout mailout = MailoutFromNewsletter(newsletter, newsletterDesign.NewsletterDesignID);
			string body = GetNewsletterHtml(mailout, true).Replace("[[EntityID]]", "");
			string bodyText = GetNewsletterText(mailout).Replace("[[EntityID]]", "");
			AttachmentCollection attachments = null;
			EmailSender es = new EmailSender(subject, subscriber, mailFrom, mailServer, bodyText, body, attachments);
			es.EnableLogging = false;
			es.Send();
		}

		public static bool ForwardNewsletter(int mailoutID, Guid entityID, string toEmail, string fromEmail, string toName, string fromName)
		{
			Mailout mailout = Mailout.GetByID(mailoutID);
			if (mailout == null)
				throw new Exception("The requested mailout does not exist");

			NewsletterSendingTypeFormat sendingFormat = Settings.NewsletterSendingType;

			SubscriptionType type;
			switch (sendingFormat)
			{
				case NewsletterSendingTypeFormat.HtmlAndText: // when forwarding, we don't ask for a preferred format, so default to HTML
				case NewsletterSendingTypeFormat.HtmlOnly:
					type = SubscriptionType.Html;
					break;
				case NewsletterSendingTypeFormat.TextOnly:
					type = SubscriptionType.PlainText;
					break;
				case NewsletterSendingTypeFormat.Multipart:
					type = SubscriptionType.MultiPart;
					break;
				default:
					throw new Exception("Newsletter Format " + sendingFormat + " not defined.");
			}

			SubscriberInfo recipient = new SubscriberInfo(toEmail, type);

			String forwardPreambleText = "This newsletter was forwarded to you by " + fromName + " (" + fromEmail + ")\n";
			String forwardPreambleHtml = "<p>" + forwardPreambleText + "</p>";

			string body = forwardPreambleHtml + GetNewsletterHtml(mailout, true).Replace("[[EntityID]]", "");
			string bodyText = forwardPreambleText + GetNewsletterText(mailout);
			string subject = "FW: " + mailout.Title;
			if (SendSingleEmail(recipient, Settings.SenderEmail, subject, body, bodyText))
			{
				Subscriber subscriberEntity = Subscriber.GetSubscriberByEntityID(entityID);
				if (subscriberEntity != null)
				    NewsletterAction.CreateForwardAction(subscriberEntity, mailoutID, toEmail);

				return true;
			}

			return false;
		}

		private static bool SendSingleEmail(SubscriberInfo recipient, string fromEmail, string subject, string htmlBody, string textBody)
		{
			SmtpClient smtp = new SmtpClient(Settings.MailServer);
			MailMessage mail = new MailMessage(Globals.Settings.FromEmail, recipient.Email);
			mail.Subject = subject;
			mail.ReplyTo = new MailAddress(fromEmail);

			switch (recipient.SubscriptionType)
			{
				case SubscriptionType.Html:
					mail.IsBodyHtml = true;
					mail.Body = htmlBody;
					break;
				case SubscriptionType.PlainText:
					mail.Body = textBody;
					break;
				case SubscriptionType.MultiPart:
					AlternateView textView = AlternateView.CreateAlternateViewFromString(textBody, null, "text/plain");
					textView.TransferEncoding = TransferEncoding.SevenBit;
					AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");
					htmlView.TransferEncoding = TransferEncoding.SevenBit;
					mail.AlternateViews.Add(textView);
					mail.AlternateViews.Add(htmlView);
					break;
				default:
					mail.Body = htmlBody;
					break;
			}

			try
			{
				smtp.Send(mail);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Send a Newsletter to a collection of mailing lists, or resend a previously sent Mailout to its collection of mailing lists
		/// </summary>
		/// <param name="newsletterId">The ID of the Newsletter to send</param>
		/// <param name="existingMailoutId">The ID of an existing Mailout to resend</param>
		/// <param name="designId">The ID of the design to wrap the newsletter content</param>
		/// <param name="mailingLists">Collection of mailing lists to send the mailout to</param>
		/// <param name="notSentSubscribers">Collection of subscribers who had invalid email addresses</param>
		/// <returns>True if at least one subscriber was sent the newsletter, false otherwise</returns>
		public static bool SendNewsletter(int? newsletterId, int? existingMailoutId, int designId, List<MailingList> mailingLists, out List<MailingListSubscriber> notSentSubscribers)
		{
			notSentSubscribers = new List<MailingListSubscriber>();

			Mailout mailout;
			if (existingMailoutId.HasValue)
			{
				mailout = Mailout.GetByID(existingMailoutId.Value);
			}
			else if (newsletterId.HasValue)
			{
				Newsletter newsletter = Newsletter.GetByID(newsletterId.Value);
				mailout = SaveNewMailoutForNewsletter(newsletter, mailingLists, designId);
			}
			else
				return false;

			String htmlBodyWithDesign = GetNewsletterHtml(mailout, true);
			String textBody = GetNewsletterText(mailout);

			List<int> mailingListIds = mailingLists.Select(ml => ml.MailingListID).ToList();

			Queue<SubscriberInfo> uniqueSubscribers = SubscriberInfo.GetMailingListSubscribersWithEmailsByListOfMailingListIDs(mailingListIds, existingMailoutId);

			if (uniqueSubscribers.Count > 0)
			{
				EmailSender es = new EmailSender(
					mailout.Title,
					uniqueSubscribers,
					Settings.SenderEmail,
					Settings.MailServer,
					textBody,
					htmlBodyWithDesign
					, null);
				es.IPAddress = HttpContext.Current.Request.UserHostAddress;
				es.MailoutID = mailout.MailoutID;
				es.MailingListIDs = mailingListIds;
				es.Send();
				EmailSender.SendEmailJobs.Add(es);

				return true;
			}
			return false;
		}

		/// <summary>
		/// This method wraps the body of the newsletter in the desired design and replaces placeholder tags with dynamic content
		/// </summary>
		/// <param name="mailout">The Mailout containing the newsletter data to be sent</param>
		/// <param name="email">Whether this newsletter should be formatted for sending out via email (vs. web-only display)</param>
		/// <returns></returns>
		public static string GetNewsletterHtml(Mailout mailout, bool email)
		{
			string body = string.Empty;
			const string trackingImage = @"<img src=""[[ROOT]]newsletter-opened.aspx?entityId=[[EntityID]]&amp;mailoutId=[[MailoutID]]"" height=""1"" width=""1"" border=""0"" />";

			NewsletterDesign newsletterDesign = NewsletterDesign.GetByID(Convert.ToInt32(mailout.DesignID));
			if (newsletterDesign != null)
			{
				if (newsletterDesign.Path != null)
				{
					body = EmailTemplateService.HtmlMessageBody("~/" + newsletterDesign.Path, new { FullCompanyName = Globals.Settings.CompanyName, FullPhysicalMailAddress = Settings.CustomPhysicalMailAddress });
					//The final remove is necessary to get rid of the "Trouble viewing this email" stuff from the Newsletter frontend pages
					if (!email)
						body = body.Remove(body.IndexOf("[[Email Only]]"), body.IndexOf("[[End Email Only]]") - body.IndexOf("[[Email Only]]") + "[[End Email Only]]".Length);
					else
						body = body.Replace("[[Email Only]]", "")
								.Replace("[[End Email Only]]", "") + trackingImage;
				}
				else if (newsletterDesign.Template != null)
					body = newsletterDesign.Template;
				else
					throw new Exception("No design path or html for selected design");
				body = InsertDynamicContent(mailout, body, mailout.Body);

				// For front-end display, remove per-subscriber replacement tags
				if (!email)
					body = body.Replace("[[EntityID]]", "");
			}
			return body.Replace("[[ROOT]]", Helpers.RootPath);
		}

		public static string GetNewsletterText(Mailout mailout)
		{
			string body = EmailTemplateService.HtmlMessageBody("~/EmailTemplates/Newsletter/EmailTextWrapper.txt", new { FullCompanyName = Globals.Settings.CompanyName, FullPhysicalMailAddress = Settings.CustomPhysicalMailAddress });
			body = InsertDynamicContent(mailout, body, mailout.BodyText).Replace("&amp;", "&").Replace("[[ROOT]]", Helpers.RootPath); //Plain text does not convert the ampersand
			return body;
		}

		private static bool ShouldReplaceWithTrackingLink(string url)
		{
			url = url.Trim().ToUpperInvariant();
			return !(url.EndsWith(".JPG") || url.EndsWith(".JPEG") || url.EndsWith(".GIF") || url.EndsWith(".PNG"));
		}

		public static String ReplaceLinksWithTrackingLinks(string emailBody)
		{
			String urlPattern = @"href=""http(s)?://([\w+?\.\w{2,6}])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?";
			Regex urlRegex = new Regex(urlPattern, RegexOptions.IgnoreCase);

			MatchCollection matchedUrls = urlRegex.Matches(emailBody);
			List<String> urlsToReplace = new List<String>();
			foreach (Match matchedUrl in matchedUrls)
			{
				if (ShouldReplaceWithTrackingLink(matchedUrl.Value))
					urlsToReplace.Add(matchedUrl.Value);
			}

			urlsToReplace = urlsToReplace.Distinct().ToList();
			urlsToReplace.Sort((first, second) => (second.Length - first.Length)); // sort links by length descending

			for (int i = 0; i < urlsToReplace.Count; i++)
			{
				string encodedUrl = "href=\"" + HttpContext.Current.Server.UrlEncode(urlsToReplace[i].Replace("href=\"", "").Replace("&amp;", "&"));
				emailBody = emailBody.Replace(urlsToReplace[i], encodedUrl);
				urlsToReplace[i] = encodedUrl;
			}

			String trackingLink = Helpers.ReplaceRootWithAbsolutePath("[[ROOT]]newsletter-link.aspx?entityId=[[EntityID]]&amp;mailoutId=[[MailoutID]]&amp;destUrl=");

			//Do RootPath first
			if (urlsToReplace.Contains("href=\"" + HttpContext.Current.Server.UrlEncode(Helpers.RootPath.TrimEnd('/'))))
			{
				urlsToReplace.RemoveAll(s => s.Contains("href=\"" + HttpContext.Current.Server.UrlEncode(Helpers.RootPath.TrimEnd('/'))));
				urlsToReplace.Insert(0, "href=\"" + HttpContext.Current.Server.UrlEncode(Helpers.RootPath.TrimEnd('/')));
			}

			if (urlsToReplace.Contains("href=\"" + HttpContext.Current.Server.UrlEncode(Helpers.RootPath)))
			{
				urlsToReplace.RemoveAll(s => s.Contains("href=\"" + HttpContext.Current.Server.UrlEncode(Helpers.RootPath)));
				urlsToReplace.Insert(0, "href=\"" + HttpContext.Current.Server.UrlEncode(Helpers.RootPath));
			}

			foreach (String urlToReplace in urlsToReplace)
			{
				string regexp = "(" + Regex.Escape(urlToReplace) + ")(?!.*destUrl=.*)";
				emailBody = Regex.Replace(emailBody, regexp, "href=\"" + trackingLink + urlToReplace.Replace("href=\"", ""));
			}

			return emailBody;
		}

		private static String InsertDynamicContent(Mailout mailout, String messageBody, String body)
		{
			string newsletterLink = "[[ROOT]]newsletter-details.aspx?mailoutId=" + mailout.MailoutID;
			string linkToSite = Globals.Settings.LinkToSite;
			string unsubscribeLink = "[[ROOT]]newsletter-unsubscribe.aspx?entityId=[[EntityID]]&amp;mailoutId=[[MailoutID]]";
			string forwardLink = "[[ROOT]]newsletter-forward.aspx?entityId=[[EntityID]]&amp;mailoutId=[[MailoutID]]";

			messageBody = messageBody
				.Replace("[[NewsletterTitle]]", mailout.Title)
				.Replace("[[NewsletterBody]]", body)
				.Replace("[[NewsletterIssue]]", mailout.Issue)
				.Replace("[[NewsletterDate]]", Helpers.ConvertUTCToClientTime(mailout.DisplayDate).ToShortDateString())
				.Replace("[[NewsletterSender]]", Settings.SenderEmail)
				.Replace("[[NewsletterLink]]", newsletterLink)
				.Replace("[[LinkToSite]]", linkToSite);

			messageBody = Helpers.ReplaceRootWithAbsolutePath(messageBody);
			messageBody = ReplaceLinksWithTrackingLinks(messageBody);
			messageBody = messageBody.Replace("[[UnsubscribeLink]]", unsubscribeLink); // track unsubscribes and forwards separately from regular click-throughs
			messageBody = messageBody.Replace("[[ForwardLink]]", forwardLink);
			messageBody = messageBody.Replace("[[MailoutID]]", mailout.MailoutID.ToString());
			return Helpers.ReplaceRootWithAbsolutePath(messageBody);
		}

		/// <summary>
		/// This method transforms a Newsletter into a Mailout, optionally replacing the Newsletter's design
		/// </summary>
		/// <param name="newsletter">The Newsletter entity to copy</param>
		/// <param name="designId">The design to override the DesignID property of the Newsletter (optional)</param>
		/// <returns>An unsaved Mailout with a copy of the Newsletter's data</returns>
		public static Mailout MailoutFromNewsletter(Newsletter newsletter, int? designId)
		{
			Mailout mailout = new Mailout();
			mailout.Timestamp = DateTime.UtcNow;
			mailout.NewsletterID = newsletter.NewsletterID;
			mailout.Body = newsletter.Body.Replace("& ", "&amp; ");
			mailout.BodyText = newsletter.BodyText;
			mailout.Description = HttpContext.Current != null ? HttpContext.Current.Server.HtmlEncode(newsletter.Description) : newsletter.Description.Replace("& ", "&amp; ");
			mailout.DesignID = designId.HasValue ? designId.Value : newsletter.DesignID;
			mailout.DisplayDate = newsletter.DisplayDate;
			mailout.Issue = HttpContext.Current != null ? HttpContext.Current.Server.HtmlEncode(newsletter.Issue) : newsletter.Issue.Replace("& ", "&amp; ");
			mailout.Keywords = HttpContext.Current != null ? HttpContext.Current.Server.HtmlEncode(newsletter.Keywords) : newsletter.Keywords.Replace("& ", "&amp; ");
			mailout.Title = newsletter.Title.Replace("& ", "&amp; ");
			return mailout;
		}

		private static Mailout SaveNewMailoutForNewsletter(Newsletter newsletter, List<MailingList> lists, int designId)
		{
			Mailout mailout = MailoutFromNewsletter(newsletter, designId);
			mailout.Save();

			foreach (MailingList mailingList in lists)
			{
				MailoutMailingList mml = new MailoutMailingList();
				mml.MailoutID = mailout.MailoutID;
				mml.MailingListID = mailingList.MailingListID;
				mml.Save();
			}

			return mailout;
		}

		public static string GetPlainEnglishSendingFormat()
		{
			string newsletterSendingFormat;
			switch (Settings.NewsletterSendingType)
			{
				case NewsletterSendingTypeFormat.HtmlAndText:
					newsletterSendingFormat = "User Preference (HTML/Plain Text)";
					break;
				case NewsletterSendingTypeFormat.HtmlOnly:
					newsletterSendingFormat = "HTML";
					break;
				case NewsletterSendingTypeFormat.TextOnly:
					newsletterSendingFormat = "Plain Text";
					break;
				case NewsletterSendingTypeFormat.Multipart:
					newsletterSendingFormat = "Multipart (HTML/Plain Text)";
					break;
				default:
					newsletterSendingFormat = "Unknown newsletterSendingType encountered: " + Settings.NewsletterSendingType;
					break;
			}

			return newsletterSendingFormat;
		}

		/// <summary>
		/// Clear the cache, if caching is enabled
		/// </summary>
		public static void ClearCache()
		{
			if (Cache.IsEnabled)
				Cache.Purge("Newsletters_");
		}
	}
}