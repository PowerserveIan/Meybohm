using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class CMPageRole : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ContentManager_CMPageRole_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public CMPageRole()
		{
		}

		public CMPageRole(CMPageRole objectToCopy)
		{
			CMPageID = objectToCopy.CMPageID;
			CMPageRoleID = objectToCopy.CMPageRoleID;
			Editor = objectToCopy.Editor;
			RoleID = objectToCopy.RoleID;
		}

		public virtual bool IsNewRecord
		{
			get { return CMPageRoleID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("CMPageRole", this);
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

		public static CMPageRole GetByID(int CMPageRoleID, IEnumerable<string> includeList = null)
		{
			CMPageRole obj = null;
			string key = cacheKeyPrefix + CMPageRoleID + GetCacheIncludeText(includeList);

			CMPageRole tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as CMPageRole;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMPageRole> itemQuery = AddIncludes(entity.CMPageRole, includeList);
					obj = itemQuery.FirstOrDefault(n => n.CMPageRoleID == CMPageRoleID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<CMPageRole> GetAll(IEnumerable<string> includeList = null)
		{
			List<CMPageRole> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<CMPageRole> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMPageRole>();
				tmpList = Cache[key] as List<CMPageRole>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMPageRole> itemQuery = AddIncludes(entity.CMPageRole, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<CMPageRole> CMPageRoleGetByCMPageID(Int32 CMPageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageRoleCMPageID = CMPageID.ToString();
			return CMPageRolePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPageRole> CMPageRoleGetByEditor(Boolean Editor, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageRoleEditor = Editor.ToString();
			return CMPageRolePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPageRole> CMPageRoleGetByRoleID(Int32 RoleID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageRoleRoleID = RoleID.ToString();
			return CMPageRolePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<CMPageRole> CMPageRolePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<CMPageRole> objects = CMPageRolePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<CMPageRole> CMPageRolePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return CMPageRolePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<CMPageRole> CMPageRolePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return CMPageRolePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<CMPageRole> CMPageRolePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "CMPageRoleID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<CMPageRole> objects;
			string baseKey = cacheKeyPrefix + "CMPageRolePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<CMPageRole> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMPageRole>();
				tmpList = Cache[key] as List<CMPageRole>;
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
					IQueryable<CMPageRole> itemQuery = SetupQuery(entity.CMPageRole, "CMPageRole", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ContentManager_CMPageRole");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterCMPageRoleCMPageID { get; set; }
			public string FilterCMPageRoleEditor { get; set; }
			public string FilterCMPageRoleRoleID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterCMPageRoleCMPageID != null)
				{
					if (FilterCMPageRoleCMPageID == string.Empty)
						filterList.Add("@FilterCMPageRoleCMPageID", string.Empty);
					else
						filterList.Add("@FilterCMPageRoleCMPageID", Convert.ToInt32(FilterCMPageRoleCMPageID));
				}
				if (FilterCMPageRoleEditor != null)
				{
					if (FilterCMPageRoleEditor == string.Empty)
						filterList.Add("@FilterCMPageRoleEditor", string.Empty);
					else
						filterList.Add("@FilterCMPageRoleEditor", Convert.ToBoolean(FilterCMPageRoleEditor));
				}
				if (FilterCMPageRoleRoleID != null)
				{
					if (FilterCMPageRoleRoleID == string.Empty)
						filterList.Add("@FilterCMPageRoleRoleID", string.Empty);
					else
						filterList.Add("@FilterCMPageRoleRoleID", Convert.ToInt32(FilterCMPageRoleRoleID));
				}
				return filterList;
			}
		}
	}
}