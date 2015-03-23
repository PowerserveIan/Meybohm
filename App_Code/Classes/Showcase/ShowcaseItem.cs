using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Classes.SEOComponent;

/// <summary>
/// Only way to extend the generated class that I know of
/// </summary>
public partial class ShowcaseItemForJSON
{

	/// <summary>
	/// With Knockout, we can't do the html replacement, so the Friendly Filename replacement needs to happen here.
	/// </summary>
	public string DetailsPageUrl { get; set; }
}

namespace Classes.Showcase
{
	public partial class ShowcaseItem
	{
		private const string m_StaticDetailsPageUrl = "home-details?id={0}&title={1}";

		public Classes.Media352_MembershipProvider.UserInfo AgentInfo { get; set; }

		public int NumberBedrooms { get; set; }

		public int NumberBathrooms { get; set; }

		public int NumberOfVisits { get; set; }

		public string PropertyType { get; set; }

		protected override void DeleteSEO()
		{
			SEOData.DeleteSEOData("~/", "home-details", "id", ShowcaseItemID);
		}

		public static List<ShowcaseItemForJSON> GetItemsFromSearch(string filters, int showcaseID, List<int> changedItemIDs)
		{
			ShowcaseItem.Filters filterList = new ShowcaseItem.Filters();
			filterList.FilterShowcaseItemActive = true.ToString();
			filterList.FilterShowcaseItemShowcaseID = showcaseID.ToString();

			filters = HttpContext.Current.Server.UrlDecode(filters);
			string filterText = string.Empty;
			string[] queryStrings = filters.Split('&');
			foreach (string obj in queryStrings)
			{
				if (obj.StartsWith("Filters="))
					filters = obj.Replace("Filters=", "");
				else if (obj.StartsWith("SearchText="))
					filterList.SearchText = obj.Replace("SearchText=", "");
				else if (obj.StartsWith("AgentID="))
					filterList.FilterShowcaseItemAgentID = obj.Replace("AgentID=", "");
				else if (obj.StartsWith("OpenHouse="))
					filterList.OpenHouse = Convert.ToBoolean(obj.Replace("OpenHouse=", ""));
			}

			return GetPagedFilteredShowcaseItems(0, 0, filters, "", true, filterList, changedItemIDs);
		}

        public static int GetItemCountFromSearch(string filters, int showcaseID, bool newHomes)
		{
			ShowcaseItem.Filters filterList = new ShowcaseItem.Filters();
			filterList.FilterShowcaseItemActive = true.ToString();
			filterList.FilterShowcaseItemShowcaseID = showcaseID.ToString();

			filters = HttpContext.Current.Server.UrlDecode(filters);
			string filterText = string.Empty;
			string[] queryStrings = filters.Split('&');
			foreach (string obj in queryStrings)
			{
				if (obj.StartsWith("Filters="))
					filters = obj.Replace("Filters=", "");
				else if (obj.StartsWith("SearchText="))
					filterList.SearchText = obj.Replace("SearchText=", "");
				else if (obj.StartsWith("AgentID="))
					filterList.FilterShowcaseItemAgentID = obj.Replace("AgentID=", "");
				else if (obj.StartsWith("OpenHouse="))
					filterList.OpenHouse = Convert.ToBoolean(obj.Replace("OpenHouse=", ""));
                if (newHomes)
					filterList.NewHomesOnly = true;

			}

			List<ShowcaseItemForJSON> items = ShowcaseItem.GetPagedFilteredShowcaseItems(0, 1, filters, "", true, filterList);
			return items.Any() ? items.FirstOrDefault().TotalRowCount.Value : 0;
		}

		public static List<ShowcaseItemForJSON> GetPagedFilteredShowcaseItems(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList, List<int> showcaseItemIDsToFilterOn = null)
		{
			string showcaseItemIDsString = showcaseItemIDsToFilterOn != null ? string.Join(",", showcaseItemIDsToFilterOn) : ""; 
			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText) + (showcaseItemIDsToFilterOn != null ? "_" + showcaseItemIDsString : "");

