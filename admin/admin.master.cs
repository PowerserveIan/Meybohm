using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using BaseCode;

public partial class admin_Master : MasterPage
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);		
		lnkIcon.Href = "~/admin/favicon.ico";		
		uxClearCaches.Visible = Page.User.IsInRole("Admin");
		decimal temp;
		if (htmlEntity != null && Request.Browser.Browser == "IE" && Decimal.TryParse(Request.Browser.Version, out temp))
			htmlEntity.Attributes["class"] = "ie" + temp.ToString().Split('.')[0];
		else if (htmlEntity != null && ((!String.IsNullOrEmpty(Request.UserAgent) && Request.UserAgent.ToLower().Contains("macintosh")) || Request.Browser.MobileDeviceManufacturer == "Apple"))
			htmlEntity.Attributes["class"] = "mac";
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (Page.User.IsInRole("Showcase Manager") && Classes.Showcase.ShowcaseHelpers.GetCurrentShowcaseID() == null && !Classes.Showcase.ShowcaseHelpers.UserCanManageOtherShowcases())
		{
			List<Classes.Showcase.ShowcaseUser> usersShowcases = Classes.Showcase.ShowcaseUser.ShowcaseUserGetByUserID(Helpers.GetCurrentUserID());
			if (usersShowcases.Count == 1)
				Classes.Showcase.ShowcaseHelpers.SetUsersCurrentShowcaseID(usersShowcases[0].ShowcaseID);
		}
		globalSettings.Visible = Page.User.IsInRole("Admin");
		DisablePageCaching();
		if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "StartupScript_" + DateTime.Now.Ticks, "MasterRunAtDocReady();", true);
		if (!IsPostBack)
		{
			try
			{
				uxNewsFeed.DataSource = Helpers.GetRssFeedAsXmlList(Globals.Settings.NewsFeed352Media, 3);
				uxNewsFeed.DataBind();
			}
			catch (Exception ex)
			{
				Helpers.LogException(ex);
				uxNewsError.Visible = true;
			}
			try
			{
				uxBlogFeed.DataSource = Helpers.GetRssFeedAsXmlList(Globals.Settings.BlogFeed352Media, 3);
				uxBlogFeed.DataBind();
			}
			catch (Exception ex)
			{
				Helpers.LogException(ex);
				uxBlogError.Visible = true;
			}
			ux352NewsLink.NavigateUrl = Globals.Settings.NewsFeed352Media;
			try
			{
				uxLatestTweet.Text = Helpers.GetRssFeedAsXmlList(Globals.Settings.TwitterFeed352Media, 1).FirstOrDefault().Element("status").Element("text").Value;
			}
			catch (Exception ex)
			{
				Helpers.LogException(ex);
				uxLatestTweet.Visible = false;
			}
			uxGiveFeedbackLink.NavigateUrl = Globals.Settings.FeedbackPostUrl352Media + "?UserID=" + Helpers.GetCurrentUserID() + "&UserName=" + Page.User.Identity.Name;
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		Helpers.GetCSSCode(uxCSSFiles);
		Helpers.GetJSCode(uxJavaScripts);
	}

	public static void DisablePageCaching()
	{
		HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
		HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
		HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
		HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
		HttpContext.Current.Response.Cache.SetNoStore();
	}

	protected void uxClearCaches_Click(object sender, EventArgs e)
	{
		Helpers.PurgeCacheItems(null);
		Response.Redirect(Request.RawUrl);
	}

	protected override void Render(HtmlTextWriter writer)
	{
		using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new System.IO.StringWriter()))
		{
			base.Render(htmlwriter);
			string html = htmlwriter.InnerWriter.ToString();
			html = html.Replace("//ajax.microsoft.com/ajax/", "//ajax.aspnetcdn.com/ajax/");
			writer.Write(html.Trim());
		}
	}
}
