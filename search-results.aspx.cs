using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Showcase;

public partial class search_results : BasePage
{
	protected int? m_ShowcaseItemID
	{
		get
		{
			int temp;
			if (!String.IsNullOrEmpty(Request.QueryString["id"]) && Int32.TryParse(Request.QueryString["id"], out temp))
			{
				ShowcaseItem itemEntity = ShowcaseItem.GetByID(temp);
				if (itemEntity != null && itemEntity.Active)
					return temp;
			}
			return null;
		}
	}

	protected int m_ShowcaseID
	{
		get
		{
			if (ViewState["ShowcaseID"] == null)
				ViewState["ShowcaseID"] = ShowcaseHelpers.GetCurrentShowcaseID();
			return Convert.ToInt32(ViewState["ShowcaseID"]);
		}
	}

	protected int PageSize
	{
		get
		{
			int temp;
			if (!String.IsNullOrEmpty(Request.QueryString["PageSize"]) && Int32.TryParse(Request.QueryString["PageSize"], out temp))
				return temp;
			return 15;
		}
	}

	protected string SearchText
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["q"]))
				return Server.UrlDecode(Request.QueryString["q"]);
			return "";
		}
	}


	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
		//m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Showcase";
		ComponentAdminPage = "showcase/admin-showcase-item.aspx";
		CanonicalLink = Helpers.RootPath + "search-results.aspx?q=" + SearchText;
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			bool showTopForAiken = ((microsite)Master).IsAiken;
			bool showBottomForAiken = ((microsite)Master).IsAugusta;
			uxHomesTop.BindData("Homes in ", GetHomesShowcaseID(showTopForAiken), SearchText, PageSize, showTopForAiken, "search?Filters=289%3AResidential&mID=112" + AddSearchTextQueryString("&"));
			uxHomesBottom.BindData("Homes in ", GetHomesShowcaseID(showBottomForAiken), SearchText, PageSize, showBottomForAiken, "search?Filters=289%3AResidential&mID=112" + AddSearchTextQueryString("&"));
			uxLotsLandTop.BindData("Lots/Land in ", GetLotsLandShowcaseID(showTopForAiken), SearchText, PageSize, showTopForAiken, "search-lots-land" + AddSearchTextQueryString("?"));
			uxLotsLandBottom.BindData("Lots/Land in ", GetLotsLandShowcaseID(showBottomForAiken), SearchText, PageSize, showBottomForAiken, "search-lots-land" + AddSearchTextQueryString("?"));
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);

		if(!uxHomesTop.Visible && !uxHomesBottom.Visible && !uxLotsLandTop.Visible && !uxLotsLandBottom.Visible)
			uxNoResults.Text = "No results were found for “" + Server.HtmlEncode(SearchText) + "”.  Please try searching again.";
	}


	private int GetHomesShowcaseID(bool showforAiken)
	{
		return (showforAiken ? (int)Classes.Showcase.MeybohmShowcases.AikenExistingHomes : (int)Classes.Showcase.MeybohmShowcases.AugustaExistingHomes);
	}
	private int GetLotsLandShowcaseID(bool showforAiken)
	{
		return (showforAiken ? (int)Classes.Showcase.MeybohmShowcases.AikenLand : (int)Classes.Showcase.MeybohmShowcases.AugustaLand);
	}

	private string AddSearchTextQueryString(string prefix)
	{
		if(string.IsNullOrWhiteSpace(SearchText))
			return string.Empty;

		return prefix + "searchtext=" + SearchText;
	}
}