using System;
using System.Web.UI;
using BaseCode;

public partial class NewsletterSubscribePopup : BasePage
{
	public override void SetComponentInformation()
	{
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		Helpers.GetCSSCode(uxCSSFiles);
		Helpers.GetJSCode(uxJavaScripts);
	}
}