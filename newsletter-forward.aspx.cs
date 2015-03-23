using System;
using BaseCode;
using Classes.Newsletters;

public partial class NewsletterForward : BasePage
{
	#region Members

	private Guid m_EntityId = Guid.Empty;
	private Mailout m_Mailout;
	private int m_MailoutId = -1;

	#endregion

	#region Properties

	protected Guid EntityID
	{
		get
		{
			if (m_EntityId == Guid.Empty && Request.QueryString["EntityID"] != null)
			{
				try
				{
					m_EntityId = new Guid(Request.QueryString["EntityID"]);
				}
				catch (FormatException)
				{
				}
			}
			return m_EntityId;
		}
	}

	protected int MailoutID
	{
		get
		{
			if (m_MailoutId <= 0 && Request.QueryString["MailoutID"] != null)
				Int32.TryParse(Request.QueryString["MailoutID"], out m_MailoutId);
			return m_MailoutId;
		}
	}

	#endregion

	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
		EnableWhiteSpaceCompression = false;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Newsletter";
		ComponentAdminPage = "newsletters/admin-newsletter.aspx";
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		m_Mailout = Mailout.GetByID(MailoutID);
		if (m_Mailout == null)
			Response.Redirect("~/newsletter.aspx");

		if (!IsPostBack)
		{
			uxHtmlPreview.Text = NewsletterSystem.GetNewsletterHtml(m_Mailout, false);

			if (EntityID != Guid.Empty)
			{
				Subscriber subscriber = Subscriber.GetSubscriberByEntityID(EntityID);
				if (subscriber != null)
				    uxFromEmail.Text = subscriber.Email;
			}
		}

		uxSendSuccess.Visible = false;
		uxFromEmailValidator.ValidationExpression = uxToEmailValidator.ValidationExpression = Helpers.EmailValidationExpression;
	}

	protected void uxSubmit_Click(object sender, EventArgs e)
	{
		SendEmail();
	}

	protected void SendEmail()
	{
		String toAddress = Server.HtmlEncode(uxToEmail.Text);
		String fromAddress = Server.HtmlEncode(uxFromEmail.Text);
		String toName = Server.HtmlEncode(uxToName.Text);
		String fromName = Server.HtmlEncode(uxFromName.Text);
		if (NewsletterSystem.ForwardNewsletter(m_Mailout.MailoutID, EntityID, toAddress, fromAddress, toName, fromName))
			uxSendSuccess.Visible = true;
		else
			uxSendFailure.IsValid = false;
	}
}