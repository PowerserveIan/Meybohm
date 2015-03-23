using System;
using System.Collections.Generic;
using System.Web.UI;
using Classes.Showcase;

public partial class Controls_Showcase_MediaCollection : UserControl
{
	public MediaTypes MediaType { get; set; }

	public int MediaCollectionID { get; set; }

	protected void Page_Load(object sender, EventArgs e)
	{
		if (Visible)
		{
			Media.Filters filterList = new Media.Filters();
			filterList.FilterMediaActive = true.ToString();
			filterList.FilterMediaShowcaseMediaCollectionID = MediaCollectionID.ToString();
			List<Media> mediaList = Media.MediaPage(0, 0, "", "DisplayOrder", true, filterList);
			if (mediaList.Count > 1)
			{
				uxSliderItemsRepeater.DataSource = mediaList;
				uxSliderItemsRepeater.DataBind();
			}
		}
	}
}