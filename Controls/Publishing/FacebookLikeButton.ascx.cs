using System;
using BaseCode;

public partial class Controls_Publishing_FacebookLikeButton : System.Web.UI.UserControl
{
	private int m_ButtonWidth = 300;
	private bool m_ShowFriendText = true;
	public string UrlToLike { get; set; }
	
	/// <summary>
	/// Width of the Like button
	/// </summary>
	public int ButtonWidth
	{
		get { return m_ButtonWidth; }
		set { m_ButtonWidth = value; }
	}

	/// <summary>
	/// If set to true, will show friends who like this
	/// </summary>
	public bool ShowFriendText
	{
		get { return m_ShowFriendText; }
		set { m_ShowFriendText = value; }
	}

	protected void Page_Load(object sender, EventArgs e)
    {
		if (!IsPostBack)
		{
			Visible = Globals.Settings.FacebookEnableLikeButton;
			if (String.IsNullOrEmpty(UrlToLike))
				UrlToLike = Request.Url.ToString();
		}
    }
}