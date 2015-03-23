using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using BaseCode;
using Classes.DynamicHeader;

public partial class AdminDynamicImage : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Title";
		m_LinkToEditPage = "admin-dynamic-image-edit.aspx?id=";
		m_ClassName = "Slide";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		base.OnInit(e);
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		base.Page_Load(sender, e);
		if (!IsPostBack)
		{
			uxFilterByCollection.DataSource = DynamicCollection.GetAll().OrderBy(c => c.Name);
			uxFilterByCollection.DataTextField = "Name";
			uxFilterByCollection.DataValueField = "DynamicCollectionID";
			uxFilterByCollection.DataBind();

			uxFilterByCollection.Visible = uxFilterByCollection.Items.Count > 2;
			if (!String.IsNullOrEmpty(Request.QueryString["FilterDynamicCollectionID"]) && uxFilterByCollection.Items.FindByValue(Request.QueryString["FilterDynamicCollectionID"]) != null)
				uxFilterByCollection.Items.FindByValue(Request.QueryString["FilterDynamicCollectionID"]).Selected = true;
		}
	}

	[WebMethod]
	public static ListingItemWithCount<DynamicImage> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, DynamicImage.Filters filterList = new DynamicImage.Filters())
	{
		int totalCount;
		List<DynamicImage> listItems = DynamicImage.PageByCollectionIDWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList);
		return new ListingItemWithCount<DynamicImage> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		DynamicImage entity = DynamicImage.GetByID(id);
		if (entity != null)
		{
			String imagePath = HttpContext.Current.Server.MapPath("~/" + Globals.Settings.UploadFolder + "images/" + entity.Name);
			if (File.Exists(imagePath))
				File.Delete(imagePath);
			entity.Delete();
		}
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		DynamicImage entity = DynamicImage.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.LastUpdated = DateTime.UtcNow;
			entity.Save();
		}
	}	

	[WebMethod]
	public static void UpdateDisplayOrder(Dictionary<string, short> displayOrders, int dynamicCollectionID)
	{
		List<DynamicImageCollection> listItems = DynamicImageCollection.DynamicImageCollectionGetByDynamicCollectionID(dynamicCollectionID);
		foreach (DynamicImageCollection entity in listItems)
		{
			if (displayOrders.ContainsKey(entity.DynamicImageID.ToString()) && displayOrders[entity.DynamicImageID.ToString()] != entity.DisplayOrder)
			{
				entity.DisplayOrder = displayOrders[entity.DynamicImageID.ToString()];
				entity.Save();
			}
		}
		Helpers.PurgeCacheItems("DynamicHeader_DynamicImage");
	}
}