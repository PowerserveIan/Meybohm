using System;
using System.Linq;
using System.Web.UI.WebControls;
using Classes.Newsletters;

public partial class Admin_MailingListEdit : BaseEditPage
{
	public MailingList MailingListEntity { get; set; }

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxImport.BlockSubscribers = uxMailingListSubscribers.BlockSubscribers;
	}

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-mailing-list.aspx";
		m_ClassName = "Mailing List";
		base.OnInit(e);
		m_AddNewButton = null;
		m_SaveAndAddNewButton.Visible = false;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (EntityId > 0)
			{
				MailingListEntity = MailingList.GetByID(EntityId);
				if (MailingListEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				uxMailingListSubscribers.MailingListID = EntityId;
				uxImport.MailingListID = EntityId;
				uxImport.Visible = true;
				LoadData();
			}
			else
			{
				//This if block is for if someone tries to manipulate the query string to create a new mailing list after the
				//max number of mailing lists has been reached.
				if (Settings.EnableMailingListLimitations)
					if (MailingList.GetByActiveDeleted(true, false).Count > Settings.MaxNumberMailingLists)
						Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				uxImport.Visible = false;
			}
		}
	}

	protected override void Save()
	{
		if (IsValid)
		{
			MailingListEntity = EntityId > 0 ? MailingList.GetByID(EntityId) : new MailingList();
			MailingListEntity.Active = uxActive.Checked;
			MailingListEntity.Name = uxName.Text;
			MailingListEntity.Save();
			EntityId = MailingListEntity.MailingListID;
			uxMailingListSubscribers.MailingListID = MailingListEntity.MailingListID;
			uxMailingListSubscribers.Visible = true;
			uxImport.MailingListID = MailingListEntity.MailingListID;
			uxImport.Visible = true;
			m_ClassTitle = MailingListEntity.Name;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = MailingListEntity.Active;
		uxName.Text = MailingListEntity.Name;
	}

	public void uxNameCV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !MailingList.MailingListGetByName(uxName.Text).Any(t => EntityId != t.MailingListID);
	}
}