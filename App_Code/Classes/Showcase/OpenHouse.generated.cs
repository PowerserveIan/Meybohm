using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class OpenHouse : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_OpenHouse_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public OpenHouse()
		{
		}

		public OpenHouse(OpenHouse objectToCopy)
		{
			BeginDate = objectToCopy.BeginDate;
			EndDate = objectToCopy.EndDate;
			OpenHouseID = objectToCopy.OpenHouseID;
			ShowcaseItemID = objectToCopy.ShowcaseItemID;
		}

		public virtual bool IsNewRecord
		{
			get { return OpenHouseID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime BeginDateClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(BeginDate); }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime? EndDateClientTime
		{
			get 
			{
				if (EndDate.HasValue)
					return Helpers.ConvertUTCToClientTime(EndDate.Value);
				return null;
			}
		}

		public virtual void Save()
		{
			SaveEntity("OpenHouse", this);
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

		public static OpenHouse GetByID(int OpenHouseID, IEnumerable<string> includeList = null)
		{
			OpenHouse obj = null;
			string key = cacheKeyPrefix + OpenHouseID + GetCacheIncludeText(includeList);

			OpenHouse tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as OpenHouse;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<OpenHouse> itemQuery = AddIncludes(entity.OpenHouse, includeList);
					obj = itemQuery.FirstOrDefault(n => n.OpenHouseID == OpenHouseID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<OpenHouse> GetAll(IEnumerable<string> includeList = null)
		{
			List<OpenHouse> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<OpenHouse> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<OpenHouse>();
				tmpList = Cache[key] as List<OpenHouse>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<OpenHouse> itemQuery = AddIncludes(entity.OpenHouse, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<OpenHouse> OpenHouseGetByShowcaseItemID(Int32 ShowcaseItemID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterOpenHouseShowcaseItemID = ShowcaseItemID.ToString();
			return OpenHousePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<OpenHouse> OpenHousePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<OpenHouse> objects = OpenHousePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<OpenHouse> OpenHousePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return OpenHousePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<OpenHouse> OpenHousePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return OpenHousePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<OpenHouse> OpenHousePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "OpenHouseID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<OpenHouse> objects;
			string baseKey = cacheKeyPrefix + "OpenHousePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<OpenHouse> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<OpenHouse>();
				tmpList = Cache[key] as List<OpenHouse>;
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
					IQueryable<OpenHouse> itemQuery = SetupQuery(entity.OpenHouse, "OpenHouse", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_OpenHouse");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterOpenHouseShowcaseItemID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterOpenHouseShowcaseItemID != null)
				{
					if (FilterOpenHouseShowcaseItemID == string.Empty)
						filterList.Add("@FilterOpenHouseShowcaseItemID", string.Empty);
					else
						filterList.Add("@FilterOpenHouseShowcaseItemID", Convert.ToInt32(FilterOpenHouseShowcaseItemID));
				}
				return filterList;
			}
		}
	}
}