using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Classes.MLS;

public partial class builders : BasePage
{
	private const int m_PageSize = 5;

	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Builders";
		ComponentAdminPage = "m-l-s/admin-builder.aspx";
		NewHomePage = true;
	}

	protected string CurrentLetter
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["letter"]))
				return Request.QueryString["letter"].Substring(0, 1);
			return "";
		}
	}

	protected List<string> m_BuilderAlphabet;

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		ObjectDataSource uxDataSource = new ObjectDataSource();
		uxDataSource.SelectMethod = "BuilderPageForFrontend";
		uxDataSource.TypeName = "Classes.MLS.Builder";
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
			uxTopPager.SetPageProperties(0, m_PageSize, true);
		else
			uxTopPager.SetPageProperties((Convert.ToInt32(Request.QueryString[uxTopPager.QueryStringField]) - 1) * m_PageSize, m_PageSize, true);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			uxSearch.Text = Request.QueryString["searchText"];
			m_BuilderAlphabet = Builder.GetBuilderAlphabet(Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite().CMMicroSiteID);
			uxAlphabet.DataSource = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
			uxAlphabet.DataBind();

			Dictionary<string, string> unacceptableValues = new Dictionary<string, string>();
			unacceptableValues.Add(uxTopPager.QueryStringField, "1");
			unacceptableValues.Add("letter", "");
			unacceptableValues.Add("searchText", "");
			CanonicalLink = BaseCode.Helpers.RootPath + MicrositePath + "/builders" + BaseCode.Helpers.GetQueryStringWithAcceptableValues(unacceptableValues);
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxListView.DataBind();
	}

	private void uxDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
	{
		e.InputParameters["searchText"] = uxSearch.Text;
		e.InputParameters["letter"] = CurrentLetter;
		e.InputParameters["cmMicrositeID"] = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite().CMMicroSiteID;
	}

	private void uxListView_DataBound(object sender, EventArgs e)
	{
		uxTopPager.Visible = uxBottomPager.Visible = (uxTopPager.PageSize < uxTopPager.TotalRowCount);
	}

	protected string FixExternalLink(string link)
	{
		if (String.IsNullOrEmpty(link))
			return string.Empty;
		return (link.StartsWith("http") ? "" : "http://") + link;
	}
}