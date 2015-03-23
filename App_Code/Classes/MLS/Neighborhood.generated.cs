using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.MLS
{
	public partial class Neighborhood : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "MLS_Neighborhood_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public Neighborhood(Neighborhood objectToCopy)
		{
			Active = objectToCopy.Active;
			AddressID = objectToCopy.AddressID;
			Amenities = objectToCopy.Amenities;
			CMMicrositeID = objectToCopy.CMMicrositeID;
			Directions = objectToCopy.Directions;
			Featured = objectToCopy.Featured;
			Image = objectToCopy.Image;
			Name = objectToCopy.Name;
			NeighborhoodID = objectToCopy.NeighborhoodID;
			Overview = objectToCopy.Overview;
			Phone = objectToCopy.Phone;
			PriceRange = objectToCopy.PriceRange;
			ShowLotsLand = objectToCopy.ShowLotsLand;
			Website = objectToCopy.Website;
		}

		public virtual bool IsNewRecord
		{
			get { return NeighborhoodID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("Neighborhood", this);
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

		public static Neighborhood GetByID(int NeighborhoodID, IEnumerable<string> includeList = null)
		{
			Neighborhood obj = null;
			string key = cacheKeyPrefix + NeighborhoodID + GetCacheIncludeText(includeList);

			Neighborhood tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Neighborhood;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Neighborhood> itemQuery = AddIncludes(entity.Neighborhood, includeList);
					obj = itemQuery.FirstOrDefault(n => n.NeighborhoodID == NeighborhoodID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Neighborhood> GetAll(IEnumerable<string> includeList = null)
		{
			List<Neighborhood> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Neighborhood> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Neighborhood>();
				tmpList = Cache[key] as List<Neighborhood>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Neighborhood> itemQuery = AddIncludes(entity.Neighborhood, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Neighborhood> NeighborhoodGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNeighborhoodActive = Active.ToString();
			return NeighborhoodPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Neighborhood> NeighborhoodGetByCMMicrositeID(Int32 CMMicrositeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNeighborhoodCMMicrositeID = CMMicrositeID.ToString();
			return NeighborhoodPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Neighborhood> NeighborhoodGetByFeatured(Boolean Featured, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNeighborhoodFeatured = Featured.ToString();
			return NeighborhoodPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Neighborhood> NeighborhoodGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNeighborhoodName = Name.ToString();
			return NeighborhoodPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Neighborhood> NeighborhoodPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Neighborhood> objects = NeighborhoodPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Neighborhood> NeighborhoodPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return NeighborhoodPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Neighborhood> NeighborhoodPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return NeighborhoodPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Neighborhood> NeighborhoodPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "NeighborhoodID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Neighborhood> objects;
			string baseKey = cacheKeyPrefix + "NeighborhoodPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Neighborhood> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Neighborhood>();
				tmpList = Cache[key] as List<Neighborhood>;
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
					IQueryable<Neighborhood> itemQuery = SetupQuery(entity.Neighborhood, "Neighborhood", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("MLS_Neighborhood");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterNeighborhoodActive { get; set; }
			public string FilterNeighborhoodCMMicrositeID { get; set; }
			public string FilterNeighborhoodFeatured { get; set; }
			public string FilterNeighborhoodName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterNeighborhoodActive != null)
				{
					if (FilterNeighborhoodActive == string.Empty)
						filterList.Add("@FilterNeighborhoodActive", string.Empty);
					else
						filterList.Add("@FilterNeighborhoodActive", Convert.ToBoolean(FilterNeighborhoodActive));
				}
				if (FilterNeighborhoodCMMicrositeID != null)
				{
					if (FilterNeighborhoodCMMicrositeID == string.Empty)
						filterList.Add("@FilterNeighborhoodCMMicrositeID", string.Empty);
					else
						filterList.Add("@FilterNeighborhoodCMMicrositeID", Convert.ToInt32(FilterNeighborhoodCMMicrositeID));
				}
				if (FilterNeighborhoodFeatured != null)
				{
					if (FilterNeighborhoodFeatured == string.Empty)
						filterList.Add("@FilterNeighborhoodFeatured", string.Empty);
					else
						filterList.Add("@FilterNeighborhoodFeatured", Convert.ToBoolean(FilterNeighborhoodFeatured));
				}
				if (FilterNeighborhoodName != null)
					filterList.Add("@FilterNeighborhoodName", FilterNeighborhoodName);
				return filterList;
			}
		}
	}
}