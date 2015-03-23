using System.Collections.Generic;
using System.Linq;

namespace Classes.Newsletters
{
	public partial class MailingList
	{
		public static List<MailingList> GetByActiveDeleted(bool active, bool deleted)
		{
			List<MailingList> objects;
			string key = cacheKeyPrefix + "GetByActiveDeleted_" + active + deleted;

			List<MailingList> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<MailingList>;

			if (tmpList != null)
				objects = tmpList.Select(entity => new MailingList(entity)).ToList();
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.MailingList.Where(n => n.Active == active && n.Deleted == deleted).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}