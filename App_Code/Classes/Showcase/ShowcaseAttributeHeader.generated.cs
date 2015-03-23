using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class ShowcaseAttributeHeader : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_ShowcaseAttributeHeader_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Text"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public ShowcaseAttributeHeader()
		{
		}

		public ShowcaseAttributeHeader(ShowcaseAttributeHeader objectToCopy)
		{
			NoPreferenceColumn = objectToCopy.NoPreferenceColumn;
			ShowcaseAttributeHeaderID = objectToCopy.ShowcaseAttributeHeaderID;
			ShowcaseAttributeID = objectToCopy.ShowcaseAttributeID;
			Text = objectToCopy.Text;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseAttributeHeaderID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("ShowcaseAttributeHeader", this);
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

		public static ShowcaseAttributeHeader GetByID(int ShowcaseAttributeHeaderID, IEnumerable<string> includeList = null)
		{
			ShowcaseAttributeHeader obj = null;
			string key = cacheKeyPrefix + ShowcaseAttributeHeaderID + GetCacheIncludeText(includeList);

			ShowcaseAttributeHeader tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as ShowcaseAttributeHeader;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseAttributeHeader> itemQuery = AddIncludes(entity.ShowcaseAttributeHeader, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseAttributeHeaderID == ShowcaseAttributeHeaderID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<ShowcaseAttributeHeader> GetAll(IEnumerable<string> includeList = null)
		{
			List<ShowcaseAttributeHeader> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<ShowcaseAttributeHeader> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseAttributeHeader>();
				tmpList = Cache[key] as List<ShowcaseAttributeHeader>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseAttributeHeader> itemQuery = AddIncludes(entity.ShowcaseAttributeHeader, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<ShowcaseAttributeHeader> ShowcaseAttributeHeaderGetByNoPreferenceColumn(Boolean NoPreferenceColumn, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseAttributeHeaderNoPreferenceColumn = NoPreferenceColumn.ToString();
			return ShowcaseAttributeHeaderPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseAttributeHeader> ShowcaseAttributeHeaderGetByShowcaseAttributeID(Int32 ShowcaseAttributeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseAttributeHeaderShowcaseAttributeID = ShowcaseAttributeID.ToString();
			return ShowcaseAttributeHeaderPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseAttributeHeader> ShowcaseAttributeHeaderGetByText(String Text, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseAttributeHeaderText = Text.ToString();
			return ShowcaseAttributeHeaderPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<ShowcaseAttributeHeader> ShowcaseAttributeHeaderPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<ShowcaseAttributeHeader> objects = ShowcaseAttributeHeaderPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseAttributeHeader> ShowcaseAttributeHeaderPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return ShowcaseAttributeHeaderPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<ShowcaseAttributeHeader> ShowcaseAttributeHeaderPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return ShowcaseAttributeHeaderPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<ShowcaseAttributeHeader> ShowcaseAttributeHeaderPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseAttributeHeaderID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<ShowcaseAttributeHeader> objects;
			string baseKey = cacheKeyPrefix + "ShowcaseAttributeHeaderPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseAttributeHeader> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseAttributeHeader>();
				tmpList = Cache[key] as List<ShowcaseAttributeHeader>;
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
					IQueryable<ShowcaseAttributeHeader> itemQuery = SetupQuery(entity.ShowcaseAttributeHeader, "ShowcaseAttributeHeader", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_ShowcaseAttributeHeader");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterShowcaseAttributeHeaderNoPreferenceColumn { get; set; }
			public string FilterShowcaseAttributeHeaderShowcaseAttributeID { get; set; }
			public string FilterShowcaseAttributeHeaderText { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterShowcaseAttributeHeaderNoPreferenceColumn != null)
				{
					if (FilterShowcaseAttributeHeaderNoPreferenceColumn == string.Empty)
						filterList.Add("@FilterShowcaseAttributeHeaderNoPreferenceColumn", string.Empty);
					else
						filterList.Add("@FilterShowcaseAttributeHeaderNoPreferenceColumn", Convert.ToBoolean(FilterShowcaseAttributeHeaderNoPreferenceColumn));
				}
				if (FilterShowcaseAttributeHeaderShowcaseAttributeID != null)
				{
					if (FilterShowcaseAttributeHeaderShowcaseAttributeID == string.Empty)
						filterList.Add("@FilterShowcaseAttributeHeaderShowcaseAttributeID", string.Empty);
					else
						filterList.Add("@FilterShowcaseAttributeHeaderShowcaseAttributeID", Convert.ToInt32(FilterShowcaseAttributeHeaderShowcaseAttributeID));
				}
				if (FilterShowcaseAttributeHeaderText != null)
					filterList.Add("@FilterShowcaseAttributeHeaderText", FilterShowcaseAttributeHeaderText);
				return filterList;
			}
		}
	}
}