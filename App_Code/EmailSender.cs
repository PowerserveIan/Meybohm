using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using BaseCode;

[Serializable]
public class InvalidEmailConfigurationException : ApplicationException
{
	public InvalidEmailConfigurationException(string message)
		: base(message)
	{
	}
}

[Serializable]
public enum SubscriptionType
{
	None = 0,
	Html = 1,
	PlainText = 2,
	MultiPart = 3
}

[Serializable]
public struct SubscriberInfo
{
	public string Email;
	public Guid? EntityID;
	public Hashtable Replacements;
	public int? SubscriberID;
	public SubscriptionType SubscriptionType;

	public SubscriberInfo(string email, SubscriptionType subscriptionType) :
		this(email, subscriptionType, null, null)
	{
	}

	public SubscriberInfo(string email, SubscriptionType subscriptionType, Guid entityID) :
		this(email, subscriptionType, entityID, null)
	{
	}

	public SubscriberInfo(string email, SubscriptionType subscriptionType, Hashtable replacements) :
		this(email, subscriptionType, null, replacements)
	{
	}

	public SubscriberInfo(string email, SubscriptionType subscriptionType, Guid? entityID, Hashtable replacements)
	{
		Email = email;
		SubscriptionType = subscriptionType;
		Replacements = replacements;
		EntityID = entityID;
		SubscriberID = null;
	}

	public SubscriberInfo(string email, SubscriptionType subscriptionType, Guid? entityID, Hashtable replacements, int subscriberID)
	{
		Email = email;
		SubscriptionType = subscriptionType;
		Replacements = replacements;
		EntityID = entityID;
		SubscriberID = subscriberID;
	}

	public static Queue<SubscriberInfo> GetMailingListSubscribersWithEmailsByListOfMailingListIDs(List<int> mailingListIDs, int? existingMailoutId)
	{
		Queue<SubscriberInfo> objects = new Queue<SubscriberInfo>();

		using (Entities entity = new Entities())
		{
			var mailingListSubscribersWithEmails = entity.Newsletter_GetMailingListSubscribersWithEmailsByListOfMailingListIDs(
				string.Join(",", mailingListIDs.Select(id => id.ToString())),
				existingMailoutId);

			foreach (var mailingListSubscribersWithEmail in mailingListSubscribersWithEmails)
			{
				SubscriberInfo obj = new SubscriberInfo();
				obj.Email = mailingListSubscribersWithEmail.Email;
				obj.EntityID = mailingListSubscribersWithEmail.EntityID;
				obj.Replacements = new Hashtable {{"[[EntityID]]", obj.EntityID.Value.ToString("D").ToLower()}};
				obj.SubscriberID = mailingListSubscribersWithEmail.SubscriberID;
				obj.SubscriptionType = (SubscriptionType)mailingListSubscribersWithEmail.DefaultNewsletterFormatID;
				objects.Enqueue(obj);
			}
		}

		return objects;
	}
}

[Serializable]
public class EmailSender
{
	private static ArrayList m_SendEmailJobs;
	public ReaderWriterLock Lock = new ReaderWriterLock();
	private bool allThreadsFinished;
	private string m_IPAddress;
	private AttachmentCollection m_attachments;

	private string m_emailServer = string.Empty;
	private bool m_enableLogging = true;
	private string m_fromEmail = string.Empty;
	private string m_htmlBody = string.Empty;
	private bool m_isSending;
	private bool m_mailServerDown;
	private List<int> m_mailingListIDs = new List<int>();
	private int m_mailoutID;
	private int m_notSentMails;
	private double m_percentageCompleted;
	private string m_plainTextBody = string.Empty;
	private int m_sentMails;
	private string m_subject = string.Empty;
	private Queue<SubscriberInfo> m_subscribers;
	private int m_totalMails = -1;
	private bool mailServerError;
	private DataTable newsletterActions;
	private int newsletterFailedTypeID = (int)Classes.Newsletters.Action.SendFailed;
	private int newsletterSendTypeID = (int)Classes.Newsletters.Action.Send;
	private int numberOfFailedSendAttempts;

