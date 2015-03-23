using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using Classes.Media352_MembershipProvider;

public class Media352_RoleProvider : RoleProvider
{
	private string m_AppName;

	public Application application
	{
		get
		{
			List<Application> app = Application.ApplicationGetByName(ApplicationName);
			if (app.Count == 1)
				return app[0];
			throw new Exception("Application not found");
		}
	}

	public override string ApplicationName
	{
		get { return m_AppName ?? "/"; }

		set
		{
			if (string.IsNullOrEmpty(value))
				throw new ProviderException("ApplicationName is null or empty");
			if (m_AppName.Length > 256)
				throw new ProviderException("Provider application name too long");
			m_AppName = value;
		}
	}

	public override void AddUsersToRoles(string[] usernames, string[] roleNames)
	{
		CheckArrayParameter(ref roleNames, true, true, true, 256, "roleNames");
		CheckArrayParameter(ref usernames, true, true, true, 256, "usernames");

		List<Role> roles = new List<Role>();

		foreach (string rolename in roleNames)
		{
			roles.AddRange(Role.RoleGetByName(rolename));
		}

		foreach (string username in usernames)
		{
			List<User> user = User.UserGetByName(username);
			if (user.Count == 1)
			{
				if (user[0].ApplicationID == application.ApplicationID)
				{
					List<UserRole> userRoles = UserRole.UserRoleGetByUserID(user[0].UserID);

					foreach (Role role in roles)
					{
						bool userInRole = false;

						foreach (UserRole userRole in userRoles)
						{
							if (role.RoleID == userRole.RoleID)
								userInRole = true;
						}

						if (!userInRole)
						{
							UserRole userRole = new UserRole();
							userRole.UserID = user[0].UserID;
							userRole.RoleID = role.RoleID;
							userRole.Save();
						}
					}
				}
			}
		}
	}

	public override void CreateRole(string roleName)
	{
		CheckParameter(ref roleName, true, true, true, 0x100, "Role Name");

		foreach (Role role in Role.RoleGetByName(roleName))
		{
			if (role.Name == roleName)
				throw new ProviderException("Role already exists");
		}
		Role role1 = new Role();

		role1.Name = roleName;
		role1.Save();
	}

	public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
	{
		CheckParameter(ref roleName, true, true, true, 0x100, "Role Name");

		if (throwOnPopulatedRole && UserRole.UserRoleGetWithUserByRoleName(roleName).Count != 0)
			throw new ProviderException("Role is not empty");

		foreach (Role role in Role.RoleGetByName(roleName))
		{
			if (role.Name == roleName)
			{
				role.Delete();
				role.Save();
				return true;
			}
		}
		return false;
	}

	public override string[] FindUsersInRole(string roleName, string usernameToMatch)
	{
		CheckParameter(ref roleName, true, true, true, 0x100, "Role name");
		CheckParameter(ref usernameToMatch, true, true, false, 0x100, "userNameToMatch");

		ArrayList list = new ArrayList();

		foreach (UserRole userRole in UserRole.UserRoleGetWithUserByRoleName(roleName))
		{
			User user = User.GetByID(userRole.UserID);
			if (user != null && user.Name.ToLower() == usernameToMatch.ToLower() && user.ApplicationID == application.ApplicationID)
				list.Add(user.Name.ToLower());
		}

		string[] users = new string[list.Count];

		list.CopyTo(users);

		return users;
	}

	/// <summary>
	/// Returns all System Roles in string format
	/// </summary>
	/// <returns></returns>
	public override string[] GetAllRoles()
	{
		List<Role> roles = Role.RoleGetBySystemRole(true);
		string[] strRoles = new string[roles.Count];
		int i = 0;
		foreach (Role role in roles)
		{
			strRoles[i] = role.Name;
			i++;
		}
		return strRoles;
	}

