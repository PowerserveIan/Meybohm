using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_MembershipProvider
{
	public partial class Team : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_MembershipProvider_Team_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "MlsID", "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public Team(Team objectToCopy)
		{
			CMMicrositeID = objectToCopy.CMMicrositeID;
			DisplayInDirectory = objectToCopy.DisplayInDirectory;
			Email = objectToCopy.Email;
			MlsID = objectToCopy.MlsID;
			Name = objectToCopy.Name;
			Phone = objectToCopy.Phone;
			Photo = objectToCopy.Photo;
			TeamID = objectToCopy.TeamID;
		}

		public virtual bool IsNewRecord
		{
			get { return TeamID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("Team", this);
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

		public static Team GetByID(int TeamID, IEnumerable<string> includeList = null)
		{
			Team obj = null;
			string key = cacheKeyPrefix + TeamID + GetCacheIncludeText(includeList);

			Team tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Team;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Team> itemQuery = AddIncludes(entity.Team, includeList);
					obj = itemQuery.FirstOrDefault(n => n.TeamID == TeamID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Team> GetAll(IEnumerable<string> includeList = null)
		{
			List<Team> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Team> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Team>();
				tmpList = Cache[key] as List<Team>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Team> itemQuery = AddIncludes(entity.Team, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Team> TeamGetByCMMicrositeID(Int32 CMMicrositeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterTeamCMMicrositeID = CMMicrositeID.ToString();
			return TeamPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Team> TeamGetByDisplayInDirectory(Boolean DisplayInDirectory, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterTeamDisplayInDirectory = DisplayInDirectory.ToString();
			return TeamPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Team> TeamGetByMlsID(String MlsID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterTeamMlsID = MlsID.ToString();
			return TeamPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Team> TeamGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterTeamName = Name.ToString();
			return TeamPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Team> TeamPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Team> objects = TeamPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Team> TeamPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return TeamPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Team> TeamPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return TeamPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Team> TeamPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "TeamID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Team> objects;
			string baseKey = cacheKeyPrefix + "TeamPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Team> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Team>();
				tmpList = Cache[key] as List<Team>;
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
					IQueryable<Team> itemQuery = SetupQuery(entity.Team, "Team", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_MembershipProvider_Team");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterTeamCMMicrositeID { get; set; }
			public string FilterTeamDisplayInDirectory { get; set; }
			public string FilterTeamMlsID { get; set; }
			public string FilterTeamName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterTeamCMMicrositeID != null)
				{
					if (FilterTeamCMMicrositeID == string.Empty)
						filterList.Add("@FilterTeamCMMicrositeID", string.Empty);
					else
						filterList.Add("@FilterTeamCMMicrositeID", Convert.ToInt32(FilterTeamCMMicrositeID));
				}
				if (FilterTeamDisplayInDirectory != null)
				{
					if (FilterTeamDisplayInDirectory == string.Empty)
						filterList.Add("@FilterTeamDisplayInDirectory", string.Empty);
					else
						filterList.Add("@FilterTeamDisplayInDirectory", Convert.ToBoolean(FilterTeamDisplayInDirectory));
				}
				if (FilterTeamMlsID != null)
					filterList.Add("@FilterTeamMlsID", FilterTeamMlsID);
				if (FilterTeamName != null)
					filterList.Add("@FilterTeamName", FilterTeamName);
				return filterList;
			}
		}
	}
}