using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_MembershipProvider
{
	public partial class UserInfo : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_MembershipProvider_UserInfo_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "FirstName", "LastName"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public UserInfo()
		{
		}

		public UserInfo(UserInfo objectToCopy)
		{
			AddressID = objectToCopy.AddressID;
			Biography = objectToCopy.Biography;
			CellPhone = objectToCopy.CellPhone;
			DisplayInDirectory = objectToCopy.DisplayInDirectory;
			Fax = objectToCopy.Fax;
			FirstName = objectToCopy.FirstName;
			HomePhone = objectToCopy.HomePhone;
			JobTitleID = objectToCopy.JobTitleID;
			LastName = objectToCopy.LastName;
			OfficePhone = objectToCopy.OfficePhone;
			Photo = objectToCopy.Photo;
			PreferredCMMicrositeID = objectToCopy.PreferredCMMicrositeID;
			PreferredLanguageID = objectToCopy.PreferredLanguageID;
			PrimaryPhone = objectToCopy.PrimaryPhone;
			Rating = objectToCopy.Rating;
			ShowListingLink = objectToCopy.ShowListingLink;
			ShowRatingOnSite = objectToCopy.ShowRatingOnSite;
			StaffTypeID = objectToCopy.StaffTypeID;
			UserID = objectToCopy.UserID;
			UserInfoID = objectToCopy.UserInfoID;
			Website = objectToCopy.Website;
		}

		public virtual bool IsNewRecord
		{
			get { return UserInfoID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("UserInfo", this);
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

		public static UserInfo GetByID(int UserInfoID, IEnumerable<string> includeList = null)
		{
			UserInfo obj = null;
			string key = cacheKeyPrefix + UserInfoID + GetCacheIncludeText(includeList);

			UserInfo tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as UserInfo;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserInfo> itemQuery = AddIncludes(entity.UserInfo, includeList);
					obj = itemQuery.FirstOrDefault(n => n.UserInfoID == UserInfoID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<UserInfo> GetAll(IEnumerable<string> includeList = null)
		{
			List<UserInfo> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<UserInfo> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserInfo>();
				tmpList = Cache[key] as List<UserInfo>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserInfo> itemQuery = AddIncludes(entity.UserInfo, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<UserInfo> UserInfoGetByDisplayInDirectory(Boolean DisplayInDirectory, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserInfoDisplayInDirectory = DisplayInDirectory.ToString();
			return UserInfoPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<UserInfo> UserInfoGetByFirstName(String FirstName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserInfoFirstName = FirstName.ToString();
			return UserInfoPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<UserInfo> UserInfoGetByLastName(String LastName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserInfoLastName = LastName.ToString();
			return UserInfoPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<UserInfo> UserInfoGetByStaffTypeID(Int32? StaffTypeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserInfoStaffTypeID = StaffTypeID.ToString();
			return UserInfoPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<UserInfo> UserInfoGetByUserID(Int32 UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserInfoUserID = UserID.ToString();
			return UserInfoPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<UserInfo> UserInfoPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<UserInfo> objects = UserInfoPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<UserInfo> UserInfoPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return UserInfoPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<UserInfo> UserInfoPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return UserInfoPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<UserInfo> UserInfoPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "UserInfoID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<UserInfo> objects;
			string baseKey = cacheKeyPrefix + "UserInfoPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<UserInfo> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserInfo>();
				tmpList = Cache[key] as List<UserInfo>;
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
					IQueryable<UserInfo> itemQuery = SetupQuery(entity.UserInfo, "UserInfo", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_MembershipProvider_UserInfo");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterUserInfoDisplayInDirectory { get; set; }
			public string FilterUserInfoFirstName { get; set; }
			public string FilterUserInfoLastName { get; set; }
			public string FilterUserInfoStaffTypeID { get; set; }
			public string FilterUserInfoUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterUserInfoDisplayInDirectory != null)
				{
					if (FilterUserInfoDisplayInDirectory == string.Empty)
						filterList.Add("@FilterUserInfoDisplayInDirectory", string.Empty);
					else
						filterList.Add("@FilterUserInfoDisplayInDirectory", Convert.ToBoolean(FilterUserInfoDisplayInDirectory));
				}
				if (FilterUserInfoFirstName != null)
					filterList.Add("@FilterUserInfoFirstName", FilterUserInfoFirstName);
				if (FilterUserInfoLastName != null)
					filterList.Add("@FilterUserInfoLastName", FilterUserInfoLastName);
				if (FilterUserInfoStaffTypeID != null)
				{
					if (FilterUserInfoStaffTypeID == string.Empty)
						filterList.Add("@FilterUserInfoStaffTypeID", string.Empty);
					else
						filterList.Add("@FilterUserInfoStaffTypeID", Convert.ToInt32(FilterUserInfoStaffTypeID));
				}
				if (FilterUserInfoUserID != null)
				{
					if (FilterUserInfoUserID == string.Empty)
						filterList.Add("@FilterUserInfoUserID", string.Empty);
					else
						filterList.Add("@FilterUserInfoUserID", Convert.ToInt32(FilterUserInfoUserID));
				}
				return filterList;
			}
		}
	}
}