using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using BaseCode;
using Classes.Media352_MembershipProvider;
using DotNetOpenAuth.AspNet;

public partial class external_login : Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!String.IsNullOrEmpty(Request.QueryString[AuthConfig.ProviderQueryString]))
		{
			string returnUrl = Request.QueryString["ReturnURL"];
			IAuthenticationClient client = AuthConfig.GetClientByProviderName(Request.QueryString[AuthConfig.ProviderQueryString]);
			AuthenticationResult result = client.VerifyAuthentication(new HttpContextWrapper(HttpContext.Current), new Uri(AuthConfig.ExternalLoginUrl + "?" + AuthConfig.ProviderQueryString + "=" + Request.QueryString[AuthConfig.ProviderQueryString] + (!String.IsNullOrWhiteSpace(returnUrl) ? "&ReturnURL=" + HttpUtility.UrlEncode(ResolveUrl(returnUrl)) : string.Empty)));
			string loginErrorMessage = client.ProviderName + " Login Error: ";
			if (result.IsSuccessful)
			{
				Classes.Media352_MembershipProvider.User userEntity = null;
				UserOpenAuthProvider uoapEntity = UserOpenAuthProvider.UserOpenAuthProviderPage(0, 1, "", "", true, new UserOpenAuthProvider.Filters { FilterUserOpenAuthProviderProviderName = result.Provider, FilterUserOpenAuthProviderProviderID = result.ProviderUserId }, new string[] { "User" }).FirstOrDefault();

				string email = result.ExtraData.ContainsKey("email") ? result.ExtraData["email"] : (Regex.IsMatch(result.UserName, Helpers.EmailValidationExpression) ? result.UserName : AuthConfig.NoEmailSuppliedAddress);
				string userName = email != AuthConfig.NoEmailSuppliedAddress ? email : (result.ExtraData.ContainsKey("name") ? result.ExtraData["name"] : (result.ExtraData.ContainsKey("full_name") ? result.ExtraData["full_name"] : result.UserName));
				bool newUser = false;

				if (uoapEntity != null)
					userEntity = uoapEntity.User;
				else if (!String.IsNullOrWhiteSpace(email) && email != AuthConfig.NoEmailSuppliedAddress) //Check if user exists under different provider for same email address
					userEntity = Classes.Media352_MembershipProvider.User.UserGetByEmail(email).FirstOrDefault();
				else if (!String.IsNullOrWhiteSpace(userName)) //Check if user exists under different provider for same user name
					userEntity = Classes.Media352_MembershipProvider.User.UserGetByName(userName).FirstOrDefault();
				if (userEntity == null)
				{
					Media352_MembershipProvider provider = (Media352_MembershipProvider)Membership.Provider;
					MembershipCreateStatus status;
					MembershipUser memUser = provider.CreateUser(userName, provider.GeneratePassword(), email, "--Not Set--", provider.GeneratePassword(), Settings.UsersApprovedByDefault, 1, out status);
					if (status == MembershipCreateStatus.Success)
						userEntity = Classes.Media352_MembershipProvider.User.GetByID((int)memUser.ProviderUserKey);
					else
					{
						Response.Write(status.ToString());
						return;
					}
					newUser = true;
				}

				UserInfo userInfoEntity = UserInfo.UserInfoGetByUserID(userEntity.UserID).FirstOrDefault() ?? new UserInfo { UserID = userEntity.UserID };
				if (result.ExtraData.ContainsKey("first_name") && !String.IsNullOrWhiteSpace(result.ExtraData["first_name"]) && String.IsNullOrWhiteSpace(userInfoEntity.FirstName))
					userInfoEntity.FirstName = result.ExtraData["first_name"];
				else if (result.ExtraData.ContainsKey("name") && !String.IsNullOrWhiteSpace(result.ExtraData["name"]) && String.IsNullOrWhiteSpace(userInfoEntity.FirstName))
					userInfoEntity.FirstName = result.ExtraData["name"].Split(' ')[0];
				else if (result.ExtraData.ContainsKey("fullName") && !String.IsNullOrWhiteSpace(result.ExtraData["fullName"]) && String.IsNullOrWhiteSpace(userInfoEntity.FirstName))
					userInfoEntity.FirstName = result.ExtraData["fullName"].Split(' ')[0];
				if (result.ExtraData.ContainsKey("last_name") && !String.IsNullOrWhiteSpace(result.ExtraData["last_name"]) && String.IsNullOrWhiteSpace(userInfoEntity.LastName))
					userInfoEntity.LastName = result.ExtraData["last_name"];
				else if (result.ExtraData.ContainsKey("name") && !String.IsNullOrWhiteSpace(result.ExtraData["name"]) && result.ExtraData["name"].Split(' ').Length > 1 && String.IsNullOrWhiteSpace(userInfoEntity.LastName))
					userInfoEntity.LastName = result.ExtraData["name"].Substring(result.ExtraData["name"].IndexOf(" ")).Trim();
				else if (result.ExtraData.ContainsKey("fullName") && !String.IsNullOrWhiteSpace(result.ExtraData["fullName"]) && result.ExtraData["fullName"].Split(' ').Length > 1 && String.IsNullOrWhiteSpace(userInfoEntity.LastName))
					userInfoEntity.LastName = result.ExtraData["fullName"].Substring(result.ExtraData["fullName"].IndexOf(" ")).Trim();

				userInfoEntity.Save();

				if (uoapEntity == null)
					new UserOpenAuthProvider { ProviderID = result.ProviderUserId, ProviderName = result.Provider, UserID = userEntity.UserID }.Save();

				FormsAuthentication.SetAuthCookie(userEntity.Name, true);
				
				if (!String.IsNullOrWhiteSpace(returnUrl))
					Response.Redirect(returnUrl);
				Response.Redirect("~/" + BaseCode.Helpers.GetLoginRedirectUrl(userEntity.Name));
			}
			else if (result.Error != null)
			{
				Helpers.LogException(result.Error);
				loginErrorMessage += result.Error.Message;
			}
			else if (!String.IsNullOrWhiteSpace(Request.QueryString["error_message"]))
				loginErrorMessage += Request.QueryString["error_message"];
			else if (!String.IsNullOrWhiteSpace(Request.QueryString["error_description"]))
				loginErrorMessage += Request.QueryString["error_description"];
			else
				loginErrorMessage += "Login failed for unknown reasons";
			Helpers.LogException(new Exception(loginErrorMessage));
			Response.Redirect("~/login?externalLoginError=" + Server.UrlEncode(loginErrorMessage));
		}
	}
}