using System;
using System.Web.UI;
using Classes.Media352_MembershipProvider;

public partial class register : BasePage
{
	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		if (Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite() != null)
			MasterPageFile = "~/microsite.master";
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxRegister.ShowProfile = Settings.UserManager == UserManagerType.Complex;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Members Area";
		ComponentAdminPage = "media352-membership-provider/admin-user.aspx";
	}
}