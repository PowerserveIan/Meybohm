using System.Collections.Generic;
using System.Linq;

namespace Classes.Showcase
{
	public partial class MediaCollection
	{
		public string MediaTypeName
		{
			get { return this.ShowcaseMediaType != null ? this.ShowcaseMediaType.Type : MediaType.GetByID(ShowcaseMediaTypeID).Type; }
		}

		public static List<MediaCollection> GetAllActiveByShowcaseItemID(int showcaseItemID)
		{
			List<MediaCollection> objects;
			string key = cacheKeyPrefix + "GetAllActiveByShowcaseItemID_" + showcaseItemID;

			List<MediaCollection> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<MediaCollection>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.MediaCollection.Where(c => c.ShowcaseItemID == showcaseItemID && c.Active && !c.IsFine && (c.ShowcaseMediaTypeID == (int)MediaTypes.TextBlock || c.ShowcaseMedia.Any(m => m.Active))).OrderBy(c => c.DisplayOrder).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

    	public static MediaCollection GetByIDAndFineStatus(int ShowcaseItemID, bool isFine, IEnumerable<string> includeList = null)
        {
            MediaCollection obj = null;
           	string key = cacheKeyPrefix + ShowcaseItemID + GetCacheIncludeText(includeList);

            MediaCollection tmpClass = null;

            if (Cache.IsEnabled)
            {
                if (Cache.IsEmptyCacheItem(key))
                   	return null;
                tmpClass = Cache[key] as MediaCollection;
            }

            if (tmpClass != null)
                obj = tmpClass;
            else
            {
                using (Entities entity = new Entities())
                {
                    IQueryable<MediaCollection> itemQuery = AddIncludes(entity.MediaCollection, includeList);
                    obj = itemQuery.FirstOrDefault(n => n.ShowcaseItemID == ShowcaseItemID
                                                    && n.IsFine == isFine);
                }
               	Cache.Store(key, obj);
            }

            return obj;
        }
	}
}