using System;
using System.Collections.Generic;
using System.Web.Services;
using BaseCode;
using Classes.Showcase;

public partial class AdminShowcaseAttributeValue : BaseListingPage
{
	/// <summary>
	/// The id for the attribute the values belong to
	/// </summary>
	protected int ShowcaseAttributeId
	{
		get
		{
			int tempID;
			if (Request.QueryString["FilterShowcaseAttributeValueShowcaseAttributeID"] != null)
				if (Int32.TryParse(Request.QueryString["FilterShowcaseAttributeValueShowcaseAttributeID"], out tempID))
					return tempID;

			return 0;
		}
	}

	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "Value";
		m_LinkToEditPage = "admin-attribute-value-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Attribute Value";
		m_CustomBreadCrumbsPH = uxCustomBreadCrumbsPH;
		m_ColumnNumberToMakeLink = 2;
		base.OnInit(e);
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			if (ShowcaseAttributeId <= 0)
				Response.Redirect("~/admin/showcase/admin-attribute.aspx");

			ShowcaseAttribute attributeEntity = ShowcaseAttribute.GetByID(ShowcaseAttributeId);
			if (attributeEntity == null || (!ShowcaseHelpers.IsShowcaseAdmin() && !ShowcaseUser.ShowcaseUserGetByShowcaseID(attributeEntity.ShowcaseID).Exists(s => s.UserID == Helpers.GetCurrentUserID())))
				Response.Redirect("~/admin/showcase/admin-attribute.aspx");

			m_AddButton.Visible = uxAddMessage.Visible = String.IsNullOrEmpty(attributeEntity.MLSAttributeName);
			m_BreadCrumbTitle.Text = m_HeaderTitle.Text = attributeEntity.Title + @" " + m_BreadCrumbTitle.Text;
			uxDistanceFilterText.Visible = attributeEntity.ShowcaseFilterID == (int)FilterTypes.Distance || attributeEntity.ShowcaseFilterID == (int)FilterTypes.DistanceRange;
			if (Settings.MultipleShowcases && ShowcaseHelpers.UserCanManageOtherShowcases())
				uxShowcaseName.Text = @"<b>" + Showcases.GetByID(attributeEntity.ShowcaseID).Title + @"</b> ";
			else
				uxShowcaseName.Visible = false;
		}
		base.Page_Load(sender, e);
	}

	[WebMethod(true)]
	public static ListingItemWithCount<ShowcaseAttributeValue> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, ShowcaseAttributeValue.Filters filterList = new ShowcaseAttributeValue.Filters())
	{
		int totalCount;
		List<ShowcaseAttributeValue> listItems = ShowcaseAttributeValue.ShowcaseAttributeValuePageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList);
		return new ListingItemWithCount<ShowcaseAttributeValue> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		ShowcaseAttributeValue entity = ShowcaseAttributeValue.GetByID(id);
		if (entity != null)
			entity.Delete();
	}

	[WebMethod]
	public static void UpdateDisplayOrder(Dictionary<string, short> displayOrders, int attributeID)
	{
		List<ShowcaseAttributeValue> listItems = ShowcaseAttributeValue.ShowcaseAttributeValueGetByShowcaseAttributeID(attributeID);
		foreach (ShowcaseAttributeValue entity in listItems)
		{
			if (displayOrders.ContainsKey(entity.ShowcaseAttributeValueID.ToString()) && displayOrders[entity.ShowcaseAttributeValueID.ToString()] != entity.DisplayOrder)
			{
				entity.DisplayOrder = displayOrders[entity.ShowcaseAttributeValueID.ToString()];
				entity.Save();
			}
		}
	}

	[WebMethod]
	public static void ToggleDisplayInFilters(int id)
	{
		ShowcaseAttributeValue entity = ShowcaseAttributeValue.GetByID(id);
		if (entity != null)
		{
			entity.DisplayInFilters = !entity.DisplayInFilters;
			entity.Save();
		}
	}
}