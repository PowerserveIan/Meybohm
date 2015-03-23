using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_BaseControls_ZipcodeControl : UserControl
{
	private bool m_Required = true;

	public bool InternationalNumbers { get; set; }

	public bool ReadOnly { get; set; }

	public bool Required
	{
		get { return m_Required; }
		set
		{
			m_Required = value;
			uxZipcodeRFV.Enabled = uxZipcodeRFV.Visible = Required;
		}
	}

	public string RequiredMessage
	{
		set { uxZipcodeRFV.ErrorMessage = value; }
	}

	public string RequiredText
	{
		set { uxZipcodeRFV.Text = value; }
	}

	public string TextBoxClass { get; set; }

	public string ValidationGroup
	{
		set { uxZipcodeRFV.ValidationGroup = uxZipcodeREV.ValidationGroup = value; }
	}

	public string Text
	{
		get { return String.IsNullOrEmpty(uxZipcode.Text) ? "" : uxZipcode.Text.TrimEnd('-'); }
		set
		{
			Regex expression = new Regex("\\d{5}(-\\d{0,6})?");
			uxZipcode.Text = value;
			uxInternationalNumber.Checked = !(String.IsNullOrEmpty(value) || expression.IsMatch(value) || !InternationalNumbers);
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		BaseCode.Helpers.GetJSCode(uxJavaScripts);
		uxInternationalNumber.Visible = InternationalNumbers;
		uxZipcodeRFV.Enabled = uxZipcodeRFV.Visible = Required;
		uxZipcode.CssClass = TextBoxClass;
		uxZipcodeREV.ClientValidationFunction = "ClientValidate_" + ClientID;
		uxZipcode.ReadOnly = ReadOnly;
	}

	protected void uxZipcodeREV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		Regex expression = uxInternationalNumber.Checked ? new Regex("^\\w{0,12}$") : new Regex("\\d{5,}");
		args.IsValid = expression.IsMatch(uxZipcode.Text);
	}
}