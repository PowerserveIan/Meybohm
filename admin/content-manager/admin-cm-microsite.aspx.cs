using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.ContentManager;
using System.Web.Services;
using System.Web;

public partial class AdminCMMicrosite : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		if (!Settings.EnableMicrosites)
			Response.Redirect("~/admin/");
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Active";
		m_LinkToEditPage = "admin-cm-microsite-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Microsite";
		base.OnInit(e);
	}

	[WebMethod]
	public static ListingItemWithCount<CMMicrosite> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<CMMicrosite> listItems = CMMicrosite.CMMicrositePageWithManagersWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount);
		return new ListingItemWithCount<CMMicrosite> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		CMMicrosite entity = CMMicrosite.GetByID(id);
		if (entity != null)
		{
			// delete the corresponding upload folders
			if (Directory.Exists(HttpContext.Current.Server.MapPath("~/" + Globals.Settings.UploadFolder + "images/" + entity.CMMicroSiteID)))
				Directory.Delete(HttpContext.Current.Server.MapPath("~/" + Globals.Settings.UploadFolder + "images/" + entity.CMMicroSiteID), true);
			if (Directory.Exists(HttpContext.Current.Server.MapPath("~/" + Globals.Settings.UploadFolder + "docs/" + entity.CMMicroSiteID)))
				Directory.Delete(HttpContext.Current.Server.MapPath("~/" + Globals.Settings.UploadFolder + "docs/" + entity.CMMicroSiteID), true);
			if (Directory.Exists(HttpContext.Current.Server.MapPath("~/" + Globals.Settings.UploadFolder + "videos/" + entity.CMMicroSiteID)))
				Directory.Delete(HttpContext.Current.Server.MapPath("~/" + Globals.Settings.UploadFolder + "videos/" + entity.CMMicroSiteID), true);
			if (Directory.Exists(HttpContext.Current.Server.MapPath("~/" + Globals.Settings.UploadFolder + "flash/" + entity.CMMicroSiteID)))
				Directory.Delete(HttpContext.Current.Server.MapPath("~/" + Globals.Settings.UploadFolder + "flash/" + entity.CMMicroSiteID), true);
			entity.Delete();
		}
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		CMMicrosite entity = CMMicrosite.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}
}