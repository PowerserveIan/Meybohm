using System;
using System.Net.Mail;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Media352_MembershipProvider;

public partial class Controls_BaseControls_Register : UserControl
{
	private bool m_ShowEditProfile = true;

	public bool ShowProfile { get; set; }

	public bool ShowEditProfile
	{
		get { return m_ShowEditProfile; }
		set { m_ShowEditProfile = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxCreateUser.Click += uxCreateUser_Click;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			string returnUrl = string.Empty;

			if (Request.QueryString["ReturnUrl"] != null)
				returnUrl = Request.QueryString["ReturnUrl"];

			uxUserProfile.Visible = ShowProfile;
			uxUserLoginInformation.SetupPasswordDisplayItems();
		}
	}

	void uxCreateUser_Click(object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			string userName = Settings.UserNameIsEmail ? uxUserLoginInformation.Email : uxUserLoginInformation.UserName;

			//Use the applications instantiated membership provider
			Media352_MembershipProvider membership = (Media352_MembershipProvider)Membership.Provider;
			MembershipCreateStatus status;
			MembershipUser user = membership.CreateUser(userName, uxUserLoginInformation.Password, uxUserLoginInformation.Email, uxUserLoginInformation.PasswordQuestion, uxUserLoginInformation.PasswordAnswer, Settings.UsersApprovedByDefault, 1, out status);
			if (status == MembershipCreateStatus.Success && user != null)
			{
				uxStep1PH.Visible = false;
				uxStepCompletePH.Visible = true;
				//Setup for profile information
				uxUserProfile.UserID = (int)user.ProviderUserKey;
				uxUserProfile.UseCurrentLoggedInUser = false;
				uxUserProfile.SaveProfile();

				//Only email the admin if users are not approved by default
				if (!Settings.UsersApprovedByDefault)
				{
					MailMessage message = new MailMessage(new MailAddress(Globals.Settings.FromEmail, Globals.Settings.CompanyName), new MailAddress(uxUserLoginInformation.Email));
					User userEntity = User.GetByID((int)user.ProviderUserKey);
					message.IsBodyHtml = true;

					message.Body = EmailTemplateService.HtmlMessageBody(EmailTemplates.NewUserActivation, new
					{
						GUID = userEntity.ChangePasswordID.ToString()
					});

					message.Subject = Globals.Settings.SiteTitle + " - Activate your account";
					SmtpClient smtpClient = new SmtpClient();
					smtpClient.Send(message);
				}
				else
				{
					Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
					FormsAuthentication.SetAuthCookie(userName, true);
				}
			}
			else
			{
				uxErrorMessage.Text = status.ToString();
				uxErrorMessage.Visible = true;
			}
		}
	}
}