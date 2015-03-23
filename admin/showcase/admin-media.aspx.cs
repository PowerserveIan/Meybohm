using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Services;
using BaseCode;
using Classes.Showcase;

public partial class AdminMedia : BaseListingPage
{
	/// <summary>
	/// The id for the media collection item the media belong to
	/// </summary>
	protected int MediaCollectionId
	{
		get
		{
			int tempID;
			if (Request.QueryString["FilterMediaShowcaseMediaCollectionID"] != null)
				if (Int32.TryParse(Request.QueryString["FilterMediaShowcaseMediaCollectionID"], out tempID))
					return tempID;

			return 0;
		}
	}

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
		m_LinkToEditPage = "admin-media-edit.aspx?id=";
		m_ClassName = "Media";
		m_CustomBreadCrumbsPH = uxCustomBreadCrumbsPH;
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ColumnNumberToMakeLink = 2;
		base.OnInit(e);
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			if (MediaCollectionId <= 0)
				Response.Redirect("~/admin/showcase/admin-media-collection.aspx?FilterMediaCollectionShowcaseItemID=" + ShowcaseItemId);

			MediaCollection collectionEntity = MediaCollection.GetByID(MediaCollectionId);

			if (collectionEntity == null)
				Response.Redirect("~/admin/showcase/admin-media-collection.aspx?FilterMediaCollectionShowcaseItemID=" + ShowcaseItemId);

			ShowcaseItem itemEntity = ShowcaseItem.GetByID(collectionEntity.ShowcaseItemID);
			if (itemEntity == null || (!ShowcaseHelpers.IsShowcaseAdmin() && !ShowcaseUser.ShowcaseUserGetByShowcaseID(itemEntity.ShowcaseID).Exists(s => s.UserID == Helpers.GetCurrentUserID())))
				Response.Redirect("~/admin/showcase/admin-media-collection.aspx?FilterMediaCollectionShowcaseItemID=" + ShowcaseItemId);

			uxLinkToMediaCollectionManager.NavigateUrl = "~/admin/showcase/admin-media-collection.aspx?FilterMediaCollectionShowcaseItemID=" + ShowcaseItemId;
			uxLinkToMediaCollectionManager.Text = @"<b>" + itemEntity.Title + @"</b> Media Collection Manager";
			m_BreadCrumbTitle.Text = m_HeaderTitle.Text = @"<b>" + MediaCollection.GetByID(MediaCollectionId).Title + @"</b> " + m_BreadCrumbTitle.Text;
			if (Settings.MultipleShowcases && ShowcaseHelpers.UserCanManageOtherShowcases())
				uxShowcaseName.Text = @"<b>" + Showcases.GetByID(itemEntity.ShowcaseID).Title + @"</b>";
			else
				uxShowcaseName.Text = @"Showcase";
		}
		base.Page_Load(sender, e);
	}

	[WebMethod(true)]
	public static ListingItemWithCount<Media> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, Media.Filters filterList = new Media.Filters())
	{
		int totalCount;
		List<Media> listItems = Media.MediaPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList);
		return new ListingItemWithCount<Media> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		Media entity = Media.GetByID(id);
		if (entity != null)
		{
			string imagePath = HttpContext.Current.Server.MapPath("~/" + Globals.Settings.UploadFolder + "images/" + entity.Thumbnail);
			if (File.Exists(imagePath))
				File.Delete(imagePath);

			imagePath = HttpContext.Current.Server.MapPath("~/" + Globals.Settings.UploadFolder + "images/" + entity.URL);
			if (File.Exists(imagePath))
				File.Delete(imagePath);
			entity.Delete();
		}
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		Media entity = Media.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}

	[WebMethod]
	public static void UpdateDisplayOrder(Dictionary<string, short> displayOrders, int collectionID)
	{
		List<Media> listItems = Media.MediaGetByShowcaseMediaCollectionID(collectionID);
		foreach (Media entity in listItems)
		{
			if (displayOrders.ContainsKey(entity.ShowcaseMediaID.ToString()) && displayOrders[entity.ShowcaseMediaID.ToString()] != entity.DisplayOrder)
			{
				entity.DisplayOrder = displayOrders[entity.ShowcaseMediaID.ToString()];
				entity.Save();
			}
		}
	}
}