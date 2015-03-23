using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.StateAndCountry
{
	public partial class State : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "StateAndCountry_State_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Abb", "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public State(State objectToCopy)
		{
			Abb = objectToCopy.Abb;
			CountryID = objectToCopy.CountryID;
			Name = objectToCopy.Name;
			ShippingMarkup = objectToCopy.ShippingMarkup;
			ShipTo = objectToCopy.ShipTo;
			StateID = objectToCopy.StateID;
		}

		public virtual bool IsNewRecord
		{
			get { return StateID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("State", this);
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

		public static State GetByID(int StateID, IEnumerable<string> includeList = null)
		{
			State obj = null;
			string key = cacheKeyPrefix + StateID + GetCacheIncludeText(includeList);

			State tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as State;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<State> itemQuery = AddIncludes(entity.State, includeList);
					obj = itemQuery.FirstOrDefault(n => n.StateID == StateID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<State> GetAll(IEnumerable<string> includeList = null)
		{
			List<State> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<State> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<State>();
				tmpList = Cache[key] as List<State>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<State> itemQuery = AddIncludes(entity.State, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<State> StateGetByAbb(String Abb, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterStateAbb = Abb.ToString();
			return StatePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<State> StateGetByCountryID(Int32 CountryID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterStateCountryID = CountryID.ToString();
			return StatePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<State> StateGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterStateName = Name.ToString();
			return StatePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<State> StateGetByShipTo(Boolean ShipTo, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterStateShipTo = ShipTo.ToString();
			return StatePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<State> StatePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<State> objects = StatePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<State> StatePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return StatePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<State> StatePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return StatePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<State> StatePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "StateID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<State> objects;
			string baseKey = cacheKeyPrefix + "StatePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<State> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<State>();
				tmpList = Cache[key] as List<State>;
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
					IQueryable<State> itemQuery = SetupQuery(entity.State, "State", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("StateAndCountry_State");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterStateAbb { get; set; }
			public string FilterStateCountryID { get; set; }
			public string FilterStateName { get; set; }
			public string FilterStateShipTo { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterStateAbb != null)
					filterList.Add("@FilterStateAbb", FilterStateAbb);
				if (FilterStateCountryID != null)
				{
					if (FilterStateCountryID == string.Empty)
						filterList.Add("@FilterStateCountryID", string.Empty);
					else
						filterList.Add("@FilterStateCountryID", Convert.ToInt32(FilterStateCountryID));
				}
				if (FilterStateName != null)
					filterList.Add("@FilterStateName", FilterStateName);
				if (FilterStateShipTo != null)
				{
					if (FilterStateShipTo == string.Empty)
						filterList.Add("@FilterStateShipTo", string.Empty);
					else
						filterList.Add("@FilterStateShipTo", Convert.ToBoolean(FilterStateShipTo));
				}
				return filterList;
			}
		}
	}
}