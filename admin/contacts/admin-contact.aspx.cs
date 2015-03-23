using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Classes.Contacts;

public partial class AdminContact : BaseListingPage
{
	protected ContactTypes m_ContactType
	{
		get
		{
			return BaseCode.EnumParser.Parse<ContactTypes>(Request.QueryString["FilterContactContactTypeID"]);
		}
	}

	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "ContactStatusID";
		m_LinkToEditPage = "admin-contact-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = m_ContactType == ContactTypes.HomeValuationRequest ? "Home Valuation Request" : m_ContactType == ContactTypes.MaintenanceRequest ? "Maintenance Request" : m_ContactType == ContactTypes.PropertyInformation ? "Property Information Request" : m_ContactType == ContactTypes.Agent ? "Agent Request" : "Contact";
		m_ColumnNumberToMakeLink = 0;
		m_ShowFiltersByDefault = true;
		base.OnInit(e);
		m_AddButton.Visible = false;
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		base.Page_Load(sender, e);
		if (!IsPostBack)
		{
			uxFilterByMicrosite.DataSource = Classes.ContentManager.CMMicrosite.GetAll().OrderBy(c => c.Name);
			uxFilterByMicrosite.DataTextField = "Name";
			uxFilterByMicrosite.DataValueField = "CMMicrositeID";
			uxFilterByMicrosite.DataBind();

			if (!String.IsNullOrEmpty(Request.QueryString["FilterContactCMMicrositeID"]) && uxFilterByMicrosite.Items.FindByValue(Request.QueryString["FilterContactCMMicrositeID"]) != null)
				uxFilterByMicrosite.Items.FindByValue(Request.QueryString["FilterContactCMMicrositeID"]).Selected = true;

			if (m_ContactType == ContactTypes.PropertyInformation)
			{
				uxFilterByShowcaseItemID.DataSource = Contact.GetAllPropertiesWithContactRequests();
				uxFilterByShowcaseItemID.DataTextField = "Title";
				uxFilterByShowcaseItemID.DataValueField = "ShowcaseItemID";
				uxFilterByShowcaseItemID.DataBind();

				uxFilterByPropertyType.DataSource = Classes.Showcase.Showcases.ShowcasesGetByActive(true, "Title");
				uxFilterByPropertyType.DataTextField = "Title";
				uxFilterByPropertyType.DataValueField = "ShowcaseID";
				uxFilterByPropertyType.DataBind();

				if (!String.IsNullOrEmpty(Request.QueryString["FilterContactShowcaseItemID"]) && uxFilterByShowcaseItemID.Items.FindByValue(Request.QueryString["FilterContactShowcaseItemID"]) != null)
					uxFilterByShowcaseItemID.Items.FindByValue(Request.QueryString["FilterContactShowcaseItemID"]).Selected = true;
				if (!String.IsNullOrEmpty(Request.QueryString["FilterShowcaseID"]) && uxFilterByPropertyType.Items.FindByValue(Request.QueryString["FilterShowcaseID"]) != null)
					uxFilterByPropertyType.Items.FindByValue(Request.QueryString["FilterShowcaseID"]).Selected = true;
			}
			else
				uxPropertySpecificPH.Visible = false;

			if (m_ContactType == ContactTypes.Agent)
			{
				uxFilterByAgentID.DataSource = Contact.GetAllAgentsWithContactRequests();
				uxFilterByAgentID.DataTextField = "FirstAndLast";
				uxFilterByAgentID.DataValueField = "UserID";
				uxFilterByAgentID.DataBind();

				if (!String.IsNullOrEmpty(Request.QueryString["FilterContactAgentID"]) && uxFilterByAgentID.Items.FindByValue(Request.QueryString["FilterContactAgentID"]) != null)
					uxFilterByAgentID.Items.FindByValue(Request.QueryString["FilterContactAgentID"]).Selected = true;
			}
			else
				uxAgentPH.Visible = false;
		}
	}

	[WebMethod]
	public static ListingItemWithCount<Contact> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, Contact.Filters filterList = new Contact.Filters())
	{
		int totalCount;
		List<string> includeList = new string[] { "ContactStatus" }.ToList();
		bool propertyInformation = !String.IsNullOrEmpty(filterList.FilterContactContactTypeID) && Convert.ToInt32(filterList.FilterContactContactTypeID) == (int)ContactTypes.PropertyInformation;
		if (propertyInformation)
			includeList.Add("ShowcaseItem.Address.State");
		List<Contact> listItems = propertyInformation ? Contact.PropertyInformationPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList, includeList) : Contact.ContactPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList, includeList);
		return new ListingItemWithCount<Contact> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		Contact entity = Contact.GetByID(id);
		if (entity != null)
			entity.Delete();
	}
}
