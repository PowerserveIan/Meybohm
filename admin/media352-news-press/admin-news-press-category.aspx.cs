using System;
using System.Collections.Generic;
using System.Web.Services;
using BaseCode;
using Classes.Media352_NewsPress;

public partial class AdminNewsPressCategory : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Name";
		m_LinkToEditPage = "admin-news-press-category-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "News Press Category";
		base.OnInit(e);
	}

	[WebMethod]
	public static ListingItemWithCount<NewsPressCategory> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<NewsPressCategory> listItems = NewsPressCategory.NewsPressCategoryPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount);
		return new ListingItemWithCount<NewsPressCategory> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		NewsPressCategory entity = NewsPressCategory.GetByID(id);
		if (entity != null)
			entity.Delete();
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		NewsPressCategory entity = NewsPressCategory.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
			Helpers.PurgeCacheItems("Media352_NewsPress");
		}
	}
}