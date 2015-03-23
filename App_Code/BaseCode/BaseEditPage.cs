using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

public class BaseEditPage : Page
{
	#region Members

	/// <summary>
	/// The add new button for the edit page
	/// </summary>
	protected HyperLink m_AddNewButton;

	protected Literal m_AddOrEdit;

	protected PlaceHolder m_AfterSavePH;

	protected Literal m_BreadCrumbTitle;

	/// <summary>
	/// The literal containing links to javascript files to be added to the bundler
	/// </summary>
	protected Literal m_AdditionalJavaScriptFiles;

	/// <summary>
	/// The html link containing links to css files to be added to the bundler
	/// </summary>
	protected HtmlGenericControl m_AdditionalCssFiles;

	/// <summary>
	/// The placeholder to inject the bottom buttons into
	/// </summary>
	protected PlaceHolder m_ButtonContainer;

	/// <summary>
	/// The cancel button for the edit page
	/// </summary>
	protected LinkButton m_CancelButton;

	/// <summary>
	/// Used for save button and messages
	/// </summary>
	protected string m_ClassName;

	/// <summary>
	/// Used for success message
	/// </summary>
	protected string m_ClassTitle;

	protected bool m_CollapseFormAfterSave = true;

	protected Literal m_CustomBreadCrumbsLiteral;
	protected PlaceHolder m_CustomBreadCrumbsPH;

	/// <summary>
	/// Boolean value used to populate add/edit address
	/// </summary>
	protected bool m_IdIsGuid;

	/// <summary>
	/// Link pointing to the edit page (with id parameter as the querystring)
	/// </summary>
	protected string m_LinkToEditPage;

	/// <summary>
	/// Link pointing to the listing page (with id parameter as the querystring)
	/// </summary>
	protected string m_LinkToListingPage;

	protected Literal m_MainSectionTitle;

	/// <summary>
	/// Literal inserted immediately after h1 for title
	/// </summary>
	protected Literal m_PostTitleText;

	/// <summary>
	/// The save button for the edit page
	/// </summary>
	protected Button m_SaveButton;

	/// <summary>
	/// The save and add new button for the edit page
	/// </summary>
	protected Button m_SaveAndAddNewButton;

	protected Panel m_SavePanel;

	protected HtmlGenericControl m_SectionTitleContainer;

	/// <summary>
	/// The success message literal
	/// </summary>
	protected Literal m_SuccessMessageLiteral;

	/// <summary>
	/// The success message placeholder
	/// </summary>
	protected PlaceHolder m_SuccessMessagePlaceholder;

	/// <summary>
	/// The placholder to inject title, breadcrumbs, validation and success messages into
	/// </summary>
	protected PlaceHolder m_Header;

	/// <summary>
	/// The validation summary of the page
	/// </summary>
	protected ValidationSummary m_ValidationSummary;

	#endregion

	#region Properties

	protected int EntityId
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

	protected Guid? EntityGuid
	{
		get
		{
			if (ViewState["EntityGuid"] == null || ViewState["EntityGuid"].ToString() == Guid.Empty.ToString())
			{
				Guid tempID;
				if (!String.IsNullOrEmpty(Request.QueryString["id"]) && Guid.TryParse(Request.QueryString["id"], out tempID))
					return tempID;

				return null;
			}
			return (Guid?)ViewState["EntityGuid"];
		}
		set { ViewState["EntityGuid"] = value; }
	}

	protected string ReturnQueryString
	{
		get
		{
			NameValueCollection myQueryString = Request.QueryString.Duplicate();
			myQueryString.Remove("id");
			myQueryString.Remove("saved");
			return Helpers.WriteQueryString(myQueryString, new System.Text.StringBuilder());
		}
	}

	protected bool NewRecord
	{
		get { return ViewState["NewRecord"] != null && Convert.ToBoolean(ViewState["NewRecord"]); }
		set { ViewState["NewRecord"] = value; }
	}

