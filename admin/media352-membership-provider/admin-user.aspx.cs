using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using BaseCode;
using Classes.Media352_MembershipProvider;

public partial class AdminUser : BaseListingPage
{
	protected bool? Staff
	{
		get
		{
			bool temp;
			if (!String.IsNullOrEmpty(Request.QueryString["FilterUserHasRole"]) && Boolean.TryParse(Request.QueryString["FilterUserHasRole"], out temp))
				return temp;
			return null;
		}
	}

	protected string RoleName
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["FilterUserRoleName"]))
				return Request.QueryString["FilterUserRoleName"];
			return null;
		}
	}

	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Email";
		m_LinkToEditPage = "admin-user-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = Staff.HasValue && !Staff.Value ? "Customer" : !String.IsNullOrWhiteSpace(RoleName) ? "Agent" : "Staff";
		base.OnInit(e);
		if (Staff.HasValue && !Staff.Value)
			m_SearchPanel.Controls.Add(uxDownloadCustomers);
		else
			uxDownloadCustomers.Visible = false;
		uxDownloadCustomers.Click += uxDownloadCustomers_Click;
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxUserNameIsEmailHeaderPH.Visible = uxUserNameNotEmailPH.Visible = !Settings.UserNameIsEmail;
		if (Settings.UserNameIsEmail)
			uxEmailTD.Attributes["class"] = uxEmailHeader.Attributes["class"] = "first";
		uxUserInfoHeaderFirstName.Visible = uxUserInfoHeaderLastName.Visible = uxComplexPH.Visible = Settings.UserManager == UserManagerType.Complex;
	}

	void uxDownloadCustomers_Click(object sender, EventArgs e)
	{
		CSVWriteHelper.WriteCSVToResponse(Classes.Media352_MembershipProvider.User.GetCustomerListForCSV(), true, Response, "Customers");
	}

	[WebMethod]
	public static ListingItemWithCount<User> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, User.Filters filterList = new User.Filters())
	{
		int totalCount;
		List<User> listItems = Classes.Media352_MembershipProvider.User.UserPageByAdminListWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList, new string[] { "UserInfo" });
		return new ListingItemWithCount<User> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		User entity = Classes.Media352_MembershipProvider.User.GetByID(id);
		if (entity != null)
			entity.Delete();
	}

	[WebMethod]
	public static void ToggleLocked(int id)
	{
		User entity = Classes.Media352_MembershipProvider.User.GetByID(id);
		if (entity != null)
		{
			entity.FailedPasswordAttempts = 0;
			entity.Locked = false;
			entity.Save();
		}
	}
}