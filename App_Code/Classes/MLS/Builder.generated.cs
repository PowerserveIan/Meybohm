using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.MLS
{
	public partial class Builder : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "MLS_Builder_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name", "OwnerName"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public Builder(Builder objectToCopy)
		{
			Active = objectToCopy.Active;
			BuilderID = objectToCopy.BuilderID;
			Image = objectToCopy.Image;
			Info = objectToCopy.Info;
			Name = objectToCopy.Name;
			OwnerName = objectToCopy.OwnerName;
			Website = objectToCopy.Website;
		}

		public virtual bool IsNewRecord
		{
			get { return BuilderID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("Builder", this);
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

		public static Builder GetByID(int BuilderID, IEnumerable<string> includeList = null)
		{
			Builder obj = null;
			string key = cacheKeyPrefix + BuilderID + GetCacheIncludeText(includeList);

			Builder tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Builder;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Builder> itemQuery = AddIncludes(entity.Builder, includeList);
					obj = itemQuery.FirstOrDefault(n => n.BuilderID == BuilderID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Builder> GetAll(IEnumerable<string> includeList = null)
		{
			List<Builder> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Builder> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Builder>();
				tmpList = Cache[key] as List<Builder>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Builder> itemQuery = AddIncludes(entity.Builder, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Builder> BuilderGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterBuilderActive = Active.ToString();
			return BuilderPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Builder> BuilderGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterBuilderName = Name.ToString();
			return BuilderPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Builder> BuilderGetByOwnerName(String OwnerName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterBuilderOwnerName = OwnerName.ToString();
			return BuilderPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Builder> BuilderPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Builder> objects = BuilderPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Builder> BuilderPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return BuilderPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Builder> BuilderPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return BuilderPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Builder> BuilderPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "BuilderID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Builder> objects;
			string baseKey = cacheKeyPrefix + "BuilderPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Builder> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Builder>();
				tmpList = Cache[key] as List<Builder>;
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
					IQueryable<Builder> itemQuery = SetupQuery(entity.Builder, "Builder", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("MLS_Builder");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterBuilderActive { get; set; }
			public string FilterBuilderName { get; set; }
			public string FilterBuilderOwnerName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterBuilderActive != null)
				{
					if (FilterBuilderActive == string.Empty)
						filterList.Add("@FilterBuilderActive", string.Empty);
					else
						filterList.Add("@FilterBuilderActive", Convert.ToBoolean(FilterBuilderActive));
				}
				if (FilterBuilderName != null)
					filterList.Add("@FilterBuilderName", FilterBuilderName);
				if (FilterBuilderOwnerName != null)
					filterList.Add("@FilterBuilderOwnerName", FilterBuilderOwnerName);
				return filterList;
			}
		}
	}
}