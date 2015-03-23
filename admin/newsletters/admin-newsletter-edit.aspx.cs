using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Newsletters;

public partial class Admin_NewsletterEdit : BaseEditPage
{
	private readonly NewsletterSendingTypeFormat m_NewsletterSendingType = Settings.NewsletterSendingType;

	protected int NewsletterId
	{
		get
		{
			int tempID;
			if (Request.QueryString["Id"] != null)
				if (Int32.TryParse(Request.QueryString["Id"], out tempID))
					return tempID;

			return 0;
		}
	}

	public Newsletter NewsletterEntity { get; set; }

	protected string ReturnQueryString
	{
		get { return Request.QueryString.ToString().Replace("id=" + Request.QueryString["id"], "").TrimEnd('&').TrimStart('&'); }
	}

	protected bool NewRecord
	{
		get
		{
			if (ViewState["NewRecord"] == null) return false;
			return Convert.ToBoolean(ViewState["NewRecord"]);
		}
		set { ViewState["NewRecord"] = value; }
	}

	/// <summary>
	/// Viewstate variable used to keep track of what tab you are on
	/// </summary>
	protected int TabNumber
	{
		get
		{
			if (ViewState["TabNumber"] == null) return 1;
			return Convert.ToInt32(ViewState["TabNumber"]);
		}
		set
		{
			if (Convert.ToInt32(value) > 0)
				ViewState["TabNumber"] = value;
			else
				ViewState["TabNumber"] = 1;
		}
	}

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_LinkToListingPage = "admin-newsletter.aspx";
		m_ClassName = "Newsletter";
		base.OnInit(e);
		Tab1LinkButton.Command += TabButtons_Command;
		Tab2LinkButton.Command += TabButtons_Command;
		Tab3LinkButton.Command += TabButtons_Command;
		Tab4LinkButton.Command += TabButtons_Command;
		uxBack.Click += uxBack_Click;
		uxNext.Click += uxNext_Click;
		uxSaveAndFinish.Click += uxSaveAndFinish_Click;
		uxCancel.Click += uxCancel_Click;
		uxSave.Click += uxSave_Click;
		uxNewsletterDesign.SelectedIndexChanged += uxNewsletterDesign_SelectedIndexChanged;
	}

	void uxNewsletterDesign_SelectedIndexChanged(object sender, EventArgs e)
	{
		NewsletterEntity = EntityId > 0 ? Newsletter.GetByID(EntityId) : new Newsletter();
		if (!String.IsNullOrEmpty(uxNewsletterDesign.SelectedValue) && EntityId > 0)
		{
			Mailout mailout = NewsletterSystem.MailoutFromNewsletter(NewsletterEntity, Convert.ToInt32(uxNewsletterDesign.SelectedValue));
			Session["newsletterPreview"] = NewsletterSystem.GetNewsletterHtml(mailout, false);
		}
		uxHtmlBody.EditorHTML = Helpers.ReplaceRootWithAbsolutePath(NewsletterEntity.Body);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		if (!IsPostBack)
		{
			if (NewsletterId < 0)
				Response.Redirect("~/admin/newsletters/admin-newsletter.aspx" + (!String.IsNullOrEmpty(ReturnQueryString) ? "?" + ReturnQueryString : ""));

			uxCMMicrositeID.DataSource = Classes.ContentManager.CMMicrosite.CMMicrositeGetByActive(true, "Name");
			uxCMMicrositeID.DataTextField = "Name";
			uxCMMicrositeID.DataValueField = "CMMicrositeID";
			uxCMMicrositeID.DataBind();		

			if (NewsletterId > 0)
			{
				//load Newsletter from database
				NewsletterEntity = Newsletter.GetByID(NewsletterId);
				if (NewsletterEntity == null) //could not find newsletter in the database
					Response.Redirect("~/admin/newsletters/admin-newsletter.aspx" + (!String.IsNullOrEmpty(ReturnQueryString) ? "?" + ReturnQueryString : ""));
				uxActive.Checked = NewsletterEntity.Active;
				LoadData();
			}
			else
			{
				NewRecord = true;
				NewsletterEntity = new Newsletter();
			}

			BindNewsletterDesignDropDown();
			SetTab();
			if (m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.HtmlOnly))
				FinalStepNumber.Text = @"3";
		}
	}

	private void LoadData()
	{
		uxCreationDatePH.Visible = true;
		//Tab 1		
		uxTitle.Text = NewsletterEntity.Title;
		if (uxCMMicrositeID.Items.FindByValue(NewsletterEntity.CMMicrositeID.ToString()) != null)
			uxCMMicrositeID.Items.FindByValue(NewsletterEntity.CMMicrositeID.ToString()).Selected = true;
		if (uxNewHomes.Items.FindByValue(NewsletterEntity.NewHomes.ToString()) != null)
			uxNewHomes.Items.FindByValue(NewsletterEntity.NewHomes.ToString()).Selected = true;
		uxDescription.Text = NewsletterEntity.Description;
		uxIssue.Text = NewsletterEntity.Issue;
		uxDisplayDate.SelectedDate = NewsletterEntity.DisplayDateClientTime;

		uxFeatured.Checked = NewsletterEntity.Featured;
		uxCreationDate.Text = NewsletterEntity.CreatedDateClientTime.ToString("MMMM d, yyyy");
		//Tab 2
		if (NewsletterEntity.Body != null)
			uxHtmlBody.EditorHTML = Helpers.ReplaceRootWithAbsolutePath(NewsletterEntity.Body);
		//Tab 3
		if (NewsletterEntity.BodyText != null)
			uxTextBody.Text = NewsletterEntity.BodyText;

		//SEO code
		if (NewsletterEntity.NewsletterID > 0)
		{
			uxSEOData.PageLinkFormatterElements.Add(Convert.ToString(NewsletterEntity.NewsletterID));
			uxSEOData.LoadControlData();
		}
		else
			uxSEOData.LoadControlData(true);
	}

	private void uxSave_Click(object sender, EventArgs e)
	{
		SaveNewsletter();
	}

	private void SaveNewsletter()
	{
		if (IsValid)
		{
			NewsletterEntity = EntityId > 0 ? Newsletter.GetByID(EntityId) : new Newsletter();
			//Tab 1
			NewsletterEntity.Title = uxTitle.Text;
			NewsletterEntity.CMMicrositeID = !String.IsNullOrWhiteSpace(uxCMMicrositeID.SelectedValue) ? (int?)Convert.ToInt32(uxCMMicrositeID.SelectedValue) : null;
			NewsletterEntity.NewHomes = !String.IsNullOrWhiteSpace(uxNewHomes.SelectedValue) ? Convert.ToBoolean(uxNewHomes.SelectedValue) : false;
			NewsletterEntity.Description = uxDescription.Text;
			NewsletterEntity.Issue = uxIssue.Text;
			NewsletterEntity.DisplayDate = Helpers.ConvertClientTimeToUTC(uxDisplayDate.SelectedDate.Value);
			NewsletterEntity.Active = uxActive.Checked;
			NewsletterEntity.Featured = uxFeatured.Checked;
			NewsletterEntity.Archived = false;
			if (String.IsNullOrEmpty(uxCreationDate.Text))
				NewsletterEntity.CreatedDate = DateTime.UtcNow;

			//Tab 2
			if (!m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.TextOnly))
				NewsletterEntity.DesignID = Convert.ToInt32(uxNewsletterDesign.SelectedValue);
			NewsletterEntity.Body = uxHtmlBody.EditorHTML;
			//Tab 3
			NewsletterEntity.BodyText = uxTextBody.Text;

			NewsletterEntity.Save();
			EntityId = NewsletterEntity.NewsletterID;

			//SEO saving should not be done until the new product has been created
			if (NewsletterEntity.NewsletterID > 0)
			{
				uxSEOData.PageLinkFormatterElements.Clear();
				uxSEOData.PageLinkFormatterElements.Add(Convert.ToString(NewsletterEntity.NewsletterID));
				if (String.IsNullOrEmpty(uxSEOData.Title))
					uxSEOData.Title = uxTitle.Text;
				if (String.IsNullOrEmpty(uxSEOData.FriendlyFilename))
					uxSEOData.FriendlyFilename = Helpers.StripNonAlphaCharacters(uxTitle.Text).ToLower().Replace(" ", "-");
				uxSEOData.SaveControlData();
			}

			SaveWarning.Visible = false;
			SuccessMessage.Visible = 
			uxCreationDatePH.Visible = true;
			LastSaveDate.Text = Helpers.ConvertUTCToClientTime(DateTime.UtcNow).ToString();
		}
	}

	private void BindNewsletterDesignDropDown()
	{
		uxNewsletterDesign.DataSource = NewsletterDesign.GetAll();
		uxNewsletterDesign.DataTextField = "Name";
		uxNewsletterDesign.DataValueField = "NewsletterDesignID";
		uxNewsletterDesign.DataBind();

		uxNewsletterDesign.Items.Insert(0, new ListItem("-- None --", ""));
		uxNewsletterDesign.SelectedValue = NewsletterEntity.DesignID.ToString();

		if (!String.IsNullOrEmpty(uxNewsletterDesign.SelectedValue) && EntityId > 0)
		{
			Mailout mailout = NewsletterSystem.MailoutFromNewsletter(NewsletterEntity, Convert.ToInt32(uxNewsletterDesign.SelectedValue));
			Session["newsletterPreview"] = NewsletterSystem.GetNewsletterHtml(mailout, false);
		}
	}

	private void TabButtons_Command(object sender, CommandEventArgs e)
	{
		switch (e.CommandName)
		{
			case "SetTab":
				TabNumber = Convert.ToInt32(e.CommandArgument.ToString());
				SetTab();
				break;
			default:
				throw new Exception("TabButtons_Command(): CommandName '" + e.CommandName + "', CommandArgument='" + e.CommandArgument + "' not defined");
		}
	}

	public void SetTab()
	{
		uxSave.Visible =
		Tab1.Visible =
		Tab2.Visible =
		Tab3.Visible =
		Tab4.Visible =
		Tab1Instructions.Visible = 
		Tab2Instructions.Visible = 
		Tab3Instructions.Visible = 
		Tab4Instructions.Visible = false;
		Tab3LinkButton.Visible = !(m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.HtmlOnly));
		Tab2LinkButton.Visible = !(m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.TextOnly));
		Tab1LinkButton.CssClass =
		Tab2LinkButton.CssClass =
		Tab3LinkButton.CssClass =
		Tab4LinkButton.CssClass = string.Empty;		
		uxBack.Visible = 
		uxNext.Visible = 
		uxSaveAndFinish.Visible = true;
		NewsletterEntity = EntityId > 0 ? Newsletter.GetByID(EntityId) : new Newsletter();
		switch (TabNumber)
		{
			case 1:
				Tab1Instructions.Visible = true;
				Tab1LinkButton.CssClass = "selected";
				Tab1.Visible = true;
				uxBack.Visible = false;
				break;
			case 2:
				Tab2Instructions.Visible = true;
				Tab2LinkButton.CssClass = "selected";
				Tab2.Visible = true;
				break;
			case 3:
				if (!(m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.HtmlOnly)))
				{
					Tab3Instructions.Visible = true;
					Tab3LinkButton.CssClass = "selected";
					Tab3.Visible = true;
					if (String.IsNullOrEmpty(uxTextBody.Text))
						uxTextBody.Text = Helpers.StripHtml(uxHtmlBody.EditorHTML, false);
				}
				else
					throw new Exception("You should not be able to access this tab when newsletterSendingType = HtmlOnly");
				break;
			case 4:
				Tab4Instructions.Visible = true;
				Tab4LinkButton.CssClass = "selected";
				Tab4.Visible = true;

				SuccessMessage.Visible =
				uxNext.Visible =
				uxSaveAndFinish.Visible = false;
				bool passedAllValidation = ValidateAllFields();
				uxSave.Visible = passedAllValidation;
				ValidationPassedPlaceHolder.Visible = passedAllValidation;
				ValidationFailedPlaceHolder.Visible = !passedAllValidation;
				NewsletterSummary.Visible = passedAllValidation;
				SaveWarning.Visible = !passedAllValidation;
				HTMLPreviewPlaceHolder.Visible = passedAllValidation && !(m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.TextOnly));
				TextPreviewPlaceHolder.Visible = passedAllValidation && !(m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.HtmlOnly));
				if (passedAllValidation)
				{
					SaveNewsletter();
					if (m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.TextOnly))
						uxHtmlPreview.Text = GetTextNewsletter().Replace(Environment.NewLine, "<br />");
					else
						uxHtmlPreview.Text = GetHtmlNewsletter();
					uxTextPreview.Text = GetTextNewsletter().Replace(Environment.NewLine, "<br />");
				}
				else
				{
					uxHtmlPreview.Text =
					uxTextPreview.Text = string.Empty;
				}
				break;
			default:
				throw new Exception("Tab number " + TabNumber + " out of range.");
		}
	}

	private string GetTextNewsletter()
	{
		return uxTextBody.Text;
	}

	private string GetHtmlNewsletter()
	{
		return "<iframe src=\"" + Helpers.RootPath + "newsletter-details.aspx?Id=" + NewsletterEntity.NewsletterID + "&adminView=true\" width=\"550\" height=\"768\" style=\"border: none;\"></iframe>";
	}

	private void uxBack_Click(object sender, EventArgs e)
	{
		TabNumber--;
		//if newsletterSendingType is HTML only, tab 3 is invisible
		if (m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.HtmlOnly) && TabNumber == 3)
			TabNumber = 2;
		SetTab();
	}

	private void uxNext_Click(object sender, EventArgs e)
	{
		TabNumber++;
		//if newsletterSendingType is HTML only, tab 3 is invisible
		if (m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.HtmlOnly) && TabNumber == 3)
			TabNumber = 4;
		SetTab();
	}

	private void uxSaveAndFinish_Click(object sender, EventArgs e)
	{
		TabNumber = 4;
		SetTab();
	}

	private void uxCancel_Click(object sender, EventArgs e)
	{
		Response.Redirect("~/admin/newsletters/admin-newsletter.aspx" + (!String.IsNullOrEmpty(ReturnQueryString) ? "?" + ReturnQueryString : ""));
	}

	public void uxTitleCV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !Newsletter.NewsletterGetByTitle(uxTitle.Text).Any(t => EntityId != t.NewsletterID && !t.Deleted);
	}

	private bool ValidateAllFields()
	{
		if (TabNumber != 4)
			throw new Exception("You should only validate in Step 4: Finish & Save");

		//tabs must be visible for their controls to be validated
		Tab1.Visible = true;
		Tab2.Visible = !(m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.TextOnly));
		Tab3.Visible = !(m_NewsletterSendingType.Equals(NewsletterSendingTypeFormat.HtmlOnly));

		Page.Validate();

		//hide the tabs after validation has fired
		Tab1.Visible =
		Tab2.Visible = 
		Tab3.Visible = false;

		return Page.IsValid;
	}
}