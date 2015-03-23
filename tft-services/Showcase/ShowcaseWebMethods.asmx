<%@ WebService Language="C#" Class="ShowcaseWebMethods" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Classes.Showcase;

[WebService(Namespace = "http://352media.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class ShowcaseWebMethods : WebService
{
	[WebMethod]
	public List<ShowcaseItemForJSON> LoadMoreItems(int pageSize, int pageNumber, string filters, int showcaseID, decimal? addressLat, decimal? addressLong, int? minDistance, int? maxDistance, int? agentID, bool? openHouse, string searchText, string sortField, bool sortDirection)
	{

		
		ShowcaseItem.Filters filterList = new ShowcaseItem.Filters();
		filterList.FilterShowcaseItemActive = true.ToString();
		filterList.NewHomesOnly = (showcaseID == (int)NewMeybohmShowcases.AikenNewHomes ||
												showcaseID == (int)NewMeybohmShowcases.AugustaNewHomes);
		showcaseID = showcaseID == (int)NewMeybohmShowcases.AikenNewHomes
						 ? (int)MeybohmShowcases.AikenExistingHomes
						 : (showcaseID == (int)NewMeybohmShowcases.AugustaNewHomes
								? (int)MeybohmShowcases.AugustaExistingHomes
								: showcaseID);
		filterList.FilterShowcaseItemShowcaseID = showcaseID.ToString();
		if (agentID.HasValue)
			filterList.FilterShowcaseItemAgentID = agentID.ToString();

		
		filterList.AddressLat = addressLat;
		filterList.AddressLong = addressLong;
		filterList.MinDistance = minDistance;
		filterList.MaxDistance = maxDistance;
		filterList.OpenHouse = openHouse;
		filterList.SearchText = searchText.Trim();
		return ShowcaseItem.GetPagedFilteredShowcaseItems((pageNumber - 1) * pageSize, pageSize, filters, sortField, sortDirection, filterList);
	}

	[WebMethod]
	public int GetItemCountForQuickSearch(string filters, int showcaseID,bool newHomes)
	{
		return ShowcaseItem.GetItemCountFromSearch(filters, showcaseID, newHomes);
	}

	[WebMethod(true)]
	public void SaveSearch(int showcaseID, string filterString, int? showcaseItemID, string name, bool enableEmailNotifications, bool separateEmail, bool dailyEmail, int? savedSearchID, bool newProperties)
	{
		SavedSearch savedSearchEntity = savedSearchID.HasValue ? SavedSearch.GetByID(savedSearchID.Value) : new SavedSearch();
		if (!savedSearchID.HasValue)
		{
			if (showcaseItemID.HasValue)
			{
				ShowcaseItem showcaseItemEntity = ShowcaseItem.GetByID(showcaseItemID.Value, new string[] { "Showcase" });
				savedSearchEntity.Image = showcaseItemEntity.Image;
				savedSearchEntity.ShowcaseItemID = showcaseItemID;
			}
			else
				savedSearchEntity.FilterString = filterString;

			savedSearchEntity.ShowcaseID = showcaseID;
			savedSearchEntity.Created = DateTime.UtcNow;
			savedSearchEntity.UserID = BaseCode.Helpers.GetCurrentUserID();
		}
		savedSearchEntity.NewHomeSearch = newProperties;
		savedSearchEntity.DailyEmail = dailyEmail;
		savedSearchEntity.EnableEmailNotifications = enableEmailNotifications;
		savedSearchEntity.Name = name;
		savedSearchEntity.SeparateEmail = separateEmail;
		savedSearchEntity.Save();
	}

	[WebMethod(true)]
	public void TrackClick(int showcaseItemID, int clickType)
	{
		int? userID = BaseCode.Helpers.GetCurrentUserID();
		if (userID == 0)
			userID = null;
		new ShowcaseItemMetric { ClickTypeID = clickType, Date = DateTime.Now, SessionID = HttpContext.Current.Session.SessionID, ShowcaseItemID = showcaseItemID, UserID = userID }.Save();
		Dictionary<int, List<int>> sessionHomeVisits = HttpContext.Current.Session["HomeVisits"] as Dictionary<int, List<int>> ?? new Dictionary<int, List<int>>();
		if (!sessionHomeVisits.ContainsKey(showcaseItemID))
			sessionHomeVisits.Add(showcaseItemID, new List<int>());
		sessionHomeVisits[showcaseItemID].Add(clickType);
		HttpContext.Current.Session["HomeVisits"] = sessionHomeVisits;
	}


    [WebMethod]
    public List<Showcase_GetFilteredShowcaseAttributeIDs_Result> GetUpdatedAttributes(string filters, int showcaseID, decimal? addressLat, decimal? addressLong, int? minDistance, int? maxDistance, int? agentID, bool? openHouse, string searchText, string sortField, bool sortDirection)
    {
        ShowcaseItem.Filters filterList = new ShowcaseItem.Filters();
        filterList.FilterShowcaseItemActive = true.ToString();
        filterList.NewHomesOnly = (showcaseID == (int)NewMeybohmShowcases.AikenNewHomes ||
                                                showcaseID == (int)NewMeybohmShowcases.AugustaNewHomes);
        showcaseID = showcaseID == (int)NewMeybohmShowcases.AikenNewHomes
                         ? (int)MeybohmShowcases.AikenExistingHomes
                         : (showcaseID == (int)NewMeybohmShowcases.AugustaNewHomes
                                ? (int)MeybohmShowcases.AugustaExistingHomes
                                : showcaseID);
        filterList.FilterShowcaseItemShowcaseID = showcaseID.ToString();
        if (agentID.HasValue)
            filterList.FilterShowcaseItemAgentID = agentID.ToString();


        filterList.AddressLat = addressLat;
        filterList.AddressLong = addressLong;
        filterList.MinDistance = minDistance;
        filterList.MaxDistance = maxDistance;
        filterList.OpenHouse = openHouse;
        filterList.SearchText = searchText.Trim();
        return ShowcaseItem.GetFilteredShowcaseItemsAttributeValues(filters, filterList);
    }


    [WebMethod]
    public List<Showcase_GetFilteredShowcaseAttributeMinMax_Result> GetUpdatedMinMaxForQuickSearch(string filters, int showcaseID)
    {
        return ShowcaseItem.GetMinMaxFromSearch(filters, showcaseID);
    }
}