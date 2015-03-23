using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.WhatsNearBy
{
	public partial class WhatsNearByLocationCategory : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "WhatsNearBy_WhatsNearByLocationCategory_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public WhatsNearByLocationCategory()
		{
		}

		public WhatsNearByLocationCategory(WhatsNearByLocationCategory objectToCopy)
		{
			WhatsNearByCategoryID = objectToCopy.WhatsNearByCategoryID;
			WhatsNearByLocationCategoryID = objectToCopy.WhatsNearByLocationCategoryID;
			WhatsNearByLocationID = objectToCopy.WhatsNearByLocationID;
		}

		public virtual bool IsNewRecord
		{
			get { return WhatsNearByLocationCategoryID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("WhatsNearByLocationCategory", this);
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

		public static WhatsNearByLocationCategory GetByID(int WhatsNearByLocationCategoryID, IEnumerable<string> includeList = null)
		{
			WhatsNearByLocationCategory obj = null;
			string key = cacheKeyPrefix + WhatsNearByLocationCategoryID + GetCacheIncludeText(includeList);

			WhatsNearByLocationCategory tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as WhatsNearByLocationCategory;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<WhatsNearByLocationCategory> itemQuery = AddIncludes(entity.WhatsNearByLocationCategory, includeList);
					obj = itemQuery.FirstOrDefault(n => n.WhatsNearByLocationCategoryID == WhatsNearByLocationCategoryID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<WhatsNearByLocationCategory> GetAll(IEnumerable<string> includeList = null)
		{
			List<WhatsNearByLocationCategory> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<WhatsNearByLocationCategory> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<WhatsNearByLocationCategory>();
				tmpList = Cache[key] as List<WhatsNearByLocationCategory>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<WhatsNearByLocationCategory> itemQuery = AddIncludes(entity.WhatsNearByLocationCategory, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<WhatsNearByLocationCategory> WhatsNearByLocationCategoryGetByWhatsNearByCategoryID(Int32 WhatsNearByCategoryID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterWhatsNearByLocationCategoryWhatsNearByCategoryID = WhatsNearByCategoryID.ToString();
			return WhatsNearByLocationCategoryPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<WhatsNearByLocationCategory> WhatsNearByLocationCategoryGetByWhatsNearByLocationID(Int32 WhatsNearByLocationID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterWhatsNearByLocationCategoryWhatsNearByLocationID = WhatsNearByLocationID.ToString();
			return WhatsNearByLocationCategoryPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<WhatsNearByLocationCategory> WhatsNearByLocationCategoryPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<WhatsNearByLocationCategory> objects = WhatsNearByLocationCategoryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<WhatsNearByLocationCategory> WhatsNearByLocationCategoryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return WhatsNearByLocationCategoryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<WhatsNearByLocationCategory> WhatsNearByLocationCategoryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return WhatsNearByLocationCategoryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<WhatsNearByLocationCategory> WhatsNearByLocationCategoryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "WhatsNearByLocationCategoryID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<WhatsNearByLocationCategory> objects;
			string baseKey = cacheKeyPrefix + "WhatsNearByLocationCategoryPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<WhatsNearByLocationCategory> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<WhatsNearByLocationCategory>();
				tmpList = Cache[key] as List<WhatsNearByLocationCategory>;
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
					IQueryable<WhatsNearByLocationCategory> itemQuery = SetupQuery(entity.WhatsNearByLocationCategory, "WhatsNearByLocationCategory", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("WhatsNearBy_WhatsNearByLocationCategory");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterWhatsNearByLocationCategoryWhatsNearByCategoryID { get; set; }
			public string FilterWhatsNearByLocationCategoryWhatsNearByLocationID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterWhatsNearByLocationCategoryWhatsNearByCategoryID != null)
				{
					if (FilterWhatsNearByLocationCategoryWhatsNearByCategoryID == string.Empty)
						filterList.Add("@FilterWhatsNearByLocationCategoryWhatsNearByCategoryID", string.Empty);
					else
						filterList.Add("@FilterWhatsNearByLocationCategoryWhatsNearByCategoryID", Convert.ToInt32(FilterWhatsNearByLocationCategoryWhatsNearByCategoryID));
				}
				if (FilterWhatsNearByLocationCategoryWhatsNearByLocationID != null)
				{
					if (FilterWhatsNearByLocationCategoryWhatsNearByLocationID == string.Empty)
						filterList.Add("@FilterWhatsNearByLocationCategoryWhatsNearByLocationID", string.Empty);
					else
						filterList.Add("@FilterWhatsNearByLocationCategoryWhatsNearByLocationID", Convert.ToInt32(FilterWhatsNearByLocationCategoryWhatsNearByLocationID));
				}
				return filterList;
			}
		}
	}
}