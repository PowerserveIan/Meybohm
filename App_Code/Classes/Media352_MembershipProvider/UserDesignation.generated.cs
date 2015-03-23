using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_MembershipProvider
{
	public partial class UserDesignation : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_MembershipProvider_UserDesignation_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public UserDesignation()
		{
		}

		public UserDesignation(UserDesignation objectToCopy)
		{
			DesignationID = objectToCopy.DesignationID;
			UserDesignationID = objectToCopy.UserDesignationID;
			UserID = objectToCopy.UserID;
		}

		public virtual bool IsNewRecord
		{
			get { return UserDesignationID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("UserDesignation", this);
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

		public static UserDesignation GetByID(int UserDesignationID, IEnumerable<string> includeList = null)
		{
			UserDesignation obj = null;
			string key = cacheKeyPrefix + UserDesignationID + GetCacheIncludeText(includeList);

			UserDesignation tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as UserDesignation;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserDesignation> itemQuery = AddIncludes(entity.UserDesignation, includeList);
					obj = itemQuery.FirstOrDefault(n => n.UserDesignationID == UserDesignationID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<UserDesignation> GetAll(IEnumerable<string> includeList = null)
		{
			List<UserDesignation> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<UserDesignation> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserDesignation>();
				tmpList = Cache[key] as List<UserDesignation>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserDesignation> itemQuery = AddIncludes(entity.UserDesignation, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<UserDesignation> UserDesignationGetByDesignationID(Int32 DesignationID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserDesignationDesignationID = DesignationID.ToString();
			return UserDesignationPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<UserDesignation> UserDesignationGetByUserID(Int32 UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserDesignationUserID = UserID.ToString();
			return UserDesignationPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<UserDesignation> UserDesignationPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<UserDesignation> objects = UserDesignationPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<UserDesignation> UserDesignationPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return UserDesignationPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<UserDesignation> UserDesignationPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return UserDesignationPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<UserDesignation> UserDesignationPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "UserDesignationID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<UserDesignation> objects;
			string baseKey = cacheKeyPrefix + "UserDesignationPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<UserDesignation> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserDesignation>();
				tmpList = Cache[key] as List<UserDesignation>;
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
					IQueryable<UserDesignation> itemQuery = SetupQuery(entity.UserDesignation, "UserDesignation", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_MembershipProvider_UserDesignation");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterUserDesignationDesignationID { get; set; }
			public string FilterUserDesignationUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterUserDesignationDesignationID != null)
				{
					if (FilterUserDesignationDesignationID == string.Empty)
						filterList.Add("@FilterUserDesignationDesignationID", string.Empty);
					else
						filterList.Add("@FilterUserDesignationDesignationID", Convert.ToInt32(FilterUserDesignationDesignationID));
				}
				if (FilterUserDesignationUserID != null)
				{
					if (FilterUserDesignationUserID == string.Empty)
						filterList.Add("@FilterUserDesignationUserID", string.Empty);
					else
						filterList.Add("@FilterUserDesignationUserID", Convert.ToInt32(FilterUserDesignationUserID));
				}
				return filterList;
			}
		}
	}
}