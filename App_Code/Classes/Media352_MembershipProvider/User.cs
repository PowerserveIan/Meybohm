using System.Collections.Generic;
using System.Linq;

namespace Classes.Media352_MembershipProvider
{
	public partial class User
	{
		public string FirstName { get { return this.UserInfo.FirstOrDefault() != null ? this.UserInfo.FirstOrDefault().FirstName : null; } }
		public string LastName { get { return this.UserInfo.FirstOrDefault() != null ? this.UserInfo.FirstOrDefault().LastName : null; } }

		public static List<User> GetAllAdminsAndEditors()
		{
			List<User> objects;
			const string key = cacheKeyPrefix + "GetAllAdminsAndEditors";
			List<User> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<User>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.User.Where(u => u.UserRole.Any(r => r.Role.Name == "Admin" || r.Role.Name == "Blog Admin" || r.Role.Name == "Blog Editor")).ToList();
				}

				Cache.Store(key, objects);
			}
			return objects;
		}

		public static List<User> UserPageByAdminListWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<User> objects = UserPageByAdminList(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<User> UserPageByAdminList(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "UserID");

			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText);
			string cachingInclude = includeList != null && includeList.Any() ? "_Include_" + string.Join(",", includeList) : string.Empty;

			List<User> objects;
			string baseKey = cacheKeyPrefix + "UserPageByAdminList_" + cachingFilterText + cachingInclude;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<User> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<User>;
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
					IQueryable<User> itemQuery = SetupOrderByClause(entity.User.Include("UserInfo").AsQueryable(), sortField, sortDirection);
					if (!string.IsNullOrWhiteSpace(searchText))
						itemQuery = itemQuery.Where(u => u.Name.Contains(searchText) || u.Email.Contains(searchText) || u.UserInfo.FirstOrDefault().FirstName.Contains(searchText) || u.UserInfo.FirstOrDefault().LastName.Contains(searchText));
					if (filterList.FilterUserHasRole.HasValue)
					{
						if (filterList.FilterUserHasRole.Value && string.IsNullOrEmpty(filterList.FilterUserRoleName))
							itemQuery = itemQuery.Where(u => u.UserRole.Any(r => r.Role.Name != "Agent"));
						else if (filterList.FilterUserHasRole.Value)
							itemQuery = itemQuery.Where(u => u.UserRole.Any(r => r.Role.Name == filterList.FilterUserRoleName));
						else
							itemQuery = itemQuery.Where(u => !u.UserRole.Any());
					}

					objects = maximumRows == 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : itemQuery.Count();
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		public static List<Membership_CustomerListForCSV> GetCustomerListForCSV()
		{
			List<Membership_CustomerListForCSV> objects;
			using (Entities entity = new Entities())
			{
				objects = entity.Membership_GetCustomerListForCSV().ToList();
			}
			return objects;
		}

		public partial struct Filters
		{
			public bool? FilterUserHasRole { get; set; }
			public string FilterUserRoleName { get; set; }

			public Dictionary<string, object> GetCustomFilterList()
			{
				filterList = GetFilterList();
				if (FilterUserHasRole.HasValue)
					filterList.Add("@FilterUserHasRole", FilterUserHasRole);
				if (FilterUserRoleName != null)
					filterList.Add("@FilterUserRoleName", FilterUserRoleName);
				return filterList;
			}
		}
	}
}