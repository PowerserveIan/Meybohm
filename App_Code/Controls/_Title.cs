using System;
using System.Linq;
using System.Web.UI;
using BaseCode;

namespace Classes.ContentManager
{
	public class Title : UserControl
	{
		protected override void OnLoad(EventArgs e)
		{
			CMPage cmPage = CMSHelpers.GetCurrentRequestCMSPage();
			if (cmPage != null)
			{
				CMPageTitle pageTitle;
				if (Settings.EnableMultipleLanguages)
				{
					pageTitle = CMPageTitle.CMPageTitleGetByCMPageIDAndLanguageID(cmPage.CMPageID, Helpers.GetCurrentLanguage().LanguageID).FirstOrDefault();
					Controls.Add(new LiteralControl((pageTitle != null) ? pageTitle.Title : cmPage.Title));
				}
				else
					Controls.Add(new LiteralControl(cmPage.Title));
			}
		}
	}
}