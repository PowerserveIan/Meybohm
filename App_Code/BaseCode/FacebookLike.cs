using System.Web.UI.HtmlControls;
using BaseCode;

public class FacebookLike
{
	public enum FBType
	{
		Activity,
		Sport,
		Bar,
		Company,
		Cafe,
		Hotel,
		Restaurant,
		Cause,
		Sports_League,
		Sports_Team,
		Band,
		Government,
		Non_Profit,
		School,
		University,
		Actor,
		Athlete,
		Author,
		Director,
		Musician,
		Politician,
		Public_Figure,
		City,
		Country,
		Landmark,
		State_Province,
		Album,
		Book,
		Drink,
		Food,
		Game,
		Product,
		Song,
		Movie,
		TV_Show,
		Article
	}

	public static void AddMetaData(System.Web.UI.Page pageEntity, string title, FBType type, string url, string imageUrl, string siteName, string description)
	{
		HtmlMeta metaTitle = new HtmlMeta();
		metaTitle.Attributes["property"] = "og:title";
		metaTitle.Content = title;
		pageEntity.Header.Controls.Add(metaTitle);

		HtmlMeta metaType = new HtmlMeta();
		metaType.Attributes["property"] = "og:type";
		metaType.Content = type.ToString().ToLower();
		pageEntity.Header.Controls.Add(metaType);

		HtmlMeta metaUrl = new HtmlMeta();
		metaUrl.Attributes["property"] = "og:url";
		metaUrl.Content = url;
		pageEntity.Header.Controls.Add(metaUrl);

		HtmlMeta metaImage = new HtmlMeta();
		metaImage.Attributes["property"] = "og:image";
		metaImage.Content = string.IsNullOrEmpty(imageUrl) ? Helpers.RootPath + Globals.Settings.MissingImagePath : imageUrl.Replace("~/", Helpers.RootPath);
		pageEntity.Header.Controls.Add(metaImage);

		HtmlMeta metaSiteName = new HtmlMeta();
		metaSiteName.Attributes["property"] = "og:site_name";
		metaSiteName.Content = siteName;
		pageEntity.Header.Controls.Add(metaSiteName);

		HtmlMeta metaDescription = new HtmlMeta();
		metaDescription.Attributes["property"] = "og:description";
		metaDescription.Content =description;
		pageEntity.Header.Controls.Add(metaDescription);

		HtmlMeta metaFBAdmins = new HtmlMeta();
		metaFBAdmins.Attributes["property"] = "fb:admins";
		metaFBAdmins.Content = Globals.Settings.FacebookAdminIDs;
		pageEntity.Header.Controls.Add(metaFBAdmins);
	} 
}