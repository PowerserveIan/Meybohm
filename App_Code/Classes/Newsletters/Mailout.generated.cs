using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Newsletters
{
	public partial class Mailout : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Newsletters_Mailout_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { }; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public Mailout(Mailout objectToCopy)
		{
			Body = objectToCopy.Body;
			BodyText = objectToCopy.BodyText;
			Description = objectToCopy.Description;
			DesignID = objectToCopy.DesignID;
			DisplayDate = objectToCopy.DisplayDate;
			Issue = objectToCopy.Issue;
			Keywords = objectToCopy.Keywords;
			MailoutID = objectToCopy.MailoutID;
			NewsletterID = objectToCopy.NewsletterID;
			Timestamp = objectToCopy.Timestamp;
			Title = objectToCopy.Title;
		}

		public virtual bool IsNewRecord
		{
			get { return MailoutID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime DisplayDateClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(DisplayDate); }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime TimestampClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(Timestamp); }
		}

		public virtual void Save()
		{
			SaveEntity("Mailout", this);
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

		public static Mailout GetByID(int MailoutID, IEnumerable<string> includeList = null)
		{
			Mailout obj = null;
			string key = cacheKeyPrefix + MailoutID + GetCacheIncludeText(includeList);

			Mailout tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Mailout;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Mailout> itemQuery = AddIncludes(entity.Mailout, includeList);
					obj = itemQuery.FirstOrDefault(n => n.MailoutID == MailoutID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Mailout> GetAll(IEnumerable<string> includeList = null)
		{
			List<Mailout> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Mailout> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Mailout>();
				tmpList = Cache[key] as List<Mailout>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Mailout> itemQuery = AddIncludes(entity.Mailout, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Mailout> MailoutGetByNewsletterID(Int32? NewsletterID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterMailoutNewsletterID = NewsletterID.ToString();
			return MailoutPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Mailout> MailoutPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Mailout> objects = MailoutPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Mailout> MailoutPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return MailoutPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Mailout> MailoutPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return MailoutPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Mailout> MailoutPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "MailoutID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Mailout> objects;
			string baseKey = cacheKeyPrefix + "MailoutPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Mailout> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Mailout>();
				tmpList = Cache[key] as List<Mailout>;
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
					IQueryable<Mailout> itemQuery = SetupQuery(entity.Mailout, "Mailout", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Newsletters_Mailout");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterMailoutNewsletterID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterMailoutNewsletterID != null)
				{
					if (FilterMailoutNewsletterID == string.Empty)
						filterList.Add("@FilterMailoutNewsletterID", string.Empty);
					else
						filterList.Add("@FilterMailoutNewsletterID", Convert.ToInt32(FilterMailoutNewsletterID));
				}
				return filterList;
			}
		}
	}
}