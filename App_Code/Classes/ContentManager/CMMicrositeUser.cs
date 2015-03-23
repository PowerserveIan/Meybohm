using System.Collections.Generic;
using System.Linq;

namespace Classes.ContentManager
{
	public partial class CMMicrositeUser
	{
		public static List<CMMicrositeUser> GetMicrositeAdminsForMicrosite(int micrositeID)
		{
			List<CMMicrositeUser> objects;
			string key = cacheKeyPrefix + "GetMicrositeAdminsForMicrosite_" + micrositeID;

			List<CMMicrositeUser> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<CMMicrositeUser>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.CMMicrositeUser.Include("User").Where(c => c.CMMicrositeID == micrositeID).OrderBy(c => c.User.Name).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}