using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaseCode;

public class BaseListingPage : Page
{
	#region Members

	/// <summary>
	/// The add button for the listing
	/// </summary>
	protected HyperLink m_AddButton;

	/// <summary>
	/// The literal containing links to javascript files to be added to the bundler
	/// </summary>
	protected Literal m_AdditionalJavaScriptFiles;

	/// <summary>
	/// The html link containing links to css files to be added to the bundler
	/// </summary>
	protected HtmlGenericControl m_AdditionalCssFiles;

	protected Literal m_BreadCrumbTitle;

	/// <summary>
	/// Used for title, breadcrumbs, and add button
	/// </summary>
	protected string m_ClassName;

	private Literal m_CustomBreadCrumbsLiteral;
	protected PlaceHolder m_CustomBreadCrumbsPH;

	protected PlaceHolder m_CustomButtonPH;

	/// <summary>
	/// By default, the listing will be sorted by this field
	/// </summary>
	protected string m_DefaultSortField;
	/// <summary>
	/// By default, the listing will be sorted by this field
	/// </summary>
	protected bool m_DefaultSortDirection = true;

	/// <summary>
	/// The placeholder to inject the pagination and filtering into
	/// </summary>
	protected PlaceHolder m_FiltersPlaceHolder;

	protected PlaceHolder m_FilterBlueToggleAreaTop;

	protected PlaceHolder m_FilterBlueToggleAreaBottom;

	/// <summary>
	/// The placeholder to inject the title, breadcrumbs, and search panel into
	/// </summary>
	protected PlaceHolder m_Header;

	/// <summary>
	/// The title of the page, typically m_Classname + " Manager"
	/// </summary>
	protected Literal m_HeaderTitle;

	/// <summary>
	/// Boolean value used to populate add/edit address
	/// </summary>
	protected bool m_IdIsGuid;

	/// <summary>
	/// Link pointing to the edit page (with id parameter as the querystring)
	/// </summary>
	protected string m_LinkToEditPage;

	protected int m_ColumnNumberToMakeLink = 1;

	protected bool m_DisablePaging;

	protected HtmlGenericControl m_PageSelectContainer;
	protected DropDownList m_PageSizeSelect;
	protected int m_PageSizeOverride;

	protected HtmlGenericControl m_PagerTop;
	protected HtmlGenericControl m_PagerBottom;

	protected Literal m_PagingNumberShown;

	private Literal m_PostTitleLiteral;

	/// <summary>
	/// Literal inserted immediately after h1 for title
	/// </summary>
	protected PlaceHolder m_PostTitlePlaceHolder;

	/// <summary>
	/// The search button for the search feature of the listing
	/// </summary>
	protected HyperLink m_SearchButton;

	/// <summary>
	/// The cancel search button for the search feature of the listing
	/// </summary>
	protected HyperLink m_SearchCancelButton;

	/// <summary>
	/// The search panel for the search feature of the listing
	/// </summary>
	protected Panel m_SearchPanel;

	/// <summary>
	/// The search box for the search feature of the listing
	/// </summary>
	protected TextBox m_SearchTextBox;

	protected bool m_SearchAsYouTypePageEnabled = true;

	protected bool m_ShowFiltersByDefault;

	#endregion

	#region Properties

	protected int PageNumber
	{
		get
		{
			int temp;
			return !String.IsNullOrEmpty(Request.QueryString["Page"]) && Int32.TryParse(Request.QueryString["Page"], out temp) ? temp : 1;
		}
	}

	protected string SortField
	{
		get { return String.IsNullOrEmpty(Request.QueryString["sortField"]) ? m_DefaultSortField : Request.QueryString["sortField"]; }
	}

	protected bool SortDirection
	{
		get
		{
			bool temp;
			return !String.IsNullOrEmpty(Request.QueryString["sortDirection"]) && Boolean.TryParse(Request.QueryString["sortDirection"], out temp) ? temp : m_DefaultSortDirection;
		}
	}

	protected string SearchText
	{
		get { return String.IsNullOrEmpty(Request.QueryString["searchText"]) ? "" : Request.QueryString["searchText"]; }
	}

