using System;
using BaseCode;
using Classes.WhatsNearBy;

public partial class Admin_AdminWhatsNearByCategoryEdit : BaseEditPage
{
	public WhatsNearByCategory WhatsNearByCategoryEntity { get; set; }
	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-whats-near-by-category.aspx";
		m_ClassName = "What's Near By Category";
		base.OnInit(e);
	} 

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (EntityId > 0)
			{
				WhatsNearByCategoryEntity = WhatsNearByCategory.GetByID(EntityId);
				if (WhatsNearByCategoryEntity == null) 
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
			}
			else
				NewRecord = true;
		}
	}

	protected override void Save()
	{
		uxPlaceholderImage.CommitChanges();
		if (IsValid)
		{
			WhatsNearByCategoryEntity = EntityId > 0 ? WhatsNearByCategory.GetByID(EntityId) : new WhatsNearByCategory();
			WhatsNearByCategoryEntity.Active = uxActive.Checked;
			WhatsNearByCategoryEntity.Name = uxName.Text;
			WhatsNearByCategoryEntity.PlaceholderImage = uxPlaceholderImage.FileName;
			WhatsNearByCategoryEntity.Save();

			EntityId = WhatsNearByCategoryEntity.WhatsNearByCategoryID;
			m_ClassTitle = WhatsNearByCategoryEntity.Name;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = WhatsNearByCategoryEntity.Active;
		uxName.Text = WhatsNearByCategoryEntity.Name;
		uxPlaceholderImage.FileName = WhatsNearByCategoryEntity.PlaceholderImage;
	}
}
