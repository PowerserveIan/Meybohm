using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class MediaCollection : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_MediaCollection_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Title"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public MediaCollection(MediaCollection objectToCopy)
		{
			Active = objectToCopy.Active;
			DisplayOrder = objectToCopy.DisplayOrder;
			IsFine = objectToCopy.IsFine;
			ShowcaseItemID = objectToCopy.ShowcaseItemID;
			ShowcaseMediaCollectionID = objectToCopy.ShowcaseMediaCollectionID;
			ShowcaseMediaTypeID = objectToCopy.ShowcaseMediaTypeID;
			TextBlock = objectToCopy.TextBlock;
			Title = objectToCopy.Title;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseMediaCollectionID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("MediaCollection", this);
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

		public static MediaCollection GetByID(int ShowcaseMediaCollectionID, IEnumerable<string> includeList = null)
		{
			MediaCollection obj = null;
			string key = cacheKeyPrefix + ShowcaseMediaCollectionID + GetCacheIncludeText(includeList);

			MediaCollection tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as MediaCollection;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<MediaCollection> itemQuery = AddIncludes(entity.MediaCollection, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseMediaCollectionID == ShowcaseMediaCollectionID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<MediaCollection> GetAll(IEnumerable<string> includeList = null)
		{
			List<MediaCollection> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<MediaCollection> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<MediaCollection>();
				tmpList = Cache[key] as List<MediaCollection>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<MediaCollection> itemQuery = AddIncludes(entity.MediaCollection, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<MediaCollection> MediaCollectionGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMediaCollectionActive = Active.ToString();
			return MediaCollectionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<MediaCollection> MediaCollectionGetByDisplayOrder(Int16? DisplayOrder, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMediaCollectionDisplayOrder = DisplayOrder.ToString();
			return MediaCollectionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<MediaCollection> MediaCollectionGetByShowcaseItemID(Int32 ShowcaseItemID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMediaCollectionShowcaseItemID = ShowcaseItemID.ToString();
			return MediaCollectionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<MediaCollection> MediaCollectionGetByShowcaseMediaTypeID(Int32 ShowcaseMediaTypeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMediaCollectionShowcaseMediaTypeID = ShowcaseMediaTypeID.ToString();
			return MediaCollectionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<MediaCollection> MediaCollectionGetByTitle(String Title, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMediaCollectionTitle = Title.ToString();
			return MediaCollectionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<MediaCollection> MediaCollectionPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<MediaCollection> objects = MediaCollectionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<MediaCollection> MediaCollectionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return MediaCollectionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<MediaCollection> MediaCollectionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return MediaCollectionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<MediaCollection> MediaCollectionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseMediaCollectionID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<MediaCollection> objects;
			string baseKey = cacheKeyPrefix + "MediaCollectionPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<MediaCollection> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<MediaCollection>();
				tmpList = Cache[key] as List<MediaCollection>;
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
					IQueryable<MediaCollection> itemQuery = SetupQuery(entity.MediaCollection, "MediaCollection", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_MediaCollection");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterMediaCollectionActive { get; set; }
			public string FilterMediaCollectionDisplayOrder { get; set; }
			public string FilterMediaCollectionShowcaseItemID { get; set; }
			public string FilterMediaCollectionShowcaseMediaTypeID { get; set; }
			public string FilterMediaCollectionTitle { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterMediaCollectionActive != null)
				{
					if (FilterMediaCollectionActive == string.Empty)
						filterList.Add("@FilterMediaCollectionActive", string.Empty);
					else
						filterList.Add("@FilterMediaCollectionActive", Convert.ToBoolean(FilterMediaCollectionActive));
				}
				if (FilterMediaCollectionDisplayOrder != null)
				{
					if (FilterMediaCollectionDisplayOrder == string.Empty)
						filterList.Add("@FilterMediaCollectionDisplayOrder", string.Empty);
					else
						filterList.Add("@FilterMediaCollectionDisplayOrder", Convert.ToInt16(FilterMediaCollectionDisplayOrder));
				}
				if (FilterMediaCollectionShowcaseItemID != null)
				{
					if (FilterMediaCollectionShowcaseItemID == string.Empty)
						filterList.Add("@FilterMediaCollectionShowcaseItemID", string.Empty);
					else
						filterList.Add("@FilterMediaCollectionShowcaseItemID", Convert.ToInt32(FilterMediaCollectionShowcaseItemID));
				}
				if (FilterMediaCollectionShowcaseMediaTypeID != null)
				{
					if (FilterMediaCollectionShowcaseMediaTypeID == string.Empty)
						filterList.Add("@FilterMediaCollectionShowcaseMediaTypeID", string.Empty);
					else
						filterList.Add("@FilterMediaCollectionShowcaseMediaTypeID", Convert.ToInt32(FilterMediaCollectionShowcaseMediaTypeID));
				}
				if (FilterMediaCollectionTitle != null)
					filterList.Add("@FilterMediaCollectionTitle", FilterMediaCollectionTitle);
				return filterList;
			}
		}
	}
}