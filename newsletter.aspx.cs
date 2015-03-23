using System;
using System.Data;
using System.Web.UI.WebControls;
using Classes.Newsletters;

public partial class NewsletterPage : BasePage
{
	protected int? m_NewsletterID
	{
		get
		{
			int temp;
			if (!String.IsNullOrEmpty(Request.QueryString["id"]) && Int32.TryParse(Request.QueryString["id"], out temp) && Newsletter.GetByID(temp) != null)
				return temp;
			return null;
		}
	}

	protected bool? m_Subscribe
	{
		get
		{
			bool temp;
			if (!String.IsNullOrEmpty(Request.QueryString["subscribe"]) && Boolean.TryParse(Request.QueryString["subscribe"], out temp))
				return temp;
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
		ComponentName = "Newsletter";
		ComponentAdminPage = "newsletters/admin-newsletter.aspx";
		CanonicalLink = BaseCode.Helpers.RootPath + "newsletter.aspx" + (!String.IsNullOrEmpty(Request.QueryString[uxTopPager.QueryStringField]) && Request.QueryString[uxTopPager.QueryStringField] != "1" ? "?" + uxTopPager.QueryStringField + "=" + Request.QueryString[uxTopPager.QueryStringField] : "");
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
		ObjectDataSource uxNewsletterDataSource = new ObjectDataSource();
		uxNewsletterDataSource.SelectMethod = "NewsletterPage";
		uxNewsletterDataSource.TypeName = "Classes.Newsletters.Newsletter";
		uxNewsletterDataSource.EnablePaging = true;
		uxNewsletterDataSource.SelectCountMethod = "SelectCount";
		Parameter searchTextParameter = new Parameter("searchText", DbType.String, "");
		searchTextParameter.ConvertEmptyStringToNull = false;
		uxNewsletterDataSource.SelectParameters.Add(searchTextParameter);
		uxNewsListView.DataSource = uxNewsletterDataSource;

		uxNewsListView.DataBound += uxNewsListView_DataBound;
		uxNewsletterDataSource.Selecting += uxNewsletterDataSource_Selecting;

		int result;
		if (Request.QueryString[uxTopPager.QueryStringField] == null || !Int32.TryParse(Request.QueryString[uxTopPager.QueryStringField], out result))
			uxTopPager.SetPageProperties(0, Settings.FrontEndPageSize, true);
		else
			uxTopPager.SetPageProperties((Convert.ToInt32(Request.QueryString[uxTopPager.QueryStringField]) - 1) * Settings.FrontEndPageSize, Settings.FrontEndPageSize, true);
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxNewsListView.DataBind();
	}

	private void uxNewsletterDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
	{
		e.InputParameters["sortField"] = "DisplayDate";
		e.InputParameters["sortDirection"] = false.ToString();
		Newsletter.Filters filterList = new Newsletter.Filters();
		filterList.FilterNewsletterActive = true.ToString();
		filterList.FilterNewsletterDeleted = false.ToString();
		Classes.ContentManager.CMMicrosite currentMicrosite = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite();
		if (currentMicrosite != null)
		{
			filterList.FilterNewsletterCMMicrositeID = currentMicrosite.CMMicroSiteID.ToString();
			filterList.FilterNewsletterNewHomes = ((microsite)Master).NewHomes.ToString();
		}
		else
			filterList.FilterNewsletterCMMicrositeID = "";		
		e.InputParameters["filterList"] = filterList;
	}

	private void uxNewsListView_DataBound(object sender, EventArgs e)
	{
		uxTopPager.Visible = uxBottomPager.Visible = (uxTopPager.PageSize < uxTopPager.TotalRowCount);
	}
}