using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Media352_MembershipProvider;

public partial class Controls_Media352_MembershipProvider_UserLoginInformation : System.Web.UI.UserControl
{
	public int EntityId
	{
		get
		{
			if (ViewState["EntityId"] == null || ViewState["EntityId"].ToString() == "0")
			{
				int tempID;
				if (Request.QueryString["id"] != null)
					if (Int32.TryParse(Request.QueryString["id"], out tempID))
						return tempID;

				return 0;
			}
			return (int)ViewState["EntityId"];
		}
		set { ViewState["EntityId"] = value; }
	}

	public User UserEntity { get; set; }

	public bool UserNameReadOnly { get; set; }

	public string UserName { get { return uxName.Text; } }

	public string Email { get { return uxEmail.Text; } }

	public string Password { get { return uxPassword.Text; } }

	public string PasswordQuestion { get { return uxPasswordQuestion.Text; } }

	public string PasswordAnswer { get { return uxPasswordAnswer.Text; } }

	public string ValidationGroup { get; set; }

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxNameCV.ServerValidate += uxNameCV_ServerValidate;
		uxEmailCV.ServerValidate += uxEmailCV_ServerValidate;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			if (Settings.SecurityQuestionRequired)
			{
				uxPasswordQuestion.DataSource = SecurityQuestion.SecurityQuestionGetByActive(true);
				uxPasswordQuestion.DataTextField = "Question";
				uxPasswordQuestion.DataValueField = "Question";
				uxPasswordQuestion.DataBind();
			}
		}

		uxEmailRegexVal2.ValidationExpression = Helpers.EmailValidationExpression;
		uxSecurityQuestionPlaceHolder.Visible = Settings.SecurityQuestionRequired;
		uxUserNamePlaceHolder.Visible = !Settings.UserNameIsEmail;
		uxPasswordRegexVal.ValidationExpression =
		uxConfirmPasswordREV.ValidationExpression = Membership.PasswordStrengthRegularExpression;
		uxName.ReadOnly = UserNameReadOnly;

		if (!String.IsNullOrWhiteSpace(ValidationGroup))
			uxConfirmPasswordCV.ValidationGroup =
			uxConfirmPasswordReqFVal.ValidationGroup =
			uxConfirmPasswordREV.ValidationGroup =
			uxEmailRegexVal.ValidationGroup =
			uxEmailRegexVal2.ValidationGroup = 
			uxEmailReqFVal.ValidationGroup = 
			uxEmailCV.ValidationGroup =
			uxNameCV.ValidationGroup =
			uxNameRegexVal.ValidationGroup =
			uxNameReqFVal.ValidationGroup = 
			uxPasswordAnswerRegexVal.ValidationGroup =
			uxPasswordAnswerReqFVal.ValidationGroup = 
			uxPasswordQuestionReqFVal.ValidationGroup =
			uxPasswordRegexVal.ValidationGroup = 
			uxPasswordReqFVal.ValidationGroup = ValidationGroup;
	}

	public void LoadData()
	{
		uxName.Text = UserEntity.Name;
		uxEmail.Text = UserEntity.Email;
		if (Settings.SecurityQuestionRequired)
		{
			Media352_MembershipProvider membership = (Media352_MembershipProvider)Membership.Provider;
			if (!String.IsNullOrEmpty(UserEntity.PasswordAnswer))
				uxPasswordAnswer.Text = membership.GetUnencodedPassword(UserEntity.PasswordAnswer, (UserEntity.PasswordFormat == (int)MembershipPasswordFormat.Hashed ? (int)MembershipPasswordFormat.Encrypted : UserEntity.PasswordFormat), UserEntity.Salt);
			if (UserEntity.PasswordQuestion != null && uxPasswordQuestion.Items.FindByText(UserEntity.PasswordQuestion) == null)
				uxPasswordQuestion.Items.Add(new ListItem(UserEntity.PasswordQuestion, UserEntity.PasswordQuestion));
			if (UserEntity.PasswordQuestion != null)
				uxPasswordQuestion.Items.FindByText(UserEntity.PasswordQuestion).Selected = true;
		}
	}

	public void SaveData()
	{
		UserEntity.Email = uxEmail.Text;
		UserEntity.Name = uxName.Text;

		//If the password is visible, change it
		if (!String.IsNullOrWhiteSpace(uxPassword.Text))
		{
			string userNameToUse = Settings.UserNameIsEmail ? uxEmail.Text : uxName.Text;
			Media352_MembershipProvider membership = (Media352_MembershipProvider)Membership.Provider;
			MembershipCreateStatus statusUpdate;
			membership.UpdateUserNameAndPasswordRelatedInfo(EntityId, userNameToUse, uxPassword.Text, uxPasswordQuestion.SelectedValue, uxPasswordAnswer.Text, out statusUpdate);
			if (statusUpdate == MembershipCreateStatus.Success)
				ToggleChangePassword(true);
			else
				uxMemberInfoStatus.Text = @"Error updating login related information: " + statusUpdate;
		}
		UserEntity.Save();
	}

	public void SetupPasswordDisplayItems()
	{
		uxPasswordPH.Visible = EntityId < 1;
		uxChangePassword.Visible = !uxPasswordPH.Visible;
	}

	protected void uxChangePassword_Click(object sender, EventArgs e)
	{
		ToggleChangePassword(false);
	}

	private void ToggleChangePassword(bool hideIt)
	{
		if (hideIt)
		{
			uxChangePassword.Text = uxChangePassword.Text.Replace("Cancel ", "");
			uxPasswordPH.Visible = false;
		}
		else if (uxChangePassword.Text.Contains("Cancel"))
		{
			uxChangePassword.Text = uxChangePassword.Text.Replace("Cancel ", "");
			uxPasswordPH.Visible = false;
		}
		else
		{
			uxChangePassword.Text = uxChangePassword.Text.Replace("<span>", "<span>Cancel ");
			uxPasswordPH.Visible = true;
		}
	}

	private void uxNameCV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !Classes.Media352_MembershipProvider.User.UserGetByName(uxName.Text).Any(u => u.UserID != EntityId);
	}

	void uxEmailCV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !Classes.Media352_MembershipProvider.User.UserGetByEmail(uxEmail.Text).Any(u => u.UserID != EntityId);
	}
}