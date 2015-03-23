using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classes.Newsletters
{
	public partial class NewsletterAction
	{
		public static NewsletterAction CreateOpenAction(Subscriber subscriber, int mailoutID)
		{
			var action = new NewsletterAction();
			action.SetCommonProperties(subscriber, mailoutID);
			action.NewsletterActionTypeID = (int)Action.Open;
			action.Save();
			return action;
		}

		public static NewsletterAction CreateClickAction(Subscriber subscriber, int mailoutID, String url)
		{
			var action = new NewsletterAction();
			action.SetCommonProperties(subscriber, mailoutID);
			action.NewsletterActionTypeID = (int)Action.Click;
			action.Details = url;
			action.Save();
			return action;
		}

		public static NewsletterAction CreateUnsubscribeAction(Subscriber subscriber, int mailoutID)
		{
			var action = new NewsletterAction();
			action.SetCommonProperties(subscriber, mailoutID);
			action.NewsletterActionTypeID = (int)Action.Unsubscribe;
			action.Save();
			return action;
		}

		public static NewsletterAction CreateForwardAction(Subscriber subscriber, int mailoutID, string toEmail)
		{
			var action = new NewsletterAction();
			action.SetCommonProperties(subscriber, mailoutID);
			action.NewsletterActionTypeID = (int)Action.Forward;
			action.Details = toEmail;
			action.Save();
			return action;
		}

		protected void SetCommonProperties(Subscriber subscriber, int mailoutID)
		{
			SubscriberID = subscriber.SubscriberID;
			Email = subscriber.Email;
			MailoutID = mailoutID;
			Timestamp = DateTime.UtcNow;
			IPAddress = HttpContext.Current.Request.UserHostAddress;
		}

		public static List<NewsletterAction> NewsletterActionGetForMetricsPage(int mailoutID, int actionTypeId, int pageNumber, int pageSize, out int totalCount)
		{
			List<NewsletterAction> objects;
			totalCount = 0;
			string baseKey = cacheKeyPrefix + "GetForMetricsPage_" + mailoutID + "_" + actionTypeId;
			string key = baseKey + "_" + pageNumber + "_" + pageSize;
			string countkey = baseKey + "_count";

			List<NewsletterAction> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<NewsletterAction>;
				tmpInt = Cache[countkey] as int?;
			}

			if (tmpList != null && tmpInt.HasValue)
			{
				objects = tmpList;
				totalCount = tmpInt.Value;
			}
			else
			{
				using (Entities entity = new Entities())
				{
					var baseQuery = from newsletterAction in entity.NewsletterAction
									where newsletterAction.NewsletterActionTypeID == actionTypeId &&
										  newsletterAction.MailoutID == mailoutID
									group newsletterAction by new { newsletterAction.SubscriberID, newsletterAction.Email, newsletterAction.IPAddress, newsletterAction.Details } into g
									orderby g.Key.Email
									select new
									{
										NewsletterActionID = g.Min(p => p.NewsletterActionID),
										NewsletterActionTypeID = g.Min(p => p.NewsletterActionTypeID),
										MailoutID = g.Min(p => p.MailoutID),
										SubscriberID = g.Key.SubscriberID,
										Email = g.Key.Email,
										Timestamp = g.Min(p => p.Timestamp),
										IPAddress = g.Key.IPAddress,
										Details = g.Key.Details
									};

					var tempObjects = pageSize == 0 ? baseQuery.ToList() : baseQuery.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
					totalCount = tmpInt.HasValue ? tmpInt.Value : baseQuery.Count();

					objects = new List<NewsletterAction>();
					foreach (var temp in tempObjects)
					{
						NewsletterAction obj = new NewsletterAction { Details = temp.Details, Email = temp.Email, IPAddress = temp.IPAddress, MailoutID = temp.MailoutID, NewsletterActionID = temp.NewsletterActionID, NewsletterActionTypeID = temp.NewsletterActionTypeID, SubscriberID = temp.SubscriberID, Timestamp = temp.Timestamp };
						objects.Add(obj);
					}
				}

				Cache.Store(key, objects);
				Cache.Store(countkey, totalCount);
			}
			return objects;
		}

		public static List<UrlClickCountContainer> GetUrlClickCountPage(int mailoutID, int pageNumber, int pageSize, out int totalCount)
		{
			List<UrlClickCountContainer> objects;
			string baseKey = cacheKeyPrefix + "GetClicksForUrlsPage_" + mailoutID;
			string key = baseKey + "_" + pageNumber + "_" + pageSize;
			string countkey = baseKey + "_count";

			List<UrlClickCountContainer> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<UrlClickCountContainer>;
				tmpInt = Cache[countkey] as int?;
			}

			if (tmpList != null && tmpInt.HasValue)
			{
				objects = tmpList;
				totalCount = tmpInt.Value;
			}
			else
			{
				using (Entities entity = new Entities())
				{
					var baseQuery = (from urls in
										 from newsletterAction in entity.NewsletterAction
										 where newsletterAction.NewsletterActionTypeID == (int)Action.Click
											 && newsletterAction.MailoutID == mailoutID
										 group newsletterAction by new { newsletterAction.SubscriberID, newsletterAction.Details, newsletterAction.Timestamp } into g
										 select new { Url = g.Key.Details }
									 group urls by new { urls.Url } into g
									 select new { Url = g.Key.Url, Count = g.Count(p => p.Url != null) }).Distinct().OrderBy(u => u.Url);

					var tempObjects = pageSize == 0 ? baseQuery.ToList() : baseQuery.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
					totalCount = tmpInt.HasValue ? tmpInt.Value : baseQuery.Count();

					objects = new List<UrlClickCountContainer>();
					foreach (var temp in tempObjects)
					{
						UrlClickCountContainer obj = new UrlClickCountContainer(temp.Url, temp.Count);
						objects.Add(obj);
					}
				}

				Cache.Store(key, objects);
				Cache.Store(countkey, totalCount);
			}
			return objects;
		}

		public static List<String> GetSubscribersNotSentEmailPage(int mailoutId, int pageNumber, int pageSize, out int totalCount, int? mailingListId = null)
		{
			List<String> objects;
			totalCount = 0;
			string baseKey = cacheKeyPrefix + "GetSubscribersNotSentForMailout_" + mailoutId + "_" + mailingListId;
			string key = baseKey + "_" + pageNumber + "_" + pageSize;
			string countkey = baseKey + "_count";

			List<String> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<String>;
				tmpInt = Cache[countkey] as int?;
			}

			if (tmpList != null && tmpInt.HasValue)
			{
				objects = tmpList;
				totalCount = tmpInt.Value;
			}
			else
			{
				using (Entities entity = new Entities())
				{
					var baseQuery = (from mailout in entity.Mailout
									 join mailoutMailingList in entity.MailoutMailingList on mailout.MailoutID equals mailoutMailingList.MailoutID
									 join mailingList in entity.MailingList on mailoutMailingList.MailingListID equals mailingList.MailingListID
									 join mailingListSubscriber in entity.MailingListSubscriber on mailingList.MailingListID equals mailingListSubscriber.MailingListID
									 join subscriber in entity.Subscriber on mailingListSubscriber.SubscriberID equals subscriber.SubscriberID
									 where (!mailingListId.HasValue || mailingListSubscriber.MailingListID == mailingListId.Value)
										   && mailout.MailoutID == mailoutId
										   && !subscriber.Deleted
										   && mailingListSubscriber.Active
										   && !entity.NewsletterAction.Any(a => a.NewsletterActionTypeID == (int)Action.Send && a.MailoutID == mailoutId && a.SubscriberID == subscriber.SubscriberID)
									 select subscriber.Email).Distinct().OrderBy(s => s);

					objects = pageSize == 0 ? baseQuery.ToList() : baseQuery.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
					totalCount = tmpInt.HasValue ? tmpInt.Value : baseQuery.Count();
				}
				Cache.Store(key, objects);
				Cache.Store(countkey, totalCount);

			}
			return objects;
		}

		public static int GetNumberOfUsersEmailedInLastMonth()
		{
			return GetNumberOfUsersEmailedInPeriod(DateTime.UtcNow.AddDays(-31), DateTime.UtcNow);
		}

		public static int GetNumberOfUsersEmailedInPeriod(DateTime beginDate, DateTime endDate)
		{
			int count;
			string key = cacheKeyPrefix + "GetNumberOfUsersEmailedInPeriod_" + beginDate + "_" + endDate;

			if (Cache.IsEnabled && Cache[key] != null)
				count = Convert.ToInt32(Cache[key]);
			else
			{
				using (Entities entity = new Entities())
				{
					count = entity.NewsletterAction.Count(n => n.NewsletterActionID == (int)Action.Send && beginDate <= n.Timestamp && n.Timestamp <= endDate);
				}
				Cache.Store(key, count);
			}

			return count;
		}


		public static int GetNotSentCountForMailout(int mailoutID)
		{
			/// TODO: Consolidate into GetActionMetricsForMailout(int mailoutID)
			if (mailoutID <= 0) return 0;

			int count;
			string key = cacheKeyPrefix + "GetNotSentCountForMailout_" + mailoutID;

			if (Cache.IsEnabled && Cache[key] != null)
				count = Convert.ToInt32(Cache[key]);
			else
			{
				using (Entities entity = new Entities())
				{
					count = entity.MailingListSubscriber.Where(s => s.Active && !s.Subscriber.Deleted && s.MailingList.MailoutMailingList.Any(m => m.MailoutID == mailoutID) && !entity.NewsletterAction.Any(n => n.MailoutID == mailoutID && n.NewsletterActionTypeID == (int)Action.Send && n.SubscriberID == s.SubscriberID)).Select(s => s.SubscriberID).Distinct().Count();
				}
				Cache.Store(key, count);
			}

			return count;
		}

		public static int GetSendCountForMailout(int mailoutID)
		{
			/// TODO: Consolidate into GetActionMetricsForMailout(int mailoutID)
			if (mailoutID <= 0) return 0;

			int count;
			string key = cacheKeyPrefix + "GetSendCountForMailout_" + mailoutID;

			if (Cache.IsEnabled && Cache[key] != null)
				count = Convert.ToInt32(Cache[key]);
			else
			{
				using (Entities entity = new Entities())
				{
					count = entity.NewsletterAction.Where(n => n.MailoutID == mailoutID && n.NewsletterActionTypeID == (int)Action.Send).Select(n => n.SubscriberID).Distinct().Count();
				}
				Cache.Store(key, count);
			}

			return count;
		}

		public static int GetOpenCountForMailout(int mailoutID)
		{
			/// TODO: Consolidate into GetActionMetricsForMailout(int mailoutID)
			if (mailoutID <= 0) return 0;

			int count;
			string key = cacheKeyPrefix + "GetOpenCountForMailout_" + mailoutID;

			if (Cache.IsEnabled && Cache[key] != null)
				count = Convert.ToInt32(Cache[key]);
			else
			{
				using (Entities entity = new Entities())
				{
					count = entity.NewsletterAction.Where(n => n.MailoutID == mailoutID && n.NewsletterActionTypeID == (int)Action.Open).Select(n => n.SubscriberID).Distinct().Count();
				}
				Cache.Store(key, count);
			}

			return count;
		}

		public static int GetClickCountForMailout(int mailoutID)
		{
			/// TODO: Consolidate into GetActionMetricsForMailout(int mailoutID)
			if (mailoutID <= 0) return 0;

			int count;
			string key = cacheKeyPrefix + "GetClickCountForMailout_" + mailoutID;

			if (Cache.IsEnabled && Cache[key] != null)
				count = Convert.ToInt32(Cache[key]);
			else
			{
				using (Entities entity = new Entities())
				{
					count = entity.NewsletterAction.Where(n => n.MailoutID == mailoutID && n.NewsletterActionTypeID == (int)Action.Click).Select(n => n.SubscriberID).Distinct().Count();
				}
				Cache.Store(key, count);
			}

			return count;
		}

		public static int GetForwardCountForMailout(int mailoutID)
		{
			/// TODO: Consolidate into GetActionMetricsForMailout(int mailoutID)
			if (mailoutID <= 0) return 0;

			int count;
			string key = cacheKeyPrefix + "GetForwardCountForMailout_" + mailoutID;

			if (Cache.IsEnabled && Cache[key] != null)
				count = Convert.ToInt32(Cache[key]);
			else
			{
				using (Entities entity = new Entities())
				{
					count = entity.NewsletterAction.Where(n => n.MailoutID == mailoutID && n.NewsletterActionTypeID == (int)Action.Forward).Select(n => n.SubscriberID).Distinct().Count();
				}
				Cache.Store(key, count);
			}

			return count;
		}

		public static int GetUnsubscribeCountForMailout(int mailoutID)
		{
			/// TODO: Consolidate into GetActionMetricsForMailout(int mailoutID)
			if (mailoutID <= 0) return 0;

			int count;
			string key = cacheKeyPrefix + "GetUnsubscribeCountForMailout_" + mailoutID;

			if (Cache.IsEnabled && Cache[key] != null)
				count = Convert.ToInt32(Cache[key]);
			else
			{
				using (Entities entity = new Entities())
				{
					count = entity.NewsletterAction.Where(n => n.MailoutID == mailoutID && n.NewsletterActionTypeID == (int)Action.Unsubscribe).Select(n => n.SubscriberID).Distinct().Count();
				}
				Cache.Store(key, count);
			}

			return count;
		}

	}
}