using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.Media352_MembershipProvider;

public partial class AdminDesignation : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Name";
		m_LinkToEditPage = "admin-designation-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Designation";
		base.OnInit(e);
	}

	[WebMethod]
	public static ListingItemWithCount<Designation> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<Designation> listItems = Designation.DesignationPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount);
		return new ListingItemWithCount<Designation> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		Designation entity = Designation.GetByID(id);
		if (entity != null)
			entity.Delete();
	}
}
