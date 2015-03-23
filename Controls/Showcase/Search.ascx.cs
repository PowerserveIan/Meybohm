using System;
using System.Linq;
using System.Web.UI;
using BaseCode;
using Classes.Showcase;

public partial class Controls_MainContent_Search : System.Web.UI.UserControl
{
	public bool IsAiken { get; set; }
	public bool NewHomes { get; set; }

	protected string SearchLink { get { return NewHomes ? "new-search" : "search"; } }

	protected int m_ShowcaseID;
	protected int m_RentShowcaseID;

	protected int m_PriceFilterID;
	protected int m_RentPriceFilterID;

	protected int m_BedroomFilterID;
	protected int m_RentBedroomFilterID;

	protected int m_BathroomFilterID;
	protected int m_RentBathroomFilterID;

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		if (Visible)
		{
			m_ShowcaseID = IsAiken ?(int)MeybohmShowcases.AikenExistingHomes :(int)MeybohmShowcases.AugustaExistingHomes;
			m_RentShowcaseID = IsAiken ? (int)MeybohmShowcases.AikenRentalHomes : (int)MeybohmShowcases.AugustaRentalHomes;


		

			ShowcaseAttribute currentAttribute = ShowcaseAttribute.ShowcaseAttributePage(0, 1, "", "", true, new ShowcaseAttribute.Filters { FilterShowcaseAttributeShowcaseID = m_ShowcaseID.ToString(), FilterShowcaseAttributeMLSAttributeName = "List Price" }).FirstOrDefault();
			m_PriceFilterID = currentAttribute != null ? currentAttribute.ShowcaseAttributeID : 0;
			currentAttribute = ShowcaseAttribute.ShowcaseAttributePage(0, 1, "", "", true, new ShowcaseAttribute.Filters { FilterShowcaseAttributeShowcaseID = m_RentShowcaseID.ToString(), FilterShowcaseAttributeMLSAttributeName = "Rental Price" }).FirstOrDefault();
			m_RentPriceFilterID = currentAttribute != null ? currentAttribute.ShowcaseAttributeID : 0;

			currentAttribute = ShowcaseAttribute.ShowcaseAttributePage(0, 1, "", "", true, new ShowcaseAttribute.Filters { FilterShowcaseAttributeShowcaseID = m_ShowcaseID.ToString(), FilterShowcaseAttributeMLSAttributeName = "Bedrooms" }).FirstOrDefault();
			m_BedroomFilterID = currentAttribute != null ? currentAttribute.ShowcaseAttributeID : 0;
			currentAttribute = ShowcaseAttribute.ShowcaseAttributePage(0, 1, "", "", true, new ShowcaseAttribute.Filters { FilterShowcaseAttributeShowcaseID = m_RentShowcaseID.ToString(), FilterShowcaseAttributeMLSAttributeName = "Bedrooms" }).FirstOrDefault();
			m_RentBedroomFilterID = currentAttribute != null ? currentAttribute.ShowcaseAttributeID : 0;

			currentAttribute = ShowcaseAttribute.ShowcaseAttributePage(0, 1, "", "", true, new ShowcaseAttribute.Filters { FilterShowcaseAttributeShowcaseID = m_ShowcaseID.ToString(), FilterShowcaseAttributeMLSAttributeName = "Full Baths" }).FirstOrDefault();
			m_BathroomFilterID = currentAttribute != null ? currentAttribute.ShowcaseAttributeID : 0;
			currentAttribute = ShowcaseAttribute.ShowcaseAttributePage(0, 1, "", "", true, new ShowcaseAttribute.Filters { FilterShowcaseAttributeShowcaseID = m_RentShowcaseID.ToString(), FilterShowcaseAttributeMLSAttributeName = "Full Baths" }).FirstOrDefault();
			m_RentBathroomFilterID = currentAttribute != null ? currentAttribute.ShowcaseAttributeID : 0;

			//Register Showcase Web Service
			ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
			if (!scriptManager.Services.Any(s => s.Path == "~/tft-services/Showcase/ShowcaseWebMethods.asmx"))
				scriptManager.Services.Add(new ServiceReference { InlineScript = true, Path = "~/tft-services/Showcase/ShowcaseWebMethods.asmx" });

			uxRentTab.Visible = uxRentLink.Visible = !NewHomes;
		}
	}
	protected void uxSearchBtn_Click(object sender, EventArgs e)
	{
		if (String.IsNullOrWhiteSpace(uxSearchBox.Text)) return;

		Classes.ContentManager.CMMicrosite current = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite();
		if(current != null)
			Response.Redirect("~/" + current.Name.ToLower() + "/search-results.aspx?q=" + uxSearchBox.Text.Trim());
	}
}