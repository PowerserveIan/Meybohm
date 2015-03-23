using System.Configuration;

namespace Classes.SiteLanguages
{
	public partial class Settings
	{
		/// <summary>
		/// Default language. This language will be required when creating a page
		/// </summary>
		public static string DefaultLanguageCulture
		{
			get { return ConfigurationManager.AppSettings["SiteWide_defaultLanguageCulture"]; }
		}
	}
}