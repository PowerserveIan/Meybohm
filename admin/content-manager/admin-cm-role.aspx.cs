using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.Media352_MembershipProvider;

public partial class AdminCMRole : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Name";
		m_LinkToEditPage = "admin-cm-role-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "CM Role";
		base.OnInit(e);
	}

	[WebMethod]
	public static ListingItemWithCount<Role> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<Role> listItems = Role.RolePageWithUsersInRoleWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, new Role.Filters { FilterRoleSystemRole = false.ToString() });
		return new ListingItemWithCount<Role> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		Role entity = Role.GetByID(id);
		if (entity != null)
			entity.Delete();
	}
}