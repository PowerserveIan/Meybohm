using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class ClickType : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_ClickType_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public ClickType(ClickType objectToCopy)
		{
			ClickTypeID = objectToCopy.ClickTypeID;
			Name = objectToCopy.Name;
		}

		public virtual bool IsNewRecord
		{
			get { return ClickTypeID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("ClickType", this);
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

		public static ClickType GetByID(int ClickTypeID, IEnumerable<string> includeList = null)
		{
			ClickType obj = null;
			string key = cacheKeyPrefix + ClickTypeID + GetCacheIncludeText(includeList);

			ClickType tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as ClickType;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ClickType> itemQuery = AddIncludes(entity.ClickType, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ClickTypeID == ClickTypeID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<ClickType> GetAll(IEnumerable<string> includeList = null)
		{
			List<ClickType> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<ClickType> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ClickType>();
				tmpList = Cache[key] as List<ClickType>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ClickType> itemQuery = AddIncludes(entity.ClickType, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
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

		public static List<ClickType> ClickTypePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<ClickType> objects = ClickTypePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ClickType> ClickTypePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return ClickTypePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<ClickType> ClickTypePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return ClickTypePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<ClickType> ClickTypePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ClickTypeID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<ClickType> objects;
			string baseKey = cacheKeyPrefix + "ClickTypePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ClickType> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ClickType>();
				tmpList = Cache[key] as List<ClickType>;
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
					IQueryable<ClickType> itemQuery = SetupQuery(entity.ClickType, "ClickType", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_ClickType");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				return filterList;
			}
		}
	}
}