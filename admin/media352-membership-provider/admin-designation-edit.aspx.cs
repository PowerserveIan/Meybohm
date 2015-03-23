using System;
using BaseCode;
using Classes.Media352_MembershipProvider;

public partial class Admin_AdminDesignationEdit : BaseEditPage
{
	public Designation DesignationEntity { get; set; }
	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-designation.aspx";
		m_ClassName = "Designation";
		base.OnInit(e);
	} 

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (EntityId > 0)
			{
				DesignationEntity = Designation.GetByID(EntityId);
				if (DesignationEntity == null) 
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
			}
			else
				NewRecord = true;
		}
	}

	protected override void Save()
	{
		uxIcon.CommitChanges();
		if (IsValid)
		{
			DesignationEntity = EntityId > 0 ? Designation.GetByID(EntityId) : new Designation();
			DesignationEntity.Icon = uxIcon.FileName;
			DesignationEntity.Name = uxName.Text;
			DesignationEntity.Save();

			EntityId = DesignationEntity.DesignationID;
			m_ClassTitle = DesignationEntity.Name;
		}
	}

	protected override void LoadData()
	{
		uxIcon.FileName = DesignationEntity.Icon;
		uxName.Text = DesignationEntity.Name;
	}
}
