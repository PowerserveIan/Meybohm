using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class Media : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_Media_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Caption"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public Media()
		{
		}

		public Media(Media objectToCopy)
		{
			Active = objectToCopy.Active;
			Caption = objectToCopy.Caption;
			DisplayOrder = objectToCopy.DisplayOrder;
			ShowcaseMediaCollectionID = objectToCopy.ShowcaseMediaCollectionID;
			ShowcaseMediaID = objectToCopy.ShowcaseMediaID;
			Thumbnail = objectToCopy.Thumbnail;
			URL = objectToCopy.URL;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseMediaID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("Media", this);
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

		public static Media GetByID(int ShowcaseMediaID, IEnumerable<string> includeList = null)
		{
			Media obj = null;
			string key = cacheKeyPrefix + ShowcaseMediaID + GetCacheIncludeText(includeList);

			Media tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Media;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Media> itemQuery = AddIncludes(entity.Media, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseMediaID == ShowcaseMediaID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Media> GetAll(IEnumerable<string> includeList = null)
		{
			List<Media> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Media> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Media>();
				tmpList = Cache[key] as List<Media>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Media> itemQuery = AddIncludes(entity.Media, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Media> MediaGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMediaActive = Active.ToString();
			return MediaPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Media> MediaGetByCaption(String Caption, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMediaCaption = Caption.ToString();
			return MediaPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Media> MediaGetByDisplayOrder(Int16 DisplayOrder, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMediaDisplayOrder = DisplayOrder.ToString();
			return MediaPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Media> MediaGetByShowcaseMediaCollectionID(Int32 ShowcaseMediaCollectionID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMediaShowcaseMediaCollectionID = ShowcaseMediaCollectionID.ToString();
			return MediaPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Media> MediaPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Media> objects = MediaPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Media> MediaPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return MediaPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Media> MediaPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return MediaPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Media> MediaPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseMediaID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Media> objects;
			string baseKey = cacheKeyPrefix + "MediaPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Media> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Media>();
				tmpList = Cache[key] as List<Media>;
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
					IQueryable<Media> itemQuery = SetupQuery(entity.Media, "Media", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_Media");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterMediaActive { get; set; }
			public string FilterMediaCaption { get; set; }
			public string FilterMediaDisplayOrder { get; set; }
			public string FilterMediaShowcaseMediaCollectionID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterMediaActive != null)
				{
					if (FilterMediaActive == string.Empty)
						filterList.Add("@FilterMediaActive", string.Empty);
					else
						filterList.Add("@FilterMediaActive", Convert.ToBoolean(FilterMediaActive));
				}
				if (FilterMediaCaption != null)
					filterList.Add("@FilterMediaCaption", FilterMediaCaption);
				if (FilterMediaDisplayOrder != null)
				{
					if (FilterMediaDisplayOrder == string.Empty)
						filterList.Add("@FilterMediaDisplayOrder", string.Empty);
					else
						filterList.Add("@FilterMediaDisplayOrder", Convert.ToInt16(FilterMediaDisplayOrder));
				}
				if (FilterMediaShowcaseMediaCollectionID != null)
				{
					if (FilterMediaShowcaseMediaCollectionID == string.Empty)
						filterList.Add("@FilterMediaShowcaseMediaCollectionID", string.Empty);
					else
						filterList.Add("@FilterMediaShowcaseMediaCollectionID", Convert.ToInt32(FilterMediaShowcaseMediaCollectionID));
				}
				return filterList;
			}
		}
	}
}