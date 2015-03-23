using System;
using System.Web.Security;
using System.Web.UI;

public partial class logout : Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		FormsAuthentication.SignOut();
		Session.Clear();
		if (Request.Cookies["Microsite"] != null)
			Response.Cookies["Microsite"].Expires = DateTime.Now.AddDays(-2);
		if (!String.IsNullOrEmpty(Request.QueryString["ReturnURL"]))
			Response.Redirect(Request.QueryString["ReturnURL"]);
		Response.Redirect("~/");
	}
}