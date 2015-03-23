using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.Newsletters;

public partial class Controls_Newsletters_AdminQuickView : UserControl
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxAdminQuickView.ComponentVersionNumber = Settings.VersionNumber;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			PlaceHolder uxContentArea = (PlaceHolder)uxAdminQuickView.FindControl("uxContentArea");
			Mailout lastMailout = Mailout.MailoutPage(0, 1, "", "TimeStamp", false).FirstOrDefault();
			if (lastMailout != null && lastMailout.NewsletterID.HasValue)
				lastMailout = Mailout.MailoutGetByNewsletterID(lastMailout.NewsletterID.Value).FirstOrDefault();
			else
				lastMailout = null;
			if (lastMailout != null)
			{
				Literal uxLastSentDate = (Literal)uxContentArea.Controls[0].FindControl("uxLastSentDate");
				HyperLink uxViewLink = (HyperLink)uxContentArea.Controls[0].FindControl("uxViewLink");
				HyperLink uxStatsLink = (HyperLink)uxContentArea.Controls[0].FindControl("uxStatsLink");
				Literal uxMailingLists = (Literal)uxContentArea.Controls[0].FindControl("uxMailingLists");
				Literal uxSendCount = (Literal)uxContentArea.Controls[0].FindControl("uxSendCount");
				Literal uxNotSentCount = (Literal)uxContentArea.Controls[0].FindControl("uxNotSentCount");
				Literal uxOpenCount = (Literal)uxContentArea.Controls[0].FindControl("uxOpenCount");
				Literal uxClickCount = (Literal)uxContentArea.Controls[0].FindControl("uxClickCount");
				Literal uxForwardCount = (Literal)uxContentArea.Controls[0].FindControl("uxForwardCount");
				Literal uxUnsubscribeCount = (Literal)uxContentArea.Controls[0].FindControl("uxUnsubscribeCount");

				uxLastSentDate.Text += lastMailout.TimestampClientTime.ToString("dddd M/d/yyyy h:mm tt");
				uxViewLink.NavigateUrl += lastMailout.MailoutID;
				uxStatsLink.NavigateUrl += lastMailout.MailoutID;
				uxMailingLists.Text = String.Join(", ", lastMailout.MailingListNames.ToArray());
				uxSendCount.Text = lastMailout.SendCount.ToString();
				uxNotSentCount.Text = lastMailout.NotSentCount.ToString();
				uxOpenCount.Text = lastMailout.OpenCount.ToString();
				uxClickCount.Text = lastMailout.ClickCount.ToString();
				uxForwardCount.Text = lastMailout.ForwardCount.ToString();
				uxUnsubscribeCount.Text = lastMailout.UnsubscribeCount.ToString();
			}
			else
			{
				PlaceHolder uxLastMailoutPH = (PlaceHolder)uxContentArea.Controls[0].FindControl("uxLastMailoutPH");
				Literal uxNoNewslettersSent = (Literal)uxContentArea.Controls[0].FindControl("uxNoNewslettersSent");
				uxLastMailoutPH.Visible = false;
				uxNoNewslettersSent.Visible = true;
			}
		}
	}
}