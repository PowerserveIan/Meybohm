using System;
using System.Linq;
using System.Xml.Linq;
using BaseCode;

public partial class Admin_Default : System.Web.UI.Page
{
	protected int numberOfComponentItems = 5;

	protected void Page_Load(object sender, EventArgs e)
	{
		//try
		//{
		//    //Uncomment the lines below for components your site has installed.
		//    string componentQueryString = ("?"
		//                                   + "&blog=" + BlogEngine.Core.Settings.VersionNumber
		//                                   + "&cms=" + Classes.ContentManager.Settings.VersionNumber
		//                                   + "&DynamicHeader=" + Classes.DynamicHeader.Settings.VersionNumber
		//                                   + "&ecommerce=" + Classes.Ecommerce.Settings.VersionNumber
		//                                   + "&events=" + Classes.Events.Settings.VersionNumber
		//                                   + "&filelibrary=" + Classes.FileLibrary.Settings.VersionNumber
		//                                   + "&forum=" + aspnetforum.Settings.VersionNumber
		//                                   + "&membersarea=" + Classes.Media352_MembershipProvider.Settings.VersionNumber
		//                                   + "&newspress=" + Classes.Media352_NewsPress.Settings.VersionNumber
		//                                   + "&newsletter=" + Classes.Newsletters.Settings.VersionNumber
		//                                   + "&openpayment=" + Classes.OpenPayments.Settings.VersionNumber
		//                                   + "&paymentgateway=" + Classes.PaymentGateway.Settings.VersionNumber
		//                                   + "&polls=" + Classes.Polls.Settings.VersionNumber
		//                                   + "&productcatalog=" + Classes.ProductCatalog.Settings.VersionNumber
		//                                   + "&search=" + Classes.SearchComponent.Settings.VersionNumber
		//                                   + "&showcase=" + Classes.Showcase.Settings.VersionNumber
		//                                   + "&beforeAndAfter=" + Classes.BeforeAndAfter.Settings.VersionNumber
		//                                  ).Replace("?&", "?").TrimEnd('?');
		//    XElement componentFeed = XElement.Load(Globals.Settings.ComponentFeed352Media + componentQueryString);
		//    var componentItems = from item in componentFeed.Elements("component")
		//                         orderby Guid.NewGuid()
		//                         select item;
		//    if (componentItems.Count() > 0)
		//    {
		//        uxComponentFeed.DataSource = componentItems.Take(numberOfComponentItems);
		//        uxComponentFeed.DataBind();
		//    }
		//    else
		//        uxUpdatesPH.Visible = false;
		//}
		//catch (Exception ex)
		//{
		//    Helpers.LogException(ex);
		//    uxComponentFeedError.Visible = true;
		//}		
	}
}