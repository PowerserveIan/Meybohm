using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.WhatsNearBy;

public partial class AdminWhatsNearByLocation : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Name";
		m_LinkToEditPage = "admin-whats-near-by-location-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "What's Near By Location";
		base.OnInit(e);
	}

	[WebMethod]
	public static ListingItemWithCount<WhatsNearByLocation> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<WhatsNearByLocation> listItems = WhatsNearByLocation.WhatsNearByLocationPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, new WhatsNearByLocation.Filters(), new string[] { "Address", "Address.State" });
		return new ListingItemWithCount<WhatsNearByLocation> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		WhatsNearByLocation entity = WhatsNearByLocation.GetByID(id);
		if (entity != null)
			entity.Delete();
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		WhatsNearByLocation entity = WhatsNearByLocation.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}
}
