using System;
using System.Collections.Generic;
using System.Linq;
using Classes.Newsletters;

public partial class NewsletterLink : BasePage
{
	#region Members

	private String m_DestinationUrl = String.Empty;
	private Guid m_EntityId = Guid.Empty;
	private int m_MailoutId;

	#endregion

	#region Properties

	protected Guid EntityID
	{
		get
		{
			Guid temp;
			if (!String.IsNullOrEmpty(Request.QueryString["EntityID"]) && Guid.TryParse(Request.QueryString["EntityID"], out temp))
				m_EntityId = temp;
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

	protected String DestinationUrl
	{
		get
		{
			if (m_DestinationUrl.Length == 0 && Request.QueryString["DestURL"] != null)
				m_DestinationUrl = Server.UrlDecode(Request.QueryString["DestURL"]);
			return m_DestinationUrl;
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
		if (EntityID != Guid.Empty && MailoutID > 0)
		{
			Subscriber subscriber = Subscriber.GetSubscriberByEntityID(EntityID);
			if (subscriber != null)
			{
				//Don't insert duplicate records, make sure the action hasn't already been submitted
				List<NewsletterAction> tempActionList = NewsletterAction.NewsletterActionGetByMailoutID(MailoutID).Where(a => a.SubscriberID.HasValue && a.SubscriberID == subscriber.SubscriberID).ToList();
				if (tempActionList.Where(a => a.NewsletterActionTypeID == (int)Classes.Newsletters.Action.Open).ToList().Count == 0)
					NewsletterAction.CreateOpenAction(subscriber, MailoutID);
				if (tempActionList.Where(a => a.Details == DestinationUrl).ToList().Count == 0)
					NewsletterAction.CreateClickAction(subscriber, MailoutID, DestinationUrl);
			}
		}

		if (DestinationUrl.Length > 0)
		{
			Response.Redirect(DestinationUrl);
		}
	}
}