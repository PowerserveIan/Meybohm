using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.Videos;

public partial class AdminVideo : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "DisplayOrder";
		m_LinkToEditPage = "admin-video-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Video";
		m_ColumnNumberToMakeLink = 2;
		base.OnInit(e);
	}

	[WebMethod]
	public static ListingItemWithCount<Video> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<Video> listItems = Video.VideoPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount);
		return new ListingItemWithCount<Video> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		Video entity = Video.GetByID(id);
		if (entity != null)
			entity.Delete();
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		Video entity = Video.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}

	[WebMethod]
	public static void ToggleFeatured(int id)
	{
		Video entity = Video.GetByID(id);
		if (entity != null)
		{
			entity.Featured = !entity.Featured;
			entity.Save();
		}
	}

	[WebMethod]
	public static void UpdateDisplayOrder(Dictionary<string, short> displayOrders)
	{
		List<Video> listItems = Video.GetAll();
		foreach (Video entity in listItems)
		{
			if (displayOrders.ContainsKey(entity.VideoID.ToString()) && displayOrders[entity.VideoID.ToString()] != entity.DisplayOrder)
			{
				entity.DisplayOrder = displayOrders[entity.VideoID.ToString()];
				entity.Save();
			}
		}
	}
}
