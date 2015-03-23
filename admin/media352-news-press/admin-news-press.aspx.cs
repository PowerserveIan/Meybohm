using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.Media352_NewsPress;

public partial class AdminNewsPress : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Title";
		m_LinkToEditPage = "admin-news-press-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "News Press Article";
		base.OnInit(e);
	}

	[WebMethod]
	public static ListingItemWithCount<NewsPress> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<NewsPress> listItems = NewsPress.NewsPressPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount);
		listItems.ForEach(n => n.StoryHTML = n.Summary = null); //To keep the JSON response size down
		return new ListingItemWithCount<NewsPress> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		NewsPress entity = NewsPress.GetByID(id);
		if (entity != null)
			entity.Delete();
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		NewsPress entity = NewsPress.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}

	[WebMethod]
	public static void ToggleFeatured(int id)
	{
		NewsPress entity = NewsPress.GetByID(id);
		if (entity != null)
		{
			entity.Featured = !entity.Featured;
			entity.Save();
		}
	}
}