using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_NewsPress
{
	public partial class NewsPressCategory : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_NewsPress_NewsPressCategory_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public NewsPressCategory(NewsPressCategory objectToCopy)
		{
			Active = objectToCopy.Active;
			DisplayOrder = objectToCopy.DisplayOrder;
			Name = objectToCopy.Name;
			NewsPressCategoryID = objectToCopy.NewsPressCategoryID;
			NewsPressParentCategoryID = objectToCopy.NewsPressParentCategoryID;
		}

		public virtual bool IsNewRecord
		{
			get { return NewsPressCategoryID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("NewsPressCategory", this);
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

		public static NewsPressCategory GetByID(int NewsPressCategoryID, IEnumerable<string> includeList = null)
		{
			NewsPressCategory obj = null;
			string key = cacheKeyPrefix + NewsPressCategoryID + GetCacheIncludeText(includeList);

			NewsPressCategory tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as NewsPressCategory;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsPressCategory> itemQuery = AddIncludes(entity.NewsPressCategory, includeList);
					obj = itemQuery.FirstOrDefault(n => n.NewsPressCategoryID == NewsPressCategoryID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<NewsPressCategory> GetAll(IEnumerable<string> includeList = null)
		{
			List<NewsPressCategory> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<NewsPressCategory> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsPressCategory>();
				tmpList = Cache[key] as List<NewsPressCategory>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsPressCategory> itemQuery = AddIncludes(entity.NewsPressCategory, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<NewsPressCategory> NewsPressCategoryGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsPressCategoryActive = Active.ToString();
			return NewsPressCategoryPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<NewsPressCategory> NewsPressCategoryGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsPressCategoryName = Name.ToString();
			return NewsPressCategoryPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<NewsPressCategory> NewsPressCategoryPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<NewsPressCategory> objects = NewsPressCategoryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<NewsPressCategory> NewsPressCategoryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return NewsPressCategoryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<NewsPressCategory> NewsPressCategoryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return NewsPressCategoryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<NewsPressCategory> NewsPressCategoryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "NewsPressCategoryID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<NewsPressCategory> objects;
			string baseKey = cacheKeyPrefix + "NewsPressCategoryPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<NewsPressCategory> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsPressCategory>();
				tmpList = Cache[key] as List<NewsPressCategory>;
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
					IQueryable<NewsPressCategory> itemQuery = SetupQuery(entity.NewsPressCategory, "NewsPressCategory", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_NewsPress_NewsPressCategory");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterNewsPressCategoryActive { get; set; }
			public string FilterNewsPressCategoryName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterNewsPressCategoryActive != null)
				{
					if (FilterNewsPressCategoryActive == string.Empty)
						filterList.Add("@FilterNewsPressCategoryActive", string.Empty);
					else
						filterList.Add("@FilterNewsPressCategoryActive", Convert.ToBoolean(FilterNewsPressCategoryActive));
				}
				if (FilterNewsPressCategoryName != null)
					filterList.Add("@FilterNewsPressCategoryName", FilterNewsPressCategoryName);
				return filterList;
			}
		}
	}
}