	protected int PageSize
	{
		get
		{
			if (m_DisablePaging)
				return 0;
			int temp;
			//An error here means you have not defined the filter placeholder, add m_FiltersPlaceHolder = uxFilterPlaceHolder; to your page.			
			return !String.IsNullOrEmpty(Request.QueryString["PageSize"]) && Int32.TryParse(Request.QueryString["PageSize"], out temp) ? temp : Convert.ToInt32(m_PageSizeSelect.SelectedValue);
		}
	}
	#endregion

	protected bool IframeView
	{
		get { return !String.IsNullOrEmpty(Request.QueryString["iframeView"]); }
	}

	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		if (Master != null)
		{
			SetCssAndJs();
			string masterPageScript = ((Literal)Master.FindControl("uxJavaScripts")).Text;
			if (m_AdditionalJavaScriptFiles != null)
			{
				masterPageScript += (String.IsNullOrEmpty(masterPageScript) ? "" : ",") + m_AdditionalJavaScriptFiles.Text;
				m_AdditionalJavaScriptFiles.Visible = false;
			}
			((Literal)Master.FindControl("uxJavaScripts")).Text = masterPageScript + (String.IsNullOrEmpty(masterPageScript) ? "" : ",") + @"~/tft-js/core/knockout.js,~/tft-js/core/jquery.dateformat.js,~/tft-js/core/admin-listing.js";

			if (m_AdditionalCssFiles != null)
			{
				string masterPageCss = ((HtmlLink)Master.FindControl("uxCSSFiles")).Href;
				((HtmlLink)Master.FindControl("uxCSSFiles")).Href = masterPageCss + (String.IsNullOrEmpty(masterPageCss) ? "" : ",") + m_AdditionalCssFiles.Attributes["href"];
				m_AdditionalCssFiles.Visible = false;
			}
			if (IframeView)
			{
				Master.FindControl("ContentMenu").Visible = false;
				string masterPageCss = ((HtmlLink)Master.FindControl("uxCSSFiles")).Href;
				((HtmlLink)Master.FindControl("uxCSSFiles")).Href = masterPageCss.Replace("~/admin/css/structure.css", "~/admin/css/structureModal.css");
			}
		}
	}

	protected virtual void SetCssAndJs()
	{
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		if (m_Header != null)
		{
			m_Header.Controls.Add(new Literal
			{
				Text = @"<div class=""title"">
						<h1>"
			});
			m_HeaderTitle = new Literal { Text = m_ClassName + @" Manager" };
			m_Header.Controls.Add(m_HeaderTitle);
			m_Header.Controls.Add(new Literal
			{
				Text = @"</h1>"
			});
			if (m_PostTitlePlaceHolder != null)
			{
				m_PostTitleLiteral = new Literal();
				m_Header.Controls.Add(m_PostTitleLiteral);
			}
			m_Header.Controls.Add(new Literal
			{
				Text = @"</div>
					<ul class=""breadcrumbs clearfix"">
						<li class=""firstBreadcrumb"">
							<a href=""" + ResolveClientUrl("~/admin/") + @""" title=""Home"">Dashboard</a></li>"
			});
			if (m_CustomBreadCrumbsPH != null)
			{
				m_CustomBreadCrumbsLiteral = new Literal();
				m_Header.Controls.Add(m_CustomBreadCrumbsLiteral);
			}
			m_Header.Controls.Add(new Literal
			{
				Text = @"<li class=""currentBreadcrumb"">"
			});
			m_BreadCrumbTitle = new Literal { Text = m_ClassName + @" Manager" };
			m_Header.Controls.Add(m_BreadCrumbTitle);
			m_Header.Controls.Add(new Literal
			{
				Text = @"</li>
					</ul>"
			});

			m_SearchPanel = new Panel { ID = "uxSearchPnl", CssClass = "bottom" };

			#region SearchPanelControls

			m_AddButton = new HyperLink { ID = "uxAdd", Text = @"<span>Add New</span>", CssClass = "button add floatRight", NavigateUrl = m_LinkToEditPage + "0" };
			m_AddButton.Attributes.Add("data-bind", "attr:{href:'" + m_LinkToEditPage + "0' + returnString() }");
			m_SearchTextBox = new TextBox { ID = "uxSearchText", CssClass = "text" };
			m_SearchButton = new HyperLink { ID = "uxSearch", Text = @"<span>Search</span>", CssClass = "button" };
			m_SearchCancelButton = new HyperLink { ID = "uxCancelSearch", Text = @"<span>Cancel Search</span>", CssClass = "button delete", NavigateUrl = "#" };
			m_SearchTextBox.Attributes.Add("data-bind", "value: searchText,enable:!listingModel.displayOrderEditable()" + (Globals.Settings.AdminSearchAsYouType && m_SearchAsYouTypePageEnabled ? ",valueUpdate:'afterkeydown'" : ""));
			m_SearchCancelButton.Attributes.Add("data-bind", "visible: searchText() != '', click: function (){searchText('');}");

			if (m_CustomButtonPH != null)
				m_SearchPanel.Controls.Add(m_CustomButtonPH);
			m_SearchPanel.Controls.Add(m_AddButton);
			m_SearchPanel.Controls.Add(m_SearchTextBox);
			m_SearchPanel.Controls.Add(m_SearchButton);
			m_SearchPanel.Controls.Add(m_SearchCancelButton);
			#endregion

			m_Header.Controls.Add(new Literal { Text = @"<div class=""sectionTitle padded"">" });
			m_Header.Controls.Add(m_SearchPanel);
			m_Header.Controls.Add(new Literal { Text = @"</div>" });
		}

		if (m_FiltersPlaceHolder != null)
		{
			string pageSizeVisibilityCondition = @"numberPages() > 1 || pageSize() != " + Globals.Settings.AdminPageSize;
			bool showFilterBlueBox = m_FiltersPlaceHolder.Controls.Count > 0;
			m_FilterBlueToggleAreaTop = new PlaceHolder();
			m_FilterBlueToggleAreaTop.Controls.Add(new Literal { Text = @"<div class=""filters"">" });

			#region Paging
			if (!m_DisablePaging)
			{
				m_PagerTop = new HtmlGenericControl("span");
				m_PagerBottom = new HtmlGenericControl("span");
				m_PagerTop.Attributes["class"] = m_PagerBottom.Attributes["class"] = "pagination";
				m_PagerTop.Attributes.Add("data-bind", "visible: numberPages() > 1");
				m_PagerBottom.Attributes.Add("data-bind", "visible: numberPages() > 1");
				const string insidePagerText = @"<a class=""prev"" data-bind=""css:{aspNetDisabled: pageNumber() == 1}, click: function(){if (pageNumber() > 1)pageNumber(pageNumber()-1);}"">Previous</a>
	<ul class=""clearfix"" data-bind=""foreach: pages""><li>
			<span data-bind=""visible: listingModel.pageNumber() == number(), text: number"">&nbsp;</span>
			<a href=""#"" data-bind=""visible: listingModel.pageNumber() != number(), click: changePage, text: number""></a>
		</li></ul>
	<a class=""next"" data-bind=""css:{aspNetDisabled: pageNumber() == numberPages()}, click: function(){if (pageNumber() < numberPages())pageNumber(pageNumber()+1);}"">Next</a>";
				m_PagerTop.Controls.Add(new Literal { Text = insidePagerText });
				m_PagerBottom.Controls.Add(new Literal { Text = insidePagerText });

				m_FilterBlueToggleAreaTop.Controls.Add(m_PagerTop);
				if (Master != null)
					Helpers.FindContentPlaceHolder(Master, "ContentWindow").Controls.Add(m_PagerBottom);
			}
			#endregion

			if (showFilterBlueBox)
				m_FilterBlueToggleAreaTop.Controls.Add(new Literal { Text = @"<a href=""#"" class=""toggle " + (m_ShowFiltersByDefault ? @"up" : @"down") + @""">" + (m_ShowFiltersByDefault ? @"hide" : @"show") + @" filtering options</a>" });
			m_PagingNumberShown = new Literal
			{
				Text = @"<h5 data-bind=""visible:listings().length > 0,text:'Showing ' + listings().length + ' of ' + totalCount()""></h5>"
			};
			m_FilterBlueToggleAreaTop.Controls.Add(m_PagingNumberShown);
			if (!m_DisablePaging)
			{
				m_PageSizeSelect = new DropDownList { CssClass = "resultsAmount" };
				int globalPageSize = Globals.Settings.AdminPageSize;
				List<int> pageSizes = new List<int> { 10, 25, 50 };
				if (globalPageSize != 0 && !pageSizes.Contains(globalPageSize))
					pageSizes.Add(globalPageSize);
				if (m_PageSizeOverride != 0 && !pageSizes.Contains(m_PageSizeOverride) && m_PageSizeOverride != 99999999)
					pageSizes.Add(m_PageSizeOverride);

				pageSizes.OrderBy(p => p).ToList().ForEach(p => m_PageSizeSelect.Items.Add(new ListItem("View " + p + " Results", p.ToString())));
				m_PageSizeSelect.Items.Add(new ListItem("View All Results", "99999999"));

				if (!IsPostBack && m_PageSizeOverride != 0 && m_PageSizeSelect.Items.FindByValue(m_PageSizeOverride.ToString()) != null)
					m_PageSizeSelect.Items.FindByValue(m_PageSizeOverride.ToString()).Selected = true;
				else if (!IsPostBack && m_PageSizeSelect.Items.FindByValue(Globals.Settings.AdminPageSize.ToString()) != null)
					m_PageSizeSelect.Items.FindByValue(Globals.Settings.AdminPageSize.ToString()).Selected = true;
				m_PageSizeSelect.Attributes.Add("data-bind", "value:pageSize,visible:" + pageSizeVisibilityCondition + ",enable:!listingModel.displayOrderEditable()");
				m_FilterBlueToggleAreaTop.Controls.Add(m_PageSizeSelect);
			}
			m_FilterBlueToggleAreaTop.Controls.Add(new Literal
			{
				Text = @"<div class=""clear""></div>
						<div class=""blue toggleArea""" + (m_ShowFiltersByDefault ? string.Empty : @" style=""display: none;""") + @">"
			});

			m_FilterBlueToggleAreaBottom = new PlaceHolder();

			if (showFilterBlueBox)
				m_FilterBlueToggleAreaBottom.Controls.Add(new Literal
				{
					Text = @"<div class=""column last"">
							<a href=""#"" class=""button small hide""><span>Hide</span></a>
							<div class=""clear""></div>
						</div>"
				});
			m_FilterBlueToggleAreaBottom.Controls.Add(new Literal
			{
				Text = @"<div class=""clear""></div>
					</div>
				</div>"
			});

			Control parentControl = m_FiltersPlaceHolder.Parent;
			parentControl.Controls.AddAt(parentControl.Controls.IndexOf(m_FiltersPlaceHolder), m_FilterBlueToggleAreaTop);
			parentControl.Controls.AddAt(parentControl.Controls.IndexOf(m_FiltersPlaceHolder) + 1, m_FilterBlueToggleAreaBottom);
		}
		ClientScript.RegisterStartupScript(GetType(), "DefaultListingVars", @"var defaultPageNumber = " + PageNumber + ", defaultPageSize = " + PageSize + ", defaultSearchText = '" + SearchText + "', defaultSortField = '" + SortField + "', defaultSortDirection = " + SortDirection.ToString().ToLower() + ", defaultPageUrl = '" + System.Web.VirtualPathUtility.GetFileName(Request.FilePath) + "', defaultBasePageSize = " + Globals.Settings.AdminPageSize + ", defaultBaseSortField = '" + m_DefaultSortField + "', linkToEditPage = '" + m_LinkToEditPage + "', entityClassName = '" + m_ClassName.Replace("'", "\\'") + "', columnNumberToMakeLink = " + m_ColumnNumberToMakeLink + ";", true);

		if (String.IsNullOrEmpty(Page.Title))
			Page.Title = @"Admin - " + m_ClassName + @" Manager";
	}

	protected virtual void Page_Load(object sender, EventArgs e)
	{
		if (String.IsNullOrEmpty(m_ClassName))
			throw new Exception("A programmer needs to correctly set up the page information: m_ClassName is missing");
		if (!IsPostBack)
		{
			if (!String.IsNullOrEmpty(SearchText))
				m_SearchTextBox.Text = SearchText;
			else
				m_SearchCancelButton.Attributes.Add("display", "none");
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		if (m_CustomBreadCrumbsPH != null)
		{
			m_CustomBreadCrumbsPH.Visible = true;
			m_CustomBreadCrumbsLiteral.Text = Helpers.RenderHtmlAsString(m_CustomBreadCrumbsPH);
			m_CustomBreadCrumbsPH.Visible = false;
		}
		if (m_PostTitlePlaceHolder != null)
		{
			m_PostTitlePlaceHolder.Visible = true;
			m_PostTitleLiteral.Text = Helpers.RenderHtmlAsString(m_PostTitlePlaceHolder);
			m_PostTitlePlaceHolder.Visible = false;
		}
	}
}