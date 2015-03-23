using System.Collections.Generic;
using System.Linq;
using BaseCode;

namespace Classes.Showcase
{
	public partial class ShowcaseItemAttributeValue
	{
		//Break cache for frontend paging since it is dependant on Items and Attributes
		protected override void SaveSearch()
		{
			Helpers.PurgeCacheItems("Showcase_ShowcaseItem_GetPagedFilteredShowcaseItems_");
		}

		public static List<ShowcaseItemAttributeValue> GetItemValuesByAttributeAndShowcaseItemID(int showcaseItemID, int showcaseAttributeID)
		{
			List<ShowcaseItemAttributeValue> objects;
			string key = cacheKeyPrefix + "GetItemValuesByAttributeAndShowcaseItemID_" + showcaseItemID + "_" + showcaseAttributeID;

			List<ShowcaseItemAttributeValue> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseItemAttributeValue>();
				tmpList = Cache[key] as List<ShowcaseItemAttributeValue>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.ShowcaseItemAttributeValue.Where(s=>s.ShowcaseItemID == showcaseItemID && s.ShowcaseAttributeValue.ShowcaseAttributeID == showcaseAttributeID).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}
