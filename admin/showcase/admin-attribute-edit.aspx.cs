using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Showcase;

public partial class Admin_AdminShowcaseAttributeEdit : BaseEditPage
{
	public ShowcaseAttribute ShowcaseAttributeEntity { get; set; }

	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-attribute.aspx";
		m_ClassName = "Attribute";
		base.OnInit(e);
		m_SaveAndAddNewButton.Visible = !ShowcaseHelpers.IsCurrentShowcaseMLS();
		uxShowcaseFilterNumericVal.ServerValidate += uxShowcaseFilterNumericVal_ServerValidate;
		uxShowcaseRangeSliderVal.ServerValidate += uxShowcaseRangeSliderVal_ServerValidate;
		uxTitleUniqueValidator.ServerValidate += uxTitleUniqueValidator_ServerValidate;
		uxShowcaseDistanceVal.ServerValidate += uxShowcaseDistanceVal_ServerValidate;
		uxShowcaseOnlyOneDistanceVal.ServerValidate += uxShowcaseOnlyOneDistanceVal_ServerValidate;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (!ShowcaseHelpers.GetCurrentShowcaseID().HasValue)
				Response.Redirect("~/admin/showcase/admin-showcases.aspx");

			if (Settings.EnableFilters)
			{
				uxShowcaseFilterID.DataSource = Filter.GetAll();
				uxShowcaseFilterID.DataTextField = "Type";
				uxShowcaseFilterID.DataValueField = "ShowcaseFilterID";
				uxShowcaseFilterID.DataBind();
			}

			uxRadioButtonGrid.DataSource = new[] { "Value 1", "Value 2", "Value 3" };
			uxRadioButtonGrid.DataBind();

			List<int> oneTo100 = new List<int>();
			for (int i = 0; i < 101; i++)
			{
				oneTo100.Add(i);
			}
			uxRangeSlider1.DataSource = uxRangeSlider2.DataSource = oneTo100;
			uxRangeSlider1.DataBind();
			uxRangeSlider2.DataBind();

