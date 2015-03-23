using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.ConfigurationSettings;
using Classes.Showcase;
using Settings = Classes.Showcase.Settings;

public partial class Admin_ConfigurationSettings : Page
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxConfigSettings.SaveClicked += uxConfigSettings_SaveClicked;
		if (!IsPostBack)
		{
			int? currentShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID();

			if (currentShowcaseID.HasValue)
			{
				List<ShowcaseSiteSettings> showcaseSettings = ShowcaseSiteSettings.ShowcaseSiteSettingsGetByShowcaseID(currentShowcaseID.Value);
				List<SiteSettings> siteSettings;
				int componentID;
				List<SiteComponent> listOfComponents = SiteComponent.SiteComponentGetByComponentName(uxConfigSettings.Component);
				if (listOfComponents.Count > 0)
				{
					componentID = listOfComponents[0].SiteComponentID;
					siteSettings = SiteSettings.GetAllWithTypeByCustom(componentID, false);
					foreach (SiteSettings siteSetting in siteSettings)
					{
						if (showcaseSettings.Find(s => s.SiteSettingsID == siteSetting.SiteSettingsID) != null)
							siteSetting.Value = showcaseSettings.Find(s => s.SiteSettingsID == siteSetting.SiteSettingsID).Value;
					}
					uxConfigSettings.CustomSettingList = siteSettings;
				}
			}
			else
				Response.Redirect("~/admin/showcase/admin-showcases.aspx");

			uxConfigSettings.CustomTitle = Showcases.GetByID(currentShowcaseID.Value).Title;
		}
	}

	private void uxConfigSettings_SaveClicked(object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			int? currentShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID();
			if (!currentShowcaseID.HasValue)
				Response.Redirect("~admin/showcase/admin-showcases.aspx");
			List<ShowcaseSiteSettings> showcaseSettings = ShowcaseSiteSettings.ShowcaseSiteSettingsGetByShowcaseID(currentShowcaseID.Value);
			bool settingChanged = false;
			foreach (ListViewDataItem theRow in uxConfigSettings.CheckBoxListView.Items)
			{
				settingChanged = SaveRow(theRow, showcaseSettings, currentShowcaseID) || settingChanged;
			}
			foreach (ListViewDataItem theRow in uxConfigSettings.SettingsListView.Items)
			{
				settingChanged = SaveRow(theRow, showcaseSettings, currentShowcaseID) || settingChanged;
			}
			if (settingChanged)
			{
				ShowcaseSiteSettings.PopulateSettingsCache(currentShowcaseID.Value);
				SiteSettings.PopulateSettingsCache();
			}
		}
	}

	bool SaveRow(ListViewDataItem theRow, List<ShowcaseSiteSettings> showcaseSettings, int? currentShowcaseID)
	{
		int uxSettingID = Convert.ToInt32(((HiddenField)theRow.FindControl("uxSettingID")).Value);
		HiddenField uxDataType = (HiddenField)theRow.FindControl("uxDataType");
		ShowcaseSiteSettings currentSetting;
		if (showcaseSettings.Exists(s => s.SiteSettingsID == uxSettingID))
			currentSetting = showcaseSettings.Find(s => s.SiteSettingsID == uxSettingID);
		else
		{
			currentSetting = new ShowcaseSiteSettings();
			currentSetting.ShowcaseID = currentShowcaseID.Value;
			currentSetting.SiteSettingsID = uxSettingID;
		}
		switch (uxDataType.Value)
		{
			case "string":
			case "int":
			case "tinyint":
			case "double":
			case "email":
				TextBox uxSettingValue = (TextBox)theRow.FindControl("uxSettingValue");
				if (currentSetting.Value != uxSettingValue.Text)
				{
					currentSetting.Value = uxSettingValue.Text;
					currentSetting.Save();
					return true;
				}
				break;
			case "bool":
				CheckBox uxSettingCheckBox = (CheckBox)theRow.FindControl("uxSettingCheckBox");
				if (currentSetting.Value != uxSettingCheckBox.Checked.ToString())
				{
					currentSetting.Value = uxSettingCheckBox.Checked.ToString();
					currentSetting.Save();
					return true;
				}
				break;
			case "enum":
				DropDownList uxSettingDDL = (DropDownList)theRow.FindControl("uxSettingDDL");
				if (currentSetting.Value != uxSettingDDL.SelectedValue)
				{
					currentSetting.Value = uxSettingDDL.SelectedValue;
					currentSetting.Save();
					return true;
				}
				break;
			case "html":
				TextBox uxSettingHTML = (TextBox)theRow.FindControl("uxSettingHTML");
				Label uxSettingHTMLText = (Label)theRow.FindControl("uxSettingHTMLText");
				HiddenField uxSettingHTMLEdited = (HiddenField)theRow.FindControl("uxSettingHTMLEdited");
				if (currentSetting.Value != Helpers.ReplaceAbsolutePathWithRoot(HttpUtility.HtmlDecode(uxSettingHTML.Text)) && Convert.ToBoolean(uxSettingHTMLEdited.Value))
				{
					currentSetting.Value = Helpers.ReplaceAbsolutePathWithRoot(HttpUtility.HtmlDecode(uxSettingHTML.Text));
					currentSetting.Save();
					return true;
				}
				uxSettingHTML.Text = uxSettingHTMLText.Text = Helpers.ReplaceRootWithRelativePath(Helpers.ReplaceAbsolutePathWithRoot(HttpUtility.HtmlDecode(uxSettingHTML.Text)), 2);
				break;
			case "date":
				Controls_BaseControls_DateTimePicker uxSettingDate = (Controls_BaseControls_DateTimePicker)theRow.FindControl("uxSettingDate");
				if (currentSetting.Value != (uxSettingDate.SelectedDate.HasValue ? Helpers.ConvertClientTimeToUTC(uxSettingDate.SelectedDate.Value).ToShortDateString() : ""))
				{
					currentSetting.Value = (uxSettingDate.SelectedDate.HasValue ? Helpers.ConvertClientTimeToUTC(uxSettingDate.SelectedDate.Value).ToShortDateString() : "");
					currentSetting.Save();
					return true;
				}
				break;
			case "list":
				CheckBoxList uxSettingList = (CheckBoxList)theRow.FindControl("uxSettingList");
				string settingValue = string.Empty;
				foreach (ListItem item in uxSettingList.Items)
				{
					if (item.Selected)
						settingValue += item.Text + ";";
				}
				settingValue = settingValue.TrimEnd(';');
				if (currentSetting.Value != settingValue)
				{
					currentSetting.Value = settingValue;
					currentSetting.Save();
					return true;
				}
				break;
		}
		return false;
	}
}