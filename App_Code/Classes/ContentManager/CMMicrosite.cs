using System.Collections.Generic;
using System.Linq;
using Classes.SEOComponent;

namespace Classes.ContentManager
{
	public partial class CMMicrosite
	{
		protected override void DeleteSEO()
		{
			SEOData.DeleteSEOData("~/" + Name, "", "", null);
		}

		public string ManagersString { get; set; }

		public static void PopulateNewMicrosite(int cmMicrositeID)
		{
			using (Entities entity = new Entities())
			{
				entity.CMS_PopulateNewMicrosite(cmMicrositeID);
			}
		}

		public static void CreatePageForMicrosites(int cmPageID, bool update)
		{
			using (Entities entity = new Entities())
			{
				entity.CMS_CreatePageForMicrosite(cmPageID, update);
			}
		}

		public static List<CMMicrosite> GetMicrositesByUserID(int? userID)
		{
			List<CMMicrosite> objects;
			string key = cacheKeyPrefix + "GetMicrositesByUserID_" + userID;

			List<CMMicrosite> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<CMMicrosite>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.CMMicrosite.Where(m => m.Active && (!userID.HasValue || m.CMMicrositeUser.Any(u => u.UserID == userID.Value))).ToList();
				}

				Cache.Store(key, objects);
			}
			return objects;
		}

		public static List<CMMicrosite> CMMicrositePageWithManagersWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters())
		{
			List<CMMicrosite> objects = CMMicrositePageWithTotalCount(startRowIndex, maximumRows, searchText, sortField, sortDirection, out totalCount, filterList);
			foreach (CMMicrosite obj in objects)
			{
				List<CMMicrositeUser> microSiteUsers = ContentManager.CMMicrositeUser.CMMicrositeUserGetByCMMicrositeID(obj.CMMicroSiteID);
				obj.ManagersString = string.Empty;
				foreach (CMMicrositeUser user in microSiteUsers)
				{
					obj.ManagersString += Media352_MembershipProvider.User.GetByID(user.UserID).Name + ",";
				}
				obj.ManagersString = obj.ManagersString.TrimEnd(',').Replace(",", "<br />");
			}
			return objects;
		}
	}
}