using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.StateAndCountry;

public partial class Controls_State_And_Country_State_Country_Controls : UserControl
{
	#region Members

	private int? m_CountryID;
	private bool m_IsRequired = true;
	private int? m_StateID;
	private string m_CountryDivClass = string.Empty;
	private string m_CountryDropdownClass = string.Empty;
	private string m_StateDivClass = string.Empty;
	private string m_StateDropdownAndTextClass = string.Empty;

	#endregion

	#region Properties - Display

	public bool UseShipTo { get; set; }

	public bool HideNonShipToCountries { get; set; }

	public bool StateOnly { get; set; }

	public string CountryDropdownClass
	{
		get { return m_CountryDropdownClass; }
		set { m_CountryDropdownClass = value; }
	}

	public string StateDropdownAndTextClass
	{
		get { return m_StateDropdownAndTextClass; }
		set { m_StateDropdownAndTextClass = value; }
	}

	public string CountryDivClass
	{
		get { return m_CountryDivClass; }
		set { m_CountryDivClass = value; }
	}

	public string StateDivClass
	{
		get { return m_StateDivClass; }
		set { m_StateDivClass = value; }
	}

	#endregion

	#region Property - Controls Setup

	/// <summary>
	/// Enable AutoPostBack on the country drop down menu
	/// </summary>
	public bool CountryDropDownAutoPostBack
	{
		get { return uxCountry.AutoPostBack; }
		set { uxCountry.AutoPostBack = value; }
	}

	public bool IsRequired
	{
		get { return m_IsRequired; }
		set
		{
			m_IsRequired = value;
			ShowRequired();
		}
	}

	public bool ReadOnly { get; set; }

	/// <summary>
	/// Used for settgin up thevalidation group for the state/country controls
	/// </summary>
	public string ValidationGroupName
	{
		set
		{
			uxStateCV.ValidationGroup = value;
			uxCountryCV.ValidationGroup = value;
		}
	}

	/// <summary>
	/// Used for settgin up the Text property for the validation controls for both state and country
	/// </summary>
	public string ValidationControlText
	{
		set
		{
			uxStateCV.Text = value;
			uxCountryCV.Text = value;
		}
	}

	/// <summary>
	/// Used for setting up the Text property for the validation controls for both state and country
	/// </summary>
	public string ValidationControlText_State
	{
		set { uxStateCV.Text = value; }
	}

	/// <summary>
	/// Used for setting up the Text property for the validation controls for both state and country
	/// </summary>
	public string ValidationControlText_Country
	{
		set { uxCountryCV.Text = value; }
	}

	/// <summary>
	/// Used for setting up the Text property for the validation controls for both state and country
	/// </summary>
	public string ValidationControlErrorMessage_State
	{
		set { uxStateCV.ErrorMessage = value; }
	}

	/// <summary>
	/// Used for setting up the Text property for the validation controls for both state and country
	/// </summary>
	public string ValidationControlErrorMessage_Country
	{
		set { uxCountryCV.ErrorMessage = value; }
	}

	/// <summary>
	/// Gets/sets the value of the CountryID used for selecting the country in the drop-down
	/// </summary>
	public int? CountryID
	{
		get
		{
			if (!m_CountryID.HasValue)
				m_CountryID = GetDropDownValue(uxCountry);
			return m_CountryID;
		}
		set
		{
			m_CountryID = value.HasValue ? value.Value : 0;
			SetDropDownValue(uxCountry, value);
		}
	}

	/// <summary>
	/// Gets the text value of the country drop-down
	/// </summary>
	public string CountryName
	{
		get
		{
			return CountryID.HasValue ? uxCountry.SelectedItem.Text : String.Empty;
		}
	}

	/// <summary>
	/// Gets/sets the value of the StateID used for selecting the country in the drop-down
	/// </summary>
	public int? StateID
	{
		get
		{
			if (!m_StateID.HasValue)
				m_StateID = GetDropDownValue(uxState);
			return m_StateID;
		}
		set
		{
			m_StateID = value.HasValue ? value.Value : 0;
			SetDropDownValue(uxState, value);
		}
	}

	/// <summary>
	/// Gets the text value of the state drop-down
	/// </summary>
	public string StateName
	{
		get
		{
			return StateID.HasValue ? Classes.StateAndCountry.State.GetByID(StateID.Value).Name : uxStateOther.Text;
		}
	}

	/// <summary>
	/// Gets/sets the value of the state text field
	/// </summary>
	public string State
	{
		get { return uxStateOther.Text; }
		set { uxStateOther.Text = value; }
	}

	#endregion

	private static void SetDropDownValue(dynamic currentElement, int? valueToUse)
	{
		if (currentElement is DropDownList)
			currentElement.ClearSelection();

		if (valueToUse.HasValue)
		{
			ListItem li = currentElement.Items.FindByValue(valueToUse.Value.ToString());
			if (li != null)
			{
				if (currentElement is DropDownList)
					li.Selected = true;
				else
					currentElement.SelectedIndex = currentElement.Items.IndexOf(li);
			}
		}
		else
			currentElement.SelectedIndex = 0;
	}

