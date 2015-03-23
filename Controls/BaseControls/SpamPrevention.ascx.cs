using System;
using System.Web.UI;
using BaseCode;

public partial class Controls_BaseControls_SpamPrevention : UserControl
{
	public bool AlwaysShow { get; set; }
	public string SubmitClientIDName { get; set; }

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		if (AlwaysShow || !Page.User.Identity.IsAuthenticated)
		{
			Helpers.GetCSSCode(uxCSSFiles);
			Helpers.GetJSCode(uxJavaScripts);
		}
		else
			Visible = false;
	}
}