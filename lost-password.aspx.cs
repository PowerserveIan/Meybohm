using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using BaseCode;
using Classes.Media352_MembershipProvider;

public partial class Lostpass : BasePage
{
	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		if (Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite() != null)
			MasterPageFile = "~/microsite.master";
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxUserInfoSubmit.Click += uxUserInfoSubmit_Click;
		uxQuestionSubmit.Click += uxQuestionSubmit_Click;
		uxEmailRegexVal.ValidationExpression = Helpers.EmailValidationExpression;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Members Area";
		ComponentAdminPage = "media352-membership-provider/admin-user.aspx";
	}

	void uxUserInfoSubmit_Click(object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			User userEntity = Classes.Media352_MembershipProvider.User.UserGetByEmail(uxEmail.Text).FirstOrDefault();
			if (userEntity != null)
			{
				if (userEntity.Email.ToLower().Contains("@meybohm.com"))
				{
					uxStaffEmailAddress.Text = Settings.LostPasswordEmailAddressForStaff;
					uxStaffEmailAddress.NavigateUrl = "mailto:" + Settings.LostPasswordEmailAddressForStaff + "?subject=" + Globals.Settings.SiteTitle + " - Forgotten Password";
					uxUserInfoPanel.Visible = false;
					uxStaffPH.Visible = true;
					return;
				}
				if (Settings.SecurityQuestionRequired)
				{
					uxUserInfoPanel.Visible = false;
					uxQuestionPanel.Visible = true;
					uxQuestion.Text = userEntity.PasswordQuestion;
				}
				else
					EmailUser(userEntity);
			}
			else
			{
				uxUserInfoFailureText.Text = "There are no accounts associated with the Email Address: " + uxEmail.Text + ".";
				uxUserInfoFailureText.Visible = true;
			}
		}
	}

	void uxQuestionSubmit_Click(object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			User user = Classes.Media352_MembershipProvider.User.UserGetByEmail(uxEmail.Text).FirstOrDefault();
			Media352_MembershipProvider provider = (Media352_MembershipProvider)Membership.Provider;
			if (provider.IsSecurityAnswerCorrect(user.Name, uxAnswer.Text))
				EmailUser(user);
			else
			{
				if (Settings.SecurityQuestionRequired)
				{
					uxQuestionFailureText.Text = "Incorrect security question answer";
					uxQuestionFailureText.Visible = true;
				}
				else
				{
					uxUserInfoFailureText.Text = "Incorrect security question answer";
					uxUserInfoFailureText.Visible = true;
				}
			}
		}
	}

	void EmailUser(User user)
	{
		user.ChangePasswordID = Guid.NewGuid();
		user.Save();
		MailMessage email = new MailMessage();
		email.From = new MailAddress(Settings.LostPasswordEmailFrom);
		email.To.Add(new MailAddress(uxEmail.Text));
		email.Subject = Settings.LostPasswordSubject;
		email.Body = EmailTemplateService.HtmlMessageBody(EmailTemplates.MembershipPasswordRecovery, new { Body = Settings.LostPasswordText, BeginRequired = "", EndRequired = "", UserName = user.Name, GUID = user.ChangePasswordID.ToString() });
		email.IsBodyHtml = true;
		SmtpClient client = new SmtpClient();
		client.Send(email);

		uxSuccessPH.Visible = true;
		uxQuestionPanel.Visible = false;
		uxUserInfoPanel.Visible = false;
		uxUserNameLabelSuccess.Text = uxEmail.Text;
	}
}