	private int? GetDropDownValue(dynamic currentElement)
	{
		string currentItem = null;
		if (!String.IsNullOrEmpty(Request.Form[currentElement.UniqueID]))
			currentItem = Request.Form[currentElement.UniqueID];
		else if (currentElement is DropDownList)
			currentItem = ((DropDownList)currentElement).SelectedValue;
		else if (currentElement is System.Web.UI.HtmlControls.HtmlSelect)
			currentItem = ((System.Web.UI.HtmlControls.HtmlSelect)currentElement).Items[((System.Web.UI.HtmlControls.HtmlSelect)currentElement).SelectedIndex].Value;

		if (string.IsNullOrEmpty(currentItem))
			return null;
		return Convert.ToInt32(currentItem);
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		if (!IsPostBack)
		{
			uxCountry.DataSource = Country.GetAll();

			uxCountry.DataTextField = "Name";
			uxCountry.DataValueField = "CountryID";
			uxCountry.DataBind();
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!String.IsNullOrEmpty(StateDropdownAndTextClass))
		{
			uxState.Attributes["class"] += " " + StateDropdownAndTextClass;
			uxStateOther.CssClass += " " + StateDropdownAndTextClass;
		}
		if (!String.IsNullOrEmpty(CountryDropdownClass))
			uxCountry.CssClass += " " + CountryDropdownClass;

		if (!String.IsNullOrEmpty(CountryDivClass))
			countrydropdowndiv.Attributes["class"] += " " + CountryDivClass;

		if (!String.IsNullOrEmpty(StateDivClass))
		{
			statedropdowndiv.Attributes["class"] += " " + StateDivClass;
			stateotherdiv.Attributes["class"] += " " + StateDivClass;
		}
		ShowRequired();
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		BaseCode.Helpers.GetJSCode(uxJavaScripts);
		//Register State and Country Web Service
		ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
		if (scriptManager.Services.Count(s => s.Path == "~/tft-services/StateAndCountry/StateAndCountryWebMethods.asmx") == 0)
			scriptManager.Services.Add(new ServiceReference { InlineScript = true, Path = "~/tft-services/StateAndCountry/StateAndCountryWebMethods.asmx" });

		if (StateOnly)
		{
			CountryID = 1;
			countrydropdowndiv.Attributes.Add("style", "display: none");
			stateotherdiv.Attributes.Add("style", "display: none");
			uxCountryCV.Enabled = false;
		}

		//Setup the drop-downs based on the fact that the user may have assigned the values already
		SetupDropDownsForUser();
		//setup the ddl in case they need to be defaulted
		SetupCountryDDLs();

		uxStateCV.ClientValidationFunction = "State" + ClientID + "_ClientValidate";
		uxCountryCV.ClientValidationFunction = "Country" + ClientID + "_ClientValidate";

		uxCountry.Enabled = !ReadOnly;
		uxStateOther.ReadOnly = ReadOnly;
		if (ReadOnly)
			uxState.Attributes["disabled"] = "disabled";

		if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack && this.Visible)
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "StartupScript_" + DateTime.Now.Ticks, "RunAtStartup_" + ClientID + "();", true);
	}

	/// <summary>
	/// Used to setup which control is visible based on what is selected
	/// </summary>
	protected void SetupCountryDDLs()
	{
		if (uxCountry.SelectedItem != null && (String.IsNullOrEmpty(uxCountry.SelectedItem.Value) || (!uxCountry.SelectedItem.Text.Equals("United States", StringComparison.OrdinalIgnoreCase) && !uxCountry.SelectedItem.Text.Equals("Canada", StringComparison.OrdinalIgnoreCase))))
			uxState.SelectedIndex = 0;
	}

	/// <summary>
	/// Used to select the correct values based on the properties values
	/// </summary>
	private void SetupDropDownsForUser()
	{
		//look for the stateid in the list
		if (CountryID.HasValue)
		{
			uxCountry.ClearSelection();
			ListItem liFind = uxCountry.Items.FindByValue(CountryID.ToString());
			if (liFind != null)
				liFind.Selected = true;
			uxState.Items.Clear();
			uxState.DataSource = Classes.StateAndCountry.State.GetStatesByCountryIDWithShipTo(CountryID.Value, UseShipTo);
			uxState.DataTextField = "Name";
			uxState.DataValueField = "StateID";
			uxState.DataBind();
			uxState.Items.Insert(0, new ListItem("--Select One--", ""));
		}

		if (StateID.HasValue)
			uxState.SelectedIndex = uxState.Items.IndexOf(uxState.Items.FindByValue(StateID.ToString()));
	}

	protected void cvCountry_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !IsRequired || !String.IsNullOrEmpty(uxCountry.SelectedValue);
	}

	protected void cvState_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !IsRequired || !String.IsNullOrEmpty(Request.Form[uxState.UniqueID]) || (!uxCountry.SelectedItem.Text.Equals("United States") && !uxCountry.SelectedItem.Text.Equals("Canada"));
	}

	protected void ShowRequired()
	{
		uxCountyRequired.Visible = IsRequired;
		uxCountryCV.Enabled = IsRequired;
		uxStateRequired.Visible = IsRequired;
		uxStateCV.Enabled = IsRequired;
	}
}