	/// <summary>
	/// Use this to send emails using a specified email server
	/// </summary>
	/// <param name="subject"></param>
	/// <param name="subscribers"></param>
	/// <param name="fromEmail"></param>
	/// <param name="emailServer"></param>
	/// <param name="plainTextBody"></param>
	/// <param name="htmlBody"></param>
	public EmailSender(string subject, Queue<SubscriberInfo> subscribers, string fromEmail, string emailServer, string plainTextBody, string htmlBody, AttachmentCollection attachments)
	{
		if (fromEmail == "")
			throw new InvalidEmailConfigurationException("From Email can't be an empty string");

		if (emailServer == "")
			throw new InvalidEmailConfigurationException("Email server can't be an empty string");

		if (htmlBody == "" && plainTextBody == "")
			throw new InvalidEmailConfigurationException("The body of the email can't be an empty string");

		if (subscribers.Count == 0)
			throw new InvalidEmailConfigurationException("There are no subscribers to send this email to");

		m_subject = subject;
		m_subscribers = subscribers;
		m_plainTextBody = plainTextBody;
		m_htmlBody = htmlBody;
		m_fromEmail = fromEmail;
		m_emailServer = emailServer;
		m_attachments = attachments;
	}

	public EmailSender(string subject, Queue<SubscriberInfo> subscribers, string fromEmail, string plainTextBody, string htmlBody, AttachmentCollection attachments)
	{
		if (fromEmail == "")
			throw new InvalidEmailConfigurationException("From Email can't be an empty string");

		if (htmlBody == "" && plainTextBody == "")
			throw new InvalidEmailConfigurationException("The body of the email can't be an empty string");

		if (subscribers.Count == 0)
			throw new InvalidEmailConfigurationException("There are no subscribers to send this email to");

		m_subject = subject;
		m_subscribers = subscribers;
		m_plainTextBody = plainTextBody;
		m_htmlBody = htmlBody;
		m_fromEmail = fromEmail;
		m_attachments = attachments;
	}

	public static ArrayList SendEmailJobs
	{
		get { return m_SendEmailJobs ?? (m_SendEmailJobs = new ArrayList()); }
		set { m_SendEmailJobs = value; }
	}

	public Queue<SubscriberInfo> Subscribers
	{
		get { return m_subscribers ?? (m_subscribers = new Queue<SubscriberInfo>()); }
	}

	public string Subject
	{
		get { return m_subject; }
		set { m_subject = value; }
	}

	public string PlainTextBody
	{
		get { return m_plainTextBody; }
		set { m_plainTextBody = value; }
	}

	public string HtmlBody
	{
		get { return m_htmlBody; }
		set { m_htmlBody = value; }
	}

	public string FromEmail
	{
		get { return m_fromEmail; }
		set { m_fromEmail = value; }
	}

	public string EmailServer
	{
		get { return m_emailServer; }
		set { m_emailServer = value; }
	}

	public AttachmentCollection Attachments
	{
		get { return m_attachments; }
		set { m_attachments = value; }
	}

	public bool IsSending
	{
		get { return m_isSending; }
		set { m_isSending = value; }
	}

	public double PercentageCompleted
	{
		get { return m_percentageCompleted; }
		set { m_percentageCompleted = value; }
	}

	public int TotalMails
	{
		get { return m_totalMails; }
		set { m_totalMails = value; }
	}

	public int SentMails
	{
		get { return m_sentMails; }
		set { m_sentMails = value; }
	}

	public int NotSentMails
	{
		get { return m_notSentMails; }
		set { m_notSentMails = value; }
	}

	public int MailoutID
	{
		get { return m_mailoutID; }
		set { m_mailoutID = value; }
	}

