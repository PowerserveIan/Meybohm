using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class CMTemplate : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ContentManager_CMTemplate_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Name"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);
		public CMTemplate(CMTemplate objectToCopy)
		{
			Addable = objectToCopy.Addable;
			CMTemplateID = objectToCopy.CMTemplateID;
			FileName = objectToCopy.FileName;
			MicrositeEnabled = objectToCopy.MicrositeEnabled;
			Name = objectToCopy.Name;
		}

		public virtual bool IsNewRecord
		{
			get { return CMTemplateID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("CMTemplate", this);
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

		public static CMTemplate GetByID(int CMTemplateID, IEnumerable<string> includeList = null)
		{
			CMTemplate obj = null;
			string key = cacheKeyPrefix + CMTemplateID + GetCacheIncludeText(includeList);

			CMTemplate tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as CMTemplate;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMTemplate> itemQuery = AddIncludes(entity.CMTemplate, includeList);
					obj = itemQuery.FirstOrDefault(n => n.CMTemplateID == CMTemplateID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<CMTemplate> GetAll(IEnumerable<string> includeList = null)
		{
			List<CMTemplate> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<CMTemplate> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMTemplate>();
				tmpList = Cache[key] as List<CMTemplate>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMTemplate> itemQuery = AddIncludes(entity.CMTemplate, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<CMTemplate> CMTemplateGetByMicrositeEnabled(Boolean MicrositeEnabled, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMTemplateMicrositeEnabled = MicrositeEnabled.ToString();
			return CMTemplatePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMTemplate> CMTemplateGetByName(String Name, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMTemplateName = Name.ToString();
			return CMTemplatePage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<CMTemplate> CMTemplatePageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<CMTemplate> objects = CMTemplatePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<CMTemplate> CMTemplatePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return CMTemplatePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<CMTemplate> CMTemplatePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return CMTemplatePage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<CMTemplate> CMTemplatePage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "CMTemplateID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<CMTemplate> objects;
			string baseKey = cacheKeyPrefix + "CMTemplatePage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<CMTemplate> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMTemplate>();
				tmpList = Cache[key] as List<CMTemplate>;
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
					IQueryable<CMTemplate> itemQuery = SetupQuery(entity.CMTemplate, "CMTemplate", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ContentManager_CMTemplate");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterCMTemplateMicrositeEnabled { get; set; }
			public string FilterCMTemplateName { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterCMTemplateMicrositeEnabled != null)
				{
					if (FilterCMTemplateMicrositeEnabled == string.Empty)
						filterList.Add("@FilterCMTemplateMicrositeEnabled", string.Empty);
					else
						filterList.Add("@FilterCMTemplateMicrositeEnabled", Convert.ToBoolean(FilterCMTemplateMicrositeEnabled));
				}
				if (FilterCMTemplateName != null)
					filterList.Add("@FilterCMTemplateName", FilterCMTemplateName);
				return filterList;
			}
		}
	}
}