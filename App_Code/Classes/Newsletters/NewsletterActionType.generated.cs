using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Newsletters
{
	public partial class NewsletterActionType : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Newsletters_NewsletterActionType_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "DisplayName", "Type"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public NewsletterActionType(NewsletterActionType objectToCopy)
		{
			DisplayName = objectToCopy.DisplayName;
			NewsletterActionTypeID = objectToCopy.NewsletterActionTypeID;
			Type = objectToCopy.Type;
		}

		public virtual bool IsNewRecord
		{
			get { return NewsletterActionTypeID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("NewsletterActionType", this);
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

		public static NewsletterActionType GetByID(int NewsletterActionTypeID, IEnumerable<string> includeList = null)
		{
			NewsletterActionType obj = null;
			string key = cacheKeyPrefix + NewsletterActionTypeID + GetCacheIncludeText(includeList);

			NewsletterActionType tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as NewsletterActionType;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsletterActionType> itemQuery = AddIncludes(entity.NewsletterActionType, includeList);
					obj = itemQuery.FirstOrDefault(n => n.NewsletterActionTypeID == NewsletterActionTypeID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<NewsletterActionType> GetAll(IEnumerable<string> includeList = null)
		{
			List<NewsletterActionType> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<NewsletterActionType> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsletterActionType>();
				tmpList = Cache[key] as List<NewsletterActionType>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsletterActionType> itemQuery = AddIncludes(entity.NewsletterActionType, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<NewsletterActionType> NewsletterActionTypeGetByDisplayName(String DisplayName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterActionTypeDisplayName = DisplayName.ToString();
			return NewsletterActionTypePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<NewsletterActionType> NewsletterActionTypeGetByType(String Type, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterActionTypeType = Type.ToString();
			return NewsletterActionTypePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<NewsletterActionType> NewsletterActionTypePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<NewsletterActionType> objects = NewsletterActionTypePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<NewsletterActionType> NewsletterActionTypePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return NewsletterActionTypePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<NewsletterActionType> NewsletterActionTypePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return NewsletterActionTypePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<NewsletterActionType> NewsletterActionTypePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "NewsletterActionTypeID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<NewsletterActionType> objects;
			string baseKey = cacheKeyPrefix + "NewsletterActionTypePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<NewsletterActionType> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsletterActionType>();
				tmpList = Cache[key] as List<NewsletterActionType>;
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
					IQueryable<NewsletterActionType> itemQuery = SetupQuery(entity.NewsletterActionType, "NewsletterActionType", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Newsletters_NewsletterActionType");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterNewsletterActionTypeDisplayName { get; set; }
			public string FilterNewsletterActionTypeType { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterNewsletterActionTypeDisplayName != null)
					filterList.Add("@FilterNewsletterActionTypeDisplayName", FilterNewsletterActionTypeDisplayName);
				if (FilterNewsletterActionTypeType != null)
					filterList.Add("@FilterNewsletterActionTypeType", FilterNewsletterActionTypeType);
				return filterList;
			}
		}
	}
}