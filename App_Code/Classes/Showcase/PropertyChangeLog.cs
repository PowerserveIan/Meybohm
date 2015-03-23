using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Showcase
{
	public partial class PropertyChangeLog
	{
		public static List<PropertyChangeLog> GetChangesInDateRange(DateTime startDate, DateTime endDate)
		{
			List<PropertyChangeLog> objects;
			string key = cacheKeyPrefix + "GetChangesInDateRange_" + startDate.ToString() + "_" + endDate.ToString();

			List<PropertyChangeLog> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<PropertyChangeLog>();
				tmpList = Cache[key] as List<PropertyChangeLog>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<PropertyChangeLog> itemQuery = entity.PropertyChangeLog.Where(c => c.DateStamp >= startDate && c.DateStamp <= endDate);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}