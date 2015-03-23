using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.Showcase;

public partial class zf_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		SavedSearch entity = SavedSearch.GetByID(26);
		Response.Write(GetItemCountFromSearch(entity.FilterString, 4));
    }

	public int GetItemCountFromSearch(string filters, int showcaseID)
	{
		ShowcaseItem.Filters filterList = new ShowcaseItem.Filters();
		filterList.FilterShowcaseItemActive = true.ToString();
		filterList.FilterShowcaseItemShowcaseID = showcaseID.ToString();

		filters = Server.UrlDecode(filters);
		string filterText = string.Empty;
		string[] queryStrings = filters.Split('&');
		foreach (string obj in queryStrings){
			if (obj.StartsWith("Filters="))
				filters = obj.Replace("Filters=", "");
			else if (obj.StartsWith("SearchText="))
				filterList.SearchText = obj.Replace("SearchText=", "");
			else if (obj.StartsWith("AgentID="))
				filterList.FilterShowcaseItemAgentID = obj.Replace("AgentID=", "");
			else if (obj.StartsWith("OpenHouse="))
				filterList.OpenHouse = Convert.ToBoolean(obj.Replace("OpenHouse=", ""));
		}


		List<ShowcaseItemForJSON> items = ShowcaseItem.GetPagedFilteredShowcaseItems(0, 1, filters, "", true, filterList);
		return items.Any() ? items.FirstOrDefault().TotalRowCount.Value : 0;
	}
}