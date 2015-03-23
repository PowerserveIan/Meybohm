using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Classes.Media352_MembershipProvider;
using Newtonsoft.Json.Linq;

namespace BaseCode
{
	public static partial class Helpers
	{
		public static bool CanAccessAdmin(string userName = "")
		{
			List<string> adminRoles = new List<string> { "Admin", "CMS Admin", "CMS Content Integrator", "CMS Page Manager", "Dynamic Header Admin", "Newsletter Admin", "Showcase Admin", "Showcase Manager", "Microsite Admin", "Newspress Admin", "New Homes Admin" };
			if (String.IsNullOrWhiteSpace(userName))
			{
				foreach (string role in adminRoles)
				{
					if (HttpContext.Current.User.IsInRole(role))
						return true;
				}
			}
			else
			{
				string[] userRoles = System.Web.Security.Roles.GetRolesForUser(userName);
				return userRoles.Any(r => adminRoles.Any(u => u == r));
			}
			return false;
		}
		/// <summary>
		/// Calculates the approximate birds-flight distance between this coordinate and the coordinate in the parameter
		/// </summary>
		/// <returns></returns>
		public static decimal DistanceBetweenPoints(decimal? lat1, decimal? long1, decimal? lat2, decimal? long2)
		{
			if (!lat1.HasValue || !lat2.HasValue || !long1.HasValue || !long2.HasValue)
				return 0;
			bool miles = true;
			double earthRadius = miles ? 3960 : 6371;
			double dLat = Deg2Rad(lat1.Value - lat2.Value);
			double dLon = Deg2Rad(long1.Value - long2.Value);
			double a = Math.Sin(dLat / 2) *
					   Math.Sin(dLat / 2) +
					   Math.Cos(Deg2Rad(lat2.Value)) *
					   Math.Cos(Deg2Rad(lat1.Value)) *
					   Math.Sin(dLon / 2) *
					   Math.Sin(dLon / 2);
			double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
			double d = earthRadius * c;
			return Convert.ToDecimal(d);
		}

		/// <summary>
		/// Converts from Degrees to Radians
		/// </summary>
		/// <param name="deg">The degrees</param>
		/// <returns>The radians</returns>
		private static double Deg2Rad(decimal deg)
		{
			return Convert.ToDouble(deg) * Math.PI / 180.0;
		}

		public static int GetLatLong(string tempaddress, out decimal? outLat, out decimal? outLong, int failCount = 0)
		{
			outLat = outLong = null;
			decimal latitude;
			decimal longitude;
			WebRequest request = WebRequest.Create(String.Format(@"http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false", tempaddress.Replace("#", "%23").Replace("&", "%26")));
			request.Timeout = 5000;
			request.Method = "GET";
			try
			{
				WebResponse response = request.GetResponse();
				using (Stream stream = response.GetResponseStream())
				{
					if (stream != null)
						using (StreamReader reader = new StreamReader(stream))
						{
							JObject result = JObject.Parse(reader.ReadToEnd());
							if (result.SelectToken("results[0].geometry.location.lat") != null &&
								Decimal.TryParse(result.SelectToken("results[0].geometry.location.lat").ToString(), out latitude) &&
								Decimal.TryParse(result.SelectToken("results[0].geometry.location.lng").ToString(), out longitude))
							{
								outLat = latitude;
								outLong = longitude;
							}
						}
				}
			}
			catch (Exception e)
			{
				return ++failCount;
			}
			return failCount;
		}

		public static string GetLoginRedirectUrl(string userName)
		{
			string landingPage = System.Web.Security.Roles.IsUserInRole(userName, "Agent") ? "agent-home" : string.Empty;
			User userEntity = User.UserGetByName(userName, includeList: new string[] { "UserInfo", "UserInfo.CMMicrosite" }).FirstOrDefault();
			if (userEntity != null && userEntity.UserInfo.FirstOrDefault() != null && userEntity.UserInfo.FirstOrDefault().CMMicrosite != null)
				return userEntity.UserInfo.FirstOrDefault().CMMicrosite.Name.ToLower().Replace(" ", "-") + "/" + landingPage;
			Classes.ContentManager.CMMicrosite currentMicrosite = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite();
			if (currentMicrosite != null)
				return currentMicrosite.Name.ToLower().Replace(" ", "-") + "/" + landingPage;
			return landingPage;
		}

		public static string FormatPhoneNumber(string phoneNumber)
		{
			if (String.IsNullOrWhiteSpace(phoneNumber))
				return string.Empty;
			long temp;
			if (!Int64.TryParse(phoneNumber, out temp))
				return phoneNumber;
			if (phoneNumber.Length == 10)
				return "(" + phoneNumber.Substring(0, 3) + ")" + phoneNumber.Substring(3, 3) + "-" + phoneNumber.Substring(6);
			if (phoneNumber.Length == 11 && phoneNumber.StartsWith("1"))
				return "1-(" + phoneNumber.Substring(1, 3) + ")" + phoneNumber.Substring(4, 3) + "-" + phoneNumber.Substring(7);
			return phoneNumber;
		}

		public static string GetCurrentMicrositePath()
		{
			Classes.ContentManager.CMMicrosite currentMicrosite = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite();
			if (currentMicrosite != null)
				return currentMicrosite.Name.ToLower().Replace(" ", "-") + "/";
			return string.Empty;
		}

		public static bool IsMobileBrowser()
		{
			HttpContext context = HttpContext.Current;
			if (context.Request == null)
				return false;
			//FIRST TRY BUILT IN ASP.NT CHECK
			if (context.Request.Browser != null && context.Request.Browser.IsMobileDevice && (context.Request.UserAgent == null || !context.Request.UserAgent.ToLower().Contains("ipad")))
				return true;
			//THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
			if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
				return true;
			//THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
			if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
				context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
				return true;
			//AND FINALLY CHECK THE HTTP_USER_AGENT 
			//HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
			if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
			{
				//Create a list of all mobile types
				string[] mobiles = new[]
				                   	{
				                   		"blackberry", "iphone", "android"
				                   	};

				//Loop through each item in the list created above 
				//and check if the header contains that text
				foreach (string s in mobiles)
				{
					if (context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()))
						return true;
				}
			}

			return false;
		}

		public static string ResizedImageUrl(string fileName, string uploadPath, int width, int height, bool trim, bool useResizerIfExternal = true)
		{
			bool isExternal = !String.IsNullOrWhiteSpace(fileName) && fileName.ToLower().StartsWith("http");
			if (isExternal && !useResizerIfExternal)
				return fileName;
			return string.Format("~/{0}width={1}&height={2}{3}", (!String.IsNullOrWhiteSpace(fileName) ? (isExternal ? "resizer.aspx?filename=" : uploadPath) + fileName + (isExternal ? "&" : "?") : BaseCode.Globals.Settings.MissingImagePath + "?"), width, height, trim ? (!String.IsNullOrWhiteSpace(fileName) && fileName.ToLower().StartsWith("http") ? "&trim=1" : "&mode=crop&anchor=middlecenter") : "");
		}
	}
}