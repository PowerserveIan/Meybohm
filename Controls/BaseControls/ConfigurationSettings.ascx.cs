using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.ConfigurationSettings;

public delegate void SaveClickedEventHandler(object sender,
											 EventArgs e);

public partial class ConfigurationSettingsControl : UserControl
{
	/// <summary>
	/// Gives the page access to the listview
	/// </summary>
	public ListView CheckBoxListView;
	/// <summary>
	/// Gives the page access to the listview
	/// </summary>
	public ListView SettingsListView;

	private ITemplate m_AdditionalSettingsTemplate;

	[TemplateContainer(typeof(AdditionalSettingsContainer))]
	[PersistenceMode(PersistenceMode.InnerProperty)]
	public ITemplate AdditionalSettingsTemplate
	{
		get { return m_AdditionalSettingsTemplate; }
		set { m_AdditionalSettingsTemplate = value; }
	}

	/// <summary>
	/// The name of the component whose settings should be loaded
	/// </summary>
	public string Component { get; set; }

	public string CustomTitle { get; set; }

	/// <summary>
	/// Override the settings to pull from with this list
	/// </summary>
	public List<SiteSettings> CustomSettingList { get; set; }

	/// <summary>
	/// If this is set, the Save method of this control will not do anything but fire the SaveClick event.
	/// </summary>
	public bool LetPageHandleSaving { get; set; }

	public event SaveClickedEventHandler SaveClicked;

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		string masterPageScript = ((Literal)Page.Master.FindControl("uxJavaScripts")).Text;
		((Literal)Page.Master.FindControl("uxJavaScripts")).Text = masterPageScript + (String.IsNullOrEmpty(masterPageScript) ? "" : ",") + uxJavaScripts.Text;
		uxJavaScripts.Visible = false;
		CheckBoxListView = uxCheckBoxSettings;
		SettingsListView = uxSettingsListView;
		uxCheckBoxSettings.ItemDataBound += uxSettingsListView_ItemDataBound;
		uxSettingsListView.ItemDataBound += uxSettingsListView_ItemDataBound;
		uxSave.Click += uxSave_Click;

		if (m_AdditionalSettingsTemplate != null)
		{
			AdditionalSettingsContainer container = new AdditionalSettingsContainer();
			m_AdditionalSettingsTemplate.InstantiateIn(container);
			uxMoreSettings.Controls.Add(container);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			List<SiteComponent> listOfComponents = SiteComponent.SiteComponentGetByComponentName(Component);
			if (listOfComponents.Count > 0)
			{
				int componentID = listOfComponents[0].SiteComponentID;
				if (String.IsNullOrEmpty(CustomTitle))
					uxComponentName.Text = uxComponentName2.Text = uxComponentName3.Text = !String.IsNullOrEmpty(listOfComponents[0].DisplayName) ? listOfComponents[0].DisplayName : listOfComponents[0].ComponentName;
				else
					uxComponentName.Text = uxComponentName2.Text = uxComponentName3.Text = CustomTitle;
				List<SiteSettings> allSettings = CustomSettingList ?? SiteSettings.GetAllWithTypeByCustom(componentID, false);
				uxCheckBoxSettings.DataSource = allSettings.Where(s => s.Type == "bool");
				uxCheckBoxSettings.DataBind();
				uxSettingsListView.DataSource = allSettings.Where(s => s.Type != "bool");
				uxSettingsListView.DataBind();
			}
		}
		SuccessMessage.Visible = false;

		//So that an error and success message will not show at the same time.
		Page.ClientScript.RegisterClientScriptBlock(typeof(string), "ClearSuccess", @"
<script type=""text/javascript"" language=""javascript"">
	//<![CDATA[
	function ClearSuccess(sender, args){
		if (document.getElementById('successMessage')!=undefined) {
			document.getElementById('successMessage').style.display='none';
		}
		args.IsValid = true;
	}
	//]]>
</script>
");
	}

