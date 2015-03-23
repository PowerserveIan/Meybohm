using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_NewsPress
{
	public partial class NewsPress : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_NewsPress_NewsPress_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Title"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public NewsPress(NewsPress objectToCopy)
		{
			Active = objectToCopy.Active;
			Archived = objectToCopy.Archived;
			Author = objectToCopy.Author;
			Date = objectToCopy.Date;
			Featured = objectToCopy.Featured;
			NewsPressID = objectToCopy.NewsPressID;
			StoryHTML = objectToCopy.StoryHTML;
			Summary = objectToCopy.Summary;
			Title = objectToCopy.Title;
		}

		public virtual bool IsNewRecord
		{
			get { return NewsPressID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime DateClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(Date); }
		}

		public virtual void Save()
		{
			SaveEntity("NewsPress", this);
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

		public static NewsPress GetByID(int NewsPressID, IEnumerable<string> includeList = null)
		{
			NewsPress obj = null;
			string key = cacheKeyPrefix + NewsPressID + GetCacheIncludeText(includeList);

			NewsPress tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as NewsPress;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsPress> itemQuery = AddIncludes(entity.NewsPress, includeList);
					obj = itemQuery.FirstOrDefault(n => n.NewsPressID == NewsPressID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<NewsPress> GetAll(IEnumerable<string> includeList = null)
		{
			List<NewsPress> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<NewsPress> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsPress>();
				tmpList = Cache[key] as List<NewsPress>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NewsPress> itemQuery = AddIncludes(entity.NewsPress, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<NewsPress> NewsPressGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsPressActive = Active.ToString();
			return NewsPressPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<NewsPress> NewsPressGetByDate(DateTime Date, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsPressDate = Date.ToString();
			return NewsPressPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<NewsPress> NewsPressGetByFeatured(Boolean Featured, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsPressFeatured = Featured.ToString();
			return NewsPressPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<NewsPress> NewsPressGetByTitle(String Title, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsPressTitle = Title.ToString();
			return NewsPressPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<NewsPress> NewsPressPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<NewsPress> objects = NewsPressPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<NewsPress> NewsPressPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return NewsPressPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<NewsPress> NewsPressPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return NewsPressPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<NewsPress> NewsPressPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "NewsPressID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<NewsPress> objects;
			string baseKey = cacheKeyPrefix + "NewsPressPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<NewsPress> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NewsPress>();
				tmpList = Cache[key] as List<NewsPress>;
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
					IQueryable<NewsPress> itemQuery = SetupQuery(entity.NewsPress, "NewsPress", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_NewsPress_NewsPress");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterNewsPressActive { get; set; }
			public string FilterNewsPressDate { get; set; }
			public string FilterNewsPressFeatured { get; set; }
			public string FilterNewsPressTitle { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterNewsPressActive != null)
				{
					if (FilterNewsPressActive == string.Empty)
						filterList.Add("@FilterNewsPressActive", string.Empty);
					else
						filterList.Add("@FilterNewsPressActive", Convert.ToBoolean(FilterNewsPressActive));
				}
				if (FilterNewsPressDate != null)
				{
					if (FilterNewsPressDate == string.Empty)
						filterList.Add("@FilterNewsPressDate", string.Empty);
					else
						filterList.Add("@FilterNewsPressDate", Convert.ToDateTime(FilterNewsPressDate));
				}
				if (FilterNewsPressFeatured != null)
				{
					if (FilterNewsPressFeatured == string.Empty)
						filterList.Add("@FilterNewsPressFeatured", string.Empty);
					else
						filterList.Add("@FilterNewsPressFeatured", Convert.ToBoolean(FilterNewsPressFeatured));
				}
				if (FilterNewsPressTitle != null)
					filterList.Add("@FilterNewsPressTitle", FilterNewsPressTitle);
				return filterList;
			}
		}
	}
}