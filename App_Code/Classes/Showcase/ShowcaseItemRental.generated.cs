using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class ShowcaseItemRental : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_ShowcaseItemRental_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public ShowcaseItemRental()
		{
		}

		public ShowcaseItemRental(ShowcaseItemRental objectToCopy)
		{
			CompanyName = objectToCopy.CompanyName;
			ContactName = objectToCopy.ContactName;
			ContactPhone = objectToCopy.ContactPhone;
			LeaseBeginDate = objectToCopy.LeaseBeginDate;
			OwnerName = objectToCopy.OwnerName;
			ShowcaseItemID = objectToCopy.ShowcaseItemID;
			ShowcaseItemRentalID = objectToCopy.ShowcaseItemRentalID;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseItemRentalID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime? LeaseBeginDateClientTime
		{
			get 
			{
				if (LeaseBeginDate.HasValue)
					return Helpers.ConvertUTCToClientTime(LeaseBeginDate.Value);
				return null;
			}
		}

		public virtual void Save()
		{
			SaveEntity("ShowcaseItemRental", this);
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

		public static ShowcaseItemRental GetByID(int ShowcaseItemRentalID, IEnumerable<string> includeList = null)
		{
			ShowcaseItemRental obj = null;
			string key = cacheKeyPrefix + ShowcaseItemRentalID + GetCacheIncludeText(includeList);

			ShowcaseItemRental tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as ShowcaseItemRental;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItemRental> itemQuery = AddIncludes(entity.ShowcaseItemRental, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseItemRentalID == ShowcaseItemRentalID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<ShowcaseItemRental> GetAll(IEnumerable<string> includeList = null)
		{
			List<ShowcaseItemRental> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<ShowcaseItemRental> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseItemRental>();
				tmpList = Cache[key] as List<ShowcaseItemRental>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItemRental> itemQuery = AddIncludes(entity.ShowcaseItemRental, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<ShowcaseItemRental> ShowcaseItemRentalGetByShowcaseItemID(Int32 ShowcaseItemID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemRentalShowcaseItemID = ShowcaseItemID.ToString();
			return ShowcaseItemRentalPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<ShowcaseItemRental> ShowcaseItemRentalPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<ShowcaseItemRental> objects = ShowcaseItemRentalPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseItemRental> ShowcaseItemRentalPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return ShowcaseItemRentalPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<ShowcaseItemRental> ShowcaseItemRentalPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return ShowcaseItemRentalPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<ShowcaseItemRental> ShowcaseItemRentalPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseItemRentalID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<ShowcaseItemRental> objects;
			string baseKey = cacheKeyPrefix + "ShowcaseItemRentalPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseItemRental> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseItemRental>();
				tmpList = Cache[key] as List<ShowcaseItemRental>;
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
					IQueryable<ShowcaseItemRental> itemQuery = SetupQuery(entity.ShowcaseItemRental, "ShowcaseItemRental", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_ShowcaseItemRental");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterShowcaseItemRentalShowcaseItemID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterShowcaseItemRentalShowcaseItemID != null)
				{
					if (FilterShowcaseItemRentalShowcaseItemID == string.Empty)
						filterList.Add("@FilterShowcaseItemRentalShowcaseItemID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemRentalShowcaseItemID", Convert.ToInt32(FilterShowcaseItemRentalShowcaseItemID));
				}
				return filterList;
			}
		}
	}
}