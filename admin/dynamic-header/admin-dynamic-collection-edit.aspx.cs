using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.DynamicHeader;

public partial class Admin_AdminDynamicCollectionEdit : BaseEditPage
{
	public DynamicCollection DynamicCollectionEntity { get; set; }

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-dynamic-collection.aspx";
		m_ClassName = "Collection";
		base.OnInit(e);
	} 

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{			
			if (EntityId > 0)
			{
				DynamicCollectionEntity = DynamicCollection.GetByID(EntityId);
				if (DynamicCollectionEntity == null) 
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
			}
			else
			{
				NewRecord = true;
			}
		}
    }

	protected override void Save()
	{
		if (IsValid)
		{
			DynamicCollectionEntity = EntityId > 0 ? DynamicCollection.GetByID(EntityId) : new DynamicCollection();
			DynamicCollectionEntity.Active = uxActive.Checked;
			DynamicCollectionEntity.Name = uxName.Text;
			DynamicCollectionEntity.Save();
			EntityId = DynamicCollectionEntity.DynamicCollectionID;
			m_ClassTitle = DynamicCollectionEntity.Name;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = DynamicCollectionEntity.Active;
		uxName.Text = DynamicCollectionEntity.Name;
	}
}