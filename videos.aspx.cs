using System;
using System.Data;
using System.Web.UI.WebControls;
using Classes.Videos;

public partial class VideoPage : BasePage
{
	protected override void SetCssAndJs()
	{
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Videos";
		ComponentAdminPage = "videos/admin-video.aspx";
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
		ObjectDataSource uxDataSource = new ObjectDataSource();
		uxDataSource.SelectMethod = "VideoPage";
		uxDataSource.TypeName = "Classes.Videos.Video";
		uxDataSource.EnablePaging = true;
		uxDataSource.SelectCountMethod = "SelectCount";
		Parameter searchTextParameter = new Parameter("searchText", DbType.String, "");
		searchTextParameter.ConvertEmptyStringToNull = false;
		uxDataSource.SelectParameters.Add(searchTextParameter);
		uxListView.DataSource = uxDataSource;

		uxListView.DataBound += uxListView_DataBound;
		uxDataSource.Selecting += uxDataSource_Selecting;

		int result;
		if (Request.QueryString[uxTopPager.QueryStringField] == null || !Int32.TryParse(Request.QueryString[uxTopPager.QueryStringField], out result))
			uxTopPager.SetPageProperties(0, Settings.FrontEndPageSize, true);
		else
			uxTopPager.SetPageProperties((Convert.ToInt32(Request.QueryString[uxTopPager.QueryStringField]) - 1) * Settings.FrontEndPageSize, Settings.FrontEndPageSize, true);
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxListView.DataBind();
	}

	private void uxDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
	{
		e.InputParameters["sortField"] = "DisplayOrder";
		e.InputParameters["sortDirection"] = true.ToString();
		Video.Filters filterList = new Video.Filters();
		filterList.FilterVideoActive = true.ToString();
		Classes.ContentManager.CMMicrosite currentMicrosite = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite();
		e.InputParameters["filterList"] = filterList;
	}

	private void uxListView_DataBound(object sender, EventArgs e)
	{
		uxTopPager.Visible = uxBottomPager.Visible = (uxTopPager.PageSize < uxTopPager.TotalRowCount);
	}
}