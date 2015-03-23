using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.ContentManager;

public partial class AdminCMSubmittedForm : BaseListingPage
{
	protected bool IsDeveloper
	{
		get { return (User.IsInRole("Admin") || User.IsInRole("CMS Admin") || User.IsInRole("CMS Content Integrator")); }
	}

	protected int MicroSiteID
	{
		get { return Session["MicroSiteID"] != null ? Convert.ToInt32(Session["MicroSiteID"]) : 0; }
		set { Session["MicroSiteID"] = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "IsProcessed";
		m_LinkToEditPage = "admin-cm-submitted-form-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Submitted Form";
		base.OnInit(e);
		m_AddButton.Visible = false;
		uxMicrositeList.SelectedIndexChanged += uxMicrositeList_SelectedIndexChanged;
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		base.Page_Load(sender, e);
		if (!IsPostBack)
		{
			if (Settings.EnableMicrosites)
			{
				uxMicrositePlaceHolder.Visible = true;
				uxMicrositeList.Visible = false;
				if (IsDeveloper || User.IsInRole("Microsite Admin"))
				{
					List<CMMicrosite> tempMicrositeList = IsDeveloper ? CMMicrosite.CMMicrositeGetByActive(true) : CMMicrosite.GetMicrositesByUserID(Helpers.GetCurrentUserID()).Where(c => c.Active).ToList();

					if (IsDeveloper)
					{
						uxMicrositeList.Items.Add(new ListItem("Main Site", ""));
						uxMicrositeList.AppendDataBoundItems = true;
					}

					if (tempMicrositeList.Count > 1)
					{
						uxMicrositeList.Visible = true;

						uxMicrositeList.DataSource = tempMicrositeList;
						uxMicrositeList.DataTextField = "Name";
						uxMicrositeList.DataValueField = "CMMicroSiteID";
						if (!IsDeveloper && MicroSiteID == 0)
							MicroSiteID = tempMicrositeList[0].CMMicroSiteID;
					}
					else if (tempMicrositeList.Count == 1)
					{
						if (IsDeveloper)
						{
							uxMicrositeList.Visible = true;

							uxMicrositeList.DataSource = tempMicrositeList;
							uxMicrositeList.DataTextField = "Name";
							uxMicrositeList.DataValueField = "CMMicroSiteID";
						}
						else
						{
							uxMicrositeList.Visible = false;
							uxMicroSiteName.Text = tempMicrositeList[0].Name;
							MicroSiteID = tempMicrositeList[0].CMMicroSiteID;
						}
					}
					else if (tempMicrositeList.Count == 0 && !IsDeveloper)
					{
						uxMicrositeInactive.Visible = true;
						uxHideThisIfMSAdmin.Visible = false;
						return;
					}
				}
				if (MicroSiteID < 0 || (MicroSiteID > 0 && (CMMicrosite.GetByID(MicroSiteID) == null || !CMMicrosite.GetByID(MicroSiteID).Active)))
					MicroSiteID = 0;
				uxMicrositeList.DataBind();
			}
			else if (!IsDeveloper && User.IsInRole("Microsite Admin"))
				Response.Redirect("~/admin");

			if (uxMicrositeList.Visible)
			{
				if (MicroSiteID > 0)
					uxMicrositeList.SelectedValue = MicroSiteID.ToString();
				uxMicroSiteName.Text = uxMicrositeList.SelectedItem.Text;
			}
		}
	}

	private void uxMicrositeList_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (String.IsNullOrEmpty(uxMicrositeList.SelectedValue))
			MicroSiteID = 0;
		else
			MicroSiteID = Convert.ToInt32(uxMicrositeList.SelectedValue);

		uxMicroSiteName.Text = uxMicrositeList.SelectedItem.Text;
	}

	[WebMethod]
	public static ListingItemWithCount<CMSubmittedForm> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, CMSubmittedForm.Filters filterList = new CMSubmittedForm.Filters())
	{
		int totalCount;
		List<CMSubmittedForm> listItems = CMSubmittedForm.CMSubmittedFormPageWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList);
		return new ListingItemWithCount<CMSubmittedForm> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		CMSubmittedForm entity = CMSubmittedForm.GetByID(id);
		if (entity != null)
			entity.Delete();
	}
}