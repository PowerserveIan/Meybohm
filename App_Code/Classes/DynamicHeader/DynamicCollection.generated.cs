using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.DynamicHeader
{
	public partial class DynamicCollection : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "DynamicHeader_DynamicCollection_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public DynamicCollection(DynamicCollection objectToCopy)
		{
			Active = objectToCopy.Active;
			DynamicCollectionID = objectToCopy.DynamicCollectionID;
			Name = objectToCopy.Name;
		}

		public virtual bool IsNewRecord
		{
			get { return DynamicCollectionID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("DynamicCollection", this);
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

		public static DynamicCollection GetByID(int DynamicCollectionID, IEnumerable<string> includeList = null)
		{
			DynamicCollection obj = null;
			string key = cacheKeyPrefix + DynamicCollectionID + GetCacheIncludeText(includeList);

			DynamicCollection tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as DynamicCollection;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<DynamicCollection> itemQuery = AddIncludes(entity.DynamicCollection, includeList);
					obj = itemQuery.FirstOrDefault(n => n.DynamicCollectionID == DynamicCollectionID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<DynamicCollection> GetAll(IEnumerable<string> includeList = null)
		{
			List<DynamicCollection> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<DynamicCollection> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<DynamicCollection>();
				tmpList = Cache[key] as List<DynamicCollection>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<DynamicCollection> itemQuery = AddIncludes(entity.DynamicCollection, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<DynamicCollection> DynamicCollectionGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterDynamicCollectionActive = Active.ToString();
			return DynamicCollectionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<DynamicCollection> DynamicCollectionGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterDynamicCollectionName = Name.ToString();
			return DynamicCollectionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<DynamicCollection> DynamicCollectionPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<DynamicCollection> objects = DynamicCollectionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<DynamicCollection> DynamicCollectionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return DynamicCollectionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<DynamicCollection> DynamicCollectionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return DynamicCollectionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<DynamicCollection> DynamicCollectionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "DynamicCollectionID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<DynamicCollection> objects;
			string baseKey = cacheKeyPrefix + "DynamicCollectionPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<DynamicCollection> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<DynamicCollection>();
				tmpList = Cache[key] as List<DynamicCollection>;
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
					IQueryable<DynamicCollection> itemQuery = SetupQuery(entity.DynamicCollection, "DynamicCollection", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("DynamicHeader_DynamicCollection");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterDynamicCollectionActive { get; set; }
			public string FilterDynamicCollectionName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterDynamicCollectionActive != null)
				{
					if (FilterDynamicCollectionActive == string.Empty)
						filterList.Add("@FilterDynamicCollectionActive", string.Empty);
					else
						filterList.Add("@FilterDynamicCollectionActive", Convert.ToBoolean(FilterDynamicCollectionActive));
				}
				if (FilterDynamicCollectionName != null)
					filterList.Add("@FilterDynamicCollectionName", FilterDynamicCollectionName);
				return filterList;
			}
		}
	}
}