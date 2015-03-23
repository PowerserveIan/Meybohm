using System;
using System.Web.UI;

public partial class sitemap : BasePage
{
	public override void SetComponentInformation()
	{
		ComponentName = "Content";
		ComponentAdminPage = "content-manager/sitemap.aspx";
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		EnableWhiteSpaceCompression = false;
	}
}