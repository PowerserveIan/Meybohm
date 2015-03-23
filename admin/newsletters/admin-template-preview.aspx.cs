using System;
using System.Web.UI;

public partial class Admin_Newsletters_TemplatePreview : Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["newsletterPreview"] != null)
		{
			uxHtml.Text = Session["newsletterPreview"].ToString();
			//Session["newsletterPreview"] = null;
		}
	}
}