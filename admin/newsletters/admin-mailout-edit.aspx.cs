using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Newsletters;

public partial class Admin_MailoutEdit : Page
{
	private readonly NewsletterSendingTypeFormat m_NewsletterSendingType = Settings.NewsletterSendingType;
	private int m_NewsletterId = -1;

	/// <summary>
	/// Newsletter object used to save and load data
	/// </summary>
	public Newsletter NewsletterEntity { get; set; }

	/// <summary>
	/// 	Viewstate variable used to keep track of what tab you are on
	/// </summary>
	protected int TabNumber
	{
		get
		{
			if (ViewState["TabNumber"] == null) return 1;
			return Convert.ToInt32(ViewState["TabNumber"]);
		}
		set
		{
			if (Convert.ToInt32(value) > 0)
				ViewState["TabNumber"] = value;
			else
				ViewState["TabNumber"] = 1;
		}
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Tab1LinkButton.Command += TabButtons_Command;
		Tab2LinkButton.Command += TabButtons_Command;
		Tab3LinkButton.Command += TabButtons_Command;
		uxBack.Click += Back_Click;
		uxNext.Click += Next_Click;
		uxSendNewsletter.Click += uxSendNewsletter_Click;
		uxNewsletterDesign.SelectedIndexChanged += uxNewsletterDesign_SelectedIndexChanged;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		if (!Int32.TryParse(Request.QueryString["NewsletterId"], out m_NewsletterId))
			SendBackToMailoutListing();
		if (!IsPostBack)
		{
			LoadData();
			SetTab();
		}
	}

	private void LoadData()
	{
		if (m_NewsletterId > 0)
		{
			//load Newsletter from database
			NewsletterEntity = Newsletter.GetByID(m_NewsletterId);
			if (NewsletterEntity == null) //could not find newsletter in the database
				SendBackToMailoutListing();
		}
		else
			SendBackToMailoutListing();

		//Tab 1
		//Instructions
		NewsletterEditLink.NavigateUrl = "admin-newsletter-edit.aspx?id=" + m_NewsletterId;
		//HTML content
		BindNewsletterDesignDropDown();
		LoadHtmlPreview();
		//Text content
		LoadTextPreview();

		//Tab 2
		//TODO: load up approval email list from database

		//Tab 3
		BindMailingListCheckboxes();
	}

	private void BindMailingListCheckboxes()
	{
		List<MailingList> mailingLists = MailingList.GetByActiveDeleted(true, false);

		uxMailingLists.DataSource = mailingLists;
		uxMailingLists.DataTextField = "Name";
		uxMailingLists.DataValueField = "MailingListID";
		uxMailingLists.DataBind();

		if (uxMailingLists.Items.Count > 0)
			noMailingListsMessage.Visible = false;

		foreach (ListItem mailingList in uxMailingLists.Items)
		{
			mailingList.Text = String.Format(mailingListTextDesign.Text, mailingList.Value, mailingList.Text);
		}

		if (Settings.EnableMailingListLimitations && Settings.MaxEmailsPerMonth > 0)
		{
			int usedCount = NewsletterAction.GetNumberOfUsersEmailedInLastMonth();
			int numberLeft = Settings.MaxEmailsPerMonth - usedCount;
			foreach (ListItem mailingList in uxMailingLists.Items)
			{
				int countInMailingList = MailingListSubscriber.GetCountByMailingListIDActive(Convert.ToInt32(mailingList.Value), true);
				if (countInMailingList > numberLeft)
				{
					mailingList.Enabled = false;
					mailingList.Text += @"<span class=""tooltip""><span>This list has been disabled because sending to all of the users in it would put you over your maximum number of emails per month.</span></span>";
				}
			}
		}
	}

