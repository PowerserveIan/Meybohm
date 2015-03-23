using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.DynamicHeader
{
	public partial class DynamicImageCollection : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "DynamicHeader_DynamicImageCollection_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public DynamicImageCollection()
		{
		}

		public DynamicImageCollection(DynamicImageCollection objectToCopy)
		{
			DisplayOrder = objectToCopy.DisplayOrder;
			DynamicCollectionID = objectToCopy.DynamicCollectionID;
			DynamicImageCollectionID = objectToCopy.DynamicImageCollectionID;
			DynamicImageID = objectToCopy.DynamicImageID;
		}

		public virtual bool IsNewRecord
		{
			get { return DynamicImageCollectionID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("DynamicImageCollection", this);
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

		public static DynamicImageCollection GetByID(int DynamicImageCollectionID, IEnumerable<string> includeList = null)
		{
			DynamicImageCollection obj = null;
			string key = cacheKeyPrefix + DynamicImageCollectionID + GetCacheIncludeText(includeList);

			DynamicImageCollection tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as DynamicImageCollection;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<DynamicImageCollection> itemQuery = AddIncludes(entity.DynamicImageCollection, includeList);
					obj = itemQuery.FirstOrDefault(n => n.DynamicImageCollectionID == DynamicImageCollectionID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<DynamicImageCollection> GetAll(IEnumerable<string> includeList = null)
		{
			List<DynamicImageCollection> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<DynamicImageCollection> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<DynamicImageCollection>();
				tmpList = Cache[key] as List<DynamicImageCollection>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<DynamicImageCollection> itemQuery = AddIncludes(entity.DynamicImageCollection, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<DynamicImageCollection> DynamicImageCollectionGetByDisplayOrder(Int16 DisplayOrder, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterDynamicImageCollectionDisplayOrder = DisplayOrder.ToString();
			return DynamicImageCollectionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<DynamicImageCollection> DynamicImageCollectionGetByDynamicCollectionID(Int32 DynamicCollectionID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterDynamicImageCollectionDynamicCollectionID = DynamicCollectionID.ToString();
			return DynamicImageCollectionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<DynamicImageCollection> DynamicImageCollectionGetByDynamicImageID(Int32 DynamicImageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterDynamicImageCollectionDynamicImageID = DynamicImageID.ToString();
			return DynamicImageCollectionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<DynamicImageCollection> DynamicImageCollectionPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<DynamicImageCollection> objects = DynamicImageCollectionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<DynamicImageCollection> DynamicImageCollectionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return DynamicImageCollectionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<DynamicImageCollection> DynamicImageCollectionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return DynamicImageCollectionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<DynamicImageCollection> DynamicImageCollectionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "DynamicImageCollectionID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<DynamicImageCollection> objects;
			string baseKey = cacheKeyPrefix + "DynamicImageCollectionPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<DynamicImageCollection> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<DynamicImageCollection>();
				tmpList = Cache[key] as List<DynamicImageCollection>;
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
					IQueryable<DynamicImageCollection> itemQuery = SetupQuery(entity.DynamicImageCollection, "DynamicImageCollection", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("DynamicHeader_DynamicImageCollection");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterDynamicImageCollectionDisplayOrder { get; set; }
			public string FilterDynamicImageCollectionDynamicCollectionID { get; set; }
			public string FilterDynamicImageCollectionDynamicImageID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterDynamicImageCollectionDisplayOrder != null)
				{
					if (FilterDynamicImageCollectionDisplayOrder == string.Empty)
						filterList.Add("@FilterDynamicImageCollectionDisplayOrder", string.Empty);
					else
						filterList.Add("@FilterDynamicImageCollectionDisplayOrder", Convert.ToInt16(FilterDynamicImageCollectionDisplayOrder));
				}
				if (FilterDynamicImageCollectionDynamicCollectionID != null)
				{
					if (FilterDynamicImageCollectionDynamicCollectionID == string.Empty)
						filterList.Add("@FilterDynamicImageCollectionDynamicCollectionID", string.Empty);
					else
						filterList.Add("@FilterDynamicImageCollectionDynamicCollectionID", Convert.ToInt32(FilterDynamicImageCollectionDynamicCollectionID));
				}
				if (FilterDynamicImageCollectionDynamicImageID != null)
				{
					if (FilterDynamicImageCollectionDynamicImageID == string.Empty)
						filterList.Add("@FilterDynamicImageCollectionDynamicImageID", string.Empty);
					else
						filterList.Add("@FilterDynamicImageCollectionDynamicImageID", Convert.ToInt32(FilterDynamicImageCollectionDynamicImageID));
				}
				return filterList;
			}
		}
	}
}