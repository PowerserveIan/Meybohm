using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.DynamicHeader;

public partial class Admin_AdminDynamicImageEdit : BaseEditPage
{
	public DynamicImage DynamicImageEntity { get; set; }

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-dynamic-image.aspx";
		m_ClassName = "Slide";
		base.OnInit(e);
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxThumbnailPlaceHolder.Visible = !String.IsNullOrEmpty(uxName.FileName);
		uxThumbnail.ImageUrl = Helpers.RootPath + Globals.Settings.UploadFolder + "images/" + uxName.FileName +"?width=70&height=70";
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			uxCollections.DataSource = DynamicCollection.GetAll().OrderBy(c => c.Name);
			uxCollections.DataTextField = "Name";
			uxCollections.DataValueField = "DynamicCollectionID";
			uxCollections.DataBind();

			uxCollectionsPH.Visible = uxCollections.Items.Count > 0;
			uxCaptionPH.Visible = Settings.EnableCaptions;

			if (EntityId > 0)
			{
				DynamicImageEntity = DynamicImage.GetByID(EntityId);
				if (DynamicImageEntity == null)
					Response.Redirect("~/admin/dynamic-header/admin-dynamic-image.aspx" + (!String.IsNullOrEmpty(ReturnQueryString) ? "?" + ReturnQueryString : ""));
				LoadData();
				uxCaption.HideEditorInitially = true;
			}
			else
				NewRecord = true;
		}
	}

	protected override void Save()
	{
		uxName.CommitChanges();
		if (IsValid)
		{
			DynamicImageEntity = EntityId > 0 ? DynamicImage.GetByID(EntityId) : new DynamicImage();
			DynamicImageEntity.Active = uxActive.Checked;
			DynamicImageEntity.Caption = uxCaption.EditorHTML;
			int cycleSpeed;
			if (Int32.TryParse(uxDuration.Text, out cycleSpeed))
				DynamicImageEntity.Duration = cycleSpeed;
			else
				DynamicImageEntity.Duration = null;
			DynamicImageEntity.IsVideo = uxIsVideo.Checked;
			DynamicImageEntity.LastUpdated = DateTime.UtcNow;
			DynamicImageEntity.Link = uxLink.Text;
			DynamicImageEntity.Name = uxName.FileName;
			DynamicImageEntity.Title = uxTitle.Text;
			DynamicImageEntity.Save();
			EntityId = DynamicImageEntity.DynamicImageID;

			SaveCategories();

			if (NewRecord)
				uxThumbnailPlaceHolder.Visible = true;

			uxCaption.HideEditorInitially = true;
			m_ClassTitle = DynamicImageEntity.Title;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = DynamicImageEntity.Active;
		uxCaption.EditorHTML = DynamicImageEntity.Caption;
		uxDuration.Text = DynamicImageEntity.Duration.ToString();
		uxIsVideo.Checked = DynamicImageEntity.IsVideo;
		uxLink.Text = DynamicImageEntity.Link;
		uxName.FileName = DynamicImageEntity.Name;
		uxTitle.Text = DynamicImageEntity.Title;		

		LoadCategories();
	}

	public void uxNameCV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = File.Exists(Server.MapPath("~/" + Globals.Settings.UploadFolder + "images/" + uxName.FileName));
	}

	protected void uxLinkToPageCustomVal_ServerValidate(object source, ServerValidateEventArgs args)
	{
		if (uxLink.Text.StartsWith("http://") || uxLink.Text.StartsWith("https://")) // || uxLinkToPage.Text.StartsWith("www.") || uxLinkToPage.Text.EndsWith(".com") || uxLinkToPage.Text.EndsWith(".net"))
			args.IsValid = true;
		else
		{
			try
			{
				FileInfo f = new FileInfo(Server.MapPath("~\\") + uxLink.Text.Split('?')[0]);
				args.IsValid = f.Exists;
			}
			catch (Exception)
			{
				args.IsValid = false;
			}
			if (!args.IsValid)
				args.IsValid = Helpers.DoesFilenameExist(uxLink.Text);
		}
	}

	#region Collection setup items

	private void LoadCategories()
	{
		List<DynamicImageCollection> joins = DynamicImageCollection.DynamicImageCollectionGetByDynamicImageID(DynamicImageEntity.DynamicImageID);
		foreach (DynamicImageCollection join in joins)
		{
			if (uxCollections.Items.FindByValue(join.DynamicCollectionID.ToString()) != null)
				uxCollections.Items.FindByValue(join.DynamicCollectionID.ToString()).Selected = true;
		}
	}

	/// <summary>
	/// Updates the list of selected categories in the DynamicImageCollection table.
	/// </summary>
	private void SaveCategories()
	{
		List<DynamicImageCollection> joins = DynamicImageCollection.DynamicImageCollectionGetByDynamicImageID(DynamicImageEntity.DynamicImageID);
		foreach (ListItem li in uxCollections.Items)
		{
			DynamicImageCollection join = joins.Find(
				npc => npc.DynamicCollectionID == Convert.ToInt32(li.Value));
			if (join != null)
			{
				if (!li.Selected)
					join.Delete();
			}
			else
			{
				if (li.Selected)
				{
					join = new DynamicImageCollection();
					join.DynamicCollectionID = Convert.ToInt32(li.Value);
					join.DynamicImageID = DynamicImageEntity.DynamicImageID;
					List<DynamicImageCollection> coll = DynamicImageCollection.DynamicImageCollectionGetByDynamicCollectionID(join.DynamicCollectionID);
					if (coll.Any())
						join.DisplayOrder = (short)(coll.Max(c => c.DisplayOrder) + 1);
					else
						join.DisplayOrder = 1;
					join.Save();
				}
			}
		}
	}

	#endregion
}