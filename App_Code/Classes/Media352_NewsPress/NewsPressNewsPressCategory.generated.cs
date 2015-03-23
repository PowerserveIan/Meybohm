using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_NewsPress
{
	public partial class NewsPressNewsPressCategory : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_NewsPress_NewsPressNewsPressCategory_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public NewsPressNewsPressCategory()
		{
		}

		public NewsPressNewsPressCategory(NewsPressNewsPressCategory objectToCopy)
		{
			NewsPressCategoryID = objectToCopy.NewsPressCategoryID;
			NewsPressID = objectToCopy.NewsPressID;
			NewsPressNewsPressCategoryID = objectToCopy.NewsPressNewsPressCategoryID;
		}

		public virtual bool IsNewRecord
		{
			get { return NewsPressNewsPressCategoryID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("NewsPressNewsPressCategory", this);
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

		public static NewsPressNewsPressCategory GetByID(int NewsPressNewsPressCategoryID, IEnumerable<string> includeList = null)
		{
			NewsPressNewsPressCategory obj = null;
			string key = cacheKeyPrefix + NewsPressNewsPressCategoryID + GetCacheIncludeText(includeList);

			NewsPressNewsPressCategory tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as NewsPressNewsPressCategory;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsPressNewsPressCategory> itemQuery = AddIncludes(entity.NewsPressNewsPressCategory, includeList);
					obj = itemQuery.FirstOrDefault(n => n.NewsPressNewsPressCategoryID == NewsPressNewsPressCategoryID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<NewsPressNewsPressCategory> GetAll(IEnumerable<string> includeList = null)
		{
			List<NewsPressNewsPressCategory> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<NewsPressNewsPressCategory> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsPressNewsPressCategory>();
				tmpList = Cache[key] as List<NewsPressNewsPressCategory>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsPressNewsPressCategory> itemQuery = AddIncludes(entity.NewsPressNewsPressCategory, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<NewsPressNewsPressCategory> NewsPressNewsPressCategoryGetByNewsPressID(Int32 NewsPressID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsPressNewsPressCategoryNewsPressID = NewsPressID.ToString();
			return NewsPressNewsPressCategoryPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<NewsPressNewsPressCategory> NewsPressNewsPressCategoryPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<NewsPressNewsPressCategory> objects = NewsPressNewsPressCategoryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<NewsPressNewsPressCategory> NewsPressNewsPressCategoryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return NewsPressNewsPressCategoryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<NewsPressNewsPressCategory> NewsPressNewsPressCategoryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return NewsPressNewsPressCategoryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<NewsPressNewsPressCategory> NewsPressNewsPressCategoryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "NewsPressNewsPressCategoryID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<NewsPressNewsPressCategory> objects;
			string baseKey = cacheKeyPrefix + "NewsPressNewsPressCategoryPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<NewsPressNewsPressCategory> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsPressNewsPressCategory>();
				tmpList = Cache[key] as List<NewsPressNewsPressCategory>;
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
					IQueryable<NewsPressNewsPressCategory> itemQuery = SetupQuery(entity.NewsPressNewsPressCategory, "NewsPressNewsPressCategory", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_NewsPress_NewsPressNewsPressCategory");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterNewsPressNewsPressCategoryNewsPressID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterNewsPressNewsPressCategoryNewsPressID != null)
				{
					if (FilterNewsPressNewsPressCategoryNewsPressID == string.Empty)
						filterList.Add("@FilterNewsPressNewsPressCategoryNewsPressID", string.Empty);
					else
						filterList.Add("@FilterNewsPressNewsPressCategoryNewsPressID", Convert.ToInt32(FilterNewsPressNewsPressCategoryNewsPressID));
				}
				return filterList;
			}
		}
	}
}