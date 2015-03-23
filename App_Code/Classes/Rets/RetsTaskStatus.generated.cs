using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Rets
{
	public partial class RetsTaskStatus : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Rets_RetsTaskStatus_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public RetsTaskStatus()
		{
		}

		public RetsTaskStatus(RetsTaskStatus objectToCopy)
		{
			RetsStatusID = objectToCopy.RetsStatusID;
			RetsTaskID = objectToCopy.RetsTaskID;
			RetsTaskStatusID = objectToCopy.RetsTaskStatusID;
			TaskCompleteTime = objectToCopy.TaskCompleteTime;
		}

		public virtual bool IsNewRecord
		{
			get { return RetsTaskStatusID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime TaskCompleteTimeClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(TaskCompleteTime); }
		}

		public virtual void Save()
		{
			SaveEntity("RetsTaskStatus", this);
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

		public static RetsTaskStatus GetByID(int RetsTaskStatusID, IEnumerable<string> includeList = null)
		{
			RetsTaskStatus obj = null;
			string key = cacheKeyPrefix + RetsTaskStatusID + GetCacheIncludeText(includeList);

			RetsTaskStatus tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as RetsTaskStatus;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<RetsTaskStatus> itemQuery = AddIncludes(entity.RetsTaskStatus, includeList);
					obj = itemQuery.FirstOrDefault(n => n.RetsTaskStatusID == RetsTaskStatusID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<RetsTaskStatus> GetAll(IEnumerable<string> includeList = null)
		{
			List<RetsTaskStatus> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<RetsTaskStatus> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<RetsTaskStatus>();
				tmpList = Cache[key] as List<RetsTaskStatus>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<RetsTaskStatus> itemQuery = AddIncludes(entity.RetsTaskStatus, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<RetsTaskStatus> RetsTaskStatusGetByRetsStatusID(Int32 RetsStatusID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterRetsTaskStatusRetsStatusID = RetsStatusID.ToString();
			return RetsTaskStatusPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<RetsTaskStatus> RetsTaskStatusGetByRetsTaskID(Int32 RetsTaskID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterRetsTaskStatusRetsTaskID = RetsTaskID.ToString();
			return RetsTaskStatusPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<RetsTaskStatus> RetsTaskStatusGetByTaskCompleteTime(DateTime TaskCompleteTime, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterRetsTaskStatusTaskCompleteTime = TaskCompleteTime.ToString();
			return RetsTaskStatusPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<RetsTaskStatus> RetsTaskStatusPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<RetsTaskStatus> objects = RetsTaskStatusPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<RetsTaskStatus> RetsTaskStatusPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return RetsTaskStatusPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<RetsTaskStatus> RetsTaskStatusPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return RetsTaskStatusPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<RetsTaskStatus> RetsTaskStatusPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "RetsTaskStatusID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<RetsTaskStatus> objects;
			string baseKey = cacheKeyPrefix + "RetsTaskStatusPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<RetsTaskStatus> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<RetsTaskStatus>();
				tmpList = Cache[key] as List<RetsTaskStatus>;
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
					IQueryable<RetsTaskStatus> itemQuery = SetupQuery(entity.RetsTaskStatus, "RetsTaskStatus", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Rets_RetsTaskStatus");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterRetsTaskStatusRetsStatusID { get; set; }
			public string FilterRetsTaskStatusRetsTaskID { get; set; }
			public string FilterRetsTaskStatusTaskCompleteTime { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterRetsTaskStatusRetsStatusID != null)
				{
					if (FilterRetsTaskStatusRetsStatusID == string.Empty)
						filterList.Add("@FilterRetsTaskStatusRetsStatusID", string.Empty);
					else
						filterList.Add("@FilterRetsTaskStatusRetsStatusID", Convert.ToInt32(FilterRetsTaskStatusRetsStatusID));
				}
				if (FilterRetsTaskStatusRetsTaskID != null)
				{
					if (FilterRetsTaskStatusRetsTaskID == string.Empty)
						filterList.Add("@FilterRetsTaskStatusRetsTaskID", string.Empty);
					else
						filterList.Add("@FilterRetsTaskStatusRetsTaskID", Convert.ToInt32(FilterRetsTaskStatusRetsTaskID));
				}
				if (FilterRetsTaskStatusTaskCompleteTime != null)
				{
					if (FilterRetsTaskStatusTaskCompleteTime == string.Empty)
						filterList.Add("@FilterRetsTaskStatusTaskCompleteTime", string.Empty);
					else
						filterList.Add("@FilterRetsTaskStatusTaskCompleteTime", Convert.ToDateTime(FilterRetsTaskStatusTaskCompleteTime));
				}
				return filterList;
			}
		}
	}
}