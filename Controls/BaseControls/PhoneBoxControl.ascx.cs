using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_BaseControls_PhoneBoxControl : UserControl
{
	private bool m_Required = true;
	private bool m_ShowExtension = true;

	public bool InternationalNumbers { get; set; }

	public bool Required
	{
		get { return m_Required; }
		set
		{
			m_Required = value;
			uxPhoneBoxRFV.Enabled = uxPhoneBoxRFV.Visible = Required;
		}
	}

	public string RequiredMessage
	{
		set { uxPhoneBoxRFV.ErrorMessage = value; }
	}

	public string RequiredText
	{
		set { uxPhoneBoxRFV.Text = value; }
	}

	public bool ShowExtension
	{
		get { return m_ShowExtension; }
		set { m_ShowExtension = value; }
	}

	public string TextBoxClass { get; set; }

	public string ValidationGroup
	{
		set { uxPhoneBoxRFV.ValidationGroup = uxPhoneBoxREV.ValidationGroup = value; }
	}

	public string Text
	{
		get { return String.IsNullOrEmpty(uxPhoneBox.Text) ? "" : uxPhoneBox.Text.EndsWith(" x ") ? uxPhoneBox.Text.Replace(" x ", "") : uxPhoneBox.Text; }
		set
		{
			Regex expression = new Regex("((\\(\\d{3}\\))\\d{3}-\\d{4})+(x\\d{0,5})?");
			uxPhoneBox.Text = value;
			uxInternationalNumber.Checked = !(String.IsNullOrEmpty(value) || expression.IsMatch(value) || !InternationalNumbers);
		}
	}

	public TextBox TextBoxControl
	{
		get { return uxPhoneBox; }
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		BaseCode.Helpers.GetJSCode(uxJavaScripts);
		uxInternationalNumber.Visible = InternationalNumbers;
		uxPhoneBoxRFV.Enabled = uxPhoneBoxRFV.Visible = Required;
		uxPhoneBox.CssClass = TextBoxClass;
		uxPhoneBoxREV.ClientValidationFunction = "ClientValidate_" + ClientID;
	}

	protected void uxPhoneBoxREV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		Regex expression = uxInternationalNumber.Checked ? new Regex("^(\\+)?((\\s)?(\\()?(\\))?([0-9x])?(\\-)?)+$") : new Regex("^\\(\\d{3}\\)\\d{3}-\\d{4}");
		args.IsValid = expression.IsMatch(uxPhoneBox.Text);
	}
}