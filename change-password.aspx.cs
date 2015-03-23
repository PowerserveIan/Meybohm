using System;
using System.Linq;
using System.Web.Security;
using BaseCode;

public partial class changepass : BasePage
{
	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
		EnableWhiteSpaceCompression = false;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Members Area";
		ComponentAdminPage = "media352-membership-provider/admin-user.aspx";
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		NewPasswordRegexVal.ValidationExpression =
		ConfirmNewPasswordRegexVal.ValidationExpression = Membership.PasswordStrengthRegularExpression;
		ChangePasswordPushButton.Click += ChangePasswordPushButton_Click;
		CancelPushButton.Click += CancelPushButton_Click;
		Guid changePasswordID;
		if (!String.IsNullOrEmpty(Request.QueryString["id"]) && Guid.TryParse(Request.QueryString["id"], out changePasswordID))
		{
			Classes.Media352_MembershipProvider.User userEntity = Classes.Media352_MembershipProvider.User.UserGetByChangePasswordID(changePasswordID).FirstOrDefault();
			if (userEntity == null)
				Response.Redirect("~/");
			else if (userEntity.LastActivity < DateTime.UtcNow.AddDays(-1))
			{
				userEntity.ChangePasswordID = null;
				userEntity.Save();
				uxLinkExpired.Visible = true;
				uxStep1PH.Visible = false;
			}
			uxCurrentPasswordPH.Visible = false;
			Continue.NavigateUrl = "~/" + BaseCode.Helpers.GetLoginRedirectUrl(userEntity.Name);
		}
		else if (!User.Identity.IsAuthenticated)
			Response.Redirect("~/");
	}

	void ChangePasswordPushButton_Click(object sender, EventArgs e)
	{
		if (IsValid)
		{
			Classes.Media352_MembershipProvider.User userEntity = null;
			Guid changePasswordID;
			if (!String.IsNullOrEmpty(Request.QueryString["id"]) && Guid.TryParse(Request.QueryString["id"], out changePasswordID))
				userEntity = Classes.Media352_MembershipProvider.User.UserGetByChangePasswordID(changePasswordID).FirstOrDefault();
			else
				userEntity = Classes.Media352_MembershipProvider.User.GetByID(Helpers.GetCurrentUserID());
			Media352_MembershipProvider provider = (Media352_MembershipProvider)Membership.Provider;
			string oldPassword = CurrentPassword.Text;
			bool success;
			if (!String.IsNullOrEmpty(oldPassword))
				success = provider.ChangePassword(userEntity.Name, oldPassword, NewPassword.Text);
			else
				success = provider.ChangePassword(userEntity.Name, ref oldPassword, NewPassword.Text);
			if (success)
			{
				uxStep1PH.Visible = false;
				uxSuccessPH.Visible = true;
			}
			else
				FailureText.Text = "Your password is incorrect, please try again";
		}
	}

	void CancelPushButton_Click(object sender, EventArgs e)
	{
		if (User.Identity.IsAuthenticated)
			Response.Redirect("~/" + BaseCode.Helpers.GetLoginRedirectUrl(User.Identity.Name));
		Response.Redirect("~/");
	}
}