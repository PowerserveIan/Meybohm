using System;
using System.Linq;
using System.Web.UI.WebControls;
using Classes.Media352_NewsPress;

public partial class Admin_AdminNewsPressCategoryEdit : BaseEditPage
{
	public NewsPressCategory NewsPressCategoryEntity { get; set; }

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-news-press-category.aspx";
		m_ClassName = "News Press Category";
		base.OnInit(e);
		uxNameUniqueValidator.ServerValidate += uxNameUniqueValidator_ServerValidate;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (EntityId > 0)
			{
				NewsPressCategoryEntity = NewsPressCategory.GetByID(EntityId);
				if (NewsPressCategoryEntity == null)
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
			NewsPressCategoryEntity = EntityId > 0 ? NewsPressCategory.GetByID(EntityId) : new NewsPressCategory();
			NewsPressCategoryEntity.Active = uxActive.Checked;
			NewsPressCategoryEntity.Name = uxName.Text;
			NewsPressCategoryEntity.Save();
			EntityId = NewsPressCategoryEntity.NewsPressCategoryID;

			BaseCode.Helpers.PurgeCacheItems("Media352_NewsPress");

			m_ClassTitle = NewsPressCategoryEntity.Name;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = NewsPressCategoryEntity.Active;
		uxName.Text = NewsPressCategoryEntity.Name;
	}

	private void uxNameUniqueValidator_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !NewsPressCategory.NewsPressCategoryGetByName(uxName.Text).Any(t => EntityId != t.NewsPressCategoryID);
	}
}