using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Media352_MembershipProvider;

public partial class Admin_AdminTeamEdit : BaseEditPage
{
	public Team TeamEntity { get; set; }
	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-team.aspx";
		m_ClassName = "Team";
		base.OnInit(e);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		uxEmailRegexVal2.ValidationExpression = Helpers.EmailValidationExpression;
		if (!IsPostBack)
		{
			uxLanguages.DataSource = Classes.SiteLanguages.Language.GetAll().Where(l => l.Culture != "English");
			uxLanguages.DataTextField = "Culture";
			uxLanguages.DataValueField = "LanguageID";
			uxLanguages.DataBind();

			uxCMMicrositeID.DataSource = Classes.ContentManager.CMMicrosite.CMMicrositePage(0, 0, "", "Name", true, new Classes.ContentManager.CMMicrosite.Filters { FilterCMMicrositeActive = true.ToString(), FilterCMMicrositePublished = true.ToString() });
			uxCMMicrositeID.DataTextField = "Name";
			uxCMMicrositeID.DataValueField = "CMMicrositeID";
			uxCMMicrositeID.DataBind();

			if (EntityId > 0)
			{
				TeamEntity = Team.GetByID(EntityId);
				if (TeamEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
				uxAfterSavePH.Visible = true;
			}
			else
				NewRecord = true;
		}
	}

	protected override void Save()
	{
		uxPhoto.CommitChanges();
		if (IsValid)
		{
			TeamEntity = EntityId > 0 ? Team.GetByID(EntityId) : new Team();
			TeamEntity.CMMicrositeID = Convert.ToInt32(uxCMMicrositeID.SelectedValue);
			TeamEntity.DisplayInDirectory = uxDisplayInDirectory.Checked;
			TeamEntity.Email = uxEmail.Text;
			TeamEntity.Name = uxName.Text;
			TeamEntity.Photo = uxPhoto.FileName;
			TeamEntity.Save();

			SaveLanguages();

			EntityId = TeamEntity.TeamID;
			m_ClassTitle = TeamEntity.Name;

			uxAfterSavePH.Visible = true;
		}
	}

	protected override void LoadData()
	{
		if (uxCMMicrositeID.Items.FindByValue(TeamEntity.CMMicrositeID.ToString()) != null)
			uxCMMicrositeID.Items.FindByValue(TeamEntity.CMMicrositeID.ToString()).Selected = true;
		uxDisplayInDirectory.Checked = TeamEntity.DisplayInDirectory;
		uxEmail.Text = TeamEntity.Email;
		uxName.Text = TeamEntity.Name;
		uxMLSID.Text = TeamEntity.MlsID;
		uxPhoto.FileName = TeamEntity.Photo;
		LoadLanguages();
		BindTeams();
	}

	private void LoadLanguages()
	{
		List<TeamLanguageSpoken> joins = TeamLanguageSpoken.TeamLanguageSpokenGetByTeamID(EntityId);
		foreach (TeamLanguageSpoken join in joins)
		{
			if (uxLanguages.Items.FindByValue(join.LanguageID.ToString()) != null)
				uxLanguages.Items.FindByValue(join.LanguageID.ToString()).Selected = true;
		}
	}

	void BindTeams()
	{
		List<UserTeam> userTeams = UserTeam.UserTeamGetByTeamID(EntityId, "User.Name", true, new string[] { "User" });
		uxUsers.DataSource = userTeams;
		uxUsers.DataBind();

		uxUser.DataSource = Classes.Media352_MembershipProvider.UserInfo.GetAllAgents().Where(o => !userTeams.Any(u => u.UserID == o.UserID));
		uxUser.DataTextField = "FirstAndLast";
		uxUser.DataValueField = "UserID";
		uxUser.DataBind();
	}

	/// <summary>
	/// Updates the list of selected categories in the TeamLanguageSpoken table.
	/// </summary>
	private void SaveLanguages()
	{
		List<TeamLanguageSpoken> joins = TeamLanguageSpoken.TeamLanguageSpokenGetByTeamID(EntityId);
		foreach (ListItem li in uxLanguages.Items)
		{
			TeamLanguageSpoken join = joins.Find(npc => npc.LanguageID == Convert.ToInt32(li.Value));
			if (join != null)
			{
				if (!li.Selected)
					join.Delete();
			}
			else
			{
				if (li.Selected)
				{
					join = new TeamLanguageSpoken();
					join.LanguageID = Convert.ToInt32(li.Value);
					join.TeamID = EntityId;
					join.Save();
				}
			}
		}
	}

	protected void Team_Command(object sender, CommandEventArgs e)
	{
		if (e.CommandName == "Add")
		{
			if (Page.IsValid)
			{
				UserTeam userTeamEntity = new UserTeam();
				userTeamEntity.TeamID = EntityId;
				userTeamEntity.UserID = Convert.ToInt32(uxUser.SelectedValue);
				userTeamEntity.Save();

				uxUser.ClearSelection();
			}
		}
		else if (e.CommandName == "Delete")
		{
			UserTeam userTeamEntity = UserTeam.GetByID(Convert.ToInt32(e.CommandArgument.ToString()));
			userTeamEntity.Delete();
		}

		BindTeams();
		Helpers.PageView.Anchor(Page, "teams");
	}
}
