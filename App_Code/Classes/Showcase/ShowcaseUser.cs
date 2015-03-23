using System.Collections.Generic;
using System.Linq;

namespace Classes.Showcase
{
	public partial class ShowcaseUser
	{
		public string UserName { get { return this.User.Name; } }
		public string Email { get { return this.User.Email; } }

		public static List<ShowcaseUser> GetShowcaseUserForAdmin(int showcaseID)
		{
			List<ShowcaseUser> objects;
			string key = cacheKeyPrefix + "GetShowcaseUserForAdmin_" + showcaseID;

			List<ShowcaseUser> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<ShowcaseUser>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.ShowcaseUser.Include("User").Where(c => c.ShowcaseID == showcaseID).OrderBy(c => c.User.Name).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}