using System;
using System.Linq;
using System.Web.UI.WebControls;
using Classes.Media352_MembershipProvider;

public partial class Admin_AdminSecurityQuestionEdit : BaseEditPage
{
	public SecurityQuestion SecurityQuestionEntity { get; set; }

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-security-question.aspx";
		m_ClassName = "Security Question";
		base.OnInit(e);
		uxQuestionCV.ServerValidate += uxQuestionCV_ServerValidate;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			if (EntityId > 0)
			{
				SecurityQuestionEntity = SecurityQuestion.GetByID(EntityId);
				if (SecurityQuestionEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
			}
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
			SecurityQuestionEntity = EntityId > 0 ? SecurityQuestion.GetByID(EntityId) : new SecurityQuestion();
			SecurityQuestionEntity.Active = uxActive.Checked;
			SecurityQuestionEntity.Question = uxQuestion.Text;
			SecurityQuestionEntity.Save();
			EntityId = SecurityQuestionEntity.SecurityQuestionID;
			m_ClassTitle = string.Empty;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = SecurityQuestionEntity.Active;
		uxQuestion.Text = SecurityQuestionEntity.Question;
	}

	void uxQuestionCV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !SecurityQuestion.SecurityQuestionGetByQuestion(uxQuestion.Text).Any(q => q.SecurityQuestionID != EntityId);
	}
}