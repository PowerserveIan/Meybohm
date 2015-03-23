using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class ShowcaseSiteSettings : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_ShowcaseSiteSettings_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public ShowcaseSiteSettings()
		{
		}

		public ShowcaseSiteSettings(ShowcaseSiteSettings objectToCopy)
		{
			ShowcaseID = objectToCopy.ShowcaseID;
			ShowcaseSettingsID = objectToCopy.ShowcaseSettingsID;
			SiteSettingsID = objectToCopy.SiteSettingsID;
			Value = objectToCopy.Value;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseSettingsID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("ShowcaseSiteSettings", this);
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

		public static ShowcaseSiteSettings GetByID(int ShowcaseSettingsID, IEnumerable<string> includeList = null)
		{
			ShowcaseSiteSettings obj = null;
			string key = cacheKeyPrefix + ShowcaseSettingsID + GetCacheIncludeText(includeList);

			ShowcaseSiteSettings tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as ShowcaseSiteSettings;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseSiteSettings> itemQuery = AddIncludes(entity.ShowcaseSiteSettings, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseSettingsID == ShowcaseSettingsID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<ShowcaseSiteSettings> GetAll(IEnumerable<string> includeList = null)
		{
			List<ShowcaseSiteSettings> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<ShowcaseSiteSettings> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseSiteSettings>();
				tmpList = Cache[key] as List<ShowcaseSiteSettings>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseSiteSettings> itemQuery = AddIncludes(entity.ShowcaseSiteSettings, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<ShowcaseSiteSettings> ShowcaseSiteSettingsGetByShowcaseID(Int32 ShowcaseID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseSiteSettingsShowcaseID = ShowcaseID.ToString();
			return ShowcaseSiteSettingsPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseSiteSettings> ShowcaseSiteSettingsGetBySiteSettingsID(Int32 SiteSettingsID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseSiteSettingsSiteSettingsID = SiteSettingsID.ToString();
			return ShowcaseSiteSettingsPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<ShowcaseSiteSettings> ShowcaseSiteSettingsPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<ShowcaseSiteSettings> objects = ShowcaseSiteSettingsPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseSiteSettings> ShowcaseSiteSettingsPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return ShowcaseSiteSettingsPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<ShowcaseSiteSettings> ShowcaseSiteSettingsPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return ShowcaseSiteSettingsPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<ShowcaseSiteSettings> ShowcaseSiteSettingsPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseSettingsID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<ShowcaseSiteSettings> objects;
			string baseKey = cacheKeyPrefix + "ShowcaseSiteSettingsPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseSiteSettings> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseSiteSettings>();
				tmpList = Cache[key] as List<ShowcaseSiteSettings>;
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
					IQueryable<ShowcaseSiteSettings> itemQuery = SetupQuery(entity.ShowcaseSiteSettings, "ShowcaseSiteSettings", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_ShowcaseSiteSettings");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterShowcaseSiteSettingsShowcaseID { get; set; }
			public string FilterShowcaseSiteSettingsSiteSettingsID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterShowcaseSiteSettingsShowcaseID != null)
				{
					if (FilterShowcaseSiteSettingsShowcaseID == string.Empty)
						filterList.Add("@FilterShowcaseSiteSettingsShowcaseID", string.Empty);
					else
						filterList.Add("@FilterShowcaseSiteSettingsShowcaseID", Convert.ToInt32(FilterShowcaseSiteSettingsShowcaseID));
				}
				if (FilterShowcaseSiteSettingsSiteSettingsID != null)
				{
					if (FilterShowcaseSiteSettingsSiteSettingsID == string.Empty)
						filterList.Add("@FilterShowcaseSiteSettingsSiteSettingsID", string.Empty);
					else
						filterList.Add("@FilterShowcaseSiteSettingsSiteSettingsID", Convert.ToInt32(FilterShowcaseSiteSettingsSiteSettingsID));
				}
				return filterList;
			}
		}
	}
}