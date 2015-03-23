using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.DynamicHeader
{
	public partial class DynamicImage
	{
		public int DisplayOrder { get; set; }

		public static List<DynamicImage> PageByCollectionIDWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters())
		{
			List<DynamicImage> objects = PageByCollectionID(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<DynamicImage> PageByCollectionID(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "DynamicImageID");

			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText);

			List<DynamicImage> objects;
			string baseKey = cacheKeyPrefix + "PageByCollectionID_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<DynamicImage> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<DynamicImage>;
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
				int collectionID = !String.IsNullOrEmpty(filterList.FilterDynamicCollectionID) && filterList.FilterDynamicCollectionID != "NULL" ? Convert.ToInt32(filterList.FilterDynamicCollectionID) : 0;

				using (Entities entity = new Entities())
				{
					IQueryable<DynamicImage> itemQuery = SetupQuery(entity.DynamicImage, "DynamicImage", filterList.GetFilterList(), searchText, m_LikeSearchProperties, string.Empty, sortDirection, new string[] { "DynamicImageCollection" });
					
					if (sortField == "DisplayOrder" && sortDirection)
						itemQuery = itemQuery.OrderBy(i => i.DynamicImageCollection.FirstOrDefault(c => c.DynamicCollectionID == collectionID).DisplayOrder);
					else if (sortField == "DisplayOrder")
						itemQuery = itemQuery.OrderByDescending(i => i.DynamicImageCollection.FirstOrDefault(c => c.DynamicCollectionID == collectionID).DisplayOrder);
					else
						itemQuery = SetupOrderByClause(itemQuery, sortField, sortDirection);
					if (filterList.FilterDynamicCollectionID == "NULL")
						itemQuery = itemQuery.Where(i => !i.DynamicImageCollection.Any());
					else if (!String.IsNullOrEmpty(filterList.FilterDynamicCollectionID))
						itemQuery = itemQuery.Where(i => i.DynamicImageCollection.Any(c => c.DynamicCollectionID == collectionID));

					objects = maximumRows == 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					if (collectionID > 0)
					{
						foreach (DynamicImage obj in objects)
						{
							obj.DisplayOrder = obj.DynamicImageCollection.FirstOrDefault(c => c.DynamicCollectionID == collectionID).DisplayOrder;
						}
					}
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : itemQuery.Count();
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		public partial struct Filters
		{
			public string FilterDynamicCollectionID { get; set; }

			public Dictionary<string, object> GetCustomFilterList()
			{
				filterList = GetFilterList();
				if (FilterDynamicCollectionID != null)
					filterList.Add("@FilterDynamicCollectionID", FilterDynamicCollectionID);
				return filterList;
			}
		}
	}
}