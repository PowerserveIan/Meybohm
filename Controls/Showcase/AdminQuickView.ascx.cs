using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.Showcase;

public partial class Controls_Showcase_AdminQuickView : UserControl
{
	private int m_NumberOfShowcaseItems = 5;

	public int NumberOfShowcaseItems
	{
		get { return m_NumberOfShowcaseItems; }
		set { m_NumberOfShowcaseItems = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxAdminQuickView.ComponentVersionNumber = Settings.VersionNumber;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			/*PlaceHolder uxContentArea = (PlaceHolder)uxAdminQuickView.FindControl("uxContentArea");
			Repeater uxMostPopularItems = (Repeater)uxContentArea.Controls[0].FindControl("uxMostPopularItems");
			if (Settings.EnableStatisticsTracking)
			{
				List<ShowcaseItemMetric> items = ShowcaseItemMetric.GetMostPopularItems(NumberOfShowcaseItems, null, DateTime.UtcNow.AddDays(-30).Date, DateTime.UtcNow.AddDays(1).Date);
				if (items.Count > 0)
				{
					uxMostPopularItems.DataSource = items;
					uxMostPopularItems.DataBind();
				}
				else
				{
					uxMostPopularItems.Visible = false;
					((Literal)uxContentArea.Controls[0].FindControl("uxNoItemsClicked")).Visible = true;
				}
			}
			else
			{
				uxMostPopularItems.Visible = false;
			}*/
		}
	}
}