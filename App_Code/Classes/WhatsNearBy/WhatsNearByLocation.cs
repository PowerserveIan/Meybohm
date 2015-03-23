using System.Collections.Generic;
using System.Linq;

namespace Classes.WhatsNearBy
{
	public partial class WhatsNearByLocation
	{
		public string ImageOrPlaceHolderSrc
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(Image))
					return Image.ToLower().StartsWith("http") ? "" : Image;
				if (WhatsNearByLocationCategory == null || !WhatsNearByLocationCategory.Any())
					WhatsNearByLocationCategory = WhatsNearBy.WhatsNearByLocationCategory.WhatsNearByLocationCategoryGetByWhatsNearByLocationID(WhatsNearByLocationID, includeList: new[] { "WhatsNearByCategory" });
				if (WhatsNearByLocationCategory.Any(w => !string.IsNullOrWhiteSpace(w.WhatsNearByCategory.PlaceholderImage)))
					return WhatsNearByLocationCategory.FirstOrDefault(w => !string.IsNullOrWhiteSpace(w.WhatsNearByCategory.PlaceholderImage)).WhatsNearByCategory.PlaceholderImage;
				return string.Empty;
			}
		}

		public static List<NearbyLocations> GetLocationsNearCoordinates(decimal latitude, decimal longitude)
		{
			int distanceAway = Classes.Showcase.Settings.DistanceForNearbyLocations;
			List<NearbyLocations> objects;
			string key = cacheKeyPrefix + "GetLocationsNearCoordinates_" + latitude + "_" + longitude + "_" + distanceAway;

			List<NearbyLocations> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<NearbyLocations>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.WhatsNearBy_GetLocationsNearCoordinates(latitude, longitude, distanceAway).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}