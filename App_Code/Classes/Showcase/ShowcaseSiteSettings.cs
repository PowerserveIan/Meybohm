using System.Collections.Generic;
using System.Linq;

namespace Classes.Showcase
{
	public partial class ShowcaseSiteSettings
	{
		/// <summary>
		/// Returns setting name and value pairs
		/// </summary>
		public static Dictionary<string, string> GetSettingKeyValuePair(int showcaseID)
		{
			Dictionary<string, string> objects;
			string key = cacheKeyPrefix + "GetSettingKeyValuePair_" + showcaseID;

			Dictionary<string, string> tmpObjects = null;

			if (Cache.IsEnabled)
				tmpObjects = Cache[key] as Dictionary<string, string>;

			objects = tmpObjects ?? PopulateSettingsCache(showcaseID);
			return objects;
		}

		/// <summary>
		/// Populates the cache to save database hits
		/// </summary>
		public static Dictionary<string, string> PopulateSettingsCache(int showcaseID)
		{
			string key = cacheKeyPrefix + "GetSettingKeyValuePair_" + showcaseID;
			Dictionary<string, string> objects = new Dictionary<string, string>();

			using (Entities entity = new Entities())
			{
				objects = entity.ShowcaseSiteSettings.Include("SiteSettings").Include("SiteComponent").Where(s => s.ShowcaseID == showcaseID).Select(s => new
				{
					Key = s.SiteSettings.SiteComponent.ComponentName + "_" + s.SiteSettings.Setting,
					Value = s.Value
				}).AsEnumerable().ToDictionary(o => o.Key, o => o.Value);
			}

			Cache.Store(key, objects);
			return objects;
		}
	}
}