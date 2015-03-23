using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.MLS
{
	public partial class NeighborhoodBuilder : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "MLS_NeighborhoodBuilder_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public NeighborhoodBuilder()
		{
		}

		public NeighborhoodBuilder(NeighborhoodBuilder objectToCopy)
		{
			BuilderID = objectToCopy.BuilderID;
			NeighborhoodBuilderID = objectToCopy.NeighborhoodBuilderID;
			NeighborhoodID = objectToCopy.NeighborhoodID;
		}

		public virtual bool IsNewRecord
		{
			get { return NeighborhoodBuilderID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("NeighborhoodBuilder", this);
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

		public static NeighborhoodBuilder GetByID(int NeighborhoodBuilderID, IEnumerable<string> includeList = null)
		{
			NeighborhoodBuilder obj = null;
			string key = cacheKeyPrefix + NeighborhoodBuilderID + GetCacheIncludeText(includeList);

			NeighborhoodBuilder tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as NeighborhoodBuilder;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NeighborhoodBuilder> itemQuery = AddIncludes(entity.NeighborhoodBuilder, includeList);
					obj = itemQuery.FirstOrDefault(n => n.NeighborhoodBuilderID == NeighborhoodBuilderID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<NeighborhoodBuilder> GetAll(IEnumerable<string> includeList = null)
		{
			List<NeighborhoodBuilder> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<NeighborhoodBuilder> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NeighborhoodBuilder>();
				tmpList = Cache[key] as List<NeighborhoodBuilder>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<NeighborhoodBuilder> itemQuery = AddIncludes(entity.NeighborhoodBuilder, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<NeighborhoodBuilder> NeighborhoodBuilderGetByBuilderID(Int32 BuilderID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNeighborhoodBuilderBuilderID = BuilderID.ToString();
			return NeighborhoodBuilderPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<NeighborhoodBuilder> NeighborhoodBuilderGetByNeighborhoodID(Int32 NeighborhoodID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNeighborhoodBuilderNeighborhoodID = NeighborhoodID.ToString();
			return NeighborhoodBuilderPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<NeighborhoodBuilder> NeighborhoodBuilderPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<NeighborhoodBuilder> objects = NeighborhoodBuilderPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<NeighborhoodBuilder> NeighborhoodBuilderPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return NeighborhoodBuilderPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<NeighborhoodBuilder> NeighborhoodBuilderPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return NeighborhoodBuilderPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<NeighborhoodBuilder> NeighborhoodBuilderPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "NeighborhoodBuilderID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<NeighborhoodBuilder> objects;
			string baseKey = cacheKeyPrefix + "NeighborhoodBuilderPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<NeighborhoodBuilder> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<NeighborhoodBuilder>();
				tmpList = Cache[key] as List<NeighborhoodBuilder>;
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
					IQueryable<NeighborhoodBuilder> itemQuery = SetupQuery(entity.NeighborhoodBuilder, "NeighborhoodBuilder", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("MLS_NeighborhoodBuilder");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterNeighborhoodBuilderBuilderID { get; set; }
			public string FilterNeighborhoodBuilderNeighborhoodID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterNeighborhoodBuilderBuilderID != null)
				{
					if (FilterNeighborhoodBuilderBuilderID == string.Empty)
						filterList.Add("@FilterNeighborhoodBuilderBuilderID", string.Empty);
					else
						filterList.Add("@FilterNeighborhoodBuilderBuilderID", Convert.ToInt32(FilterNeighborhoodBuilderBuilderID));
				}
				if (FilterNeighborhoodBuilderNeighborhoodID != null)
				{
					if (FilterNeighborhoodBuilderNeighborhoodID == string.Empty)
						filterList.Add("@FilterNeighborhoodBuilderNeighborhoodID", string.Empty);
					else
						filterList.Add("@FilterNeighborhoodBuilderNeighborhoodID", Convert.ToInt32(FilterNeighborhoodBuilderNeighborhoodID));
				}
				return filterList;
			}
		}
	}
}