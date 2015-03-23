using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.SiteLanguages
{
	public partial class Language : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "SiteLanguages_Language_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Culture", "CultureName"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public Language(Language objectToCopy)
		{
			Active = objectToCopy.Active;
			Culture = objectToCopy.Culture;
			CultureName = objectToCopy.CultureName;
			LanguageID = objectToCopy.LanguageID;
		}

		public virtual bool IsNewRecord
		{
			get { return LanguageID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("Language", this);
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

		public static Language GetByID(int LanguageID, IEnumerable<string> includeList = null)
		{
			Language obj = null;
			string key = cacheKeyPrefix + LanguageID + GetCacheIncludeText(includeList);

			Language tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Language;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Language> itemQuery = AddIncludes(entity.Language, includeList);
					obj = itemQuery.FirstOrDefault(n => n.LanguageID == LanguageID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Language> GetAll(IEnumerable<string> includeList = null)
		{
			List<Language> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Language> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Language>();
				tmpList = Cache[key] as List<Language>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Language> itemQuery = AddIncludes(entity.Language, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Language> LanguageGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterLanguageActive = Active.ToString();
			return LanguagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Language> LanguageGetByCulture(String Culture, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterLanguageCulture = Culture.ToString();
			return LanguagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Language> LanguageGetByCultureName(String CultureName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterLanguageCultureName = CultureName.ToString();
			return LanguagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Language> LanguagePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Language> objects = LanguagePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Language> LanguagePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return LanguagePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Language> LanguagePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return LanguagePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Language> LanguagePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "LanguageID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Language> objects;
			string baseKey = cacheKeyPrefix + "LanguagePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Language> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Language>();
				tmpList = Cache[key] as List<Language>;
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
					IQueryable<Language> itemQuery = SetupQuery(entity.Language, "Language", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("SiteLanguages_Language");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterLanguageActive { get; set; }
			public string FilterLanguageCulture { get; set; }
			public string FilterLanguageCultureName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterLanguageActive != null)
				{
					if (FilterLanguageActive == string.Empty)
						filterList.Add("@FilterLanguageActive", string.Empty);
					else
						filterList.Add("@FilterLanguageActive", Convert.ToBoolean(FilterLanguageActive));
				}
				if (FilterLanguageCulture != null)
					filterList.Add("@FilterLanguageCulture", FilterLanguageCulture);
				if (FilterLanguageCultureName != null)
					filterList.Add("@FilterLanguageCultureName", FilterLanguageCultureName);
				return filterList;
			}
		}
	}
}