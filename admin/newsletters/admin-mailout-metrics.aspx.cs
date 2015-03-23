using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Newsletters;
using Action = Classes.Newsletters.Action;

public partial class Admin_Newsletters_MailoutMetrics : Page
{
	#region Members and Properties

	private int m_mailoutId;

	protected int MailoutID
	{
		get
		{
			if (m_mailoutId <= 0 && Request.QueryString["mailoutid"] != null)
				Int32.TryParse(Request.QueryString["mailoutid"], out m_mailoutId);
			return m_mailoutId;
		}
	}

	#endregion

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Helpers.GetCSSCode(uxCSSFiles);
	}

	protected void Export_Command(object sender, CommandEventArgs e)
	{
		int totalCount;
		switch (e.CommandArgument.ToString())
		{
			case "Sent":
				CSVWriteHelper.WriteCSVToResponse(
					NewsletterAction.NewsletterActionGetForMetricsPage(MailoutID, (int)Action.Send, 1, 0, out totalCount),
					true,
					Response,
					"NewsletterExport");
				break;
			case "NotSent":
				CSVWriteHelper.WriteCSVToResponse(
					NewsletterAction.GetSubscribersNotSentEmailPage(MailoutID, 1, 0, out totalCount),
					true,
					Response,
					"NewsletterExport");
				break;
			case "Opened":
				CSVWriteHelper.WriteCSVToResponse(
					NewsletterAction.NewsletterActionGetForMetricsPage(MailoutID, (int)Action.Open, 1, 0, out totalCount),
					true,
					Response,
					"NewsletterExport");
				break;
			case "ClicksUser":
				CSVWriteHelper.WriteCSVToResponse(
					NewsletterAction.NewsletterActionGetForMetricsPage(MailoutID, (int)Action.Click, 1, 0, out totalCount),
					true,
					Response,
					"NewsletterExport");
				break;
			case "ClicksURL":
				CSVWriteHelper.WriteCSVToResponse(
					NewsletterAction.GetUrlClickCountPage(MailoutID, 1, 0, out totalCount),
					true,
					Response,
					"NewsletterExport");
				break;
			case "Forward":
				CSVWriteHelper.WriteCSVToResponse(
					NewsletterAction.NewsletterActionGetForMetricsPage(MailoutID, (int)Action.Forward, 1, 0, out totalCount),
					true,
					Response,
					"NewsletterExport");
				break;
			case "Unsubscribe":
				CSVWriteHelper.WriteCSVToResponse(
					NewsletterAction.NewsletterActionGetForMetricsPage(MailoutID, (int)Action.Unsubscribe, 1, 0, out totalCount),
					true,
					Response,
					"NewsletterExport");
				break;
		}
	}

	[WebMethod]
	public static PagedActionContent GetRowPageForSentActions(int mailoutId, int startIndex, String loadType)
	{
		const string rowFormat = @"<tr><td class=""[ROWCLASS]"">[EMAIL]</td><td class=""[ROWCLASS]"">[TIMESTAMP]</td></tr>";
		const int pageSize = 50;
		return GetRowPageForActionType(mailoutId, Action.Send, loadType, startIndex, pageSize, rowFormat);
	}

	[WebMethod]
	public static PagedActionContent GetRowPageForNotSentAddresses(int mailoutId, int startIndex, String loadType)
	{
		const string rowFormat = @"<tr><td class=""[ROWCLASS]"">[EMAIL]</td></tr>";
		const int pageSize = 50;
		return GetRowPageForNotSentAddresses(mailoutId, loadType, startIndex, pageSize, rowFormat);
	}

	[WebMethod]
	public static PagedActionContent GetRowPageForOpenActions(int mailoutId, int startIndex, String loadType)
	{
		const string rowFormat = @"<tr><td class=""[ROWCLASS]"">[EMAIL]</td><td class=""[ROWCLASS]"">[TIMESTAMP]</td></tr>";
		const int pageSize = 50;
		return GetRowPageForActionType(mailoutId, Action.Open, loadType, startIndex, pageSize, rowFormat);
	}

	[WebMethod]
	public static PagedActionContent GetRowPageForForwardActions(int mailoutId, int startIndex, String loadType)
	{
		const string rowFormat = @"<tr><td class=""[ROWCLASS]"">[EMAIL]</td><td class=""[ROWCLASS]"">[DETAILS]</td><td class=""[ROWCLASS]"">[TIMESTAMP]</td></tr>";
		const int pageSize = 50;
		return GetRowPageForActionType(mailoutId, Action.Forward, loadType, startIndex, pageSize, rowFormat);
	}

	[WebMethod]
	public static PagedActionContent GetRowPageForUnsubscribeActions(int mailoutId, int startIndex, String loadType)
	{
		const string rowFormat = @"<tr><td class=""[ROWCLASS]"">[EMAIL]</td><td class=""[ROWCLASS]"">[TIMESTAMP]</td></tr>";
		const int pageSize = 50;
		return GetRowPageForActionType(mailoutId, Action.Unsubscribe, loadType, startIndex, pageSize, rowFormat);
	}

	[WebMethod]
	public static PagedActionContent GetRowPageForClickActions(int mailoutId, int startIndex, String loadType)
	{
		const string rowFormat = @"<tr><td class=""[ROWCLASS]"">[EMAIL]</td><td class=""[ROWCLASS]"">[DETAILS]</td><td class=""[ROWCLASS]"">[TIMESTAMP]</td></tr>";
		const int pageSize = 20;
		return GetRowPageForActionType(mailoutId, Action.Click, loadType, startIndex, pageSize, rowFormat);
	}

	[WebMethod]
	public static PagedActionContent GetRowPageForClicksByUrl(int mailoutId, int startIndex, String loadType)
	{
		const string rowFormat = @"<tr><td class=""[ROWCLASS]"">[URL]</td><td class=""[ROWCLASS]"">[CLICKCOUNT]</td></tr>";
		const int pageSize = 20;
		return GetRowPageForLinkClicks(mailoutId, loadType, startIndex, pageSize, rowFormat);
	}

	/// <summary>
	/// 	Get the markup for table rows that contain details on various newsletter actions of a particular type
	/// 	NOTE: All indexing in the metrics system is 1-based!
	/// </summary>
	/// <param name = "mailoutId">The mailout to retrieve data for</param>
	/// <param name = "actionType">The Action to retrieve data for</param>
	/// <param name = "loadType">Either "page" or "all" depending on whether one page or all remaining data is desired</param>
	/// <param name = "startIndex">The index of the first row to retrieve; must be (a multiple of pageSize) + 1</param>
	/// <param name = "pageSize">The maximum size of the data set to return upon each call</param>
	/// <param name = "rowFormat">The template string of a table row</param>
	public static PagedActionContent GetRowPageForActionType(int mailoutId, Action actionType, string loadType, int startIndex, int pageSize, String rowFormat)
	{
		int totalCount;
		int pageNumber = ((startIndex - 1) / pageSize) + 1;
		if (loadType.Equals("all"))
			pageSize = 0; // indicate we want all records from the current page onward

		List<NewsletterAction> actions = NewsletterAction.NewsletterActionGetForMetricsPage(mailoutId, (int)actionType, pageNumber, pageSize, out totalCount);

		// build the markup based on the provided format
		StringBuilder markup = new StringBuilder();
		int index = startIndex;
		foreach (NewsletterAction action in actions)
		{
			markup.Append(rowFormat
							.Replace("[ROWCLASS]", (index % 2 == 0) ? "listTableRow_green" : "listTableRow_white")
							.Replace("[EMAIL]", action.Email)
							.Replace("[IPADDRESS]", action.IPAddress)
							.Replace("[DETAILS]", action.Details)
							.Replace("[TIMESTAMP]", action.Timestamp.ToString("M/d/yyyy h:mm tt")));
			++index;
		}

		int lastIndex = startIndex + actions.Count - 1;
		return new PagedActionContent(totalCount, pageSize, lastIndex, markup.ToString());
	}

	/// <summary>
	/// 	Get the markup for table rows that contain details on which URLs have been clicked in newsletters, grouped by URL
	/// </summary>
	/// <param name = "mailoutId">The mailout to retrieve data for</param>
	/// <param name = "loadType">Either "page" or "all" depending on whether one page or all remaining data is desired</param>
	/// <param name = "startIndex">The index of the first row to retrieve; must be (a multiple of pageSize) + 1</param>
	/// <param name = "pageSize">The maximum size of the data set to return upon each call</param>
	/// <param name = "rowFormat">The template string of a table row</param>
	public static PagedActionContent GetRowPageForLinkClicks(int mailoutId, string loadType, int startIndex, int pageSize, String rowFormat)
	{
		int totalCount;
		int pageNumber = ((startIndex - 1) / pageSize) + 1;
		if (loadType.Equals("all"))
			pageSize = 0; // indicate we want all records from the current page onward

		List<UrlClickCountContainer> urlClickCounts = NewsletterAction.GetUrlClickCountPage(mailoutId, pageNumber, pageSize, out totalCount);

		// build the markup based on the provided format
		StringBuilder markup = new StringBuilder();
		int index = startIndex;
		foreach (UrlClickCountContainer urlClickCount in urlClickCounts)
		{
			markup.Append(rowFormat
							.Replace("[ROWCLASS]", (index % 2 == 0) ? "listTableRow_green" : "listTableRow_white")
							.Replace("[URL]", urlClickCount.Url)
							.Replace("[CLICKCOUNT]", urlClickCount.ClickCount.ToString()));
			++index;
		}

		int lastIndex = startIndex + urlClickCounts.Count - 1;
		return new PagedActionContent(totalCount, pageSize, lastIndex, markup.ToString());
	}

	/// <summary>
	/// 	Get the markup for table rows that contain details on which current subscribers have not been sent this mailout
	/// </summary>
	/// <param name = "mailoutId">The mailout to retrieve data for</param>
	/// <param name = "loadType">Either "page" or "all" depending on whether one page or all remaining data is desired</param>
	/// <param name = "startIndex">The index of the first row to retrieve; must be (a multiple of pageSize) + 1</param>
	/// <param name = "pageSize">The maximum size of the data set to return upon each call</param>
	/// <param name = "rowFormat">The template string of a table row</param>
	public static PagedActionContent GetRowPageForNotSentAddresses(int mailoutId, string loadType, int startIndex, int pageSize, String rowFormat)
	{
		int totalCount;
		int pageNumber = ((startIndex - 1) / pageSize) + 1;
		if (loadType.Equals("all"))
			pageSize = 0; // indicate we want all records from the current page onward

		List<String> addresses = NewsletterAction.GetSubscribersNotSentEmailPage(mailoutId, pageNumber, pageSize, out totalCount);

		// build the markup based on the provided format
		StringBuilder markup = new StringBuilder();
		int index = startIndex;
		foreach (String address in addresses)
		{
			markup.Append(rowFormat
							.Replace("[ROWCLASS]", (index % 2 == 0) ? "listTableRow_green" : "listTableRow_white")
							.Replace("[EMAIL]", address));
			++index;
		}

		int lastIndex = startIndex + addresses.Count - 1;
		return new PagedActionContent(totalCount, pageSize, lastIndex, markup.ToString());
	}

	#region Nested type: PagedActionContent

	/// <summary>
	/// 	This structure encapsulates the markup for a set of actions 
	/// 	and other information for updating the metrics display via Ajax.
	/// </summary>
	public struct PagedActionContent
	{
		public int LastIndex;
		public String Markup;
		public int PageSize;
		public int TotalCount;

		public PagedActionContent(int totalCount, int pageSize, int lastIndex, String markup)
		{
			TotalCount = totalCount;
			PageSize = pageSize;
			LastIndex = lastIndex;
			Markup = markup;
		}
	}

	#endregion
}