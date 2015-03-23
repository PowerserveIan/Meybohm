using System;
using System.Web.UI;
using Classes.Newsletters;
using BaseCode;

public partial class NewsletterDetail : BasePage
{
	#region Members

	private int m_MailoutId;
	private int m_NewsletterId;

	#endregion

	#region Properties

	protected int NewsletterID
	{
		get
		{
			if (m_NewsletterId <= 0 && Request.QueryString["Id"] != null)
				Int32.TryParse(Request.QueryString["Id"], out m_NewsletterId);
			return m_NewsletterId;
		}
	}

	protected int MailoutID
	{
		get
		{
			if (m_MailoutId <= 0 && Request.QueryString["mailoutId"] != null)
				Int32.TryParse(Request.QueryString["mailoutId"], out m_MailoutId);
			return m_MailoutId;
		}
	}

	protected bool ViewedFromAdmin
	{
		get
		{
			bool temp;
			if (!String.IsNullOrEmpty(Request.QueryString["adminView"]) && Boolean.TryParse(Request.QueryString["adminView"], out temp))
				return temp;
			return false;
		}
	}

	#endregion

	public override void SetComponentInformation()
	{
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		string redirectString = String.IsNullOrEmpty(Request.QueryString["Page"]) ? "" : "Page=" + Request.QueryString["Page"];

		if (String.IsNullOrEmpty(redirectString))
			redirectString = "~/newsletter.aspx";
		else
			redirectString = "~/newsletter.aspx?" + redirectString;

		Mailout mailout = null;
		if (NewsletterID > 0) // Default to using newsletter ID if coming from on-site link, or mailout ID if coming from embedded link
		{
			Newsletter newsletter = Newsletter.GetByID(NewsletterID);
			//If newsletter doesn't exist or the newsletter is inactive and not being viewed from the Admin area, then redirect
			if (newsletter == null || (!newsletter.Active && !ViewedFromAdmin))
				Response.Redirect(redirectString);
			else
				mailout = NewsletterSystem.MailoutFromNewsletter(newsletter, newsletter.DesignID);
			CanonicalLink = Helpers.RootPath + "newsletter-details.aspx?id=" + newsletter.NewsletterID + "&title=" + Server.UrlEncode(newsletter.Title);
		}
		else if (MailoutID > 0)
			mailout = Mailout.GetByID(MailoutID);

		if (mailout == null)
			Response.Redirect(redirectString);

		uxHtmlPreview.Text = NewsletterSystem.GetNewsletterHtml(mailout, false);

		uxBackButton.NavigateUrl = redirectString;
		uxBackButton.Visible = !ViewedFromAdmin;
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		Helpers.GetCSSCode(uxCSSFiles);
		if (String.IsNullOrEmpty(Title) || Title.Equals("Default Title", StringComparison.OrdinalIgnoreCase))
			Title = "Newsletter Detail";
	}
}