			if (!String.IsNullOrEmpty(filterList.FilterShowcaseItemShowcaseID))
				ShowcaseHelpers.SetUsersCurrentShowcaseID(Convert.ToInt32(filterList.FilterShowcaseItemShowcaseID));

			List<ShowcaseItemForJSON> objects;
			string baseKey = cacheKeyPrefix + "GetPagedFilteredShowcaseItems_" + cachingFilterText + "_" + Settings.EnableFilters;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseItemForJSON> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<ShowcaseItemForJSON>;
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
					objects = entity.Showcase_GetFilteredShowcaseItemIDs(maximumRows, pageNumber, searchText, sortDirection, sortField, filterList.FilterShowcaseItemActive, filterList.FilterShowcaseItemShowcaseID, filterList.FilterShowcaseItemNeighborhoodID, filterList.FilterShowcaseItemAgentID, filterList.AddressLat, filterList.AddressLong, filterList.MinDistance, filterList.MaxDistance, filterList.OpenHouse, filterList.SearchText, Settings.EnableFilters, filterList.NewHomesOnly, filterList.ShowLotsLand, showcaseItemIDsString).ToList();
					m_ItemCount = objects.Any() ? objects.FirstOrDefault().TotalRowCount.Value : 0;

                    for(int intIndex = 0; intIndex < objects.Count(); intIndex++)
                    {
                        if (!objects[intIndex].Title.Contains("$"))
                        {
                            objects[intIndex].Title = objects[intIndex].Title.Replace("BA, ", "BA, $");
                        }

                        objects[intIndex].Title = objects[intIndex].Title.Replace(", Available", " Available");
                    }
				}

