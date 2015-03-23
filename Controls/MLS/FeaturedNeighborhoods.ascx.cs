using System;
using System.Web.UI;
using BaseCode;
using Classes.ContentManager;
using Classes.MLS;

public partial class Controls_MLS_FeaturedNeighborhoods : System.Web.UI.UserControl
{
	protected string m_CurrentMicrositePath { get { return ((microsite)Page.Master).CurrentMicrositePath; } }

	protected CMMicrosite m_CurrentMicrosite { get { return ((microsite)Page.Master).CurrentMicrosite; } }

	protected bool m_UseCarousel;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			uxNeighborhoods.DataSource = Neighborhood.NeighborhoodPage(0, 0, "", "Name", true, new Neighborhood.Filters { FilterNeighborhoodActive = true.ToString(), FilterNeighborhoodCMMicrositeID = m_CurrentMicrosite.CMMicroSiteID.ToString(), FilterNeighborhoodFeatured = true.ToString() });
			uxNeighborhoods.DataBind();

			if (uxNeighborhoods.Items.Count == 0)
				Visible = false;
			m_UseCarousel = uxNeighborhoods.Items.Count > 3;
			if (m_UseCarousel)
				Helpers.GetJSCode(uxJavaScripts);
			else 
			uxJavaScripts.Visible = false;
		}
	}
}