using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class PropertyChangeLog : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_PropertyChangeLog_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Attribute"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public PropertyChangeLog()
		{
		}

		public PropertyChangeLog(PropertyChangeLog objectToCopy)
		{
			Attribute = objectToCopy.Attribute;
			DateStamp = objectToCopy.DateStamp;
			NewValue = objectToCopy.NewValue;
			OldValue = objectToCopy.OldValue;
			PropertyChangeLogID = objectToCopy.PropertyChangeLogID;
			ShowcaseItemID = objectToCopy.ShowcaseItemID;
		}

		public virtual bool IsNewRecord
		{
			get { return PropertyChangeLogID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime DateStampClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(DateStamp); }
		}

		public virtual void Save()
		{
			SaveEntity("PropertyChangeLog", this);
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

		public static PropertyChangeLog GetByID(int PropertyChangeLogID, IEnumerable<string> includeList = null)
		{
			PropertyChangeLog obj = null;
			string key = cacheKeyPrefix + PropertyChangeLogID + GetCacheIncludeText(includeList);

			PropertyChangeLog tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as PropertyChangeLog;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<PropertyChangeLog> itemQuery = AddIncludes(entity.PropertyChangeLog, includeList);
					obj = itemQuery.FirstOrDefault(n => n.PropertyChangeLogID == PropertyChangeLogID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<PropertyChangeLog> GetAll(IEnumerable<string> includeList = null)
		{
			List<PropertyChangeLog> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<PropertyChangeLog> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<PropertyChangeLog>();
				tmpList = Cache[key] as List<PropertyChangeLog>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<PropertyChangeLog> itemQuery = AddIncludes(entity.PropertyChangeLog, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<PropertyChangeLog> PropertyChangeLogGetByAttribute(String Attribute, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterPropertyChangeLogAttribute = Attribute.ToString();
			return PropertyChangeLogPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<PropertyChangeLog> PropertyChangeLogGetByShowcaseItemID(Int32 ShowcaseItemID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterPropertyChangeLogShowcaseItemID = ShowcaseItemID.ToString();
			return PropertyChangeLogPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<PropertyChangeLog> PropertyChangeLogPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<PropertyChangeLog> objects = PropertyChangeLogPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<PropertyChangeLog> PropertyChangeLogPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return PropertyChangeLogPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<PropertyChangeLog> PropertyChangeLogPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return PropertyChangeLogPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<PropertyChangeLog> PropertyChangeLogPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "PropertyChangeLogID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<PropertyChangeLog> objects;
			string baseKey = cacheKeyPrefix + "PropertyChangeLogPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<PropertyChangeLog> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<PropertyChangeLog>();
				tmpList = Cache[key] as List<PropertyChangeLog>;
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
					IQueryable<PropertyChangeLog> itemQuery = SetupQuery(entity.PropertyChangeLog, "PropertyChangeLog", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_PropertyChangeLog");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterPropertyChangeLogAttribute { get; set; }
			public string FilterPropertyChangeLogShowcaseItemID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterPropertyChangeLogAttribute != null)
					filterList.Add("@FilterPropertyChangeLogAttribute", FilterPropertyChangeLogAttribute);
				if (FilterPropertyChangeLogShowcaseItemID != null)
				{
					if (FilterPropertyChangeLogShowcaseItemID == string.Empty)
						filterList.Add("@FilterPropertyChangeLogShowcaseItemID", string.Empty);
					else
						filterList.Add("@FilterPropertyChangeLogShowcaseItemID", Convert.ToInt32(FilterPropertyChangeLogShowcaseItemID));
				}
				return filterList;
			}
		}
	}
}