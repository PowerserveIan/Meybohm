using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class CMPage : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ContentManager_CMPage_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "FileName", "FormRecipient", "Title"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public CMPage(CMPage objectToCopy)
		{
			CanDelete = objectToCopy.CanDelete;
			CMMicrositeID = objectToCopy.CMMicrositeID;
			CMPageID = objectToCopy.CMPageID;
			CMTemplateID = objectToCopy.CMTemplateID;
			Created = objectToCopy.Created;
			Deleted = objectToCopy.Deleted;
			DynamicCollectionID = objectToCopy.DynamicCollectionID;
			EditorDeleted = objectToCopy.EditorDeleted;
			EditorUserIDs = objectToCopy.EditorUserIDs;
			FeaturedPage = objectToCopy.FeaturedPage;
			FileName = objectToCopy.FileName;
			FormRecipient = objectToCopy.FormRecipient;
			MicrositeDefault = objectToCopy.MicrositeDefault;
			NeedsApproval = objectToCopy.NeedsApproval;
			OriginalCMPageID = objectToCopy.OriginalCMPageID;
			ResponsePageID = objectToCopy.ResponsePageID;
			Title = objectToCopy.Title;
			UserID = objectToCopy.UserID;
		}

		public virtual bool IsNewRecord
		{
			get { return CMPageID < 1; }
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
			SaveEntity("CMPage", this);
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

		public static CMPage GetByID(int CMPageID, IEnumerable<string> includeList = null)
		{
			CMPage obj = null;
			string key = cacheKeyPrefix + CMPageID + GetCacheIncludeText(includeList);

			CMPage tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as CMPage;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMPage> itemQuery = AddIncludes(entity.CMPage, includeList);
					obj = itemQuery.FirstOrDefault(n => n.CMPageID == CMPageID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<CMPage> GetAll(IEnumerable<string> includeList = null)
		{
			List<CMPage> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<CMPage> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMPage>();
				tmpList = Cache[key] as List<CMPage>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMPage> itemQuery = AddIncludes(entity.CMPage, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<CMPage> CMPageGetByCanDelete(Boolean CanDelete, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageCanDelete = CanDelete.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByCMMicrositeID(Int32? CMMicrositeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageCMMicrositeID = CMMicrositeID.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByCMTemplateID(Int32? CMTemplateID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageCMTemplateID = CMTemplateID.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByCreated(DateTime Created, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageCreated = Created.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByDeleted(Boolean Deleted, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageDeleted = Deleted.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByDynamicCollectionID(Int32? DynamicCollectionID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageDynamicCollectionID = DynamicCollectionID.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByEditorDeleted(Boolean? EditorDeleted, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageEditorDeleted = EditorDeleted.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByFeaturedPage(Boolean FeaturedPage, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageFeaturedPage = FeaturedPage.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByFileName(String FileName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageFileName = FileName.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByFormRecipient(String FormRecipient, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageFormRecipient = FormRecipient.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByMicrositeDefault(Boolean MicrositeDefault, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageMicrositeDefault = MicrositeDefault.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByNeedsApproval(Boolean NeedsApproval, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageNeedsApproval = NeedsApproval.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByOriginalCMPageID(Int32? OriginalCMPageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageOriginalCMPageID = OriginalCMPageID.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByResponsePageID(Int32? ResponsePageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageResponsePageID = ResponsePageID.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByTitle(String Title, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageTitle = Title.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMPage> CMPageGetByUserID(Int32 UserID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageUserID = UserID.ToString();
			return CMPagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<CMPage> CMPagePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<CMPage> objects = CMPagePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<CMPage> CMPagePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return CMPagePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<CMPage> CMPagePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return CMPagePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<CMPage> CMPagePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "CMPageID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<CMPage> objects;
			string baseKey = cacheKeyPrefix + "CMPagePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<CMPage> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMPage>();
				tmpList = Cache[key] as List<CMPage>;
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
					IQueryable<CMPage> itemQuery = SetupQuery(entity.CMPage, "CMPage", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ContentManager_CMPage");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterCMPageCanDelete { get; set; }
			public string FilterCMPageCMMicrositeID { get; set; }
			public string FilterCMPageCMTemplateID { get; set; }
			public string FilterCMPageCreated { get; set; }
			public string FilterCMPageDeleted { get; set; }
			public string FilterCMPageDynamicCollectionID { get; set; }
			public string FilterCMPageEditorDeleted { get; set; }
			public string FilterCMPageFeaturedPage { get; set; }
			public string FilterCMPageFileName { get; set; }
			public string FilterCMPageFormRecipient { get; set; }
			public string FilterCMPageMicrositeDefault { get; set; }
			public string FilterCMPageNeedsApproval { get; set; }
			public string FilterCMPageOriginalCMPageID { get; set; }
			public string FilterCMPageResponsePageID { get; set; }
			public string FilterCMPageTitle { get; set; }
			public string FilterCMPageUserID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterCMPageCanDelete != null)
				{
					if (FilterCMPageCanDelete == string.Empty)
						filterList.Add("@FilterCMPageCanDelete", string.Empty);
					else
						filterList.Add("@FilterCMPageCanDelete", Convert.ToBoolean(FilterCMPageCanDelete));
				}
				if (FilterCMPageCMMicrositeID != null)
				{
					if (FilterCMPageCMMicrositeID == string.Empty)
						filterList.Add("@FilterCMPageCMMicrositeID", string.Empty);
					else
						filterList.Add("@FilterCMPageCMMicrositeID", Convert.ToInt32(FilterCMPageCMMicrositeID));
				}
				if (FilterCMPageCMTemplateID != null)
				{
					if (FilterCMPageCMTemplateID == string.Empty)
						filterList.Add("@FilterCMPageCMTemplateID", string.Empty);
					else
						filterList.Add("@FilterCMPageCMTemplateID", Convert.ToInt32(FilterCMPageCMTemplateID));
				}
				if (FilterCMPageCreated != null)
				{
					if (FilterCMPageCreated == string.Empty)
						filterList.Add("@FilterCMPageCreated", string.Empty);
					else
						filterList.Add("@FilterCMPageCreated", Convert.ToDateTime(FilterCMPageCreated));
				}
				if (FilterCMPageDeleted != null)
				{
					if (FilterCMPageDeleted == string.Empty)
						filterList.Add("@FilterCMPageDeleted", string.Empty);
					else
						filterList.Add("@FilterCMPageDeleted", Convert.ToBoolean(FilterCMPageDeleted));
				}
				if (FilterCMPageDynamicCollectionID != null)
				{
					if (FilterCMPageDynamicCollectionID == string.Empty)
						filterList.Add("@FilterCMPageDynamicCollectionID", string.Empty);
					else
						filterList.Add("@FilterCMPageDynamicCollectionID", Convert.ToInt32(FilterCMPageDynamicCollectionID));
				}
				if (FilterCMPageEditorDeleted != null)
				{
					if (FilterCMPageEditorDeleted == string.Empty)
						filterList.Add("@FilterCMPageEditorDeleted", string.Empty);
					else
						filterList.Add("@FilterCMPageEditorDeleted", Convert.ToBoolean(FilterCMPageEditorDeleted));
				}
				if (FilterCMPageFeaturedPage != null)
				{
					if (FilterCMPageFeaturedPage == string.Empty)
						filterList.Add("@FilterCMPageFeaturedPage", string.Empty);
					else
						filterList.Add("@FilterCMPageFeaturedPage", Convert.ToBoolean(FilterCMPageFeaturedPage));
				}
				if (FilterCMPageFileName != null)
					filterList.Add("@FilterCMPageFileName", FilterCMPageFileName);
				if (FilterCMPageFormRecipient != null)
					filterList.Add("@FilterCMPageFormRecipient", FilterCMPageFormRecipient);
				if (FilterCMPageMicrositeDefault != null)
				{
					if (FilterCMPageMicrositeDefault == string.Empty)
						filterList.Add("@FilterCMPageMicrositeDefault", string.Empty);
					else
						filterList.Add("@FilterCMPageMicrositeDefault", Convert.ToBoolean(FilterCMPageMicrositeDefault));
				}
				if (FilterCMPageNeedsApproval != null)
				{
					if (FilterCMPageNeedsApproval == string.Empty)
						filterList.Add("@FilterCMPageNeedsApproval", string.Empty);
					else
						filterList.Add("@FilterCMPageNeedsApproval", Convert.ToBoolean(FilterCMPageNeedsApproval));
				}
				if (FilterCMPageOriginalCMPageID != null)
				{
					if (FilterCMPageOriginalCMPageID == string.Empty)
						filterList.Add("@FilterCMPageOriginalCMPageID", string.Empty);
					else
						filterList.Add("@FilterCMPageOriginalCMPageID", Convert.ToInt32(FilterCMPageOriginalCMPageID));
				}
				if (FilterCMPageResponsePageID != null)
				{
					if (FilterCMPageResponsePageID == string.Empty)
						filterList.Add("@FilterCMPageResponsePageID", string.Empty);
					else
						filterList.Add("@FilterCMPageResponsePageID", Convert.ToInt32(FilterCMPageResponsePageID));
				}
				if (FilterCMPageTitle != null)
					filterList.Add("@FilterCMPageTitle", FilterCMPageTitle);
				if (FilterCMPageUserID != null)
				{
					if (FilterCMPageUserID == string.Empty)
						filterList.Add("@FilterCMPageUserID", string.Empty);
					else
						filterList.Add("@FilterCMPageUserID", Convert.ToInt32(FilterCMPageUserID));
				}
				return filterList;
			}
		}
	}
}