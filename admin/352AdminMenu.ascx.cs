using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Classes.ConfigurationSettings;

public partial class AdminMenu352Media : UserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string basePath = Server.MapPath("~/admin");
		List<SiteComponent> allComponents = SiteComponent.SiteComponentPage(0, 0, "", "MenuDisplayOrder", true);
		foreach (SiteComponent component in allComponents)
		{
			string formattedComponentName = string.Empty;
			foreach (char c in component.ComponentName)
			{
				if (Char.IsUpper(c))
					formattedComponentName += "-" + Char.ToLower(c);
				else
					formattedComponentName += c;
			}
			formattedComponentName = formattedComponentName.Replace("_", "").TrimStart('-');
			if (File.Exists(basePath + "\\" + formattedComponentName + "\\menu.ascx"))
			{
				Control subMenu = Page.LoadControl("~/admin/" + formattedComponentName + "/menu.ascx");
				foreach (Control c in subMenu.Controls)
				{
					if (c is HtmlAnchor && (((HtmlAnchor)c).HRef.Equals(Request.AppRelativeCurrentExecutionFilePath, StringComparison.OrdinalIgnoreCase)
							|| ((HtmlAnchor)c).HRef.Equals(Request.AppRelativeCurrentExecutionFilePath.Replace("-edit", ""), StringComparison.OrdinalIgnoreCase)
							|| ((HtmlAnchor)c).HRef.Equals(Request.AppRelativeCurrentExecutionFilePath.Replace("-page", ""), StringComparison.OrdinalIgnoreCase)))
						((HtmlAnchor)c).Attributes["class"] = ((HtmlAnchor)c).Attributes["class"] + (!String.IsNullOrEmpty(((HtmlAnchor)c).Attributes["class"]) ? " " : "") + "current";
				}
				ModuleMenus.Controls.Add(subMenu);
			}
		}
	}
}