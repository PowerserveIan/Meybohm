using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using BaseCode;
using Classes.Media352_NewsPress;

public partial class NewsPressDetail : BasePage
{
	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "News Press";
		ComponentAdminPage = "media352-news-press/admin-news-press.aspx";
		ComponentAdditionalLink = "~/admin/media352-news-press/admin-news-press-edit.aspx?id=" + Request.QueryString["Id"] + "&frontendView=true";
	}

	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		if (Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite() != null)
			MasterPageFile = "~/microsite.master";
	}	

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		int id;
		bool itemFound = false;

		NameValueCollection myQueryString = Request.QueryString.Duplicate();
		myQueryString.Remove("id");
		myQueryString.Remove("title");
		string queryString = Helpers.WriteQueryString(myQueryString, new System.Text.StringBuilder());
		uxBackButton.NavigateUrl = uxBackButtonTop.NavigateUrl = Helpers.GetCurrentMicrositePath() + "news-press" + queryString;

		if (int.TryParse(Request.QueryString["Id"], out id))
		{
			NewsPress newsPressEntity = NewsPress.GetByID(id);
			if (newsPressEntity != null && newsPressEntity.Active)
			{
				itemFound = true;
				uxTitle.Text = Server.HtmlEncode(newsPressEntity.Title);
				uxDate.Text = @"Posted " + String.Format("{0:d}", newsPressEntity.DateClientTime);
				if (!String.IsNullOrEmpty(newsPressEntity.Author))
					uxAuthor.Text = @" by " + Server.HtmlEncode(newsPressEntity.Author);
				uxStoryHTML.Text = Helpers.ReplaceRootWithRelativePath(newsPressEntity.StoryHTML, 0);

				#region NextPreviousPopups
				NewsPress.Filters filterList = new NewsPress.Filters();
				filterList.FilterNewsPressAutoArchived = String.IsNullOrEmpty(Request.QueryString["archived"]) ? false.ToString() : Request.QueryString["archived"];
				int tempCategoryID;
				if (Settings.EnableCategories && Request.QueryString["Category"] != null && Request.QueryString["Category"] != "all" && Int32.TryParse(Request.QueryString["Category"], out tempCategoryID))
					filterList.FilterNewsPressCategoryID = Request.QueryString["Category"];
				else
					filterList.FilterNewsPressCategoryID = null;

				filterList.FilterNewsPressActive = true.ToString();
				List<NewsPress> allArticles = NewsPress.NewsPressPageByNewsPressCategoryID(0, 0, "", "Date", false, filterList);
				//Find current article to get position
				int currentIndex = allArticles.FindIndex(n => n.NewsPressID == newsPressEntity.NewsPressID);
				if (currentIndex > 0)
				{
					NewsPress previousArticle = allArticles[currentIndex - 1];
					uxPreviousLink.NavigateUrl = uxPreviousReadMoreLink.NavigateUrl = Helpers.GetCurrentMicrositePath() + "news-press-details.aspx?id=" + previousArticle.NewsPressID + "&title=" + Server.UrlEncode(previousArticle.Title) + (String.IsNullOrEmpty(queryString.TrimStart('?')) ? "" : "&" + queryString.TrimStart('?'));
					uxPreviousLink.Text = Server.HtmlEncode(previousArticle.Title);
				}
				else
					upPreviousWrapper.Visible = false;

				if (currentIndex < allArticles.Count - 1)
				{
					NewsPress nextArticle = allArticles[currentIndex + 1];
					uxNextLink.NavigateUrl = uxNextReadMoreLink.NavigateUrl = Helpers.GetCurrentMicrositePath() + "news-press-details.aspx?id=" + nextArticle.NewsPressID + "&title=" + Server.UrlEncode(nextArticle.Title) + (String.IsNullOrEmpty(queryString.TrimStart('?')) ? "" : "&" + queryString.TrimStart('?'));
					uxNextLink.Text = Server.HtmlEncode(nextArticle.Title);
				}
				else
					upNextWrapper.Visible = false;

				NewsPressCategory categoryEntity = null;
				int temp;
				if (!String.IsNullOrEmpty(Request.QueryString["Category"]) && Int32.TryParse(Request.QueryString["Category"], out temp))
					categoryEntity = NewsPressCategory.GetByID(temp);
				uxPreviousCategoryTitle.Text = uxNextCategoryTitle.Text = categoryEntity == null ? "All Articles" : categoryEntity.Name;
				uxPreviousNumberArticles.Text = uxNextNumberArticles.Text = @"(1 of " + allArticles.Count + @" articles)";
				#endregion

				if (Globals.Settings.FacebookEnableLikeButton)
					FacebookLike.AddMetaData(Page, newsPressEntity.Title, FacebookLike.FBType.Article, Request.Url.ToString(),
											 "", Globals.Settings.CompanyName, newsPressEntity.Summary);
				CanonicalLink = Helpers.RootPath + "news-press-details.aspx?id=" + newsPressEntity.NewsPressID + "&title=" + Server.UrlEncode(newsPressEntity.Title);
			}
		}
		if (!itemFound)
		{
			uxNoItemFoundMsg.Visible = true;
			uxItemFoundPH.Visible =
			uxNextPreviousPH.Visible = false;
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		if (String.IsNullOrEmpty(Title) || Title.Equals("Default Title", StringComparison.OrdinalIgnoreCase))
			Title = @"Article Details";
	}
}