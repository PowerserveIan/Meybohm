using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Rets
{
	public partial class RetsStatus : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Rets_RetsStatus_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public RetsStatus(RetsStatus objectToCopy)
		{
			Name = objectToCopy.Name;
			RetsStatusID = objectToCopy.RetsStatusID;
		}

		public virtual bool IsNewRecord
		{
			get { return RetsStatusID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("RetsStatus", this);
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

		public static RetsStatus GetByID(int RetsStatusID, IEnumerable<string> includeList = null)
		{
			RetsStatus obj = null;
			string key = cacheKeyPrefix + RetsStatusID + GetCacheIncludeText(includeList);

			RetsStatus tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as RetsStatus;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<RetsStatus> itemQuery = AddIncludes(entity.RetsStatus, includeList);
					obj = itemQuery.FirstOrDefault(n => n.RetsStatusID == RetsStatusID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<RetsStatus> GetAll(IEnumerable<string> includeList = null)
		{
			List<RetsStatus> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<RetsStatus> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<RetsStatus>();
				tmpList = Cache[key] as List<RetsStatus>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<RetsStatus> itemQuery = AddIncludes(entity.RetsStatus, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<RetsStatus> RetsStatusGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterRetsStatusName = Name.ToString();
			return RetsStatusPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<RetsStatus> RetsStatusPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<RetsStatus> objects = RetsStatusPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<RetsStatus> RetsStatusPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return RetsStatusPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<RetsStatus> RetsStatusPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return RetsStatusPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<RetsStatus> RetsStatusPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "RetsStatusID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<RetsStatus> objects;
			string baseKey = cacheKeyPrefix + "RetsStatusPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<RetsStatus> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<RetsStatus>();
				tmpList = Cache[key] as List<RetsStatus>;
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
					IQueryable<RetsStatus> itemQuery = SetupQuery(entity.RetsStatus, "RetsStatus", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Rets_RetsStatus");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterRetsStatusName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterRetsStatusName != null)
					filterList.Add("@FilterRetsStatusName", FilterRetsStatusName);
				return filterList;
			}
		}
	}
}