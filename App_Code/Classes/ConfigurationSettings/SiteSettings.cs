using System.Collections.Generic;
using System.Linq;

namespace Classes.ConfigurationSettings
{
	public partial class SiteSettings
	{
		/// <summary>
		/// The data type of the setting, populated only by GetAllWithTypeByCustom
		/// </summary>
		public virtual string Type
		{
			get { return this.SiteSettingsDataType.Type; }
		}

		/// <summary>
		/// Returns setting name and value pairs
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, string> GetSettingKeyValuePair()
		{
			const string key = cacheKeyPrefix + "GetSettingKeyValuePair";

			Dictionary<string, string> tmpObjects = null;

			if (Cache.IsEnabled)
				tmpObjects = Cache[key] as Dictionary<string, string>;

			Dictionary<string, string> objects = tmpObjects ?? PopulateSettingsCache();
			return objects;
		}

		/// <summary>
		/// Populates the cache to save database hits
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, string> PopulateSettingsCache()
		{
			const string key = cacheKeyPrefix + "GetSettingKeyValuePair";
			Dictionary<string, string> objects = new Dictionary<string, string>();
			Dictionary<string, string> tmpList = null;
			if (Cache.IsEnabled)
				tmpList = Cache[key] as Dictionary<string, string>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.SiteSettings.Include("SiteComponent").Select(s => new
					{
						Key = s.SiteComponent.ComponentName + "_" + s.Setting,
						Value = s.Value
					}).AsEnumerable().ToDictionary(o => o.Key, o => o.Value);
				}

				Cache.Store(key, objects);
			}
			return objects;
		}

		/// <summary>
		/// Returns all settings for a component with their data types, filtered by isCustom
		/// </summary>
		/// <param name="componentID"></param>
		/// <param name="isCustom"></param>
		/// <returns></returns>
		public static List<SiteSettings> GetAllWithTypeByCustom(int componentID, bool isCustom)
		{
			List<SiteSettings> objects;
			string key = cacheKeyPrefix + "GetAllWithTypeByCustom_" + componentID + "_" + isCustom;

			List<SiteSettings> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<SiteSettings>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.SiteSettings.Include("SiteSettingsDataType").Where(s => s.SiteComponentID == componentID && s.IsCustom == isCustom).OrderBy(s => s.DisplayOrder).ToList();
				}

				Cache.Store(key, objects);
			}
			return objects;
		}

		public static List<SiteSettings> GetByComponentName(string componentName)
		{
			List<SiteSettings> objects;
			string key = cacheKeyPrefix + "GetByComponentName_" + componentName;

			List<SiteSettings> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<SiteSettings>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.SiteSettings.Where(s => s.SiteComponent.ComponentName == componentName).ToList();
				}

				Cache.Store(key, objects);
			}
			return objects;
		}
	}
}