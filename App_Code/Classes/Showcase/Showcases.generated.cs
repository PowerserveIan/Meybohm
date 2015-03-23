using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class Showcases : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_Showcases_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Title"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public Showcases(Showcases objectToCopy)
		{
			Active = objectToCopy.Active;
			CMMicrositeID = objectToCopy.CMMicrositeID;
			MLSData = objectToCopy.MLSData;
			ShowcaseID = objectToCopy.ShowcaseID;
			Title = objectToCopy.Title;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("Showcases", this);
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

		public static Showcases GetByID(int ShowcaseID, IEnumerable<string> includeList = null)
		{
			Showcases obj = null;
			string key = cacheKeyPrefix + ShowcaseID + GetCacheIncludeText(includeList);

			Showcases tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Showcases;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Showcases> itemQuery = AddIncludes(entity.Showcases, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseID == ShowcaseID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Showcases> GetAll(IEnumerable<string> includeList = null)
		{
			List<Showcases> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Showcases> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Showcases>();
				tmpList = Cache[key] as List<Showcases>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Showcases> itemQuery = AddIncludes(entity.Showcases, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Showcases> ShowcasesGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcasesActive = Active.ToString();
			return ShowcasesPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Showcases> ShowcasesGetByMLSData(Boolean MLSData, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcasesMLSData = MLSData.ToString();
			return ShowcasesPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Showcases> ShowcasesGetByTitle(String Title, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcasesTitle = Title.ToString();
			return ShowcasesPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Showcases> ShowcasesPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Showcases> objects = ShowcasesPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Showcases> ShowcasesPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return ShowcasesPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Showcases> ShowcasesPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return ShowcasesPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Showcases> ShowcasesPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Showcases> objects;
			string baseKey = cacheKeyPrefix + "ShowcasesPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Showcases> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Showcases>();
				tmpList = Cache[key] as List<Showcases>;
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
					IQueryable<Showcases> itemQuery = SetupQuery(entity.Showcases, "Showcases", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_Showcases");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterShowcasesActive { get; set; }
			public string FilterShowcasesMLSData { get; set; }
			public string FilterShowcasesTitle { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterShowcasesActive != null)
				{
					if (FilterShowcasesActive == string.Empty)
						filterList.Add("@FilterShowcasesActive", string.Empty);
					else
						filterList.Add("@FilterShowcasesActive", Convert.ToBoolean(FilterShowcasesActive));
				}
				if (FilterShowcasesMLSData != null)
				{
					if (FilterShowcasesMLSData == string.Empty)
						filterList.Add("@FilterShowcasesMLSData", string.Empty);
					else
						filterList.Add("@FilterShowcasesMLSData", Convert.ToBoolean(FilterShowcasesMLSData));
				}
				if (FilterShowcasesTitle != null)
					filterList.Add("@FilterShowcasesTitle", FilterShowcasesTitle);
				return filterList;
			}
		}
	}
}