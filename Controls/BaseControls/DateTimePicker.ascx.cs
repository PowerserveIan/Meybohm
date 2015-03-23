using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaseCode;

public partial class Controls_BaseControls_DateTimePicker : UserControl
{
	public enum Picker
	{
		DateOnly,
		TimeOnly,
		DateTime
	}

	private bool m_Enabled = true;
	private Picker m_PickerStyle = Picker.DateOnly;
	private string m_DateFormat = "MM/dd/yyyy";
	private string m_TimeFormat = "hh:mm tt";
	protected string m_DotsToRoot;

	public bool AutoPostBack { get; set; }

	public string DateFormat
	{
		get { return m_DateFormat; }
		set { m_DateFormat = value; }
	}

	public bool Enabled
	{
		get { return m_Enabled; }
		set { m_Enabled = value; }
	}

	public DateTime? MinDate { get; set; }
	public DateTime? MaxDate { get; set; }

	public Picker PickerStyle
	{
		get { return m_PickerStyle; }
		set { m_PickerStyle = value; }
	}

	public string TextBoxCssClass
	{
		get { return uxDate.CssClass; }
		set { uxDate.CssClass = value; }
	}

	public TextBox TextBoxControl
	{
		get { return uxDate; }
		set { uxDate = value; }
	}

	public string TimeFormat
	{
		get { return m_TimeFormat; }
		set { m_TimeFormat = value; }
	}

	public DateTime? SelectedDate
	{
		get
		{
			DateTime temp;
			if (String.IsNullOrEmpty(uxDate.Text) || !DateTime.TryParse(uxDate.Text, out temp))
				return null;
			return Convert.ToDateTime(uxDate.Text);
		}
		set { uxDate.Text = value.HasValue ? value.Value.ToString((PickerStyle == Picker.DateOnly ? DateFormat : (PickerStyle == Picker.TimeOnly ? TimeFormat : DateFormat + " " + TimeFormat))).ToLower() : ""; }
	}

	private bool m_Required;
	public bool Required
	{
		get { return m_Required; }
		set
		{
			m_Required = value;
			uxDateRFV.Enabled = value && Enabled;
		}
	}

	public string RequiredErrorMessage
	{
		set { uxDateRFV.ErrorMessage = value; }
	}

	public string ValidationGroup { get; set; }

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxDateREV.ClientValidationFunction = "ValidateDate_" + ClientID;
		uxDateREV.ServerValidate += uxDateREV_ServerValidate;

		if (!String.IsNullOrEmpty(ValidationGroup))
			uxDateREV.ValidationGroup =
			uxDateRFV.ValidationGroup = ValidationGroup;

		m_DotsToRoot = string.Empty;
		for (int i = 0; i < (Request.AppRelativeCurrentExecutionFilePath.Split('/').Length - 2); i++)
			m_DotsToRoot += "../";

		uxDate.AutoPostBack = AutoPostBack;
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		Helpers.GetCSSCode(uxCSSFiles);
		Helpers.GetJSCode(uxJavaScripts);
		uxDate.Enabled =
		uxDateREV.Enabled = Enabled;
		uxDateRFV.Enabled = Required && Enabled;
	}

	void uxDateREV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		DateTime temp;
		args.IsValid = DateTime.TryParse(uxDate.Text, out temp);
	}
}