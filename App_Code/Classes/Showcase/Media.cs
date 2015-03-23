using System.Linq;

namespace Classes.Showcase
{
	public partial class Media
	{
		public int MediaTypeID
		{
			get { return MediaCollection.GetByID(ShowcaseMediaCollectionID).ShowcaseMediaTypeID; }
		}

		public static int GetNumberOfPhotos(int showcaseItemID)
		{
			int obj;
			string key = cacheKeyPrefix + "GetNumberOfPhotos_" + showcaseItemID;

			int? tmpObj = null;

			if (Cache.IsEnabled)
				tmpObj = Cache[key] as int?;

			if (tmpObj.HasValue)
				obj = tmpObj.Value;
			else
			{
				using (Entities entity = new Entities())
				{
					obj = entity.Media.Count(s => s.ShowcaseMediaCollection.ShowcaseItemID == showcaseItemID && s.ShowcaseMediaCollection.Title == "Photos" && s.Active);
				}

				//All communities have at least their main image
				if (obj == 0)
					obj = 1;

				Cache.Store(key, obj);
			}
			return obj;
		}
	}
}
