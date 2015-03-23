using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Contacts;

public partial class Controls_Contacts_ContactForm : System.Web.UI.UserControl
{
	private ContactTypes m_ContactFormType = ContactTypes.ContactUs;

	public int? AgentID { get; set; }

	public ContactTypes ContactFormType { get { return m_ContactFormType; } set { m_ContactFormType = value; } }

	public bool EnableClientSideSubmission { get; set; }

	public bool HideIntroText { get; set; }

	public string MessageFieldText { get; set; }

	public bool ShowAddressFields { get; set; }

	public int? ShowcaseItemID { get; set; }

	protected int? m_CurrentMicrositeID
	{
		get
		{
			Classes.ContentManager.CMMicrosite currentMicrosite = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite();
			if (currentMicrosite != null)
				return currentMicrosite.CMMicroSiteID;
			return null;
		}
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxContactTimeRFV.ServerValidate += uxContactTimeRFV_ServerValidate;
		uxSubmit.Click += uxSubmit_Click;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		uxEmailRegexVal.ValidationExpression = Helpers.EmailValidationExpression;
		uxIntroTextPH.Visible = !HideIntroText;
		if (!IsPostBack)
		{
			uxContactTime.DataSource = ContactTime.GetAll();
			uxContactTime.DataTextField = "Name";
			uxContactTime.DataValueField = "ContactTimeID";
			uxContactTime.DataBind();

			uxContactMethod.DataSource = ContactMethod.GetAll();
			uxContactMethod.DataTextField = "Name";
			uxContactMethod.DataValueField = "ContactMethodID";
			uxContactMethod.DataBind();

			if (!String.IsNullOrEmpty(MessageFieldText))
				uxMessageFieldText.Text = MessageFieldText;

			uxAddress.Visible = ShowAddressFields;

			if (EnableClientSideSubmission)
			{
				uxSuccessPH.Visible = true;
				uxSuccessPH.Style.Add("display", "none");
			}
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		if (EnableClientSideSubmission)
		{
			//Register Showcase Web Service
			ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
			if (!scriptManager.Services.Any(s => s.Path == "~/tft-services/Contacts/ContactsWebMethods.asmx"))
				scriptManager.Services.Add(new ServiceReference { InlineScript = true, Path = "~/tft-services/Contacts/ContactsWebMethods.asmx" });
		}
	}

	void uxSubmit_Click(object sender, EventArgs e)
	{
		if (!String.IsNullOrEmpty(uxContactMethod.SelectedValue) && Convert.ToInt32(uxContactMethod.SelectedValue) == (int)ContactMethods.Email)
		{
			uxEmailRegexVal.Enabled =
			uxEmailReqFVal.Enabled = true;
			uxPhone.Required = false;
		}
		else
		{
			uxEmailRegexVal.Enabled =
			uxEmailReqFVal.Enabled = false;
			uxPhone.Required = true;
		}
		if (Page.IsValid)
		{
			Contact contactEntity = new Contact();
			if (ShowAddressFields)
			{
				uxAddress.Save();
				contactEntity.AddressID = uxAddress.AddressID;
			}
			if (AgentID.HasValue)
				contactEntity.AgentID = AgentID;
			else if (ShowcaseItemID.HasValue)
				contactEntity.AgentID = Classes.Showcase.ShowcaseItem.GetByID(ShowcaseItemID.Value).AgentID;
			contactEntity.CMMicrositeID = m_CurrentMicrositeID;
			contactEntity.ContactMethodID = Convert.ToInt32(uxContactMethod.SelectedValue);
			contactEntity.ContactStatusID = (int)ContactStatuses.Unread;
			contactEntity.ContactTimeID = Convert.ToInt32(uxContactTime.SelectedValue);
			contactEntity.ContactTypeID = (int)ContactFormType;
			contactEntity.Created = DateTime.UtcNow;
			contactEntity.Email = uxEmail.Text;
			contactEntity.FirstName = uxFirstName.Text;
			contactEntity.LastName = uxLastName.Text;
			contactEntity.Message = uxMessage.Text;
			contactEntity.Phone = uxPhone.Text;
			contactEntity.ShowcaseItemID = ShowcaseItemID;
			contactEntity.Save();

			Contact.SendSubmissionEmail(contactEntity, ContactFormType);

			uxContactPH.Visible = false;
			uxSuccessPH.Visible = true;
			Helpers.PageView.Anchor(Page, uxSuccessPH.ClientID);
		}
	}

	void uxContactTimeRFV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !String.IsNullOrEmpty(uxContactTime.SelectedValue);
	}
}