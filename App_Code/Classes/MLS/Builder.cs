using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace Classes.MLS
{
	public partial class Builder
	{
		public static List<Builder> GetByNeighborhoodID(int neighborhoodID)
		{
			List<Builder> objects;
			string key = "MLS_NeighborhoodBuilder_GetByNeighborhoodID_" + neighborhoodID;

			List<Builder> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<Builder>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.Builder.Where(b => b.NeighborhoodBuilder.Any(n => n.NeighborhoodID == neighborhoodID)).OrderBy(b => b.Name).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<string> GetBuilderAlphabet(int cmMicrositeID)
		{
			List<string> objects;
			string key = cacheKeyPrefix + "GetBuilderAlphabet_" + cmMicrositeID;

			List<string> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<string>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.Builder.Where(b => b.Active && b.BuilderMicrosite.Any(c => c.CMMicrositeID == cmMicrositeID)).Select(b => b.Name.Substring(0, 1)).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static int SelectCount(string searchText, string letter, int cmMicrositeID)
		{
			return SelectCount();
		}

		public static List<Builder> BuilderPageForFrontend(int startRowIndex, int maximumRows, string searchText, string letter, int cmMicrositeID)
		{
			string cachingFilterText = searchText + "_" + letter + "_" + cmMicrositeID;

			List<Builder> objects;
			string baseKey = cacheKeyPrefix + "BuilderPageForFrontend_" + cachingFilterText;
			string key = baseKey + "_" + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Builder> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<Builder>;
				tmpInt = Cache[countKey] as int?;
			}

			if (tmpList != null && tmpInt.HasValue)
			{
				objects = tmpList;
				m_ItemCount = tmpInt.Value;
			}
			else
			{
				int pageNumber = maximumRows > 0 ? 1 + startRowIndex / maximumRows : 1;

				using (Entities entity = new Entities())
				{
					IQueryable<Builder> itemQuery = entity.Builder.Include("NeighborhoodBuilder").Include("NeighborhoodBuilder.Neighborhood").Where(b => b.Active && b.BuilderMicrosite.Any(c => c.CMMicrositeID == cmMicrositeID));
					if (!string.IsNullOrEmpty(searchText))
						itemQuery = itemQuery.Where(b => b.Name.ToLower().Contains(searchText.ToLower()));
					if (!string.IsNullOrEmpty(letter))
						itemQuery = itemQuery.Where(b => b.Name.ToLower().StartsWith(letter.ToLower()));

					itemQuery = itemQuery.OrderBy(b => b.Name);

					objects = maximumRows <= 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : (maximumRows <= 0 || (pageNumber == 1 && objects.Count < maximumRows) ? objects.Count : itemQuery.Count());
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}
	}
}