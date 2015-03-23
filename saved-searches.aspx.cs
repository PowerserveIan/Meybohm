using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using BaseCode;
using Classes.Showcase;

public partial class saved_searches : BasePage
{
	protected bool NewHomePage { get; set; }
	protected override void SetCssAndJs()
	{
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	public override void SetComponentInformation()
	{
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxAddSavedSearch.ShowcaseID = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite().Name == "Aiken" ? (int)MeybohmShowcases.AikenExistingHomes : (int)MeybohmShowcases.AugustaExistingHomes;
		BaseMasterPage master = (microsite) Master;
		if (master != null) NewHomePage = (((microsite)Master).NewHomes);
		
		//Register Showcase Web Service
		ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
		if (scriptManager.Services.Count(s => s.Path == "~/tft-services/Showcase/ShowcaseWebMethods.asmx") == 0)
			scriptManager.Services.Add(new ServiceReference { InlineScript = true, Path = "~/tft-services/Showcase/ShowcaseWebMethods.asmx" });
	}

	[WebMethod]
	public static ListingItemWithCount<SavedSearch> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection, SavedSearch.Filters filterList = new SavedSearch.Filters())
	{
		int totalCount;
		filterList.FilterSavedSearchUserID = Helpers.GetCurrentUserID().ToString();
		List<SavedSearch> listItems = SavedSearch.SavedSearchPageByActiveCommunitiesWithTotalCount((pageNumber - 1) * pageSize, pageSize, searchText, sortField, sortDirection, out totalCount, filterList);
		return new ListingItemWithCount<SavedSearch> { Items = listItems, TotalCount = totalCount };
	}

	[WebMethod]
	public static void DeleteRecord(int id)
	{
		SavedSearch entity = SavedSearch.GetByID(id);
		if (entity != null)
			entity.Delete();
	}
}