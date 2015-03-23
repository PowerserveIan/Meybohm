using System;

public partial class _404 : BasePage
{
	public override void SetComponentInformation()
	{
	}

	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		if (Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite() != null)
			MasterPageFile = "~/microsite.master";
	}

    protected void Page_Load(object sender, EventArgs e)
    {
		Response.Status = "404 Not Found";
		Response.TrySkipIisCustomErrors = true;
    }
}