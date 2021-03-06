using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.WhatsNearBy
{
	public partial class WhatsNearByLocation : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "WhatsNearBy_WhatsNearByLocation_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public WhatsNearByLocation(WhatsNearByLocation objectToCopy)
		{
			Active = objectToCopy.Active;
			AddressID = objectToCopy.AddressID;
			Description = objectToCopy.Description;
			Image = objectToCopy.Image;
			Name = objectToCopy.Name;
			Phone = objectToCopy.Phone;
			Website = objectToCopy.Website;
			WhatsNearByLocationID = objectToCopy.WhatsNearByLocationID;
		}

		public virtual bool IsNewRecord
		{
			get { return WhatsNearByLocationID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("WhatsNearByLocation", this);
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

		public static WhatsNearByLocation GetByID(int WhatsNearByLocationID, IEnumerable<string> includeList = null)
		{
			WhatsNearByLocation obj = null;
			string key = cacheKeyPrefix + WhatsNearByLocationID + GetCacheIncludeText(includeList);

			WhatsNearByLocation tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as WhatsNearByLocation;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<WhatsNearByLocation> itemQuery = AddIncludes(entity.WhatsNearByLocation, includeList);
					obj = itemQuery.FirstOrDefault(n => n.WhatsNearByLocationID == WhatsNearByLocationID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<WhatsNearByLocation> GetAll(IEnumerable<string> includeList = null)
		{
			List<WhatsNearByLocation> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<WhatsNearByLocation> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<WhatsNearByLocation>();
				tmpList = Cache[key] as List<WhatsNearByLocation>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<WhatsNearByLocation> itemQuery = AddIncludes(entity.WhatsNearByLocation, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<WhatsNearByLocation> WhatsNearByLocationGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterWhatsNearByLocationActive = Active.ToString();
			return WhatsNearByLocationPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<WhatsNearByLocation> WhatsNearByLocationGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterWhatsNearByLocationName = Name.ToString();
			return WhatsNearByLocationPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<WhatsNearByLocation> WhatsNearByLocationPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<WhatsNearByLocation> objects = WhatsNearByLocationPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<WhatsNearByLocation> WhatsNearByLocationPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return WhatsNearByLocationPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<WhatsNearByLocation> WhatsNearByLocationPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return WhatsNearByLocationPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<WhatsNearByLocation> WhatsNearByLocationPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "WhatsNearByLocationID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<WhatsNearByLocation> objects;
			string baseKey = cacheKeyPrefix + "WhatsNearByLocationPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<WhatsNearByLocation> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<WhatsNearByLocation>();
				tmpList = Cache[key] as List<WhatsNearByLocation>;
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
					IQueryable<WhatsNearByLocation> itemQuery = SetupQuery(entity.WhatsNearByLocation, "WhatsNearByLocation", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("WhatsNearBy_WhatsNearByLocation");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterWhatsNearByLocationActive { get; set; }
			public string FilterWhatsNearByLocationName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterWhatsNearByLocationActive != null)
				{
					if (FilterWhatsNearByLocationActive == string.Empty)
						filterList.Add("@FilterWhatsNearByLocationActive", string.Empty);
					else
						filterList.Add("@FilterWhatsNearByLocationActive", Convert.ToBoolean(FilterWhatsNearByLocationActive));
				}
				if (FilterWhatsNearByLocationName != null)
					filterList.Add("@FilterWhatsNearByLocationName", FilterWhatsNearByLocationName);
				return filterList;
			}
		}
	}
}