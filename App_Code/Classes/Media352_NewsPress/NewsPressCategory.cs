using System.Collections.Generic;
using System.Linq;

namespace Classes.Media352_NewsPress
{
	public partial class NewsPressCategory
	{
		/// <summary>
		/// Used by Search component to add Category names as Keywords
		/// </summary>
		public static List<string> GetAllCategoryNamesByNewsPressID(int newsPressID)
		{
			List<string> objects;
			string key = cacheKeyPrefix + "GetAllCategoryNamesByNewsPressID_" + newsPressID;

			List<string> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<string>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.NewsPressNewsPressCategory.Where(n => n.NewsPressID == newsPressID).Select(n => n.NewsPressCategory.Name).ToList();
				}

				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}