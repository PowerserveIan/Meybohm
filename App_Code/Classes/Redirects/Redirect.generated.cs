using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Redirects
{
	public partial class Redirect : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Redirects_Redirect_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "NewUrl", "OldUrl"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public Redirect()
		{
		}

		public Redirect(Redirect objectToCopy)
		{
			NewUrl = objectToCopy.NewUrl;
			OldUrl = objectToCopy.OldUrl;
			RedirectID = objectToCopy.RedirectID;
		}

		public virtual bool IsNewRecord
		{
			get { return RedirectID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("Redirect", this);
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

		public static Redirect GetByID(int RedirectID, IEnumerable<string> includeList = null)
		{
			Redirect obj = null;
			string key = cacheKeyPrefix + RedirectID + GetCacheIncludeText(includeList);

			Redirect tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Redirect;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Redirect> itemQuery = AddIncludes(entity.Redirect, includeList);
					obj = itemQuery.FirstOrDefault(n => n.RedirectID == RedirectID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Redirect> GetAll(IEnumerable<string> includeList = null)
		{
			List<Redirect> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Redirect> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Redirect>();
				tmpList = Cache[key] as List<Redirect>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Redirect> itemQuery = AddIncludes(entity.Redirect, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Redirect> RedirectGetByNewUrl(String NewUrl, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterRedirectNewUrl = NewUrl.ToString();
			return RedirectPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Redirect> RedirectGetByOldUrl(String OldUrl, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterRedirectOldUrl = OldUrl.ToString();
			return RedirectPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Redirect> RedirectPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Redirect> objects = RedirectPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Redirect> RedirectPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return RedirectPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Redirect> RedirectPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return RedirectPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Redirect> RedirectPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "RedirectID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Redirect> objects;
			string baseKey = cacheKeyPrefix + "RedirectPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Redirect> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Redirect>();
				tmpList = Cache[key] as List<Redirect>;
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
					IQueryable<Redirect> itemQuery = SetupQuery(entity.Redirect, "Redirect", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Redirects_Redirect");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterRedirectNewUrl { get; set; }
			public string FilterRedirectOldUrl { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterRedirectNewUrl != null)
					filterList.Add("@FilterRedirectNewUrl", FilterRedirectNewUrl);
				if (FilterRedirectOldUrl != null)
					filterList.Add("@FilterRedirectOldUrl", FilterRedirectOldUrl);
				return filterList;
			}
		}
	}
}