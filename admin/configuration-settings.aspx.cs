using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.ConfigurationSettings;

public partial class Admin_ConfigurationSettings : Page
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxConfigSettings.SaveClicked += uxConfigSettings_SaveClicked;
		PlaceHolder uxMoreSettings = (PlaceHolder)uxConfigSettings.FindControl("uxMoreSettings");

		List<SiteComponent> listOfComponents = SiteComponent.SiteComponentGetByComponentName(uxConfigSettings.Component);
		int componentID = listOfComponents[0].SiteComponentID;

		List<SiteSettings> customSettings = SiteSettings.GetAllWithTypeByCustom(componentID, true);

		DropDownList uxTimeZoneID = (DropDownList)uxMoreSettings.Controls[0].FindControl("uxTimeZoneID");
		uxTimeZoneID.DataSource = TimeZoneInfo.GetSystemTimeZones().OrderBy(tz => tz.DisplayName);
		uxTimeZoneID.DataValueField = "Id";
		uxTimeZoneID.DataTextField = "DisplayName";
		uxTimeZoneID.DataBind();
		(uxTimeZoneID).SelectedValue = customSettings.Find(s => s.Setting.Equals("defaultDisplayTimeZone")) != null ? customSettings.Find(s => s.Setting.Equals("defaultDisplayTimeZone")).Value : "";
	}

	private void uxConfigSettings_SaveClicked(object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			PlaceHolder uxMoreSettings = (PlaceHolder)uxConfigSettings.FindControl("uxMoreSettings");
			DropDownList uxTimeZoneID = (DropDownList)uxMoreSettings.Controls[0].FindControl("uxTimeZoneID");

			string[] values = { uxTimeZoneID.SelectedValue };

			List<SiteComponent> listOfComponents = SiteComponent.SiteComponentGetByComponentName(uxConfigSettings.Component);
			int componentID = listOfComponents[0].SiteComponentID;

			List<SiteSettings> customSettings = SiteSettings.GetAllWithTypeByCustom(componentID, true);
			for (int i = 0; i < customSettings.Count; i++)
			{
				if (customSettings[i].Value != values[i])
				{
					customSettings[i].Value = values[i];
					customSettings[i].Save();
				}
			}
		}
	}
}