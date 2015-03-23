public partial class microsite_new_home : BaseCMSPage
{
	protected override void OnInit(System.EventArgs e)
	{
		base.OnInit(e);
		NewHomePage = true;
		uxSearchWidget.IsAiken = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite().Name == "Aiken";
	}
}