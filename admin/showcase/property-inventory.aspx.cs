using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using BaseCode;
using Classes.Showcase;

public partial class AdminShowcaseItem : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "NumberOfVisits";
		m_LinkToEditPage = "";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Property Inventory and Statistics";
		m_ShowFiltersByDefault = true;
		m_ColumnNumberToMakeLink = 0;
		m_DefaultSortDirection = false;
		base.OnInit(e);
		m_HeaderTitle.Text = m_BreadCrumbTitle.Text = m_BreadCrumbTitle.Text.Replace(" Manager", "");
		m_AddButton.Visible = false;
		m_SearchPanel.Controls.Add(uxDownloadReport);
		uxDownloadReport.Click += uxDownloadReport_Click;
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		base.Page_Load(sender, e);
		if (!IsPostBack)
		{
			uxFilterByAgentID.DataSource = Classes.Media352_MembershipProvider.UserInfo.GetAllAgents();
			uxFilterByAgentID.DataTextField = "FirstAndLast";
			uxFilterByAgentID.DataValueField = "UserID";
			uxFilterByAgentID.DataBind();

			uxFilterByShowcaseID.DataSource = Showcases.GetAll().Where(s => !s.Title.ToLower().Contains("existing")).OrderBy(s => s.Title);
			uxFilterByShowcaseID.DataTextField = "Title";
			uxFilterByShowcaseID.DataValueField = "ShowcaseID";
			uxFilterByShowcaseID.DataBind();

			uxFilterByOfficeID.DataSource = Classes.MLS.Office.OfficeGetByIsMeybohm(true, "Name");
			uxFilterByOfficeID.DataTextField = "Name";
			uxFilterByOfficeID.DataValueField = "OfficeID";
			uxFilterByOfficeID.DataBind();

			uxFilterByPropertyType.DataSource = ShowcaseAttributeValue.GetAllPropertyTypes();
			uxFilterByPropertyType.DataBind();

			uxBeginDate.TextBoxControl.Attributes.Add("data-bind", "value:pageFilter.FilterBeginDate,css:{error:!listingModel.startDateValid()}");
			uxEndDate.TextBoxControl.Attributes.Add("data-bind", "value:pageFilter.FilterEndDate,css:{error:!listingModel.endDateValid()}");

			uxBeginDate.MaxDate = uxEndDate.MaxDate = DateTime.Now;

			//uxBeginDate.SelectedDate = DateTime.Now.AddMonths(-1);
			//uxEndDate.SelectedDate = DateTime.Now;
		}
	}

	void uxDownloadReport_Click(object sender, EventArgs e)
	{
		ShowcaseItem.Filters filterList = new ShowcaseItem.Filters();
		filterList.FilterBeginDate = uxBeginDate.SelectedDate;
		filterList.FilterEndDate = uxEndDate.SelectedDate;
		if (!String.IsNullOrEmpty(uxFilterByAgentID.SelectedValue))
			filterList.FilterShowcaseItemAgentID = uxFilterByAgentID.SelectedValue;
		if (!String.IsNullOrEmpty(uxFilterByOfficeID.SelectedValue))
			filterList.FilterShowcaseItemOfficeID = uxFilterByOfficeID.SelectedValue;
		if (!String.IsNullOrEmpty(uxFilterByPropertyType.SelectedValue))
			filterList.FilterPropertyType = uxFilterByPropertyType.SelectedValue;
		if (!String.IsNullOrEmpty(uxFilterByShowcaseID.SelectedValue))
			filterList.FilterShowcaseItemShowcaseID = uxFilterByShowcaseID.SelectedValue;
		decimal temp;
		if (!String.IsNullOrEmpty(uxPriceRangeMin.Text) && Decimal.TryParse(uxPriceRangeMin.Text, out temp))
			filterList.FilterListPriceMin = temp;
		if (!String.IsNullOrEmpty(uxPriceRangeMax.Text) && Decimal.TryParse(uxPriceRangeMax.Text, out temp))
			filterList.FilterListPriceMax = temp;
		CSVWriteHelper.WriteCSVToResponse(ShowcaseItem.PageInventoryAndStatisticsReportForCSV(0, 0, m_SearchTextBox.Text, "NumberOfVisits", false, filterList).Rows, true, Response, "Property Inventory");
	}

	[WebMethod]
	public static ListingItemWithCount<ShowcaseItem> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, ShowcaseItem.Filters filterList = new ShowcaseItem.Filters())
	{
		int totalCount;
		filterList.FilterShowcaseItemActive = true.ToString();
		List<ShowcaseItem> listItems = ShowcaseItem.PageInventoryAndStatisticsReportWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList);

		listItems.ForEach(l => { l.Summary = null; });
		return new ListingItemWithCount<ShowcaseItem> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static Dictionary<string, int> GetPropertyStats(int showcaseItemID, string beginDateStr, string endDateStr)
	{
		DateTime temp;
		DateTime? beginDate = null;
		DateTime? endDate = null;
		if (!String.IsNullOrEmpty(beginDateStr) && DateTime.TryParse(beginDateStr, out temp))
			beginDate = temp;
		if (!String.IsNullOrEmpty(endDateStr) && DateTime.TryParse(endDateStr, out temp))
			endDate = temp;
		return ShowcaseItemMetric.GetStatisticsForProperty(showcaseItemID, beginDate, endDate);
	}
}