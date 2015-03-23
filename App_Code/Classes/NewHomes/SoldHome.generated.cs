using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.NewHomes
{
	public partial class SoldHome : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "NewHomes_SoldHome_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public SoldHome()
		{
		}

		public SoldHome(SoldHome objectToCopy)
		{
			CloseDate = objectToCopy.CloseDate;
			ListingAgentID = objectToCopy.ListingAgentID;
			SalePrice = objectToCopy.SalePrice;
			SalesAgentID = objectToCopy.SalesAgentID;
			SalesAgentPercentage = objectToCopy.SalesAgentPercentage;
			SellerOfficeID = objectToCopy.SellerOfficeID;
			SellerOfficePercentage = objectToCopy.SellerOfficePercentage;
			SellerPercentage = objectToCopy.SellerPercentage;
			ShowcaseItemID = objectToCopy.ShowcaseItemID;
			SoldHomeID = objectToCopy.SoldHomeID;
		}

		public virtual bool IsNewRecord
		{
			get { return SoldHomeID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime CloseDateClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(CloseDate); }
		}

		public virtual void Save()
		{
			SaveEntity("SoldHome", this);
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

		public static SoldHome GetByID(int SoldHomeID, IEnumerable<string> includeList = null)
		{
			SoldHome obj = null;
			string key = cacheKeyPrefix + SoldHomeID + GetCacheIncludeText(includeList);

			SoldHome tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as SoldHome;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SoldHome> itemQuery = AddIncludes(entity.SoldHome, includeList);
					obj = itemQuery.FirstOrDefault(n => n.SoldHomeID == SoldHomeID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<SoldHome> GetAll(IEnumerable<string> includeList = null)
		{
			List<SoldHome> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<SoldHome> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SoldHome>();
				tmpList = Cache[key] as List<SoldHome>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SoldHome> itemQuery = AddIncludes(entity.SoldHome, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<SoldHome> SoldHomeGetByListingAgentID(Int32? ListingAgentID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSoldHomeListingAgentID = ListingAgentID.ToString();
			return SoldHomePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SoldHome> SoldHomeGetBySalesAgentID(Int32? SalesAgentID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSoldHomeSalesAgentID = SalesAgentID.ToString();
			return SoldHomePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SoldHome> SoldHomeGetBySellerOfficeID(Int32? SellerOfficeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSoldHomeSellerOfficeID = SellerOfficeID.ToString();
			return SoldHomePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SoldHome> SoldHomeGetByShowcaseItemID(Int32 ShowcaseItemID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSoldHomeShowcaseItemID = ShowcaseItemID.ToString();
			return SoldHomePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<SoldHome> SoldHomePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<SoldHome> objects = SoldHomePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<SoldHome> SoldHomePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return SoldHomePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<SoldHome> SoldHomePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return SoldHomePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<SoldHome> SoldHomePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SoldHomeID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<SoldHome> objects;
			string baseKey = cacheKeyPrefix + "SoldHomePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<SoldHome> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SoldHome>();
				tmpList = Cache[key] as List<SoldHome>;
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
					IQueryable<SoldHome> itemQuery = SetupQuery(entity.SoldHome, "SoldHome", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("NewHomes_SoldHome");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterSoldHomeListingAgentID { get; set; }
			public string FilterSoldHomeSalesAgentID { get; set; }
			public string FilterSoldHomeSellerOfficeID { get; set; }
			public string FilterSoldHomeShowcaseItemID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterSoldHomeListingAgentID != null)
				{
					if (FilterSoldHomeListingAgentID == string.Empty)
						filterList.Add("@FilterSoldHomeListingAgentID", string.Empty);
					else
						filterList.Add("@FilterSoldHomeListingAgentID", Convert.ToInt32(FilterSoldHomeListingAgentID));
				}
				if (FilterSoldHomeSalesAgentID != null)
				{
					if (FilterSoldHomeSalesAgentID == string.Empty)
						filterList.Add("@FilterSoldHomeSalesAgentID", string.Empty);
					else
						filterList.Add("@FilterSoldHomeSalesAgentID", Convert.ToInt32(FilterSoldHomeSalesAgentID));
				}
				if (FilterSoldHomeSellerOfficeID != null)
				{
					if (FilterSoldHomeSellerOfficeID == string.Empty)
						filterList.Add("@FilterSoldHomeSellerOfficeID", string.Empty);
					else
						filterList.Add("@FilterSoldHomeSellerOfficeID", Convert.ToInt32(FilterSoldHomeSellerOfficeID));
				}
				if (FilterSoldHomeShowcaseItemID != null)
				{
					if (FilterSoldHomeShowcaseItemID == string.Empty)
						filterList.Add("@FilterSoldHomeShowcaseItemID", string.Empty);
					else
						filterList.Add("@FilterSoldHomeShowcaseItemID", Convert.ToInt32(FilterSoldHomeShowcaseItemID));
				}
				return filterList;
			}
		}
	}
}