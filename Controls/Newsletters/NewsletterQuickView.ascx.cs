using System;
using System.Web.UI;

public partial class Controls_Newsletters_NewsletterQuickView : UserControl
{
	private int m_NumNewsletters = 5;
	private int m_SummaryLength = 50;
	private string m_MailingListName = string.Empty;

	/// <summary>
	/// The number of newsletters to display
	/// </summary>
	public int NumNewsletters
	{
		get { return m_NumNewsletters; }
		set
		{
			if (value >= 0)
				m_NumNewsletters = value;
		}
	}

	/// <summary>
	/// This uses BaseCode.Helpers.ForceShorten method to cut off any words after this number of characters and add an ellipsis to the end.  Will not cut off in middle of words, will instead not show the word if the character count intersects the word.
	/// </summary>
	public int SummaryLength
	{
		get { return m_SummaryLength; }
		set
		{
			if (value >= 0)
				m_SummaryLength = value;
		}
	}

	/// <summary>
	/// If a mailing list name is specified, a subscribe box will appear, and users filling out the subscribe form will be subscribed to the specified mailing list.
	/// </summary>
	public string MailingListName
	{
		get { return m_MailingListName; }
		set { m_MailingListName = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxNewsListDataSource.SelectParameters["numArticles"].DefaultValue = NumNewsletters.ToString();
		uxSubscribePH.Visible = !String.IsNullOrEmpty(MailingListName);
		uxSubscribeQV.MailingListName = MailingListName;
	}
}