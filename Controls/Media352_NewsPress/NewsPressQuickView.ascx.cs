using System;
using System.Web.UI;
using Classes.Media352_NewsPress;

public partial class Controls_Media352_NewsPress_NewsPressQuickView : UserControl
{
	private int m_NumArticles = 5;
	private int m_SummaryLength = 50;
	private int m_TitleLength = 255;

	public Categories Category { get; set; }

	/// <summary>
	/// The number of articles to display
	/// </summary>
	public int NumArticles
	{
		get { return m_NumArticles; }
		set
		{
			if (value >= 0)
				m_NumArticles = value;
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
	/// This uses BaseCode.Helpers.ForceShorten method to cut off any words after this number of characters and add an ellipsis to the end.  Will not cut off in middle of words, will instead not show the word if the character count intersects the word.
	/// </summary>
	public int TitleLength
	{
		get { return m_TitleLength; }
		set
		{
			if (value >= 0)
				m_TitleLength = value;
		}
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxNewsListDataSource.SelectParameters["numArticles"].DefaultValue = NumArticles.ToString();
		uxNewsListDataSource.SelectParameters["categoryID"].DefaultValue = ((int)Category).ToString();
	}
}