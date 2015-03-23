using System;
using BaseCode;
using Classes.Showcase;

public partial class Admin_AdminMediaEdit : BaseEditPage
{
	protected string embeddedMarkup = "<embed src=\"[LINK]" + (Settings.AutoplayVideos ? "&autoplay=1" : "") + "\" type=\"application/x-shockwave-flash\" allowscriptaccess=\"always\" allowfullscreen=\"true\" wmode=\"transparent\" width=\"425\" height=\"275\"></embed>";

	protected int MediaCollectionID
	{
		get
		{
			int tempID;
			if (Request.QueryString["FilterMediaShowcaseMediaCollectionID"] != null)
				if (Int32.TryParse(Request.QueryString["FilterMediaShowcaseMediaCollectionID"], out tempID))
					return tempID;

			return 0;
		}
	}

	public Media MediaEntity { get; set; }

	public MediaCollection MediaCollectionEntity { get; set; }

	protected MediaTypes CurrentType
	{
		get
		{
			if (ViewState["currentType"] == null)
				ViewState["currentType"] = EnumParser.Parse<MediaTypes>(MediaCollectionEntity.ShowcaseMediaTypeID.ToString());
			return EnumParser.Parse<MediaTypes>(ViewState["currentType"].ToString());
		}
	}

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-media.aspx";
		m_ClassName = "Media";
		m_CustomBreadCrumbsPH = uxCustomBreadCrumbsPH;
		base.OnInit(e);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		ShowcaseItem itemEntity = null;
		if (!IsPostBack)
		{
			if (MediaCollectionID <= 0)
				Response.Redirect(m_LinkToListingPage + ReturnQueryString);

			if (EntityId == 0)
				MediaCollectionEntity = MediaCollection.GetByID(MediaCollectionID);
			else
			{
				MediaEntity = Media.GetByID(EntityId);
				if (MediaEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				MediaCollectionEntity = MediaCollection.GetByID(MediaEntity.ShowcaseMediaCollectionID);
			}
			if (MediaCollectionEntity == null)
				Response.Redirect(m_LinkToListingPage + ReturnQueryString);

			itemEntity = ShowcaseItem.GetByID(MediaCollectionEntity.ShowcaseItemID);

            var showcaseItemFinePropertyInformation = ShowcaseItemFinePropertyInformation.Get(MediaCollectionEntity.ShowcaseItemID);
            if (showcaseItemFinePropertyInformation.IsFineFeatured)
            {
                spnFeaturedDisplay.Style["display"] = "inline";
            }
            else
            {
                spnNormalDisplay.Style["display"] = "inline";
            }

			if (itemEntity == null || (!ShowcaseHelpers.IsShowcaseAdmin() && !ShowcaseUser.ShowcaseUserGetByShowcaseID(itemEntity.ShowcaseID).Exists(s => s.UserID == Helpers.GetCurrentUserID())))
				Response.Redirect(m_LinkToListingPage + ReturnQueryString);

			if (CurrentType == MediaTypes.Video || CurrentType == MediaTypes.VideoAndText)
			{
				uxImagePlaceHolder.Visible = false;
				uxVideoPlaceHolder.Visible = true;
				uxPreviewPlaceHolder.Visible = true;
			}
			else if (CurrentType == MediaTypes.Image || CurrentType == MediaTypes.ImageAndText)
			{
				uxImagePlaceHolder.Visible = true;
				uxVideoPlaceHolder.Visible = false;
				uxPreviewPlaceHolder.Visible = false;
			}
			else
				Response.Redirect(m_LinkToListingPage + ReturnQueryString);

			if (EntityId > 0)
				LoadData();
			else
			{
				NewRecord = true;
				uxPreviewPlaceHolder.Visible = false;
				if (CurrentType == MediaTypes.Image || CurrentType == MediaTypes.ImageAndText)
					uxThumbnailPlaceholder.Visible = false;
			}
		}
		if (MediaCollectionEntity == null)
			MediaCollectionEntity = MediaCollection.GetByID(MediaCollectionID);
		if (itemEntity == null)
			itemEntity = ShowcaseItem.GetByID(MediaCollectionEntity.ShowcaseItemID);

		uxLinkToMediaCollectionManager.NavigateUrl = "admin-media-collection.aspx?FilterMediaCollectionShowcaseItemID=" + MediaCollectionEntity.ShowcaseItemID;
		uxLinkToMediaCollectionManager.Text = @"<b>" + itemEntity.Title + @"</b> Media Collection Manager";

		uxLinkToMediaManager.NavigateUrl = m_LinkToListingPage + ReturnQueryString;
		uxLinkToMediaManager.Text = @"<b>" + MediaCollection.GetByID(MediaCollectionID).Title + @"</b> Media Manager";
		if (Settings.MultipleShowcases && ShowcaseHelpers.UserCanManageOtherShowcases())
			uxShowcaseName.Text = @"<b>" + Showcases.GetByID(itemEntity.ShowcaseID).Title + @"</b>";
		else
			uxShowcaseName.Text = @"Showcase";
	}

