using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class CMSubmittedForm : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "ContentManager_CMSubmittedForm_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "FormRecipient"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public CMSubmittedForm()
		{
		}

		public CMSubmittedForm(CMSubmittedForm objectToCopy)
		{
			CMMicrositeID = objectToCopy.CMMicrositeID;
			CMSubmittedFormID = objectToCopy.CMSubmittedFormID;
			DateSubmitted = objectToCopy.DateSubmitted;
			FormHTML = objectToCopy.FormHTML;
			FormRecipient = objectToCopy.FormRecipient;
			IsProcessed = objectToCopy.IsProcessed;
			ResponsePageID = objectToCopy.ResponsePageID;
			UploadedFile = objectToCopy.UploadedFile;
		}

		public virtual bool IsNewRecord
		{
			get { return CMSubmittedFormID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime DateSubmittedClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(DateSubmitted); }
		}

		public virtual void Save()
		{
			SaveEntity("CMSubmittedForm", this);
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

		public static CMSubmittedForm GetByID(int CMSubmittedFormID, IEnumerable<string> includeList = null)
		{
			CMSubmittedForm obj = null;
			string key = cacheKeyPrefix + CMSubmittedFormID + GetCacheIncludeText(includeList);

			CMSubmittedForm tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as CMSubmittedForm;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMSubmittedForm> itemQuery = AddIncludes(entity.CMSubmittedForm, includeList);
					obj = itemQuery.FirstOrDefault(n => n.CMSubmittedFormID == CMSubmittedFormID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<CMSubmittedForm> GetAll(IEnumerable<string> includeList = null)
		{
			List<CMSubmittedForm> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<CMSubmittedForm> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMSubmittedForm>();
				tmpList = Cache[key] as List<CMSubmittedForm>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<CMSubmittedForm> itemQuery = AddIncludes(entity.CMSubmittedForm, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<CMSubmittedForm> CMSubmittedFormGetByCMMicrositeID(Int32? CMMicrositeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMSubmittedFormCMMicrositeID = CMMicrositeID.ToString();
			return CMSubmittedFormPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMSubmittedForm> CMSubmittedFormGetByDateSubmitted(DateTime DateSubmitted, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMSubmittedFormDateSubmitted = DateSubmitted.ToString();
			return CMSubmittedFormPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMSubmittedForm> CMSubmittedFormGetByFormRecipient(String FormRecipient, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMSubmittedFormFormRecipient = FormRecipient.ToString();
			return CMSubmittedFormPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMSubmittedForm> CMSubmittedFormGetByIsProcessed(Boolean IsProcessed, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMSubmittedFormIsProcessed = IsProcessed.ToString();
			return CMSubmittedFormPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<CMSubmittedForm> CMSubmittedFormGetByResponsePageID(Int32? ResponsePageID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterCMSubmittedFormResponsePageID = ResponsePageID.ToString();
			return CMSubmittedFormPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<CMSubmittedForm> CMSubmittedFormPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<CMSubmittedForm> objects = CMSubmittedFormPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<CMSubmittedForm> CMSubmittedFormPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return CMSubmittedFormPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<CMSubmittedForm> CMSubmittedFormPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return CMSubmittedFormPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<CMSubmittedForm> CMSubmittedFormPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "CMSubmittedFormID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<CMSubmittedForm> objects;
			string baseKey = cacheKeyPrefix + "CMSubmittedFormPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<CMSubmittedForm> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<CMSubmittedForm>();
				tmpList = Cache[key] as List<CMSubmittedForm>;
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
					IQueryable<CMSubmittedForm> itemQuery = SetupQuery(entity.CMSubmittedForm, "CMSubmittedForm", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("ContentManager_CMSubmittedForm");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterCMSubmittedFormCMMicrositeID { get; set; }
			public string FilterCMSubmittedFormDateSubmitted { get; set; }
			public string FilterCMSubmittedFormFormRecipient { get; set; }
			public string FilterCMSubmittedFormIsProcessed { get; set; }
			public string FilterCMSubmittedFormResponsePageID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterCMSubmittedFormCMMicrositeID != null)
				{
					if (FilterCMSubmittedFormCMMicrositeID == string.Empty)
						filterList.Add("@FilterCMSubmittedFormCMMicrositeID", string.Empty);
					else
						filterList.Add("@FilterCMSubmittedFormCMMicrositeID", Convert.ToInt32(FilterCMSubmittedFormCMMicrositeID));
				}
				if (FilterCMSubmittedFormDateSubmitted != null)
				{
					if (FilterCMSubmittedFormDateSubmitted == string.Empty)
						filterList.Add("@FilterCMSubmittedFormDateSubmitted", string.Empty);
					else
						filterList.Add("@FilterCMSubmittedFormDateSubmitted", Convert.ToDateTime(FilterCMSubmittedFormDateSubmitted));
				}
				if (FilterCMSubmittedFormFormRecipient != null)
					filterList.Add("@FilterCMSubmittedFormFormRecipient", FilterCMSubmittedFormFormRecipient);
				if (FilterCMSubmittedFormIsProcessed != null)
				{
					if (FilterCMSubmittedFormIsProcessed == string.Empty)
						filterList.Add("@FilterCMSubmittedFormIsProcessed", string.Empty);
					else
						filterList.Add("@FilterCMSubmittedFormIsProcessed", Convert.ToBoolean(FilterCMSubmittedFormIsProcessed));
				}
				if (FilterCMSubmittedFormResponsePageID != null)
				{
					if (FilterCMSubmittedFormResponsePageID == string.Empty)
						filterList.Add("@FilterCMSubmittedFormResponsePageID", string.Empty);
					else
						filterList.Add("@FilterCMSubmittedFormResponsePageID", Convert.ToInt32(FilterCMSubmittedFormResponsePageID));
				}
				return filterList;
			}
		}
	}
}