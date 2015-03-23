using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_MembershipProvider
{
	public partial class UserTeam : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_MembershipProvider_UserTeam_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public UserTeam()
		{
		}

		public UserTeam(UserTeam objectToCopy)
		{
			TeamID = objectToCopy.TeamID;
			UserID = objectToCopy.UserID;
			UserTeamID = objectToCopy.UserTeamID;
		}

		public virtual bool IsNewRecord
		{
			get { return UserTeamID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("UserTeam", this);
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

		public static UserTeam GetByID(int UserTeamID, IEnumerable<string> includeList = null)
		{
			UserTeam obj = null;
			string key = cacheKeyPrefix + UserTeamID + GetCacheIncludeText(includeList);

			UserTeam tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as UserTeam;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserTeam> itemQuery = AddIncludes(entity.UserTeam, includeList);
					obj = itemQuery.FirstOrDefault(n => n.UserTeamID == UserTeamID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<UserTeam> GetAll(IEnumerable<string> includeList = null)
		{
			List<UserTeam> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<UserTeam> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserTeam>();
				tmpList = Cache[key] as List<UserTeam>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserTeam> itemQuery = AddIncludes(entity.UserTeam, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<UserTeam> UserTeamGetByTeamID(Int32 TeamID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserTeamTeamID = TeamID.ToString();
			return UserTeamPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<UserTeam> UserTeamGetByUserID(Int32 UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserTeamUserID = UserID.ToString();
			return UserTeamPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<UserTeam> UserTeamPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<UserTeam> objects = UserTeamPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<UserTeam> UserTeamPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return UserTeamPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<UserTeam> UserTeamPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return UserTeamPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<UserTeam> UserTeamPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "UserTeamID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<UserTeam> objects;
			string baseKey = cacheKeyPrefix + "UserTeamPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<UserTeam> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserTeam>();
				tmpList = Cache[key] as List<UserTeam>;
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
					IQueryable<UserTeam> itemQuery = SetupQuery(entity.UserTeam, "UserTeam", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_MembershipProvider_UserTeam");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterUserTeamTeamID { get; set; }
			public string FilterUserTeamUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterUserTeamTeamID != null)
				{
					if (FilterUserTeamTeamID == string.Empty)
						filterList.Add("@FilterUserTeamTeamID", string.Empty);
					else
						filterList.Add("@FilterUserTeamTeamID", Convert.ToInt32(FilterUserTeamTeamID));
				}
				if (FilterUserTeamUserID != null)
				{
					if (FilterUserTeamUserID == string.Empty)
						filterList.Add("@FilterUserTeamUserID", string.Empty);
					else
						filterList.Add("@FilterUserTeamUserID", Convert.ToInt32(FilterUserTeamUserID));
				}
				return filterList;
			}
		}
	}
}