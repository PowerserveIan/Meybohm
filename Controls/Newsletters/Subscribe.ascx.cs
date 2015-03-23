using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Newsletters;

public partial class Controls_Newsletters_Subscribe : UserControl
{
	private readonly List<string> listsSubscribedTo = new List<string>();
	private bool listSelected;
	public string MailingListName { get; set; }

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			uxEmailValidator.ValidationExpression = Helpers.EmailValidationExpression;
			uxEmailRequired.ValidationGroup =
			uxEmailValidator.ValidationGroup =
			uxSubmit.ValidationGroup = ClientID + "_NewsletterSubscribe";
		}
	}

	protected void uxSubmit_Click(object o, EventArgs e)
	{
		if (Page.IsValid)
		{
			if (String.IsNullOrEmpty(MailingListName) && Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite() != null)
				MailingListName = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite().Name;
			MailingList mailingListEntity = !String.IsNullOrEmpty(MailingListName) ? MailingList.MailingListGetByName(MailingListName).FirstOrDefault() : MailingList.GetByActiveDeleted(true, false).FirstOrDefault();
			if (mailingListEntity == null)
				return;//TODO

			//attempt to subscribe the user
			SubscribeUserReturnCode subscribeUser = NewsletterSystem.SubscribeUser(mailingListEntity.MailingListID, uxEmail.Text, (int)NewsletterSendingTypeFormat.HtmlOnly, 0);
			if (subscribeUser == SubscribeUserReturnCode.Success)
			{
				//if new, success is returned. if already existing, subscriber account is updated.
				uxResponseMessage.Text = "<h2>Thank You for Subscribing!</h2><p>You will begin receiving our emailed newsletters.";
				uxSubscribePH.Visible = false;
				uxResponseMessagePH.Visible = true;
			}
			else if (subscribeUser == SubscribeUserReturnCode.Already_Subscribed)
			{
				uxEmailAlreadySubscribed.Text = "The email address you entered is already in our mailing list.";
				uxEmailAlreadySubscribed.Visible = true;
				return;
			}
			else if (subscribeUser == SubscribeUserReturnCode.MailingList_Full)
			{
				uxEmailAlreadySubscribed.Text = Settings.MaxNumberSubscribersErrorMessage;
				uxEmailAlreadySubscribed.Visible = true;
				return;
			}
			else
			{
				uxEmailAlreadySubscribed.Text = "There was an error adding the email address to our mailing list. Please try again later.";
				uxEmailAlreadySubscribed.Visible = true;
				return;
			}
			SendWelcomeMessage(mailingListEntity.Name);
		}
	}

	private void SendWelcomeMessage(string mailingListName)
	{
		SubscriberInfo s = new SubscriberInfo();
		s.Email = uxEmail.Text;
		s.SubscriptionType = SubscriptionType.Html;

		Queue<SubscriberInfo> al = new Queue<SubscriberInfo>();
		al.Enqueue(s);
		string textBody = Settings.CustomWelcomeMessage;
		EmailSender es = new EmailSender("Welcome to " + mailingListName, al, Settings.SenderEmail, Settings.MailServer, textBody, textBody, null);
		es.Send();
	}
}