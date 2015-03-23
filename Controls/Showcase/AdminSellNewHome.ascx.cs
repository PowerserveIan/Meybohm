using System;
using System.Linq;
using Classes.NewHomes;

public partial class Controls_Showcase_AdminSellNewHome : System.Web.UI.UserControl
{
	public int ShowcaseItemID { get; set; }

	public SoldHome SoldHomeEntity { get; set; }

	private bool m_Required;
	public bool Required
	{
		get { return m_Required; }
		set
		{
			m_Required = value;
			uxSoldHomeCloseDate.Required =
			uxSoldHomeSalePriceRFV.Enabled = value;
		}
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		if (!IsPostBack)
		{
			uxSoldHomeListingAgent.DataSource = uxSoldHomeSalesAgent.DataSource = Classes.Media352_MembershipProvider.UserInfo.GetAllAgents();
			uxSoldHomeListingAgent.DataTextField = uxSoldHomeSalesAgent.DataTextField = "FirstAndLast";
			uxSoldHomeListingAgent.DataValueField = uxSoldHomeSalesAgent.DataValueField = "UserID";
			uxSoldHomeListingAgent.DataBind();
			uxSoldHomeSalesAgent.DataBind();

			uxSoldHomeSellerOffice.DataSource = Classes.MLS.Office.GetAll();
			uxSoldHomeSellerOffice.DataTextField = "Name";
			uxSoldHomeSellerOffice.DataValueField = "OfficeID";
			uxSoldHomeSellerOffice.DataBind();

			uxSoldHomeAddSalesAgent.Visible = uxSoldHomeAddSeller.Visible = Page.User.IsInRole("Admin");
		}
	}

	public void LoadData()
	{
		SoldHomeEntity = SoldHome.SoldHomeGetByShowcaseItemID(ShowcaseItemID).FirstOrDefault();
		if (SoldHomeEntity != null)
		{
			uxSoldHomeCloseDate.SelectedDate = SoldHomeEntity.CloseDate;
			if (uxSoldHomeListingAgent.Items.FindByValue(SoldHomeEntity.ListingAgentID.ToString()) != null)
				uxSoldHomeListingAgent.Items.FindByValue(SoldHomeEntity.ListingAgentID.ToString()).Selected = true;
			uxSoldHomeSalePrice.Text = SoldHomeEntity.SalePrice.ToString().Replace(".00", "");
			if (uxSoldHomeSalesAgent.Items.FindByValue(SoldHomeEntity.SalesAgentID.ToString()) != null)
				uxSoldHomeSalesAgent.Items.FindByValue(SoldHomeEntity.SalesAgentID.ToString()).Selected = true;
			uxSoldHomeSalesAgentPercentage.Text = SoldHomeEntity.SalesAgentPercentage.ToString().Replace(".000", "");
			if (uxSoldHomeSellerOffice.Items.FindByValue(SoldHomeEntity.SellerOfficeID.ToString()) != null)
				uxSoldHomeSellerOffice.Items.FindByValue(SoldHomeEntity.SellerOfficeID.ToString()).Selected = true;
			uxSoldHomeSellerOfficePercentage.Text = SoldHomeEntity.SellerOfficePercentage.ToString().Replace(".000", "");
			uxSoldHomeSellerPercentage.Text = SoldHomeEntity.SellerPercentage.ToString().Replace(".000", "");
		}
	}

	public void SaveData()
	{
		SoldHomeEntity = SoldHome.SoldHomeGetByShowcaseItemID(ShowcaseItemID).FirstOrDefault();
		if (SoldHomeEntity == null)
			SoldHomeEntity = new SoldHome { ShowcaseItemID = ShowcaseItemID };
		SoldHomeEntity.CloseDate = uxSoldHomeCloseDate.SelectedDate.Value;
		SoldHomeEntity.ListingAgentID = !String.IsNullOrEmpty(uxSoldHomeListingAgent.SelectedValue) ? (int?)Convert.ToInt32(uxSoldHomeListingAgent.SelectedValue) : null;
		SoldHomeEntity.SalePrice = Convert.ToDecimal(uxSoldHomeSalePrice.Text);
		SoldHomeEntity.SalesAgentID = !String.IsNullOrEmpty(uxSoldHomeSalesAgent.SelectedValue) ? (int?)Convert.ToInt32(uxSoldHomeSalesAgent.SelectedValue) : null;
		SoldHomeEntity.SalesAgentPercentage = !String.IsNullOrEmpty(uxSoldHomeSalesAgentPercentage.Text) ? (decimal?)Convert.ToDecimal(uxSoldHomeSalesAgentPercentage.Text) : null;
		SoldHomeEntity.SellerOfficeID = !String.IsNullOrEmpty(uxSoldHomeSellerOffice.SelectedValue) ? (int?)Convert.ToInt32(uxSoldHomeSellerOffice.SelectedValue) : null;
		SoldHomeEntity.SellerOfficePercentage = !String.IsNullOrEmpty(uxSoldHomeSellerOfficePercentage.Text) ? (decimal?)Convert.ToDecimal(uxSoldHomeSellerOfficePercentage.Text) : null;
		SoldHomeEntity.SellerPercentage = !String.IsNullOrEmpty(uxSoldHomeSellerPercentage.Text) ? (decimal?)Convert.ToDecimal(uxSoldHomeSellerPercentage.Text) : null;
		SoldHomeEntity.Save();
	}

	public void ClearForm()
	{
		uxSoldHomeCloseDate.SelectedDate = null;
		uxSoldHomeListingAgent.ClearSelection();
		uxSoldHomeSalesAgent.ClearSelection();
		uxSoldHomeSellerOffice.ClearSelection();
		uxSoldHomeSalePrice.Text = 
		uxSoldHomeSalesAgentPercentage.Text = 
		uxSoldHomeSellerOfficePercentage.Text = 
		uxSoldHomeSellerPercentage.Text = string.Empty;
	}
}