using System.Collections.Generic;

namespace Classes.Showcase
{
	public partial class Showcases
	{
		private string m_ManagersString;

		public string ManagersString
		{
			get { return m_ManagersString; }
			set { m_ManagersString = value; }
		}

		public static List<Showcases> ShowcasesPageWithManagersWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters())
		{
			List<Showcases> objects = ShowcasesPageWithTotalCount(startRowIndex, maximumRows, searchText, sortField, sortDirection, out totalCount, filterList);
			foreach (Showcases obj in objects)
			{
				List<ShowcaseUser> users = Showcase.ShowcaseUser.ShowcaseUserGetByShowcaseID(obj.ShowcaseID);
				obj.ManagersString = string.Empty;
				foreach (ShowcaseUser user in users)
				{
					obj.ManagersString += Classes.Media352_MembershipProvider.User.GetByID(user.UserID).Name + ",";
				}
				obj.ManagersString = obj.ManagersString.TrimEnd(',').Replace(",", "<br />");
			}
			return objects;
		}

		public partial struct Filters
		{
			public string FilterShowcaseUserUserID { get; set; }

			public Dictionary<string, object> GetCustomFilterList()
			{
				filterList = GetFilterList();
				if (FilterShowcaseUserUserID != null)
					filterList.Add("@FilterShowcaseUserUserID", FilterShowcaseUserUserID);
				return filterList;
			}
		}
	}
}