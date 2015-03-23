using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using BaseCode;
using Classes.NewHomes;
using Classes.Showcase;

public partial class SalesReport : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "CloseDate";
		m_LinkToEditPage = "";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "New Home Sales Report";
		m_ShowFiltersByDefault = true;
		m_ColumnNumberToMakeLink = 0;
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
			uxFilterByOfficeID.DataSource = Classes.MLS.Office.GetAll().OrderBy(n => n.Name);
			uxFilterByOfficeID.DataTextField = "Name";
			uxFilterByOfficeID.DataValueField = "OfficeID";
			uxFilterByOfficeID.DataBind();

			uxFilterByAgentID.DataSource = Classes.Media352_MembershipProvider.UserInfo.GetAllAgents();
			uxFilterByAgentID.DataTextField = "FirstAndLast";
			uxFilterByAgentID.DataValueField = "UserID";
			uxFilterByAgentID.DataBind();

			uxBeginDate.TextBoxControl.Attributes.Add("data-bind", "value:pageFilter.FilterBeginDate,css:{error:!listingModel.startDateValid()}");
			uxEndDate.TextBoxControl.Attributes.Add("data-bind", "value:pageFilter.FilterEndDate,css:{error:!listingModel.endDateValid()}");
		}
	}

	void uxDownloadReport_Click(object sender, EventArgs e)
	{
		SoldHome.Filters filterList = new SoldHome.Filters();
		filterList.FilterBeginDate = uxBeginDate.SelectedDate;
		filterList.FilterEndDate = uxEndDate.SelectedDate;
		if (!String.IsNullOrEmpty(uxFilterByAgentID.SelectedValue))
			filterList.FilterSoldHomeListingAgentID = uxFilterByAgentID.SelectedValue;
		if (!String.IsNullOrEmpty(uxFilterByOfficeID.SelectedValue))
			filterList.FilterSoldHomeSellerOfficeID = uxFilterByOfficeID.SelectedValue;
		decimal temp;
		CSVWriteHelper.WriteCSVToResponse(SoldHome.SoldHomeReport(0, 0, m_SearchTextBox.Text, "CloseDate", true, out temp, filterList), true, Response, "New Homes Sold");
	}

	[WebMethod]
	public static ListingItemWithAdditionalOutput<SoldHomeReportItem> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, SoldHome.Filters filterList = new SoldHome.Filters())
	{
		int totalCount;
		decimal totalSales;
		List<SoldHomeReportItem> listItems = SoldHome.SoldHomeReportWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, out totalSales, filterList);
		return new ListingItemWithAdditionalOutput<SoldHomeReportItem> { Items = listItems, TotalCount = totalCount, TotalSales = totalSales };
	}
}