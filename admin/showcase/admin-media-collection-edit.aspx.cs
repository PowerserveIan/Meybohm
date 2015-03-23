using System;
using System.Web;
using BaseCode;
using Classes.Showcase;

public partial class Admin_AdminMediaCollectionEdit : BaseEditPage
{
	protected int ShowcaseItemId
	{
		get
		{
			int tempID;
			if (Request.QueryString["FilterMediaCollectionShowcaseItemID"] != null)
				if (Int32.TryParse(Request.QueryString["FilterMediaCollectionShowcaseItemID"], out tempID))
					return tempID;

			return 0;
		}
	}

	protected bool IsFineProperty
    {
        get
        {
            int tempID;
            if (Request.QueryString["isFineProperty"] != null)
            {
                if (Int32.TryParse(Request.QueryString["isFineProperty"], out tempID))
                {
                    return true;
                }
            }

            return false;
        }
    }

	public MediaCollection MediaCollectionEntity { get; set; }

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-media-collection.aspx";
		m_ClassName = "Media Collection";
		m_CustomBreadCrumbsPH = uxCustomBreadCrumbsPH;
		base.OnInit(e);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		ShowcaseItem itemEntity = null;
		if (!IsPostBack)
		{
			if (ShowcaseItemId <= 0 || ShowcaseItem.GetByID(ShowcaseItemId) == null)
				Response.Redirect(m_LinkToListingPage + ReturnQueryString);

			uxShowcaseMediaTypeID.DataSource = MediaType.GetAll();
			uxShowcaseMediaTypeID.DataTextField = "Type";
			uxShowcaseMediaTypeID.DataValueField = "ShowcaseMediaTypeID";
			uxShowcaseMediaTypeID.DataBind();

			if (EntityId <= 0 && this.IsFineProperty)
            {
                // Get the Entity Id of the MediaCollectionEntity for a Fine Property.
                MediaCollectionEntity = MediaCollection.GetByIDAndFineStatus(ShowcaseItemId, this.IsFineProperty);
                EntityId = MediaCollectionEntity == null ? 0 : MediaCollectionEntity.ShowcaseMediaCollectionID;

                uxActive.Visible = false;
            }

			if (EntityId > 0)
			{
				MediaCollectionEntity = MediaCollection.GetByID(EntityId);
				if (MediaCollectionEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);

				itemEntity = ShowcaseItem.GetByID(MediaCollectionEntity.ShowcaseItemID);
				if (itemEntity == null || (!ShowcaseHelpers.IsShowcaseAdmin() && !ShowcaseUser.ShowcaseUserGetByShowcaseID(itemEntity.ShowcaseID).Exists(s => s.UserID == Helpers.GetCurrentUserID())))
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);

				LoadData();
				uxMediaPlaceHolder.Visible = true;
				uxEditMedia.NavigateUrl = "~/admin/showcase/admin-media.aspx?FilterMediaCollectionShowcaseItemID=" + ShowcaseItemId + "&FilterMediaShowcaseMediaCollectionID=" + MediaCollectionEntity.ShowcaseMediaCollectionID + "&isFineProperty=" + (MediaCollectionEntity.IsFine ? "1" : "0");
                
				MediaTypes currentType = EnumParser.Parse<MediaTypes>(MediaCollectionEntity.ShowcaseMediaTypeID.ToString());
				if (currentType == MediaTypes.Video || currentType == MediaTypes.VideoAndText)
					uxEditMedia.Text = @"<span>Edit the videos for this collection</span>";
				else if (currentType == MediaTypes.Image || currentType == MediaTypes.ImageAndText)
					uxEditMedia.Text = @"<span>Edit the images for this collection</span>";
				else
					uxMediaPlaceHolder.Visible = false;
			}
			else
				NewRecord = true;
		}
		uxLinkToMediaCollectionManager.NavigateUrl = m_LinkToListingPage + ReturnQueryString;
		if (itemEntity == null)
			itemEntity = ShowcaseItem.GetByID(ShowcaseItemId);
		uxLinkToMediaCollectionManager.Text = @"<b>" + itemEntity.Title + @"</b> Media Collection Manager";
		if (Settings.MultipleShowcases && ShowcaseHelpers.UserCanManageOtherShowcases())
			uxShowcaseName.Text = @"<b>" + Showcases.GetByID(itemEntity.ShowcaseID).Title + @"</b>";
		else
			uxShowcaseName.Text = @"Showcase";
			
		if (this.IsFineProperty)
        {
            m_CancelButton.PostBackUrl = "~/admin/showcase/admin-showcase-item-statusEdit.aspx?id=" + ShowcaseItemId;
            m_CancelButton.Text = "return to " + (Settings.MultipleShowcases && ShowcaseHelpers.UserCanManageOtherShowcases() ? Showcases.GetByID(ShowcaseHelpers.GetCurrentShowcaseID().Value).Title + " " : "") + " prop. status";
        }
	}

	protected override void Save()
	{
		if (IsValid)
		{
			MediaCollectionEntity = EntityId > 0 ? MediaCollection.GetByID(EntityId) : new MediaCollection();
			if (NewRecord)
			{
				MediaCollectionEntity.DisplayOrder = (short)(Helpers.GetMaxDisplayOrder("ShowcaseMediaCollection", "ShowcaseMediaCollectionID", "ShowcaseItemID", ShowcaseItemId) + 1);
				MediaCollectionEntity.ShowcaseItemID = ShowcaseItemId;
				uxMediaPlaceHolder.Visible = true;
			}
			MediaCollectionEntity.Active = uxActive.Checked;
			MediaCollectionEntity.ShowcaseMediaTypeID = Convert.ToInt32(uxShowcaseMediaTypeID.SelectedValue);
			MediaCollectionEntity.TextBlock = uxTextBlock.EditorHTML;
			MediaCollectionEntity.Title = uxTitle.Text;
			MediaCollectionEntity.IsFine = this.IsFineProperty || MediaCollectionEntity.IsFine;
			MediaCollectionEntity.Save();
			EntityId = MediaCollectionEntity.ShowcaseMediaCollectionID;

			if (NewRecord)
				                uxEditMedia.NavigateUrl = "~/admin/showcase/admin-media.aspx?FilterMediaCollectionShowcaseItemID=" + ShowcaseItemId + "&FilterMediaShowcaseMediaCollectionID=" + MediaCollectionEntity.ShowcaseMediaCollectionID + "&isFineProperty=" + Request.QueryString["isFineProperty"];
			MediaTypes currentType = EnumParser.Parse<MediaTypes>(MediaCollectionEntity.ShowcaseMediaTypeID.ToString());
			if (currentType == MediaTypes.Video || currentType == MediaTypes.VideoAndText)
			{
				uxEditMedia.Text = @"<span>Edit the videos for this collection</span>";
				uxMediaPlaceHolder.Visible = true;
			}
			else if (currentType == MediaTypes.Image || currentType == MediaTypes.ImageAndText)
			{
				uxEditMedia.Text = @"<span>Edit the images for this collection</span>";
				uxMediaPlaceHolder.Visible = true;
			}
			else
				uxMediaPlaceHolder.Visible = false;
			m_ClassTitle = MediaCollectionEntity.Title;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = MediaCollectionEntity.Active;
		if (uxShowcaseMediaTypeID.Items.FindByValue(MediaCollectionEntity.ShowcaseMediaTypeID.ToString()) != null)
			uxShowcaseMediaTypeID.SelectedValue = MediaCollectionEntity.ShowcaseMediaTypeID.ToString();
		uxTextBlock.EditorHTML = MediaCollectionEntity.TextBlock;
		uxTitle.Text = MediaCollectionEntity.Title;
	}
}