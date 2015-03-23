using System;
using System.Linq;
using System.Web;
using BaseCode;

namespace Classes.Showcase
{
	public static class ShowcaseHelpers
	{
		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Settings.EnableCaching, Settings.CacheDuration);

		public static bool IsShowcaseAdmin()
		{
			return (HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("Showcase Admin"));
		}

		public static int? GetDefaultShowcaseID()
		{
			int? obj = null;
			string key = "Showcase_DefaultShowcaseID";

			int? tmpClass = null;

			if (Cache.IsEnabled)
				tmpClass = Cache[key] as int?;

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				Showcases showcaseEntity = Showcases.GetAll().OrderBy(s => s.ShowcaseID).FirstOrDefault();
				if (showcaseEntity != null)
					obj = showcaseEntity.ShowcaseID;
				else if (!Settings.MultipleShowcases)
				{
					showcaseEntity = new Showcases { Active = true, Title = "Default Showcase" };
					showcaseEntity.Save();
					obj = showcaseEntity.ShowcaseID;
				}

				Cache.Store(key, obj);
			}

			return obj;
		}

		public static int? GetCurrentShowcaseID(HttpContext requestContext = null)
		{
			if (requestContext == null)
				requestContext = HttpContext.Current;
			if (requestContext == null || requestContext.Items["CurrentShowcaseID"] == "null")
				return null;
			if (requestContext.Items["CurrentShowcaseID"] == null)
			{
				//Frontend Check
				if (!HttpContext.Current.Request.Path.ToLower().Contains("admin/"))
				{
					if (Settings.MultipleShowcases)
					{
						HttpRequest currentRequest = HttpContext.Current.Request;
						int temp = 0;
						if (currentRequest.RawUrl.ToLower().Contains("showcasewebmethods.asmx") && currentRequest.UrlReferrer != null ? !String.IsNullOrEmpty(HttpUtility.ParseQueryString(currentRequest.UrlReferrer.Query)["ShowcaseID"]) && Int32.TryParse(HttpUtility.ParseQueryString(currentRequest.UrlReferrer.Query)["ShowcaseID"], out temp) : !String.IsNullOrEmpty(currentRequest.QueryString["ShowcaseID"]) && Int32.TryParse(currentRequest.QueryString["ShowcaseID"], out temp))
						{
							Showcases showcaseEntity = Showcases.GetByID(temp);
							if (showcaseEntity != null && showcaseEntity.Active)
							{
								requestContext.Items["CurrentShowcaseID"] = temp;
								return temp;
							}
						}
						else if (currentRequest.RawUrl.ToLower().Contains("showcase-item.aspx") && !String.IsNullOrEmpty(currentRequest.QueryString["ID"]) && Int32.TryParse(currentRequest.QueryString["ID"], out temp))
						{
							Showcases showcaseEntity = Showcases.GetByID(ShowcaseItem.GetByID(temp).ShowcaseID);
							if (showcaseEntity != null && showcaseEntity.Active)
							{
								requestContext.Items["CurrentShowcaseID"] = showcaseEntity.ShowcaseID;
								return showcaseEntity.ShowcaseID;
							}
						}
					}
					requestContext.Items["CurrentShowcaseID"] = GetDefaultShowcaseID();
					return GetDefaultShowcaseID();
				}
				//Backend Check
				if (Settings.MultipleShowcases)
				{
					int showcaseID;
					if (HttpContext.Current.Session != null && HttpContext.Current.Session["ShowcaseID"] != null && Int32.TryParse(HttpContext.Current.Session["ShowcaseID"].ToString(), out showcaseID))
					{
						if (Showcases.GetByID(showcaseID) == null || (!IsShowcaseAdmin() && !ShowcaseUser.ShowcaseUserGetByShowcaseID(showcaseID).Exists(s => s.UserID == Helpers.GetCurrentUserID())))
						{
							HttpContext.Current.Session.Remove("ShowcaseID");
							return null;
						}
						requestContext.Items["CurrentShowcaseID"] = showcaseID;
						return showcaseID;
					}
					return null;
				}
				int? defaultID = GetDefaultShowcaseID();
				if (defaultID != null)
				{
					SetUsersCurrentShowcaseID(defaultID.Value);
					requestContext.Items["CurrentShowcaseID"] = defaultID;
					return defaultID;
				}
				if (requestContext.Items["CurrentShowcaseID"] == null)
					requestContext.Items["CurrentShowcaseID"] = 0;
			}

			return Convert.ToInt32(requestContext.Items["CurrentShowcaseID"]);
		}

		public static void SetUsersCurrentShowcaseID(int showcaseID, HttpContext requestContext = null)
		{
			if (requestContext == null)
				requestContext = HttpContext.Current;
			if (requestContext.Session != null)
				requestContext.Session["ShowcaseID"] = showcaseID;
			requestContext.Items["CurrentShowcaseID"] = showcaseID;
		}

		public static bool IsCurrentShowcaseMLS()
		{
			int? showcaseID = GetCurrentShowcaseID();
			if (!showcaseID.HasValue)
				return false;
			return Showcases.GetByID(showcaseID.Value).MLSData;
		}		

		public static bool UserCanManageOtherShowcases()
		{
			bool returnValue;
			bool? tmpValue = null;
			int userID = Helpers.GetCurrentUserID();
			string key = "Showcase_UserCanManageOtherShowcases_" + userID;

			if (Cache.IsEnabled)
				tmpValue = Cache[key] as bool?;

			if (tmpValue != null)
				returnValue = tmpValue.Value;
			else
			{
				returnValue = IsShowcaseAdmin() || ShowcaseUser.ShowcaseUserGetByUserID(userID).Count > 1;

				Cache.Store(key, returnValue);
			}

			return returnValue;
		}

		public static void BreakUserCache(int userID)
		{
			HttpContext.Current.Cache.Remove("Showcase_UserCanManageOtherShowcases_" + userID);
		}

		

		public static bool IsAikenShowcase(int showcaseID)
		{
			return showcaseID == (int)MeybohmShowcases.AikenExistingHomes || showcaseID == (int)MeybohmShowcases.AikenLand || showcaseID == (int)MeybohmShowcases.AikenRentalHomes;
		}
	}
}