using System;
using Classes.Newsletters;

public partial class Admin_MailingListSubscriberEdit : BaseEditPage
{
	private Subscriber m_SubscriberEntity;
	private int m_MailinglistId = -1;

	public MailingListSubscriber MailingListSubscriberEntity { get; set; }

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-mailing-list.aspx";
		m_ClassName = "Mailing List Subscriber";
		base.OnInit(e);
		m_AddNewButton = null;
		m_SaveAndAddNewButton.Visible = false;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (EntityId > 0)
			{
				MailingListSubscriberEntity = MailingListSubscriber.GetByID(EntityId);

				if (MailingListSubscriberEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
			}
			else
				Response.Redirect(m_LinkToListingPage + ReturnQueryString);
		}
	}

	protected override void Save()
	{
		if (IsValid)
		{
			MailingListSubscriberEntity = EntityId > 0 ? MailingListSubscriber.GetByID(EntityId) : new MailingListSubscriber();
			MailingListSubscriberEntity.NewsletterFormatID = Convert.ToInt32(uxNewsletterFormat.SelectedValue);
			MailingListSubscriberEntity.Active = uxSubscribed.Checked;
			MailingListSubscriberEntity.Save();
			EntityId = MailingListSubscriberEntity.MailingListSubscriberID;
			m_ClassTitle = string.Empty;
		}
	}

	protected override void LoadData()
	{
		m_SubscriberEntity = Subscriber.GetByID(MailingListSubscriberEntity.SubscriberID);
		uxEmail.Text = m_SubscriberEntity.Email;
		uxNewsletterFormat.SelectedValue = MailingListSubscriberEntity.NewsletterFormatID.ToString();
		uxSubscribed.Checked = MailingListSubscriberEntity.Active;
	}

	protected override void CancelButton_Click(object sender, EventArgs e)
	{
		string redirectUrl = m_LinkToListingPage;
		if (Int32.TryParse(Request.QueryString["mid"], out m_MailinglistId) && m_MailinglistId > 0)
			redirectUrl = "admin-mailing-list-edit.aspx?id=" + m_MailinglistId;
		Response.Redirect(redirectUrl);
	}
}