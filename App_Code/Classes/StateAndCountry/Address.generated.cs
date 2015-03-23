
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.StateAndCountry
{
	public partial class Address : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "StateAndCountry_Address_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "City", "Zip"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public Address(Address objectToCopy)
		{
			Address1 = objectToCopy.Address1;
			Address2 = objectToCopy.Address2;
			AddressID = objectToCopy.AddressID;
			City = objectToCopy.City;
			Latitude = objectToCopy.Latitude;
			Longitude = objectToCopy.Longitude;
			StateID = objectToCopy.StateID;
			Zip = objectToCopy.Zip;
		}

		public virtual bool IsNewRecord
		{
			get { return AddressID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("Address", this);
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

		public static Address GetByID(int AddressID, IEnumerable<string> includeList = null)
		{
			Address obj = null;
			string key = cacheKeyPrefix + AddressID + GetCacheIncludeText(includeList);

			Address tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Address;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Address> itemQuery = AddIncludes(entity.Address, includeList);
					obj = itemQuery.FirstOrDefault(n => n.AddressID == AddressID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Address> GetAll(IEnumerable<string> includeList = null)
		{
			List<Address> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Address> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Address>();
				tmpList = Cache[key] as List<Address>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Address> itemQuery = AddIncludes(entity.Address, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Address> AddressGetByCity(String City, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterAddressCity = City.ToString();
			return AddressPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Address> AddressGetByStateID(Int32? StateID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterAddressStateID = StateID.ToString();
			return AddressPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Address> AddressGetByZip(String Zip, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterAddressZip = Zip.ToString();
			return AddressPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Address> AddressPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Address> objects = AddressPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Address> AddressPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return AddressPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Address> AddressPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return AddressPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Address> AddressPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "AddressID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Address> objects;
			string baseKey = cacheKeyPrefix + "AddressPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Address> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Address>();
				tmpList = Cache[key] as List<Address>;
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
					IQueryable<Address> itemQuery = SetupQuery(entity.Address, "Address", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("StateAndCountry_Address");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterAddressCity { get; set; }
			public string FilterAddressStateID { get; set; }
			public string FilterAddressZip { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterAddressCity != null)
					filterList.Add("@FilterAddressCity", FilterAddressCity);
				if (FilterAddressStateID != null)
				{
					if (FilterAddressStateID == string.Empty)
						filterList.Add("@FilterAddressStateID", string.Empty);
					else
						filterList.Add("@FilterAddressStateID", Convert.ToInt32(FilterAddressStateID));
				}
				if (FilterAddressZip != null)
					filterList.Add("@FilterAddressZip", FilterAddressZip);
				return filterList;
			}
		}
	}
}