	private void uxSendNewsletter_Click(object sender, EventArgs e)
	{
		SentSuccessMessage.Visible = false;

		if (Page.IsValid)
		{
			List<MailingList> selectedMailingLists = new List<MailingList>();
			foreach (ListItem li in uxMailingLists.Items)
			{
				if (li.Selected)
					selectedMailingLists.Add(MailingList.GetByID(Convert.ToInt32(li.Value)));
			}
			if (selectedMailingLists.Count == 0)
				throw new Exception("No mailing lists selected to send to");

			int designId = Convert.ToInt32(uxNewsletterDesign.SelectedValue);
			List<MailingListSubscriber> notSentSubscribers;

			uxProgressWindow.Visible = true;
			uxSendNewsletter.Visible = false;
			if (NewsletterSystem.SendNewsletter(m_NewsletterId, null, designId, selectedMailingLists, out notSentSubscribers))
			{
				LastSaveDate.Text = Helpers.ConvertUTCToClientTime(DateTime.UtcNow).ToString();
				SentSuccessMessage.Visible = true;
				ClientScript.RegisterStartupScript(Page.GetType(), "showProgress", "$('.floatingBox').show();", true);
			}
			else if (notSentSubscribers.Count == 0)
			{
				uxNoActionTaken.Visible = true;
				ClientScript.RegisterStartupScript(Page.GetType(), "showProgress", "$('.floatingBox').hide();", true);
			}
			else
				ClientScript.RegisterStartupScript(Page.GetType(), "showProgress", "$('.floatingBox').hide();", true);

			if (notSentSubscribers.Count > 0)
			{
				uxBadEmailsRepeater.Visible = true;
				uxBadEmailsRepeater.DataSource = notSentSubscribers;
				uxBadEmailsRepeater.DataBind();
			}
		}
	}

	private void SendBackToMailoutListing()
	{
		Response.Redirect("~/admin/newsletters/admin-newsletter.aspx");
	}

	protected void SendApprovalEmails(Object sender, EventArgs e)
	{
		if (IsValid)
		{
			//send approval email
			approvalEmailSentPH.Visible = false;
			string approvalCode = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
			int newsletterId = m_NewsletterId;
			int newsletterDesignId = Convert.ToInt32(uxNewsletterDesign.SelectedValue);
			string toemail = uxApprovalEmail.Text;
			string fromemail = Settings.SenderEmail;
			string mailserver = Settings.MailServer;

			bool sendHtmlCopy = false;
			bool sendTextCopy = false;
			bool sendMultipartCopy = false;

			switch (m_NewsletterSendingType)
			{
				case NewsletterSendingTypeFormat.HtmlAndText:
					sendHtmlCopy = true;
					sendTextCopy = true;
					break;
				case NewsletterSendingTypeFormat.HtmlOnly:
					sendHtmlCopy = true;
					break;
				case NewsletterSendingTypeFormat.TextOnly:
					sendTextCopy = true;
					break;
				case NewsletterSendingTypeFormat.Multipart:
					sendMultipartCopy = true;
					break;
				default:
					throw new Exception("Unexpected newsletterSendingType encountered: " + m_NewsletterSendingType);
			}

			if (sendHtmlCopy)
				NewsletterSystem.SendNewsletterTest(newsletterId, newsletterDesignId, 1, toemail, mailserver, fromemail, approvalCode);
			if (sendTextCopy)
				NewsletterSystem.SendNewsletterTest(newsletterId, newsletterDesignId, 2, toemail, mailserver, fromemail, approvalCode);
			if (sendMultipartCopy)
				NewsletterSystem.SendNewsletterTest(newsletterId, newsletterDesignId, 3, toemail, mailserver, fromemail, approvalCode);

			approvalEmailListLiteral.Text = toemail;
			approvalEmailSentPH.Visible = true;
		}
	}

	private void BindNewsletterDesignDropDown()
	{
		uxNewsletterDesign.DataSource = NewsletterDesign.GetAll();
		uxNewsletterDesign.DataTextField = "Name";
		uxNewsletterDesign.DataValueField = "NewsletterDesignID";
		uxNewsletterDesign.DataBind();

		uxNewsletterDesign.Items.Insert(0, new ListItem("-- None --", ""));
		uxNewsletterDesign.SelectedValue = NewsletterEntity.DesignID.ToString();
	}

