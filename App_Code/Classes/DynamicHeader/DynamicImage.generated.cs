using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.DynamicHeader
{
	public partial class DynamicImage : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "DynamicHeader_DynamicImage_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name", "Title"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public DynamicImage(DynamicImage objectToCopy)
		{
			Active = objectToCopy.Active;
			Caption = objectToCopy.Caption;
			Duration = objectToCopy.Duration;
			DynamicImageID = objectToCopy.DynamicImageID;
			IsVideo = objectToCopy.IsVideo;
			LastUpdated = objectToCopy.LastUpdated;
			Link = objectToCopy.Link;
			Name = objectToCopy.Name;
			Title = objectToCopy.Title;
		}

		public virtual bool IsNewRecord
		{
			get { return DynamicImageID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime LastUpdatedClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(LastUpdated); }
		}

		public virtual void Save()
		{
			SaveEntity("DynamicImage", this);
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

		public static DynamicImage GetByID(int DynamicImageID, IEnumerable<string> includeList = null)
		{
			DynamicImage obj = null;
			string key = cacheKeyPrefix + DynamicImageID + GetCacheIncludeText(includeList);

			DynamicImage tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as DynamicImage;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<DynamicImage> itemQuery = AddIncludes(entity.DynamicImage, includeList);
					obj = itemQuery.FirstOrDefault(n => n.DynamicImageID == DynamicImageID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<DynamicImage> GetAll(IEnumerable<string> includeList = null)
		{
			List<DynamicImage> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<DynamicImage> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<DynamicImage>();
				tmpList = Cache[key] as List<DynamicImage>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<DynamicImage> itemQuery = AddIncludes(entity.DynamicImage, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<DynamicImage> DynamicImageGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterDynamicImageActive = Active.ToString();
			return DynamicImagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<DynamicImage> DynamicImageGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterDynamicImageName = Name.ToString();
			return DynamicImagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<DynamicImage> DynamicImageGetByTitle(String Title, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterDynamicImageTitle = Title.ToString();
			return DynamicImagePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<DynamicImage> DynamicImagePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<DynamicImage> objects = DynamicImagePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<DynamicImage> DynamicImagePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return DynamicImagePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<DynamicImage> DynamicImagePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return DynamicImagePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<DynamicImage> DynamicImagePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "DynamicImageID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<DynamicImage> objects;
			string baseKey = cacheKeyPrefix + "DynamicImagePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<DynamicImage> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<DynamicImage>();
				tmpList = Cache[key] as List<DynamicImage>;
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
					IQueryable<DynamicImage> itemQuery = SetupQuery(entity.DynamicImage, "DynamicImage", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("DynamicHeader_DynamicImage");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterDynamicImageActive { get; set; }
			public string FilterDynamicImageName { get; set; }
			public string FilterDynamicImageTitle { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterDynamicImageActive != null)
				{
					if (FilterDynamicImageActive == string.Empty)
						filterList.Add("@FilterDynamicImageActive", string.Empty);
					else
						filterList.Add("@FilterDynamicImageActive", Convert.ToBoolean(FilterDynamicImageActive));
				}
				if (FilterDynamicImageName != null)
					filterList.Add("@FilterDynamicImageName", FilterDynamicImageName);
				if (FilterDynamicImageTitle != null)
					filterList.Add("@FilterDynamicImageTitle", FilterDynamicImageTitle);
				return filterList;
			}
		}
	}
}