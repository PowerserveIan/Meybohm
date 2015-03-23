using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class CMMicrositeUser : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ContentManager_CMMicrositeUser_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public CMMicrositeUser()
		{
		}

		public CMMicrositeUser(CMMicrositeUser objectToCopy)
		{
			CMMicrositeID = objectToCopy.CMMicrositeID;
			CMMicrositeUserID = objectToCopy.CMMicrositeUserID;
			UserID = objectToCopy.UserID;
		}

		public virtual bool IsNewRecord
		{
			get { return CMMicrositeUserID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("CMMicrositeUser", this);
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

		public static CMMicrositeUser GetByID(int CMMicrositeUserID, IEnumerable<string> includeList = null)
		{
			CMMicrositeUser obj = null;
			string key = cacheKeyPrefix + CMMicrositeUserID + GetCacheIncludeText(includeList);

			CMMicrositeUser tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as CMMicrositeUser;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMMicrositeUser> itemQuery = AddIncludes(entity.CMMicrositeUser, includeList);
					obj = itemQuery.FirstOrDefault(n => n.CMMicrositeUserID == CMMicrositeUserID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<CMMicrositeUser> GetAll(IEnumerable<string> includeList = null)
		{
			List<CMMicrositeUser> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<CMMicrositeUser> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMMicrositeUser>();
				tmpList = Cache[key] as List<CMMicrositeUser>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMMicrositeUser> itemQuery = AddIncludes(entity.CMMicrositeUser, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<CMMicrositeUser> CMMicrositeUserGetByCMMicrositeID(Int32 CMMicrositeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMMicrositeUserCMMicrositeID = CMMicrositeID.ToString();
			return CMMicrositeUserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMMicrositeUser> CMMicrositeUserGetByUserID(Int32 UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMMicrositeUserUserID = UserID.ToString();
			return CMMicrositeUserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<CMMicrositeUser> CMMicrositeUserPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<CMMicrositeUser> objects = CMMicrositeUserPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<CMMicrositeUser> CMMicrositeUserPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return CMMicrositeUserPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<CMMicrositeUser> CMMicrositeUserPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return CMMicrositeUserPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<CMMicrositeUser> CMMicrositeUserPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "CMMicrositeUserID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<CMMicrositeUser> objects;
			string baseKey = cacheKeyPrefix + "CMMicrositeUserPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<CMMicrositeUser> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMMicrositeUser>();
				tmpList = Cache[key] as List<CMMicrositeUser>;
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
					IQueryable<CMMicrositeUser> itemQuery = SetupQuery(entity.CMMicrositeUser, "CMMicrositeUser", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ContentManager_CMMicrositeUser");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterCMMicrositeUserCMMicrositeID { get; set; }
			public string FilterCMMicrositeUserUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterCMMicrositeUserCMMicrositeID != null)
				{
					if (FilterCMMicrositeUserCMMicrositeID == string.Empty)
						filterList.Add("@FilterCMMicrositeUserCMMicrositeID", string.Empty);
					else
						filterList.Add("@FilterCMMicrositeUserCMMicrositeID", Convert.ToInt32(FilterCMMicrositeUserCMMicrositeID));
				}
				if (FilterCMMicrositeUserUserID != null)
				{
					if (FilterCMMicrositeUserUserID == string.Empty)
						filterList.Add("@FilterCMMicrositeUserUserID", string.Empty);
					else
						filterList.Add("@FilterCMMicrositeUserUserID", Convert.ToInt32(FilterCMMicrositeUserUserID));
				}
				return filterList;
			}
		}
	}
}