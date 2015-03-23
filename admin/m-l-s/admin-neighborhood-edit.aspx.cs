using System;
using BaseCode;
using Classes.MLS;

public partial class Admin_AdminNeighborhoodEdit : BaseEditPage
{
	public Neighborhood NeighborhoodEntity { get; set; }
	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-neighborhood.aspx";
		m_ClassName = "Neighborhood";
		base.OnInit(e);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			uxCMMicrositeID.DataSource = Classes.ContentManager.CMMicrosite.CMMicrositeGetByActive(true, "Name");
			uxCMMicrositeID.DataTextField = "Name";
			uxCMMicrositeID.DataValueField = "CMMicrositeID";
			uxCMMicrositeID.DataBind();

			if (EntityId > 0)
			{
				NeighborhoodEntity = Neighborhood.GetByID(EntityId);
				if (NeighborhoodEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
			}
			else
				NewRecord = true;

			//SEO code
			if (EntityId > 0)
			{
				uxSEOData.PageLinkFormatterElements.Add(Convert.ToString(EntityId));
				uxSEOData.LoadControlData();
			}
			else
				uxSEOData.LoadControlData(true);
		}
	}

	protected override void Save()
	{
		uxImage.CommitChanges();
		if (IsValid)
		{
			uxAddress.Save();

			NeighborhoodEntity = EntityId > 0 ? Neighborhood.GetByID(EntityId) : new Neighborhood();
			NeighborhoodEntity.Active = uxActive.Checked;
			NeighborhoodEntity.AddressID = uxAddress.AddressID.Value;
			NeighborhoodEntity.Amenities = uxAmenities.EditorHTML;
			NeighborhoodEntity.CMMicrositeID = Convert.ToInt32(uxCMMicrositeID.SelectedValue);
			NeighborhoodEntity.Directions = uxDirections.EditorHTML;
			NeighborhoodEntity.Featured = uxFeatured.Checked;
			NeighborhoodEntity.Image = uxImage.FileName;
			NeighborhoodEntity.Name = uxName.Text;
			NeighborhoodEntity.Overview = uxOverview.EditorHTML;
			NeighborhoodEntity.Phone = uxPhone.Text;
			NeighborhoodEntity.PriceRange = uxPriceRange.Text;
			NeighborhoodEntity.ShowLotsLand = uxShowLotsLand.Checked;
			NeighborhoodEntity.Website = (!String.IsNullOrEmpty(uxWebsite.Text) && !uxWebsite.Text.StartsWith("http") ? "http://" : "") + uxWebsite.Text;
			NeighborhoodEntity.Save();

			EntityId = NeighborhoodEntity.NeighborhoodID;

			uxSEOData.PageLinkFormatterElements.Clear();
			uxSEOData.PageLinkFormatterElements.Add(EntityId.ToString());
			if (String.IsNullOrEmpty(uxSEOData.Title))
				uxSEOData.Title = uxName.Text;
			uxSEOData.SaveControlData();

			m_ClassTitle = NeighborhoodEntity.Name;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = NeighborhoodEntity.Active;
		uxAddress.AddressID = NeighborhoodEntity.AddressID;
		uxAddress.Load();
		uxAmenities.EditorHTML = NeighborhoodEntity.Amenities;
		if (uxCMMicrositeID.Items.FindByValue(NeighborhoodEntity.CMMicrositeID.ToString()) != null)
			uxCMMicrositeID.Items.FindByValue(NeighborhoodEntity.CMMicrositeID.ToString()).Selected = true;
		uxDirections.EditorHTML = NeighborhoodEntity.Directions;
		uxFeatured.Checked = NeighborhoodEntity.Featured;
		uxImage.FileName = NeighborhoodEntity.Image;
		uxName.Text = NeighborhoodEntity.Name;
		uxOverview.EditorHTML = NeighborhoodEntity.Overview;
		uxPhone.Text = NeighborhoodEntity.Phone;
		uxPriceRange.Text = NeighborhoodEntity.PriceRange;
		uxShowLotsLand.Checked = NeighborhoodEntity.ShowLotsLand;
		uxWebsite.Text = NeighborhoodEntity.Website;
	}
}
