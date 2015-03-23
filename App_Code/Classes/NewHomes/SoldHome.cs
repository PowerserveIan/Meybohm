using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.NewHomes
{
	public partial class SoldHome
	{
		public string ListingAgentFirstAndLast { get { return this.ListingAgent != null && this.ListingAgent.UserInfo.Any() ? this.ListingAgent.UserInfo.FirstOrDefault().FirstAndLast : string.Empty; } }

		public static List<SoldHome> SoldHomePageForAdminWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<SoldHome> objects = SoldHomePageForAdmin(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<SoldHome> SoldHomePageForAdmin(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SoldHomeID");

			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText, includeList);

			List<SoldHome> objects;
			string baseKey = cacheKeyPrefix + "SoldHomePageForAdmin_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<SoldHome> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SoldHome>();
				tmpList = Cache[key] as List<SoldHome>;
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
					IQueryable<SoldHome> itemQuery = SetupQuery(entity.SoldHome, "SoldHome", filterList.GetFilterList(), string.Empty, new List<string>(), sortField, sortDirection, includeList);
					if (filterList.FilterShowcaseID.HasValue)
						itemQuery = itemQuery.Where(h => h.ShowcaseItem.ShowcaseID == filterList.FilterShowcaseID.Value);
					if (filterList.FilterNeighborhoodID.HasValue)
						itemQuery = itemQuery.Where(h => h.ShowcaseItem.NeighborhoodID == filterList.FilterNeighborhoodID);
					if (filterList.FilterBuilderID.HasValue)
						itemQuery = itemQuery.Where(h => h.ShowcaseItem.BuilderID == filterList.FilterBuilderID);
					if (filterList.FilterBeginDate.HasValue)
						itemQuery = itemQuery.Where(h => h.CloseDate >= filterList.FilterBeginDate.Value);
					if (filterList.FilterEndDate.HasValue)
						itemQuery = itemQuery.Where(h => h.CloseDate <= filterList.FilterEndDate.Value);
					if (!String.IsNullOrEmpty(searchText))
						itemQuery = itemQuery.Where(h => h.ShowcaseItem.MlsID.ToString().Contains(searchText) || h.ShowcaseItem.Address.Address1.Contains(searchText) || h.ShowcaseItem.Address.Zip.Contains(searchText));
					objects = maximumRows <= 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : (maximumRows <= 0 || (pageNumber == 1 && objects.Count < maximumRows) ? objects.Count : itemQuery.Count());
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		public static List<SoldHomeReportItem> SoldHomeReportWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, out decimal totalSales, Filters filterList = new Filters())
		{
			List<SoldHomeReportItem> objects = SoldHomeReport(startRowIndex, maximumRows, searchText, sortField, sortDirection, out totalSales, filterList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<SoldHomeReportItem> SoldHomeReport(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out decimal totalSales, Filters filterList = new Filters())
		{
			totalSales = 0;
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SoldHomeID");

			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText);

			List<SoldHomeReportItem> objects;
			string baseKey = cacheKeyPrefix + "SoldHomeReport_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<SoldHomeReportItem> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SoldHomeReportItem>();
				tmpList = Cache[key] as List<SoldHomeReportItem>;
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
				objects = new List<SoldHomeReportItem>();
				using (Entities entity = new Entities())
				{
					IQueryable<SoldHome> baseQuery = SetupOrderByClause(entity.SoldHome, sortField, sortDirection);
					if (filterList.FilterBeginDate.HasValue)
						baseQuery = baseQuery.Where(h => h.CloseDate >= filterList.FilterBeginDate.Value);
					if (filterList.FilterEndDate.HasValue)
						baseQuery = baseQuery.Where(h => h.CloseDate <= filterList.FilterEndDate.Value);
					if (!String.IsNullOrEmpty(filterList.FilterSoldHomeListingAgentID))
					{
						int agentID = Convert.ToInt32(filterList.FilterSoldHomeListingAgentID);
						baseQuery = baseQuery.Where(h => h.ListingAgentID == agentID);
					}
					if (!String.IsNullOrEmpty(filterList.FilterSoldHomeSellerOfficeID))
					{
						int officeID = Convert.ToInt32(filterList.FilterSoldHomeSellerOfficeID);
						baseQuery = baseQuery.Where(h => h.SellerOfficeID == officeID);
					}
					if (!String.IsNullOrEmpty(searchText))
						baseQuery = baseQuery.Where(h => h.SellerOffice.Name.Contains(searchText) || h.ListingAgent.UserInfo.FirstOrDefault().FirstName.Contains(searchText) || h.ListingAgent.UserInfo.FirstOrDefault().LastName.Contains(searchText) || h.ShowcaseItem.Address.Address1.Contains(searchText) || h.ShowcaseItem.Neighborhood.Name.Contains(searchText));
					var itemQuery = baseQuery.Select(s => new
					{
						Address = s.ShowcaseItem.Address.Address1,
						AgentFirst = s.ListingAgent.UserInfo.FirstOrDefault().FirstName,
						AgentLast = s.ListingAgent.UserInfo.FirstOrDefault().LastName,
						CloseDate = s.CloseDate,
						NeighborhoodName = s.ShowcaseItem.Neighborhood.Name,
						OfficeName = s.SellerOffice.Name,
						SalePrice = s.SalePrice
					});

					var tempObjects = maximumRows <= 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : (maximumRows <= 0 || (pageNumber == 1 && tempObjects.Count < maximumRows) ? tempObjects.Count : itemQuery.Count());

					foreach (var obj in tempObjects)
					{
						objects.Add(new SoldHomeReportItem { Address = obj.Address, AgentFirstAndLast = obj.AgentFirst + (!String.IsNullOrEmpty(obj.AgentLast) ? " " + obj.AgentLast : ""), CloseDate = obj.CloseDate, NeighborhoodName = obj.NeighborhoodName, OfficeName = obj.OfficeName, SalePrice = obj.SalePrice });
					}
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			totalSales = objects.Sum(h => h.SalePrice);
			return objects;
		}

		public partial struct Filters
		{
			public int? FilterBuilderID { get; set; }
			public int? FilterNeighborhoodID { get; set; }
			public int? FilterShowcaseID { get; set; }
			public DateTime? FilterBeginDate { get; set; }
			public DateTime? FilterEndDate { get; set; }

			public Dictionary<string, object> GetCustomFilterList()
			{
				filterList = GetFilterList();
				if (FilterBuilderID.HasValue)
					filterList.Add("@FilterBuilderID", FilterBuilderID);
				if (FilterNeighborhoodID.HasValue)
					filterList.Add("@FilterNeighborhoodID", FilterNeighborhoodID);
				if (FilterShowcaseID.HasValue)
					filterList.Add("@FilterShowcaseID", FilterShowcaseID);
				if (FilterBeginDate.HasValue)
					filterList.Add("@FilterBeginDate", FilterBeginDate);
				if (FilterEndDate.HasValue)
					filterList.Add("@FilterEndDate", FilterEndDate);
				return filterList;
			}
		}
	}

	public class SoldHomeReportItem
	{
		public string Address { get; set; }
		public string AgentFirstAndLast { get; set; }
		public DateTime CloseDate { get; set; }
		public string NeighborhoodName { get; set; }
		public string OfficeName { get; set; }
		public decimal SalePrice { get; set; }
	}
}