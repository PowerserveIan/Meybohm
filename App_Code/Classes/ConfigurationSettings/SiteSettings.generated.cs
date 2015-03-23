using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ConfigurationSettings
{
	public partial class SiteSettings : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ConfigurationSettings_SiteSettings_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public SiteSettings(SiteSettings objectToCopy)
		{
			DateCreated = objectToCopy.DateCreated;
			DateLastModified = objectToCopy.DateLastModified;
			Description = objectToCopy.Description;
			DisplayOrder = objectToCopy.DisplayOrder;
			EmbeddedDescription = objectToCopy.EmbeddedDescription;
			IsCustom = objectToCopy.IsCustom;
			IsRequired = objectToCopy.IsRequired;
			Options = objectToCopy.Options;
			Setting = objectToCopy.Setting;
			SiteComponentID = objectToCopy.SiteComponentID;
			SiteSettingsDataTypeID = objectToCopy.SiteSettingsDataTypeID;
			SiteSettingsID = objectToCopy.SiteSettingsID;
			Value = objectToCopy.Value;
		}

		public virtual bool IsNewRecord
		{
			get { return SiteSettingsID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime DateCreatedClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(DateCreated); }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime DateLastModifiedClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(DateLastModified); }
		}

		public virtual void Save()
		{
			SaveEntity("SiteSettings", this);
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

		public static SiteSettings GetByID(int SiteSettingsID, IEnumerable<string> includeList = null)
		{
			SiteSettings obj = null;
			string key = cacheKeyPrefix + SiteSettingsID + GetCacheIncludeText(includeList);

			SiteSettings tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as SiteSettings;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SiteSettings> itemQuery = AddIncludes(entity.SiteSettings, includeList);
					obj = itemQuery.FirstOrDefault(n => n.SiteSettingsID == SiteSettingsID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<SiteSettings> GetAll(IEnumerable<string> includeList = null)
		{
			List<SiteSettings> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<SiteSettings> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SiteSettings>();
				tmpList = Cache[key] as List<SiteSettings>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SiteSettings> itemQuery = AddIncludes(entity.SiteSettings, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<SiteSettings> SiteSettingsGetBySiteComponentID(Int32 SiteComponentID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSiteSettingsSiteComponentID = SiteComponentID.ToString();
			return SiteSettingsPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<SiteSettings> SiteSettingsPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<SiteSettings> objects = SiteSettingsPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<SiteSettings> SiteSettingsPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return SiteSettingsPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<SiteSettings> SiteSettingsPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return SiteSettingsPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<SiteSettings> SiteSettingsPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SiteSettingsID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<SiteSettings> objects;
			string baseKey = cacheKeyPrefix + "SiteSettingsPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<SiteSettings> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SiteSettings>();
				tmpList = Cache[key] as List<SiteSettings>;
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
					IQueryable<SiteSettings> itemQuery = SetupQuery(entity.SiteSettings, "SiteSettings", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ConfigurationSettings_SiteSettings");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterSiteSettingsSiteComponentID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterSiteSettingsSiteComponentID != null)
				{
					if (FilterSiteSettingsSiteComponentID == string.Empty)
						filterList.Add("@FilterSiteSettingsSiteComponentID", string.Empty);
					else
						filterList.Add("@FilterSiteSettingsSiteComponentID", Convert.ToInt32(FilterSiteSettingsSiteComponentID));
				}
				return filterList;
			}
		}
	}
}