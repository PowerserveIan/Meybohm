using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using BaseCode;
using Classes.Showcase;

public partial class AdminShowcases : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		if (!Settings.MultipleShowcases)
			Response.Redirect("~/admin");
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Title";
		m_LinkToEditPage = "admin-showcases-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Showcase";
		base.OnInit(e);
		m_AddButton.Visible =
		uxActiveHeaderPlaceHolder.Visible =
		uxManagedByHeaderPlaceHolder.Visible =
		uxShowcaseAdminOnlyPH.Visible =
		uxShowcaseAdminOnly2PH.Visible = ShowcaseHelpers.IsShowcaseAdmin();
		if (!ShowcaseHelpers.UserCanManageOtherShowcases())
			Response.Redirect("~/admin/");
	}

	[WebMethod]
	public static ListingItemWithCount<Showcases> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		Showcases.Filters filterList = new Showcases.Filters();
		if (!ShowcaseHelpers.IsShowcaseAdmin())
			filterList.FilterShowcaseUserUserID = Helpers.GetCurrentUserID().ToString();
		List<Showcases> listItems = Showcases.ShowcasesPageWithManagersWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList);
		return new ListingItemWithCount<Showcases> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod(true)]
	public static void DeleteRecord(int id)
	{
		Showcases entity = Showcases.GetByID(id);
		if (entity != null)
			entity.Delete();
		if (HttpContext.Current.Session["ShowcaseID"] != null && id == Convert.ToInt32(HttpContext.Current.Session["ShowcaseID"]))
			HttpContext.Current.Session.Remove("ShowcaseID");
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		Showcases entity = Showcases.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}
}