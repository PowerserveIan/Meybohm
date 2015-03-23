using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Classes.Media352_MembershipProvider;

public partial class Controls_BaseControls_GenericLoginControl : UserControl
{
	private string m_FormClassName = "formWhole";

	/// <summary>
	/// The url to redirect to.  Redirects to the page specified regardless of returnurl
	/// </summary>
	public string DestinationPageUrl
	{
		get { return uxLogin.DestinationPageUrl; }
		set { uxLogin.DestinationPageUrl = value; }
	}

	public string FormClassName
	{
		get { return m_FormClassName; }
		set { m_FormClassName = value; }
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		uxAccessDeniedLabel.Visible = !String.IsNullOrEmpty(Request.QueryString["ReturnUrl"]);
		((HtmlGenericControl)uxLogin.FindControl("uxUserNameDiv")).Attributes["class"] = FormClassName;
		((HtmlGenericControl)uxLogin.FindControl("uxPasswordDiv")).Attributes["class"] = FormClassName;
		uxLogin.Authenticate += uxLogin_Authenticate;
		uxLogin.LoggedIn += uxLogin_LoggedIn;

		uxValidationSummary.ValidationGroup =
		((RequiredFieldValidator)uxLogin.FindControl("UserNameRequired")).ValidationGroup =
		((RequiredFieldValidator)uxLogin.FindControl("PasswordRequired")).ValidationGroup =
		((Button)uxLogin.FindControl("LoginButton")).ValidationGroup = ClientID + "_LoginGroup";

		Classes.ContentManager.CMMicrosite currentMicrosite = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite();
		if (currentMicrosite != null)
		{
			((HyperLink)uxLogin.FindControl("CreateUserLink")).NavigateUrl = "~/" + currentMicrosite.Name.ToLower().Replace(" ", "-") + "/register";
			((HyperLink)uxLogin.FindControl("PasswordRecoveryLink")).NavigateUrl = "~/" + currentMicrosite.Name.ToLower().Replace(" ", "-") + "/lost-password";
			((Controls_BaseControls_ExternalLoginProviders)uxLogin.FindControl("uxExternalLoginProviders")).ReturnUrl = "~/" + currentMicrosite.Name.ToLower().Replace(" ", "-");
		}
	}

	private void uxLogin_Authenticate(object sender, AuthenticateEventArgs e)
	{
		try
		{
			string userName = uxLogin.UserName;
			if (userName.Contains("@") && !Classes.Media352_MembershipProvider.Settings.UserNameIsEmail)
			{
				Classes.Media352_MembershipProvider.User userEntity = Classes.Media352_MembershipProvider.User.UserGetByEmail(uxLogin.UserName).FirstOrDefault();
				if (userEntity != null)
					userName = userEntity.Name;
			}

			Media352_MembershipProvider provider = (Media352_MembershipProvider)Membership.Provider;
			e.Authenticated = provider.ValidateUser(userName, uxLogin.Password);
			if (e.Authenticated)
				uxLogin.UserName = userName;
			else
			{
				LdapAuthentication adAuth = new LdapAuthentication(Settings.ActiveDirectoryPath);
				e.Authenticated = adAuth.IsAuthenticated("meybohm", userName.Split('@')[0], uxLogin.Password);
				if (e.Authenticated)
				{
					uxLogin.UserName = userName;
					Media352_MembershipProvider membership = (Media352_MembershipProvider)Membership.Provider;
					if (!User.UserGetByName(userName).Any())
					{
						MembershipCreateStatus status;
						MembershipUser user = membership.CreateUser(userName, uxLogin.Password, (userName.Contains("@") ? userName : userName + "@meybohm.com"), "You created your account with Active Directory", "Reset this", Settings.UsersApprovedByDefault, 1, out status);
						if (status == MembershipCreateStatus.Success)
						{
							UserInfo userInfoEntity = new UserInfo();
							userInfoEntity.UserID = (int)user.ProviderUserKey;
							userInfoEntity.Save();

							string roles = adAuth.GetGroups();
							if (!String.IsNullOrEmpty(roles))
							{
								Media352_RoleProvider role = new Media352_RoleProvider();
								role.AddUsersToRoles(new[] { userName }, roles.Split('|'));
							}
						}
						else
						{
							e.Authenticated = false;
							uxLogin.FailureText = status.ToString();
						}
					}
					else
					{
						string temp = string.Empty;
						membership.ChangePassword(userName, ref temp, uxLogin.Password);
					}
				}
			}
		}
		catch (MemberAccessException ex)
		{
			uxLogin.FailureText = ex.Message;
		}
		if (!e.Authenticated)
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ToggleLogin_" + DateTime.Now.Ticks, "$('div.login').show();", true);
	}

	protected void uxLogin_LoggedIn(object sender, EventArgs e)
	{
		if (!String.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
			Response.Redirect(Request.QueryString["ReturnUrl"]);
		else if (!String.IsNullOrEmpty(DestinationPageUrl))
			Response.Redirect(DestinationPageUrl);
		else if (BaseCode.Helpers.CanAccessAdmin(uxLogin.UserName))
			Response.Redirect("~/admin");
		else
			Response.Redirect("~/" + BaseCode.Helpers.GetLoginRedirectUrl(uxLogin.UserName));
	}
}