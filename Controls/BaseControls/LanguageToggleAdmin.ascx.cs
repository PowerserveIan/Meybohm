using System;
using System.Threading;
using System.Web.UI;
using BaseCode;

public partial class Controls_BaseControls_LanguageToggleAdmin : UserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			Language.DataSource = Classes.SiteLanguages.Language.LanguageGetByActive(true);
			Language.DataTextField = "Culture";
			Language.DataValueField = "CultureName";
			Language.DataBind();
			Language.SelectedValue = Thread.CurrentThread.CurrentCulture.Name;
		}
	}

	protected void ToggleLanguage(object sender, EventArgs e)
	{
		Response.Redirect(Request.QueryString.Duplicate().ChangeField("language", Language.SelectedItem.Value).WriteLocalPathWithQuery(Request.Url));
	}
}