using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Newsletters
{
	public partial class MailingListSubscriber
	{
		public static int GetCountByMailingListIDActive(int mailingListID, bool active)
		{
			int count;
			string key = cacheKeyPrefix + "GetCountByMailingListIDActive_" + mailingListID + "_" + active;

			if (Cache.IsEnabled && Cache[key] != null)
				count = Convert.ToInt32(Cache[key]);
			else
			{
				using (Entities entity = new Entities())
				{
					count = entity.MailingListSubscriber.Count(n => n.MailingListID == mailingListID && n.Active == active);
				}
				Cache.Store(key, count);
			}

			return count;
		}

		public static List<string> GetMailingListSubscriberEmails(int mailingListID)
		{
			List<string> objects;
			string key = cacheKeyPrefix + "GetMailingListSubscriberEmails_" + mailingListID;

			List<string> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<string>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = (from mailingListSubscriber in entity.MailingListSubscriber
							   join subscriber in entity.Subscriber on mailingListSubscriber.SubscriberID equals subscriber.SubscriberID
							   where mailingListSubscriber.MailingListID == mailingListID
							   select subscriber.Email).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;

		}

		public static List<SubscriberEmailWithFormatContainer> GetSubscriberEmailsForExport(int mailingListID)
		{
			List<SubscriberEmailWithFormatContainer> objects;

			using (Entities entity = new Entities())
			{
				objects = (from mailingListSubscriber in entity.MailingListSubscriber
						   join subscriber in entity.Subscriber on mailingListSubscriber.SubscriberID equals subscriber.SubscriberID into subscriber_join
						   from subscriber in subscriber_join.DefaultIfEmpty()
						   join newsletterformat in entity.NewsletterFormat on new { subscriber.DefaultNewsletterFormatID } equals new { DefaultNewsletterFormatID = newsletterformat.NewsletterFormatID }
						   where mailingListSubscriber.MailingListID == mailingListID
						   orderby subscriber.Email
						   select new SubscriberEmailWithFormatContainer(subscriber.Email, newsletterformat.Name)).ToList();
			}

			return objects;
		}

		public static List<MailingListSubscriber> MailingListSubscriberPageBySubscriberEmail(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "MailingListSubscriberID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText);

			List<MailingListSubscriber> objects;
			string baseKey = cacheKeyPrefix + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<MailingListSubscriber> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<MailingListSubscriber>;
				tmpInt = Cache[countKey] as int?;
			}


			if (tmpList != null && tmpInt.HasValue)
			{
				objects = tmpList.Select(entity => new MailingListSubscriber(entity)).ToList();
				m_ItemCount = tmpInt.Value;
			}
			else
			{
				int pageNumber = maximumRows > 0 ? 1 + startRowIndex / maximumRows : 1;

				using (Entities entity = new Entities())
				{
					IQueryable<MailingListSubscriber> itemQuery = SetupWhereClause(entity.MailingListSubscriber, "MailingListSubscriber", filterList.GetFilterList(), null, null);
					if (sortField != "Email")
						itemQuery = SetupOrderByClause(itemQuery, sortField, sortDirection);

					if (!string.IsNullOrWhiteSpace(searchText) || sortField == "Email")
					{
						if (!string.IsNullOrWhiteSpace(searchText))
							itemQuery = itemQuery.Where(mls => mls.Subscriber.Email.Contains(searchText));

						if (sortField == "Email")
							itemQuery = sortDirection ? itemQuery.OrderBy(a => a.Subscriber.Email) : itemQuery.OrderByDescending(a => a.Subscriber.Email);
					}

					objects = maximumRows == 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : itemQuery.Count();
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}
	}
}