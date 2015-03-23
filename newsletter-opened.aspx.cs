using System;
using System.Web.UI;
using Classes.Newsletters;

public partial class NewsletterOpened : Page
{
	#region Members

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

	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
		if (EntityID != Guid.Empty && MailoutID > 0)
		{
			Subscriber subscriber = Subscriber.GetSubscriberByEntityID(EntityID);
			if (subscriber != null)
			    NewsletterAction.CreateOpenAction(subscriber, MailoutID);
		}

		//Response.Buffer = false; // This will cause a SecurityException when running under Cassini. Comment out for testing purposes, or run under IIS instead.
		Response.Clear();
		Response.ContentType = "image/gif";
		Response.TransmitFile(Server.MapPath("~/img/tracker.gif"));
	}
}