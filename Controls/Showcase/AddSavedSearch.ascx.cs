using System;
using BaseCode;

public partial class Controls_Showcase_AddSavedSearch : System.Web.UI.UserControl
{
	public bool Editing { get; set; }
	public int ShowcaseID { get; set; }
	public int? ShowcaseItemID { get; set; }
	public bool NewProperties { get; set; }

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		Visible = Page.User.Identity.IsAuthenticated;
		if (Visible)
			Helpers.GetJSCode(uxJavaScripts);
		uxSavedSearchID.Visible = Editing;
		uxShowcaseItemID.Visible = ShowcaseItemID.HasValue;
		uxShowcaseItemID.Value = ShowcaseItemID.ToString();
		uxShowcaseID.Value = ShowcaseID.ToString();
		uxNewProperties.Value = NewProperties.ToString();
	}
}