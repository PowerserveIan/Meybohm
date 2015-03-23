using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ConfigurationSettings
{
	public partial class SiteSettingsDataType : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ConfigurationSettings_SiteSettingsDataType_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public SiteSettingsDataType(SiteSettingsDataType objectToCopy)
		{
			SiteSettingsDataTypeID = objectToCopy.SiteSettingsDataTypeID;
			Type = objectToCopy.Type;
		}

		public virtual bool IsNewRecord
		{
			get { return SiteSettingsDataTypeID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("SiteSettingsDataType", this);
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

		public static SiteSettingsDataType GetByID(int SiteSettingsDataTypeID, IEnumerable<string> includeList = null)
		{
			SiteSettingsDataType obj = null;
			string key = cacheKeyPrefix + SiteSettingsDataTypeID + GetCacheIncludeText(includeList);

			SiteSettingsDataType tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as SiteSettingsDataType;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SiteSettingsDataType> itemQuery = AddIncludes(entity.SiteSettingsDataType, includeList);
					obj = itemQuery.FirstOrDefault(n => n.SiteSettingsDataTypeID == SiteSettingsDataTypeID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<SiteSettingsDataType> GetAll(IEnumerable<string> includeList = null)
		{
			List<SiteSettingsDataType> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<SiteSettingsDataType> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SiteSettingsDataType>();
				tmpList = Cache[key] as List<SiteSettingsDataType>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SiteSettingsDataType> itemQuery = AddIncludes(entity.SiteSettingsDataType, includeList);
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

		public static List<SiteSettingsDataType> SiteSettingsDataTypePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<SiteSettingsDataType> objects = SiteSettingsDataTypePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<SiteSettingsDataType> SiteSettingsDataTypePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return SiteSettingsDataTypePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<SiteSettingsDataType> SiteSettingsDataTypePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return SiteSettingsDataTypePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<SiteSettingsDataType> SiteSettingsDataTypePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SiteSettingsDataTypeID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<SiteSettingsDataType> objects;
			string baseKey = cacheKeyPrefix + "SiteSettingsDataTypePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<SiteSettingsDataType> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SiteSettingsDataType>();
				tmpList = Cache[key] as List<SiteSettingsDataType>;
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
					IQueryable<SiteSettingsDataType> itemQuery = SetupQuery(entity.SiteSettingsDataType, "SiteSettingsDataType", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ConfigurationSettings_SiteSettingsDataType");
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