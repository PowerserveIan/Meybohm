using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using BaseCode;
using System.Web;

public partial class Controls_BaseControls_BaseAdminQuickView : UserControl
{
	private const string m_BaseVideoAndGuideURL = "http://userguides.352media.com/";
	private ITemplate m_ContentArea;
	/// <summary>
	/// Name of the component as it will appear in the display
	/// </summary>
	public string ComponentName { get; set; }
	/// <summary>
	/// Location of component's Admin folder and menu control
	/// </summary>
	public string ComponentFolderLocation { get; set; }

	public enum ColumnsWide
	{
		One,
		Two,
		Three
	}

	private ColumnsWide m_NumberColumnsWide = ColumnsWide.One;
	public ColumnsWide NumberColumnsWide
	{
		get { return m_NumberColumnsWide; }
		set { m_NumberColumnsWide = value; }
	}
	/// <summary>
	/// Specify this only if the component does not have an Admin folder.  This link will display in the Manage dropdown.
	/// </summary>
	public string ManageLink { get; set; }
	/// <summary>
	/// Version number of the component.  Pulled from appSettings.
	/// </summary>
	public string ComponentVersionNumber { get; set; }

	[TemplateContainer(typeof(ContentAreaContainer))]
	[PersistenceMode(PersistenceMode.InnerProperty)]
	public ITemplate ContentAreaTemplate
	{
		get { return m_ContentArea; }
		set { m_ContentArea = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		if (m_ContentArea == null)
			throw new Exception("You don't have any content for the " + ComponentName + " component Admin Quick View");
		if (!IsPostBack && !Directory.Exists(Server.MapPath(ComponentFolderLocation)))
			throw new Exception("The component folder location you specified does not exist: " + ComponentFolderLocation);
		ContentAreaContainer container = new ContentAreaContainer();
		m_ContentArea.InstantiateIn(container);
		uxContentArea.Controls.Add(container);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		this.Visible = Page.User.IsInRole("Admin");
		if (!IsPostBack)
		{
			if (!String.IsNullOrEmpty(ComponentFolderLocation))
			{
				//Render Menu control so it shows only links the user should be seeing
				Control menuControl = Page.LoadControl(ComponentFolderLocation + "menu.ascx");
				StringBuilder sb = new StringBuilder();
				StringWriter tw = new StringWriter(sb);
				HtmlTextWriter hw = new HtmlTextWriter(tw);

				menuControl.RenderControl(hw);

				Regex r = new Regex("<a[^>]*href=[\"'](?<url>[^\"]+[.\\s]*)[\"'][^>]*>(?<name>[^<]+[.\\s]*)</a>");
				foreach (Match m in r.Matches(sb.ToString()))
				{
					uxManageLinks.Text += m.Value;
				}
			}
			else if (!String.IsNullOrEmpty(ManageLink))
				uxManageLinks.Text = "<a href=\"" + Page.ResolveClientUrl(ManageLink) + "\">Manage " + ComponentName + "</a>";
			else
				uxManagePH.Visible = false;
		}
	}

	#region Nested type: ContentAreaContainer

	public class ContentAreaContainer : Control, INamingContainer
	{
		internal ContentAreaContainer()
		{
		}
	}

	#endregion
}