using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class ShowcaseUser : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_ShowcaseUser_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public ShowcaseUser()
		{
		}

		public ShowcaseUser(ShowcaseUser objectToCopy)
		{
			ShowcaseID = objectToCopy.ShowcaseID;
			ShowcaseUserID = objectToCopy.ShowcaseUserID;
			UserID = objectToCopy.UserID;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseUserID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("ShowcaseUser", this);
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

		public static ShowcaseUser GetByID(int ShowcaseUserID, IEnumerable<string> includeList = null)
		{
			ShowcaseUser obj = null;
			string key = cacheKeyPrefix + ShowcaseUserID + GetCacheIncludeText(includeList);

			ShowcaseUser tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as ShowcaseUser;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseUser> itemQuery = AddIncludes(entity.ShowcaseUser, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseUserID == ShowcaseUserID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<ShowcaseUser> GetAll(IEnumerable<string> includeList = null)
		{
			List<ShowcaseUser> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<ShowcaseUser> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseUser>();
				tmpList = Cache[key] as List<ShowcaseUser>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseUser> itemQuery = AddIncludes(entity.ShowcaseUser, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<ShowcaseUser> ShowcaseUserGetByShowcaseID(Int32 ShowcaseID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseUserShowcaseID = ShowcaseID.ToString();
			return ShowcaseUserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseUser> ShowcaseUserGetByUserID(Int32 UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseUserUserID = UserID.ToString();
			return ShowcaseUserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<ShowcaseUser> ShowcaseUserPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<ShowcaseUser> objects = ShowcaseUserPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseUser> ShowcaseUserPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return ShowcaseUserPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<ShowcaseUser> ShowcaseUserPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return ShowcaseUserPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<ShowcaseUser> ShowcaseUserPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseUserID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<ShowcaseUser> objects;
			string baseKey = cacheKeyPrefix + "ShowcaseUserPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseUser> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseUser>();
				tmpList = Cache[key] as List<ShowcaseUser>;
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
					IQueryable<ShowcaseUser> itemQuery = SetupQuery(entity.ShowcaseUser, "ShowcaseUser", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_ShowcaseUser");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterShowcaseUserShowcaseID { get; set; }
			public string FilterShowcaseUserUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterShowcaseUserShowcaseID != null)
				{
					if (FilterShowcaseUserShowcaseID == string.Empty)
						filterList.Add("@FilterShowcaseUserShowcaseID", string.Empty);
					else
						filterList.Add("@FilterShowcaseUserShowcaseID", Convert.ToInt32(FilterShowcaseUserShowcaseID));
				}
				if (FilterShowcaseUserUserID != null)
				{
					if (FilterShowcaseUserUserID == string.Empty)
						filterList.Add("@FilterShowcaseUserUserID", string.Empty);
					else
						filterList.Add("@FilterShowcaseUserUserID", Convert.ToInt32(FilterShowcaseUserUserID));
				}
				return filterList;
			}
		}
	}
}