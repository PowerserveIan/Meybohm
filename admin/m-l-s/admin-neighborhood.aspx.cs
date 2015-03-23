using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Classes.MLS;

public partial class AdminNeighborhood : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Name";
		m_LinkToEditPage = "admin-neighborhood-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Neighborhood";
		m_ShowFiltersByDefault = true;
		base.OnInit(e);
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		base.Page_Load(sender, e);
		if (!IsPostBack)
		{
			uxFilterByMicrosite.DataSource = Classes.ContentManager.CMMicrosite.GetAll().OrderBy(c=>c.Name);
			uxFilterByMicrosite.DataTextField = "Name";
			uxFilterByMicrosite.DataValueField = "CMMicrositeID";
			uxFilterByMicrosite.DataBind();

			if (!String.IsNullOrEmpty(Request.QueryString["FilterNeighborhoodCMMicrositeID"]) && uxFilterByMicrosite.Items.FindByValue(Request.QueryString["FilterNeighborhoodCMMicrositeID"]) != null)
				uxFilterByMicrosite.Items.FindByValue(Request.QueryString["FilterNeighborhoodCMMicrositeID"]).Selected = true;
		}
	}

	[WebMethod]
	public static ListingItemWithCount<Neighborhood> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, Neighborhood.Filters filterList = new Neighborhood.Filters())
	{
		int totalCount;
		List<Neighborhood> listItems = Neighborhood.NeighborhoodPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList);
		return new ListingItemWithCount<Neighborhood> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		Neighborhood entity = Neighborhood.GetByID(id);
		if (entity != null)
			entity.Delete();
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		Neighborhood entity = Neighborhood.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}

	[WebMethod]
	public static void ToggleFeatured(int id)
	{
		Neighborhood entity = Neighborhood.GetByID(id);
		if (entity != null)
		{
			entity.Featured = !entity.Featured;
			entity.Save();
		}
	}
}
