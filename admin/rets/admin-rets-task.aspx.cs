using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.Rets;

public partial class AdminRetsTask : BaseListingPage
{
	protected static string ParentID = null;

	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "TaskName";
		ParentID = Request.QueryString["id"];
		m_LinkToEditPage = ParentID == null ? "admin-rets-task.aspx?id=" : "admin-rets-task-status.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Rets Task";
		base.OnInit(e);
		m_AddButton.Visible = false;
	}

	[WebMethod]
	public static ListingItemWithCount<RetsTask> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		RetsTask.Filters filterList = new RetsTask.Filters();
		filterList.FilterRetsTaskParentRetsTaskID = ParentID ?? string.Empty;
		List<RetsTask> listItems = RetsTask.RetsTaskPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList);
		return new ListingItemWithCount<RetsTask> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		RetsTask entity = RetsTask.GetByID(id);
		if (entity != null)
			entity.Delete();
	}
}
