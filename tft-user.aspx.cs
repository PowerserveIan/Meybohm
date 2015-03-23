using System;
using System.Linq;
using System.Web;
using System.Web.Security;

public partial class tft_user : System.Web.UI.Page
{
	protected override void OnLoad(EventArgs e)
	{
		if ((!Request.UserHostAddress.Equals("::1") && !Request.UserHostAddress.StartsWith("10.") && !Request.UserHostAddress.StartsWith("fe80::")) || !System.Web.HttpContext.Current.IsDebuggingEnabled)
			Response.Redirect("~/");
		base.OnLoad(e);
		if (!IsPostBack)
		{
			uxUsers.DataSource = Classes.Media352_MembershipProvider.User.UserPage(0, 0, "352", "Name", true, new Classes.Media352_MembershipProvider.User.Filters { FilterUserDeleted = false.ToString(), FilterUserIsApproved = true.ToString() }).Where(u => u.Name.StartsWith("352"));
			uxUsers.DataBind();

			uxCurrentlyLoggedInAs.Visible = Page.User.Identity.IsAuthenticated;
			uxCurrentlyLoggedInAs.Text = "You are currently logged in as: " + Page.User.Identity.Name;
		}
	}

	protected void Login_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
	{
		FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, e.CommandArgument.ToString(), DateTime.Now, DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes), true, e.CommandArgument.ToString(), FormsAuthentication.FormsCookiePath);
		string hashCookies = FormsAuthentication.Encrypt(ticket);
		HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
		cookie.Domain = FormsAuthentication.CookieDomain;
		Response.Cookies.Add(cookie);
		if (!String.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
			Response.Redirect(Request.QueryString["ReturnUrl"]);
		Response.Redirect("~/");
	}
}