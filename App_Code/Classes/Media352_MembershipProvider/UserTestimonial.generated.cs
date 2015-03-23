using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_MembershipProvider
{
	public partial class UserTestimonial : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_MembershipProvider_UserTestimonial_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public UserTestimonial()
		{
		}

		public UserTestimonial(UserTestimonial objectToCopy)
		{
			GiverNameAndLocation = objectToCopy.GiverNameAndLocation;
			Testimonial = objectToCopy.Testimonial;
			UserID = objectToCopy.UserID;
			UserTestimonialID = objectToCopy.UserTestimonialID;
		}

		public virtual bool IsNewRecord
		{
			get { return UserTestimonialID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("UserTestimonial", this);
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

		public static UserTestimonial GetByID(int UserTestimonialID, IEnumerable<string> includeList = null)
		{
			UserTestimonial obj = null;
			string key = cacheKeyPrefix + UserTestimonialID + GetCacheIncludeText(includeList);

			UserTestimonial tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as UserTestimonial;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserTestimonial> itemQuery = AddIncludes(entity.UserTestimonial, includeList);
					obj = itemQuery.FirstOrDefault(n => n.UserTestimonialID == UserTestimonialID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<UserTestimonial> GetAll(IEnumerable<string> includeList = null)
		{
			List<UserTestimonial> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<UserTestimonial> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserTestimonial>();
				tmpList = Cache[key] as List<UserTestimonial>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserTestimonial> itemQuery = AddIncludes(entity.UserTestimonial, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<UserTestimonial> UserTestimonialGetByUserID(Int32 UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterUserTestimonialUserID = UserID.ToString();
			return UserTestimonialPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<UserTestimonial> UserTestimonialPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<UserTestimonial> objects = UserTestimonialPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<UserTestimonial> UserTestimonialPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return UserTestimonialPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<UserTestimonial> UserTestimonialPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return UserTestimonialPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<UserTestimonial> UserTestimonialPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "UserTestimonialID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<UserTestimonial> objects;
			string baseKey = cacheKeyPrefix + "UserTestimonialPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<UserTestimonial> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<UserTestimonial>();
				tmpList = Cache[key] as List<UserTestimonial>;
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
					IQueryable<UserTestimonial> itemQuery = SetupQuery(entity.UserTestimonial, "UserTestimonial", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_MembershipProvider_UserTestimonial");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterUserTestimonialUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterUserTestimonialUserID != null)
				{
					if (FilterUserTestimonialUserID == string.Empty)
						filterList.Add("@FilterUserTestimonialUserID", string.Empty);
					else
						filterList.Add("@FilterUserTestimonialUserID", Convert.ToInt32(FilterUserTestimonialUserID));
				}
				return filterList;
			}
		}
	}
}