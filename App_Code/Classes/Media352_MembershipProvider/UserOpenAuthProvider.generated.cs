using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_MembershipProvider
{
	public partial class UserOpenAuthProvider : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_MembershipProvider_UserOpenAuthProvider_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "ProviderID", "ProviderName"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public UserOpenAuthProvider()
		{
		}

		public UserOpenAuthProvider(UserOpenAuthProvider objectToCopy)
		{
			ProviderID = objectToCopy.ProviderID;
			ProviderName = objectToCopy.ProviderName;
			UserID = objectToCopy.UserID;
			UserOpenAuthProviderID = objectToCopy.UserOpenAuthProviderID;
		}

		public virtual bool IsNewRecord
		{
			get { return UserOpenAuthProviderID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("UserOpenAuthProvider", this);
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

		public static UserOpenAuthProvider GetByID(int UserOpenAuthProviderID, IEnumerable<string> includeList = null)
		{
			UserOpenAuthProvider obj = null;
			string key = cacheKeyPrefix + UserOpenAuthProviderID + GetCacheIncludeText(includeList);

			UserOpenAuthProvider tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as UserOpenAuthProvider;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserOpenAuthProvider> itemQuery = AddIncludes(entity.UserOpenAuthProvider, includeList);
					obj = itemQuery.FirstOrDefault(n => n.UserOpenAuthProviderID == UserOpenAuthProviderID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<UserOpenAuthProvider> GetAll(IEnumerable<string> includeList = null)
		{
			List<UserOpenAuthProvider> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<UserOpenAuthProvider> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserOpenAuthProvider>();
				tmpList = Cache[key] as List<UserOpenAuthProvider>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserOpenAuthProvider> itemQuery = AddIncludes(entity.UserOpenAuthProvider, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<UserOpenAuthProvider> UserOpenAuthProviderGetByProviderID(String ProviderID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserOpenAuthProviderProviderID = ProviderID.ToString();
			return UserOpenAuthProviderPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<UserOpenAuthProvider> UserOpenAuthProviderGetByProviderName(String ProviderName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserOpenAuthProviderProviderName = ProviderName.ToString();
			return UserOpenAuthProviderPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<UserOpenAuthProvider> UserOpenAuthProviderGetByUserID(Int32 UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserOpenAuthProviderUserID = UserID.ToString();
			return UserOpenAuthProviderPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<UserOpenAuthProvider> UserOpenAuthProviderPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<UserOpenAuthProvider> objects = UserOpenAuthProviderPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<UserOpenAuthProvider> UserOpenAuthProviderPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return UserOpenAuthProviderPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<UserOpenAuthProvider> UserOpenAuthProviderPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return UserOpenAuthProviderPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<UserOpenAuthProvider> UserOpenAuthProviderPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "UserOpenAuthProviderID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<UserOpenAuthProvider> objects;
			string baseKey = cacheKeyPrefix + "UserOpenAuthProviderPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<UserOpenAuthProvider> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserOpenAuthProvider>();
				tmpList = Cache[key] as List<UserOpenAuthProvider>;
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
					IQueryable<UserOpenAuthProvider> itemQuery = SetupQuery(entity.UserOpenAuthProvider, "UserOpenAuthProvider", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_MembershipProvider_UserOpenAuthProvider");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterUserOpenAuthProviderProviderID { get; set; }
			public string FilterUserOpenAuthProviderProviderName { get; set; }
			public string FilterUserOpenAuthProviderUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterUserOpenAuthProviderProviderID != null)
					filterList.Add("@FilterUserOpenAuthProviderProviderID", FilterUserOpenAuthProviderProviderID);
				if (FilterUserOpenAuthProviderProviderName != null)
					filterList.Add("@FilterUserOpenAuthProviderProviderName", FilterUserOpenAuthProviderProviderName);
				if (FilterUserOpenAuthProviderUserID != null)
				{
					if (FilterUserOpenAuthProviderUserID == string.Empty)
						filterList.Add("@FilterUserOpenAuthProviderUserID", string.Empty);
					else
						filterList.Add("@FilterUserOpenAuthProviderUserID", Convert.ToInt32(FilterUserOpenAuthProviderUserID));
				}
				return filterList;
			}
		}
	}
}