using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Newsletters
{
	public partial class Subscriber : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Newsletters_Subscriber_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Email"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public Subscriber(Subscriber objectToCopy)
		{
			DefaultNewsletterFormatID = objectToCopy.DefaultNewsletterFormatID;
			Deleted = objectToCopy.Deleted;
			Email = objectToCopy.Email;
			SubscriberID = objectToCopy.SubscriberID;
			UserID = objectToCopy.UserID;
		}

		public virtual bool IsNewRecord
		{
			get { return SubscriberID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("Subscriber", this);
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

		public static Subscriber GetByID(int SubscriberID, IEnumerable<string> includeList = null)
		{
			Subscriber obj = null;
			string key = cacheKeyPrefix + SubscriberID + GetCacheIncludeText(includeList);

			Subscriber tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Subscriber;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Subscriber> itemQuery = AddIncludes(entity.Subscriber, includeList);
					obj = itemQuery.FirstOrDefault(n => n.SubscriberID == SubscriberID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Subscriber> GetAll(IEnumerable<string> includeList = null)
		{
			List<Subscriber> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Subscriber> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Subscriber>();
				tmpList = Cache[key] as List<Subscriber>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Subscriber> itemQuery = AddIncludes(entity.Subscriber, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Subscriber> SubscriberGetByDeleted(Boolean Deleted, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSubscriberDeleted = Deleted.ToString();
			return SubscriberPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Subscriber> SubscriberGetByEmail(String Email, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSubscriberEmail = Email.ToString();
			return SubscriberPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Subscriber> SubscriberPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Subscriber> objects = SubscriberPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Subscriber> SubscriberPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return SubscriberPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Subscriber> SubscriberPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return SubscriberPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Subscriber> SubscriberPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SubscriberID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Subscriber> objects;
			string baseKey = cacheKeyPrefix + "SubscriberPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Subscriber> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Subscriber>();
				tmpList = Cache[key] as List<Subscriber>;
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
					IQueryable<Subscriber> itemQuery = SetupQuery(entity.Subscriber, "Subscriber", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Newsletters_Subscriber");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterSubscriberDeleted { get; set; }
			public string FilterSubscriberEmail { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterSubscriberDeleted != null)
				{
					if (FilterSubscriberDeleted == string.Empty)
						filterList.Add("@FilterSubscriberDeleted", string.Empty);
					else
						filterList.Add("@FilterSubscriberDeleted", Convert.ToBoolean(FilterSubscriberDeleted));
				}
				if (FilterSubscriberEmail != null)
					filterList.Add("@FilterSubscriberEmail", FilterSubscriberEmail);
				return filterList;
			}
		}
	}
}