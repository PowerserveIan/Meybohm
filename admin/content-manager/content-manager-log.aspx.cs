using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.ContentManager;
using System.Collections.Generic;

namespace ContentManager2.Admin
{
	/// <summary>
	/// 	Written By: Charles Cook
	/// </summary>
	public partial class ContentManagerLog : Page
	{
		private int PageId
		{
			get
			{
				if (ViewState["PageId"] == null)
					return 0;
				return (int)ViewState["PageId"];
			}
			set { ViewState["PageId"] = value; }
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			DataBinding += ContentManagerLog_DataBinding;
			pageLog.ItemDataBound += pageLog_ItemDataBound;
			lbBack.Click += lbBack_Click;
			pageLog.ItemCommand += pageLog_ItemCommand;
			Back.Click += Back_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				try
				{
					PageId = int.Parse(Request.QueryString["id"]);
				}
				catch (FormatException)
				{
				}
				catch (ArgumentNullException)
				{
				}
				if (PageId == 0)
					Response.Redirect("~/admin/content-manager/content-manager.aspx");

				DataBind();
			}
		}

		private void ContentManagerLog_DataBinding(object sender, EventArgs e)
		{
			CMPage cmPage = CMPage.GetByID(PageId);
			if (cmPage == null)
				Response.Redirect("~/admin/content-manager/content-manager.aspx");
			List<CMPageRegion> pageRegions = CMPageRegion.CMPageRegionPage(0, 0, "", "", true, new CMPageRegion.Filters { FilterCMPageRegionCMPageID = PageId.ToString() });
			Dictionary<int, string> distinctRegions = new Dictionary<int, string>();
			foreach (CMPageRegion region in pageRegions)
			{
				if (!distinctRegions.ContainsKey(region.CMRegionID))
					distinctRegions.Add(region.CMRegionID, CMRegion.GetByID(region.CMRegionID).Name);
			}
			pageLog.DataSource = distinctRegions;
		}

		private void pageLog_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
		}

		private void pageLog_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Repeater regionLog = (Repeater)e.Item.FindControl("regionLog");
				KeyValuePair<int, string> cmPR = (KeyValuePair<int, string>)e.Item.DataItem;
				regionLog.DataSource = (from pr in CMPageRegion.CMPageRegionGetByCMRegionID(cmPR.Key) where pr.CMPageID == PageId && !pr.NeedsApproval orderby pr.Created descending select pr).ToList();
				regionLog.DataBind();
			}
		}

		protected string GetUserName(CMPageRegion cmPR)
		{
			if (cmPR.UserID != null && cmPR.UserID > 0)

				return Classes.Media352_MembershipProvider.User.GetByID(cmPR.UserID.Value).Name;
			return "N/A";
		}

		private void Back_Click(object sender, ImageClickEventArgs e)
		{
			Response.Redirect("~/admin/content-manager/content-manager.aspx");
		}

		private void lbBack_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/admin/content-manager/content-manager.aspx");
		}
	}
}