	public List<int> MailingListIDs
	{
		get { return m_mailingListIDs; }
		set { m_mailingListIDs = value; }
	}

	public bool MailServerDown
	{
		get { return m_mailServerDown; }
		set { m_mailServerDown = value; }
	}

	public string IPAddress
	{
		get { return m_IPAddress; }
		set { m_IPAddress = value; }
	}

	public bool EnableLogging
	{
		get { return m_enableLogging; }
		set { m_enableLogging = value; }
	}

	public int Send()
	{
		return StartThreads();
	}

	/// <summary>
	/// Sends emails
	/// </summary>
	private int StartThreads()
	{
		TotalMails = Subscribers.Count;
		SentMails = 0;
		PercentageCompleted = 0.0;
		IsSending = true;

		newsletterActions = new DataTable("NewsletterAction");
		newsletterActions.Columns.Add("NewsletterActionTypeID");
		newsletterActions.Columns.Add("MailoutID");
		newsletterActions.Columns.Add("Timestamp");
		newsletterActions.Columns.Add("IPAddress");
		newsletterActions.Columns.Add("SubscriberID");
		newsletterActions.Columns.Add("Email");

		int threadCount = TotalMails > 90 ? 90 : TotalMails;

		for (int i = 0; i < threadCount; i++)
		{
			Thread thread = new Thread(StartSend);
			thread.Start();
		}

		if (EnableLogging)
		{
			Thread saveThread = new Thread(SaveActions);
			saveThread.Start();
		}
		Thread progressThread = new Thread(ProgressThread);
		progressThread.Start();

		return 1;
	}

	private void SaveActions()
	{
		int sleepTime;
		if (TotalMails < 1000)
			sleepTime = TotalMails * 20; //Emails are sent roughly 50 per second
		else
			sleepTime = 30000;
		SqlBulkCopy bulkCopy = new SqlBulkCopy(Globals.Settings.ConnectionString);
		bulkCopy.DestinationTableName = "NewsletterAction";
		bulkCopy.ColumnMappings.Add("NewsletterActionTypeID", "NewsletterActionTypeID");
		bulkCopy.ColumnMappings.Add("MailoutID", "MailoutID");
		bulkCopy.ColumnMappings.Add("Timestamp", "Timestamp");
		bulkCopy.ColumnMappings.Add("IPAddress", "IPAddress");
		bulkCopy.ColumnMappings.Add("Email", "Email");
		bulkCopy.ColumnMappings.Add("SubscriberID", "SubscriberID");

		//Let it sleep for at least one second to give all other threads time to finish for a small mailing list
		Thread.Sleep(1000);

		while (!allThreadsFinished || newsletterActions.Rows.Count > 0)
		{
			if (newsletterActions.Rows.Count > 0)
			{
				Lock.AcquireWriterLock(Timeout.Infinite);
				bulkCopy.WriteToServer(newsletterActions);
				newsletterActions.Rows.Clear();
				Lock.ReleaseWriterLock();
			}
			Thread.Sleep(sleepTime);
		}

		if (mailServerError && Subscribers.Count == 0 && numberOfFailedSendAttempts < 2)
		{
			mailServerError = false;
			allThreadsFinished = false;
			m_subscribers = SubscriberInfo.GetMailingListSubscribersWithEmailsByListOfMailingListIDs(MailingListIDs, MailoutID);
			numberOfFailedSendAttempts++;
			NotSentMails = 0;
			StartThreads();
		}
		else
			IsSending = false;
	}

	private void ProgressThread()
	{
		while (!allThreadsFinished)
		{
			SentMails = TotalMails - Subscribers.Count;
			PercentageCompleted =
				(double)SentMails * 100 / TotalMails;
			Thread.Sleep(2500);
		}
		SentMails = TotalMails;
		PercentageCompleted =
			(double)SentMails * 100 / TotalMails;
	}

