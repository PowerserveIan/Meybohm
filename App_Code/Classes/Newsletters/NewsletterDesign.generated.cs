using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Newsletters
{
	public partial class NewsletterDesign : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Newsletters_NewsletterDesign_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public NewsletterDesign(NewsletterDesign objectToCopy)
		{
			Name = objectToCopy.Name;
			NewsletterDesignID = objectToCopy.NewsletterDesignID;
			Path = objectToCopy.Path;
			Template = objectToCopy.Template;
		}

		public virtual bool IsNewRecord
		{
			get { return NewsletterDesignID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("NewsletterDesign", this);
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

		public static NewsletterDesign GetByID(int NewsletterDesignID, IEnumerable<string> includeList = null)
		{
			NewsletterDesign obj = null;
			string key = cacheKeyPrefix + NewsletterDesignID + GetCacheIncludeText(includeList);

			NewsletterDesign tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as NewsletterDesign;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsletterDesign> itemQuery = AddIncludes(entity.NewsletterDesign, includeList);
					obj = itemQuery.FirstOrDefault(n => n.NewsletterDesignID == NewsletterDesignID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<NewsletterDesign> GetAll(IEnumerable<string> includeList = null)
		{
			List<NewsletterDesign> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<NewsletterDesign> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsletterDesign>();
				tmpList = Cache[key] as List<NewsletterDesign>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsletterDesign> itemQuery = AddIncludes(entity.NewsletterDesign, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<NewsletterDesign> NewsletterDesignGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterDesignName = Name.ToString();
			return NewsletterDesignPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<NewsletterDesign> NewsletterDesignPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<NewsletterDesign> objects = NewsletterDesignPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<NewsletterDesign> NewsletterDesignPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return NewsletterDesignPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<NewsletterDesign> NewsletterDesignPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return NewsletterDesignPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<NewsletterDesign> NewsletterDesignPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "NewsletterDesignID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<NewsletterDesign> objects;
			string baseKey = cacheKeyPrefix + "NewsletterDesignPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<NewsletterDesign> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsletterDesign>();
				tmpList = Cache[key] as List<NewsletterDesign>;
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
					IQueryable<NewsletterDesign> itemQuery = SetupQuery(entity.NewsletterDesign, "NewsletterDesign", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Newsletters_NewsletterDesign");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterNewsletterDesignName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterNewsletterDesignName != null)
					filterList.Add("@FilterNewsletterDesignName", FilterNewsletterDesignName);
				return filterList;
			}
		}
	}
}