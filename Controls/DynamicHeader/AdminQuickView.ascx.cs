using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.DynamicHeader;

public partial class Controls_DynamicHeader_AdminQuickView : UserControl
{
	private string m_CollectionName = "Homepage";
	public string CollectionName
	{
		get { return m_CollectionName; }
		set { m_CollectionName = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxAdminQuickView.ComponentVersionNumber = Settings.VersionNumber;
		if (!IsPostBack)
		{
			PlaceHolder uxContentArea = (PlaceHolder)uxAdminQuickView.FindControl("uxContentArea");
			Literal uxUpdatedOn = (Literal)uxContentArea.Controls[0].FindControl("uxUpdatedOn");
			Controls_DynamicHeader_DynamicHeader uxDynamicHeader = (Controls_DynamicHeader_DynamicHeader)uxContentArea.Controls[0].FindControl("uxDynamicHeader");
			uxDynamicHeader.CollectionName = CollectionName;
			DynamicCollection collection = DynamicCollection.DynamicCollectionGetByName(CollectionName).FirstOrDefault();
			if (collection != null)
			{
				DynamicImage lastUpdatedImage = DynamicImage.DynamicImagePage(0, 1, "", "LastUpdated", false, new DynamicImage.Filters { FilterDynamicCollectionID = collection.DynamicCollectionID.ToString() }).FirstOrDefault();
				if (lastUpdatedImage != null)
					uxUpdatedOn.Text = "Last Updated: " + lastUpdatedImage.LastUpdatedClientTime.ToString();
			}
		}
	}
}