	/// <summary>
	/// Sends the emails to all subscribers
	/// </summary>
	private void StartSend()
	{
		bool lastItem = false;
		int numberOfMailServerErrors = 0;
		SmtpClient smtp = new SmtpClient();
		if (!String.IsNullOrEmpty(EmailServer))
		{
			smtp.Host = EmailServer;
		}
		else if (String.IsNullOrEmpty(smtp.Host))
		{
			smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
		}
		try
		{
			// send the emails
			for (; ; )
			{
				// make local copies of the message body so we don't get a race condition on the instance variables
				string htmlBodyCopy = HtmlBody;
				string plainTextBodyCopy = PlainTextBody;

				Lock.AcquireWriterLock(Timeout.Infinite);
				if (Subscribers.Count == 0 || MailServerDown)
				{
					Lock.ReleaseWriterLock();
					break;
				}

				SubscriberInfo subscriber = Subscribers.Dequeue();
				if (Subscribers.Count == 0)
					lastItem = true;
				Lock.ReleaseWriterLock();

				MailMessage mail = new MailMessage();
				mail.From = new MailAddress(FromEmail);
				mail.To.Add(subscriber.Email);
				mail.Subject = Subject;

				if (Attachments != null)
				{
					foreach (Attachment att in Attachments)
					{
						mail.Attachments.Add(att);
					}
				}

				if (subscriber.Replacements != null && subscriber.Replacements.Count > 0)
				{
					foreach (string key in subscriber.Replacements.Keys)
					{
						if (!string.IsNullOrEmpty(key) && subscriber.Replacements[key] != null)
						{
							htmlBodyCopy = htmlBodyCopy.Replace(key, subscriber.Replacements[key].ToString());
							plainTextBodyCopy = plainTextBodyCopy.Replace(key, subscriber.Replacements[key].ToString());
						}
					}
				}

				switch (subscriber.SubscriptionType)
				{
					case SubscriptionType.Html:
						mail.IsBodyHtml = true;
						mail.Body = htmlBodyCopy;
						break;
					case SubscriptionType.PlainText:
						mail.Body = plainTextBodyCopy;
						break;
					case SubscriptionType.MultiPart:
						AlternateView textView = AlternateView.CreateAlternateViewFromString(plainTextBodyCopy, null, "text/plain");
						textView.TransferEncoding = TransferEncoding.SevenBit;
						AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBodyCopy, null, "text/html");
						htmlView.TransferEncoding = TransferEncoding.SevenBit;
						mail.AlternateViews.Add(textView);
						mail.AlternateViews.Add(htmlView);
						break;
					default:
						mail.Body = htmlBodyCopy;
						break;
				}

				try
				{
					smtp.Send(mail);

					Lock.AcquireWriterLock(Timeout.Infinite);
					DataRow dr = newsletterActions.NewRow();
					dr["MailoutID"] = MailoutID;
					dr["Timestamp"] = DateTime.UtcNow;
					dr["IPAddress"] = IPAddress;
					dr["Email"] = subscriber.Email;
					dr["SubscriberID"] = subscriber.SubscriberID;
					dr["NewsletterActionTypeID"] = newsletterSendTypeID;
					newsletterActions.Rows.Add(dr);
					Lock.ReleaseWriterLock();
					if (lastItem)
						allThreadsFinished = true;
				}
				catch (SmtpException e)
				{
					mailServerError = true;
					numberOfMailServerErrors++;
					if (numberOfMailServerErrors > 2) //If the thread fails at the mail server 3 times in a row (thats 2 minutes of failure) assume the server is down and stop execution
						MailServerDown = true;
					Lock.AcquireWriterLock(Timeout.Infinite);
					NotSentMails++;
					Lock.ReleaseWriterLock();
					Helpers.LogException(e);
					if (lastItem)
						allThreadsFinished = true;
					Thread.Sleep(30000);
				}
			}
		}
		catch (Exception e)
		{
			Helpers.LogException(e);
		}
	}
}