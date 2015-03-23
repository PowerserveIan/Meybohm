using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class ShowcaseItem : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_ShowcaseItem_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Title"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public ShowcaseItem(ShowcaseItem objectToCopy)
		{
			Active = objectToCopy.Active;
			AddressID = objectToCopy.AddressID;
			AgentID = objectToCopy.AgentID;
			AvailabilityDate = objectToCopy.AvailabilityDate;
			BuilderID = objectToCopy.BuilderID;
			DateListed = objectToCopy.DateListed;
			Directions = objectToCopy.Directions;
			ElementarySchoolID = objectToCopy.ElementarySchoolID;
			EmailAddresses = objectToCopy.EmailAddresses;
			Featured = objectToCopy.Featured;
			HighSchoolID = objectToCopy.HighSchoolID;
			Image = objectToCopy.Image;
			ListPrice = objectToCopy.ListPrice;
			MiddleSchoolID = objectToCopy.MiddleSchoolID;
			MlsID = objectToCopy.MlsID;
			NeighborhoodID = objectToCopy.NeighborhoodID;
			NewHome = objectToCopy.NewHome;
			OfficeID = objectToCopy.OfficeID;
			OpenHouseAgentID = objectToCopy.OpenHouseAgentID;
			Rented = objectToCopy.Rented;
			ShowcaseID = objectToCopy.ShowcaseID;
			ShowcaseItemID = objectToCopy.ShowcaseItemID;
			StatsSentToAgent = objectToCopy.StatsSentToAgent;
			StatsSentToOwner = objectToCopy.StatsSentToOwner;
			Summary = objectToCopy.Summary;
			TeamID = objectToCopy.TeamID;
			tempOldID = objectToCopy.tempOldID;
			Title = objectToCopy.Title;
			VirtualTourURL = objectToCopy.VirtualTourURL;
			Website = objectToCopy.Website;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseItemID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime? AvailabilityDateClientTime
		{
			get 
			{
				if (AvailabilityDate.HasValue)
					return Helpers.ConvertUTCToClientTime(AvailabilityDate.Value);
				return null;
			}
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime? DateListedClientTime
		{
			get 
			{
				if (DateListed.HasValue)
					return Helpers.ConvertUTCToClientTime(DateListed.Value);
				return null;
			}
		}

		public virtual void Save()
		{
			SaveEntity("ShowcaseItem", this);
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

		public static ShowcaseItem GetByID(int ShowcaseItemID, IEnumerable<string> includeList = null)
		{
			ShowcaseItem obj = null;
			string key = cacheKeyPrefix + ShowcaseItemID + GetCacheIncludeText(includeList);

			ShowcaseItem tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as ShowcaseItem;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItem> itemQuery = AddIncludes(entity.ShowcaseItem, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseItemID == ShowcaseItemID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<ShowcaseItem> GetAll(IEnumerable<string> includeList = null)
		{
			List<ShowcaseItem> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<ShowcaseItem> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseItem>();
				tmpList = Cache[key] as List<ShowcaseItem>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItem> itemQuery = AddIncludes(entity.ShowcaseItem, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<ShowcaseItem> ShowcaseItemGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemActive = Active.ToString();
			return ShowcaseItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItem> ShowcaseItemGetByAgentID(Int32? AgentID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemAgentID = AgentID.ToString();
			return ShowcaseItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItem> ShowcaseItemGetByBuilderID(Int32? BuilderID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemBuilderID = BuilderID.ToString();
			return ShowcaseItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItem> ShowcaseItemGetByFeatured(Boolean Featured, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemFeatured = Featured.ToString();
			return ShowcaseItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItem> ShowcaseItemGetByMlsID(Int32? MlsID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemMlsID = MlsID.ToString();
			return ShowcaseItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItem> ShowcaseItemGetByNeighborhoodID(Int32? NeighborhoodID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemNeighborhoodID = NeighborhoodID.ToString();
			return ShowcaseItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItem> ShowcaseItemGetByNewHome(Boolean NewHome, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemNewHome = NewHome.ToString();
			return ShowcaseItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItem> ShowcaseItemGetByOfficeID(Int32? OfficeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemOfficeID = OfficeID.ToString();
			return ShowcaseItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItem> ShowcaseItemGetByRented(Boolean Rented, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemRented = Rented.ToString();
			return ShowcaseItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItem> ShowcaseItemGetByShowcaseID(Int32 ShowcaseID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemShowcaseID = ShowcaseID.ToString();
			return ShowcaseItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItem> ShowcaseItemGetByStatsSentToOwner(Boolean StatsSentToOwner, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemStatsSentToOwner = StatsSentToOwner.ToString();
			return ShowcaseItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItem> ShowcaseItemGetByTitle(String Title, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemTitle = Title.ToString();
			return ShowcaseItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<ShowcaseItem> ShowcaseItemPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<ShowcaseItem> objects = ShowcaseItemPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseItem> ShowcaseItemPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return ShowcaseItemPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<ShowcaseItem> ShowcaseItemPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return ShowcaseItemPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<ShowcaseItem> ShowcaseItemPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseItemID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<ShowcaseItem> objects;
			string baseKey = cacheKeyPrefix + "ShowcaseItemPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseItem> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseItem>();
				tmpList = Cache[key] as List<ShowcaseItem>;
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
					IQueryable<ShowcaseItem> itemQuery = SetupQuery(entity.ShowcaseItem, "ShowcaseItem", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_ShowcaseItem");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterShowcaseItemActive { get; set; }
			public string FilterShowcaseItemAgentID { get; set; }
			public string FilterShowcaseItemBuilderID { get; set; }
			public string FilterShowcaseItemFeatured { get; set; }
			public string FilterShowcaseItemMlsID { get; set; }
			public string FilterShowcaseItemNeighborhoodID { get; set; }
			public string FilterShowcaseItemNewHome { get; set; }
			public string FilterShowcaseItemOfficeID { get; set; }
			public string FilterShowcaseItemRented { get; set; }
			public string FilterShowcaseItemShowcaseID { get; set; }
			public string FilterShowcaseItemStatsSentToOwner { get; set; }
			public string FilterShowcaseItemTitle { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterShowcaseItemActive != null)
				{
					if (FilterShowcaseItemActive == string.Empty)
						filterList.Add("@FilterShowcaseItemActive", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemActive", Convert.ToBoolean(FilterShowcaseItemActive));
				}
				if (FilterShowcaseItemAgentID != null)
				{
					if (FilterShowcaseItemAgentID == string.Empty)
						filterList.Add("@FilterShowcaseItemAgentID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemAgentID", Convert.ToInt32(FilterShowcaseItemAgentID));
				}
				if (FilterShowcaseItemBuilderID != null)
				{
					if (FilterShowcaseItemBuilderID == string.Empty)
						filterList.Add("@FilterShowcaseItemBuilderID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemBuilderID", Convert.ToInt32(FilterShowcaseItemBuilderID));
				}
				if (FilterShowcaseItemFeatured != null)
				{
					if (FilterShowcaseItemFeatured == string.Empty)
						filterList.Add("@FilterShowcaseItemFeatured", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemFeatured", Convert.ToBoolean(FilterShowcaseItemFeatured));
				}
				if (FilterShowcaseItemMlsID != null)
				{
					if (FilterShowcaseItemMlsID == string.Empty)
						filterList.Add("@FilterShowcaseItemMlsID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemMlsID", Convert.ToInt32(FilterShowcaseItemMlsID));
				}
				if (FilterShowcaseItemNeighborhoodID != null)
				{
					if (FilterShowcaseItemNeighborhoodID == string.Empty)
						filterList.Add("@FilterShowcaseItemNeighborhoodID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemNeighborhoodID", Convert.ToInt32(FilterShowcaseItemNeighborhoodID));
				}
				if (FilterShowcaseItemNewHome != null)
				{
					if (FilterShowcaseItemNewHome == string.Empty)
						filterList.Add("@FilterShowcaseItemNewHome", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemNewHome", Convert.ToBoolean(FilterShowcaseItemNewHome));
				}
				if (FilterShowcaseItemOfficeID != null)
				{
					if (FilterShowcaseItemOfficeID == string.Empty)
						filterList.Add("@FilterShowcaseItemOfficeID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemOfficeID", Convert.ToInt32(FilterShowcaseItemOfficeID));
				}
				if (FilterShowcaseItemRented != null)
				{
					if (FilterShowcaseItemRented == string.Empty)
						filterList.Add("@FilterShowcaseItemRented", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemRented", Convert.ToBoolean(FilterShowcaseItemRented));
				}
				if (FilterShowcaseItemShowcaseID != null)
				{
					if (FilterShowcaseItemShowcaseID == string.Empty)
						filterList.Add("@FilterShowcaseItemShowcaseID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemShowcaseID", Convert.ToInt32(FilterShowcaseItemShowcaseID));
				}
				if (FilterShowcaseItemStatsSentToOwner != null)
				{
					if (FilterShowcaseItemStatsSentToOwner == string.Empty)
						filterList.Add("@FilterShowcaseItemStatsSentToOwner", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemStatsSentToOwner", Convert.ToBoolean(FilterShowcaseItemStatsSentToOwner));
				}
				if (FilterShowcaseItemTitle != null)
					filterList.Add("@FilterShowcaseItemTitle", FilterShowcaseItemTitle);
				return filterList;
			}
		}
	}
}