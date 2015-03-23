using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.Redirects;

public partial class AdminRedirect : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "NewUrl";
		m_LinkToEditPage = "admin-redirect-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "301 Redirect";
		m_ColumnNumberToMakeLink = 0;
		base.OnInit(e);
	}

	[WebMethod]
	public static ListingItemWithCount<Redirect> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<Redirect> listItems = Redirect.RedirectPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount);
		foreach (Redirect r in listItems)
		{
			r.NewUrl = BaseCode.Helpers.ReplaceRootWithAbsolutePath(r.NewUrl);
			r.OldUrl = BaseCode.Helpers.ReplaceRootWithAbsolutePath(r.OldUrl);
		}
		return new ListingItemWithCount<Redirect> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		Redirect entity = Redirect.GetByID(id);
		if (entity != null)
			entity.Delete();
	}
}
