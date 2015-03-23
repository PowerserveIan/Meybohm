using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class PropertyStatisticsEmailLog : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_PropertyStatisticsEmailLog_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public PropertyStatisticsEmailLog()
		{
		}

		public PropertyStatisticsEmailLog(PropertyStatisticsEmailLog objectToCopy)
		{
			Email = objectToCopy.Email;
			PropertyStatisticsEmailLogID = objectToCopy.PropertyStatisticsEmailLogID;
			ShowcaseItemID = objectToCopy.ShowcaseItemID;
			TimeSent = objectToCopy.TimeSent;
		}

		public virtual bool IsNewRecord
		{
			get { return PropertyStatisticsEmailLogID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime TimeSentClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(TimeSent); }
		}

		public virtual void Save()
		{
			SaveEntity("PropertyStatisticsEmailLog", this);
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

		public static PropertyStatisticsEmailLog GetByID(int PropertyStatisticsEmailLogID, IEnumerable<string> includeList = null)
		{
			PropertyStatisticsEmailLog obj = null;
			string key = cacheKeyPrefix + PropertyStatisticsEmailLogID + GetCacheIncludeText(includeList);

			PropertyStatisticsEmailLog tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as PropertyStatisticsEmailLog;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<PropertyStatisticsEmailLog> itemQuery = AddIncludes(entity.PropertyStatisticsEmailLog, includeList);
					obj = itemQuery.FirstOrDefault(n => n.PropertyStatisticsEmailLogID == PropertyStatisticsEmailLogID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<PropertyStatisticsEmailLog> GetAll(IEnumerable<string> includeList = null)
		{
			List<PropertyStatisticsEmailLog> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<PropertyStatisticsEmailLog> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<PropertyStatisticsEmailLog>();
				tmpList = Cache[key] as List<PropertyStatisticsEmailLog>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<PropertyStatisticsEmailLog> itemQuery = AddIncludes(entity.PropertyStatisticsEmailLog, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
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

		public static List<PropertyStatisticsEmailLog> PropertyStatisticsEmailLogPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<PropertyStatisticsEmailLog> objects = PropertyStatisticsEmailLogPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<PropertyStatisticsEmailLog> PropertyStatisticsEmailLogPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return PropertyStatisticsEmailLogPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<PropertyStatisticsEmailLog> PropertyStatisticsEmailLogPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return PropertyStatisticsEmailLogPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<PropertyStatisticsEmailLog> PropertyStatisticsEmailLogPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "PropertyStatisticsEmailLogID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<PropertyStatisticsEmailLog> objects;
			string baseKey = cacheKeyPrefix + "PropertyStatisticsEmailLogPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<PropertyStatisticsEmailLog> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<PropertyStatisticsEmailLog>();
				tmpList = Cache[key] as List<PropertyStatisticsEmailLog>;
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
					IQueryable<PropertyStatisticsEmailLog> itemQuery = SetupQuery(entity.PropertyStatisticsEmailLog, "PropertyStatisticsEmailLog", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_PropertyStatisticsEmailLog");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				return filterList;
			}
		}
	}
}