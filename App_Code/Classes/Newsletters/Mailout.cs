using System;
using System.Collections.Generic;

namespace Classes.Newsletters
{
	public partial class Mailout
	{
		#region Tracking Properties

		private int? m_NotSentCount;
		public int NotSentCount
		{
			get
			{
				if (!m_NotSentCount.HasValue) m_NotSentCount = Newsletters.NewsletterAction.GetNotSentCountForMailout(MailoutID);
				return m_NotSentCount.Value >= 0 ? m_NotSentCount.Value : 0;
			}
		}

		private int? m_SendCount;
		public int SendCount
		{
			get
			{
				if (!m_SendCount.HasValue) m_SendCount = Newsletters.NewsletterAction.GetSendCountForMailout(MailoutID);
				return m_SendCount.Value >= 0 ? m_SendCount.Value : 0;
			}
		}

		private int? m_OpenCount;
		public int OpenCount
		{
			get
			{
				if (!m_OpenCount.HasValue) m_OpenCount = Newsletters.NewsletterAction.GetOpenCountForMailout(MailoutID);
				return m_OpenCount.Value >= 0 ? m_OpenCount.Value : 0;
			}
		}

		private int? m_ClickCount;
		public int ClickCount
		{
			get
			{
				if (!m_ClickCount.HasValue) m_ClickCount = Newsletters.NewsletterAction.GetClickCountForMailout(MailoutID);
				return m_ClickCount.Value >= 0 ? m_ClickCount.Value : 0;
			}
		}

		private int? m_ForwardCount;
		public int ForwardCount
		{
			get
			{
				if (!m_ForwardCount.HasValue) m_ForwardCount = Newsletters.NewsletterAction.GetForwardCountForMailout(MailoutID);
				return m_ForwardCount.Value >= 0 ? m_ForwardCount.Value : 0;
			}
		}

		private int? m_UnsubscribeCount;
		public int UnsubscribeCount
		{
			get
			{
				if (!m_UnsubscribeCount.HasValue) m_UnsubscribeCount = Newsletters.NewsletterAction.GetUnsubscribeCountForMailout(MailoutID);
				return m_UnsubscribeCount.Value >= 0 ? m_UnsubscribeCount.Value : 0;
			}
		}

		private int? m_SubscriberCount;
		public int SubscriberCount
		{
			get
			{
				if (!m_SubscriberCount.HasValue) m_SubscriberCount = Subscriber.GetSubscriberCountForMailout(MailoutID);
				return m_SubscriberCount.Value >= 0 ? m_SubscriberCount.Value : 0;
			}
		}

		private List<String> m_MailingListNames;
		public List<String> MailingListNames
		{
			get { return m_MailingListNames ?? (m_MailingListNames = new List<String>()); }
		}

		#endregion

	}
}