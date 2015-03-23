using System;
using System.Linq;
using BaseCode;
using Classes.Contacts;

public partial class Admin_AdminContactEdit : BaseEditPage
{
	public Contact ContactEntity { get; set; }

	protected ContactTypes m_ContactType
	{
		get
		{
			return BaseCode.EnumParser.Parse<ContactTypes>(Request.QueryString["FilterContactContactTypeID"]);
		}
	}

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-contact.aspx";
		m_ClassName = m_ContactType == ContactTypes.HomeValuationRequest ? "Home Valuation Request" : m_ContactType == ContactTypes.MaintenanceRequest ? "Maintenance Request" : m_ContactType == ContactTypes.PropertyInformation ? "Property Information Request" : m_ContactType == ContactTypes.Agent ? "Agent Request" : "Contact";
		base.OnInit(e);
		m_AddNewButton.Visible = m_SaveAndAddNewButton.Visible = false;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			uxContactStatusID.DataSource = ContactStatus.GetAll();
			uxContactStatusID.DataTextField = "Name";
			uxContactStatusID.DataValueField = "ContactStatusID";
			uxContactStatusID.DataBind();

			if (EntityId > 0)
			{
				ContactEntity = Contact.GetByID(EntityId, new string[] { "ContactMethod", "ContactTime", "Address", "Address.State" });
				if (ContactEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
			}
			else
				Response.Redirect(m_LinkToListingPage + ReturnQueryString);
		}
	}

	protected override void Save()
	{
		if (IsValid)
		{
			ContactEntity = EntityId > 0 ? Contact.GetByID(EntityId) : new Contact();
			ContactEntity.ContactStatusID = Convert.ToInt32(uxContactStatusID.Text);
			ContactEntity.Save();

			EntityId = ContactEntity.ContactID;
			m_ClassTitle = "";
		}
	}

	protected override void LoadData()
	{
		uxAddressPH.Visible = ContactEntity.ContactTypeID == (int)ContactTypes.MaintenanceRequest || ContactEntity.ContactTypeID == (int)ContactTypes.HomeValuationRequest;
		if (uxAddressPH.Visible && ContactEntity.AddressID.HasValue)
		{
			uxAddress1.Text = ContactEntity.Address.Address1;
			uxAddress2.Text = ContactEntity.Address.Address2;
			uxCity.Text = ContactEntity.Address.City;
			uxState.Text = ContactEntity.Address.State.Name;
			uxZip.Text = ContactEntity.Address.Zip;
		}
		uxContactMethod.Text = ContactEntity.ContactMethod.Name;
		if (uxContactStatusID.Items.FindByValue(ContactEntity.ContactStatusID.ToString()) != null)
			uxContactStatusID.Items.FindByValue(ContactEntity.ContactStatusID.ToString()).Selected = true;
		uxContactTime.Text = ContactEntity.ContactTime.Name;
		uxEmail.Text = ContactEntity.Email;
		uxFirstName.Text = ContactEntity.FirstName;
		uxLastName.Text = ContactEntity.LastName;
		uxMessage.Text = ContactEntity.Message;
		uxPhone.Text = ContactEntity.Phone;
		uxTimestamp.Text = ContactEntity.CreatedClientTime.ToString();

		uxShowcaseItemPH.Visible = ContactEntity.ShowcaseItemID.HasValue;
		if (uxShowcaseItemPH.Visible)
			uxShowcaseItemTitle.Text = Classes.Showcase.ShowcaseItem.GetByID(ContactEntity.ShowcaseItemID.Value, new[] { "Address.State" }).Address.FormattedAddress;

		uxAgentPH.Visible = ContactEntity.AgentID.HasValue;
		if (uxAgentPH.Visible)
			uxAgentName.Text = Classes.Media352_MembershipProvider.UserInfo.UserInfoGetByUserID(ContactEntity.AgentID.Value).FirstOrDefault().FirstAndLast;
	}
}
