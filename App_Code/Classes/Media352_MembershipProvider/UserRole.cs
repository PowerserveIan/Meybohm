using System.Collections.Generic;
using System.Linq;

namespace Classes.Media352_MembershipProvider
{
	public partial class UserRole
	{
		public static List<UserRole> UserRoleGetWithUserByRoleName(string roleName)
		{
			List<UserRole> objects;
			string key = cacheKeyPrefix + "UserRoleGetWithUserByRoleName_" + roleName;

			List<UserRole> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<UserRole>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.UserRole.Include("User").Where(r=>r.Role.Name == roleName).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<UserRole> UserRoleGetBySystemRole(bool systemRole)
		{
			List<UserRole> objects;
			string key = cacheKeyPrefix + "UserRoleGetBySystemRole_" + systemRole;

			List<UserRole> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<UserRole>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.UserRole.Where(r => r.Role.SystemRole == systemRole).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}