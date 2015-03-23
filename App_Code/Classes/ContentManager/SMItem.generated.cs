using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class SMItem : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ContentManager_SMItem_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public SMItem(SMItem objectToCopy)
		{
			CMMicrositeID = objectToCopy.CMMicrositeID;
			CMPageID = objectToCopy.CMPageID;
			EditorDeleted = objectToCopy.EditorDeleted;
			LanguageID = objectToCopy.LanguageID;
			MicrositeDefault = objectToCopy.MicrositeDefault;
			Name = objectToCopy.Name;
			NeedsApproval = objectToCopy.NeedsApproval;
			NewHomes = objectToCopy.NewHomes;
			OriginalSMItemID = objectToCopy.OriginalSMItemID;
			Rank = objectToCopy.Rank;
			ShowInMenu = objectToCopy.ShowInMenu;
			SMItemID = objectToCopy.SMItemID;
			SMItemParentID = objectToCopy.SMItemParentID;
		}

		public virtual bool IsNewRecord
		{
			get { return SMItemID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("SMItem", this);
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

		public static SMItem GetByID(int SMItemID, IEnumerable<string> includeList = null)
		{
			SMItem obj = null;
			string key = cacheKeyPrefix + SMItemID + GetCacheIncludeText(includeList);

			SMItem tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as SMItem;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SMItem> itemQuery = AddIncludes(entity.SMItem, includeList);
					obj = itemQuery.FirstOrDefault(n => n.SMItemID == SMItemID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<SMItem> GetAll(IEnumerable<string> includeList = null)
		{
			List<SMItem> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<SMItem> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SMItem>();
				tmpList = Cache[key] as List<SMItem>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SMItem> itemQuery = AddIncludes(entity.SMItem, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<SMItem> SMItemGetByCMMicrositeID(Int32? CMMicrositeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemCMMicrositeID = CMMicrositeID.ToString();
			return SMItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItem> SMItemGetByCMPageID(Int32 CMPageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemCMPageID = CMPageID.ToString();
			return SMItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItem> SMItemGetByEditorDeleted(Boolean EditorDeleted, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemEditorDeleted = EditorDeleted.ToString();
			return SMItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItem> SMItemGetByLanguageID(Int32? LanguageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemLanguageID = LanguageID.ToString();
			return SMItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItem> SMItemGetByMicrositeDefault(Boolean MicrositeDefault, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemMicrositeDefault = MicrositeDefault.ToString();
			return SMItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItem> SMItemGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemName = Name.ToString();
			return SMItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItem> SMItemGetByNeedsApproval(Boolean NeedsApproval, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemNeedsApproval = NeedsApproval.ToString();
			return SMItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItem> SMItemGetByNewHomes(Boolean? NewHomes, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemNewHomes = NewHomes.ToString();
			return SMItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItem> SMItemGetByRank(Int16 Rank, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemRank = Rank.ToString();
			return SMItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItem> SMItemGetByShowInMenu(Boolean ShowInMenu, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemShowInMenu = ShowInMenu.ToString();
			return SMItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SMItem> SMItemGetBySMItemParentID(Int32? SMItemParentID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSMItemSMItemParentID = SMItemParentID.ToString();
			return SMItemPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<SMItem> SMItemPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<SMItem> objects = SMItemPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<SMItem> SMItemPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return SMItemPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<SMItem> SMItemPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return SMItemPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<SMItem> SMItemPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SMItemID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<SMItem> objects;
			string baseKey = cacheKeyPrefix + "SMItemPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<SMItem> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SMItem>();
				tmpList = Cache[key] as List<SMItem>;
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
					IQueryable<SMItem> itemQuery = SetupQuery(entity.SMItem, "SMItem", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ContentManager_SMItem");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterSMItemCMMicrositeID { get; set; }
			public string FilterSMItemCMPageID { get; set; }
			public string FilterSMItemEditorDeleted { get; set; }
			public string FilterSMItemLanguageID { get; set; }
			public string FilterSMItemMicrositeDefault { get; set; }
			public string FilterSMItemName { get; set; }
			public string FilterSMItemNeedsApproval { get; set; }
			public string FilterSMItemNewHomes { get; set; }
			public string FilterSMItemRank { get; set; }
			public string FilterSMItemShowInMenu { get; set; }
			public string FilterSMItemSMItemParentID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterSMItemCMMicrositeID != null)
				{
					if (FilterSMItemCMMicrositeID == string.Empty)
						filterList.Add("@FilterSMItemCMMicrositeID", string.Empty);
					else
						filterList.Add("@FilterSMItemCMMicrositeID", Convert.ToInt32(FilterSMItemCMMicrositeID));
				}
				if (FilterSMItemCMPageID != null)
				{
					if (FilterSMItemCMPageID == string.Empty)
						filterList.Add("@FilterSMItemCMPageID", string.Empty);
					else
						filterList.Add("@FilterSMItemCMPageID", Convert.ToInt32(FilterSMItemCMPageID));
				}
				if (FilterSMItemEditorDeleted != null)
				{
					if (FilterSMItemEditorDeleted == string.Empty)
						filterList.Add("@FilterSMItemEditorDeleted", string.Empty);
					else
						filterList.Add("@FilterSMItemEditorDeleted", Convert.ToBoolean(FilterSMItemEditorDeleted));
				}
				if (FilterSMItemLanguageID != null)
				{
					if (FilterSMItemLanguageID == string.Empty)
						filterList.Add("@FilterSMItemLanguageID", string.Empty);
					else
						filterList.Add("@FilterSMItemLanguageID", Convert.ToInt32(FilterSMItemLanguageID));
				}
				if (FilterSMItemMicrositeDefault != null)
				{
					if (FilterSMItemMicrositeDefault == string.Empty)
						filterList.Add("@FilterSMItemMicrositeDefault", string.Empty);
					else
						filterList.Add("@FilterSMItemMicrositeDefault", Convert.ToBoolean(FilterSMItemMicrositeDefault));
				}
				if (FilterSMItemName != null)
					filterList.Add("@FilterSMItemName", FilterSMItemName);
				if (FilterSMItemNeedsApproval != null)
				{
					if (FilterSMItemNeedsApproval == string.Empty)
						filterList.Add("@FilterSMItemNeedsApproval", string.Empty);
					else
						filterList.Add("@FilterSMItemNeedsApproval", Convert.ToBoolean(FilterSMItemNeedsApproval));
				}
				if (FilterSMItemNewHomes != null)
				{
					if (FilterSMItemNewHomes == string.Empty)
						filterList.Add("@FilterSMItemNewHomes", string.Empty);
					else
						filterList.Add("@FilterSMItemNewHomes", Convert.ToBoolean(FilterSMItemNewHomes));
				}
				if (FilterSMItemRank != null)
				{
					if (FilterSMItemRank == string.Empty)
						filterList.Add("@FilterSMItemRank", string.Empty);
					else
						filterList.Add("@FilterSMItemRank", Convert.ToInt16(FilterSMItemRank));
				}
				if (FilterSMItemShowInMenu != null)
				{
					if (FilterSMItemShowInMenu == string.Empty)
						filterList.Add("@FilterSMItemShowInMenu", string.Empty);
					else
						filterList.Add("@FilterSMItemShowInMenu", Convert.ToBoolean(FilterSMItemShowInMenu));
				}
				if (FilterSMItemSMItemParentID != null)
				{
					if (FilterSMItemSMItemParentID == string.Empty)
						filterList.Add("@FilterSMItemSMItemParentID", string.Empty);
					else
						filterList.Add("@FilterSMItemSMItemParentID", Convert.ToInt32(FilterSMItemSMItemParentID));
				}
				return filterList;
			}
		}
	}
}