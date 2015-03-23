using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class ShowcaseItemAttributeValue : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_ShowcaseItemAttributeValue_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public ShowcaseItemAttributeValue()
		{
		}

		public ShowcaseItemAttributeValue(ShowcaseItemAttributeValue objectToCopy)
		{
			ShowcaseAttributeValueID = objectToCopy.ShowcaseAttributeValueID;
			ShowcaseItemAttributeValueID = objectToCopy.ShowcaseItemAttributeValueID;
			ShowcaseItemID = objectToCopy.ShowcaseItemID;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseItemAttributeValueID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("ShowcaseItemAttributeValue", this);
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

		public static ShowcaseItemAttributeValue GetByID(int ShowcaseItemAttributeValueID, IEnumerable<string> includeList = null)
		{
			ShowcaseItemAttributeValue obj = null;
			string key = cacheKeyPrefix + ShowcaseItemAttributeValueID + GetCacheIncludeText(includeList);

			ShowcaseItemAttributeValue tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as ShowcaseItemAttributeValue;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItemAttributeValue> itemQuery = AddIncludes(entity.ShowcaseItemAttributeValue, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseItemAttributeValueID == ShowcaseItemAttributeValueID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<ShowcaseItemAttributeValue> GetAll(IEnumerable<string> includeList = null)
		{
			List<ShowcaseItemAttributeValue> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<ShowcaseItemAttributeValue> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseItemAttributeValue>();
				tmpList = Cache[key] as List<ShowcaseItemAttributeValue>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItemAttributeValue> itemQuery = AddIncludes(entity.ShowcaseItemAttributeValue, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<ShowcaseItemAttributeValue> ShowcaseItemAttributeValueGetByShowcaseAttributeValueID(Int32 ShowcaseAttributeValueID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemAttributeValueShowcaseAttributeValueID = ShowcaseAttributeValueID.ToString();
			return ShowcaseItemAttributeValuePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseItemAttributeValue> ShowcaseItemAttributeValueGetByShowcaseItemID(Int32 ShowcaseItemID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseItemAttributeValueShowcaseItemID = ShowcaseItemID.ToString();
			return ShowcaseItemAttributeValuePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<ShowcaseItemAttributeValue> ShowcaseItemAttributeValuePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<ShowcaseItemAttributeValue> objects = ShowcaseItemAttributeValuePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseItemAttributeValue> ShowcaseItemAttributeValuePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return ShowcaseItemAttributeValuePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<ShowcaseItemAttributeValue> ShowcaseItemAttributeValuePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return ShowcaseItemAttributeValuePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<ShowcaseItemAttributeValue> ShowcaseItemAttributeValuePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseItemAttributeValueID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<ShowcaseItemAttributeValue> objects;
			string baseKey = cacheKeyPrefix + "ShowcaseItemAttributeValuePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseItemAttributeValue> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseItemAttributeValue>();
				tmpList = Cache[key] as List<ShowcaseItemAttributeValue>;
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
					IQueryable<ShowcaseItemAttributeValue> itemQuery = SetupQuery(entity.ShowcaseItemAttributeValue, "ShowcaseItemAttributeValue", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_ShowcaseItemAttributeValue");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterShowcaseItemAttributeValueShowcaseAttributeValueID { get; set; }
			public string FilterShowcaseItemAttributeValueShowcaseItemID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterShowcaseItemAttributeValueShowcaseAttributeValueID != null)
				{
					if (FilterShowcaseItemAttributeValueShowcaseAttributeValueID == string.Empty)
						filterList.Add("@FilterShowcaseItemAttributeValueShowcaseAttributeValueID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemAttributeValueShowcaseAttributeValueID", Convert.ToInt32(FilterShowcaseItemAttributeValueShowcaseAttributeValueID));
				}
				if (FilterShowcaseItemAttributeValueShowcaseItemID != null)
				{
					if (FilterShowcaseItemAttributeValueShowcaseItemID == string.Empty)
						filterList.Add("@FilterShowcaseItemAttributeValueShowcaseItemID", string.Empty);
					else
						filterList.Add("@FilterShowcaseItemAttributeValueShowcaseItemID", Convert.ToInt32(FilterShowcaseItemAttributeValueShowcaseItemID));
				}
				return filterList;
			}
		}
	}
}