	protected override void Save()
	{
		uxImage.CommitChanges();
        if (IsValid)
		{
            //If there is a list of images, then add each one, else run through normal execution.
            if (uxImage.ListFileName.Count > 1)
            {
                for (int index = 0; index < uxImage.ListFileName.Count; index++)
                {
                    MediaEntity = new Media();
                    MediaEntity.Active = uxActive.Checked;
                    MediaEntity.Caption = uxImage.ListFileCaption[index];

                    if (NewRecord)
                        MediaEntity.DisplayOrder = (short)(Helpers.GetMaxDisplayOrder("ShowcaseMedia", "ShowcaseMediaID", "ShowcaseMediaCollectionID", MediaCollectionID) + 1);
                    
                    MediaEntity.ShowcaseMediaCollectionID = MediaCollectionID;
                    MediaEntity.URL = uxImage.ListFileName[index];

                    MediaEntity.Save();
                    EntityId = MediaEntity.ShowcaseMediaID;

                    if (NewRecord)
                        uxThumbnailPlaceholder.Visible = true;
                    m_ClassTitle = MediaEntity.Caption;
                }
            }
            else
            {
                MediaEntity = EntityId > 0 ? Media.GetByID(EntityId) : new Media();
                MediaEntity.Active = uxActive.Checked;
                MediaEntity.Caption = String.IsNullOrEmpty(uxImage.CaptionText) ? uxCaption.Text : uxImage.CaptionText;
                if (NewRecord)
                    MediaEntity.DisplayOrder = (short)(Helpers.GetMaxDisplayOrder("ShowcaseMedia", "ShowcaseMediaID", "ShowcaseMediaCollectionID", MediaCollectionID) + 1);
                MediaEntity.ShowcaseMediaCollectionID = MediaCollectionID;

                if (CurrentType == MediaTypes.Video || CurrentType == MediaTypes.VideoAndText)
                {
                    MediaEntity.URL = FormatYouTubeURL(uxURL.Text);
                    uxURL.Text = MediaEntity.URL;
                    MediaEntity.Thumbnail = GetYouTubeID(uxURL.Text);
                    uxPreviewLiteral.Text = embeddedMarkup.Replace("[LINK]", MediaEntity.URL);
                    uxPreviewPlaceHolder.Visible = true;
                }
                else
                    MediaEntity.URL = uxImage.FileName;

                MediaEntity.Save();
                EntityId = MediaEntity.ShowcaseMediaID;
                if (NewRecord)
                    uxThumbnailPlaceholder.Visible = true;
                m_ClassTitle = MediaEntity.Caption;
            }
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = MediaEntity.Active;
		uxCaption.Text = MediaEntity.Caption;
		uxURL.Text = MediaEntity.URL;
		uxImage.FileName = MediaEntity.URL;
		uxPreviewLiteral.Text = embeddedMarkup.Replace("[LINK]", MediaEntity.URL);
	}

	private static string FormatYouTubeURL(string url)
	{
		return "http://www.youtube.com/v/" + GetYouTubeID(url) + "&hl=en&fs=1";
	}

	private static string GetYouTubeID(string url)
	{
		string id;
		if (url.Contains("http://www.youtube.com/watch?v="))
			id = url.Substring(url.IndexOf("/watch?v=") + "/watch?v=".Length);
		else
			id = url.Substring(url.IndexOf("/v/") + "/v/".Length);
		return id.Split('&')[0].Split('\'')[0].Split('"')[0];
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxThumbnailImage.ImageUrl = CurrentType == MediaTypes.Image || CurrentType == MediaTypes.ImageAndText ? Helpers.RootPath + (uxImage.FileName.ToLower().StartsWith("http") ? "resizer.aspx?filename=" : Globals.Settings.UploadFolder + "images/") + uxImage.FileName + (uxImage.FileName.ToLower().StartsWith("http") ? "&" : "?") + "width=96&height=60" : "http://img.youtube.com/vi/" + MediaEntity.Thumbnail + "/2.jpg";
	}
}