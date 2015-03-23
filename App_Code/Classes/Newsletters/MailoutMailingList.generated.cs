using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Newsletters
{
	public partial class MailoutMailingList : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Newsletters_MailoutMailingList_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public MailoutMailingList()
		{
		}

		public MailoutMailingList(MailoutMailingList objectToCopy)
		{
			MailingListID = objectToCopy.MailingListID;
			MailoutID = objectToCopy.MailoutID;
			MailoutMailingListID = objectToCopy.MailoutMailingListID;
		}

		public virtual bool IsNewRecord
		{
			get { return MailoutMailingListID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("MailoutMailingList", this);
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

		public static MailoutMailingList GetByID(int MailoutMailingListID, IEnumerable<string> includeList = null)
		{
			MailoutMailingList obj = null;
			string key = cacheKeyPrefix + MailoutMailingListID + GetCacheIncludeText(includeList);

			MailoutMailingList tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as MailoutMailingList;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<MailoutMailingList> itemQuery = AddIncludes(entity.MailoutMailingList, includeList);
					obj = itemQuery.FirstOrDefault(n => n.MailoutMailingListID == MailoutMailingListID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<MailoutMailingList> GetAll(IEnumerable<string> includeList = null)
		{
			List<MailoutMailingList> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<MailoutMailingList> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<MailoutMailingList>();
				tmpList = Cache[key] as List<MailoutMailingList>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<MailoutMailingList> itemQuery = AddIncludes(entity.MailoutMailingList, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<MailoutMailingList> MailoutMailingListGetByMailingListID(Int32 MailingListID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMailoutMailingListMailingListID = MailingListID.ToString();
			return MailoutMailingListPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<MailoutMailingList> MailoutMailingListGetByMailoutID(Int32 MailoutID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMailoutMailingListMailoutID = MailoutID.ToString();
			return MailoutMailingListPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<MailoutMailingList> MailoutMailingListPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<MailoutMailingList> objects = MailoutMailingListPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<MailoutMailingList> MailoutMailingListPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return MailoutMailingListPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<MailoutMailingList> MailoutMailingListPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return MailoutMailingListPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<MailoutMailingList> MailoutMailingListPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "MailoutMailingListID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<MailoutMailingList> objects;
			string baseKey = cacheKeyPrefix + "MailoutMailingListPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<MailoutMailingList> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<MailoutMailingList>();
				tmpList = Cache[key] as List<MailoutMailingList>;
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
					IQueryable<MailoutMailingList> itemQuery = SetupQuery(entity.MailoutMailingList, "MailoutMailingList", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Newsletters_MailoutMailingList");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterMailoutMailingListMailingListID { get; set; }
			public string FilterMailoutMailingListMailoutID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterMailoutMailingListMailingListID != null)
				{
					if (FilterMailoutMailingListMailingListID == string.Empty)
						filterList.Add("@FilterMailoutMailingListMailingListID", string.Empty);
					else
						filterList.Add("@FilterMailoutMailingListMailingListID", Convert.ToInt32(FilterMailoutMailingListMailingListID));
				}
				if (FilterMailoutMailingListMailoutID != null)
				{
					if (FilterMailoutMailingListMailoutID == string.Empty)
						filterList.Add("@FilterMailoutMailingListMailoutID", string.Empty);
					else
						filterList.Add("@FilterMailoutMailingListMailoutID", Convert.ToInt32(FilterMailoutMailingListMailoutID));
				}
				return filterList;
			}
		}
	}
}