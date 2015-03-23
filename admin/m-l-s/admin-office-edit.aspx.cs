using System;
using BaseCode;
using Classes.MLS;

public partial class Admin_AdminOfficeEdit : BaseEditPage
{
	public Office OfficeEntity { get; set; }
	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-office.aspx";
		m_ClassName = "Office";
		base.OnInit(e);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (EntityId > 0)
			{
				OfficeEntity = Office.GetByID(EntityId);
				if (OfficeEntity == null)
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

			OfficeEntity = EntityId > 0 ? Office.GetByID(EntityId) : new Office();
			OfficeEntity.Active = uxActive.Checked;
			OfficeEntity.AddressID = uxAddress.AddressID.Value;
			OfficeEntity.Fax = uxFax.Text;
			OfficeEntity.HasNewHomes = uxHasNewHomes.Checked;
			OfficeEntity.HasRentals = uxHasRentals.Checked;
			OfficeEntity.Image = uxImage.FileName;
			OfficeEntity.MlsID = Convert.ToInt32(uxMlsID.Text);
			OfficeEntity.Name = uxName.Text;
			OfficeEntity.Phone = uxPhone.Text;
			OfficeEntity.Save();

			EntityId = OfficeEntity.OfficeID;

			uxSEOData.PageLinkFormatterElements.Clear();
			uxSEOData.PageLinkFormatterElements.Add(EntityId.ToString());
			if (String.IsNullOrEmpty(uxSEOData.Title))
				uxSEOData.Title = uxName.Text;
			uxSEOData.SaveControlData();

			m_ClassTitle = OfficeEntity.Name;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = OfficeEntity.Active;
		uxAddress.AddressID = OfficeEntity.AddressID;
		uxAddress.Load();
		uxFax.Text = OfficeEntity.Fax;
		uxHasNewHomes.Checked = OfficeEntity.HasNewHomes;
		uxHasRentals.Checked = OfficeEntity.HasRentals;
		uxImage.FileName = OfficeEntity.Image;
		uxMlsID.Text = OfficeEntity.MlsID.ToString();
		uxName.Text = OfficeEntity.Name;
		uxPhone.Text = OfficeEntity.Phone;		
	}
}