			if (EntityId > 0)
			{
				ShowcaseAttributeEntity = ShowcaseAttribute.GetByID(EntityId);
				if (ShowcaseAttributeEntity == null || (!ShowcaseHelpers.IsShowcaseAdmin() && !ShowcaseUser.ShowcaseUserGetByShowcaseID(ShowcaseAttributeEntity.ShowcaseID).Exists(s => s.UserID == Helpers.GetCurrentUserID())))
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
				uxValuesPlaceHolder.Visible = !uxNumeric.Checked || uxShowcaseFilterID.SelectedItem.Text.Contains("Distance");
				uxEditValues.NavigateUrl = "~/admin/showcase/admin-attribute-value.aspx?FilterShowcaseAttributeValueShowcaseAttributeID=" + EntityId;
			}
			else
				NewRecord = true;
		}

		uxFilterPlaceHolder.Visible = Settings.EnableFilters;
	}

	protected override void Save()
	{
		uxHeaderNoPreferenceRFV.Enabled =
		uxHeaderNoRFV.Enabled =
		uxHeaderYesRFV.Enabled = Convert.ToInt32(uxShowcaseFilterID.SelectedValue) == (int)FilterTypes.RadioButtonGrid;
		Validate();
		if (IsValid)
		{
			ShowcaseAttributeEntity = EntityId > 0 ? ShowcaseAttribute.GetByID(EntityId) : new ShowcaseAttribute();
			ShowcaseAttributeEntity.Active = uxActive.Checked;
			//Wipe existing values if non-numeric to numeric
			if (!NewRecord && !ShowcaseAttributeEntity.Numeric && uxNumeric.Checked)
			{
				List<ShowcaseAttributeValue> attributeValues = ShowcaseAttributeValue.ShowcaseAttributeValueGetByShowcaseAttributeID(ShowcaseAttributeEntity.ShowcaseAttributeID);
				foreach (ShowcaseAttributeValue value in attributeValues)
				{
					int temp;
					if (!Int32.TryParse(value.Value, out temp))
						value.Delete();
				}
			}
			ShowcaseAttributeEntity.ImportItemAttribute = true;
			ShowcaseAttributeEntity.Numeric = uxNumeric.Checked;
			int oldFilter = ShowcaseAttributeEntity.ShowcaseFilterID;
			if (Settings.EnableFilters)
				ShowcaseAttributeEntity.ShowcaseFilterID = Convert.ToInt32(uxShowcaseFilterID.SelectedValue);
			else
				ShowcaseAttributeEntity.ShowcaseFilterID = Filter.FilterGetByType("Slider").FirstOrDefault().ShowcaseFilterID;
			ShowcaseAttributeEntity.Title = uxTitle.Text;
			if (ShowcaseAttributeEntity.IsNewRecord)
				ShowcaseAttributeEntity.DisplayOrder = (short)(Helpers.GetMaxDisplayOrder("ShowcaseAttribute", "ShowcaseAttributeID", "ImportItemAttribute", 1, "ShowcaseID", ShowcaseHelpers.GetCurrentShowcaseID().Value) + 1);

			ShowcaseAttributeEntity.ShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID().Value;

			ShowcaseAttributeEntity.MinimumValue = ShowcaseAttributeEntity.Numeric && !String.IsNullOrEmpty(uxMinimumValue.Text) ? (decimal?)Convert.ToDecimal(uxMinimumValue.Text) : null;
			ShowcaseAttributeEntity.MaximumValue = ShowcaseAttributeEntity.Numeric && !String.IsNullOrEmpty(uxMaximumValue.Text) ? (decimal?)Convert.ToDecimal(uxMaximumValue.Text) : null;
			ShowcaseAttributeEntity.SingleItemValue = uxSingleItemValue.Checked;
			ShowcaseAttributeEntity.Save();
			EntityId = ShowcaseAttributeEntity.ShowcaseAttributeID;
			uxValuesPlaceHolder.Visible = !uxNumeric.Checked || uxShowcaseFilterID.SelectedItem.Text.Contains("Distance");

			if (ShowcaseAttributeEntity.ShowcaseFilterID == (int)FilterTypes.RadioButtonGrid)
			{
				List<ShowcaseAttributeHeader> headers = ShowcaseAttributeHeader.ShowcaseAttributeHeaderGetByShowcaseAttributeID(ShowcaseAttributeEntity.ShowcaseAttributeID);
				if (headers.Count != 3)
				{
					//Clear the data and insert again
					headers.ForEach(h => h.Delete());
					ShowcaseAttributeHeader headerEntity = new ShowcaseAttributeHeader();
					headerEntity.NoPreferenceColumn = false;
					headerEntity.ShowcaseAttributeID = ShowcaseAttributeEntity.ShowcaseAttributeID;
					headerEntity.Text = uxHeaderYes.Text;
					headerEntity.Save();

					headerEntity.Text = uxHeaderNo.Text;
					headerEntity.ShowcaseAttributeHeaderID = 0;
					headerEntity.Save();

					headerEntity.NoPreferenceColumn = true;
					headerEntity.Text = uxHeaderNoPreference.Text;
					headerEntity.ShowcaseAttributeHeaderID = 0;
					headerEntity.Save();
				}
				else
				{
					headers[0].Text = uxHeaderYes.Text;
					headers[1].Text = uxHeaderNo.Text;
					headers[2].Text = uxHeaderNoPreference.Text;
					headers.ForEach(h => h.Save());
				}
			}
			else if (oldFilter != ShowcaseAttributeEntity.ShowcaseFilterID && oldFilter == (int)FilterTypes.RadioButtonGrid)
				//Delete old headers
				ShowcaseAttributeHeader.ShowcaseAttributeHeaderGetByShowcaseAttributeID(ShowcaseAttributeEntity.ShowcaseAttributeID).ForEach(h => h.Delete());

			if (NewRecord)
				uxEditValues.NavigateUrl = "~/admin/showcase/admin-attribute-value.aspx?FilterShowcaseAttributeValueShowcaseAttributeID=" + ShowcaseAttributeEntity.ShowcaseAttributeID;
			m_ClassTitle = ShowcaseAttributeEntity.Title;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = ShowcaseAttributeEntity.Active;
		uxMLSAttributeName.Text = ShowcaseAttributeEntity.MLSAttributeName;
		uxNumeric.Checked = ShowcaseAttributeEntity.Numeric;
		if (uxShowcaseFilterID.Items.FindByValue(ShowcaseAttributeEntity.ShowcaseFilterID.ToString()) != null)
			uxShowcaseFilterID.SelectedValue = ShowcaseAttributeEntity.ShowcaseFilterID.ToString();
		uxTitle.Text = ShowcaseAttributeEntity.Title;
		if (ShowcaseAttributeEntity.ShowcaseFilterID == (int)FilterTypes.RadioButtonGrid)
			BindAttributeHeaders();
		uxMLSNamePH.Visible = ShowcaseHelpers.IsCurrentShowcaseMLS() && !String.IsNullOrEmpty(ShowcaseAttributeEntity.MLSAttributeName);
		if (ShowcaseAttributeEntity.Numeric)
		{
			uxMinimumValue.Text = ShowcaseAttributeEntity.MinimumValue.ToString().Replace(".00", "");
			uxMaximumValue.Text = ShowcaseAttributeEntity.MaximumValue.ToString().Replace(".00", "");
		}

		uxNumeric.Enabled = !uxMLSNamePH.Visible;
		uxSingleItemValue.Checked = ShowcaseAttributeEntity.SingleItemValue;
	}

	void BindAttributeHeaders()
	{
		List<ShowcaseAttributeHeader> headers = ShowcaseAttributeHeader.ShowcaseAttributeHeaderGetByShowcaseAttributeID(ShowcaseAttributeEntity.ShowcaseAttributeID).OrderBy(h => h.NoPreferenceColumn).ToList();
		if (headers.Count > 0)
		{
			uxHeaderNoPreference.Text = headers.Find(h => h.NoPreferenceColumn).Text;
			if (headers.Count(h => !h.NoPreferenceColumn) >= 1)
				uxHeaderYes.Text = headers[0].Text;
			if (headers.Count(h => !h.NoPreferenceColumn) >= 2)
				uxHeaderNo.Text = headers[1].Text;
		}
	}

	private void uxTitleUniqueValidator_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !ShowcaseAttribute.ShowcaseAttributeGetByTitle(uxTitle.Text).Any(t => EntityId != t.ShowcaseAttributeID && t.ShowcaseID == ShowcaseHelpers.GetCurrentShowcaseID().Value);
	}

	private void uxShowcaseRangeSliderVal_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = uxNumeric.Checked || (!uxNumeric.Checked && !uxShowcaseFilterID.SelectedItem.Text.Contains("Range Slider"));
	}

	private void uxShowcaseFilterNumericVal_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !uxNumeric.Checked || uxShowcaseFilterID.SelectedItem.Text.Contains("Slider") ||
					   uxShowcaseFilterID.SelectedItem.Text.Contains("Distance") || uxShowcaseFilterID.SelectedItem.Text.Contains("Range");
	}

	void uxShowcaseDistanceVal_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = uxNumeric.Checked || (!uxNumeric.Checked && !uxShowcaseFilterID.SelectedItem.Text.Contains("Distance"));
	}

	void uxShowcaseOnlyOneDistanceVal_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !uxShowcaseFilterID.SelectedItem.Text.Contains("Distance") || (ShowcaseAttribute.ShowcaseAttributeGetByShowcaseID(ShowcaseHelpers.GetCurrentShowcaseID().Value).Count(a => (a.ShowcaseFilterID == (int)FilterTypes.Distance || a.ShowcaseFilterID == (int)FilterTypes.DistanceRange) && a.ShowcaseAttributeID != EntityId) == 0);
	}
}