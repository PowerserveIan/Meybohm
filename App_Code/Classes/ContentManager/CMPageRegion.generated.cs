using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class CMPageRegion : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ContentManager_CMPageRegion_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public CMPageRegion()
		{
		}

		public CMPageRegion(CMPageRegion objectToCopy)
		{
			CMPageID = objectToCopy.CMPageID;
			CMPageRegionID = objectToCopy.CMPageRegionID;
			CMRegionID = objectToCopy.CMRegionID;
			Content = objectToCopy.Content;
			ContentClean = objectToCopy.ContentClean;
			Created = objectToCopy.Created;
			CurrentVersion = objectToCopy.CurrentVersion;
			Draft = objectToCopy.Draft;
			EditorUserIDs = objectToCopy.EditorUserIDs;
			GlobalAreaCMPageID = objectToCopy.GlobalAreaCMPageID;
			LanguageID = objectToCopy.LanguageID;
			NeedsApproval = objectToCopy.NeedsApproval;
			UserID = objectToCopy.UserID;
		}

		public virtual bool IsNewRecord
		{
			get { return CMPageRegionID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime CreatedClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(Created); }
		}

		public virtual void Save()
		{
			SaveEntity("CMPageRegion", this);
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

		public static CMPageRegion GetByID(int CMPageRegionID, IEnumerable<string> includeList = null)
		{
			CMPageRegion obj = null;
			string key = cacheKeyPrefix + CMPageRegionID + GetCacheIncludeText(includeList);

			CMPageRegion tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as CMPageRegion;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMPageRegion> itemQuery = AddIncludes(entity.CMPageRegion, includeList);
					obj = itemQuery.FirstOrDefault(n => n.CMPageRegionID == CMPageRegionID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<CMPageRegion> GetAll(IEnumerable<string> includeList = null)
		{
			List<CMPageRegion> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<CMPageRegion> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMPageRegion>();
				tmpList = Cache[key] as List<CMPageRegion>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMPageRegion> itemQuery = AddIncludes(entity.CMPageRegion, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<CMPageRegion> CMPageRegionGetByCMPageID(Int32 CMPageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageRegionCMPageID = CMPageID.ToString();
			return CMPageRegionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPageRegion> CMPageRegionGetByCMRegionID(Int32 CMRegionID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageRegionCMRegionID = CMRegionID.ToString();
			return CMPageRegionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPageRegion> CMPageRegionGetByCreated(DateTime Created, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageRegionCreated = Created.ToString();
			return CMPageRegionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPageRegion> CMPageRegionGetByCurrentVersion(Boolean CurrentVersion, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageRegionCurrentVersion = CurrentVersion.ToString();
			return CMPageRegionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPageRegion> CMPageRegionGetByDraft(Boolean Draft, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageRegionDraft = Draft.ToString();
			return CMPageRegionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPageRegion> CMPageRegionGetByLanguageID(Int32? LanguageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageRegionLanguageID = LanguageID.ToString();
			return CMPageRegionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPageRegion> CMPageRegionGetByNeedsApproval(Boolean NeedsApproval, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageRegionNeedsApproval = NeedsApproval.ToString();
			return CMPageRegionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPageRegion> CMPageRegionGetByUserID(Int32? UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageRegionUserID = UserID.ToString();
			return CMPageRegionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<CMPageRegion> CMPageRegionPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<CMPageRegion> objects = CMPageRegionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<CMPageRegion> CMPageRegionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return CMPageRegionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<CMPageRegion> CMPageRegionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return CMPageRegionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<CMPageRegion> CMPageRegionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "CMPageRegionID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<CMPageRegion> objects;
			string baseKey = cacheKeyPrefix + "CMPageRegionPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<CMPageRegion> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMPageRegion>();
				tmpList = Cache[key] as List<CMPageRegion>;
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
					IQueryable<CMPageRegion> itemQuery = SetupQuery(entity.CMPageRegion, "CMPageRegion", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ContentManager_CMPageRegion");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterCMPageRegionCMPageID { get; set; }
			public string FilterCMPageRegionCMRegionID { get; set; }
			public string FilterCMPageRegionCreated { get; set; }
			public string FilterCMPageRegionCurrentVersion { get; set; }
			public string FilterCMPageRegionDraft { get; set; }
			public string FilterCMPageRegionLanguageID { get; set; }
			public string FilterCMPageRegionNeedsApproval { get; set; }
			public string FilterCMPageRegionUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterCMPageRegionCMPageID != null)
				{
					if (FilterCMPageRegionCMPageID == string.Empty)
						filterList.Add("@FilterCMPageRegionCMPageID", string.Empty);
					else
						filterList.Add("@FilterCMPageRegionCMPageID", Convert.ToInt32(FilterCMPageRegionCMPageID));
				}
				if (FilterCMPageRegionCMRegionID != null)
				{
					if (FilterCMPageRegionCMRegionID == string.Empty)
						filterList.Add("@FilterCMPageRegionCMRegionID", string.Empty);
					else
						filterList.Add("@FilterCMPageRegionCMRegionID", Convert.ToInt32(FilterCMPageRegionCMRegionID));
				}
				if (FilterCMPageRegionCreated != null)
				{
					if (FilterCMPageRegionCreated == string.Empty)
						filterList.Add("@FilterCMPageRegionCreated", string.Empty);
					else
						filterList.Add("@FilterCMPageRegionCreated", Convert.ToDateTime(FilterCMPageRegionCreated));
				}
				if (FilterCMPageRegionCurrentVersion != null)
				{
					if (FilterCMPageRegionCurrentVersion == string.Empty)
						filterList.Add("@FilterCMPageRegionCurrentVersion", string.Empty);
					else
						filterList.Add("@FilterCMPageRegionCurrentVersion", Convert.ToBoolean(FilterCMPageRegionCurrentVersion));
				}
				if (FilterCMPageRegionDraft != null)
				{
					if (FilterCMPageRegionDraft == string.Empty)
						filterList.Add("@FilterCMPageRegionDraft", string.Empty);
					else
						filterList.Add("@FilterCMPageRegionDraft", Convert.ToBoolean(FilterCMPageRegionDraft));
				}
				if (FilterCMPageRegionLanguageID != null)
				{
					if (FilterCMPageRegionLanguageID == string.Empty)
						filterList.Add("@FilterCMPageRegionLanguageID", string.Empty);
					else
						filterList.Add("@FilterCMPageRegionLanguageID", Convert.ToInt32(FilterCMPageRegionLanguageID));
				}
				if (FilterCMPageRegionNeedsApproval != null)
				{
					if (FilterCMPageRegionNeedsApproval == string.Empty)
						filterList.Add("@FilterCMPageRegionNeedsApproval", string.Empty);
					else
						filterList.Add("@FilterCMPageRegionNeedsApproval", Convert.ToBoolean(FilterCMPageRegionNeedsApproval));
				}
				if (FilterCMPageRegionUserID != null)
				{
					if (FilterCMPageRegionUserID == string.Empty)
						filterList.Add("@FilterCMPageRegionUserID", string.Empty);
					else
						filterList.Add("@FilterCMPageRegionUserID", Convert.ToInt32(FilterCMPageRegionUserID));
				}
				return filterList;
			}
		}
	}
}