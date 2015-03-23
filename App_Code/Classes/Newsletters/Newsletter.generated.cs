using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Newsletters
{
	public partial class Newsletter : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Newsletters_Newsletter_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Issue", "Title"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public Newsletter(Newsletter objectToCopy)
		{
			Active = objectToCopy.Active;
			Archived = objectToCopy.Archived;
			Body = objectToCopy.Body;
			BodyText = objectToCopy.BodyText;
			CMMicrositeID = objectToCopy.CMMicrositeID;
			CreatedDate = objectToCopy.CreatedDate;
			Deleted = objectToCopy.Deleted;
			Description = objectToCopy.Description;
			DesignID = objectToCopy.DesignID;
			DisplayDate = objectToCopy.DisplayDate;
			Featured = objectToCopy.Featured;
			Issue = objectToCopy.Issue;
			Keywords = objectToCopy.Keywords;
			NewHomes = objectToCopy.NewHomes;
			NewsletterID = objectToCopy.NewsletterID;
			Title = objectToCopy.Title;
		}

		public virtual bool IsNewRecord
		{
			get { return NewsletterID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime CreatedDateClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(CreatedDate); }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime DisplayDateClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(DisplayDate); }
		}

		public virtual void Save()
		{
			SaveEntity("Newsletter", this);
			ClearCache();
			ClearRelatedCacheItems();
			SaveSearch();
		}

		public virtual void Delete()
		{
			DeleteSEO();
			DeleteSearch();
			using (Entities entity = new Entities())
			{
				entity.Entry(this).State = System.Data.EntityState.Deleted;
				entity.SaveChanges();
			}
			ClearCache();
			ClearRelatedCacheItems();
		}

		public static Newsletter GetByID(int NewsletterID, IEnumerable<string> includeList = null)
		{
			Newsletter obj = null;
			string key = cacheKeyPrefix + NewsletterID + GetCacheIncludeText(includeList);

			Newsletter tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Newsletter;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Newsletter> itemQuery = AddIncludes(entity.Newsletter, includeList);
					obj = itemQuery.FirstOrDefault(n => n.NewsletterID == NewsletterID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Newsletter> GetAll(IEnumerable<string> includeList = null)
		{
			List<Newsletter> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Newsletter> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Newsletter>();
				tmpList = Cache[key] as List<Newsletter>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Newsletter> itemQuery = AddIncludes(entity.Newsletter, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Newsletter> NewsletterGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterActive = Active.ToString();
			return NewsletterPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Newsletter> NewsletterGetByArchived(Boolean Archived, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterArchived = Archived.ToString();
			return NewsletterPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Newsletter> NewsletterGetByCMMicrositeID(Int32? CMMicrositeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterCMMicrositeID = CMMicrositeID.ToString();
			return NewsletterPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Newsletter> NewsletterGetByDeleted(Boolean Deleted, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterDeleted = Deleted.ToString();
			return NewsletterPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Newsletter> NewsletterGetByFeatured(Boolean Featured, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterFeatured = Featured.ToString();
			return NewsletterPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Newsletter> NewsletterGetByIssue(String Issue, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterIssue = Issue.ToString();
			return NewsletterPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Newsletter> NewsletterGetByNewHomes(Boolean NewHomes, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterNewHomes = NewHomes.ToString();
			return NewsletterPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Newsletter> NewsletterGetByTitle(String Title, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterNewsletterTitle = Title.ToString();
			return NewsletterPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		/// <summary>
		/// The total number of records that will be displayed in the grid
		/// </summary>
		/// <returns></returns>
		public static int SelectCount()
		{
			//return the itemcount that was set when the records were retrieved
			return m_ItemCount;
		}

		public static int SelectCount(string searchText, string sortField, bool sortDirection)
		{
			return SelectCount();
		}

		public static int SelectCount(string searchText, string sortField, bool sortDirection, Filters filterList)
		{
			return SelectCount();
		}


		public static int SelectCount(string searchText, string sortField, bool sortDirection, Filters filterList, IEnumerable<string> includeList)
		{
			return SelectCount();
		}

		public static List<Newsletter> NewsletterPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Newsletter> objects = NewsletterPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Newsletter> NewsletterPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return NewsletterPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Newsletter> NewsletterPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return NewsletterPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Newsletter> NewsletterPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "NewsletterID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Newsletter> objects;
			string baseKey = cacheKeyPrefix + "NewsletterPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Newsletter> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Newsletter>();
				tmpList = Cache[key] as List<Newsletter>;
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
					IQueryable<Newsletter> itemQuery = SetupQuery(entity.Newsletter, "Newsletter", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

					objects = maximumRows <= 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : (maximumRows <= 0 || (pageNumber == 1 && objects.Count < maximumRows) ? objects.Count : itemQuery.Count());
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		/// <summary>
		/// Clear the cache, if caching is enabled
		/// </summary>
		public static void ClearCache()
		{
			if (Cache.IsEnabled)
				Cache.Purge("Newsletters_Newsletter");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterNewsletterActive { get; set; }
			public string FilterNewsletterArchived { get; set; }
			public string FilterNewsletterCMMicrositeID { get; set; }
			public string FilterNewsletterDeleted { get; set; }
			public string FilterNewsletterFeatured { get; set; }
			public string FilterNewsletterIssue { get; set; }
			public string FilterNewsletterNewHomes { get; set; }
			public string FilterNewsletterTitle { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterNewsletterActive != null)
				{
					if (FilterNewsletterActive == string.Empty)
						filterList.Add("@FilterNewsletterActive", string.Empty);
					else
						filterList.Add("@FilterNewsletterActive", Convert.ToBoolean(FilterNewsletterActive));
				}
				if (FilterNewsletterArchived != null)
				{
					if (FilterNewsletterArchived == string.Empty)
						filterList.Add("@FilterNewsletterArchived", string.Empty);
					else
						filterList.Add("@FilterNewsletterArchived", Convert.ToBoolean(FilterNewsletterArchived));
				}
				if (FilterNewsletterCMMicrositeID != null)
				{
					if (FilterNewsletterCMMicrositeID == string.Empty)
						filterList.Add("@FilterNewsletterCMMicrositeID", string.Empty);
					else
						filterList.Add("@FilterNewsletterCMMicrositeID", Convert.ToInt32(FilterNewsletterCMMicrositeID));
				}
				if (FilterNewsletterDeleted != null)
				{
					if (FilterNewsletterDeleted == string.Empty)
						filterList.Add("@FilterNewsletterDeleted", string.Empty);
					else
						filterList.Add("@FilterNewsletterDeleted", Convert.ToBoolean(FilterNewsletterDeleted));
				}
				if (FilterNewsletterFeatured != null)
				{
					if (FilterNewsletterFeatured == string.Empty)
						filterList.Add("@FilterNewsletterFeatured", string.Empty);
					else
						filterList.Add("@FilterNewsletterFeatured", Convert.ToBoolean(FilterNewsletterFeatured));
				}
				if (FilterNewsletterIssue != null)
					filterList.Add("@FilterNewsletterIssue", FilterNewsletterIssue);
				if (FilterNewsletterNewHomes != null)
				{
					if (FilterNewsletterNewHomes == string.Empty)
						filterList.Add("@FilterNewsletterNewHomes", string.Empty);
					else
						filterList.Add("@FilterNewsletterNewHomes", Convert.ToBoolean(FilterNewsletterNewHomes));
				}
				if (FilterNewsletterTitle != null)
					filterList.Add("@FilterNewsletterTitle", FilterNewsletterTitle);
				return filterList;
			}
		}
	}
}