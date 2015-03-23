using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class CMMicrosite : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ContentManager_CMMicrosite_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Description", "Location", "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public CMMicrosite(CMMicrosite objectToCopy)
		{
			Active = objectToCopy.Active;
			CMMicroSiteID = objectToCopy.CMMicroSiteID;
			Description = objectToCopy.Description;
			Image = objectToCopy.Image;
			Location = objectToCopy.Location;
			Name = objectToCopy.Name;
			Phone = objectToCopy.Phone;
			Published = objectToCopy.Published;
		}

		public virtual bool IsNewRecord
		{
			get { return CMMicroSiteID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("CMMicrosite", this);
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

		public static CMMicrosite GetByID(int CMMicroSiteID, IEnumerable<string> includeList = null)
		{
			CMMicrosite obj = null;
			string key = cacheKeyPrefix + CMMicroSiteID + GetCacheIncludeText(includeList);

			CMMicrosite tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as CMMicrosite;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMMicrosite> itemQuery = AddIncludes(entity.CMMicrosite, includeList);
					obj = itemQuery.FirstOrDefault(n => n.CMMicroSiteID == CMMicroSiteID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<CMMicrosite> GetAll(IEnumerable<string> includeList = null)
		{
			List<CMMicrosite> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<CMMicrosite> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMMicrosite>();
				tmpList = Cache[key] as List<CMMicrosite>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMMicrosite> itemQuery = AddIncludes(entity.CMMicrosite, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<CMMicrosite> CMMicrositeGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMMicrositeActive = Active.ToString();
			return CMMicrositePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMMicrosite> CMMicrositeGetByDescription(String Description, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMMicrositeDescription = Description.ToString();
			return CMMicrositePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMMicrosite> CMMicrositeGetByLocation(String Location, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMMicrositeLocation = Location.ToString();
			return CMMicrositePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMMicrosite> CMMicrositeGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMMicrositeName = Name.ToString();
			return CMMicrositePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMMicrosite> CMMicrositeGetByPublished(Boolean Published, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMMicrositePublished = Published.ToString();
			return CMMicrositePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<CMMicrosite> CMMicrositePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<CMMicrosite> objects = CMMicrositePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<CMMicrosite> CMMicrositePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return CMMicrositePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<CMMicrosite> CMMicrositePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return CMMicrositePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<CMMicrosite> CMMicrositePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "CMMicroSiteID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<CMMicrosite> objects;
			string baseKey = cacheKeyPrefix + "CMMicrositePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<CMMicrosite> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMMicrosite>();
				tmpList = Cache[key] as List<CMMicrosite>;
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
					IQueryable<CMMicrosite> itemQuery = SetupQuery(entity.CMMicrosite, "CMMicrosite", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ContentManager_CMMicrosite");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterCMMicrositeActive { get; set; }
			public string FilterCMMicrositeDescription { get; set; }
			public string FilterCMMicrositeLocation { get; set; }
			public string FilterCMMicrositeName { get; set; }
			public string FilterCMMicrositePublished { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterCMMicrositeActive != null)
				{
					if (FilterCMMicrositeActive == string.Empty)
						filterList.Add("@FilterCMMicrositeActive", string.Empty);
					else
						filterList.Add("@FilterCMMicrositeActive", Convert.ToBoolean(FilterCMMicrositeActive));
				}
				if (FilterCMMicrositeDescription != null)
					filterList.Add("@FilterCMMicrositeDescription", FilterCMMicrositeDescription);
				if (FilterCMMicrositeLocation != null)
					filterList.Add("@FilterCMMicrositeLocation", FilterCMMicrositeLocation);
				if (FilterCMMicrositeName != null)
					filterList.Add("@FilterCMMicrositeName", FilterCMMicrositeName);
				if (FilterCMMicrositePublished != null)
				{
					if (FilterCMMicrositePublished == string.Empty)
						filterList.Add("@FilterCMMicrositePublished", string.Empty);
					else
						filterList.Add("@FilterCMMicrositePublished", Convert.ToBoolean(FilterCMMicrositePublished));
				}
				return filterList;
			}
		}
	}
}