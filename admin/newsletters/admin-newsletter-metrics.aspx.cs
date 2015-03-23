using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Classes.Newsletters;

public partial class Admin_Newsletters_NewsletterMetrics : BaseListingPage
{
	private ObjectDataSource m_DataSourceEntity = new ObjectDataSource();

	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		string masterPageScript = ((Literal)Master.FindControl("uxJavaScripts")).Text;
		((Literal)Master.FindControl("uxJavaScripts")).Text = masterPageScript.Replace(",~/tft-js/core/knockout.js,~/tft-js/core/jquery.dateformat.js,~/tft-js/core/admin-listing.js", "");
	}

	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "CreatedDate";
		m_LinkToEditPage = string.Empty;
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Newsletter Statistics";
		m_CustomBreadCrumbsPH = uxCustomBreadCrumbsPH;		
		base.OnInit(e);
		m_FilterBlueToggleAreaTop.Visible = m_FilterBlueToggleAreaBottom.Visible = m_PagerBottom.Visible = m_SearchPanel.Visible = false;
		uxNewsletterRpt.DataSource = m_DataSourceEntity;
		m_AddButton.Visible = false;
		uxNewsletterRpt.ItemDataBound += uxNewsletterRpt_ItemDataBound;
		m_DataSourceEntity.Selecting += DataSourceEntity_Selecting;
		uxNewsletterRpt.Sorting += uxNewsletterRpt_Sorting;
	}

	private void DataSourceEntity_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
	{
		Newsletter.Filters filterList = new Newsletter.Filters();
		filterList.FilterNewsletterDeleted = false.ToString();
		e.Arguments.MaximumRows = 0;
		e.InputParameters["sortField"] = m_DefaultSortField;
		e.InputParameters["sortDirection"] = false.ToString();
		e.InputParameters["filterList"] = filterList;
	}

	private void uxNewsletterRpt_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		ListView mailoutRepeater = (ListView)e.Item.FindControl("uxMailoutLV");
		HiddenField uxNewsletterID = (HiddenField)e.Item.FindControl("uxNewsletterID");
		if (mailoutRepeater != null && uxNewsletterID != null)
		{
			mailoutRepeater.DataSource = Mailout.MailoutGetByNewsletterID(Convert.ToInt32(uxNewsletterID.Value));
			mailoutRepeater.DataBind();
		}
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		m_DataSourceEntity.TypeName = "Classes.Newsletters.Newsletter";
		m_DataSourceEntity.SelectMethod = "NewsletterPage";	
		m_DataSourceEntity.SelectCountMethod = "SelectCount";
		m_DataSourceEntity.EnablePaging = true;
		Parameter searchTextParameter = new Parameter("searchText", DbType.String, "");
		searchTextParameter.ConvertEmptyStringToNull = false;
		m_DataSourceEntity.SelectParameters.Add(searchTextParameter);
		base.Page_Load(sender, e);
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxNewsletterRpt.DataBind();		
	}

	private void uxNewsletterRpt_Sorting(object sender, ListViewSortEventArgs e)
	{
		//do nothing, handled by base class
		//Listview will throw error without this
	}

	#region Resend Methods

	protected void Resend_Command(object sender, CommandEventArgs e)
	{
		if (e.CommandName.Equals("Resend", StringComparison.OrdinalIgnoreCase))
		{
			int mailoutId = 0;
			if (Int32.TryParse(e.CommandArgument.ToString(), out mailoutId))
				ResendMailout(mailoutId);
		}
	}

	protected void ResendMailout(int mailoutId)
	{
		List<MailingListSubscriber> notSentSubscribers;

		Mailout mailout = Mailout.GetByID(mailoutId);
		if (mailout != null)
		{
			List<MailingList> mailingLists = new List<MailingList>();
			foreach (MailoutMailingList moml in MailoutMailingList.MailoutMailingListGetByMailoutID(mailoutId))
				mailingLists.Add(MailingList.GetByID(moml.MailingListID));
			if (mailingLists.Count == 0)
				throw new Exception("No mailing lists selected to send to");

			uxProgressWindow.Visible = true;
			if (NewsletterSystem.SendNewsletter(mailout.NewsletterID, mailoutId, mailout.DesignID.Value, mailingLists, out notSentSubscribers))
			{
				ClientScript.RegisterStartupScript(Page.GetType(), "showProgress", "$('.floatingBox').show();", true);
				uxResendSuccessMessage.Visible = true;
			}

			if (notSentSubscribers.Count > 0)
			{
				uxBadEmailsRepeater.Visible = true;
				uxBadEmailsRepeater.DataSource = notSentSubscribers;
				uxBadEmailsRepeater.DataBind();
			}
		}
	}

	#endregion
}