using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Rets
{
	public partial class RetsTask : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Rets_RetsTask_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "TaskName"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public RetsTask(RetsTask objectToCopy)
		{
			ParentRetsTaskID = objectToCopy.ParentRetsTaskID;
			RetsTaskID = objectToCopy.RetsTaskID;
			TaskName = objectToCopy.TaskName;
		}

		public virtual bool IsNewRecord
		{
			get { return RetsTaskID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("RetsTask", this);
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

		public static RetsTask GetByID(int RetsTaskID, IEnumerable<string> includeList = null)
		{
			RetsTask obj = null;
			string key = cacheKeyPrefix + RetsTaskID + GetCacheIncludeText(includeList);

			RetsTask tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as RetsTask;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<RetsTask> itemQuery = AddIncludes(entity.RetsTask, includeList);
					obj = itemQuery.FirstOrDefault(n => n.RetsTaskID == RetsTaskID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<RetsTask> GetAll(IEnumerable<string> includeList = null)
		{
			List<RetsTask> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<RetsTask> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<RetsTask>();
				tmpList = Cache[key] as List<RetsTask>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<RetsTask> itemQuery = AddIncludes(entity.RetsTask, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<RetsTask> RetsTaskGetByParentRetsTaskID(Int32? ParentRetsTaskID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterRetsTaskParentRetsTaskID = ParentRetsTaskID.ToString();
			return RetsTaskPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<RetsTask> RetsTaskGetByTaskName(String TaskName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterRetsTaskTaskName = TaskName.ToString();
			return RetsTaskPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<RetsTask> RetsTaskPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<RetsTask> objects = RetsTaskPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<RetsTask> RetsTaskPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return RetsTaskPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<RetsTask> RetsTaskPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return RetsTaskPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<RetsTask> RetsTaskPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "RetsTaskID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<RetsTask> objects;
			string baseKey = cacheKeyPrefix + "RetsTaskPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<RetsTask> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<RetsTask>();
				tmpList = Cache[key] as List<RetsTask>;
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
					IQueryable<RetsTask> itemQuery = SetupQuery(entity.RetsTask, "RetsTask", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Rets_RetsTask");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterRetsTaskParentRetsTaskID { get; set; }
			public string FilterRetsTaskTaskName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterRetsTaskParentRetsTaskID != null)
				{
					if (FilterRetsTaskParentRetsTaskID == string.Empty)
						filterList.Add("@FilterRetsTaskParentRetsTaskID", string.Empty);
					else
						filterList.Add("@FilterRetsTaskParentRetsTaskID", Convert.ToInt32(FilterRetsTaskParentRetsTaskID));
				}
				if (FilterRetsTaskTaskName != null)
					filterList.Add("@FilterRetsTaskTaskName", FilterRetsTaskTaskName);
				return filterList;
			}
		}
	}
}