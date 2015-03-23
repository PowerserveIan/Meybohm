using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.MLS
{
	public partial class Office : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "MLS_Office_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public Office(Office objectToCopy)
		{
			Active = objectToCopy.Active;
			AddressID = objectToCopy.AddressID;
			CMMicrositeID = objectToCopy.CMMicrositeID;
			Fax = objectToCopy.Fax;
			HasNewHomes = objectToCopy.HasNewHomes;
			HasRentals = objectToCopy.HasRentals;
			Image = objectToCopy.Image;
			IsMeybohm = objectToCopy.IsMeybohm;
			MlsID = objectToCopy.MlsID;
			Name = objectToCopy.Name;
			OfficeID = objectToCopy.OfficeID;
			Phone = objectToCopy.Phone;
		}

		public virtual bool IsNewRecord
		{
			get { return OfficeID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("Office", this);
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

		public static Office GetByID(int OfficeID, IEnumerable<string> includeList = null)
		{
			Office obj = null;
			string key = cacheKeyPrefix + OfficeID + GetCacheIncludeText(includeList);

			Office tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Office;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Office> itemQuery = AddIncludes(entity.Office, includeList);
					obj = itemQuery.FirstOrDefault(n => n.OfficeID == OfficeID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Office> GetAll(IEnumerable<string> includeList = null)
		{
			List<Office> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Office> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Office>();
				tmpList = Cache[key] as List<Office>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Office> itemQuery = AddIncludes(entity.Office, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Office> OfficeGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterOfficeActive = Active.ToString();
			return OfficePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Office> OfficeGetByCMMicrositeID(Int32 CMMicrositeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterOfficeCMMicrositeID = CMMicrositeID.ToString();
			return OfficePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Office> OfficeGetByHasNewHomes(Boolean HasNewHomes, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterOfficeHasNewHomes = HasNewHomes.ToString();
			return OfficePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Office> OfficeGetByHasRentals(Boolean HasRentals, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterOfficeHasRentals = HasRentals.ToString();
			return OfficePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Office> OfficeGetByIsMeybohm(Boolean IsMeybohm, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterOfficeIsMeybohm = IsMeybohm.ToString();
			return OfficePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Office> OfficeGetByMlsID(Int32 MlsID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterOfficeMlsID = MlsID.ToString();
			return OfficePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Office> OfficeGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterOfficeName = Name.ToString();
			return OfficePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Office> OfficePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Office> objects = OfficePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Office> OfficePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return OfficePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Office> OfficePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return OfficePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Office> OfficePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "OfficeID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Office> objects;
			string baseKey = cacheKeyPrefix + "OfficePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Office> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Office>();
				tmpList = Cache[key] as List<Office>;
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
					IQueryable<Office> itemQuery = SetupQuery(entity.Office, "Office", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("MLS_Office");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterOfficeActive { get; set; }
			public string FilterOfficeCMMicrositeID { get; set; }
			public string FilterOfficeHasNewHomes { get; set; }
			public string FilterOfficeHasRentals { get; set; }
			public string FilterOfficeIsMeybohm { get; set; }
			public string FilterOfficeMlsID { get; set; }
			public string FilterOfficeName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterOfficeActive != null)
				{
					if (FilterOfficeActive == string.Empty)
						filterList.Add("@FilterOfficeActive", string.Empty);
					else
						filterList.Add("@FilterOfficeActive", Convert.ToBoolean(FilterOfficeActive));
				}
				if (FilterOfficeCMMicrositeID != null)
				{
					if (FilterOfficeCMMicrositeID == string.Empty)
						filterList.Add("@FilterOfficeCMMicrositeID", string.Empty);
					else
						filterList.Add("@FilterOfficeCMMicrositeID", Convert.ToInt32(FilterOfficeCMMicrositeID));
				}
				if (FilterOfficeHasNewHomes != null)
				{
					if (FilterOfficeHasNewHomes == string.Empty)
						filterList.Add("@FilterOfficeHasNewHomes", string.Empty);
					else
						filterList.Add("@FilterOfficeHasNewHomes", Convert.ToBoolean(FilterOfficeHasNewHomes));
				}
				if (FilterOfficeHasRentals != null)
				{
					if (FilterOfficeHasRentals == string.Empty)
						filterList.Add("@FilterOfficeHasRentals", string.Empty);
					else
						filterList.Add("@FilterOfficeHasRentals", Convert.ToBoolean(FilterOfficeHasRentals));
				}
				if (FilterOfficeIsMeybohm != null)
				{
					if (FilterOfficeIsMeybohm == string.Empty)
						filterList.Add("@FilterOfficeIsMeybohm", string.Empty);
					else
						filterList.Add("@FilterOfficeIsMeybohm", Convert.ToBoolean(FilterOfficeIsMeybohm));
				}
				if (FilterOfficeMlsID != null)
				{
					if (FilterOfficeMlsID == string.Empty)
						filterList.Add("@FilterOfficeMlsID", string.Empty);
					else
						filterList.Add("@FilterOfficeMlsID", Convert.ToInt32(FilterOfficeMlsID));
				}
				if (FilterOfficeName != null)
					filterList.Add("@FilterOfficeName", FilterOfficeName);
				return filterList;
			}
		}
	}
}