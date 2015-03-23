using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Media352_NewsPress;

public partial class Controls_Media352_NewsPress_AdminQuickView : UserControl
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxAdminQuickView.ComponentVersionNumber = Settings.VersionNumber;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			PlaceHolder uxContentArea = (PlaceHolder)uxAdminQuickView.FindControl("uxContentArea");
			PlaceHolder uxUpdateCallout = (PlaceHolder)uxContentArea.Controls[0].FindControl("uxUpdateCallout");
			NewsPress mostRecent = NewsPress.NewsPressPage(0, 1, "", "Date", false).FirstOrDefault();
			if (mostRecent != null)
			{
				Literal uxDate = (Literal)uxContentArea.Controls[0].FindControl("uxDate");
				Literal uxSummary = (Literal)uxContentArea.Controls[0].FindControl("uxSummary");
				HyperLink uxTitleLink = (HyperLink)uxContentArea.Controls[0].FindControl("uxTitleLink");

				uxDate.Text = mostRecent.DateClientTime.ToString();
				uxSummary.Text = Helpers.ForceShorten(mostRecent.Summary, 250);
				uxTitleLink.Text = mostRecent.Title;
				uxTitleLink.NavigateUrl = "~/admin/media352-news-press/admin-news-press-edit.aspx?id=" + mostRecent.NewsPressID;
				if (mostRecent.Date.AddMonths(1) <= DateTime.UtcNow)
					uxUpdateCallout.Visible = true;
			}
			else
			{
				uxUpdateCallout.Visible = true;
				Literal uxCallOutText = (Literal)uxContentArea.Controls[0].FindControl("uxCallOutText");
				uxCallOutText.Text = "You need to add News Press Articles!";
			}
		}
	}
}