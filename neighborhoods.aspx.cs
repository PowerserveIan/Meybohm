using System;
using System.Collections.Generic;
using System.Linq;
using Classes.MLS;

public partial class neighborhoods : BasePage
{
	protected string m_NeighborhoodJS;

	public override void SetComponentInformation()
	{
		ComponentName = "Neighborhood";
		ComponentAdminPage = "m-l-s/admin-neighborhood.aspx";
		NewHomePage = true;
	}

	protected override void SetCssAndJs()
	{
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			if (!String.IsNullOrEmpty(Request.QueryString["searchText"]))
				uxSearch.Text = Request.QueryString["searchText"];

			Neighborhood.Filters filterList = new Neighborhood.Filters();
			filterList.FilterNeighborhoodActive = true.ToString();
			filterList.FilterNeighborhoodCMMicrositeID = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite().CMMicroSiteID.ToString();
			string searchText = uxSearch.Text;
			if (!String.IsNullOrEmpty(searchText))
			{
				int zip;
				if (Int32.TryParse(uxSearch.Text, out zip))
				{
					filterList.FilterNeighborhoodZip = zip.ToString();
					searchText = string.Empty;
				}
			}

			List<Neighborhood> neighborhoods = Neighborhood.NeighborhoodPageForFrontend(0, 0, searchText, "Name", true, filterList);

			uxNeighborhoods.DataSource = neighborhoods;
			uxNeighborhoods.DataBind();

			uxNeighborhoods.Visible = uxMapContainerPH.Visible = neighborhoods.Any();
			uxNoNeighborhoods.Visible = !neighborhoods.Any();

			m_NeighborhoodJS = string.Empty;
			foreach (Neighborhood n in neighborhoods.Where(n => n.Address.Latitude.HasValue && n.Address.Longitude.HasValue))
			{
				m_NeighborhoodJS += "new markerItem(" + n.Address.Latitude + ", " + n.Address.Longitude + ", \"" + GetMarkerContent(n) + "\"),";
			}
			m_NeighborhoodJS = m_NeighborhoodJS.TrimEnd(',');

			Dictionary<string, string> unacceptableValues = new Dictionary<string, string>();
			unacceptableValues.Add("searchText", "");
			CanonicalLink = BaseCode.Helpers.RootPath + MicrositePath + "/neighborhoods" + BaseCode.Helpers.GetQueryStringWithAcceptableValues(unacceptableValues);
		}
	}

	private string GetMarkerContent(Neighborhood n)
	{
		return string.Format("<img src='{7}' width='116' height='81' align='left' /><a href='neighborhood-details?id={0}'>{1}</a><br /> {2}<br /> {3}, {4}, {5}<br /> {6}", n.NeighborhoodID, n.Name, n.Address.Address1, n.Address.City, n.Address.State.Name, n.Address.Zip, n.PriceRange, BaseCode.Helpers.RootPath + "uploads/neighborhoods/" + n.Image + "?width=116&height=81&mode=crop&anchor=middlecenter");
	}
}