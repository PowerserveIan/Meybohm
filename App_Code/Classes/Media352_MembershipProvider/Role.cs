using System.Collections.Generic;

namespace Classes.Media352_MembershipProvider
{
	public partial class Role
	{
		private string m_RoleUsersString;

		public string RoleUsersString
		{
			get { return m_RoleUsersString; }
			set { m_RoleUsersString = value; }
		}

		public static List<Role> RolePageWithUsersInRoleWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters())
		{
			List<Role> objects = RolePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList);
			foreach (Role obj in objects)
			{
				List<UserRole> roleUsers = Media352_MembershipProvider.UserRole.UserRoleGetWithUserByRoleName(obj.Name);
				obj.RoleUsersString = string.Empty;
				foreach (UserRole user in roleUsers)
				{
					obj.RoleUsersString += user.User.Name + ",";
				}
				obj.RoleUsersString = obj.RoleUsersString.TrimEnd(',').Replace(",", "<br />");
			}
			totalCount = m_ItemCount;
			return objects;
		}
	}
}
