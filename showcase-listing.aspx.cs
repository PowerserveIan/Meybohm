using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.Showcase;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

public partial class ShowcaseListing : Page
{
	protected int m_ShowcaseID
	{
		get
		{
			int id;
			if (!String.IsNullOrEmpty(Request.QueryString["ShowcaseID"]) && Int32.TryParse(Request.QueryString["ShowcaseID"], out id))
				return id;
			return 0;
		}
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		ObjectDataSource uxShowcaseDataSource = new ObjectDataSource();
		uxShowcaseDataSource.SelectMethod = "ShowcaseItemPage";
		uxShowcaseDataSource.TypeName = "Classes.Showcase.ShowcaseItem";
		uxShowcaseDataSource.EnablePaging = true;
		uxShowcaseDataSource.SelectCountMethod = "SelectCount";
		Parameter searchTextParameter = new Parameter("searchText", DbType.String, "");
		searchTextParameter.ConvertEmptyStringToNull = false;
		uxShowcaseDataSource.SelectParameters.Add(searchTextParameter);
		uxShowcaseItemsListView.DataSource = uxShowcaseDataSource;

		uxShowcaseDataSource.Selecting += uxShowcaseDataSource_Selecting;
		int result;
		if (Request.QueryString[uxTopPager.QueryStringField] == null || !Int32.TryParse(Request.QueryString[uxTopPager.QueryStringField], out result))
			uxTopPager.SetPageProperties(0, Settings.FrontEndPageSize, true);
		else
			uxTopPager.SetPageProperties((Convert.ToInt32(Request.QueryString[uxTopPager.QueryStringField]) - 1) * Settings.FrontEndPageSize, Settings.FrontEndPageSize, true);
	}

	private void uxShowcaseDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
	{
		ShowcaseItem.Filters filterList = new ShowcaseItem.Filters();
		filterList.FilterShowcaseItemActive = true.ToString();
		filterList.FilterShowcaseItemShowcaseID = m_ShowcaseID.ToString();
		e.InputParameters["sortField"] = "DateListed";
		e.InputParameters["sortDirection"] = false.ToString();

		e.InputParameters["filterList"] = filterList;
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxShowcaseItemsListView.DataBind();

		Dictionary<string, string> unacceptableValues = new Dictionary<string, string>();
		unacceptableValues.Add("Page", "1");
		Dictionary<string, string> acceptableValues = new Dictionary<string, string>();
		acceptableValues.Add("Page", "");
		HtmlLink canon = new HtmlLink();
		canon.Attributes["rel"] = "canonical";
		canon.Href = BaseCode.Helpers.RootPath + "showcase-listing.aspx" + BaseCode.Helpers.GetQueryStringWithAcceptableValues(unacceptableValues, acceptableValues);
		Header.Controls.Add(canon);
	}
}