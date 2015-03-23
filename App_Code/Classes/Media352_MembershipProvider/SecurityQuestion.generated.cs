using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Media352_MembershipProvider
{
	public partial class SecurityQuestion : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Media352_MembershipProvider_SecurityQuestion_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Question"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public SecurityQuestion()
		{
		}

		public SecurityQuestion(SecurityQuestion objectToCopy)
		{
			Active = objectToCopy.Active;
			Question = objectToCopy.Question;
			SecurityQuestionID = objectToCopy.SecurityQuestionID;
		}

		public virtual bool IsNewRecord
		{
			get { return SecurityQuestionID < 1; }
		}

		public virtual void Save()
		{
			SaveEntity("SecurityQuestion", this);
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

		public static SecurityQuestion GetByID(int SecurityQuestionID, IEnumerable<string> includeList = null)
		{
			SecurityQuestion obj = null;
			string key = cacheKeyPrefix + SecurityQuestionID + GetCacheIncludeText(includeList);

			SecurityQuestion tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as SecurityQuestion;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SecurityQuestion> itemQuery = AddIncludes(entity.SecurityQuestion, includeList);
					obj = itemQuery.FirstOrDefault(n => n.SecurityQuestionID == SecurityQuestionID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<SecurityQuestion> GetAll(IEnumerable<string> includeList = null)
		{
			List<SecurityQuestion> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<SecurityQuestion> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SecurityQuestion>();
				tmpList = Cache[key] as List<SecurityQuestion>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<SecurityQuestion> itemQuery = AddIncludes(entity.SecurityQuestion, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<SecurityQuestion> SecurityQuestionGetByActive(Boolean Active, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSecurityQuestionActive = Active.ToString();
			return SecurityQuestionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<SecurityQuestion> SecurityQuestionGetByQuestion(String Question, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterSecurityQuestionQuestion = Question.ToString();
			return SecurityQuestionPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<SecurityQuestion> SecurityQuestionPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<SecurityQuestion> objects = SecurityQuestionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<SecurityQuestion> SecurityQuestionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return SecurityQuestionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<SecurityQuestion> SecurityQuestionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return SecurityQuestionPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<SecurityQuestion> SecurityQuestionPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SecurityQuestionID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<SecurityQuestion> objects;
			string baseKey = cacheKeyPrefix + "SecurityQuestionPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<SecurityQuestion> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SecurityQuestion>();
				tmpList = Cache[key] as List<SecurityQuestion>;
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
					IQueryable<SecurityQuestion> itemQuery = SetupQuery(entity.SecurityQuestion, "SecurityQuestion", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Media352_MembershipProvider_SecurityQuestion");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterSecurityQuestionActive { get; set; }
			public string FilterSecurityQuestionQuestion { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterSecurityQuestionActive != null)
				{
					if (FilterSecurityQuestionActive == string.Empty)
						filterList.Add("@FilterSecurityQuestionActive", string.Empty);
					else
						filterList.Add("@FilterSecurityQuestionActive", Convert.ToBoolean(FilterSecurityQuestionActive));
				}
				if (FilterSecurityQuestionQuestion != null)
					filterList.Add("@FilterSecurityQuestionQuestion", FilterSecurityQuestionQuestion);
				return filterList;
			}
		}
	}
}