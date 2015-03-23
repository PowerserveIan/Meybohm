using System.Configuration;

namespace Classes.Redirects
{
	public partial class Settings
	{
		/// <summary>
		/// Version number of the component
		/// </summary>
		public static string VersionNumber
		{
			get { return ConfigurationManager.AppSettings["Redirects_componentVersion"]; }
		}
	}
}