using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.WhatsNearBy
{
	public partial class WhatsNearByCategory : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "WhatsNearBy_WhatsNearByCategory_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public WhatsNearByCategory(WhatsNearByCategory objectToCopy)
		{
			Active = objectToCopy.Active;
			Name = objectToCopy.Name;
			PlaceholderImage = objectToCopy.PlaceholderImage;
			WhatsNearByCategoryID = objectToCopy.WhatsNearByCategoryID;
		}

		public virtual bool IsNewRecord
		{
			get { return WhatsNearByCategoryID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("WhatsNearByCategory", this);
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

		public static WhatsNearByCategory GetByID(int WhatsNearByCategoryID, IEnumerable<string> includeList = null)
		{
			WhatsNearByCategory obj = null;
			string key = cacheKeyPrefix + WhatsNearByCategoryID + GetCacheIncludeText(includeList);

			WhatsNearByCategory tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as WhatsNearByCategory;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<WhatsNearByCategory> itemQuery = AddIncludes(entity.WhatsNearByCategory, includeList);
					obj = itemQuery.FirstOrDefault(n => n.WhatsNearByCategoryID == WhatsNearByCategoryID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<WhatsNearByCategory> GetAll(IEnumerable<string> includeList = null)
		{
			List<WhatsNearByCategory> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<WhatsNearByCategory> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<WhatsNearByCategory>();
				tmpList = Cache[key] as List<WhatsNearByCategory>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<WhatsNearByCategory> itemQuery = AddIncludes(entity.WhatsNearByCategory, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<WhatsNearByCategory> WhatsNearByCategoryGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterWhatsNearByCategoryActive = Active.ToString();
			return WhatsNearByCategoryPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<WhatsNearByCategory> WhatsNearByCategoryGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterWhatsNearByCategoryName = Name.ToString();
			return WhatsNearByCategoryPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<WhatsNearByCategory> WhatsNearByCategoryPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<WhatsNearByCategory> objects = WhatsNearByCategoryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<WhatsNearByCategory> WhatsNearByCategoryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return WhatsNearByCategoryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<WhatsNearByCategory> WhatsNearByCategoryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return WhatsNearByCategoryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<WhatsNearByCategory> WhatsNearByCategoryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "WhatsNearByCategoryID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<WhatsNearByCategory> objects;
			string baseKey = cacheKeyPrefix + "WhatsNearByCategoryPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<WhatsNearByCategory> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<WhatsNearByCategory>();
				tmpList = Cache[key] as List<WhatsNearByCategory>;
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
					IQueryable<WhatsNearByCategory> itemQuery = SetupQuery(entity.WhatsNearByCategory, "WhatsNearByCategory", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("WhatsNearBy_WhatsNearByCategory");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterWhatsNearByCategoryActive { get; set; }
			public string FilterWhatsNearByCategoryName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterWhatsNearByCategoryActive != null)
				{
					if (FilterWhatsNearByCategoryActive == string.Empty)
						filterList.Add("@FilterWhatsNearByCategoryActive", string.Empty);
					else
						filterList.Add("@FilterWhatsNearByCategoryActive", Convert.ToBoolean(FilterWhatsNearByCategoryActive));
				}
				if (FilterWhatsNearByCategoryName != null)
					filterList.Add("@FilterWhatsNearByCategoryName", FilterWhatsNearByCategoryName);
				return filterList;
			}
		}
	}
}