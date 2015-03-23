using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Videos
{
	public partial class Video : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Videos_Video_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Title"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public Video()
		{
		}

		public Video(Video objectToCopy)
		{
			Active = objectToCopy.Active;
			DateAdded = objectToCopy.DateAdded;
			DisplayOrder = objectToCopy.DisplayOrder;
			Featured = objectToCopy.Featured;
			Title = objectToCopy.Title;
			Url = objectToCopy.Url;
			VideoID = objectToCopy.VideoID;
		}

		public virtual bool IsNewRecord
		{
			get { return VideoID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime DateAddedClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(DateAdded); }
		}

		public virtual void Save()
		{
			SaveEntity("Video", this);
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

		public static Video GetByID(int VideoID, IEnumerable<string> includeList = null)
		{
			Video obj = null;
			string key = cacheKeyPrefix + VideoID + GetCacheIncludeText(includeList);

			Video tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Video;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Video> itemQuery = AddIncludes(entity.Video, includeList);
					obj = itemQuery.FirstOrDefault(n => n.VideoID == VideoID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Video> GetAll(IEnumerable<string> includeList = null)
		{
			List<Video> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Video> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Video>();
				tmpList = Cache[key] as List<Video>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Video> itemQuery = AddIncludes(entity.Video, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Video> VideoGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterVideoActive = Active.ToString();
			return VideoPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Video> VideoGetByFeatured(Boolean Featured, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterVideoFeatured = Featured.ToString();
			return VideoPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Video> VideoGetByTitle(String Title, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterVideoTitle = Title.ToString();
			return VideoPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Video> VideoPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Video> objects = VideoPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Video> VideoPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return VideoPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Video> VideoPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return VideoPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Video> VideoPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "VideoID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Video> objects;
			string baseKey = cacheKeyPrefix + "VideoPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Video> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Video>();
				tmpList = Cache[key] as List<Video>;
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
					IQueryable<Video> itemQuery = SetupQuery(entity.Video, "Video", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Videos_Video");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterVideoActive { get; set; }
			public string FilterVideoFeatured { get; set; }
			public string FilterVideoTitle { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterVideoActive != null)
				{
					if (FilterVideoActive == string.Empty)
						filterList.Add("@FilterVideoActive", string.Empty);
					else
						filterList.Add("@FilterVideoActive", Convert.ToBoolean(FilterVideoActive));
				}
				if (FilterVideoFeatured != null)
				{
					if (FilterVideoFeatured == string.Empty)
						filterList.Add("@FilterVideoFeatured", string.Empty);
					else
						filterList.Add("@FilterVideoFeatured", Convert.ToBoolean(FilterVideoFeatured));
				}
				if (FilterVideoTitle != null)
					filterList.Add("@FilterVideoTitle", FilterVideoTitle);
				return filterList;
			}
		}
	}
}