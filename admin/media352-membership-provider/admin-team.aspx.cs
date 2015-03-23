using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.Media352_MembershipProvider;

public partial class AdminTeam : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Name";
		m_LinkToEditPage = "admin-team-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Team";
		base.OnInit(e);
	}

	[WebMethod]
	public static ListingItemWithCount<Team> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<Team> listItems = Team.TeamPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount);
		return new ListingItemWithCount<Team> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		Team entity = Team.GetByID(id);
		if (entity != null)
			entity.Delete();
	}
}
