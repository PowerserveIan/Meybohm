using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class MediaType : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_MediaType_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Type"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public MediaType(MediaType objectToCopy)
		{
			ShowcaseMediaTypeID = objectToCopy.ShowcaseMediaTypeID;
			Type = objectToCopy.Type;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseMediaTypeID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("MediaType", this);
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

		public static MediaType GetByID(int ShowcaseMediaTypeID, IEnumerable<string> includeList = null)
		{
			MediaType obj = null;
			string key = cacheKeyPrefix + ShowcaseMediaTypeID + GetCacheIncludeText(includeList);

			MediaType tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as MediaType;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<MediaType> itemQuery = AddIncludes(entity.MediaType, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseMediaTypeID == ShowcaseMediaTypeID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<MediaType> GetAll(IEnumerable<string> includeList = null)
		{
			List<MediaType> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<MediaType> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<MediaType>();
				tmpList = Cache[key] as List<MediaType>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<MediaType> itemQuery = AddIncludes(entity.MediaType, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<MediaType> MediaTypeGetByType(String Type, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMediaTypeType = Type.ToString();
			return MediaTypePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<MediaType> MediaTypePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<MediaType> objects = MediaTypePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<MediaType> MediaTypePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return MediaTypePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<MediaType> MediaTypePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return MediaTypePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<MediaType> MediaTypePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseMediaTypeID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<MediaType> objects;
			string baseKey = cacheKeyPrefix + "MediaTypePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<MediaType> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<MediaType>();
				tmpList = Cache[key] as List<MediaType>;
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
					IQueryable<MediaType> itemQuery = SetupQuery(entity.MediaType, "MediaType", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_MediaType");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterMediaTypeType { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterMediaTypeType != null)
					filterList.Add("@FilterMediaTypeType", FilterMediaTypeType);
				return filterList;
			}
		}
	}
}