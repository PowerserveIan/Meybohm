using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.DynamicHeader;

public partial class AdminDynamicCollection : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{		
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Name";
		m_LinkToEditPage = "admin-dynamic-collection-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Collection";		
		base.OnInit(e);
	}

	[WebMethod]
	public static ListingItemWithCount<DynamicCollection> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<DynamicCollection> listItems = DynamicCollection.DynamicCollectionPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount);
		return new ListingItemWithCount<DynamicCollection> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		DynamicCollection entity = DynamicCollection.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		DynamicCollection entity = DynamicCollection.GetByID(id);
		if (entity != null)
			entity.Delete();
	}
}