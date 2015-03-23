using System.Collections.Generic;
using System.Linq;

namespace Classes.Showcase
{
	public partial class ShowcaseAttributeValue
	{
		protected override void ClearRelatedCacheItems()
		{
			BaseCode.Helpers.PurgeCacheItems("Showcase_ShowcaseAttribute_GetAttributesAndValuesByShowcaseItemID");
		}

		public static List<ShowcaseAttributeValue> GetAllAttributeValuesByShowcaseItemID(int showcaseItemID)
		{
			List<ShowcaseAttributeValue> objects;
			string key = cacheKeyPrefix + "GetAllAttributeValuesByShowcaseItemID_" + showcaseItemID;

			List<ShowcaseAttributeValue> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<ShowcaseAttributeValue>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.ShowcaseAttributeValue.Where(i => i.ShowcaseItemAttributeValue.Any(z => z.ShowcaseItemID == showcaseItemID)).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<string> GetAllPropertyTypes()
		{
			List<string> objects;
			string key = cacheKeyPrefix + "GetAllPropertyTypes";

			List<string> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<string>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.ShowcaseAttributeValue.Where(a => a.ShowcaseAttribute.MLSAttributeName == "Property Type").Select(a => a.Value).Distinct().ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}