	public override string[] GetRolesForUser(string username)
	{
		CheckParameter(ref username, true, false, true, 0x100, "username");

		ArrayList userroles = new ArrayList();

		//find the user
		List<User> user = User.UserGetByName(username);
		if (HttpContext.Current.User.Identity.IsAuthenticated && username == HttpContext.Current.User.Identity.Name)
		{
			if (user.Count != 1)
				HttpContext.Current.Response.Redirect("~/logout.aspx" + (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ReturnURL"]) ? "?" + HttpContext.Current.Request.QueryString.ToString() : "?ReturnURL=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.RawUrl)));
			if (user[0].ApplicationID != application.ApplicationID)
				HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl + (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ReturnURL"]) ? "?" + HttpContext.Current.Request.QueryString.ToString() : "?ReturnURL=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.RawUrl)));
		}
		else if (user.Count != 1 || user[0].ApplicationID != application.ApplicationID)
			return new string[0];
		User thisUser = user[0];

		List<UserRole> userRoles = UserRole.UserRoleGetByUserID(thisUser.UserID);
		foreach (UserRole userRole in userRoles)
		{
			Role role = Role.GetByID(userRole.RoleID);
			if (role != null)
				userroles.Add(role.Name);
		}

		string[] strUserRoles = new string[userroles.Count];
		userroles.CopyTo(strUserRoles);

		return strUserRoles;
	}

	public override string[] GetUsersInRole(string roleName)
	{
		CheckParameter(ref roleName, true, true, true, 0x100, "role name");
		ArrayList users = new ArrayList();
		foreach (UserRole userRole in UserRole.UserRoleGetWithUserByRoleName(roleName))
		{
			User user = User.GetByID(userRole.UserID);
			if (user != null && application.ApplicationID == user.ApplicationID)
					users.Add(user.Name);
		}
		string[] strUsers = new string[users.Count];

		users.CopyTo(strUsers);

		return strUsers;
	}

	public override bool IsUserInRole(string username, string roleName)
	{
		bool isinrole = false;

		CheckParameter(ref roleName, true, true, true, 0x100, "role name");
		CheckParameter(ref username, true, false, true, 0x100, "user name");

		if (username.Length < 1)
			return false;

		List<User> users = User.UserGetByName(username);
		if (users.Count != 1)
			return false;
		if (users[0].ApplicationID != application.ApplicationID)
			return false;

		foreach (UserRole userRole in UserRole.UserRoleGetByUserID(users[0].UserID))
		{
			Role role = Role.GetByID(userRole.RoleID); //assumes constrained
			if (role.Name.ToLower() == roleName.ToLower())
				isinrole = true;
		}

		return isinrole;
	}

	public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
	{
		ArrayList roles = new ArrayList(roleNames);

		CheckArrayParameter(ref roleNames, true, true, true, 0x100, "roleNames");
		CheckArrayParameter(ref usernames, true, true, true, 0x100, "usernames");

		foreach (string username in usernames)
		{
			User user = User.UserGetByName(username).FirstOrDefault();
			if (user != null)
			{
				foreach (UserRole userrole in UserRole.UserRoleGetByUserID(user.UserID))
				{
					if (roles.Contains(Role.GetByID(userrole.RoleID).Name))
						userrole.Delete();
				}
			}
		}
	}

	public override bool RoleExists(string roleName)
	{
		CheckParameter(ref roleName, true, true, true, 0x100, "roleName");

		return Role.RoleGetByName(roleName).Count > 0;
	}

	internal static void CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
	{
		if (param == null)
		{
			if (checkForNull)
				throw new ArgumentNullException(paramName);
		}
		else
		{
			param = param.Trim();

			if (checkIfEmpty && String.IsNullOrEmpty(param))
				throw new ArgumentException(paramName + " cannot be empty");

			if (maxSize > 0 && param.Length > maxSize)
				throw new ArgumentException(paramName + " is too long");

			if (checkForCommas && param.Contains(","))
				throw new ArgumentException(paramName + " contains ','");
		}
	}

	internal static void CheckArrayParameter(ref string[] param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
	{
		if (param == null)
			throw new ArgumentNullException(paramName);
		if (param.Length < 1)
			throw new ArgumentException("Parameter_array_empty");
		Hashtable hashtable1 = new Hashtable(param.Length);
		for (int num1 = param.Length - 1; num1 >= 0; num1--)
		{
			CheckParameter(ref param[num1], checkForNull, checkIfEmpty, checkForCommas, maxSize, paramName + "[ " + num1.ToString(CultureInfo.InvariantCulture) + " ]");
			if (hashtable1.Contains(param[num1]))
				throw new ArgumentException("Parameter duplicate array element");
			hashtable1.Add(param[num1], param[num1]);
		}
	}
}