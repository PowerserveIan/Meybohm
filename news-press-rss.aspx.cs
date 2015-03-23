using System;
using System.IO;
using System.Web.UI;
using Classes.Media352_NewsPress;

public partial class NewsPressRSS : Page
{
	protected bool? m_Archived
	{
		get
		{
			bool archived;
			if (!String.IsNullOrEmpty(Request.QueryString["archived"]) && Boolean.TryParse(Request.QueryString["archived"], out archived))
				return archived;
			return null;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		NewsPress.Filters filterList = new NewsPress.Filters();
		filterList.FilterNewsPressAutoArchived = m_Archived == null ? false.ToString() : m_Archived.ToString();
		if (Settings.EnableCategories && Request.QueryString["Category"] != null && Request.QueryString["Category"] != "all")
			filterList.FilterNewsPressCategoryID = Request.QueryString["Category"];
		else
			filterList.FilterNewsPressCategoryID = null;
		filterList.FilterNewsPressActive = true.ToString();
		uxRSSRepeater.DataSource = NewsPress.NewsPressPageByNewsPressCategoryID(0, Settings.FrontEndPageSize, "", "Date", false, filterList);
		uxRSSRepeater.DataBind();
	}

	protected override void Render(HtmlTextWriter writer)
	{
		using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new StringWriter()))
		{
			base.Render(htmlwriter);
			string html = htmlwriter.InnerWriter.ToString();
			html = Classes.SEOComponent.SEOData.ReplaceHtmlWithFriendlyFilenames(html, "news-press-details.aspx");
			writer.Write(html.Trim());
		}
	}
}