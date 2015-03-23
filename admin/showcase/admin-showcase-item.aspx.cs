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
		if (!String.IsNullOrEmpty(Request.QueryString["ShowcaseID"]))
			Session["ShowcaseID"] = Request.QueryString["ShowcaseID"];
		if (!ShowcaseHelpers.GetCurrentShowcaseID().HasValue)
			Response.Redirect("~/admin/showcase/admin-showcases.aspx");
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Title";
		m_LinkToEditPage = "admin-showcase-item-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = (Settings.MultipleShowcases && ShowcaseHelpers.UserCanManageOtherShowcases() ? Showcases.GetByID(ShowcaseHelpers.GetCurrentShowcaseID().Value).Title + " " : "") + "Property";
		m_ShowFiltersByDefault = true;
		base.OnInit(e);
		uxFilterMessage.Visible = Settings.EnableFilters;
		m_AddButton.Visible = uxAddMessage.Visible = uxRentalPH.Visible = uxRentalPH2.Visible = uxRentalStatusPH.Visible = !ShowcaseHelpers.IsCurrentShowcaseMLS();
		uxNonRentalPH.Visible = uxNonRentalPH2.Visible = uxOfficeFilterPH.Visible = !uxRentalPH.Visible;
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		base.Page_Load(sender, e);
		if (!IsPostBack)
		{
			uxFilterByAgentID.DataSource = Classes.Media352_MembershipProvider.UserInfo.GetAllAgents(ShowcaseHelpers.GetCurrentShowcaseID().Value);
			uxFilterByAgentID.DataTextField = "FirstAndLast";
			uxFilterByAgentID.DataValueField = "UserID";
			uxFilterByAgentID.DataBind();

			uxFilterByNeighborhoodID.DataSource = Classes.MLS.Neighborhood.GetAll().OrderBy(n => n.Name);
			uxFilterByNeighborhoodID.DataTextField = "Name";
			uxFilterByNeighborhoodID.DataValueField = "NeighborhoodID";
			uxFilterByNeighborhoodID.DataBind();

			uxFilterByOfficeID.DataSource = Classes.MLS.Office.OfficeGetByIsMeybohm(true, "Name");
			uxFilterByOfficeID.DataTextField = "Name";
			uxFilterByOfficeID.DataValueField = "OfficeID";
			uxFilterByOfficeID.DataBind();

			if (!String.IsNullOrEmpty(Request.QueryString["FilterShowcaseItemAgentID"]) && uxFilterByAgentID.Items.FindByValue(Request.QueryString["FilterShowcaseItemAgentID"]) != null)
				uxFilterByAgentID.Items.FindByValue(Request.QueryString["FilterShowcaseItemAgentID"]).Selected = true;
			if (!String.IsNullOrEmpty(Request.QueryString["FilterShowcaseItemNeighborhoodID"]) && uxFilterByNeighborhoodID.Items.FindByValue(Request.QueryString["FilterShowcaseItemNeighborhoodID"]) != null)
				uxFilterByNeighborhoodID.Items.FindByValue(Request.QueryString["FilterShowcaseItemNeighborhoodID"]).Selected = true;
			if (!String.IsNullOrEmpty(Request.QueryString["FilterShowcaseItemOfficeID"]) && uxFilterByOfficeID.Items.FindByValue(Request.QueryString["FilterShowcaseItemOfficeID"]) != null)
				uxFilterByOfficeID.Items.FindByValue(Request.QueryString["FilterShowcaseItemOfficeID"]).Selected = true;
			if (!String.IsNullOrEmpty(Request.QueryString["FilterShowcaseItemRented"]) && uxFilterByRentalStatus.Items.FindByValue(Request.QueryString["FilterShowcaseItemRented"]) != null)
				uxFilterByRentalStatus.Items.FindByValue(Request.QueryString["FilterShowcaseItemRented"]).Selected = true;
		}
	}

	[WebMethod(true)]
	public static ListingItemWithCount<ShowcaseItem> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, ShowcaseItem.Filters filterList = new ShowcaseItem.Filters())
	{
		int totalCount;
		filterList.FilterShowcaseItemShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID().ToString();
		int temp;
		if (Int32.TryParse(searchText, out temp))
		{
			filterList.FilterShowcaseItemMlsID = temp.ToString();
			searchText = string.Empty;
		}
		List<string> includeList = new List<string> { "Neighborhood" };
		if (!ShowcaseHelpers.IsCurrentShowcaseMLS())
		{
			includeList.Add("Address");
			includeList.Add("Agent");
		}
		List<ShowcaseItem> listItems = ShowcaseItem.ShowcaseItemPageForAdminWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList, includeList);

		listItems.ForEach(l => { l.Summary = Helpers.ForceShorten(l.Summary, 100); });
		return new ListingItemWithCount<ShowcaseItem> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		ShowcaseItem entity = ShowcaseItem.GetByID(id);
		if (entity != null)
		{
			string imagePath = HttpContext.Current.Server.MapPath("~/" + Globals.Settings.UploadFolder + "images/" + entity.Image);
			if (File.Exists(imagePath))
				File.Delete(imagePath);
			entity.Delete();
		}
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		ShowcaseItem entity = ShowcaseItem.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}

	[WebMethod]
	public static void ToggleFeatured(int id)
	{
		ShowcaseItem entity = ShowcaseItem.GetByID(id);
		if (entity != null)
		{
			entity.Featured = !entity.Featured;
			entity.Save();
		}
	}
}