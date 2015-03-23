using System;
using System.Collections.Generic;
using System.Linq;
using BaseCode;

namespace Classes.Newsletters
{
	public partial class Subscriber
	{

		public virtual void CustomInsert()
		{
			// see if a subscriber exists for the email
			Subscriber existingSubscriber = SubscriberGetByEmail(Email).FirstOrDefault();
			if (existingSubscriber == null || existingSubscriber.IsNewRecord)
				Save();
			else
				SubscriberID = existingSubscriber.SubscriberID;
			Helpers.PurgeCacheItems("Newsletters_Subscriber");
			// Not sure if I agree with how this method is implemented, but it matches the previous sproc functionality.
		}

		public static List<Subscriber> GetSubscribersNotSentForMailout(int mailoutId, int mailingListId)
		{
			List<Subscriber> objects;
			string key = cacheKeyPrefix + "GetSubscribersNotSentForMailout_" + mailoutId + "_" + mailingListId;

			List<Subscriber> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<Subscriber>;

			if (tmpList != null)
				objects = tmpList.Select(entity => new Subscriber(entity)).ToList();
			else
			{
				using (Entities entity = new Entities())
				{
					objects = (from subscriber in entity.Subscriber
					           join mailingListSubscriber in entity.MailingListSubscriber on subscriber.SubscriberID equals mailingListSubscriber.SubscriberID
							   join mailoutMailingList in entity.MailoutMailingList on mailingListSubscriber.MailingListID equals mailoutMailingList.MailingListID
							   where mailingListSubscriber.MailingListID == mailingListId
									&& mailoutMailingList.MailoutID == mailoutId
									&& !subscriber.Deleted
									&& mailingListSubscriber.Active
									&& !entity.NewsletterAction.Any(e => e.SubscriberID == subscriber.SubscriberID && e.NewsletterActionTypeID == (int)Action.Send && e.MailoutID == mailoutId)
							   select subscriber).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;

		}

		public static int GetSubscriberCountForMailout(int mailoutID)
		{
			if (mailoutID <= 0) return 0;

			int count;
			string key = cacheKeyPrefix + "GetSubscriberCountForMailout" + mailoutID;

			if (Cache.IsEnabled && Cache[key] != null)
				count = Convert.ToInt32(Cache[key]);
			else
			{
				using (Entities entity = new Entities())
				{
					count = (from subscriber in entity.Subscriber
							 join mailingListSubscriber in entity.MailingListSubscriber on subscriber.SubscriberID equals mailingListSubscriber.SubscriberID
							 join mailoutMailingList in entity.MailoutMailingList on mailingListSubscriber.MailingListID equals mailoutMailingList.MailingListID
							 where mailoutMailingList.MailoutID == mailoutID
								&& !subscriber.Deleted
								&& mailingListSubscriber.Active
							 select subscriber.Email).Distinct().Count();
				}
				Cache.Store(key, count);
			}

			return count;
		}

		public static Subscriber GetSubscriberByEntityID(Guid entityID)
		{
			Subscriber obj;
			string key = cacheKeyPrefix + "GetSubscriberByEntityID_" + entityID;

			Subscriber tmpObj = null;

			if (Cache.IsEnabled)
				tmpObj = Cache[key] as Subscriber;

			if (tmpObj != null)
				obj = tmpObj;
			else
			{
				using (Entities entity = new Entities())
				{
					obj = entity.Subscriber.Where(s => s.MailingListSubscriber.Any(m => m.EntityID == entityID)).FirstOrDefault();
				}
				Cache.Store(key, obj);
			}

			return obj;
		}
	}
}