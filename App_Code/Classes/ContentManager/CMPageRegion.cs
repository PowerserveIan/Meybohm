using System;
using System.Linq;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class CMPageRegion
	{
		public static CMPageRegion LoadContentRegion(Filters filterList = new Filters())
		{
			int defaultLanguageID = Helpers.GetDefaultLanguageID();
			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), string.Empty);

			CMPageRegion obj = null;
			string key = cacheKeyPrefix + "LoadContentRegion_" + cachingFilterText + "_" + defaultLanguageID;

			CMPageRegion tmpObj = null;

			if (Cache.IsEnabled)
				tmpObj = Cache[key] as CMPageRegion;

			if (tmpObj != null)
				obj = tmpObj;
			else
			{
				using (Entities entity = new Entities())
				{
					obj = entity.CMS_LoadContentRegion(filterList.FilterCMPageRegionCMPageID, filterList.FilterCMPageRegionCMRegionID, filterList.FilterCMPageRegionLanguageID, filterList.FilterCMPageRegionUserID, filterList.FilterCMPageRegionCreated, (!String.IsNullOrEmpty(filterList.FilterCMPageRegionNeedsApproval) ? (bool?)Convert.ToBoolean(filterList.FilterCMPageRegionNeedsApproval) : null), defaultLanguageID).FirstOrDefault();
					if (obj != null)
						obj.ContentClean = null;
				}

				Cache.Store(key, obj);
			}
			return obj;
		}
	}
}
