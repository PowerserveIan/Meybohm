using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class ShowcaseItemMetric : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_ShowcaseItemMetric_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "SessionID"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public ShowcaseItemMetric()
		{
		}

		public ShowcaseItemMetric(ShowcaseItemMetric objectToCopy)
		{
			ClickTypeID = objectToCopy.ClickTypeID;
			Date = objectToCopy.Date;
			SessionID = objectToCopy.SessionID;
			ShowcaseItemID = objectToCopy.ShowcaseItemID;
			ShowcaseItemMetricID = objectToCopy.ShowcaseItemMetricID;
			UserID = objectToCopy.UserID;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseItemMetricID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime DateClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(Date); }
		}

		public virtual void Save()
		{
			SaveEntity("ShowcaseItemMetric", this);
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

		public static ShowcaseItemMetric GetByID(int ShowcaseItemMetricID, IEnumerable<string> includeList = null)
		{
			ShowcaseItemMetric obj = null;
			string key = cacheKeyPrefix + ShowcaseItemMetricID + GetCacheIncludeText(includeList);

			ShowcaseItemMetric tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as ShowcaseItemMetric;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItemMetric> itemQuery = AddIncludes(entity.ShowcaseItemMetric, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseItemMetricID == ShowcaseItemMetricID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<ShowcaseItemMetric> GetAll(IEnumerable<string> includeList = null)
		{
			List<ShowcaseItemMetric> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<ShowcaseItemMetric> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseItemMetric>();
				tmpList = Cache[key] as List<ShowcaseItemMetric>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItemMetric> itemQuery = AddIncludes(entity.ShowcaseItemMetric, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<ShowcaseItemMetric> ShowcaseItemMetricGetByClickTypeID(Int32 ClickTypeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemMetricClickTypeID = ClickTypeID.ToString();
			return ShowcaseItemMetricPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItemMetric> ShowcaseItemMetricGetByDate(DateTime Date, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemMetricDate = Date.ToString();
			return ShowcaseItemMetricPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItemMetric> ShowcaseItemMetricGetBySessionID(String SessionID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemMetricSessionID = SessionID.ToString();
			return ShowcaseItemMetricPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItemMetric> ShowcaseItemMetricGetByShowcaseItemID(Int32 ShowcaseItemID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemMetricShowcaseItemID = ShowcaseItemID.ToString();
			return ShowcaseItemMetricPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItemMetric> ShowcaseItemMetricGetByUserID(Int32? UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemMetricUserID = UserID.ToString();
			return ShowcaseItemMetricPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<ShowcaseItemMetric> ShowcaseItemMetricPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<ShowcaseItemMetric> objects = ShowcaseItemMetricPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseItemMetric> ShowcaseItemMetricPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return ShowcaseItemMetricPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<ShowcaseItemMetric> ShowcaseItemMetricPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return ShowcaseItemMetricPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<ShowcaseItemMetric> ShowcaseItemMetricPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseItemMetricID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<ShowcaseItemMetric> objects;
			string baseKey = cacheKeyPrefix + "ShowcaseItemMetricPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseItemMetric> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseItemMetric>();
				tmpList = Cache[key] as List<ShowcaseItemMetric>;
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
					IQueryable<ShowcaseItemMetric> itemQuery = SetupQuery(entity.ShowcaseItemMetric, "ShowcaseItemMetric", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_ShowcaseItemMetric");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterShowcaseItemMetricClickTypeID { get; set; }
			public string FilterShowcaseItemMetricDate { get; set; }
			public string FilterShowcaseItemMetricSessionID { get; set; }
			public string FilterShowcaseItemMetricShowcaseItemID { get; set; }
			public string FilterShowcaseItemMetricUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterShowcaseItemMetricClickTypeID != null)
				{
					if (FilterShowcaseItemMetricClickTypeID == string.Empty)
						filterList.Add("@FilterShowcaseItemMetricClickTypeID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemMetricClickTypeID", Convert.ToInt32(FilterShowcaseItemMetricClickTypeID));
				}
				if (FilterShowcaseItemMetricDate != null)
				{
					if (FilterShowcaseItemMetricDate == string.Empty)
						filterList.Add("@FilterShowcaseItemMetricDate", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemMetricDate", Convert.ToDateTime(FilterShowcaseItemMetricDate));
				}
				if (FilterShowcaseItemMetricSessionID != null)
					filterList.Add("@FilterShowcaseItemMetricSessionID", FilterShowcaseItemMetricSessionID);
				if (FilterShowcaseItemMetricShowcaseItemID != null)
				{
					if (FilterShowcaseItemMetricShowcaseItemID == string.Empty)
						filterList.Add("@FilterShowcaseItemMetricShowcaseItemID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemMetricShowcaseItemID", Convert.ToInt32(FilterShowcaseItemMetricShowcaseItemID));
				}
				if (FilterShowcaseItemMetricUserID != null)
				{
					if (FilterShowcaseItemMetricUserID == string.Empty)
						filterList.Add("@FilterShowcaseItemMetricUserID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemMetricUserID", Convert.ToInt32(FilterShowcaseItemMetricUserID));
				}
				return filterList;
			}
		}
	}
}