using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_MembershipProvider
{
	public partial class TeamLanguageSpoken : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_MembershipProvider_TeamLanguageSpoken_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public TeamLanguageSpoken()
		{
		}

		public TeamLanguageSpoken(TeamLanguageSpoken objectToCopy)
		{
			LanguageID = objectToCopy.LanguageID;
			TeamID = objectToCopy.TeamID;
			TeamLanguageSpokenID = objectToCopy.TeamLanguageSpokenID;
		}

		public virtual bool IsNewRecord
		{
			get { return TeamLanguageSpokenID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("TeamLanguageSpoken", this);
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

		public static TeamLanguageSpoken GetByID(int TeamLanguageSpokenID, IEnumerable<string> includeList = null)
		{
			TeamLanguageSpoken obj = null;
			string key = cacheKeyPrefix + TeamLanguageSpokenID + GetCacheIncludeText(includeList);

			TeamLanguageSpoken tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as TeamLanguageSpoken;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<TeamLanguageSpoken> itemQuery = AddIncludes(entity.TeamLanguageSpoken, includeList);
					obj = itemQuery.FirstOrDefault(n => n.TeamLanguageSpokenID == TeamLanguageSpokenID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<TeamLanguageSpoken> GetAll(IEnumerable<string> includeList = null)
		{
			List<TeamLanguageSpoken> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<TeamLanguageSpoken> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<TeamLanguageSpoken>();
				tmpList = Cache[key] as List<TeamLanguageSpoken>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<TeamLanguageSpoken> itemQuery = AddIncludes(entity.TeamLanguageSpoken, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<TeamLanguageSpoken> TeamLanguageSpokenGetByLanguageID(Int32 LanguageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterTeamLanguageSpokenLanguageID = LanguageID.ToString();
			return TeamLanguageSpokenPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<TeamLanguageSpoken> TeamLanguageSpokenGetByTeamID(Int32 TeamID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterTeamLanguageSpokenTeamID = TeamID.ToString();
			return TeamLanguageSpokenPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<TeamLanguageSpoken> TeamLanguageSpokenPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<TeamLanguageSpoken> objects = TeamLanguageSpokenPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<TeamLanguageSpoken> TeamLanguageSpokenPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return TeamLanguageSpokenPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<TeamLanguageSpoken> TeamLanguageSpokenPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return TeamLanguageSpokenPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<TeamLanguageSpoken> TeamLanguageSpokenPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "TeamLanguageSpokenID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<TeamLanguageSpoken> objects;
			string baseKey = cacheKeyPrefix + "TeamLanguageSpokenPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<TeamLanguageSpoken> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<TeamLanguageSpoken>();
				tmpList = Cache[key] as List<TeamLanguageSpoken>;
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
					IQueryable<TeamLanguageSpoken> itemQuery = SetupQuery(entity.TeamLanguageSpoken, "TeamLanguageSpoken", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_MembershipProvider_TeamLanguageSpoken");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterTeamLanguageSpokenLanguageID { get; set; }
			public string FilterTeamLanguageSpokenTeamID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterTeamLanguageSpokenLanguageID != null)
				{
					if (FilterTeamLanguageSpokenLanguageID == string.Empty)
						filterList.Add("@FilterTeamLanguageSpokenLanguageID", string.Empty);
					else
						filterList.Add("@FilterTeamLanguageSpokenLanguageID", Convert.ToInt32(FilterTeamLanguageSpokenLanguageID));
				}
				if (FilterTeamLanguageSpokenTeamID != null)
				{
					if (FilterTeamLanguageSpokenTeamID == string.Empty)
						filterList.Add("@FilterTeamLanguageSpokenTeamID", string.Empty);
					else
						filterList.Add("@FilterTeamLanguageSpokenTeamID", Convert.ToInt32(FilterTeamLanguageSpokenTeamID));
				}
				return filterList;
			}
		}
	}
}