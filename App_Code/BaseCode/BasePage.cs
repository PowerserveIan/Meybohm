using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BaseCode;
using System.Web.UI.WebControls;

/// <summary>
/// To be used with BaseMasterPage
/// </summary>
public abstract class BasePage : Page
{
	private static readonly Regex RegexBetweenTags = new Regex(@">(?! )\s+", RegexOptions.Compiled);
	private static readonly Regex RegexLineBreaks = new Regex(@"([\n\s])+?(?<= {2,})<", RegexOptions.Compiled);


	/// <summary>
	/// The literal containing links to javascript files to be added to the bundler
	/// </summary>
	protected Literal m_AdditionalJavaScriptFiles;

	/// <summary>
	/// The html link containing links to css files to be added to the bundler
	/// </summary>
	protected HtmlGenericControl m_AdditionalCssFiles;

	private bool m_EnableWhiteSpaceCompression = true;

	protected bool EnableWhiteSpaceCompression
	{
		get { return m_EnableWhiteSpaceCompression; }
		set { m_EnableWhiteSpaceCompression = value; }
	}

	/// <summary>
	/// If a canonical has not already been set by the SEO component, it will be set via whatever is passed in here.
	/// </summary>
	public string CanonicalLink { get; set; }
	/// <summary>
	/// Display Name of the Component for use with the Bottom Bar
	/// </summary>
	public string ComponentName { set { ((BaseMasterPage)Master).ComponentName = value; } }
	/// <summary>
	/// Landing page for the Component Return to Admin button in the Bottom Bar ("~/admin/" will be prepended)
	/// </summary>
	public string ComponentAdminPage { set { ((BaseMasterPage)Master).ComponentAdminPage = value; } }
	/// <summary>
	/// Can be used to pass any other URL to the bottom bar that will open up in a fancybox
	/// </summary>
	public string ComponentAdditionalLink { set { ((BaseMasterPage)Master).ComponentAdditionalLink = value; } }
	/// <summary>
	/// Primarily used by the Dynamic Header to show/hide the content placeholder
	/// </summary>
	public bool PageUsesBaseCMPage { set { ((BaseMasterPage)Master).PageUsesBaseCMPage = value; } }

	public bool? NewHomePage { set { ((BaseMasterPage)Master).NewHomePage = value; } }

	public string MicrositePath
	{
		get
		{
			Classes.ContentManager.CMMicrosite micrositeEntity = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite();
			if (micrositeEntity != null)
				return micrositeEntity.Name.ToLower().Replace(" ", "-");
			return string.Empty;
		}
	}

	public abstract void SetComponentInformation();

	protected virtual void SetCssAndJs()
	{
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
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
		}
		SetComponentInformation();
		Classes.SEOComponent.SEOData seoData = Classes.SEOComponent.SEOData.GetSEOForSpecificPath(Request.AppRelativeCurrentExecutionFilePath, Request.QueryString.ToString());
		if ((!String.IsNullOrEmpty(Request.QueryString["filename"])) || (seoData != null && !String.IsNullOrEmpty(seoData.FriendlyFilename)))
		{
			HtmlLink canon = new HtmlLink();
			canon.Attributes["rel"] = "canonical";
			if (!String.IsNullOrEmpty(Request.QueryString["filename"]))
				canon.Href = Helpers.RootPath + MicrositePath + (!String.IsNullOrWhiteSpace(MicrositePath) ? "/" : "") + Request.QueryString["filename"].ToLower();
			else
				canon.Href = Helpers.RootPath + MicrositePath + (!String.IsNullOrWhiteSpace(MicrositePath) ? "/" : "") + seoData.FriendlyFilename;
			Header.Controls.Add(canon);			
		}
		ClientScript.RegisterStartupScript(Page.GetType(), "ClearFormAction", "$(\"form\").attr(\"action\", \"\");", true);
	}

	/// <summary>
	/// Initializes the <see cref="T:System.Web.UI.HtmlTextWriter"></see> object and calls on the child
	/// controls of the <see cref="T:System.Web.UI.Page"></see> to render.
	/// </summary>
	/// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> that receives the page content.</param>
	protected override void Render(HtmlTextWriter writer)
	{
		using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new StringWriter()))
		{
			base.Render(htmlwriter);
			string html = htmlwriter.InnerWriter.ToString();
			html = Classes.SEOComponent.SEOData.ReplaceHtmlWithFriendlyFilenames(html);
			if (Globals.Settings.EnableParallelization)
			{
				html = html.Replace("\"resizer.aspx", "\"" + Globals.Settings.ResizerSubdomain + "resizer.aspx");
				html = Regex.Replace(html, Regex.Escape("\"img/"), "\"" + Globals.Settings.ResizerSubdomain + "img/", RegexOptions.IgnoreCase);
				html = Regex.Replace(html, Regex.Escape("\"uploads/"), "\"" + Globals.Settings.UploadsSubdomain + Globals.Settings.UploadFolder, RegexOptions.IgnoreCase);
				html = Regex.Replace(html, Regex.Escape("\"css/"), "\"" + Globals.Settings.CSSSubdomain + "css/", RegexOptions.IgnoreCase);
				html = Regex.Replace(html, Regex.Escape("\"HttpCombiner.ashx"), "\"" + Globals.Settings.CSSSubdomain + "HttpCombiner.ashx", RegexOptions.IgnoreCase);
			}
			html = html.Replace("//ajax.microsoft.com/ajax/", "//ajax.aspnetcdn.com/ajax/");
			if (m_EnableWhiteSpaceCompression && Request["HTTP_X_MICROSOFTAJAX"] == null)
			{
				html = RegexBetweenTags.Replace(html, ">");
				html = RegexLineBreaks.Replace(html, "<");
			}
			writer.Write(html.Trim());
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		if (!IsPostBack)
		{
			bool hasCanon = false;
			foreach (Control c in Header.Controls)
			{
				if (c is HtmlLink && ((HtmlLink)c).Attributes["rel"] == "canonical")
				{
					hasCanon = true;
					break;
				}
			}
			if (!hasCanon)
			{
				HtmlLink canon = new HtmlLink();
				canon.Attributes["rel"] = "canonical";
				canon.Href = !String.IsNullOrEmpty(CanonicalLink) ? CanonicalLink : Request.AppRelativeCurrentExecutionFilePath.ToLower().Replace("~/", Helpers.RootPath).Replace("/default.aspx", "/");
				Header.Controls.Add(canon);
			}
		}
	}
}