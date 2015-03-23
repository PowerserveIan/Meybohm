using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class ShowcaseItemMetricHistorical : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_ShowcaseItemMetricHistorical_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public ShowcaseItemMetricHistorical()
		{
		}

		public ShowcaseItemMetricHistorical(ShowcaseItemMetricHistorical objectToCopy)
		{
			ClickTypeID = objectToCopy.ClickTypeID;
			Date = objectToCopy.Date;
			ShowcaseItemID = objectToCopy.ShowcaseItemID;
			ShowcaseItemMetricHistoricalID = objectToCopy.ShowcaseItemMetricHistoricalID;
			TotalCount = objectToCopy.TotalCount;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseItemMetricHistoricalID < 1; }
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
			SaveEntity("ShowcaseItemMetricHistorical", this);
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

		public static ShowcaseItemMetricHistorical GetByID(int ShowcaseItemMetricHistoricalID, IEnumerable<string> includeList = null)
		{
			ShowcaseItemMetricHistorical obj = null;
			string key = cacheKeyPrefix + ShowcaseItemMetricHistoricalID + GetCacheIncludeText(includeList);

			ShowcaseItemMetricHistorical tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as ShowcaseItemMetricHistorical;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItemMetricHistorical> itemQuery = AddIncludes(entity.ShowcaseItemMetricHistorical, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseItemMetricHistoricalID == ShowcaseItemMetricHistoricalID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<ShowcaseItemMetricHistorical> GetAll(IEnumerable<string> includeList = null)
		{
			List<ShowcaseItemMetricHistorical> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<ShowcaseItemMetricHistorical> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseItemMetricHistorical>();
				tmpList = Cache[key] as List<ShowcaseItemMetricHistorical>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItemMetricHistorical> itemQuery = AddIncludes(entity.ShowcaseItemMetricHistorical, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<ShowcaseItemMetricHistorical> ShowcaseItemMetricHistoricalGetByClickTypeID(Int32 ClickTypeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemMetricHistoricalClickTypeID = ClickTypeID.ToString();
			return ShowcaseItemMetricHistoricalPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItemMetricHistorical> ShowcaseItemMetricHistoricalGetByShowcaseItemID(Int32 ShowcaseItemID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemMetricHistoricalShowcaseItemID = ShowcaseItemID.ToString();
			return ShowcaseItemMetricHistoricalPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<ShowcaseItemMetricHistorical> ShowcaseItemMetricHistoricalPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<ShowcaseItemMetricHistorical> objects = ShowcaseItemMetricHistoricalPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseItemMetricHistorical> ShowcaseItemMetricHistoricalPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return ShowcaseItemMetricHistoricalPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<ShowcaseItemMetricHistorical> ShowcaseItemMetricHistoricalPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return ShowcaseItemMetricHistoricalPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<ShowcaseItemMetricHistorical> ShowcaseItemMetricHistoricalPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseItemMetricHistoricalID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<ShowcaseItemMetricHistorical> objects;
			string baseKey = cacheKeyPrefix + "ShowcaseItemMetricHistoricalPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseItemMetricHistorical> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseItemMetricHistorical>();
				tmpList = Cache[key] as List<ShowcaseItemMetricHistorical>;
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
					IQueryable<ShowcaseItemMetricHistorical> itemQuery = SetupQuery(entity.ShowcaseItemMetricHistorical, "ShowcaseItemMetricHistorical", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_ShowcaseItemMetricHistorical");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterShowcaseItemMetricHistoricalClickTypeID { get; set; }
			public string FilterShowcaseItemMetricHistoricalShowcaseItemID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterShowcaseItemMetricHistoricalClickTypeID != null)
				{
					if (FilterShowcaseItemMetricHistoricalClickTypeID == string.Empty)
						filterList.Add("@FilterShowcaseItemMetricHistoricalClickTypeID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemMetricHistoricalClickTypeID", Convert.ToInt32(FilterShowcaseItemMetricHistoricalClickTypeID));
				}
				if (FilterShowcaseItemMetricHistoricalShowcaseItemID != null)
				{
					if (FilterShowcaseItemMetricHistoricalShowcaseItemID == string.Empty)
						filterList.Add("@FilterShowcaseItemMetricHistoricalShowcaseItemID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemMetricHistoricalShowcaseItemID", Convert.ToInt32(FilterShowcaseItemMetricHistoricalShowcaseItemID));
				}
				return filterList;
			}
		}
	}
}