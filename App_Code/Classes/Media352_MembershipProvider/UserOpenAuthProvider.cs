using System.Collections.Generic;
using System.Linq;

namespace Classes.Media352_MembershipProvider
{
	public partial class UserOpenAuthProvider
	{
		public static List<string> GetNamesOfProvidersUsed()
		{
			List<string> objects;
			string key = cacheKeyPrefix + "GetNamesOfProvidersUsed";

			List<string> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<string>();
				tmpList = Cache[key] as List<string>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.UserOpenAuthProvider.Select(o => o.ProviderName).Distinct().OrderBy(o => o).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}