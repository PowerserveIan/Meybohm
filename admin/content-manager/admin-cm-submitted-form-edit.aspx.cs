using System;
using System.IO;
using BaseCode;
using Classes.ContentManager;

public partial class Admin_AdminCMSubmittedFormEdit : BaseEditPage
{
	public CMSubmittedForm CMSubmittedFormEntity { get; set; }

	protected bool IsDeveloper
	{
		get { return (User.IsInRole("Admin") || User.IsInRole("CMS Admin") || User.IsInRole("CMS Content Integrator")); }
	}

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-cm-submitted-form.aspx";
		m_ClassName = "Submitted Form";
		base.OnInit(e);
		m_AddNewButton.Visible = false;
		m_SaveAndAddNewButton.Visible = false;
		uxDownloadFile.Command += uxDownloadFile_Command;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (EntityId <= 0)
				Response.Redirect(m_LinkToListingPage + ReturnQueryString);
			else if (EntityId != 0)
			{
				CMSubmittedFormEntity = CMSubmittedForm.GetByID(EntityId);
				if (CMSubmittedFormEntity == null || !IsDeveloper && (!CMSubmittedFormEntity.CMMicrositeID.HasValue || (!CMMicrositeUser.CMMicrositeUserGetByUserID(Helpers.GetCurrentUserID()).Exists(p => p.CMMicrositeID == CMSubmittedFormEntity.CMMicrositeID))))
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
			}

			if (CMSubmittedFormEntity != null)
				LoadData();
			else
			{
				NewRecord = true;
			}
		}
	}

	protected override void Save()
	{
		if (IsValid)
		{
			CMSubmittedFormEntity = EntityId > 0 ? CMSubmittedForm.GetByID(EntityId) : new CMSubmittedForm();
			CMSubmittedFormEntity.IsProcessed = uxIsProcessed.Checked;
			CMSubmittedFormEntity.Save();

			m_ClassTitle = string.Empty;
		}
	}

	protected override void LoadData()
	{
		uxDateSubmitted.Text = CMSubmittedFormEntity.DateSubmittedClientTime.ToShortDateString();
		uxFormHTML.Text = CMSubmittedFormEntity.FormHTML;
		uxFormRecipient.Text = CMSubmittedFormEntity.FormRecipient;
		uxIsProcessed.Checked = CMSubmittedFormEntity.IsProcessed;
		if (CMSubmittedFormEntity.ResponsePageID != null)
			uxResponsePage.Text = CMPage.GetByID(CMSubmittedFormEntity.ResponsePageID.Value).FileName;
		uxUploadedFilePH.Visible = !String.IsNullOrEmpty(CMSubmittedFormEntity.UploadedFile);
		uxDownloadFile.CommandArgument = Classes.ContentManager.DynamicForm.UploadedFilesLocation + CMSubmittedFormEntity.UploadedFile;
		uxDownloadFile.Text = CMSubmittedFormEntity.UploadedFile;
	}
	
	void uxDownloadFile_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
	{
		string filePath = Server.MapPath(e.CommandArgument.ToString());
		FileInfo fileInfo = new FileInfo(filePath);
		if (fileInfo.Exists)
		{
			Response.Clear();
			Response.Buffer = true;
			Response.ContentType = "application/octstream";
			Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
			Response.TransmitFile(filePath);
			Response.End();
		}
		else
			ClientScript.RegisterStartupScript(Page.GetType(), "FileDoesntExist", "alert('The file cannot be found, please try again later.');", true);
	}
}