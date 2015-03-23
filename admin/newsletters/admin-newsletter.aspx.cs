using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Classes.Newsletters;

public partial class AdminNewsletters : BaseListingPage
{
	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "CreatedDate";
		m_LinkToEditPage = "admin-newsletter-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Newsletter";
		m_DefaultSortDirection = false;
		m_ShowFiltersByDefault = true;
		base.OnInit(e);
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		base.Page_Load(sender, e);
		if (!IsPostBack)
		{
			if (Settings.EnableMailingListLimitations && Settings.MaxEmailsPerMonth > 0)
			{
				uxEmailLimitationsPH.Visible = true;
				int used = NewsletterAction.GetNumberOfUsersEmailedInLastMonth();
				if (used >= Settings.MaxEmailsPerMonth)
				{
					uxEmailsRemaining.Text = @"You have reached the maximum number of emails you may mail out via the Newsletter component this month.";
					uxMailoutEnabledPH.Visible = false;
				}
				else
					uxEmailsRemaining.Text = @"You may send out " + (Settings.MaxEmailsPerMonth - used) + @" more emails via the Newsletter component this month.";
			}

			uxFilterByMicrosite.DataSource = Classes.ContentManager.CMMicrosite.GetAll().OrderBy(c => c.Name);
			uxFilterByMicrosite.DataTextField = "Name";
			uxFilterByMicrosite.DataValueField = "CMMicrositeID";
			uxFilterByMicrosite.DataBind();

			if (!String.IsNullOrEmpty(Request.QueryString["FilterNewsletterCMMicrositeID"]) && uxFilterByMicrosite.Items.FindByValue(Request.QueryString["FilterNewsletterCMMicrositeID"]) != null)
				uxFilterByMicrosite.Items.FindByValue(Request.QueryString["FilterNewsletterCMMicrositeID"]).Selected = true;
		}
	}

	[WebMethod]
	public static ListingItemWithCount<Newsletter> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, Newsletter.Filters filterList = new Newsletter.Filters())
	{
		int totalCount;
		filterList.FilterNewsletterDeleted = false.ToString();
		List<Newsletter> listItems = Newsletter.NewsletterPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList);
		return new ListingItemWithCount<Newsletter> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		Newsletter entity = Newsletter.GetByID(id);
		if (entity != null)
		{
			entity.Deleted = true;
			entity.Save();
		}
	}
}