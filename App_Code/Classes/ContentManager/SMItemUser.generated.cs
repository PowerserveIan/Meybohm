using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class SMItemUser : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ContentManager_SMItemUser_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public SMItemUser()
		{
		}

		public SMItemUser(SMItemUser objectToCopy)
		{
			CMMicrositeID = objectToCopy.CMMicrositeID;
			LanguageID = objectToCopy.LanguageID;
			MicrositeDefault = objectToCopy.MicrositeDefault;
			SMItemUserID = objectToCopy.SMItemUserID;
			UserID = objectToCopy.UserID;
		}

		public virtual bool IsNewRecord
		{
			get { return SMItemUserID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("SMItemUser", this);
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

		public static SMItemUser GetByID(int SMItemUserID, IEnumerable<string> includeList = null)
		{
			SMItemUser obj = null;
			string key = cacheKeyPrefix + SMItemUserID + GetCacheIncludeText(includeList);

			SMItemUser tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as SMItemUser;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SMItemUser> itemQuery = AddIncludes(entity.SMItemUser, includeList);
					obj = itemQuery.FirstOrDefault(n => n.SMItemUserID == SMItemUserID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<SMItemUser> GetAll(IEnumerable<string> includeList = null)
		{
			List<SMItemUser> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<SMItemUser> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SMItemUser>();
				tmpList = Cache[key] as List<SMItemUser>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SMItemUser> itemQuery = AddIncludes(entity.SMItemUser, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<SMItemUser> SMItemUserGetByCMMicrositeID(Int32? CMMicrositeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemUserCMMicrositeID = CMMicrositeID.ToString();
			return SMItemUserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItemUser> SMItemUserGetByLanguageID(Int32? LanguageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemUserLanguageID = LanguageID.ToString();
			return SMItemUserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItemUser> SMItemUserGetByMicrositeDefault(Boolean MicrositeDefault, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemUserMicrositeDefault = MicrositeDefault.ToString();
			return SMItemUserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItemUser> SMItemUserGetByUserID(Int32 UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemUserUserID = UserID.ToString();
			return SMItemUserPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<SMItemUser> SMItemUserPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<SMItemUser> objects = SMItemUserPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<SMItemUser> SMItemUserPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return SMItemUserPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<SMItemUser> SMItemUserPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return SMItemUserPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<SMItemUser> SMItemUserPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SMItemUserID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<SMItemUser> objects;
			string baseKey = cacheKeyPrefix + "SMItemUserPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<SMItemUser> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SMItemUser>();
				tmpList = Cache[key] as List<SMItemUser>;
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
					IQueryable<SMItemUser> itemQuery = SetupQuery(entity.SMItemUser, "SMItemUser", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ContentManager_SMItemUser");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterSMItemUserCMMicrositeID { get; set; }
			public string FilterSMItemUserLanguageID { get; set; }
			public string FilterSMItemUserMicrositeDefault { get; set; }
			public string FilterSMItemUserUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterSMItemUserCMMicrositeID != null)
				{
					if (FilterSMItemUserCMMicrositeID == string.Empty)
						filterList.Add("@FilterSMItemUserCMMicrositeID", string.Empty);
					else
						filterList.Add("@FilterSMItemUserCMMicrositeID", Convert.ToInt32(FilterSMItemUserCMMicrositeID));
				}
				if (FilterSMItemUserLanguageID != null)
				{
					if (FilterSMItemUserLanguageID == string.Empty)
						filterList.Add("@FilterSMItemUserLanguageID", string.Empty);
					else
						filterList.Add("@FilterSMItemUserLanguageID", Convert.ToInt32(FilterSMItemUserLanguageID));
				}
				if (FilterSMItemUserMicrositeDefault != null)
				{
					if (FilterSMItemUserMicrositeDefault == string.Empty)
						filterList.Add("@FilterSMItemUserMicrositeDefault", string.Empty);
					else
						filterList.Add("@FilterSMItemUserMicrositeDefault", Convert.ToBoolean(FilterSMItemUserMicrositeDefault));
				}
				if (FilterSMItemUserUserID != null)
				{
					if (FilterSMItemUserUserID == string.Empty)
						filterList.Add("@FilterSMItemUserUserID", string.Empty);
					else
						filterList.Add("@FilterSMItemUserUserID", Convert.ToInt32(FilterSMItemUserUserID));
				}
				return filterList;
			}
		}
	}
}