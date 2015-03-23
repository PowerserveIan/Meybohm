using System;
using System.Web.UI;

public partial class SessionKeepAlive : Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		Response.ContentType = "text/html";
		Response.Write("alive");
	}
}