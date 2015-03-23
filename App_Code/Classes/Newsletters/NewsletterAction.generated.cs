using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Newsletters
{
	public partial class NewsletterAction : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Newsletters_NewsletterAction_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public NewsletterAction()
		{
		}

		public NewsletterAction(NewsletterAction objectToCopy)
		{
			Details = objectToCopy.Details;
			Email = objectToCopy.Email;
			IPAddress = objectToCopy.IPAddress;
			MailoutID = objectToCopy.MailoutID;
			NewsletterActionID = objectToCopy.NewsletterActionID;
			NewsletterActionTypeID = objectToCopy.NewsletterActionTypeID;
			SubscriberID = objectToCopy.SubscriberID;
			Timestamp = objectToCopy.Timestamp;
		}

		public virtual bool IsNewRecord
		{
			get { return NewsletterActionID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime TimestampClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(Timestamp); }
		}

		public virtual void Save()
		{
			SaveEntity("NewsletterAction", this);
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

		public static NewsletterAction GetByID(int NewsletterActionID, IEnumerable<string> includeList = null)
		{
			NewsletterAction obj = null;
			string key = cacheKeyPrefix + NewsletterActionID + GetCacheIncludeText(includeList);

			NewsletterAction tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as NewsletterAction;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsletterAction> itemQuery = AddIncludes(entity.NewsletterAction, includeList);
					obj = itemQuery.FirstOrDefault(n => n.NewsletterActionID == NewsletterActionID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<NewsletterAction> GetAll(IEnumerable<string> includeList = null)
		{
			List<NewsletterAction> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<NewsletterAction> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsletterAction>();
				tmpList = Cache[key] as List<NewsletterAction>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsletterAction> itemQuery = AddIncludes(entity.NewsletterAction, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<NewsletterAction> NewsletterActionGetByMailoutID(Int32 MailoutID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterActionMailoutID = MailoutID.ToString();
			return NewsletterActionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<NewsletterAction> NewsletterActionPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<NewsletterAction> objects = NewsletterActionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<NewsletterAction> NewsletterActionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return NewsletterActionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<NewsletterAction> NewsletterActionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return NewsletterActionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<NewsletterAction> NewsletterActionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "NewsletterActionID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<NewsletterAction> objects;
			string baseKey = cacheKeyPrefix + "NewsletterActionPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<NewsletterAction> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsletterAction>();
				tmpList = Cache[key] as List<NewsletterAction>;
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
					IQueryable<NewsletterAction> itemQuery = SetupQuery(entity.NewsletterAction, "NewsletterAction", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Newsletters_NewsletterAction");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterNewsletterActionMailoutID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterNewsletterActionMailoutID != null)
				{
					if (FilterNewsletterActionMailoutID == string.Empty)
						filterList.Add("@FilterNewsletterActionMailoutID", string.Empty);
					else
						filterList.Add("@FilterNewsletterActionMailoutID", Convert.ToInt32(FilterNewsletterActionMailoutID));
				}
				return filterList;
			}
		}
	}
}