	protected bool FrontendView
	{
		get { return !String.IsNullOrEmpty(Request.QueryString["frontendView"]); }
	}
	#endregion

	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		if (Master != null)
		{
			SetCssAndJs();
			if (m_AdditionalJavaScriptFiles != null)
			{
				string masterPageScript = ((Literal)Master.FindControl("uxJavaScripts")).Text;
				((Literal)Master.FindControl("uxJavaScripts")).Text = masterPageScript + (String.IsNullOrEmpty(masterPageScript) ? "" : ",") + m_AdditionalJavaScriptFiles.Text;
				m_AdditionalJavaScriptFiles.Visible = false;
			}
			if (m_AdditionalCssFiles != null)
			{
				string masterPageCss = ((HtmlLink)Master.FindControl("uxCSSFiles")).Href;
				((HtmlLink)Master.FindControl("uxCSSFiles")).Href = masterPageCss + (String.IsNullOrEmpty(masterPageCss) ? "" : ",") + m_AdditionalCssFiles.Attributes["href"];
				m_AdditionalCssFiles.Visible = false;
			}
			if (FrontendView && Master != null)
			{
				Master.FindControl("ContentMenu").Visible = false;
				string masterPageCss = ((HtmlLink)Master.FindControl("uxCSSFiles")).Href;
				((HtmlLink)Master.FindControl("uxCSSFiles")).Href = masterPageCss.Replace("~/admin/css/structure.css", "~/admin/css/structureModal.css");
			}
		}
	}

	protected virtual void SetCssAndJs()
	{
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		m_LinkToEditPage = Path.GetFileName(Request.PhysicalPath) + "?id=";
		if (m_Header != null)
		{
			m_AddOrEdit = new Literal();
			m_BreadCrumbTitle = new Literal();
			CustomValidator clientSideValidationEventCapturer = new CustomValidator { ID = "ClientSideValidationEventCapturer", Display = ValidatorDisplay.None, ClientValidationFunction = "ClearSuccess" };
			m_ValidationSummary = new ValidationSummary { ID = "uxErrorSummary", DisplayMode = ValidationSummaryDisplayMode.BulletList, CssClass = "validation", HeaderText = "<h3 class='failure'>Please correct the following errors:</h3>" };
			m_SuccessMessagePlaceholder = new PlaceHolder();
			m_SuccessMessageLiteral = new Literal();
			m_SuccessMessagePlaceholder.Controls.Add(new Literal { Text = @"<h3 class=""success"">" });
			m_SuccessMessagePlaceholder.Controls.Add(m_SuccessMessageLiteral);
			m_SuccessMessagePlaceholder.Controls.Add(new Literal { Text = @"</h3>" });

			m_Header.Controls.Add(new Literal
			{
				Text = @"<div class=""title"">
						<h1>"
			});
			m_Header.Controls.Add(m_AddOrEdit);
			m_Header.Controls.Add(new Literal
			{
				Text = @"</h1>"
			});
			if (m_PostTitleText != null)
				m_Header.Controls.Add(m_PostTitleText);

			m_Header.Controls.Add(new Literal
			{
				Text = @"<a href=""" + m_LinkToListingPage + ReturnQueryString + @""" class=""icon back"">Return to " + m_ClassName + @" Manager</a>
					</div>
					<ul class=""breadcrumbs clearfix"">
						<li class=""firstBreadcrumb"">
							<a href=""" + ResolveClientUrl("~/admin/") + @""" title=""Home"">Dashboard</a></li>"
			});
			if (m_CustomBreadCrumbsPH != null)
			{
				m_CustomBreadCrumbsLiteral = new Literal();
				m_Header.Controls.Add(m_CustomBreadCrumbsLiteral);
			}
			m_Header.Controls.Add(new Literal
			{
				Text = @"<li>
							<a href=""" + m_LinkToListingPage + ReturnQueryString + @""">" + m_ClassName + @" Manager</a></li>
						<li class=""currentBreadcrumb"">"
			});
			m_Header.Controls.Add(m_BreadCrumbTitle);
			m_Header.Controls.Add(new Literal
			{
				Text = @"</li>
					</ul><a id='" + Helpers.PageView.PageAnchors.center + @"'></a>"
			});
			m_Header.Controls.Add(clientSideValidationEventCapturer);
			m_Header.Controls.Add(m_ValidationSummary);
			m_Header.Controls.Add(m_SuccessMessagePlaceholder);

			if (m_CollapseFormAfterSave)
			{
				m_AfterSavePH = new PlaceHolder();
				m_AfterSavePH.Visible = false;
				m_AfterSavePH.Controls.Add(new Literal
				{
					Text = @"<div id='afterSaveButtons' class='buttons'><h4>What would you like to do next?</h4>"
				});
				m_AddNewButton = new HyperLink { CssClass = "button add", NavigateUrl = GetAddUrl(), Text = "<span>Add New " + m_ClassName + "</span>" };
				m_AfterSavePH.Controls.Add(m_AddNewButton);
				m_AfterSavePH.Controls.Add(new Literal
				{
					Text = @"<a class='button edit' href='#' id='editToggle'><span>Edit " + m_ClassName + @" Info</span></a>
					<a class='button back' href='" + m_LinkToListingPage + ReturnQueryString + @"'><span>Back to " + m_ClassName + @" Manager</span></a></div>"
				});
				m_Header.Controls.Add(m_AfterSavePH);
			}

			m_SectionTitleContainer = new HtmlGenericControl("div");
			m_SectionTitleContainer.ID = "sectionTitleContainer";
			m_SectionTitleContainer.ClientIDMode = System.Web.UI.ClientIDMode.Static;
			m_SectionTitleContainer.Controls.Add(new Literal
			{
				Text = @"<!-- required fields -->
		<div class=""requiredFields"">
			<span class=""asterisk"">*</span> required fields</div>
		<!-- error message --><div class=""sectionTitle"">
			<div class=""bottom"">
				<h2>"
			});

			m_MainSectionTitle = new Literal { Text = m_ClassName + " Information" };
			m_SectionTitleContainer.Controls.Add(m_MainSectionTitle);
			m_SectionTitleContainer.Controls.Add(new Literal
			{
				Text = @"</h2>
			</div>
		</div>"
			});
			m_Header.Controls.Add(m_SectionTitleContainer);
		}
		if (m_ButtonContainer != null)
		{
			m_CancelButton = new LinkButton { ID = "uxCancel", CausesValidation = false, Text = "Return to " + m_ClassName + " Manager", CssClass = "icon back" };
			m_SaveButton = new Button { ID = "uxSave", Text = "Save New " + m_ClassName, CssClass = "button save" };
			m_SaveAndAddNewButton = new Button { ID = "uxSaveAndAddNew", Text = "Save and Add New " + m_ClassName, CssClass = "button saveAdd" };
			m_ButtonContainer.Controls.Add(m_SaveButton);
			m_ButtonContainer.Controls.Add(m_SaveAndAddNewButton);
			m_ButtonContainer.Controls.Add(new Literal { Text = @"<div class=""clear""></div>" });
			m_ButtonContainer.Controls.Add(m_CancelButton);
		}
		if (m_SaveButton != null)
			m_SaveButton.Click += SaveButton_Click;
		if (m_SaveAndAddNewButton != null)
			m_SaveAndAddNewButton.Click += SaveAndAddNewButton_Click;
		if (m_CancelButton != null)
			m_CancelButton.Click += CancelButton_Click;
		if (FrontendView)
			m_CancelButton.Visible = m_SaveAndAddNewButton.Visible = false;
		m_AddOrEdit.Text = m_BreadCrumbTitle.Text = "Add New " + m_ClassName;

		if (String.IsNullOrEmpty(Page.Title))
			Page.Title = "Admin - " + m_ClassName + " Edit";
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (String.IsNullOrEmpty(m_ClassName))
			throw new Exception("A programmer needs to correctly set up the success message information: m_ClassName is missing");
		if (!IsPostBack)
		{
			if ((!m_IdIsGuid && EntityId < 0) || (m_IdIsGuid && EntityGuid == null))
				Response.Redirect(m_LinkToListingPage + ReturnQueryString);
			if ((!m_IdIsGuid && EntityId > 0) || (m_IdIsGuid && EntityGuid != Guid.Empty))
			{
				if (m_SaveButton != null)
					m_SaveButton.Text = "Save " + m_ClassName;
				m_AddOrEdit.Text = m_BreadCrumbTitle.Text = "Edit " + m_ClassName;
			}
		}

		if (m_SuccessMessagePlaceholder != null)
			m_SuccessMessagePlaceholder.Visible = false;

		//So that an error and success message will not show at the same time.
		ClientScript.RegisterClientScriptBlock(typeof(string), "ClearSuccess", @"function ClearSuccess(sender, args){$(""div.successMessage"").hide();args.IsValid = true;}", true);
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		if (m_CustomBreadCrumbsPH != null && !IsPostBack)
		{
			m_CustomBreadCrumbsLiteral.Text = Helpers.RenderHtmlAsString(m_CustomBreadCrumbsPH);
			m_CustomBreadCrumbsPH.Visible = false;
		}
	}

	protected virtual void Save()
	{
	}

	protected virtual void SaveButton_Click(object sender, EventArgs e)
	{
		Save();
		if (Page.IsValid)
		{
			if (FrontendView)
				ClientScript.RegisterStartupScript(Page.GetType(), "CloseFancyboxReloadPage", "$(document).ready(function(){parent.window.location.reload();parent.$.fancybox.close();});", true);
			else
			{
				if (m_SuccessMessagePlaceholder != null)
					m_SuccessMessagePlaceholder.Visible = true;

				m_SuccessMessageLiteral.Text = @"The " + (NewRecord ? @"new " : "") + m_ClassName + (!String.IsNullOrEmpty(m_ClassTitle) ? @" """ + m_ClassTitle + @"""" : "") + @" has been successfully <u>" + (NewRecord ? @"added" : @"updated") + @"</u>.";
				if (NewRecord)
				{
					NewRecord = false;
					if (m_AddNewButton != null)
						m_AddNewButton.Visible = true;
					m_SaveButton.Text = "Save " + m_ClassName;
					m_SaveButton.CssClass = "button save";
				}

				Helpers.PageView.Anchor(Page, Helpers.PageView.PageAnchors.center);
				SetCollapseForm();
			}
		}
	}

	protected void SetCollapseForm()
	{
		if (m_AfterSavePH != null)
		{
			m_AfterSavePH.Visible = m_CollapseFormAfterSave;
			if (m_CollapseFormAfterSave)
			{
				ClientScript.RegisterStartupScript(Page.GetType(), "ToggleEdit", "document.getElementById('" + m_SectionTitleContainer.ClientID + "').style.display = 'none';document.getElementById('" + m_SavePanel.ClientID + "').style.display = 'none';$(document).ready(function(){$('#afterSaveButtons a.edit').click(function(){$('#" + m_SectionTitleContainer.ClientID + ",#" + m_SavePanel.ClientID + ", h3.success').slideToggle();$('#afterSaveButtons').remove();return false;});});", true);
			}
		}
	}

	protected virtual void SaveAndAddNewButton_Click(object sender, EventArgs e)
	{
		Save();
		if (Page.IsValid)
			Response.Redirect(String.Format("{0}{1}", GetAddUrl(), "&saved=true"));
	}

	protected virtual void LoadData()
	{
	}

	protected virtual string GetAddUrl()
	{
		return String.Format(m_LinkToEditPage + "{0}{1}", (m_IdIsGuid ? "00000000-0000-0000-0000-000000000000" : "0"), !String.IsNullOrEmpty(ReturnQueryString) ? "&" + ReturnQueryString.TrimStart('?') : "");
	}

	protected virtual void CancelButton_Click(object sender, EventArgs e)
	{
		Response.Redirect(m_LinkToListingPage + ReturnQueryString);
	}
}