using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Classes.NewHomes;
using Classes.Showcase;

public partial class NewHomesSold : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "CloseDate";
		m_LinkToEditPage = "";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "New Home Sale";
		m_ShowFiltersByDefault = true;
		m_ColumnNumberToMakeLink = 0;
		base.OnInit(e);
		m_AddButton.NavigateUrl = "#sellHome";
		m_AddButton.Attributes.Remove("data-bind");
		uxSaveSoldHome.Click += uxSaveSoldHome_Click;
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

			uxFilterByAgentID.DataSource = Classes.Media352_MembershipProvider.UserInfo.GetAllAgents();
			uxFilterByAgentID.DataTextField = "FirstAndLast";
			uxFilterByAgentID.DataValueField = "UserID";
			uxFilterByAgentID.DataBind();

			uxFilterByNeighborhoodID.DataSource = Classes.MLS.Neighborhood.GetAll().OrderBy(n => n.Name);
			uxFilterByNeighborhoodID.DataTextField = "Name";
			uxFilterByNeighborhoodID.DataValueField = "NeighborhoodID";
			uxFilterByNeighborhoodID.DataBind();

			uxFilterByBuilderID.DataSource = Classes.MLS.Builder.GetAll().OrderBy(n => n.Name);
			uxFilterByBuilderID.DataTextField = "Name";
			uxFilterByBuilderID.DataValueField = "BuilderID";
			uxFilterByBuilderID.DataBind();
			
			uxBeginDate.TextBoxControl.Attributes.Add("data-bind", "value:pageFilter.FilterBeginDate,css:{error:!listingModel.startDateValid()}");
			uxEndDate.TextBoxControl.Attributes.Add("data-bind", "value:pageFilter.FilterEndDate,css:{error:!listingModel.endDateValid()}");
		}
	}

	void uxSaveSoldHome_Click(object sender, EventArgs e)
	{
		if (IsValid)
		{
			uxSoldHomeControl.ShowcaseItemID = Convert.ToInt32(uxShowcaseItemID.Value);
			uxSoldHomeControl.SaveData();
			uxSoldHomeControl.ClearForm();
		}
	}

	[WebMethod]
	public static ListingItemWithCount<SoldHome> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, SoldHome.Filters filterList = new SoldHome.Filters())
	{
		int totalCount;
		List<SoldHome> listItems = SoldHome.SoldHomePageForAdminWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList, new string[] { "ShowcaseItem", "ListingAgent.UserInfo", "ShowcaseItem.Neighborhood", "ShowcaseItem.Builder" });

		listItems.ForEach(l => { l.ShowcaseItem.Summary = null; });
		return new ListingItemWithCount<SoldHome> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		SoldHome entity = SoldHome.GetByID(id);
		if (entity != null)
			entity.Delete();
	}

	[WebMethod]
	public static SoldHomeInfoForEdit GetSoldHomeInfo(string mlsIDOrAddress)
	{
		ShowcaseItem entity;
		int mlsID;
		if (Int32.TryParse(mlsIDOrAddress, out mlsID))
			entity = ShowcaseItem.ShowcaseItemGetByMlsID(mlsID, string.Empty, true, new string[] { "Address" }).FirstOrDefault();
		else
			entity = ShowcaseItem.GetByAddressLine1(mlsIDOrAddress, new string[] { "Address" });
		if (entity == null)
			return null;
		SoldHomeInfoForEdit soldHome = new SoldHomeInfoForEdit { Address = entity.Address, ShowcaseItemID = entity.ShowcaseItemID, SoldHome = SoldHome.SoldHomeGetByShowcaseItemID(entity.ShowcaseItemID).FirstOrDefault() };
		if (soldHome.SoldHome != null)
			soldHome.SoldHome.ShowcaseItem = null;
		return soldHome;
	}

	public class SoldHomeInfoForEdit
	{
		public Classes.StateAndCountry.Address Address { get; set; }
		public int ShowcaseItemID { get; set; }
		public SoldHome SoldHome { get; set; }
	}
}