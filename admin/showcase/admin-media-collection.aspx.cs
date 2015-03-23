using System;
using System.Collections.Generic;
using System.Web.Services;
using BaseCode;
using Classes.Showcase;

public partial class AdminMediaCollection : BaseListingPage
{
	/// <summary>
	/// The id for the showcase item the collections belong to
	/// </summary>
	protected int ShowcaseItemId
	{
		get
		{
			int tempID;
			if (Request.QueryString["FilterMediaCollectionShowcaseItemID"] != null)
				if (Int32.TryParse(Request.QueryString["FilterMediaCollectionShowcaseItemID"], out tempID))
					return tempID;

			return 0;
		}
	}

	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "DisplayOrder";
		m_LinkToEditPage = "admin-media-collection-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Media Collection";
		m_CustomBreadCrumbsPH = uxCustomBreadCrumbsPH;
		m_ColumnNumberToMakeLink = 2;
		base.OnInit(e);
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			if (ShowcaseItemId <= 0)
				Response.Redirect("~/admin/showcase/admin-showcase-item.aspx");

			ShowcaseItem itemEntity = ShowcaseItem.GetByID(ShowcaseItemId);
			if (itemEntity == null || (!ShowcaseHelpers.IsShowcaseAdmin() && !ShowcaseUser.ShowcaseUserGetByShowcaseID(itemEntity.ShowcaseID).Exists(s => s.UserID == Helpers.GetCurrentUserID())))
				Response.Redirect("~/admin/showcase/admin-showcase-item.aspx");

			m_BreadCrumbTitle.Text = m_HeaderTitle.Text = itemEntity.Title + @" " + m_BreadCrumbTitle.Text;
			if (Settings.MultipleShowcases && ShowcaseHelpers.UserCanManageOtherShowcases())
				uxShowcaseName.Text = @"<b>" + Showcases.GetByID(itemEntity.ShowcaseID).Title + @"</b>";
			else
				uxShowcaseName.Text = @"Showcase";
		}
		base.Page_Load(sender, e);
	}

	[WebMethod]
	public static ListingItemWithCount<MediaCollection> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, MediaCollection.Filters filterList = new MediaCollection.Filters())
	{
		int totalCount;
		List<MediaCollection> listItems = MediaCollection.MediaCollectionPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList, new string[] { "ShowcaseMediaType" });
		return new ListingItemWithCount<MediaCollection> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		MediaCollection entity = MediaCollection.GetByID(id);
		if (entity != null)
			entity.Delete();
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		MediaCollection entity = MediaCollection.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}

	[WebMethod]
	public static void UpdateDisplayOrder(Dictionary<string, short> displayOrders, int showcaseItemID)
	{
		List<MediaCollection> listItems = MediaCollection.MediaCollectionGetByShowcaseItemID(showcaseItemID);
		foreach (MediaCollection entity in listItems)
		{
			if (displayOrders.ContainsKey(entity.ShowcaseMediaCollectionID.ToString()) && displayOrders[entity.ShowcaseMediaCollectionID.ToString()] != entity.DisplayOrder)
			{
				entity.DisplayOrder = displayOrders[entity.ShowcaseMediaCollectionID.ToString()];
				entity.Save();
			}
		}
	}
}