	private void uxNewsletterDesign_SelectedIndexChanged(object sender, EventArgs e)
	{
		NewsletterEntity = Newsletter.GetByID(m_NewsletterId);
		LoadHtmlPreview();
	}

	private void LoadHtmlPreview()
	{
		int newsletterDesignId;
		if (Int32.TryParse(uxNewsletterDesign.SelectedValue, out newsletterDesignId) && newsletterDesignId > 0)
		{
			Mailout mailout = NewsletterSystem.MailoutFromNewsletter(NewsletterEntity, newsletterDesignId);
			uxHtmlPreview.Text = @"<iframe src=""" + Helpers.RootPath + @"newsletter-details.aspx?Id=" + NewsletterEntity.NewsletterID + @"&mailoutId=" + mailout.MailoutID + @"&adminView=true"" width=""550"" height=""768"" style=""border: none;""></iframe>";
			NoDesignSelectedText.Visible = false;
		}
		else
		{
			uxHtmlPreview.Text = string.Empty;
			NoDesignSelectedText.Visible = true;
		}
	}

	private void LoadTextPreview()
	{
		Mailout mailout = NewsletterSystem.MailoutFromNewsletter(NewsletterEntity, null);
		uxTextPreview.Text = NewsletterSystem.GetNewsletterText(mailout);
	}

	private void TabButtons_Command(object sender, CommandEventArgs e)
	{
		switch (e.CommandName)
		{
			case "SetTab":
				TabNumber = Convert.ToInt32(e.CommandArgument.ToString());
				SetTab();
				break;
			default:
				throw new Exception("TabButtons_Command(): CommandName '" + e.CommandName + "', CommandArgument='" + e.CommandArgument + "' not defined");
		}
	}

	public void SetTab()
	{
		Tab1Instructions.Visible = Tab2Instructions.Visible = Tab3Instructions.Visible = false;
		TextPreviewPH.Visible = !(m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.HtmlOnly));
		Tab1LinkButton.CssClass = Tab2LinkButton.CssClass = Tab3LinkButton.CssClass = string.Empty;
		Tab1.Visible = Tab2.Visible = Tab3.Visible = false;
		uxBack.Visible = uxNext.Visible = true;
		uxSendNewsletter.Visible = SentSuccessMessage.Visible = false;

		switch (TabNumber)
		{
			case 1:
				Tab1Instructions.Visible = true;
				Tab1LinkButton.CssClass = "selected";
				Tab1.Visible = true;
				uxBack.Visible = false; //hide "Back" button
				break;
			case 2:
				Tab2Instructions.Visible = true;
				Tab2LinkButton.CssClass = "selected";
				Tab2.Visible = true;
				Tab1LinkButton.CausesValidation = false; // going backwards should not check validation
				break;
			case 3:
				Tab3Instructions.Visible = true;
				Tab3LinkButton.CssClass = "selected";
				Tab3.Visible = true;
				Tab1LinkButton.CausesValidation = false;
				Tab2LinkButton.CausesValidation = false;
				uxNext.Visible = false; //hide "Next" button
				uxSendNewsletter.Visible = true;
				break;
			default:
				throw new Exception("Tab number " + TabNumber + " out of range.");
		}
	}

	private void Back_Click(object sender, EventArgs e)
	{
		TabNumber--;
		SetTab();
	}

	private void Next_Click(object sender, EventArgs e)
	{
		TabNumber++;
		SetTab();
	}

	protected void uxMailingLists_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = uxMailingLists.SelectedItem != null;
	}

	protected void uxEmailRegexVal_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = true;
		Regex emailExpression = new Regex(Helpers.EmailValidationExpression);
		string[] emailArray = uxApprovalEmail.Text.Split(',');
		foreach (string email in emailArray)
		{
			args.IsValid = args.IsValid && emailExpression.IsMatch(email.Trim());
		}
	}
}