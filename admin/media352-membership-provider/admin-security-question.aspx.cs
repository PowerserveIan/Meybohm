using System;
using System.Collections.Generic;
using System.Web.Services;
using Classes.Media352_MembershipProvider;

public partial class AdminSecurityQuestion : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Active";
		m_LinkToEditPage = "admin-security-question-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Security Question";
		base.OnInit(e);
	}

	[WebMethod]
	public static ListingItemWithCount<SecurityQuestion> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		int totalCount;
		List<SecurityQuestion> listItems = SecurityQuestion.SecurityQuestionPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount);
		return new ListingItemWithCount<SecurityQuestion> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		SecurityQuestion entity = SecurityQuestion.GetByID(id);
		if (entity != null)
			entity.Delete();
	}

	[WebMethod]
	public static void ToggleActive(int id)
	{
		SecurityQuestion entity = SecurityQuestion.GetByID(id);
		if (entity != null)
		{
			entity.Active = !entity.Active;
			entity.Save();
		}
	}
}