using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_MembershipProvider
{
	public partial class Role : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_MembershipProvider_Role_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public Role(Role objectToCopy)
		{
			Name = objectToCopy.Name;
			RoleID = objectToCopy.RoleID;
			SystemRole = objectToCopy.SystemRole;
		}

		public virtual bool IsNewRecord
		{
			get { return RoleID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("Role", this);
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

		public static Role GetByID(int RoleID, IEnumerable<string> includeList = null)
		{
			Role obj = null;
			string key = cacheKeyPrefix + RoleID + GetCacheIncludeText(includeList);

			Role tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Role;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Role> itemQuery = AddIncludes(entity.Role, includeList);
					obj = itemQuery.FirstOrDefault(n => n.RoleID == RoleID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Role> GetAll(IEnumerable<string> includeList = null)
		{
			List<Role> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Role> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Role>();
				tmpList = Cache[key] as List<Role>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Role> itemQuery = AddIncludes(entity.Role, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Role> RoleGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterRoleName = Name.ToString();
			return RolePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Role> RoleGetBySystemRole(Boolean SystemRole, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterRoleSystemRole = SystemRole.ToString();
			return RolePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Role> RolePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Role> objects = RolePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Role> RolePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return RolePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Role> RolePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return RolePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Role> RolePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "RoleID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Role> objects;
			string baseKey = cacheKeyPrefix + "RolePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Role> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Role>();
				tmpList = Cache[key] as List<Role>;
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
					IQueryable<Role> itemQuery = SetupQuery(entity.Role, "Role", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_MembershipProvider_Role");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterRoleName { get; set; }
			public string FilterRoleSystemRole { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterRoleName != null)
					filterList.Add("@FilterRoleName", FilterRoleName);
				if (FilterRoleSystemRole != null)
				{
					if (FilterRoleSystemRole == string.Empty)
						filterList.Add("@FilterRoleSystemRole", string.Empty);
					else
						filterList.Add("@FilterRoleSystemRole", Convert.ToBoolean(FilterRoleSystemRole));
				}
				return filterList;
			}
		}
	}
}