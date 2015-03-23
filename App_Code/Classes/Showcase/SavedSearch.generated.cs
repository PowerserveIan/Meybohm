using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class SavedSearch : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_SavedSearch_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public SavedSearch()
		{
		}

		public SavedSearch(SavedSearch objectToCopy)
		{
			Created = objectToCopy.Created;
			DailyEmail = objectToCopy.DailyEmail;
			EnableEmailNotifications = objectToCopy.EnableEmailNotifications;
			FilterString = objectToCopy.FilterString;
			Image = objectToCopy.Image;
			LastAlertCount = objectToCopy.LastAlertCount;
			LastAlertDate = objectToCopy.LastAlertDate;
			Name = objectToCopy.Name;
			NewHomeSearch = objectToCopy.NewHomeSearch;
			SavedSearchID = objectToCopy.SavedSearchID;
			SeparateEmail = objectToCopy.SeparateEmail;
			ShowcaseID = objectToCopy.ShowcaseID;
			ShowcaseItemID = objectToCopy.ShowcaseItemID;
			UserID = objectToCopy.UserID;
		}

		public virtual bool IsNewRecord
		{
			get { return SavedSearchID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime CreatedClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(Created); }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime? LastAlertDateClientTime
		{
			get 
			{
				if (LastAlertDate.HasValue)
					return Helpers.ConvertUTCToClientTime(LastAlertDate.Value);
				return null;
			}
		}

		public virtual void Save()
		{
			SaveEntity("SavedSearch", this);
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

		public static SavedSearch GetByID(int SavedSearchID, IEnumerable<string> includeList = null)
		{
			SavedSearch obj = null;
			string key = cacheKeyPrefix + SavedSearchID + GetCacheIncludeText(includeList);

			SavedSearch tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as SavedSearch;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SavedSearch> itemQuery = AddIncludes(entity.SavedSearch, includeList);
					obj = itemQuery.FirstOrDefault(n => n.SavedSearchID == SavedSearchID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<SavedSearch> GetAll(IEnumerable<string> includeList = null)
		{
			List<SavedSearch> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<SavedSearch> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SavedSearch>();
				tmpList = Cache[key] as List<SavedSearch>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SavedSearch> itemQuery = AddIncludes(entity.SavedSearch, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<SavedSearch> SavedSearchGetByEnableEmailNotifications(Boolean EnableEmailNotifications, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSavedSearchEnableEmailNotifications = EnableEmailNotifications.ToString();
			return SavedSearchPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SavedSearch> SavedSearchGetByNewHomeSearch(Boolean NewHomeSearch, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSavedSearchNewHomeSearch = NewHomeSearch.ToString();
			return SavedSearchPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SavedSearch> SavedSearchGetBySeparateEmail(Boolean SeparateEmail, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSavedSearchSeparateEmail = SeparateEmail.ToString();
			return SavedSearchPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SavedSearch> SavedSearchGetByShowcaseID(Int32 ShowcaseID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSavedSearchShowcaseID = ShowcaseID.ToString();
			return SavedSearchPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SavedSearch> SavedSearchGetByShowcaseItemID(Int32? ShowcaseItemID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSavedSearchShowcaseItemID = ShowcaseItemID.ToString();
			return SavedSearchPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SavedSearch> SavedSearchGetByUserID(Int32 UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSavedSearchUserID = UserID.ToString();
			return SavedSearchPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<SavedSearch> SavedSearchPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<SavedSearch> objects = SavedSearchPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<SavedSearch> SavedSearchPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return SavedSearchPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<SavedSearch> SavedSearchPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return SavedSearchPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<SavedSearch> SavedSearchPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SavedSearchID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<SavedSearch> objects;
			string baseKey = cacheKeyPrefix + "SavedSearchPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<SavedSearch> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SavedSearch>();
				tmpList = Cache[key] as List<SavedSearch>;
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
					IQueryable<SavedSearch> itemQuery = SetupQuery(entity.SavedSearch, "SavedSearch", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_SavedSearch");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterSavedSearchEnableEmailNotifications { get; set; }
			public string FilterSavedSearchNewHomeSearch { get; set; }
			public string FilterSavedSearchSeparateEmail { get; set; }
			public string FilterSavedSearchShowcaseID { get; set; }
			public string FilterSavedSearchShowcaseItemID { get; set; }
			public string FilterSavedSearchUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterSavedSearchEnableEmailNotifications != null)
				{
					if (FilterSavedSearchEnableEmailNotifications == string.Empty)
						filterList.Add("@FilterSavedSearchEnableEmailNotifications", string.Empty);
					else
						filterList.Add("@FilterSavedSearchEnableEmailNotifications", Convert.ToBoolean(FilterSavedSearchEnableEmailNotifications));
				}
				if (FilterSavedSearchNewHomeSearch != null)
				{
					if (FilterSavedSearchNewHomeSearch == string.Empty)
						filterList.Add("@FilterSavedSearchNewHomeSearch", string.Empty);
					else
						filterList.Add("@FilterSavedSearchNewHomeSearch", Convert.ToBoolean(FilterSavedSearchNewHomeSearch));
				}
				if (FilterSavedSearchSeparateEmail != null)
				{
					if (FilterSavedSearchSeparateEmail == string.Empty)
						filterList.Add("@FilterSavedSearchSeparateEmail", string.Empty);
					else
						filterList.Add("@FilterSavedSearchSeparateEmail", Convert.ToBoolean(FilterSavedSearchSeparateEmail));
				}
				if (FilterSavedSearchShowcaseID != null)
				{
					if (FilterSavedSearchShowcaseID == string.Empty)
						filterList.Add("@FilterSavedSearchShowcaseID", string.Empty);
					else
						filterList.Add("@FilterSavedSearchShowcaseID", Convert.ToInt32(FilterSavedSearchShowcaseID));
				}
				if (FilterSavedSearchShowcaseItemID != null)
				{
					if (FilterSavedSearchShowcaseItemID == string.Empty)
						filterList.Add("@FilterSavedSearchShowcaseItemID", string.Empty);
					else
						filterList.Add("@FilterSavedSearchShowcaseItemID", Convert.ToInt32(FilterSavedSearchShowcaseItemID));
				}
				if (FilterSavedSearchUserID != null)
				{
					if (FilterSavedSearchUserID == string.Empty)
						filterList.Add("@FilterSavedSearchUserID", string.Empty);
					else
						filterList.Add("@FilterSavedSearchUserID", Convert.ToInt32(FilterSavedSearchUserID));
				}
				return filterList;
			}
		}
	}
}