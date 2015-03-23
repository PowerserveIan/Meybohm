using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Media352_NewsPress;

public partial class Admin_AdminNewsPressEdit : BaseEditPage
{
	public NewsPress NewsPressEntity { get; set; }

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-news-press.aspx";
		m_ClassName = "News Press Article";
		base.OnInit(e);
		uxTitleUniqueValidator.ServerValidate += uxTitleUniqueValidator_ServerValidate;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			uxCategoryPlaceHolder.Visible = Settings.EnableCategories;

			if (Settings.EnableCategories)
			{
				List<NewsPressCategory> activeCategories = NewsPressCategory.NewsPressCategoryGetByActive(true);
				uxCategory.DataSource = activeCategories;
				uxCategory.DataTextField = "Name";
				uxCategory.DataValueField = "NewsPressCategoryId";
				uxCategory.DataBind();

				uxCategoriesExistPlaceHolder.Visible = activeCategories.Count > 0;
				uxCategoriesDontExistPlaceHolder.Visible = activeCategories.Count <= 0;
			}

			uxArchivePH.Visible = Settings.ArchiveType == ArchiveTypes.ManualArchiving;

			if (EntityId > 0)
			{
				NewsPressEntity = NewsPress.GetByID(EntityId);
				if (NewsPressEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
				uxStoryHTML.HideEditorInitially = true;
			}
			else
				NewRecord = true;

			//SEO code
			if (EntityId > 0)
			{
				uxSEOData.PageLinkFormatterElements.Add(Convert.ToString(NewsPressEntity.NewsPressID));
				uxSEOData.LoadControlData();
			}
			else
				uxSEOData.LoadControlData(true);
		}
	}

	protected override void Save()
	{
		if (IsValid)
		{
			NewsPressEntity = EntityId > 0 ? NewsPress.GetByID(EntityId) : new NewsPress();
			NewsPressEntity.Archived = uxArchived.Checked;
			NewsPressEntity.Author = uxAuthor.Text;
			NewsPressEntity.Date = uxDate.SelectedDate.Value;
			NewsPressEntity.Featured = uxFeatured.Checked;
			NewsPressEntity.Active = uxActive.Checked;
			NewsPressEntity.StoryHTML = uxStoryHTML.EditorHTML;
			NewsPressEntity.Summary = uxSummary.Text;
			NewsPressEntity.Title = uxTitle.Text;
			NewsPressEntity.Save();
			EntityId = NewsPressEntity.NewsPressID;

			if (Settings.EnableCategories)
				SaveCategories();

			//SEO saving should not be done until the new product has been created
			if (NewsPressEntity.NewsPressID > 0)
			{
				uxSEOData.PageLinkFormatterElements.Clear();
				uxSEOData.PageLinkFormatterElements.Add(Convert.ToString(NewsPressEntity.NewsPressID));
				if (String.IsNullOrEmpty(uxSEOData.Title))
					uxSEOData.Title = uxTitle.Text;
				if (String.IsNullOrEmpty(uxSEOData.FriendlyFilename))
					uxSEOData.FriendlyFilename = Helpers.StripNonAlphaCharacters(Helpers.ForceShorten(uxTitle.Text, 45).TrimEnd('.')).ToLower().Replace(" ", "-");
				uxSEOData.SaveControlData();
			}

			uxStoryHTML.HideEditorInitially = true;
			m_ClassTitle = NewsPressEntity.Title;
		}
	}

	protected override void LoadData()
	{
		uxArchived.Checked = NewsPressEntity.Archived;
		uxAuthor.Text = NewsPressEntity.Author;
		uxDate.SelectedDate = NewsPressEntity.Date;
		uxFeatured.Checked = NewsPressEntity.Featured;
		uxActive.Checked = NewsPressEntity.Active;
		uxStoryHTML.EditorHTML = NewsPressEntity.StoryHTML;
		uxSummary.Text = NewsPressEntity.Summary;
		uxTitle.Text = NewsPressEntity.Title;

		LoadCategories();
	}

	private void uxTitleUniqueValidator_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !NewsPress.NewsPressGetByTitle(uxTitle.Text).Any(t => EntityId != t.NewsPressID);
	}

	#region Category setup items

	private void LoadCategories()
	{
		List<NewsPressNewsPressCategory> joins = NewsPressNewsPressCategory.NewsPressNewsPressCategoryGetByNewsPressID(NewsPressEntity.NewsPressID);
		foreach (NewsPressNewsPressCategory join in joins)
		{
			if (uxCategory.Items.FindByValue(join.NewsPressCategoryID.ToString()) != null)
				uxCategory.Items.FindByValue(join.NewsPressCategoryID.ToString()).Selected = true;
		}
	}

	/// <summary>
	/// Updates the list of selected categories in the NewsPressNewsPressCategory table.
	/// </summary>
	private void SaveCategories()
	{
		List<NewsPressNewsPressCategory> joins = NewsPressNewsPressCategory.NewsPressNewsPressCategoryGetByNewsPressID(NewsPressEntity.NewsPressID);
		foreach (ListItem li in uxCategory.Items)
		{
			NewsPressNewsPressCategory join = joins.Find(
				npc => npc.NewsPressCategoryID == Convert.ToInt32(li.Value));
			if (join != null)
			{
				if (!li.Selected)
					join.Delete();
			}
			else
			{
				if (li.Selected)
				{
					join = new NewsPressNewsPressCategory();
					join.NewsPressCategoryID = Convert.ToInt32(li.Value);
					join.NewsPressID = NewsPressEntity.NewsPressID;
					join.Save();
				}
			}
		}
	}

	protected void uxCategory_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = uxCategory.SelectedItem != null;
	}

	#endregion
}