using System;
using System.Collections.Generic;
using System.Linq;
using Classes.SEOComponent;

namespace Classes.Media352_NewsPress
{
	public partial class NewsPress
	{
		protected override void DeleteSEO()
		{
			SEOData.DeleteSEOData("~/", "news-press-details.aspx", "Id", NewsPressID);
		}

		public static List<NewsPress> GetNumArticlesQuickView(int numArticles, int categoryID)
		{
			List<NewsPress> objects;
			string key = cacheKeyPrefix + "GetNumArticlesQuickView_" + numArticles + "_" + categoryID;

			List<NewsPress> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<NewsPress>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.NewsPress.Where(n => n.Active && !n.Archived && n.Featured && n.NewsPressNewsPressCategory.Any(c => c.NewsPressCategoryID == categoryID)).OrderByDescending(n => n.Date).Take(numArticles).ToList();
				}

				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<NewsPress> NewsPressPageByNewsPressCategoryID(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "NewsPressID");

			string cachingFilterText = GetCacheFilterText(filterList.GetCustomFilterList(), searchText);

			List<NewsPress> objects;
			string baseKey = cacheKeyPrefix + "PageByNewsPressCategoryID_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<NewsPress> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				tmpList = Cache[key] as List<NewsPress>;
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
					IQueryable<NewsPress> itemQuery = SetupQuery(entity.NewsPress, "NewsPress", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection);
					if (Settings.EnableCategories && !String.IsNullOrWhiteSpace(filterList.FilterNewsPressCategoryID))
					{
						int categoryID = Convert.ToInt32(filterList.FilterNewsPressCategoryID);
						itemQuery = itemQuery.Where(n => n.NewsPressNewsPressCategory.Any(c => c.NewsPressCategoryID == categoryID));
					}
					if (Settings.ArchiveType == ArchiveTypes.NumCurrentArticles)
					{
						if (!String.IsNullOrWhiteSpace(filterList.FilterNewsPressAutoArchived) && Convert.ToBoolean(filterList.FilterNewsPressAutoArchived))
							itemQuery = itemQuery.OrderByDescending(n => n.Date).Skip(Settings.NumCurrentArticles);
						else
							itemQuery = itemQuery.OrderByDescending(n => n.Date).Take(Settings.NumCurrentArticles);
						if (sortField == "Date" && sortDirection)
							itemQuery = itemQuery.OrderBy(n => n.Date);
					}
					else if (Settings.ArchiveType == ArchiveTypes.ArchiveAfterNumDays)
					{
						DateTime earliestDate = DateTime.UtcNow.AddDays(-Settings.NumDaysToKeepCurrent);
						if (!String.IsNullOrWhiteSpace(filterList.FilterNewsPressAutoArchived) && Convert.ToBoolean(filterList.FilterNewsPressAutoArchived))
							itemQuery = itemQuery.Where(n => n.Date < earliestDate);
						else
							itemQuery = itemQuery.Where(n => n.Date >= earliestDate);
					}
					else
					{
						bool archived = !String.IsNullOrWhiteSpace(filterList.FilterNewsPressAutoArchived) && Convert.ToBoolean(filterList.FilterNewsPressAutoArchived);
						itemQuery = itemQuery.Where(n => n.Archived == archived);
					}

					objects = maximumRows == 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : itemQuery.Count();
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		#region Nested type: Filters

		public partial struct Filters
		{
			public string FilterNewsPressCategoryID { get; set; }
			public string FilterNewsPressAutoArchived { get; set; }

			public Dictionary<string, object> GetCustomFilterList()
			{
				filterList = GetFilterList();
				if (FilterNewsPressCategoryID != null)
					filterList.Add("@FilterNewsPressCategoryID", FilterNewsPressCategoryID);
				if (FilterNewsPressAutoArchived != null)
					filterList.Add("@FilterNewsPressAutoArchived", FilterNewsPressAutoArchived);
				return filterList;
			}
		}

		#endregion
	}
}