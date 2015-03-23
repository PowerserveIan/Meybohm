using System;
using Classes.StateAndCountry;

public partial class Controls_State_And_Country_Address : System.Web.UI.UserControl
{
	private bool m_ShowLatAndLong = true;
	public int? AddressID { get; set; }
	public string AddressLabel { get; set; }
	public string Address2Label { get; set; }
	public bool AutoCalculateCoordinates { get; set; }
	public bool ReadOnly { get; set; }
	public bool Required { get; set; }
	public bool ShowAddress2 { get; set; }
	public bool ShowLatAndLong { get { return m_ShowLatAndLong; } set { m_ShowLatAndLong = value; } }
	public string ValidationGroup { get; set; }

	protected override void OnInit(EventArgs e)
	{
		if (!IsPostBack)
		{
			uxStateID.DataSource = State.GetAll();
			uxStateID.DataTextField = "Name";
			uxStateID.DataValueField = "StateID";
			uxStateID.DataBind();
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxAddress.ReadOnly =
		uxCity.ReadOnly =
		uxZip.ReadOnly =
		uxLatitude.ReadOnly =
		uxLongitude.ReadOnly = ReadOnly;
		uxStateID.Enabled = !ReadOnly;

		uxAddressReqAst.Visible =
		uxAddressReqFVal.Enabled =
		uxCityReqAst.Visible =
		uxCityReqFVal.Enabled =
		uxStateReqAst.Visible =
		uxStateIDReqFVal.Enabled =
		uxZipReqAst.Visible =
		uxZip.Required =
		uxLatitudeReqAst.Visible =
		uxLatitudeReqFVal.Enabled =
		uxLongitudeReqAst.Visible =
		uxLongitudeReqFVal.Enabled = Required;

		if (!String.IsNullOrEmpty(ValidationGroup))
			uxAddressRegexVal.ValidationGroup =
			uxAddressReqFVal.ValidationGroup =
			uxCityRegexVal.ValidationGroup =
			uxCityReqFVal.ValidationGroup =
			uxLatitudeRangeVal.ValidationGroup =
			uxLatitudeReqFVal.ValidationGroup =
			uxLongitudeRangeVal.ValidationGroup =
			uxLongitudeReqFVal.ValidationGroup =
			uxStateIDReqFVal.ValidationGroup =
			uxZip.ValidationGroup = ValidationGroup;

		if (!String.IsNullOrEmpty(AddressLabel))
		{
			uxAddressLabel.Text = AddressLabel;
			uxAddressRegexVal.ErrorMessage = AddressLabel + " is too long.  It must be 255 characters or less.";
			uxAddressReqFVal.ErrorMessage = AddressLabel + " is required.";
		}
		if (!String.IsNullOrEmpty(Address2Label))
		{
			uxAddress2Label.Text = Address2Label;
			uxAddress2RegexVal.ErrorMessage = Address2Label + " is too long.  It must be 255 characters or less.";
		}

		uxAddress2PH.Visible = ShowAddress2;
		uxLatLongPH.Visible = ShowLatAndLong;
	}

	public void Load()
	{
		if (!AddressID.HasValue)
			return;
		Address addressEntity = Address.GetByID(AddressID.Value);
		uxAddress.Text = addressEntity.Address1;
		uxAddress2.Text = addressEntity.Address2;
		uxCity.Text = addressEntity.City;
		if (uxStateID.Items.FindByValue(addressEntity.StateID.ToString()) != null)
			uxStateID.Items.FindByValue(addressEntity.StateID.ToString()).Selected = true;
		uxZip.Text = addressEntity.Zip;
		uxLatitude.Text = addressEntity.Latitude.ToString();
		uxLongitude.Text = addressEntity.Longitude.ToString();
	}

	public void Save()
	{
		Address addressEntity = AddressID.HasValue ? Address.GetByID(AddressID.Value) : new Address();
		addressEntity.Address1 = uxAddress.Text;
		addressEntity.Address2 = uxAddress2.Text;
		addressEntity.City = uxCity.Text;
		addressEntity.StateID = !String.IsNullOrEmpty(uxStateID.SelectedValue) ? (int?)Convert.ToInt32(uxStateID.SelectedValue) : null;
		addressEntity.Zip = uxZip.Text;
		addressEntity.Latitude = !String.IsNullOrEmpty(uxLatitude.Text) ? (decimal?)Convert.ToDecimal(uxLatitude.Text) : null;
		addressEntity.Longitude = !String.IsNullOrEmpty(uxLongitude.Text) ? (decimal?)Convert.ToDecimal(uxLongitude.Text) : null;
		addressEntity.Save();

		AddressID = addressEntity.AddressID;
	}
}