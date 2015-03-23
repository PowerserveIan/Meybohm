using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.WhatsNearBy;

public partial class AdminWhatsNearByCategory : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Name";
		m_LinkToEditPage = "admin-whats-near-by-category-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "What's Near By Category";
		base.OnInit(e);
	}

	[WebMethod]
	public static ListingItemWithCount<WhatsNearByCategory> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<WhatsNearByCategory> listItems = WhatsNearByCategory.WhatsNearByCategoryPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount);
		return new ListingItemWithCount<WhatsNearByCategory> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		WhatsNearByCategory entity = WhatsNearByCategory.GetByID(id);
		if (entity != null)
			entity.Delete();
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		WhatsNearByCategory entity = WhatsNearByCategory.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}
}
