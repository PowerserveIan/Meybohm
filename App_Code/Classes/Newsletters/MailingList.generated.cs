using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Newsletters
{
	public partial class MailingList : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Newsletters_MailingList_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public MailingList(MailingList objectToCopy)
		{
			Active = objectToCopy.Active;
			Deleted = objectToCopy.Deleted;
			MailingListID = objectToCopy.MailingListID;
			Name = objectToCopy.Name;
		}

		public virtual bool IsNewRecord
		{
			get { return MailingListID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("MailingList", this);
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

		public static MailingList GetByID(int MailingListID, IEnumerable<string> includeList = null)
		{
			MailingList obj = null;
			string key = cacheKeyPrefix + MailingListID + GetCacheIncludeText(includeList);

			MailingList tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as MailingList;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<MailingList> itemQuery = AddIncludes(entity.MailingList, includeList);
					obj = itemQuery.FirstOrDefault(n => n.MailingListID == MailingListID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<MailingList> GetAll(IEnumerable<string> includeList = null)
		{
			List<MailingList> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<MailingList> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<MailingList>();
				tmpList = Cache[key] as List<MailingList>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<MailingList> itemQuery = AddIncludes(entity.MailingList, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<MailingList> MailingListGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMailingListActive = Active.ToString();
			return MailingListPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<MailingList> MailingListGetByDeleted(Boolean Deleted, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMailingListDeleted = Deleted.ToString();
			return MailingListPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<MailingList> MailingListGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMailingListName = Name.ToString();
			return MailingListPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<MailingList> MailingListPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<MailingList> objects = MailingListPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<MailingList> MailingListPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return MailingListPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<MailingList> MailingListPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return MailingListPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<MailingList> MailingListPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "MailingListID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<MailingList> objects;
			string baseKey = cacheKeyPrefix + "MailingListPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<MailingList> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<MailingList>();
				tmpList = Cache[key] as List<MailingList>;
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
					IQueryable<MailingList> itemQuery = SetupQuery(entity.MailingList, "MailingList", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Newsletters_MailingList");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterMailingListActive { get; set; }
			public string FilterMailingListDeleted { get; set; }
			public string FilterMailingListName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterMailingListActive != null)
				{
					if (FilterMailingListActive == string.Empty)
						filterList.Add("@FilterMailingListActive", string.Empty);
					else
						filterList.Add("@FilterMailingListActive", Convert.ToBoolean(FilterMailingListActive));
				}
				if (FilterMailingListDeleted != null)
				{
					if (FilterMailingListDeleted == string.Empty)
						filterList.Add("@FilterMailingListDeleted", string.Empty);
					else
						filterList.Add("@FilterMailingListDeleted", Convert.ToBoolean(FilterMailingListDeleted));
				}
				if (FilterMailingListName != null)
					filterList.Add("@FilterMailingListName", FilterMailingListName);
				return filterList;
			}
		}
	}
}