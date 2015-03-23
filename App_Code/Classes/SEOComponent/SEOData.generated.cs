using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.SEOComponent
{
	public partial class SEOData : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "SEOComponent_SEOData_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "FriendlyFilename", "Keywords", "PageURL", "Title"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public SEOData(SEOData objectToCopy)
		{
			Approved = objectToCopy.Approved;
			ApprovedSEODataID = objectToCopy.ApprovedSEODataID;
			DateCreated = objectToCopy.DateCreated;
			DateLastUpdated = objectToCopy.DateLastUpdated;
			Description = objectToCopy.Description;
			FriendlyFilename = objectToCopy.FriendlyFilename;
			Keywords = objectToCopy.Keywords;
			LanguageID = objectToCopy.LanguageID;
			PageURL = objectToCopy.PageURL;
			SEODataID = objectToCopy.SEODataID;
			Title = objectToCopy.Title;
		}

		public virtual bool IsNewRecord
		{
			get { return SEODataID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime DateCreatedClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(DateCreated); }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime DateLastUpdatedClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(DateLastUpdated); }
		}

		public virtual void Save()
		{
			SaveEntity("SEOData", this);
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

		public static SEOData GetByID(int SEODataID, IEnumerable<string> includeList = null)
		{
			SEOData obj = null;
			string key = cacheKeyPrefix + SEODataID + GetCacheIncludeText(includeList);

			SEOData tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as SEOData;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SEOData> itemQuery = AddIncludes(entity.SEOData, includeList);
					obj = itemQuery.FirstOrDefault(n => n.SEODataID == SEODataID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<SEOData> GetAll(IEnumerable<string> includeList = null)
		{
			List<SEOData> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<SEOData> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SEOData>();
				tmpList = Cache[key] as List<SEOData>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SEOData> itemQuery = AddIncludes(entity.SEOData, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<SEOData> SEODataGetByApproved(Boolean Approved, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSEODataApproved = Approved.ToString();
			return SEODataPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SEOData> SEODataGetByApprovedSEODataID(Int32? ApprovedSEODataID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSEODataApprovedSEODataID = ApprovedSEODataID.ToString();
			return SEODataPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SEOData> SEODataGetByDateCreated(DateTime DateCreated, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSEODataDateCreated = DateCreated.ToString();
			return SEODataPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SEOData> SEODataGetByFriendlyFilename(String FriendlyFilename, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSEODataFriendlyFilename = FriendlyFilename.ToString();
			return SEODataPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SEOData> SEODataGetByKeywords(String Keywords, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSEODataKeywords = Keywords.ToString();
			return SEODataPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SEOData> SEODataGetByPageURL(String PageURL, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSEODataPageURL = PageURL.ToString();
			return SEODataPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SEOData> SEODataGetByTitle(String Title, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSEODataTitle = Title.ToString();
			return SEODataPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<SEOData> SEODataPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<SEOData> objects = SEODataPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<SEOData> SEODataPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return SEODataPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<SEOData> SEODataPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return SEODataPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<SEOData> SEODataPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SEODataID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<SEOData> objects;
			string baseKey = cacheKeyPrefix + "SEODataPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<SEOData> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SEOData>();
				tmpList = Cache[key] as List<SEOData>;
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
					IQueryable<SEOData> itemQuery = SetupQuery(entity.SEOData, "SEOData", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("SEOComponent_SEOData");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterSEODataApproved { get; set; }
			public string FilterSEODataApprovedSEODataID { get; set; }
			public string FilterSEODataDateCreated { get; set; }
			public string FilterSEODataFriendlyFilename { get; set; }
			public string FilterSEODataKeywords { get; set; }
			public string FilterSEODataPageURL { get; set; }
			public string FilterSEODataTitle { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterSEODataApproved != null)
				{
					if (FilterSEODataApproved == string.Empty)
						filterList.Add("@FilterSEODataApproved", string.Empty);
					else
						filterList.Add("@FilterSEODataApproved", Convert.ToBoolean(FilterSEODataApproved));
				}
				if (FilterSEODataApprovedSEODataID != null)
				{
					if (FilterSEODataApprovedSEODataID == string.Empty)
						filterList.Add("@FilterSEODataApprovedSEODataID", string.Empty);
					else
						filterList.Add("@FilterSEODataApprovedSEODataID", Convert.ToInt32(FilterSEODataApprovedSEODataID));
				}
				if (FilterSEODataDateCreated != null)
				{
					if (FilterSEODataDateCreated == string.Empty)
						filterList.Add("@FilterSEODataDateCreated", string.Empty);
					else
						filterList.Add("@FilterSEODataDateCreated", Convert.ToDateTime(FilterSEODataDateCreated));
				}
				if (FilterSEODataFriendlyFilename != null)
					filterList.Add("@FilterSEODataFriendlyFilename", FilterSEODataFriendlyFilename);
				if (FilterSEODataKeywords != null)
					filterList.Add("@FilterSEODataKeywords", FilterSEODataKeywords);
				if (FilterSEODataPageURL != null)
					filterList.Add("@FilterSEODataPageURL", FilterSEODataPageURL);
				if (FilterSEODataTitle != null)
					filterList.Add("@FilterSEODataTitle", FilterSEODataTitle);
				return filterList;
			}
		}
	}
}