using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace Classes.Media352_MembershipProvider
{
	public partial class UserInfo
	{
		public string FirstAndLast
		{
			get { return FirstName + (!String.IsNullOrEmpty(LastName) ? " " + LastName : ""); }
		}

		public static List<UserInfo> GetAllAgents(int? showcaseID = null)
		{
			List<UserInfo> objects;
			string key = cacheKeyPrefix + "GetAllAgents_" + showcaseID;

			List<UserInfo> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<UserInfo>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<UserInfo> itemQuery = entity.UserInfo.Where(u => u.User.UserRole.Any(r => r.RoleID == (int)RolesEnum.Agent));
					if (showcaseID.HasValue)
						itemQuery = itemQuery.Where(u => u.User.ShowcaseItem.Any(s => s.ShowcaseID == showcaseID.Value));
					objects = itemQuery.OrderBy(u => u.FirstName).ThenBy(u => u.LastName).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static int SelectCount(string searchText, string letter, Filters filterList)
		{
			return SelectCount();
		}

		public static List<UserInfo> PageAgentsForStaffDirectory(int startRowIndex, int maximumRows, string searchText, string letter, Filters filterList = new Filters())
		{
			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText);

			List<UserInfo> objects;
			string baseKey = cacheKeyPrefix + "PageAgentsForStaffDirectory_" + cachingFilterText + "_" + letter;
			string key = baseKey + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<UserInfo> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<UserInfo>;
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
					IQueryable<UserInfo> itemQuery = entity.UserInfo.Include("JobTitle").Where(u => u.DisplayInDirectory);
					if (!String.IsNullOrEmpty(filterList.FilterMicrositeID))
					{
						int micrositeID = Convert.ToInt32(filterList.FilterMicrositeID);
						itemQuery = itemQuery.Where(u => u.User.UserOffice.Any(o => o.Office.CMMicrositeID == micrositeID));
					}
					if (!String.IsNullOrEmpty(filterList.FilterLanguageID))
					{
						int languageID = Convert.ToInt32(filterList.FilterLanguageID);
						itemQuery = itemQuery.Where(u => u.User.UserLanguageSpoken.Any(l => l.LanguageID == languageID));
					}
					if (!String.IsNullOrEmpty(filterList.FilterUserInfoStaffTypeID))
					{
						int staffTypeID = Convert.ToInt32(filterList.FilterUserInfoStaffTypeID);
						itemQuery = itemQuery.Where(u => u.StaffTypeID == staffTypeID);
					}
					if (!String.IsNullOrEmpty(searchText))
						itemQuery = itemQuery.Where(u => u.FirstName.Contains(searchText.Trim()) || u.LastName.Contains(searchText.Trim()) || u.User.Email.Contains(searchText.Trim()) || (u.FirstName + " " + u.LastName).Contains(searchText.Trim()));
					if (!String.IsNullOrEmpty(letter))
						itemQuery = itemQuery.Where(u => u.LastName.StartsWith(letter));
					itemQuery = itemQuery.OrderBy(u => u.LastName).ThenBy(u => u.FirstName);
					objects = maximumRows <= 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : (maximumRows <= 0 || (pageNumber == 1 && objects.Count < maximumRows) ? objects.Count : itemQuery.Count());
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}

			return objects;
		}

		public static List<string> GetStaffAlphabet(int? cmMicrositeID)
		{
			List<string> objects;
			string key = cacheKeyPrefix + "GetStaffAlphabet_" + cmMicrositeID;

			List<string> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<string>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.UserInfo.Where(u => u.DisplayInDirectory && u.User.UserRole.Any(r => r.RoleID == (int)RolesEnum.Agent) && (!cmMicrositeID.HasValue || u.User.UserOffice.Any(o => o.Office.CMMicrositeID == cmMicrositeID))).Select(u => u.LastName.Substring(0, 1)).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public partial struct Filters
		{
			public string FilterLanguageID { get; set; }
			public string FilterMicrositeID { get; set; }

			public Dictionary<string, object> GetCustomFilterList()
			{
				filterList = GetFilterList();
				if (FilterLanguageID != null)
				{
					if (FilterLanguageID == string.Empty)
						filterList.Add("@FilterLanguageID", string.Empty);
					else
						filterList.Add("@FilterLanguageID", Convert.ToInt32(FilterLanguageID));
				}
				if (FilterMicrositeID != null)
				{
					if (FilterMicrositeID == string.Empty)
						filterList.Add("@FilterMicrositeID", string.Empty);
					else
						filterList.Add("@FilterMicrositeID", Convert.ToInt32(FilterMicrositeID));
				}
				return filterList;
			}
		}
	}
}