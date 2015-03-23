using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.DynamicHeader;

public partial class Controls_DynamicHeader_DynamicHeader : BaseDynamicHeader
{
	public enum TransitionStyle
	{
		Crossfade,
		Slide
	}

	private int m_ImageWidth = 600;
	private int m_ImageHeight = 400;
	private int m_ThumbnailHeight = 70;
	private int m_ThumbnailWidth = 70;
	private bool m_GenericThumbnail;
	private bool m_PlayByDefault = true;
	private bool m_ShowNavigation = true;
	private bool m_ShowThumbnails = true;
	private bool m_ShowNumbers;
	private TransitionStyle m_Transition = TransitionStyle.Crossfade;
	protected bool m_HasVideos = false;

	/// <summary>
	/// Width of the main image
	/// </summary>
	public int ImageWidth
	{
		get { return m_ImageWidth; }
		set { m_ImageWidth = value; }
	}

	/// <summary>
	/// Height of the main image
	/// </summary>
	public int ImageHeight
	{
		get { return m_ImageHeight; }
		set { m_ImageHeight = value; }
	}

	/// <summary>
	/// Height of thumbnail images
	/// </summary>
	public int ThumbnailHeight
	{
		get { return m_ThumbnailHeight; }
		set { m_ThumbnailHeight = value; }
	}

	/// <summary>
	/// Width of thumbnail images
	/// </summary>
	public int ThumbnailWidth
	{
		get { return m_ThumbnailWidth; }
		set { m_ThumbnailWidth = value; }
	}

	/// <summary>
	/// Crossfade stacks the current image and the next image on top of each other, then fades the first one out and the second one in simultaneously
	/// Slide slides the images like a carousel
	/// </summary>
	public TransitionStyle Transition
	{
		get { return m_Transition; }
		set { m_Transition = value; }
	}

	/// <summary>
	/// If you set this property, you should add a CSS image to the hyperlink with class "thumb"
	/// </summary>
	public bool GenericThumbnail
	{
		get { return m_GenericThumbnail; }
		set { m_GenericThumbnail = value; }
	}

	/// <summary>
	/// If set to true, will play the slideshow automatically upon page load
	/// </summary>
	public bool PlayByDefault
	{
		get { return m_PlayByDefault; }
		set { m_PlayByDefault = value; }
	}

	/// <summary>
	/// Show or hide left and right arrows
	/// </summary>
	public bool ShowNavigation
	{
		get { return m_ShowNavigation; }
		set { m_ShowNavigation = value; }
	}

	/// <summary>
	/// Show or hide thumbnails for fast navigation
	/// </summary>
	public bool ShowThumbnails
	{
		get { return m_ShowThumbnails; }
		set { m_ShowThumbnails = value; }
	}

	/// <summary>
	/// Show or hide slide numbers for fast navigation
	/// </summary>
	public bool ShowNumbers
	{
		get { return m_ShowNumbers; }
		set { m_ShowNumbers = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		if (ShowNumbers && ShowThumbnails)
			throw new Exception("You can not have thumbnails and numbers visible, please only choose one");
		DynamicImage.Filters filterList = new DynamicImage.Filters();
		filterList.FilterDynamicImageActive = true.ToString();
		try
		{
			if (!String.IsNullOrEmpty(CollectionName))
				filterList.FilterDynamicCollectionID = DynamicCollection.DynamicCollectionGetByName(CollectionName).FirstOrDefault().DynamicCollectionID.ToString();
			else
				filterList.FilterDynamicCollectionID = "NULL";
		}
		catch (Exception ex)
		{
			Helpers.LogException(ex);
			uxCollectionNotFound.Visible = true;
			uxSlideShowPH.Visible = false;
		}
		List<DynamicImage> allDynamicImages = DynamicImage.PageByCollectionID(0, 0, "", "DisplayOrder", true, filterList);
		if (allDynamicImages.Count > 0)
		{
			uxImagesRepeater.DataSource = allDynamicImages;
			uxImagesRepeater.DataBind();
			m_HasVideos = allDynamicImages.Exists(d => d.IsVideo);
		}
		else
			Visible = false;
		if (Visible)
		{
			string masterPageCss = ((HtmlLink)Page.Master.FindControl("uxCSSFiles")).Href;
			if (!masterPageCss.Contains(uxCSSFiles.Attributes["href"]))
				((HtmlLink)Page.Master.FindControl("uxCSSFiles")).Href = masterPageCss + (String.IsNullOrEmpty(masterPageCss) ? "" : ",") + uxCSSFiles.Attributes["href"];
			uxCSSFiles.Visible = false;
			string masterPageScript = ((Literal)Page.Master.FindControl("uxJavaScripts")).Text;
			((Literal)Page.Master.FindControl("uxJavaScripts")).Text = masterPageScript + (String.IsNullOrEmpty(masterPageScript) ? "" : ",") + uxJavaScripts.Text + (m_HasVideos ? ",~/tft-js/core/flowplayer-3.2.6.min.js" : "");
			uxJavaScripts.Visible = false;
		}
	}
}