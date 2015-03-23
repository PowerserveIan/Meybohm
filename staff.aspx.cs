using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Classes.Media352_MembershipProvider;
using Classes.MLS;

public partial class staff : BasePage
{
	protected const int m_PageSize = 5;

	protected string CurrentLetter
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["letter"]))
				return Request.QueryString["letter"].Substring(0, 1);
			return "";
		}
	}

	protected List<string> m_StaffAlphabet;

	private string m_SearchQueryString;
	protected string SearchQueryString
	{
		get
		{
			if (String.IsNullOrEmpty(m_SearchQueryString))
			{
				Dictionary<string, string> unacceptableValues = new Dictionary<string, string>();
				unacceptableValues.Add(uxTopPager.QueryStringField, "1");
				unacceptableValues.Add("marketID", "");
				unacceptableValues.Add("letter", "");
				unacceptableValues.Add("languageID", "1");
				unacceptableValues.Add("searchText", "");
				m_SearchQueryString = BaseCode.Helpers.GetQueryStringWithAcceptableValues(unacceptableValues);
			}
			return m_SearchQueryString;
		}
	}

	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Staff";
		ComponentAdminPage = "media352-membership-provider/admin-user.aspx?FilterUserHasRole=true";
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		ObjectDataSource uxDataSource = new ObjectDataSource();
		uxDataSource.SelectMethod = "PageAgentsForStaffDirectory";
		uxDataSource.TypeName = "Classes.Media352_MembershipProvider.UserInfo";
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


	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			uxMarket.DataSource = Classes.ContentManager.CMMicrosite.CMMicrositePage(0, 0, "", "Name", true, new Classes.ContentManager.CMMicrosite.Filters { FilterCMMicrositeActive = true.ToString(), FilterCMMicrositePublished = true.ToString() });
			uxMarket.DataTextField = "Name";
			uxMarket.DataValueField = "CMMicrositeID";
			uxMarket.DataBind();

			uxLanguage.DataSource = Classes.SiteLanguages.Language.GetAll().OrderBy(l => l.Culture);
			uxLanguage.DataTextField = "Culture";
			uxLanguage.DataValueField = "LanguageID";
			uxLanguage.DataBind();

			uxStaffType.DataSource = StaffType.GetAll();
			uxStaffType.DataTextField = "Name";
			uxStaffType.DataValueField = "StaffTypeID";
			uxStaffType.DataBind();

			if (!String.IsNullOrEmpty(Request.QueryString["marketID"]) && uxMarket.Items.FindByValue(Request.QueryString["marketID"]) != null)
				uxMarket.Items.FindByValue(Request.QueryString["marketID"]).Selected = true;
			if (!String.IsNullOrEmpty(Request.QueryString["languageID"]) && uxLanguage.Items.FindByValue(Request.QueryString["languageID"]) != null)
				uxLanguage.Items.FindByValue(Request.QueryString["languageID"]).Selected = true;
			if (!String.IsNullOrEmpty(Request.QueryString["staffTypeID"]) && uxStaffType.Items.FindByValue(Request.QueryString["staffTypeID"]) != null)
				uxStaffType.Items.FindByValue(Request.QueryString["staffTypeID"]).Selected = true;
			
			m_StaffAlphabet = UserInfo.GetStaffAlphabet(!String.IsNullOrEmpty(uxMarket.SelectedValue) ? (int?)Convert.ToInt32(uxMarket.SelectedValue) : null);
			uxAlphabet.DataSource = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
			uxAlphabet.DataBind();

			uxName.Text = Request.QueryString["searchText"];

			CanonicalLink = BaseCode.Helpers.RootPath + MicrositePath + "/staff" + SearchQueryString;

			if (!String.IsNullOrEmpty(Request.QueryString.ToString()))
				BaseCode.Helpers.PageView.Anchor(Page, "alphabetPaging");
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		//if (!String.IsNullOrWhiteSpace(CurrentLetter) || !String.IsNullOrWhiteSpace(uxName.Text))
			uxListView.DataBind();
	}

	private void uxDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
	{
		e.InputParameters["searchText"] = uxName.Text;
		e.InputParameters["letter"] = CurrentLetter;
		UserInfo.Filters filterList = new UserInfo.Filters();
		if (!String.IsNullOrEmpty(uxMarket.SelectedValue))
			filterList.FilterMicrositeID = uxMarket.SelectedValue;
		if (!String.IsNullOrEmpty(uxLanguage.SelectedValue) && uxLanguage.SelectedItem.Text != "English")
			filterList.FilterLanguageID = uxLanguage.SelectedValue;
		if (!String.IsNullOrEmpty(uxStaffType.SelectedValue) && uxStaffType.SelectedValue != "-1")
			filterList.FilterUserInfoStaffTypeID = uxStaffType.SelectedValue;

		e.InputParameters["filterList"] = filterList;
	}

	private void uxListView_DataBound(object sender, EventArgs e)
	{
		uxTopPager.Visible = uxBottomPager.Visible = (uxTopPager.PageSize < uxTopPager.TotalRowCount);
	}

	protected string GetPreferredMicrosite(UserInfo userInfoEntity)
	{
		int micrositeID = userInfoEntity.PreferredCMMicrositeID.HasValue ? userInfoEntity.PreferredCMMicrositeID.Value : 0;
		if (micrositeID == 0)
		{
			UserOffice userOfficeEntity = UserOffice.UserOfficeGetByUserID(userInfoEntity.UserID, includeList: new string[] { "Office" }).FirstOrDefault();
			if (userOfficeEntity != null)
				micrositeID = userOfficeEntity.Office.CMMicrositeID;
		}
		if (micrositeID != 0)
			return micrositeID == 2 ? "augusta" : "aiken";
		return ((microsite)Master).CurrentMicrosite.Name.ToLower().Replace(" ", "-");
	}

	protected string GetPrimaryPhoneNumber(UserInfo userInfoEntity)
	{
		switch (userInfoEntity.PrimaryPhone){
			case "Cell Phone":
				return userInfoEntity.CellPhone;
			case "Office Phone":
				return userInfoEntity.OfficePhone;
			case "Fax":
				return userInfoEntity.Fax;
			case "Home Phone":
			default:
				return userInfoEntity.HomePhone;			
		}
		return userInfoEntity.HomePhone;
	}
}