using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Rets
{
	public partial class RetsUpdatePageTracker : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Rets_RetsUpdatePageTracker_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public RetsUpdatePageTracker()
		{
		}

		public RetsUpdatePageTracker(RetsUpdatePageTracker objectToCopy)
		{
			CurrentRunStartTime = objectToCopy.CurrentRunStartTime;
			RetsUpdatePageTrackerID = objectToCopy.RetsUpdatePageTrackerID;
			RunCompleted = objectToCopy.RunCompleted;
		}

		public virtual bool IsNewRecord
		{
			get { return RetsUpdatePageTrackerID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime CurrentRunStartTimeClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(CurrentRunStartTime); }
		}

		public virtual void Save()
		{
			SaveEntity("RetsUpdatePageTracker", this);
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

		public static RetsUpdatePageTracker GetByID(int RetsUpdatePageTrackerID, IEnumerable<string> includeList = null)
		{
			RetsUpdatePageTracker obj = null;
			string key = cacheKeyPrefix + RetsUpdatePageTrackerID + GetCacheIncludeText(includeList);

			RetsUpdatePageTracker tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as RetsUpdatePageTracker;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<RetsUpdatePageTracker> itemQuery = AddIncludes(entity.RetsUpdatePageTracker, includeList);
					obj = itemQuery.FirstOrDefault(n => n.RetsUpdatePageTrackerID == RetsUpdatePageTrackerID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<RetsUpdatePageTracker> GetAll(IEnumerable<string> includeList = null)
		{
			List<RetsUpdatePageTracker> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<RetsUpdatePageTracker> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<RetsUpdatePageTracker>();
				tmpList = Cache[key] as List<RetsUpdatePageTracker>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<RetsUpdatePageTracker> itemQuery = AddIncludes(entity.RetsUpdatePageTracker, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<RetsUpdatePageTracker> RetsUpdatePageTrackerGetByRunCompleted(Boolean RunCompleted, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterRetsUpdatePageTrackerRunCompleted = RunCompleted.ToString();
			return RetsUpdatePageTrackerPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<RetsUpdatePageTracker> RetsUpdatePageTrackerPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<RetsUpdatePageTracker> objects = RetsUpdatePageTrackerPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<RetsUpdatePageTracker> RetsUpdatePageTrackerPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return RetsUpdatePageTrackerPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<RetsUpdatePageTracker> RetsUpdatePageTrackerPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return RetsUpdatePageTrackerPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<RetsUpdatePageTracker> RetsUpdatePageTrackerPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "RetsUpdatePageTrackerID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<RetsUpdatePageTracker> objects;
			string baseKey = cacheKeyPrefix + "RetsUpdatePageTrackerPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<RetsUpdatePageTracker> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<RetsUpdatePageTracker>();
				tmpList = Cache[key] as List<RetsUpdatePageTracker>;
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
					IQueryable<RetsUpdatePageTracker> itemQuery = SetupQuery(entity.RetsUpdatePageTracker, "RetsUpdatePageTracker", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Rets_RetsUpdatePageTracker");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterRetsUpdatePageTrackerRunCompleted { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterRetsUpdatePageTrackerRunCompleted != null)
				{
					if (FilterRetsUpdatePageTrackerRunCompleted == string.Empty)
						filterList.Add("@FilterRetsUpdatePageTrackerRunCompleted", string.Empty);
					else
						filterList.Add("@FilterRetsUpdatePageTrackerRunCompleted", Convert.ToBoolean(FilterRetsUpdatePageTrackerRunCompleted));
				}
				return filterList;
			}
		}
	}
}