				if (objects.Any() && maximumRows != 1)
				{
					Dictionary<string, string> replacePages = Classes.SEOComponent.SEOData.GetPagesWithFriendlyFilenames(m_StaticDetailsPageUrl.Substring(0, m_StaticDetailsPageUrl.IndexOf("{0}")));
					foreach (ShowcaseItemForJSON obj in objects)
					{
						obj.DetailsPageUrl = string.Format(m_StaticDetailsPageUrl, obj.ShowcaseItemID, HttpContext.Current.Server.UrlEncode(obj.Title));
						if (replacePages.ContainsKey("~/" + obj.DetailsPageUrl.Substring(0, obj.DetailsPageUrl.IndexOf("&"))))
							obj.DetailsPageUrl = replacePages["~/" + obj.DetailsPageUrl.Substring(0, obj.DetailsPageUrl.IndexOf("&"))].Replace("~/", "");

						int? showcaseID = !String.IsNullOrWhiteSpace(filterList.FilterShowcaseItemShowcaseID) ? (int?)Convert.ToInt32(filterList.FilterShowcaseItemShowcaseID) : null;
						if (!showcaseID.HasValue || (showcaseID.Value != (int)MeybohmShowcases.AikenRentalHomes && showcaseID.Value != (int)MeybohmShowcases.AugustaRentalHomes))
							obj.Summary = null;
						else
							obj.Summary = BaseCode.Helpers.ForceShorten(obj.Summary, 50);
					}
				}
				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
		    }
			return objects;
		}

		public static ShowcaseItem GetByAddressLine1(string addressLine1, IEnumerable<string> includeList = null)
		{
			ShowcaseItem obj = null;
			string key = cacheKeyPrefix + "GetByAddressLine1_" + addressLine1 + GetCacheIncludeText(includeList);

			ShowcaseItem tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as ShowcaseItem;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItem> itemQuery = AddIncludes(entity.ShowcaseItem, includeList);
					obj = itemQuery.FirstOrDefault(n => n.Address.Address1 == addressLine1);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<ShowcaseItem> GetPropertiesForAgentWithTotalCount(int maximumRows, string sortField, bool sortDirection, int agentID, out int totalCount)
		{
			List<ShowcaseItem> objects = GetPropertiesForAgent(maximumRows, sortField, sortDirection, agentID);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseItem> GetPropertiesForAgent(int maximumRows, string sortField, bool sortDirection, int agentID, bool updateStats = false)
		{
			List<ShowcaseItem> objects;
			string baseKey = cacheKeyPrefix + "GetPropertiesForAgent_" + agentID;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseItem> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<ShowcaseItem>;
				tmpInt = Cache[countKey] as int?;
			}

			if (tmpList != null && tmpInt.HasValue)
			{
				objects = tmpList;
				m_ItemCount = tmpInt.Value;
			}
			else
			{
				objects = new List<ShowcaseItem>();
				if (updateStats)
					Classes.Showcase.ShowcaseItemMetric.UpdateHistoricalStats(null);
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItem> itemQuery = entity.ShowcaseItem;
					if (sortField != "NumberOfVisits")
						itemQuery = SetupOrderByClause(itemQuery, sortField, sortDirection);

					var anonQuery = itemQuery.Where(s => s.Active && (s.AgentID == agentID || (s.TeamID.HasValue && s.Team.UserTeam.Any(u => u.UserID == agentID)))).Select(s => new
					{
						ShowcaseItemEntity = s,
						Address = s.Address,
						AddressState = s.Address.State,
						NumVisits = (int?)s.ShowcaseItemMetricHistorical.Where(m => m.ClickTypeID == (int)ClickTypes.Visit).Select(m => m.TotalCount).Sum(),
						PropertyType = s.ShowcaseItemAttributeValue.Where(a => a.ShowcaseAttributeValue.ShowcaseAttribute.MLSAttributeName == "Property Type").Select(a => a.ShowcaseAttributeValue.Value).FirstOrDefault()
					});

					if (sortField == "NumberOfVisits" && sortDirection)
						anonQuery = anonQuery.OrderBy(s => s.NumVisits);
					else if (sortField == "NumberOfVisits")
						anonQuery = anonQuery.OrderByDescending(s => s.NumVisits);

					var items = anonQuery.Take(maximumRows).ToList();
					foreach (var item in items)
					{
						ShowcaseItem obj = item.ShowcaseItemEntity;
						obj.Address = item.Address;
						obj.Address.State = item.AddressState;
						if (item.NumVisits.HasValue)
							obj.NumberOfVisits = item.NumVisits.Value;
						obj.PropertyType = item.PropertyType;
						objects.Add(obj);
					}
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : (maximumRows <= 0 || objects.Count < maximumRows ? objects.Count : anonQuery.Count());
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}

			return objects;
		}

		public static List<ShowcaseItem> GetPropertiesWithAnAgentForStatsEmail()
		{
			List<ShowcaseItem> objects;
			string key = cacheKeyPrefix + "GetPropertiesWithAnAgentForStatsEmail";

			List<ShowcaseItem> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<ShowcaseItem>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.ShowcaseItem.Include("Address").Include("Address.State").Where(s => s.AgentID.HasValue && s.Active && s.StatsSentToAgent && !s.Rented).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<ShowcaseItem> PageInventoryAndStatisticsReportWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters())
		{
			List<ShowcaseItem> objects = PageInventoryAndStatisticsReport(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseItem> PageInventoryAndStatisticsReport(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText);

			List<ShowcaseItem> objects;
			string baseKey = cacheKeyPrefix + "PageInventoryAndStatisticsReport_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseItem> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<ShowcaseItem>;
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
				objects = new List<ShowcaseItem>();
				Classes.Showcase.ShowcaseItemMetric.UpdateHistoricalStats(null);
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItem> itemQuery = SetupWhereClause(entity.ShowcaseItem, "ShowcaseItem", filterList.GetFilterList(), searchText, null);
					if (String.IsNullOrEmpty(filterList.FilterShowcaseItemShowcaseID))
						itemQuery = itemQuery.Where(s => s.ShowcaseID == (int)MeybohmShowcases.AikenExistingHomes || s.ShowcaseID == (int)MeybohmShowcases.AugustaExistingHomes);
					if (filterList.FilterBeginDate.HasValue)
						itemQuery = itemQuery.Where(s => s.DateListed >= filterList.FilterBeginDate.Value);
					if (filterList.FilterEndDate.HasValue)
						itemQuery = itemQuery.Where(s => s.DateListed <= filterList.FilterEndDate.Value);
					if (filterList.FilterListPriceMin.HasValue)
						itemQuery = itemQuery.Where(s => s.ListPrice >= filterList.FilterListPriceMin.Value);
					if (filterList.FilterListPriceMax.HasValue)
						itemQuery = itemQuery.Where(s => s.ListPrice <= filterList.FilterListPriceMax.Value);
					if (!String.IsNullOrEmpty(filterList.FilterPropertyType))
						itemQuery = itemQuery.Where(s => s.ShowcaseItemAttributeValue.Any(a => a.ShowcaseAttributeValue.ShowcaseAttribute.MLSAttributeName == "Property Type" && a.ShowcaseAttributeValue.Value == filterList.FilterPropertyType));
					if (sortField != "NumberOfVisits")
						itemQuery = SetupOrderByClause(itemQuery, sortField, sortDirection);

					var anonQuery = itemQuery.Select(s => new
					{
						ShowcaseItemEntity = s,
						Address = s.Address,
						AgentInfo = s.Agent.UserInfo.FirstOrDefault(),
						NumVisits = (int?)s.ShowcaseItemMetricHistorical.Where(m => m.ClickTypeID == (int)ClickTypes.Visit).Select(m => m.TotalCount).Sum(),
						PropertyType = s.ShowcaseItemAttributeValue.Where(a => a.ShowcaseAttributeValue.ShowcaseAttribute.MLSAttributeName == "Property Type").Select(a => a.ShowcaseAttributeValue.Value).FirstOrDefault()
					});

					if (sortField == "NumberOfVisits" && sortDirection)
						anonQuery = anonQuery.OrderBy(s => s.NumVisits);
					else if (sortField == "NumberOfVisits")
						anonQuery = anonQuery.OrderByDescending(s => s.NumVisits);
					else if (sortField == "PropertyType" && sortDirection)
						anonQuery = anonQuery.OrderBy(s => s.PropertyType);
					else if (sortField == "PropertyType")
						anonQuery = anonQuery.OrderByDescending(s => s.PropertyType);

					var items = maximumRows <= 0 ? anonQuery.ToList() : anonQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : (maximumRows <= 0 || (pageNumber == 1 && items.Count < maximumRows) ? items.Count : anonQuery.Count());

					foreach (var item in items)
					{
						ShowcaseItem obj = item.ShowcaseItemEntity;
						obj.Address = item.Address;
						obj.AgentInfo = item.AgentInfo;
						if (item.NumVisits.HasValue)
							obj.NumberOfVisits = item.NumVisits.Value;
						obj.PropertyType = item.PropertyType;
						objects.Add(obj);
					}
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		public static DataTable PageInventoryAndStatisticsReportForCSV(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText);

			DataTable objects;
			string baseKey = cacheKeyPrefix + "PageInventoryAndStatisticsReportForCSV_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			DataTable tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as DataTable;
				tmpInt = Cache[countKey] as int?;
			}

			if (tmpList != null && tmpInt.HasValue)
			{
				objects = tmpList;
				m_ItemCount = tmpInt.Value;
			}
			else
			{
				List<ShowcaseItem> reportItems = PageInventoryAndStatisticsReport(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList);
				List<string> columnNames = new List<string> { "MLS ID", "Market", "Agent", "List Date", "Address", "Price", "Property Type" };
				objects = new DataTable();
				foreach (string column in columnNames)
				{
					objects.Columns.Add(column);
				}
				List<ClickType> clickTypes = ClickType.GetAll();
				foreach (ClickType clickType in clickTypes)
				{
					objects.Columns.Add(clickType.Name + " Clicks");
				}
				foreach (ShowcaseItem obj in reportItems)
				{
					DataRow dr = objects.NewRow();
					dr["MLS ID"] = obj.MlsID;
					dr["Market"] = obj.ShowcaseID == (int)MeybohmShowcases.AikenExistingHomes ? "Aiken" : "Augusta";
					dr["Agent"] = obj.AgentInfo != null ? obj.AgentInfo.FirstAndLast : string.Empty;
					dr["List Date"] = obj.DateListed.HasValue ? obj.DateListed.Value.ToShortDateString() : string.Empty;
					dr["Address"] = obj.Address != null ? obj.Address.Address1 : string.Empty;
					dr["Price"] = obj.ListPrice.HasValue ? obj.ListPrice.Value.ToString("C") : string.Empty;
					dr["Property Type"] = obj.PropertyType;
					//TODO: Change this so its a single database hit instead of one per property :(
					Dictionary<string, int> clicks = Classes.Showcase.ShowcaseItemMetric.GetStatisticsForProperty(obj.ShowcaseItemID, filterList.FilterBeginDate, filterList.FilterEndDate, false);
					foreach (KeyValuePair<string, int> kvp in clicks)
					{
						dr[kvp.Key + " Clicks"] = kvp.Value;
					}
					objects.Rows.Add(dr);
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		public static List<ShowcaseItem> GetNewHomesForSaleWithTotalCount(string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters())
		{
			List<ShowcaseItem> objects = GetNewHomesForSale(searchText, sortField, sortDirection, filterList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseItem> GetNewHomesForSale(string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText);

			List<ShowcaseItem> objects;
			string baseKey = cacheKeyPrefix + "GetNewHomesForSale_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection;
			string countKey = baseKey + "_count";

			List<ShowcaseItem> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<ShowcaseItem>;
				tmpInt = Cache[countKey] as int?;
			}

			if (tmpList != null && tmpInt.HasValue)
			{
				objects = tmpList;
				m_ItemCount = tmpInt.Value;
			}
			else
			{
				objects = new List<ShowcaseItem>();
				Classes.Showcase.ShowcaseItemMetric.UpdateHistoricalStats(null);
				using (Entities entity = new Entities())
				{
					IQueryable<ShowcaseItem> itemQuery = SetupWhereClause(entity.ShowcaseItem, "ShowcaseItem", filterList.GetFilterList(), string.Empty, null).Where(s => !s.SoldHome.Any());
					if (String.IsNullOrEmpty(filterList.FilterShowcaseItemShowcaseID))
						itemQuery = itemQuery.Where(s =>s.NewHome);
					if (sortField != "NumberOfVisits" && sortField != "NumberBedrooms" && sortField != "NumberBathrooms")
						itemQuery = SetupOrderByClause(itemQuery, sortField, sortDirection);
					if (!String.IsNullOrWhiteSpace(searchText))
					{
						int? mlsID = null;
						int temp;
						if (Int32.TryParse(searchText, out temp))
							mlsID = temp;
						itemQuery = itemQuery.Where(s => s.Address.Address1.Contains(searchText) || s.Address.Zip.Contains(searchText) || s.Builder.Name.Contains(searchText) || s.Neighborhood.Name.Contains(searchText) || (mlsID.HasValue && s.MlsID == mlsID));
					}

					var anonQuery = itemQuery.Select(s => new
					{
						ShowcaseItemEntity = s,
						Address = s.Address,
						AgentInfo = s.Agent.UserInfo.FirstOrDefault(),
						Builder = s.Builder,
						Neighborhood = s.Neighborhood,
						NumVisits = (int?)s.ShowcaseItemMetricHistorical.Where(m => m.ClickTypeID == (int)ClickTypes.Visit).Select(m => m.TotalCount).Sum(),
						NumberBedrooms = s.ShowcaseItemAttributeValue.Where(a => a.ShowcaseAttributeValue.ShowcaseAttribute.MLSAttributeName == "Bedrooms").Select(a => a.ShowcaseAttributeValue.Value).FirstOrDefault(),
						NumberBathrooms = s.ShowcaseItemAttributeValue.Where(a => a.ShowcaseAttributeValue.ShowcaseAttribute.MLSAttributeName == "Full Baths").Select(a => a.ShowcaseAttributeValue.Value).FirstOrDefault()
					});

					if (sortField == "NumberOfVisits" && sortDirection)
						anonQuery = anonQuery.OrderBy(s => s.NumVisits);
					else if (sortField == "NumberOfVisits")
						anonQuery = anonQuery.OrderByDescending(s => s.NumVisits);
					else if (sortField == "NumberBedrooms" && sortDirection)
						anonQuery = anonQuery.OrderBy(s => s.NumberBedrooms);
					else if (sortField == "NumberBedrooms")
						anonQuery = anonQuery.OrderByDescending(s => s.NumberBedrooms);
					else if (sortField == "NumberBathrooms" && sortDirection)
						anonQuery = anonQuery.OrderBy(s => s.NumberBathrooms);
					else if (sortField == "NumberBathrooms")
						anonQuery = anonQuery.OrderByDescending(s => s.NumberBathrooms);

					var items = anonQuery.ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : anonQuery.Count();

					foreach (var item in items)
					{
						ShowcaseItem obj = item.ShowcaseItemEntity;
						obj.Address = item.Address;
						obj.AgentInfo = item.AgentInfo;
						obj.Builder = item.Builder;
						obj.Neighborhood = item.Neighborhood;
						int temp;
						if (!String.IsNullOrWhiteSpace(item.NumberBedrooms) && Int32.TryParse(item.NumberBedrooms, out temp))
							obj.NumberBedrooms = temp;
						if (!String.IsNullOrWhiteSpace(item.NumberBathrooms) && Int32.TryParse(item.NumberBathrooms, out temp))
							obj.NumberBathrooms = temp;
						if (item.NumVisits.HasValue)
							obj.NumberOfVisits = item.NumVisits.Value;
						objects.Add(obj);
					}
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		public static DataTable GetNewHomesForSaleForCSV(string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText);

			DataTable objects;
			string baseKey = cacheKeyPrefix + "GetNewHomesForSaleForCSV_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection;
			string countKey = baseKey + "_count";

			DataTable tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as DataTable;
				tmpInt = Cache[countKey] as int?;
			}

			if (tmpList != null && tmpInt.HasValue)
			{
				objects = tmpList;
				m_ItemCount = tmpInt.Value;
			}
			else
			{
				List<ShowcaseItem> reportItems = GetNewHomesForSale(searchText, sortField, sortDirection, filterList).OrderBy(s => s.Neighborhood != null ? s.Neighborhood.Name : "zzz").ThenBy(s => s.NumberOfVisits).ToList();
				List<string> columnNames = new List<string> { "Neighborhood", "MLS ID", "Market", "Agent", "List Date", "Address", "Price", "Builder" };
				objects = new DataTable();
				foreach (string column in columnNames)
				{
					objects.Columns.Add(column);
				}
				List<ClickType> clickTypes = ClickType.GetAll();
				foreach (ClickType clickType in clickTypes)
				{
					objects.Columns.Add(clickType.Name + " Clicks");
				}
				foreach (ShowcaseItem obj in reportItems)
				{
					DataRow dr = objects.NewRow();
					dr["Neighborhood"] = obj.Neighborhood != null ? obj.Neighborhood.Name : string.Empty;
					dr["MLS ID"] = obj.MlsID;
					dr["Market"] = obj.ShowcaseID == (int)MeybohmShowcases.AikenExistingHomes ? "Aiken" : "Augusta";
					dr["Agent"] = obj.AgentInfo != null ? obj.AgentInfo.FirstAndLast : string.Empty;
					dr["List Date"] = obj.DateListed.HasValue ? obj.DateListed.Value.ToShortDateString() : string.Empty;
					dr["Address"] = obj.Address != null ? obj.Address.Address1 : string.Empty;
					dr["Price"] = obj.ListPrice.HasValue ? obj.ListPrice.Value.ToString("C") : string.Empty;
					dr["Builder"] = obj.Builder != null ? obj.Builder.Name : string.Empty;
					//TODO: Change this so its a single database hit instead of one per property :(
					Dictionary<string, int> clicks = Classes.Showcase.ShowcaseItemMetric.GetStatisticsForProperty(obj.ShowcaseItemID, filterList.FilterBeginDate, filterList.FilterEndDate);
					foreach (KeyValuePair<string, int> kvp in clicks)
					{
						dr[kvp.Key + " Clicks"] = kvp.Value;
					}
					objects.Rows.Add(dr);
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		public static List<ShowcaseItem> ShowcaseItemPageForAdminWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<ShowcaseItem> objects = ShowcaseItemPageForAdmin(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<ShowcaseItem> ShowcaseItemPageForAdmin(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ShowcaseItemID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<ShowcaseItem> objects;
			string baseKey = cacheKeyPrefix + "ShowcaseItemPageForAdmin_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<ShowcaseItem> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ShowcaseItem>();
				tmpList = Cache[key] as List<ShowcaseItem>;
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
					IQueryable<ShowcaseItem> itemQuery = SetupQuery(entity.ShowcaseItem, "ShowcaseItem", filterList.GetFilterList(), string.Empty, null, sortField, sortDirection, includeList);
					if (!String.IsNullOrWhiteSpace(searchText))
						itemQuery = itemQuery.Where(s => s.Title.Contains(searchText) || s.EmailAddresses.Contains(searchText) || s.Address.Address1.Contains(searchText));

					objects = maximumRows <= 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : (maximumRows <= 0 || (pageNumber == 1 && objects.Count < maximumRows) ? objects.Count : itemQuery.Count());
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}


		public static List<Showcase_GetFilteredShowcaseAttributeIDs_Result> GetFilteredShowcaseItemsAttributeValues(string searchText, Filters filterList, List<int> showcaseItemIDsToFilterOn = null)
		{
			string showcaseItemIDsString = showcaseItemIDsToFilterOn != null ? string.Join(",", showcaseItemIDsToFilterOn) : "";
			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText) + (showcaseItemIDsToFilterOn != null ? "_" + showcaseItemIDsString : "");

			List<Showcase_GetFilteredShowcaseAttributeIDs_Result> objects;
			string baseKey = cacheKeyPrefix + "GetPagedFilteredShowcaseItems_Attributes_" + cachingFilterText + "_" + Settings.EnableFilters;
			string key = baseKey;

			List<Showcase_GetFilteredShowcaseAttributeIDs_Result> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<Showcase_GetFilteredShowcaseAttributeIDs_Result>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.Showcase_GetFilteredShowcaseAttributeIDs(searchText, filterList.FilterShowcaseItemActive, filterList.FilterShowcaseItemShowcaseID, filterList.FilterShowcaseItemNeighborhoodID, filterList.FilterShowcaseItemAgentID, filterList.AddressLat, filterList.AddressLong, filterList.MinDistance, filterList.MaxDistance, filterList.OpenHouse, filterList.SearchText, Settings.EnableFilters, filterList.NewHomesOnly, filterList.ShowLotsLand, showcaseItemIDsString).ToList();
				}

				Cache.Store(key, objects);
			}
			return objects;
		}


		public static List<Showcase_GetFilteredShowcaseAttributeMinMax_Result> GetMinMaxFromSearch(string filters, int showcaseID)
		{
			ShowcaseItem.Filters filterList = new ShowcaseItem.Filters();
			filterList.FilterShowcaseItemActive = true.ToString();
			filterList.FilterShowcaseItemShowcaseID = showcaseID.ToString();

			filters = HttpContext.Current.Server.UrlDecode(filters);
			string filterText = string.Empty;
			string[] queryStrings = filters.Split('&');
			foreach (string obj in queryStrings)
			{
				if (obj.StartsWith("Filters="))
					filters = obj.Replace("Filters=", "");
				else if (obj.StartsWith("SearchText="))
					filterList.SearchText = obj.Replace("SearchText=", "");
				else if (obj.StartsWith("AgentID="))
					filterList.FilterShowcaseItemAgentID = obj.Replace("AgentID=", "");
				else if (obj.StartsWith("OpenHouse="))
					filterList.OpenHouse = Convert.ToBoolean(obj.Replace("OpenHouse=", ""));
			}

			return GetMinMaxFromSearch(filters, filterList);
		}

		public static List<Showcase_GetFilteredShowcaseAttributeMinMax_Result> GetMinMaxFromSearch(string searchText, Filters filterList, List<int> showcaseItemIDsToFilterOn = null)
		{
			string showcaseItemIDsString = showcaseItemIDsToFilterOn != null ? string.Join(",", showcaseItemIDsToFilterOn) : "";
			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText) + (showcaseItemIDsToFilterOn != null ? "_" + showcaseItemIDsString : "");

			List<Showcase_GetFilteredShowcaseAttributeMinMax_Result> objects;
			string baseKey = cacheKeyPrefix + "GetMinMaxFromSearch_Attributes_" + cachingFilterText + "_" + Settings.EnableFilters;
			string key = baseKey;

			List<Showcase_GetFilteredShowcaseAttributeMinMax_Result> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<Showcase_GetFilteredShowcaseAttributeMinMax_Result>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.Showcase_GetFilteredShowcaseAttributeMinMax(searchText, filterList.FilterShowcaseItemActive, filterList.FilterShowcaseItemShowcaseID, filterList.FilterShowcaseItemNeighborhoodID, filterList.FilterShowcaseItemAgentID, filterList.AddressLat, filterList.AddressLong, filterList.MinDistance, filterList.MaxDistance, filterList.OpenHouse, filterList.SearchText, Settings.EnableFilters, filterList.NewHomesOnly, filterList.ShowLotsLand, showcaseItemIDsString).ToList();
				}

				Cache.Store(key, objects);
			}
			return objects;
		}


		#region Nested type: Filters

		public partial struct Filters
		{
			public decimal? AddressLat { get; set; }
			public decimal? AddressLong { get; set; }
			public int? MinDistance { get; set; }
			public int? MaxDistance { get; set; }
			public bool? OpenHouse { get; set; }
			public string SearchText { get; set; }

			public string FilterPropertyType { get; set; }
			public decimal? FilterListPriceMin { get; set; }
			public decimal? FilterListPriceMax { get; set; }
			public DateTime? FilterBeginDate { get; set; }
			public DateTime? FilterEndDate { get; set; }
			public bool? NewHomesOnly { get; set; }
			public bool? ShowLotsLand { get; set; }

			public Dictionary<string, object> GetCustomFilterList()
			{
				filterList = GetFilterList();
				if (AddressLat != null)
					filterList.Add("@AddressLat", AddressLat.ToString());
				if (AddressLong != null)
					filterList.Add("@AddressLong", AddressLong.ToString());
				if (MinDistance != null)
					filterList.Add("@MinDistance", MinDistance.ToString());
				if (MaxDistance != null)
					filterList.Add("@MaxDistance", MaxDistance.ToString());
				if (OpenHouse != null)
					filterList.Add("@OpenHouse", OpenHouse.ToString());
				if (!String.IsNullOrWhiteSpace(SearchText))
					filterList.Add("@SearchText", SearchText);

				if (!String.IsNullOrWhiteSpace(FilterPropertyType))
					filterList.Add("@FilterPropertyType", FilterPropertyType);
				if (FilterListPriceMin.HasValue)
					filterList.Add("@FilterListPriceMin", FilterListPriceMin);
				if (FilterListPriceMax.HasValue)
					filterList.Add("@FilterListPriceMax", FilterListPriceMax);
				if (FilterBeginDate.HasValue)
					filterList.Add("@FilterBeginDate", FilterBeginDate);
				if (FilterEndDate.HasValue)
					filterList.Add("@FilterEndDate", FilterEndDate);
				if (NewHomesOnly.HasValue)
					filterList.Add("@NewHomesOnly", NewHomesOnly);
				if (ShowLotsLand.HasValue)
					filterList.Add("@ShowLotsLand", ShowLotsLand);
				return filterList;
			}
		}

		#endregion
	}
}