using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class CMPageTitle : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ContentManager_CMPageTitle_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Title"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public CMPageTitle()
		{
		}

		public CMPageTitle(CMPageTitle objectToCopy)
		{
			CMPageID = objectToCopy.CMPageID;
			CMPageTitleID = objectToCopy.CMPageTitleID;
			LanguageID = objectToCopy.LanguageID;
			Title = objectToCopy.Title;
		}

		public virtual bool IsNewRecord
		{
			get { return CMPageTitleID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("CMPageTitle", this);
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

		public static CMPageTitle GetByID(int CMPageTitleID, IEnumerable<string> includeList = null)
		{
			CMPageTitle obj = null;
			string key = cacheKeyPrefix + CMPageTitleID + GetCacheIncludeText(includeList);

			CMPageTitle tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as CMPageTitle;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMPageTitle> itemQuery = AddIncludes(entity.CMPageTitle, includeList);
					obj = itemQuery.FirstOrDefault(n => n.CMPageTitleID == CMPageTitleID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<CMPageTitle> GetAll(IEnumerable<string> includeList = null)
		{
			List<CMPageTitle> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<CMPageTitle> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMPageTitle>();
				tmpList = Cache[key] as List<CMPageTitle>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMPageTitle> itemQuery = AddIncludes(entity.CMPageTitle, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<CMPageTitle> CMPageTitleGetByCMPageID(Int32 CMPageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageTitleCMPageID = CMPageID.ToString();
			return CMPageTitlePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPageTitle> CMPageTitleGetByLanguageID(Int32 LanguageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageTitleLanguageID = LanguageID.ToString();
			return CMPageTitlePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPageTitle> CMPageTitleGetByTitle(String Title, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageTitleTitle = Title.ToString();
			return CMPageTitlePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<CMPageTitle> CMPageTitlePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<CMPageTitle> objects = CMPageTitlePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<CMPageTitle> CMPageTitlePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return CMPageTitlePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<CMPageTitle> CMPageTitlePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return CMPageTitlePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<CMPageTitle> CMPageTitlePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "CMPageTitleID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<CMPageTitle> objects;
			string baseKey = cacheKeyPrefix + "CMPageTitlePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<CMPageTitle> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMPageTitle>();
				tmpList = Cache[key] as List<CMPageTitle>;
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
					IQueryable<CMPageTitle> itemQuery = SetupQuery(entity.CMPageTitle, "CMPageTitle", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ContentManager_CMPageTitle");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterCMPageTitleCMPageID { get; set; }
			public string FilterCMPageTitleLanguageID { get; set; }
			public string FilterCMPageTitleTitle { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterCMPageTitleCMPageID != null)
				{
					if (FilterCMPageTitleCMPageID == string.Empty)
						filterList.Add("@FilterCMPageTitleCMPageID", string.Empty);
					else
						filterList.Add("@FilterCMPageTitleCMPageID", Convert.ToInt32(FilterCMPageTitleCMPageID));
				}
				if (FilterCMPageTitleLanguageID != null)
				{
					if (FilterCMPageTitleLanguageID == string.Empty)
						filterList.Add("@FilterCMPageTitleLanguageID", string.Empty);
					else
						filterList.Add("@FilterCMPageTitleLanguageID", Convert.ToInt32(FilterCMPageTitleLanguageID));
				}
				if (FilterCMPageTitleTitle != null)
					filterList.Add("@FilterCMPageTitleTitle", FilterCMPageTitleTitle);
				return filterList;
			}
		}
	}
}