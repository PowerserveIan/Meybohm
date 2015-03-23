using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public partial class ShowcaseAttribute : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Showcase_ShowcaseAttribute_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "MLSAttributeName", "Title"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public ShowcaseAttribute(ShowcaseAttribute objectToCopy)
		{
			Active = objectToCopy.Active;
			DisplayOrder = objectToCopy.DisplayOrder;
			ImportItemAttribute = objectToCopy.ImportItemAttribute;
			MaximumValue = objectToCopy.MaximumValue;
			MinimumValue = objectToCopy.MinimumValue;
			MLSAttributeName = objectToCopy.MLSAttributeName;
			Numeric = objectToCopy.Numeric;
			ShowcaseAttributeID = objectToCopy.ShowcaseAttributeID;
			ShowcaseFilterID = objectToCopy.ShowcaseFilterID;
			ShowcaseID = objectToCopy.ShowcaseID;
			SingleItemValue = objectToCopy.SingleItemValue;
			Title = objectToCopy.Title;
		}

		public virtual bool IsNewRecord
		{
			get { return ShowcaseAttributeID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("ShowcaseAttribute", this);
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

		public static ShowcaseAttribute GetByID(int ShowcaseAttributeID, IEnumerable<string> includeList = null)
		{
			ShowcaseAttribute obj = null;
			string key = cacheKeyPrefix + ShowcaseAttributeID + GetCacheIncludeText(includeList);

			ShowcaseAttribute tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as ShowcaseAttribute;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseAttribute> itemQuery = AddIncludes(entity.ShowcaseAttribute, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ShowcaseAttributeID == ShowcaseAttributeID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<ShowcaseAttribute> GetAll(IEnumerable<string> includeList = null)
		{
			List<ShowcaseAttribute> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<ShowcaseAttribute> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseAttribute>();
				tmpList = Cache[key] as List<ShowcaseAttribute>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseAttribute> itemQuery = AddIncludes(entity.ShowcaseAttribute, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<ShowcaseAttribute> ShowcaseAttributeGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseAttributeActive = Active.ToString();
			return ShowcaseAttributePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseAttribute> ShowcaseAttributeGetByDisplayOrder(Int16? DisplayOrder, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseAttributeDisplayOrder = DisplayOrder.ToString();
			return ShowcaseAttributePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseAttribute> ShowcaseAttributeGetByImportItemAttribute(Boolean ImportItemAttribute, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseAttributeImportItemAttribute = ImportItemAttribute.ToString();
			return ShowcaseAttributePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseAttribute> ShowcaseAttributeGetByMLSAttributeName(String MLSAttributeName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseAttributeMLSAttributeName = MLSAttributeName.ToString();
			return ShowcaseAttributePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseAttribute> ShowcaseAttributeGetByNumeric(Boolean Numeric, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseAttributeNumeric = Numeric.ToString();
			return ShowcaseAttributePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseAttribute> ShowcaseAttributeGetByShowcaseFilterID(Int32 ShowcaseFilterID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseAttributeShowcaseFilterID = ShowcaseFilterID.ToString();
			return ShowcaseAttributePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseAttribute> ShowcaseAttributeGetByShowcaseID(Int32 ShowcaseID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseAttributeShowcaseID = ShowcaseID.ToString();
			return ShowcaseAttributePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<ShowcaseAttribute> ShowcaseAttributeGetByTitle(String Title, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterShowcaseAttributeTitle = Title.ToString();
			return ShowcaseAttributePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<ShowcaseAttribute> ShowcaseAttributePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<ShowcaseAttribute> objects = ShowcaseAttributePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseAttribute> ShowcaseAttributePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return ShowcaseAttributePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<ShowcaseAttribute> ShowcaseAttributePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return ShowcaseAttributePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<ShowcaseAttribute> ShowcaseAttributePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseAttributeID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<ShowcaseAttribute> objects;
			string baseKey = cacheKeyPrefix + "ShowcaseAttributePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseAttribute> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseAttribute>();
				tmpList = Cache[key] as List<ShowcaseAttribute>;
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
					IQueryable<ShowcaseAttribute> itemQuery = SetupQuery(entity.ShowcaseAttribute, "ShowcaseAttribute", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Showcase_ShowcaseAttribute");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterShowcaseAttributeActive { get; set; }
			public string FilterShowcaseAttributeDisplayOrder { get; set; }
			public string FilterShowcaseAttributeImportItemAttribute { get; set; }
			public string FilterShowcaseAttributeMLSAttributeName { get; set; }
			public string FilterShowcaseAttributeNumeric { get; set; }
			public string FilterShowcaseAttributeShowcaseFilterID { get; set; }
			public string FilterShowcaseAttributeShowcaseID { get; set; }
			public string FilterShowcaseAttributeTitle { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterShowcaseAttributeActive != null)
				{
					if (FilterShowcaseAttributeActive == string.Empty)
						filterList.Add("@FilterShowcaseAttributeActive", string.Empty);
					else
						filterList.Add("@FilterShowcaseAttributeActive", Convert.ToBoolean(FilterShowcaseAttributeActive));
				}
				if (FilterShowcaseAttributeDisplayOrder != null)
				{
					if (FilterShowcaseAttributeDisplayOrder == string.Empty)
						filterList.Add("@FilterShowcaseAttributeDisplayOrder", string.Empty);
					else
						filterList.Add("@FilterShowcaseAttributeDisplayOrder", Convert.ToInt16(FilterShowcaseAttributeDisplayOrder));
				}
				if (FilterShowcaseAttributeImportItemAttribute != null)
				{
					if (FilterShowcaseAttributeImportItemAttribute == string.Empty)
						filterList.Add("@FilterShowcaseAttributeImportItemAttribute", string.Empty);
					else
						filterList.Add("@FilterShowcaseAttributeImportItemAttribute", Convert.ToBoolean(FilterShowcaseAttributeImportItemAttribute));
				}
				if (FilterShowcaseAttributeMLSAttributeName != null)
					filterList.Add("@FilterShowcaseAttributeMLSAttributeName", FilterShowcaseAttributeMLSAttributeName);
				if (FilterShowcaseAttributeNumeric != null)
				{
					if (FilterShowcaseAttributeNumeric == string.Empty)
						filterList.Add("@FilterShowcaseAttributeNumeric", string.Empty);
					else
						filterList.Add("@FilterShowcaseAttributeNumeric", Convert.ToBoolean(FilterShowcaseAttributeNumeric));
				}
				if (FilterShowcaseAttributeShowcaseFilterID != null)
				{
					if (FilterShowcaseAttributeShowcaseFilterID == string.Empty)
						filterList.Add("@FilterShowcaseAttributeShowcaseFilterID", string.Empty);
					else
						filterList.Add("@FilterShowcaseAttributeShowcaseFilterID", Convert.ToInt32(FilterShowcaseAttributeShowcaseFilterID));
				}
				if (FilterShowcaseAttributeShowcaseID != null)
				{
					if (FilterShowcaseAttributeShowcaseID == string.Empty)
						filterList.Add("@FilterShowcaseAttributeShowcaseID", string.Empty);
					else
						filterList.Add("@FilterShowcaseAttributeShowcaseID", Convert.ToInt32(FilterShowcaseAttributeShowcaseID));
				}
				if (FilterShowcaseAttributeTitle != null)
					filterList.Add("@FilterShowcaseAttributeTitle", FilterShowcaseAttributeTitle);
				return filterList;
			}
		}
	}
}