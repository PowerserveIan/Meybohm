using System;
using BaseCode;
using Classes.Redirects;

public partial class Admin_AdminRedirectEdit : BaseEditPage
{
	public Redirect RedirectEntity { get; set; }
	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-redirect.aspx";
		m_ClassName = "301 Redirect";
		base.OnInit(e);
	} 

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (EntityId > 0)
			{
				RedirectEntity = Redirect.GetByID(EntityId);
				if (RedirectEntity == null) 
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
			RedirectEntity = EntityId > 0 ? Redirect.GetByID(EntityId) : new Redirect();
			RedirectEntity.NewUrl = Helpers.ReplaceAbsolutePathWithRoot(uxNewUrl.Text);
			RedirectEntity.OldUrl = Helpers.ReplaceAbsolutePathWithRoot(uxOldUrl.Text);
			RedirectEntity.Save();

			EntityId = RedirectEntity.RedirectID;
		}
	}

	protected override void LoadData()
	{
		uxNewUrl.Text = Helpers.ReplaceRootWithAbsolutePath(RedirectEntity.NewUrl);
		uxOldUrl.Text = Helpers.ReplaceRootWithAbsolutePath(RedirectEntity.OldUrl);
	}
}
