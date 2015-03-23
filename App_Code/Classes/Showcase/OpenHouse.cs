using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Showcase
{
	public partial class OpenHouse
	{
		public static void AddRecurringOpenHouses(string dateString, int showcaseItemID)
		{
			using (Entities entity = new Entities())
			{
				entity.OpenHouse_AddRecurringOpenHouses(dateString, showcaseItemID);
			}
			ClearCache();
		}

		public static List<OpenHouse> GetFutureOpenHouses(int showcaseItemID, DateTime? endDate = null)
		{
			List<OpenHouse> objects;
			string key = cacheKeyPrefix + "GetFutureOpenHouses_" + showcaseItemID + "_" + endDate;

			List<OpenHouse> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<OpenHouse>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.OpenHouse.Where(o => o.ShowcaseItemID == showcaseItemID && (o.BeginDate >= DateTime.UtcNow || o.EndDate >= DateTime.UtcNow) && (!endDate.HasValue || o.EndDate <= endDate.Value)).OrderBy(o => o.BeginDate).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}