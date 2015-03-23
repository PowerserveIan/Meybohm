using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_MembershipProvider
{
	public partial class UserLanguageSpoken : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_MembershipProvider_UserLanguageSpoken_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public UserLanguageSpoken()
		{
		}

		public UserLanguageSpoken(UserLanguageSpoken objectToCopy)
		{
			LanguageID = objectToCopy.LanguageID;
			UserID = objectToCopy.UserID;
			UserLanguageSpokenID = objectToCopy.UserLanguageSpokenID;
		}

		public virtual bool IsNewRecord
		{
			get { return UserLanguageSpokenID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("UserLanguageSpoken", this);
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

		public static UserLanguageSpoken GetByID(int UserLanguageSpokenID, IEnumerable<string> includeList = null)
		{
			UserLanguageSpoken obj = null;
			string key = cacheKeyPrefix + UserLanguageSpokenID + GetCacheIncludeText(includeList);

			UserLanguageSpoken tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as UserLanguageSpoken;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserLanguageSpoken> itemQuery = AddIncludes(entity.UserLanguageSpoken, includeList);
					obj = itemQuery.FirstOrDefault(n => n.UserLanguageSpokenID == UserLanguageSpokenID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<UserLanguageSpoken> GetAll(IEnumerable<string> includeList = null)
		{
			List<UserLanguageSpoken> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<UserLanguageSpoken> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserLanguageSpoken>();
				tmpList = Cache[key] as List<UserLanguageSpoken>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserLanguageSpoken> itemQuery = AddIncludes(entity.UserLanguageSpoken, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<UserLanguageSpoken> UserLanguageSpokenGetByLanguageID(Int32 LanguageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserLanguageSpokenLanguageID = LanguageID.ToString();
			return UserLanguageSpokenPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<UserLanguageSpoken> UserLanguageSpokenGetByUserID(Int32 UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserLanguageSpokenUserID = UserID.ToString();
			return UserLanguageSpokenPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<UserLanguageSpoken> UserLanguageSpokenPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<UserLanguageSpoken> objects = UserLanguageSpokenPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<UserLanguageSpoken> UserLanguageSpokenPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return UserLanguageSpokenPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<UserLanguageSpoken> UserLanguageSpokenPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return UserLanguageSpokenPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<UserLanguageSpoken> UserLanguageSpokenPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "UserLanguageSpokenID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<UserLanguageSpoken> objects;
			string baseKey = cacheKeyPrefix + "UserLanguageSpokenPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<UserLanguageSpoken> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserLanguageSpoken>();
				tmpList = Cache[key] as List<UserLanguageSpoken>;
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
					IQueryable<UserLanguageSpoken> itemQuery = SetupQuery(entity.UserLanguageSpoken, "UserLanguageSpoken", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_MembershipProvider_UserLanguageSpoken");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterUserLanguageSpokenLanguageID { get; set; }
			public string FilterUserLanguageSpokenUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterUserLanguageSpokenLanguageID != null)
				{
					if (FilterUserLanguageSpokenLanguageID == string.Empty)
						filterList.Add("@FilterUserLanguageSpokenLanguageID", string.Empty);
					else
						filterList.Add("@FilterUserLanguageSpokenLanguageID", Convert.ToInt32(FilterUserLanguageSpokenLanguageID));
				}
				if (FilterUserLanguageSpokenUserID != null)
				{
					if (FilterUserLanguageSpokenUserID == string.Empty)
						filterList.Add("@FilterUserLanguageSpokenUserID", string.Empty);
					else
						filterList.Add("@FilterUserLanguageSpokenUserID", Convert.ToInt32(FilterUserLanguageSpokenUserID));
				}
				return filterList;
			}
		}
	}
}