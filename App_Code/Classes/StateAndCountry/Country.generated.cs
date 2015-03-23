using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.StateAndCountry
{
	public partial class Country : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "StateAndCountry_Country_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Abb", "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public Country(Country objectToCopy)
		{
			Abb = objectToCopy.Abb;
			CountryID = objectToCopy.CountryID;
			Name = objectToCopy.Name;
		}

		public virtual bool IsNewRecord
		{
			get { return CountryID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("Country", this);
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

		public static Country GetByID(int CountryID, IEnumerable<string> includeList = null)
		{
			Country obj = null;
			string key = cacheKeyPrefix + CountryID + GetCacheIncludeText(includeList);

			Country tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Country;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Country> itemQuery = AddIncludes(entity.Country, includeList);
					obj = itemQuery.FirstOrDefault(n => n.CountryID == CountryID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Country> GetAll(IEnumerable<string> includeList = null)
		{
			List<Country> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Country> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Country>();
				tmpList = Cache[key] as List<Country>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Country> itemQuery = AddIncludes(entity.Country, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Country> CountryGetByAbb(String Abb, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCountryAbb = Abb.ToString();
			return CountryPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Country> CountryGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCountryName = Name.ToString();
			return CountryPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Country> CountryPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Country> objects = CountryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Country> CountryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return CountryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Country> CountryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return CountryPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Country> CountryPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "CountryID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Country> objects;
			string baseKey = cacheKeyPrefix + "CountryPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Country> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Country>();
				tmpList = Cache[key] as List<Country>;
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
					IQueryable<Country> itemQuery = SetupQuery(entity.Country, "Country", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("StateAndCountry_Country");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterCountryAbb { get; set; }
			public string FilterCountryName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterCountryAbb != null)
					filterList.Add("@FilterCountryAbb", FilterCountryAbb);
				if (FilterCountryName != null)
					filterList.Add("@FilterCountryName", FilterCountryName);
				return filterList;
			}
		}
	}
}