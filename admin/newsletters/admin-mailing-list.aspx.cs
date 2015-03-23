using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.Newsletters;

public partial class AdminMailingList : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Name";
		m_LinkToEditPage = "admin-mailing-list-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Mailing List";
		base.OnInit(e);
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxNoMoreMailingLists.Text = @"<h3>" + Settings.MaxNumberMailingListsErrorMessage + @"</h3>";
		string visibleCondition = Settings.EnableMailingListLimitations.ToString().ToLower() + @" && " + Settings.MaxNumberMailingLists + @" <= listingModel.totalCount()";
		uxNoMoreMailingLists.Attributes.Add("data-bind", "visible:" + visibleCondition);
		m_AddButton.Attributes.Add("data-bind", "visible:!(" + visibleCondition + ")");
	}

	[WebMethod]
	public static ListingItemWithCount<MailingList> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<MailingList> listItems = MailingList.MailingListPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, new MailingList.Filters { FilterMailingListDeleted = false.ToString() });
		return new ListingItemWithCount<MailingList> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		MailingList entity = MailingList.GetByID(id);
		if (entity != null)
		{
			entity.Deleted = true;
			entity.Save();
		}
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		MailingList entity = MailingList.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}
}