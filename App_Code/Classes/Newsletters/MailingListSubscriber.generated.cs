using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Newsletters
{
	public partial class MailingListSubscriber : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Newsletters_MailingListSubscriber_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public MailingListSubscriber()
		{
		}

		public MailingListSubscriber(MailingListSubscriber objectToCopy)
		{
			Active = objectToCopy.Active;
			EntityID = objectToCopy.EntityID;
			MailingListID = objectToCopy.MailingListID;
			MailingListSubscriberID = objectToCopy.MailingListSubscriberID;
			NewsletterFormatID = objectToCopy.NewsletterFormatID;
			SubscriberID = objectToCopy.SubscriberID;
		}

		public virtual bool IsNewRecord
		{
			get { return MailingListSubscriberID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("MailingListSubscriber", this);
			ClearCache();
			ClearRelatedCacheItems();
			SaveSearch();
		}

		public virtual void Delete()
		{
			DeleteSEO();
			DeleteSearch();
			using (Entities entity = new Entities())
			{
				entity.Entry(this).State = System.Data.EntityState.Deleted;
				entity.SaveChanges();
			}
			ClearCache();
			ClearRelatedCacheItems();
		}

		public static MailingListSubscriber GetByID(int MailingListSubscriberID, IEnumerable<string> includeList = null)
		{
			MailingListSubscriber obj = null;
			string key = cacheKeyPrefix + MailingListSubscriberID + GetCacheIncludeText(includeList);

			MailingListSubscriber tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as MailingListSubscriber;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<MailingListSubscriber> itemQuery = AddIncludes(entity.MailingListSubscriber, includeList);
					obj = itemQuery.FirstOrDefault(n => n.MailingListSubscriberID == MailingListSubscriberID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<MailingListSubscriber> GetAll(IEnumerable<string> includeList = null)
		{
			List<MailingListSubscriber> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<MailingListSubscriber> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<MailingListSubscriber>();
				tmpList = Cache[key] as List<MailingListSubscriber>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<MailingListSubscriber> itemQuery = AddIncludes(entity.MailingListSubscriber, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<MailingListSubscriber> MailingListSubscriberGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMailingListSubscriberActive = Active.ToString();
			return MailingListSubscriberPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<MailingListSubscriber> MailingListSubscriberGetByMailingListID(Int32 MailingListID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMailingListSubscriberMailingListID = MailingListID.ToString();
			return MailingListSubscriberPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<MailingListSubscriber> MailingListSubscriberGetByNewsletterFormatID(Int32? NewsletterFormatID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMailingListSubscriberNewsletterFormatID = NewsletterFormatID.ToString();
			return MailingListSubscriberPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<MailingListSubscriber> MailingListSubscriberGetBySubscriberID(Int32 SubscriberID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMailingListSubscriberSubscriberID = SubscriberID.ToString();
			return MailingListSubscriberPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		/// <summary>
		/// The total number of records that will be displayed in the grid
		/// </summary>
		/// <returns></returns>
		public static int SelectCount()
		{
			//return the itemcount that was set when the records were retrieved
			return m_ItemCount;
		}

		public static int SelectCount(string searchText, string sortField, bool sortDirection)
		{
			return SelectCount();
		}

		public static int SelectCount(string searchText, string sortField, bool sortDirection, Filters filterList)
		{
			return SelectCount();
		}


		public static int SelectCount(string searchText, string sortField, bool sortDirection, Filters filterList, IEnumerable<string> includeList)
		{
			return SelectCount();
		}

		public static List<MailingListSubscriber> MailingListSubscriberPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<MailingListSubscriber> objects = MailingListSubscriberPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<MailingListSubscriber> MailingListSubscriberPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return MailingListSubscriberPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<MailingListSubscriber> MailingListSubscriberPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return MailingListSubscriberPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<MailingListSubscriber> MailingListSubscriberPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "MailingListSubscriberID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<MailingListSubscriber> objects;
			string baseKey = cacheKeyPrefix + "MailingListSubscriberPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<MailingListSubscriber> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<MailingListSubscriber>();
				tmpList = Cache[key] as List<MailingListSubscriber>;
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
					IQueryable<MailingListSubscriber> itemQuery = SetupQuery(entity.MailingListSubscriber, "MailingListSubscriber", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

					objects = maximumRows <= 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : (maximumRows <= 0 || (pageNumber == 1 && objects.Count < maximumRows) ? objects.Count : itemQuery.Count());
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		/// <summary>
		/// Clear the cache, if caching is enabled
		/// </summary>
		public static void ClearCache()
		{
			if (Cache.IsEnabled)
				Cache.Purge("Newsletters_MailingListSubscriber");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterMailingListSubscriberActive { get; set; }
			public string FilterMailingListSubscriberMailingListID { get; set; }
			public string FilterMailingListSubscriberNewsletterFormatID { get; set; }
			public string FilterMailingListSubscriberSubscriberID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterMailingListSubscriberActive != null)
				{
					if (FilterMailingListSubscriberActive == string.Empty)
						filterList.Add("@FilterMailingListSubscriberActive", string.Empty);
					else
						filterList.Add("@FilterMailingListSubscriberActive", Convert.ToBoolean(FilterMailingListSubscriberActive));
				}
				if (FilterMailingListSubscriberMailingListID != null)
				{
					if (FilterMailingListSubscriberMailingListID == string.Empty)
						filterList.Add("@FilterMailingListSubscriberMailingListID", string.Empty);
					else
						filterList.Add("@FilterMailingListSubscriberMailingListID", Convert.ToInt32(FilterMailingListSubscriberMailingListID));
				}
				if (FilterMailingListSubscriberNewsletterFormatID != null)
				{
					if (FilterMailingListSubscriberNewsletterFormatID == string.Empty)
						filterList.Add("@FilterMailingListSubscriberNewsletterFormatID", string.Empty);
					else
						filterList.Add("@FilterMailingListSubscriberNewsletterFormatID", Convert.ToInt32(FilterMailingListSubscriberNewsletterFormatID));
				}
				if (FilterMailingListSubscriberSubscriberID != null)
				{
					if (FilterMailingListSubscriberSubscriberID == string.Empty)
						filterList.Add("@FilterMailingListSubscriberSubscriberID", string.Empty);
					else
						filterList.Add("@FilterMailingListSubscriberSubscriberID", Convert.ToInt32(FilterMailingListSubscriberSubscriberID));
				}
				return filterList;
			}
		}
	}
}