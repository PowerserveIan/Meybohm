using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using BaseCode;
using Classes.Showcase;

public partial class AdminNewHomesForSale : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_DefaultSortField = "NumberOfVisits";
		m_LinkToEditPage = "../showcase/admin-showcase-item-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "New Homes for Sale";
		m_ShowFiltersByDefault = true;
		m_DisablePaging = true;
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
			uxFilterByShowcaseID.DataSource = Showcases.GetAll().Where(s => s.Title.ToLower().Contains("new")).OrderBy(s => s.Title);
			uxFilterByShowcaseID.DataTextField = "Title";
			uxFilterByShowcaseID.DataValueField = "ShowcaseID";
			uxFilterByShowcaseID.DataBind();

			uxFilterByNeighborhoodID.DataSource = Classes.MLS.Neighborhood.GetAll().OrderBy(n => n.Name);
			uxFilterByNeighborhoodID.DataTextField = "Name";
			uxFilterByNeighborhoodID.DataValueField = "NeighborhoodID";
			uxFilterByNeighborhoodID.DataBind();

			uxFilterByBuilderID.DataSource = Classes.MLS.Builder.GetAll().OrderBy(n => n.Name);
			uxFilterByBuilderID.DataTextField = "Name";
			uxFilterByBuilderID.DataValueField = "BuilderID";
			uxFilterByBuilderID.DataBind();
		}
	}

	private void uxDownloadReport_Click(object sender, EventArgs e)
	{
		ShowcaseItem.Filters filterList = new ShowcaseItem.Filters();
		if (!String.IsNullOrEmpty(uxFilterByBuilderID.SelectedValue))
			filterList.FilterShowcaseItemBuilderID = uxFilterByBuilderID.SelectedValue;
		if (!String.IsNullOrEmpty(uxFilterByNeighborhoodID.SelectedValue))
			filterList.FilterShowcaseItemNeighborhoodID = uxFilterByNeighborhoodID.SelectedValue;
		if (!String.IsNullOrEmpty(uxFilterByShowcaseID.SelectedValue))
			filterList.FilterShowcaseItemShowcaseID = uxFilterByShowcaseID.SelectedValue;
		CSVWriteHelper.WriteCSVToResponse(ShowcaseItem.GetNewHomesForSaleForCSV(m_SearchTextBox.Text, "NumberOfVisits", false, filterList).Rows, true, Response, "New Homes for Sale");
	}

	[WebMethod]
	public static ListingItemWithCount<ShowcaseItem> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, ShowcaseItem.Filters filterList = new ShowcaseItem.Filters())
	{
		int totalCount;
		filterList.FilterShowcaseItemActive = true.ToString();
		List<ShowcaseItem> listItems = ShowcaseItem.GetNewHomesForSaleWithTotalCount(searchText, sortField, sortDirection, out totalCount, filterList);

		listItems.ForEach(l => { l.Summary = null; });
		return new ListingItemWithCount<ShowcaseItem> { Items = listItems, TotalCount = totalCount };
	}	
}