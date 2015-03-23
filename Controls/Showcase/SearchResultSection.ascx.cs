using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.Showcase;

public partial class Controls_Showcase_SearchResultSection : System.Web.UI.UserControl
{
	protected string micrositePath = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

    }


	public void BindData(string sectionTitlePrefix, int showCaseID, string searchText, int pageSize, bool isForAiken, string pageForMore)
	{
		micrositePath = (isForAiken ? "aiken" : "augusta") + "/";
		uxSectionTitle.Text = sectionTitlePrefix + (sectionTitlePrefix.EndsWith(" ") ? string.Empty : " ") + (isForAiken ? "Greater Aiken MLS <span>including Aiken & Edgefield</span>" : "Greater Augusta MLS <span>including Augusta, Evans, Grovetown, Martinez, Harlem, & North Augusta</span>");

		ShowcaseItem.Filters filterList = new ShowcaseItem.Filters();
		filterList.FilterShowcaseItemActive = true.ToString();
		filterList.SearchText = searchText;

		filterList.FilterShowcaseItemShowcaseID = showCaseID.ToString();


		int attributeID = ShowcaseAttribute.GetAttributeIDByShowcaseIDAndAttributeTitle(showCaseID, "Property Type");
		List<ShowcaseItemForJSON> resultList = ShowcaseItem.GetPagedFilteredShowcaseItems(0, pageSize, (showCaseID <= 4 ? attributeID.ToString() + ":Residential" : string.Empty), "ListPrice", false, filterList);
		
		uxItemList.DataSource = resultList;
		uxItemList.DataBind();

		if (uxItemList.Items.Count <= 0)
			Visible = false;
		else
		{
			int totalCount = (resultList != null && resultList.Any() ? resultList[0].TotalRowCount.GetValueOrDefault() : 0);
			uxCount.Text = string.Format("Showing 1-{0} of {1} results.&nbsp;", uxItemList.Items.Count, totalCount);
			if (totalCount <= pageSize)
				uxSeeMore.Visible = false;
			else
			{
				uxSeeMore.InnerHtml = "See more " + uxSectionTitle.Text.Split(new string[] { "including" }, StringSplitOptions.None)[0] + " that match “" + Server.HtmlEncode(searchText) + "” &raquo;";
				uxSeeMore.HRef = ResolveUrl("~/" + micrositePath + pageForMore);
			}
		}
	}


	protected string GetImageUrl(string currentUrl)
	{
		if (!string.IsNullOrWhiteSpace(currentUrl))
			return currentUrl;
		return ResolveUrl("~/uploads/images/missingFile.jpg?width=136&height=97&mode=crop&anchor=middlecenter");
	}
}