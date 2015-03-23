using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetOpenAuth.AspNet;

public partial class Controls_BaseControls_ExternalLoginProviders : UserControl
{
	public string ReturnUrl { get; set; }

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		if (!IsPostBack)
		{
			uxProviders.DataSource = AuthConfig.GetClients();
			uxProviders.DataBind();
		}
	}

	protected void ExternalLogin_Command(object sender, CommandEventArgs e)
	{
		IAuthenticationClient client = AuthConfig.GetClientByProviderName(e.CommandArgument.ToString());
		client.RequestAuthentication(new HttpContextWrapper(HttpContext.Current), new Uri(AuthConfig.ExternalLoginUrl + "?" + AuthConfig.ProviderQueryString + "=" + client.ProviderName + (!String.IsNullOrWhiteSpace(ReturnUrl) ? "&ReturnURL=" + HttpUtility.UrlEncode(ResolveUrl(ReturnUrl)) : string.Empty)));
	}
}