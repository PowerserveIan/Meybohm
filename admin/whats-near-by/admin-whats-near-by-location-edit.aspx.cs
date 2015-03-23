using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.WhatsNearBy;

public partial class Admin_AdminWhatsNearByLocationEdit : BaseEditPage
{
	public WhatsNearByLocation WhatsNearByLocationEntity { get; set; }
	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-whats-near-by-location.aspx";
		m_ClassName = "What's Near By Location";
		base.OnInit(e);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			uxCategory.DataSource = WhatsNearByCategory.GetAll();
			uxCategory.DataTextField = "Name";
			uxCategory.DataValueField = "WhatsNearByCategoryID";
			uxCategory.DataBind();

			if (EntityId > 0)
			{
				WhatsNearByLocationEntity = WhatsNearByLocation.GetByID(EntityId);
				if (WhatsNearByLocationEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
				uxDescription.HideEditorInitially = true;
			}
			else
				NewRecord = true;
		}
	}

	protected override void Save()
	{
		uxImage.CommitChanges();
		if (IsValid)
		{
			uxAddress.Save();

			WhatsNearByLocationEntity = EntityId > 0 ? WhatsNearByLocation.GetByID(EntityId) : new WhatsNearByLocation();
			WhatsNearByLocationEntity.Active = uxActive.Checked;			
			WhatsNearByLocationEntity.AddressID = uxAddress.AddressID.Value;
			WhatsNearByLocationEntity.Description = uxDescription.EditorHTML;
			WhatsNearByLocationEntity.Image = uxImage.FileName;
			WhatsNearByLocationEntity.Name = uxName.Text;
			WhatsNearByLocationEntity.Phone = uxPhone.Text;
			WhatsNearByLocationEntity.Website = (!String.IsNullOrEmpty(uxWebsite.Text) && !uxWebsite.Text.StartsWith("http") ? "http://" : "") + uxWebsite.Text;
			uxWebsite.Text = WhatsNearByLocationEntity.Website;
			WhatsNearByLocationEntity.Save();

			SaveCategories();

			EntityId = WhatsNearByLocationEntity.WhatsNearByLocationID;

			uxDescription.HideEditorInitially = true;
			m_ClassTitle = WhatsNearByLocationEntity.Name;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = WhatsNearByLocationEntity.Active;
		uxAddress.AddressID = WhatsNearByLocationEntity.AddressID;
		uxAddress.Load();
		uxDescription.EditorHTML = WhatsNearByLocationEntity.Description;
		uxImage.FileName = WhatsNearByLocationEntity.Image;
		uxName.Text = WhatsNearByLocationEntity.Name;
		uxPhone.Text = WhatsNearByLocationEntity.Phone;
		uxWebsite.Text = WhatsNearByLocationEntity.Website;
		
		LoadCategories();
	}

	#region Collection setup items

	private void LoadCategories()
	{
		List<WhatsNearByLocationCategory> joins = WhatsNearByLocationCategory.WhatsNearByLocationCategoryGetByWhatsNearByLocationID(WhatsNearByLocationEntity.WhatsNearByLocationID);
		foreach (WhatsNearByLocationCategory join in joins)
		{
			if (uxCategory.Items.FindByValue(join.WhatsNearByCategoryID.ToString()) != null)
				uxCategory.Items.FindByValue(join.WhatsNearByCategoryID.ToString()).Selected = true;
		}
	}

	/// <summary>
	/// Updates the list of selected categories in the WhatsNearByLocationCategory table.
	/// </summary>
	private void SaveCategories()
	{
		List<WhatsNearByLocationCategory> joins = WhatsNearByLocationCategory.WhatsNearByLocationCategoryGetByWhatsNearByLocationID(WhatsNearByLocationEntity.WhatsNearByLocationID);
		foreach (ListItem li in uxCategory.Items)
		{
			WhatsNearByLocationCategory join = joins.Find(
				npc => npc.WhatsNearByCategoryID == Convert.ToInt32(li.Value));
			if (join != null)
			{
				if (!li.Selected)
					join.Delete();
			}
			else
			{
				if (li.Selected)
				{
					join = new WhatsNearByLocationCategory();
					join.WhatsNearByCategoryID = Convert.ToInt32(li.Value);
					join.WhatsNearByLocationID = WhatsNearByLocationEntity.WhatsNearByLocationID;
					join.Save();
				}
			}
		}
	}

	#endregion

	protected void uxCategory_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = uxCategory.SelectedItem != null;
	}
}
