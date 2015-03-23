using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ConfigurationSettings
{
	public partial class SiteComponent : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ConfigurationSettings_SiteComponent_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "ComponentName", "DisplayName"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public SiteComponent(SiteComponent objectToCopy)
		{
			ComponentName = objectToCopy.ComponentName;
			DisplayName = objectToCopy.DisplayName;
			MenuDisplayOrder = objectToCopy.MenuDisplayOrder;
			SiteComponentID = objectToCopy.SiteComponentID;
		}

		public virtual bool IsNewRecord
		{
			get { return SiteComponentID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("SiteComponent", this);
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

		public static SiteComponent GetByID(int SiteComponentID, IEnumerable<string> includeList = null)
		{
			SiteComponent obj = null;
			string key = cacheKeyPrefix + SiteComponentID + GetCacheIncludeText(includeList);

			SiteComponent tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as SiteComponent;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SiteComponent> itemQuery = AddIncludes(entity.SiteComponent, includeList);
					obj = itemQuery.FirstOrDefault(n => n.SiteComponentID == SiteComponentID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<SiteComponent> GetAll(IEnumerable<string> includeList = null)
		{
			List<SiteComponent> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<SiteComponent> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SiteComponent>();
				tmpList = Cache[key] as List<SiteComponent>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SiteComponent> itemQuery = AddIncludes(entity.SiteComponent, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<SiteComponent> SiteComponentGetByComponentName(String ComponentName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSiteComponentComponentName = ComponentName.ToString();
			return SiteComponentPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SiteComponent> SiteComponentGetByDisplayName(String DisplayName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSiteComponentDisplayName = DisplayName.ToString();
			return SiteComponentPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<SiteComponent> SiteComponentPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<SiteComponent> objects = SiteComponentPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<SiteComponent> SiteComponentPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return SiteComponentPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<SiteComponent> SiteComponentPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return SiteComponentPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<SiteComponent> SiteComponentPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SiteComponentID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<SiteComponent> objects;
			string baseKey = cacheKeyPrefix + "SiteComponentPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<SiteComponent> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SiteComponent>();
				tmpList = Cache[key] as List<SiteComponent>;
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
					IQueryable<SiteComponent> itemQuery = SetupQuery(entity.SiteComponent, "SiteComponent", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ConfigurationSettings_SiteComponent");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterSiteComponentComponentName { get; set; }
			public string FilterSiteComponentDisplayName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterSiteComponentComponentName != null)
					filterList.Add("@FilterSiteComponentComponentName", FilterSiteComponentComponentName);
				if (FilterSiteComponentDisplayName != null)
					filterList.Add("@FilterSiteComponentDisplayName", FilterSiteComponentDisplayName);
				return filterList;
			}
		}
	}
}