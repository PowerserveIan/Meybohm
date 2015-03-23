using System;
using System.Linq;
using System.Web.Security;

public partial class _Default : BasePage
{
	public override void SetComponentInformation()
	{
		ComponentName = "Content";
		ComponentAdminPage = "content-manager/content-manager.aspx";
		ComponentAdditionalLink = "~/admin/content-manager/content-manager-page.aspx?id=1&frontendView=true";
	}

	protected override void SetCssAndJs()
	{
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	protected override void OnInit(System.EventArgs e)
	{
		base.OnInit(e);
		Guid userID;
		if (!string.IsNullOrWhiteSpace(Request.QueryString["userID"]) && Guid.TryParse(Request.QueryString["userID"], out userID))
		{
			Classes.Media352_MembershipProvider.User userEntity = Classes.Media352_MembershipProvider.User.UserGetByChangePasswordID(userID).FirstOrDefault();
			if (userEntity != null)
			{
				userEntity.ChangePasswordID = null;
				userEntity.IsApproved = true;
				userEntity.Save();

				FormsAuthentication.SetAuthCookie(userEntity.Name, true);
				Response.Redirect("~/" + BaseCode.Helpers.GetLoginRedirectUrl(userEntity.Name));
			}
		}
		if (Request.Cookies["Microsite"] != null)
			Response.Redirect(Request.Cookies["Microsite"].Value + "/");
	}
}
