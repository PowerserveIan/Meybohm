using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Classes.Media352_NewsPress;

public partial class NewsPressPage : BasePage
{
	protected int m_PageSize;

	public int PageSize
	{
		get { return m_PageSize; }
		set { m_PageSize = value; }
	}

	protected bool? m_Archived
	{
		get
		{
			bool archived;
			if (!String.IsNullOrEmpty(Request.QueryString["archived"]) && Boolean.TryParse(Request.QueryString["archived"], out archived))
				return archived;
			return null;
		}
	}

	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "News Press";
		ComponentAdminPage = "media352-news-press/admin-news-press.aspx";
		Dictionary<string, string> acceptableValues = new Dictionary<string, string>();
		acceptableValues.Add("archived", "true");
		Dictionary<string, string> unacceptableValues = new Dictionary<string, string>();
		unacceptableValues.Add("Category", "all");
		unacceptableValues.Add(uxTopPager.QueryStringField, "1");
		CanonicalLink = BaseCode.Helpers.RootPath + "news-press.aspx" + BaseCode.Helpers.GetQueryStringWithAcceptableValues(unacceptableValues, acceptableValues);
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
		ObjectDataSource uxNewsListDataSource = new ObjectDataSource();
		uxNewsListDataSource.SelectMethod = "NewsPressPageByNewsPressCategoryID";
		uxNewsListDataSource.TypeName = "Classes.Media352_NewsPress.NewsPress";
		uxNewsListDataSource.EnablePaging = true;
		uxNewsListDataSource.SelectCountMethod = "SelectCount";
		Parameter searchTextParameter = new Parameter("searchText", DbType.String, "");
		searchTextParameter.ConvertEmptyStringToNull = false;
		uxNewsListDataSource.SelectParameters.Add(searchTextParameter);
		uxNewsListView.DataSource = uxNewsListDataSource;

		uxCategories.DataBound += uxCategories_DataBound;
		uxNewsListView.DataBound += uxNewsListView_DataBound;
		uxNewsListDataSource.Selecting += uxNewsListDataSource_Selecting;
		int result;
		if (Request.QueryString[uxTopPager.QueryStringField] == null || !Int32.TryParse(Request.QueryString[uxTopPager.QueryStringField], out result))
			uxTopPager.SetPageProperties(0, Settings.FrontEndPageSize, true);
		else
			uxTopPager.SetPageProperties((Convert.ToInt32(Request.QueryString[uxTopPager.QueryStringField]) - 1) * Settings.FrontEndPageSize, Settings.FrontEndPageSize, true);

		uxCategoryPlaceHolder.Visible = Settings.EnableCategories;

		if (!IsPostBack)
		{
			uxCategories.DataSource = NewsPressCategory.NewsPressCategoryGetByActive(true).OrderBy(c => c.Name).ToList();
			uxCategories.DataTextField = "Name";
			uxCategories.DataValueField = "NewsPressCategoryID";
			uxCategories.DataBind();

			uxRssFeed.NavigateUrl = "~/news-press-rss.aspx" + (!String.IsNullOrEmpty(Request.QueryString.ToString()) ? "?" + Request.QueryString : "");
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxNewsListView.DataBind();
	}

	private void uxNewsListDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
	{
		NewsPress.Filters filterList = new NewsPress.Filters();
		filterList.FilterNewsPressAutoArchived = m_Archived == null ? false.ToString() : m_Archived.ToString();
		if (Settings.EnableCategories && !String.IsNullOrEmpty(Request.QueryString["Category"]) && Request.QueryString["Category"] != "all")
			filterList.FilterNewsPressCategoryID = Request.QueryString["Category"];
		else
			filterList.FilterNewsPressCategoryID = null;

		e.InputParameters["sortField"] = "Date";
		e.InputParameters["sortDirection"] = false.ToString();

		filterList.FilterNewsPressActive = true.ToString();
		e.InputParameters["filterList"] = filterList;
	}

	private void uxNewsListView_DataBound(object sender, EventArgs e)
	{
		uxTopPager.Visible = uxBottomPager.Visible = (uxTopPager.PageSize < uxTopPager.TotalRowCount);
	}

	private void uxCategories_DataBound(object sender, EventArgs e)
	{
		foreach (ListItem l in uxCategories.Items)
		{
			l.Selected = l.Value == Request.QueryString["Category"];
		}
		//Give the links their paths here or else they will have Category=all everytime
		if (m_Archived.HasValue && m_Archived.Value)
		{
			uxNewsArchives.Visible = false;
			uxCurrentNews.Visible = true;
			uxCurrentNews.NavigateUrl = BaseCode.Helpers.GetCurrentMicrositePath() + "news-press?Category=" + uxCategories.SelectedItem.Value;
		}
		else
		{
			uxNewsArchives.Visible = true;
			uxCurrentNews.Visible = false;
			uxNewsArchives.NavigateUrl = BaseCode.Helpers.GetCurrentMicrositePath() + "news-press?archived=True&Category=" + uxCategories.SelectedItem.Value;
		}
		uxCurrentNewsLbl.Visible = !uxCurrentNews.Visible;
		uxNewsArchivesLbl.Visible = !uxNewsArchives.Visible;
	}
}