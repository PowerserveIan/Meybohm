using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Contacts
{
	public partial class Contact : BaseGeneratedClass
	{
		protected const string cacheKeyPrefix = "Contacts_Contact_";

		protected static List<string> m_LikeSearchProperties { get { return new List<string> { "Email", "FirstName", "LastName"}; } }

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public Contact()
		{
		}

		public Contact(Contact objectToCopy)
		{
			AddressID = objectToCopy.AddressID;
			AgentID = objectToCopy.AgentID;
			CMMicrositeID = objectToCopy.CMMicrositeID;
			ContactID = objectToCopy.ContactID;
			ContactMethodID = objectToCopy.ContactMethodID;
			ContactStatusID = objectToCopy.ContactStatusID;
			ContactTimeID = objectToCopy.ContactTimeID;
			ContactTypeID = objectToCopy.ContactTypeID;
			Created = objectToCopy.Created;
			Email = objectToCopy.Email;
			FirstName = objectToCopy.FirstName;
			LastName = objectToCopy.LastName;
			Message = objectToCopy.Message;
			Phone = objectToCopy.Phone;
			ShowcaseItemID = objectToCopy.ShowcaseItemID;
			TeamID = objectToCopy.TeamID;
		}

		public virtual bool IsNewRecord
		{
			get { return ContactID < 1; }
		}

		/// <summary>
		/// Use this when you need to display the date to the user
		/// </summary>
		public virtual DateTime CreatedClientTime
		{
			get { return Helpers.ConvertUTCToClientTime(Created); }
		}

		public virtual void Save()
		{
			SaveEntity("Contact", this);
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

		public static Contact GetByID(int ContactID, IEnumerable<string> includeList = null)
		{
			Contact obj = null;
			string key = cacheKeyPrefix + ContactID + GetCacheIncludeText(includeList);

			Contact tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return null;
				tmpClass = Cache[key] as Contact;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Contact> itemQuery = AddIncludes(entity.Contact, includeList);
					obj = itemQuery.FirstOrDefault(n => n.ContactID == ContactID);
				}
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static List<Contact> GetAll(IEnumerable<string> includeList = null)
		{
			List<Contact> objects;
			string key = cacheKeyPrefix + "GetAll" + GetCacheIncludeText(includeList);

			List<Contact> tmpList = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Contact>();
				tmpList = Cache[key] as List<Contact>;
			}

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					IQueryable<Contact> itemQuery = AddIncludes(entity.Contact, includeList);
					objects = itemQuery.ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<Contact> ContactGetByAgentID(Int32? AgentID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterContactAgentID = AgentID.ToString();
			return ContactPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Contact> ContactGetByCMMicrositeID(Int32? CMMicrositeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterContactCMMicrositeID = CMMicrositeID.ToString();
			return ContactPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Contact> ContactGetByContactStatusID(Int32 ContactStatusID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterContactContactStatusID = ContactStatusID.ToString();
			return ContactPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Contact> ContactGetByContactTypeID(Int32 ContactTypeID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterContactContactTypeID = ContactTypeID.ToString();
			return ContactPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Contact> ContactGetByEmail(String Email, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterContactEmail = Email.ToString();
			return ContactPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Contact> ContactGetByFirstName(String FirstName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterContactFirstName = FirstName.ToString();
			return ContactPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Contact> ContactGetByLastName(String LastName, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterContactLastName = LastName.ToString();
			return ContactPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
		}

		public static List<Contact> ContactGetByShowcaseItemID(Int32? ShowcaseItemID, string sortField = "", bool sortDirection = true, IEnumerable<string> includeList = null)
		{
			Filters filterList = new Filters();
			filterList.FilterContactShowcaseItemID = ShowcaseItemID.ToString();
			return ContactPage(0, 0, string.Empty, sortField, sortDirection, filterList, includeList);
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

		public static List<Contact> ContactPageWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<Contact> objects = ContactPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<Contact> ContactPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection)
		{
			return ContactPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, new Filters());
		}

		public static List<Contact> ContactPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters())
		{
			return ContactPage(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, null);
		}

		public static List<Contact> ContactPage(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "ContactID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<Contact> objects;
			string baseKey = cacheKeyPrefix + "ContactPage_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<Contact> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<Contact>();
				tmpList = Cache[key] as List<Contact>;
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
					IQueryable<Contact> itemQuery = SetupQuery(entity.Contact, "Contact", filterList.GetFilterList(), searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);

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
				Cache.Purge("Contacts_Contact");
		}

		public partial struct Filters
		{
			private Dictionary<string, object> filterList;
			public string FilterContactAgentID { get; set; }
			public string FilterContactCMMicrositeID { get; set; }
			public string FilterContactContactStatusID { get; set; }
			public string FilterContactContactTypeID { get; set; }
			public string FilterContactEmail { get; set; }
			public string FilterContactFirstName { get; set; }
			public string FilterContactLastName { get; set; }
			public string FilterContactShowcaseItemID { get; set; }

			public Dictionary<string, object> GetFilterList()
			{
				filterList = new Dictionary<string, object>();
				if (FilterContactAgentID != null)
				{
					if (FilterContactAgentID == string.Empty)
						filterList.Add("@FilterContactAgentID", string.Empty);
					else
						filterList.Add("@FilterContactAgentID", Convert.ToInt32(FilterContactAgentID));
				}
				if (FilterContactCMMicrositeID != null)
				{
					if (FilterContactCMMicrositeID == string.Empty)
						filterList.Add("@FilterContactCMMicrositeID", string.Empty);
					else
						filterList.Add("@FilterContactCMMicrositeID", Convert.ToInt32(FilterContactCMMicrositeID));
				}
				if (FilterContactContactStatusID != null)
				{
					if (FilterContactContactStatusID == string.Empty)
						filterList.Add("@FilterContactContactStatusID", string.Empty);
					else
						filterList.Add("@FilterContactContactStatusID", Convert.ToInt32(FilterContactContactStatusID));
				}
				if (FilterContactContactTypeID != null)
				{
					if (FilterContactContactTypeID == string.Empty)
						filterList.Add("@FilterContactContactTypeID", string.Empty);
					else
						filterList.Add("@FilterContactContactTypeID", Convert.ToInt32(FilterContactContactTypeID));
				}
				if (FilterContactEmail != null)
					filterList.Add("@FilterContactEmail", FilterContactEmail);
				if (FilterContactFirstName != null)
					filterList.Add("@FilterContactFirstName", FilterContactFirstName);
				if (FilterContactLastName != null)
					filterList.Add("@FilterContactLastName", FilterContactLastName);
				if (FilterContactShowcaseItemID != null)
				{
					if (FilterContactShowcaseItemID == string.Empty)
						filterList.Add("@FilterContactShowcaseItemID", string.Empty);
					else
						filterList.Add("@FilterContactShowcaseItemID", Convert.ToInt32(FilterContactShowcaseItemID));
				}
				return filterList;
			}
		}
	}
}