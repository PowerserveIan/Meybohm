using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_MembershipProvider
{
	public partial class User : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_MembershipProvider_User_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Email", "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public User(User objectToCopy)
		{
			ApplicationID = objectToCopy.ApplicationID;
			ChangePasswordID = objectToCopy.ChangePasswordID;
			Created = objectToCopy.Created;
			Deleted = objectToCopy.Deleted;
			Email = objectToCopy.Email;
			FailedAnswerAttempts = objectToCopy.FailedAnswerAttempts;
			FailedPasswordAttempts = objectToCopy.FailedPasswordAttempts;
			IsApproved = objectToCopy.IsApproved;
			LastActivity = objectToCopy.LastActivity;
			LastLockout = objectToCopy.LastLockout;
			LastLogin = objectToCopy.LastLogin;
			LastPasswordChange = objectToCopy.LastPasswordChange;
			Locked = objectToCopy.Locked;
			Name = objectToCopy.Name;
			Online = objectToCopy.Online;
			Password = objectToCopy.Password;
			PasswordAnswer = objectToCopy.PasswordAnswer;
			PasswordFormat = objectToCopy.PasswordFormat;
			PasswordQuestion = objectToCopy.PasswordQuestion;
			Salt = objectToCopy.Salt;
			UniqueEmail = objectToCopy.UniqueEmail;
			UserID = objectToCopy.UserID;
		}

		public virtual bool IsNewRecord
		{
			get { return UserID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime CreatedClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(Created); }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime LastActivityClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(LastActivity); }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime? LastLockoutClientTime
		{
			get 
			{
				if (LastLockout.HasValue)
					return Helpers.ConvertUTCToClientTime(LastLockout.Value);
				return null;
			}
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime LastLoginClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(LastLogin); }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime LastPasswordChangeClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(LastPasswordChange); }
		}

		public virtual void Save()
		{
			SaveEntity("User", this);
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

		public static User GetByID(int UserID, IEnumerable<string> includeList = null)
		{
			User obj = null;
			string key = cacheKeyPrefix + UserID + GetCacheIncludeText(includeList);

			User tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as User;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<User> itemQuery = AddIncludes(entity.User, includeList);
					obj = itemQuery.FirstOrDefault(n => n.UserID == UserID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<User> GetAll(IEnumerable<string> includeList = null)
		{
			List<User> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<User> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<User>();
				tmpList = Cache[key] as List<User>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<User> itemQuery = AddIncludes(entity.User, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<User> UserGetByChangePasswordID(Guid? ChangePasswordID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserChangePasswordID = ChangePasswordID.ToString();
			return UserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<User> UserGetByDeleted(Boolean Deleted, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserDeleted = Deleted.ToString();
			return UserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<User> UserGetByEmail(String Email, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserEmail = Email.ToString();
			return UserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<User> UserGetByIsApproved(Boolean IsApproved, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserIsApproved = IsApproved.ToString();
			return UserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<User> UserGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserName = Name.ToString();
			return UserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<User> UserGetByOnline(Boolean Online, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserOnline = Online.ToString();
			return UserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<User> UserPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<User> objects = UserPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<User> UserPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return UserPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<User> UserPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return UserPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<User> UserPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "UserID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<User> objects;
			string baseKey = cacheKeyPrefix + "UserPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<User> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<User>();
				tmpList = Cache[key] as List<User>;
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
					IQueryable<User> itemQuery = SetupQuery(entity.User, "User", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_MembershipProvider_User");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterUserChangePasswordID { get; set; }
			public string FilterUserDeleted { get; set; }
			public string FilterUserEmail { get; set; }
			public string FilterUserIsApproved { get; set; }
			public string FilterUserName { get; set; }
			public string FilterUserOnline { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterUserChangePasswordID != null)
				{
					if (FilterUserChangePasswordID == string.Empty)
						filterList.Add("@FilterUserChangePasswordID", string.Empty);
					else
						filterList.Add("@FilterUserChangePasswordID", new Guid(FilterUserChangePasswordID));
				}
				if (FilterUserDeleted != null)
				{
					if (FilterUserDeleted == string.Empty)
						filterList.Add("@FilterUserDeleted", string.Empty);
					else
						filterList.Add("@FilterUserDeleted", Convert.ToBoolean(FilterUserDeleted));
				}
				if (FilterUserEmail != null)
					filterList.Add("@FilterUserEmail", FilterUserEmail);
				if (FilterUserIsApproved != null)
				{
					if (FilterUserIsApproved == string.Empty)
						filterList.Add("@FilterUserIsApproved", string.Empty);
					else
						filterList.Add("@FilterUserIsApproved", Convert.ToBoolean(FilterUserIsApproved));
				}
				if (FilterUserName != null)
					filterList.Add("@FilterUserName", FilterUserName);
				if (FilterUserOnline != null)
				{
					if (FilterUserOnline == string.Empty)
						filterList.Add("@FilterUserOnline", string.Empty);
					else
						filterList.Add("@FilterUserOnline", Convert.ToBoolean(FilterUserOnline));
				}
				return filterList;
			}
		}
	}
}