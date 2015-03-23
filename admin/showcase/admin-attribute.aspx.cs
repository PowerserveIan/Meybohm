using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.Showcase;

public partial class AdminShowcaseAttribute : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		if (!ShowcaseHelpers.GetCurrentShowcaseID().HasValue)
			Response.Redirect("~/admin/showcase/admin-showcases.aspx");		
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "DisplayOrder";
		m_LinkToEditPage = "admin-attribute-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = (Settings.MultipleShowcases && ShowcaseHelpers.UserCanManageOtherShowcases() ? Showcases.GetByID(ShowcaseHelpers.GetCurrentShowcaseID().Value).Title + " " : "") + "Attribute";
		m_ColumnNumberToMakeLink = 2;
		base.OnInit(e);
		if (!IsPostBack)
			uxFilterTHPlaceHolder.Visible = uxFilterPH.Visible = Settings.EnableFilters;
	}

	[WebMethod(true)]
	public static ListingItemWithCount<ShowcaseAttribute> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<ShowcaseAttribute> listItems = ShowcaseAttribute.ShowcaseAttributePageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, new ShowcaseAttribute.Filters { FilterShowcaseAttributeShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID().ToString(), FilterShowcaseAttributeImportItemAttribute = true.ToString() });
		return new ListingItemWithCount<ShowcaseAttribute> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		ShowcaseAttribute entity = ShowcaseAttribute.GetByID(id);
		if (entity != null)
			entity.Delete();
	} 

	[WebMethod]
	public static void ToggleActive(int id)
	{
		ShowcaseAttribute entity = ShowcaseAttribute.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}

	[WebMethod(true)]
	public static void UpdateDisplayOrder(Dictionary<string, short> displayOrders)
	{
		List<ShowcaseAttribute> listItems = ShowcaseAttribute.ShowcaseAttributeGetByShowcaseID(ShowcaseHelpers.GetCurrentShowcaseID().Value);
		foreach (ShowcaseAttribute entity in listItems)
		{
			if (displayOrders.ContainsKey(entity.ShowcaseAttributeID.ToString()) && displayOrders[entity.ShowcaseAttributeID.ToString()] != entity.DisplayOrder)
			{
				entity.DisplayOrder = displayOrders[entity.ShowcaseAttributeID.ToString()];
				entity.Save();
			}
		}
	}
}