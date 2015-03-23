public partial class microsite_home : BaseCMSPage
{
	protected override void OnInit(System.EventArgs e)
	{
		base.OnInit(e);
		NewHomePage = false;
		uxSearchWidget.IsAiken = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite().Name == "Aiken";
	}
}