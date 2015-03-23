using System;
using System.Collections.Generic;
using BaseCode;
using Classes.MLS;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Admin_AdminBuilderEdit : BaseEditPage
{
	public Builder BuilderEntity { get; set; }
	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-builder.aspx";
		m_ClassName = "Builder";
		base.OnInit(e);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			uxMicrosites.DataSource = Classes.ContentManager.CMMicrosite.CMMicrositeGetByActive(true, "Name");
			uxMicrosites.DataTextField = "Name";
			uxMicrosites.DataValueField = "CMMicrositeID";
			uxMicrosites.DataBind();

			if (EntityId > 0)
			{
				BuilderEntity = Builder.GetByID(EntityId);
				if (BuilderEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();

			}
			else
			{
				NewRecord = true;
				uxAfterSavePH.Visible = false;
			}

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
			BuilderEntity = EntityId > 0 ? Builder.GetByID(EntityId) : new Builder();
			BuilderEntity.Active = uxActive.Checked;
			BuilderEntity.Image = uxImage.FileName;
			BuilderEntity.Info = uxInfo.EditorHTML;
			BuilderEntity.Name = uxName.Text;
			BuilderEntity.OwnerName = uxOwnerName.Text;
			BuilderEntity.Website = (!String.IsNullOrEmpty(uxWebsite.Text) && !uxWebsite.Text.StartsWith("http") ? "http://" : "") + uxWebsite.Text;
			uxWebsite.Text = BuilderEntity.Website;
			BuilderEntity.Save();
			SaveMicrosites();
			if (NewRecord)
			{
				uxAfterSavePH.Visible = true;
				BindNeighborhoodList();
			}			

			EntityId = BuilderEntity.BuilderID;

			uxSEOData.PageLinkFormatterElements.Clear();
			uxSEOData.PageLinkFormatterElements.Add(EntityId.ToString());
			if (String.IsNullOrEmpty(uxSEOData.Title))
				uxSEOData.Title = uxName.Text;
			uxSEOData.SaveControlData();

			m_ClassTitle = BuilderEntity.Name;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = BuilderEntity.Active;
		uxImage.FileName = BuilderEntity.Image;
		uxInfo.EditorHTML = BuilderEntity.Info;
		uxName.Text = BuilderEntity.Name;
		uxOwnerName.Text = BuilderEntity.OwnerName;
		uxWebsite.Text = BuilderEntity.Website;
		LoadMicrosites();
		BindNeighborhoodList();
	}

	void BindNeighborhoodList()
	{
		List<NeighborhoodBuilder> existingNeighborhoods = new List<NeighborhoodBuilder>();
		if (EntityId > 0)
		{
			existingNeighborhoods = NeighborhoodBuilder.NeighborhoodBuilderGetByBuilderID(EntityId, "Neighborhood.Name", true, new string[] { "Neighborhood" }.ToList());
			uxExistingNeighborhoods.DataSource = existingNeighborhoods;
			uxExistingNeighborhoods.DataBind();
		}

		uxNeighborhoods.DataSource = Neighborhood.GetAll().Where(n => !existingNeighborhoods.Any(nn => nn.NeighborhoodID == n.NeighborhoodID)).OrderBy(n => n.Name);
		uxNeighborhoods.DataTextField = "Name";
		uxNeighborhoods.DataValueField = "NeighborhoodID";
		uxNeighborhoods.DataBind();
		uxNeighborhoods.Items.Insert(0, new ListItem("--Select a Neighborhood--", ""));
	}

	protected void Neighborhood_Command(object sender, CommandEventArgs e)
	{
		if (e.CommandName == "Add")
		{
			NeighborhoodBuilder nbEntity = new NeighborhoodBuilder();
			nbEntity.BuilderID = EntityId;
			nbEntity.NeighborhoodID = Convert.ToInt32(uxNeighborhoods.SelectedValue);
			nbEntity.Save();
		}
		else if (e.CommandName == "Delete")
		{
			NeighborhoodBuilder nbEntity = NeighborhoodBuilder.GetByID(Convert.ToInt32(e.CommandArgument.ToString()));
			nbEntity.Delete();
		}

		m_CollapseFormAfterSave = false;
		BindNeighborhoodList();
		Helpers.PageView.Anchor(Page, "neighborhoods");
	}

	#region Collection setup items

	private void LoadMicrosites()
	{
		List<BuilderMicrosite> joins = BuilderMicrosite.BuilderMicrositeGetByBuilderID(BuilderEntity.BuilderID);
		foreach (BuilderMicrosite join in joins)
		{
			if (uxMicrosites.Items.FindByValue(join.CMMicrositeID.ToString()) != null)
				uxMicrosites.Items.FindByValue(join.CMMicrositeID.ToString()).Selected = true;
		}
	}

	/// <summary>
	/// Updates the list of selected categories in the BuilderMicrosite table.
	/// </summary>
	private void SaveMicrosites()
	{
		List<BuilderMicrosite> joins = BuilderMicrosite.BuilderMicrositeGetByBuilderID(BuilderEntity.BuilderID);
		foreach (ListItem li in uxMicrosites.Items)
		{
			BuilderMicrosite join = joins.Find(
				npc => npc.CMMicrositeID == Convert.ToInt32(li.Value));
			if (join != null)
			{
				if (!li.Selected)
					join.Delete();
			}
			else
			{
				if (li.Selected)
				{
					join = new BuilderMicrosite();
					join.CMMicrositeID = Convert.ToInt32(li.Value);
					join.BuilderID = BuilderEntity.BuilderID;
					join.Save();
				}
			}
		}
	}

	#endregion
}
