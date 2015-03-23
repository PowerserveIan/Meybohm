using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Newsletters
{
	public partial class NewsletterFormat : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Newsletters_NewsletterFormat_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public NewsletterFormat(NewsletterFormat objectToCopy)
		{
			Name = objectToCopy.Name;
			NewsletterFormatID = objectToCopy.NewsletterFormatID;
		}

		public virtual bool IsNewRecord
		{
			get { return NewsletterFormatID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("NewsletterFormat", this);
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

		public static NewsletterFormat GetByID(int NewsletterFormatID, IEnumerable<string> includeList = null)
		{
			NewsletterFormat obj = null;
			string key = cacheKeyPrefix + NewsletterFormatID + GetCacheIncludeText(includeList);

			NewsletterFormat tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as NewsletterFormat;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsletterFormat> itemQuery = AddIncludes(entity.NewsletterFormat, includeList);
					obj = itemQuery.FirstOrDefault(n => n.NewsletterFormatID == NewsletterFormatID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<NewsletterFormat> GetAll(IEnumerable<string> includeList = null)
		{
			List<NewsletterFormat> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<NewsletterFormat> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsletterFormat>();
				tmpList = Cache[key] as List<NewsletterFormat>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsletterFormat> itemQuery = AddIncludes(entity.NewsletterFormat, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<NewsletterFormat> NewsletterFormatGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterFormatName = Name.ToString();
			return NewsletterFormatPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<NewsletterFormat> NewsletterFormatPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<NewsletterFormat> objects = NewsletterFormatPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<NewsletterFormat> NewsletterFormatPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return NewsletterFormatPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<NewsletterFormat> NewsletterFormatPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return NewsletterFormatPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<NewsletterFormat> NewsletterFormatPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "NewsletterFormatID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<NewsletterFormat> objects;
			string baseKey = cacheKeyPrefix + "NewsletterFormatPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<NewsletterFormat> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsletterFormat>();
				tmpList = Cache[key] as List<NewsletterFormat>;
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
					IQueryable<NewsletterFormat> itemQuery = SetupQuery(entity.NewsletterFormat, "NewsletterFormat", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Newsletters_NewsletterFormat");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterNewsletterFormatName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterNewsletterFormatName != null)
					filterList.Add("@FilterNewsletterFormatName", FilterNewsletterFormatName);
				return filterList;
			}
		}
	}
}