	private void uxSettingsListView_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		RangeValidator uxSettingRangeVal = (RangeValidator)e.Item.FindControl("uxSettingRangeVal");
		RegularExpressionValidator uxSettingValueMaxLengthVal = (RegularExpressionValidator)e.Item.FindControl("uxSettingValueMaxLengthVal");
		TextBox uxSettingValue = (TextBox)e.Item.FindControl("uxSettingValue");
		HiddenField uxDataType = (HiddenField)e.Item.FindControl("uxDataType");
		HiddenField uxOptions = (HiddenField)e.Item.FindControl("uxOptions");
		Label uxValueRequiredToPassValidation = (Label)e.Item.FindControl("uxValueRequiredToPassValidation");
		HtmlGenericControl uxColumnContainer = (HtmlGenericControl)e.Item.FindControl("uxColumnContainer");
		switch (uxDataType.Value)
		{
			case "string":
				uxSettingValue.TextMode = TextBoxMode.MultiLine;
				uxSettingValue.Rows = 5;
				uxSettingValue.Columns = 40;
				uxSettingValueMaxLengthVal.Enabled = true;
				uxColumnContainer.Attributes["class"] = "formWhole";
				break;
			case "int":
				uxSettingRangeVal.Type = ValidationDataType.Integer;
				uxSettingRangeVal.MinimumValue = "0";
				uxSettingRangeVal.Enabled = true;
				break;
			case "tinyint":
				uxSettingRangeVal.Type = ValidationDataType.Integer;
				uxSettingRangeVal.Enabled = true;
				uxSettingRangeVal.MinimumValue = "0";
				uxSettingRangeVal.MaximumValue = "255";
				uxSettingRangeVal.ErrorMessage = uxSettingRangeVal.ErrorMessage.Replace("numeric and", "numeric,").TrimEnd('.') + ", and less than 256.";
				uxValueRequiredToPassValidation.Text = "(must be between 0 and 255)";
				uxValueRequiredToPassValidation.Visible = true;
				break;
			case "bool":
				break;
			case "date":
				Controls_BaseControls_DateTimePicker uxSettingDate = (Controls_BaseControls_DateTimePicker)e.Item.FindControl("uxSettingDate");
				DateTime theDate;
				if (DateTime.TryParse(uxSettingValue.Text, out theDate))
					uxSettingDate.SelectedDate = Helpers.ConvertUTCToClientTime(theDate);
				uxSettingDate.Visible = true;
				uxSettingValue.Visible = false;
				break;
			case "double":
				uxSettingRangeVal.Type = ValidationDataType.Double;
				uxSettingRangeVal.MinimumValue = (0.01).ToString();
				uxSettingRangeVal.Enabled = true;
				break;
			case "enum":
				DropDownList uxSettingDDL = (DropDownList)e.Item.FindControl("uxSettingDDL");
				string[] options = uxOptions.Value.Split(';');
				foreach (string s in options)
				{
					if (s.Contains(':'))
						uxSettingDDL.Items.Add(new ListItem(s.Split(':')[1], s.Split(':')[0]));
					else
						uxSettingDDL.Items.Add(new ListItem(s, s));
				}
				uxSettingDDL.SelectedValue = uxSettingValue.Text;
				uxSettingDDL.Visible = true;
				uxSettingValue.Visible = false;
				break;
			case "html":
				PlaceHolder uxRadEditorPH = (PlaceHolder)e.Item.FindControl("uxRadEditorPH");
				uxRadEditorPH.Visible = true;
				uxSettingValue.Visible = false;
				uxSettingValueMaxLengthVal.Enabled = true;
				uxSettingValueMaxLengthVal.ControlToValidate = "uxSettingHTML";
				uxColumnContainer.Attributes["class"] = "formWhole";
				break;
			case "email":
				RegularExpressionValidator uxSettingValueEmailVal = (RegularExpressionValidator)e.Item.FindControl("uxSettingValueEmailVal");
				uxSettingValueEmailVal.Enabled = true;
				uxSettingValueMaxLengthVal.Enabled = true;
				uxSettingValue.MaxLength = 382;
				break;
			case "list":
				CheckBoxList uxSettingList = (CheckBoxList)e.Item.FindControl("uxSettingList");
				string[] selectedOptions = uxSettingValue.Text.Split(';');
				string[] listOptions = uxOptions.Value.Split(';');
				foreach (string s in listOptions)
				{
					ListItem current = new ListItem(s, s);
					current.Selected = selectedOptions.Contains(s);
					uxSettingList.Items.Add(current);
				}
				uxSettingList.Visible = true;
				uxSettingValue.Visible = false;
				break;
		}
	}

	protected void uxSettingListRFV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = Helpers.IsAListItemSelected((CheckBoxList)((CustomValidator)source).Parent.FindControl("uxSettingList"));
	}

	private void uxSave_Click(object sender, EventArgs e)
	{
		//This is an excessive validation call, but if its not there, 
		//clicking save after blanking out a textbox removes the validation
		if (Page.IsValid)
		{
			if (!LetPageHandleSaving)
			{
				List<SiteSettings> listOfChangedSettings = new List<SiteSettings>();
				foreach (ListViewDataItem theRow in uxCheckBoxSettings.Items)
				{
					listOfChangedSettings = SaveRow(theRow, listOfChangedSettings);
				}
				foreach (ListViewDataItem theRow in uxSettingsListView.Items)
				{
					listOfChangedSettings = SaveRow(theRow, listOfChangedSettings);
				}
				//Need to do this or else you'll be updating the GetSettingKeyValue pair on each update
				if (listOfChangedSettings.Count > 0)
				{
					foreach (SiteSettings setting in listOfChangedSettings)
					{
						setting.Save();
					}
					SiteSettings.PopulateSettingsCache();
				}
			}
			Helpers.PageView.Anchor(Page, Helpers.PageView.PageAnchors.center);
			if (SaveClicked != null)
				SaveClicked(sender, null);

			SuccessMessage.Visible = true;
		}
	}

	List<SiteSettings> SaveRow(ListViewDataItem theRow, List<SiteSettings> listOfChangedSettings)
	{
		int uxSettingID = Convert.ToInt32(((HiddenField)theRow.FindControl("uxSettingID")).Value);
		HiddenField uxDataType = (HiddenField)theRow.FindControl("uxDataType");
		HiddenField uxSettingName = (HiddenField)theRow.FindControl("uxSettingName");
		string completeSettingName = Component + "_" + uxSettingName.Value;
		switch (uxDataType.Value)
		{
			case "string":
			case "int":
			case "tinyint":
			case "double":
			case "email":
				TextBox uxSettingValue = (TextBox)theRow.FindControl("uxSettingValue");
				if (SiteSettings.GetSettingKeyValuePair()[completeSettingName] != uxSettingValue.Text)
				{
					SiteSettings changedSetting = SiteSettings.GetByID(uxSettingID);
					changedSetting.Value = uxSettingValue.Text;
					changedSetting.DateLastModified = DateTime.UtcNow;
					listOfChangedSettings.Add(changedSetting);
				}
				break;
			case "bool":
				CheckBox uxSettingCheckBox = (CheckBox)theRow.FindControl("uxSettingCheckBox");
				if (SiteSettings.GetSettingKeyValuePair()[completeSettingName] != uxSettingCheckBox.Checked.ToString())
				{
					SiteSettings changedSetting = SiteSettings.GetByID(uxSettingID);
					changedSetting.Value = uxSettingCheckBox.Checked.ToString();
					changedSetting.DateLastModified = DateTime.UtcNow;
					listOfChangedSettings.Add(changedSetting);
				}
				break;
			case "enum":
				DropDownList uxSettingDDL = (DropDownList)theRow.FindControl("uxSettingDDL");
				if (SiteSettings.GetSettingKeyValuePair()[completeSettingName] != uxSettingDDL.SelectedValue)
				{
					SiteSettings changedSetting = SiteSettings.GetByID(uxSettingID);
					changedSetting.Value = uxSettingDDL.SelectedValue;
					changedSetting.DateLastModified = DateTime.UtcNow;
					listOfChangedSettings.Add(changedSetting);
				}
				break;
			case "html":
				TextBox uxSettingHTML = (TextBox)theRow.FindControl("uxSettingHTML");
				Label uxSettingHTMLText = (Label)theRow.FindControl("uxSettingHTMLText");
				HiddenField uxSettingHTMLEdited = (HiddenField)theRow.FindControl("uxSettingHTMLEdited");
				if (SiteSettings.GetSettingKeyValuePair()[completeSettingName] != Helpers.ReplaceAbsolutePathWithRoot(HttpUtility.HtmlDecode(uxSettingHTML.Text)) && Convert.ToBoolean(uxSettingHTMLEdited.Value))
				{
					SiteSettings changedSetting = SiteSettings.GetByID(uxSettingID);
					changedSetting.Value = Helpers.ReplaceAbsolutePathWithRoot(HttpUtility.HtmlDecode(uxSettingHTML.Text));
					changedSetting.DateLastModified = DateTime.UtcNow;
					listOfChangedSettings.Add(changedSetting);
				}
				uxSettingHTML.Text = uxSettingHTMLText.Text = Helpers.ReplaceRootWithRelativePath(Helpers.ReplaceAbsolutePathWithRoot(HttpUtility.HtmlDecode(uxSettingHTML.Text)), 2);
				break;
			case "date":
				Controls_BaseControls_DateTimePicker uxSettingDate = (Controls_BaseControls_DateTimePicker)theRow.FindControl("uxSettingDate");
				if (SiteSettings.GetSettingKeyValuePair()[completeSettingName] != (uxSettingDate.SelectedDate.HasValue ? Helpers.ConvertClientTimeToUTC(uxSettingDate.SelectedDate.Value).ToShortDateString() : ""))
				{
					SiteSettings changedSetting = SiteSettings.GetByID(uxSettingID);
					changedSetting.Value = (uxSettingDate.SelectedDate.HasValue ? Helpers.ConvertClientTimeToUTC(uxSettingDate.SelectedDate.Value).ToShortDateString() : "");
					changedSetting.DateLastModified = DateTime.UtcNow;
					listOfChangedSettings.Add(changedSetting);
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
				if (SiteSettings.GetSettingKeyValuePair()[completeSettingName] != settingValue)
				{
					SiteSettings changedSetting = SiteSettings.GetByID(uxSettingID);
					changedSetting.Value = settingValue;
					changedSetting.DateLastModified = DateTime.UtcNow;
					listOfChangedSettings.Add(changedSetting);
				}
				break;
		}
		return listOfChangedSettings;
	}

	#region Nested type: AdditionalSettingsContainer

	public class AdditionalSettingsContainer : Control, INamingContainer
	{
		internal AdditionalSettingsContainer()
		{
		}
	}

	#endregion
}