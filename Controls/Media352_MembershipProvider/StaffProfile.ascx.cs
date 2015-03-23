using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Media352_MembershipProvider;

public partial class Controls_Media352_MembershipProvider_StaffProfile : System.Web.UI.UserControl
{
	public int EntityId
	{
		get
		{
			if (ViewState["EntityId"] == null || ViewState["EntityId"].ToString() == "0")
			{
				int tempID;
				if (Request.QueryString["id"] != null)
					if (Int32.TryParse(Request.QueryString["id"], out tempID))
						return tempID;

				return 0;
			}
			return (int)ViewState["EntityId"];
		}
		set { ViewState["EntityId"] = value; }
	}

	public UserInfo UserInfoEntity { get; set; }

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		if (!IsPostBack)
		{
			uxStaffType.DataSource = StaffType.GetAll().OrderBy(t => t.Name);
			uxStaffType.DataTextField = "Name";
			uxStaffType.DataValueField = "StaffTypeID";
			uxStaffType.DataBind();

			uxLanguages.DataSource = Classes.SiteLanguages.Language.GetAll().Where(l => l.Culture != "English");
			uxLanguages.DataTextField = "Culture";
			uxLanguages.DataValueField = "LanguageID";
			uxLanguages.DataBind();

			uxDesignations.DataSource = Designation.GetAll().OrderBy(t=>t.Name);
			uxDesignations.DataTextField = "Name";
			uxDesignations.DataValueField = "DesignationID";
			uxDesignations.DataBind();

			uxJobTitle.DataSource = JobTitle.GetAll();
			uxJobTitle.DataTextField = "Name";
			uxJobTitle.DataValueField = "JobTitleID";
			uxJobTitle.DataBind();

			uxPermissionsPH.Visible =
			uxShowRatingOnSite.Visible =
			uxStaffType.Enabled = Page.User.IsInRole("Admin");

			uxRating.ReadOnly = !Page.User.IsInRole("Admin");
		}
	}

	public void LoadData()
	{
		uxBiography.Text = UserInfoEntity.Biography;
		uxCellPhone.Text = UserInfoEntity.CellPhone;
		uxDisplayInDirectory.Checked = UserInfoEntity.DisplayInDirectory;
		uxFax.Text = UserInfoEntity.Fax;
		uxHomePhone.Text = UserInfoEntity.HomePhone;
		if (uxJobTitle.Items.FindByValue(UserInfoEntity.JobTitleID.ToString()) != null)
			uxJobTitle.Items.FindByValue(UserInfoEntity.JobTitleID.ToString()).Selected = true;
		uxOfficePhone.Text = UserInfoEntity.OfficePhone;
		uxPhoto.FileName = UserInfoEntity.Photo;
		if (uxPrimaryPhone.Items.FindByValue(UserInfoEntity.PrimaryPhone) != null)
			uxPrimaryPhone.Items.FindByValue(UserInfoEntity.PrimaryPhone).Selected = true;
		uxRating.Text = UserInfoEntity.Rating.ToString();
		uxShowListingLink.Checked = UserInfoEntity.ShowListingLink;
		uxShowRatingOnSite.Checked = UserInfoEntity.ShowRatingOnSite;
		if (uxStaffType.Items.FindByValue(UserInfoEntity.StaffTypeID.ToString()) != null)
			uxStaffType.Items.FindByValue(UserInfoEntity.StaffTypeID.ToString()).Selected = true;
		uxWebsite.Text = UserInfoEntity.Website;

		LoadLanguages();
		LoadDesignations();
		BindOffices();
		BindTeams();
		BindTestimonials();
	}

	private void LoadLanguages()
	{
		List<UserLanguageSpoken> joins = UserLanguageSpoken.UserLanguageSpokenGetByUserID(EntityId);
		foreach (UserLanguageSpoken join in joins)
		{
			if (uxLanguages.Items.FindByValue(join.LanguageID.ToString()) != null)
				uxLanguages.Items.FindByValue(join.LanguageID.ToString()).Selected = true;
		}
	}

	private void LoadDesignations()
	{
		List<UserDesignation> joins = UserDesignation.UserDesignationGetByUserID(EntityId);
		foreach (UserDesignation join in joins)
		{
			if (uxDesignations.Items.FindByValue(join.DesignationID.ToString()) != null)
				uxDesignations.Items.FindByValue(join.DesignationID.ToString()).Selected = true;
		}
	}

	public void SaveData()
	{
		uxPhoto.CommitChanges();
		if (Page.IsValid)
		{
			UserInfoEntity.Biography = uxBiography.Text;
			UserInfoEntity.CellPhone = uxCellPhone.Text;
			UserInfoEntity.DisplayInDirectory = uxDisplayInDirectory.Checked;
			UserInfoEntity.Fax = uxFax.Text;
			UserInfoEntity.HomePhone = uxHomePhone.Text;
			UserInfoEntity.JobTitleID = !String.IsNullOrEmpty(uxJobTitle.SelectedValue) ? (int?)Convert.ToInt32(uxJobTitle.SelectedValue) : null;
			UserInfoEntity.OfficePhone = uxOfficePhone.Text;
			UserInfoEntity.Photo = uxPhoto.FileName;
			UserInfoEntity.PrimaryPhone = uxPrimaryPhone.SelectedValue;
			UserInfoEntity.Rating = !String.IsNullOrEmpty(uxRating.Text) ? (decimal?)Convert.ToDecimal(uxRating.Text) : null;
			UserInfoEntity.ShowListingLink = uxShowListingLink.Checked;
			UserInfoEntity.ShowRatingOnSite = uxShowRatingOnSite.Checked;
			UserInfoEntity.StaffTypeID = !String.IsNullOrEmpty(uxStaffType.SelectedValue) ? (int?)Convert.ToInt32(uxStaffType.SelectedValue) : null;
			UserInfoEntity.Website = (!String.IsNullOrEmpty(uxWebsite.Text) && !uxWebsite.Text.StartsWith("http") ? "http://" : "") + uxWebsite.Text;
			UserInfoEntity.Save();

			SaveLanguages();
			SaveDesignations();
		}
	}

	/// <summary>
	/// Updates the list of selected categories in the UserLanguageSpoken table.
	/// </summary>
	private void SaveLanguages()
	{
		List<UserLanguageSpoken> joins = UserLanguageSpoken.UserLanguageSpokenGetByUserID(EntityId);
		foreach (ListItem li in uxLanguages.Items)
		{
			UserLanguageSpoken join = joins.Find(npc => npc.LanguageID == Convert.ToInt32(li.Value));
			if (join != null)
			{
				if (!li.Selected)
					join.Delete();
			}
			else
			{
				if (li.Selected)
				{
					join = new UserLanguageSpoken();
					join.LanguageID = Convert.ToInt32(li.Value);
					join.UserID = EntityId;
					join.Save();
				}
			}
		}
	}

	/// <summary>
	/// Updates the list of selected designations in the UserDesignation table.
	/// </summary>
	private void SaveDesignations()
	{
		List<UserDesignation> joins = UserDesignation.UserDesignationGetByUserID(EntityId);
		foreach (ListItem li in uxDesignations.Items)
		{
			UserDesignation join = joins.Find(npc => npc.DesignationID == Convert.ToInt32(li.Value));
			if (join != null)
			{
				if (!li.Selected)
					join.Delete();
			}
			else
			{
				if (li.Selected)
				{
					join = new UserDesignation();
					join.DesignationID = Convert.ToInt32(li.Value);
					join.UserID = EntityId;
					join.Save();
				}
			}
		}
	}

	void BindOffices()
	{
		List<UserOffice> userOffices = UserOffice.UserOfficeGetByUserID(EntityId, "Office.Name", true, new string[] { "Office" });
		uxUserOffices.DataSource = userOffices;
		uxUserOffices.DataBind();

		uxOffice.DataSource = Classes.MLS.Office.GetAll().Where(o => !userOffices.Any(u => u.OfficeID == o.OfficeID)).OrderBy(o => o.Name);
		uxOffice.DataTextField = "Name";
		uxOffice.DataValueField = "OfficeID";
		uxOffice.DataBind();
	}

	void BindTeams()
	{
		List<UserTeam> userTeams = UserTeam.UserTeamGetByUserID(EntityId, "Team.Name", true, new string[] { "Team" });
		uxUserTeams.DataSource = userTeams;
		uxUserTeams.DataBind();

		uxTeam.DataSource = Team.GetAll().Where(o => !userTeams.Any(u => u.TeamID == o.TeamID)).OrderBy(o => o.Name);
		uxTeam.DataTextField = "NameAndMlsID";
		uxTeam.DataValueField = "TeamID";
		uxTeam.DataBind();
	}

	void BindTestimonials()
	{
		List<UserTestimonial> userTestimonials = UserTestimonial.UserTestimonialGetByUserID(EntityId);
		uxUserTestimonial.DataSource = userTestimonials;
		uxUserTestimonial.DataBind();
	}

	protected void Office_Command(object sender, CommandEventArgs e)
	{
		if (e.CommandName == "Add")
		{
			if (Page.IsValid)
			{
				UserOffice userOfficeEntity = new UserOffice();
				userOfficeEntity.MlsID = uxMLSID.Text;
				userOfficeEntity.OfficeID = Convert.ToInt32(uxOffice.SelectedValue);
				userOfficeEntity.UserID = EntityId;
				userOfficeEntity.Save();

				uxMLSID.Text = string.Empty;
				uxOffice.ClearSelection();
			}
		}
		else if (e.CommandName == "Delete")
		{
			UserOffice userOfficeEntity = UserOffice.GetByID(Convert.ToInt32(e.CommandArgument.ToString()));
			userOfficeEntity.Delete();
		}

		BindOffices();
		Helpers.PageView.Anchor(Page, "offices");
	}

	protected void Team_Command(object sender, CommandEventArgs e)
	{
		if (e.CommandName == "Add")
		{
			if (Page.IsValid)
			{
				UserTeam userTeamEntity = new UserTeam();
				userTeamEntity.TeamID = Convert.ToInt32(uxTeam.SelectedValue);
				userTeamEntity.UserID = EntityId;
				userTeamEntity.Save();

				uxTeam.ClearSelection();
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

	protected void Testimonial_Command(object sender, CommandEventArgs e)
	{
		if (e.CommandName == "Add")
		{
			if (Page.IsValid)
			{
				UserTestimonial userTestimonialEntity = new UserTestimonial();
				userTestimonialEntity.GiverNameAndLocation = uxGiverNameAndLocation.Text;
				userTestimonialEntity.Testimonial = uxTestimonial.Text;
				userTestimonialEntity.UserID = EntityId;
				userTestimonialEntity.Save();

				uxGiverNameAndLocation.Text = uxTestimonial.Text = string.Empty;
			}
		}
		else if (e.CommandName == "Delete")
		{
			UserTestimonial userTestimonialEntity = UserTestimonial.GetByID(Convert.ToInt32(e.CommandArgument.ToString()));
			userTestimonialEntity.Delete();
		}

		BindTestimonials();
		Helpers.PageView.Anchor(Page, "testomonials");
	}
}