using System.Collections.Generic;
using System.Linq;

namespace Classes.MLS
{
	public partial class Neighborhood
	{
		public int NumberHomesAvailable { get; set; }

		public static List<Neighborhood> GetByBuilderID(int builderID)
		{
			List<Neighborhood> objects;
			string key = "MLS_NeighborhoodBuilder_GetByBuilderID_" + builderID;

			List<Neighborhood> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<Neighborhood>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.Neighborhood.Where(n => n.NeighborhoodBuilder.Any(b => b.BuilderID == builderID)).OrderBy(n => n.Name).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Neighborhood> NeighborhoodPageForFrontend(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "NeighborhoodID");

			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText);

			List<Neighborhood> objects;
			string baseKey = cacheKeyPrefix + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Neighborhood> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<Neighborhood>;
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

				objects = new List<Neighborhood>();
				using (Entities entity = new Entities())
				{
					var itemQuery = SetupQuery(entity.Neighborhood, "Neighborhood", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection).Select(n => new { Neigborhood = n, n.Address, n.Address.State, NumberHomesAvailable = n.ShowcaseItem.Count(s => ((s.NewHome || (n.ShowLotsLand && (s.ShowcaseID == (int)Showcase.MeybohmShowcases.AikenLand || s.ShowcaseID == (int)Showcase.MeybohmShowcases.AugustaLand))) && s.Active && s.Rented == false)) });
					if (!string.IsNullOrEmpty(filterList.FilterNeighborhoodZip))
						itemQuery = itemQuery.Where(n => n.Neigborhood.Address.Zip == filterList.FilterNeighborhoodZip);
					var tempList = maximumRows <= 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : (maximumRows <= 0 || (pageNumber == 1 && tempList.Count < maximumRows) ? tempList.Count : itemQuery.Count());

					foreach (var n in tempList)
					{
						Neighborhood obj = n.Neigborhood;
						obj.Address = n.Address;
						obj.Address.State = n.State;
						obj.NumberHomesAvailable = n.NumberHomesAvailable;
						objects.Add(obj);
					}
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		public partial struct Filters
		{
			public string FilterNeighborhoodZip { get; set; }

			public Dictionary<string, object> GetCustomFilterList()
			{
				filterList = GetFilterList();
				if (FilterNeighborhoodZip != null)
					filterList.Add("@FilterNeighborhoodZip", FilterNeighborhoodZip);
				return filterList;
			}
		}
	}
}