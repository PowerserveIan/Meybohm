using System;
using BaseCode;
using Classes.Videos;

public partial class Admin_AdminVideoEdit : BaseEditPage
{
	public Video VideoEntity { get; set; }
	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-video.aspx";
		m_ClassName = "Video";
		base.OnInit(e);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (EntityId > 0)
			{
				VideoEntity = Video.GetByID(EntityId);
				if (VideoEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
			}
			else
				NewRecord = true;
		}
	}

	protected override void Save()
	{
		if (IsValid)
		{
			VideoEntity = EntityId > 0 ? Video.GetByID(EntityId) : new Video();
			if (NewRecord)
			{
				VideoEntity.DateAdded = DateTime.UtcNow;
				VideoEntity.DisplayOrder = (short)(Helpers.GetMaxDisplayOrder("Video", "VideoID", null, null, null, null) + 1);
			}
			VideoEntity.Active = uxActive.Checked;
			VideoEntity.Featured = uxFeatured.Checked;
			VideoEntity.Title = uxTitle.Text;
			VideoEntity.Url = uxUrl.Text;
			VideoEntity.Save();

			EntityId = VideoEntity.VideoID;
			m_ClassTitle = VideoEntity.Title;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = VideoEntity.Active;
		uxFeatured.Checked = VideoEntity.Featured;
		uxTitle.Text = VideoEntity.Title;
		uxUrl.Text = VideoEntity.Url;
	}
}
