using System;
using System.Collections.Generic;
using System.Web.UI;
using Classes.SiteLanguages;

public partial class Controls_BaseControls_LanguageToggle : UserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			List<Language> languages = Language.LanguageGetByActive(true);
			if (languages.Count > 1)
			{
				uxLanguages.DataSource = languages;
				uxLanguages.DataBind();
			}
			else
				uxLanguages.Visible = false;
		}
	}
}