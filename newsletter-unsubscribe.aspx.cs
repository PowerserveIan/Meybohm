using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Newsletters;

public partial class NewsletterUnsubscribe : BasePage
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
		uxEmailValidator.ValidationExpression = Helpers.EmailValidationExpression;

		if (!IsPostBack)
		{
			List<MailingList> mailingLists = MailingList.GetByActiveDeleted(true, false);
			if (mailingLists.Count == 1)
			{
				uxMailingList.Items.Add(new ListItem(mailingLists[0].Name, mailingLists[0].MailingListID.ToString()));
				uxMailingList.SelectedValue = mailingLists[0].MailingListID.ToString();
				uxMailingListsPH.Visible = false;
			}
			else if (mailingLists.Count < 1)
			{
				uxSubscribePH.Visible = false;
				uxResponseMessagePH.Visible = true;
				uxResponseMessage.Text = "There are no mailing lists to unsubscribe from";
			}
			else
			{
				foreach (MailingList ml in mailingLists)
					uxMailingList.Items.Add(new ListItem(ml.Name, ml.MailingListID.ToString()));
			}

			// populate email address from subscriber if available
			if (EntityID != Guid.Empty)
			{
				Subscriber subscriber = Subscriber.GetSubscriberByEntityID(EntityID);
				if (subscriber != null)
				    uxEmail.Text = subscriber.Email;
			}
		}
	}

	protected void uxSubmit_Click(object o, EventArgs e)
	{
		if (Page.IsValid)
		{
			bool listSelected = false;
			foreach (ListItem checkedMailingList in uxMailingList.Items)
			{
				if (checkedMailingList.Selected)
				{
					listSelected = true;
					int mailingListID = Convert.ToInt32(checkedMailingList.Value);

					UnsubscribeUserReturnCode unSubscribeUser = NewsletterSystem.UnsubscribeUser(mailingListID, uxEmail.Text);
					if (unSubscribeUser == UnsubscribeUserReturnCode.Success)
					{
						uxResponseMessage.Text = "You have been successfully unsubscribed from the mailing list.";
						uxResponseMessage.ForeColor = Color.Green;
						uxSubscribePH.Visible = false;

						if (EntityID != Guid.Empty && MailoutID > 0) // store tracking information 
						{
							Subscriber subscriber = Subscriber.GetSubscriberByEntityID(EntityID);
							if (subscriber != null && subscriber.Email.Equals(uxEmail.Text, StringComparison.InvariantCultureIgnoreCase))
							    NewsletterAction.CreateUnsubscribeAction(subscriber, MailoutID);
						}
					}
					else if (unSubscribeUser == UnsubscribeUserReturnCode.Never_Subscribed)
					{
						uxResponseMessage.Text = "The email address you entered does not exist in the mailing list you selected.";
						uxResponseMessage.ForeColor = Color.Red;
					}
					else if (unSubscribeUser == UnsubscribeUserReturnCode.Already_Unsubscribed)
					{
						uxResponseMessage.Text = "You have already unsubscribed from the mailing list. If this is not your desired action, you may change your selection and resubmit this form.";
						uxResponseMessage.ForeColor = Color.Green;
					}
					else
					{
						uxResponseMessage.Text = "There was an error removing the email address to our mailing list. Please try again later.";
						uxResponseMessage.ForeColor = Color.Red;
					}

					uxResponseMessagePH.Visible = true;
				}
			}
			if (!listSelected)
				uxMailingListRequired.IsValid = false;
		}
	}

	protected void uxCancel_Click(object o, EventArgs e)
	{
		Response.Redirect("~/newsletter.aspx");
	}
}