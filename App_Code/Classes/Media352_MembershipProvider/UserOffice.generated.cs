using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_MembershipProvider
{
	public partial class UserOffice : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_MembershipProvider_UserOffice_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "MlsID"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public UserOffice()
		{
		}

		public UserOffice(UserOffice objectToCopy)
		{
			MlsID = objectToCopy.MlsID;
			OfficeID = objectToCopy.OfficeID;
			UserID = objectToCopy.UserID;
			UserOfficeID = objectToCopy.UserOfficeID;
		}

		public virtual bool IsNewRecord
		{
			get { return UserOfficeID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("UserOffice", this);
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

		public static UserOffice GetByID(int UserOfficeID, IEnumerable<string> includeList = null)
		{
			UserOffice obj = null;
			string key = cacheKeyPrefix + UserOfficeID + GetCacheIncludeText(includeList);

			UserOffice tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as UserOffice;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserOffice> itemQuery = AddIncludes(entity.UserOffice, includeList);
					obj = itemQuery.FirstOrDefault(n => n.UserOfficeID == UserOfficeID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<UserOffice> GetAll(IEnumerable<string> includeList = null)
		{
			List<UserOffice> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<UserOffice> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserOffice>();
				tmpList = Cache[key] as List<UserOffice>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserOffice> itemQuery = AddIncludes(entity.UserOffice, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<UserOffice> UserOfficeGetByMlsID(String MlsID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserOfficeMlsID = MlsID.ToString();
			return UserOfficePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<UserOffice> UserOfficeGetByOfficeID(Int32 OfficeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserOfficeOfficeID = OfficeID.ToString();
			return UserOfficePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<UserOffice> UserOfficeGetByUserID(Int32 UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserOfficeUserID = UserID.ToString();
			return UserOfficePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<UserOffice> UserOfficePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<UserOffice> objects = UserOfficePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<UserOffice> UserOfficePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return UserOfficePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<UserOffice> UserOfficePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return UserOfficePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<UserOffice> UserOfficePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "UserOfficeID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<UserOffice> objects;
			string baseKey = cacheKeyPrefix + "UserOfficePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<UserOffice> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserOffice>();
				tmpList = Cache[key] as List<UserOffice>;
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
					IQueryable<UserOffice> itemQuery = SetupQuery(entity.UserOffice, "UserOffice", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_MembershipProvider_UserOffice");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterUserOfficeMlsID { get; set; }
			public string FilterUserOfficeOfficeID { get; set; }
			public string FilterUserOfficeUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterUserOfficeMlsID != null)
					filterList.Add("@FilterUserOfficeMlsID", FilterUserOfficeMlsID);
				if (FilterUserOfficeOfficeID != null)
				{
					if (FilterUserOfficeOfficeID == string.Empty)
						filterList.Add("@FilterUserOfficeOfficeID", string.Empty);
					else
						filterList.Add("@FilterUserOfficeOfficeID", Convert.ToInt32(FilterUserOfficeOfficeID));
				}
				if (FilterUserOfficeUserID != null)
				{
					if (FilterUserOfficeUserID == string.Empty)
						filterList.Add("@FilterUserOfficeUserID", string.Empty);
					else
						filterList.Add("@FilterUserOfficeUserID", Convert.ToInt32(FilterUserOfficeUserID));
				}
				return filterList;
			}
		}
	}
}