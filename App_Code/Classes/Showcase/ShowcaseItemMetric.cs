using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using BaseCode;
using Classes.Media352_MembershipProvider;

namespace Classes.Showcase
{
	public partial class ShowcaseItemMetric
	{
		public string ShowcaseItemTitle { get; set; }

		public int Count { get; set; }

		public decimal Percentage { get; set; }

		protected override void ClearRelatedCacheItems()
		{
			if (Cache.IsEnabled)
				Cache.Purge("Showcase_ShowcaseItem_GetPropertiesForAgent_");
		}

		public static List<ShowcaseItemMetric> GetMostPopularItems(int numberOfShowcaseItems, int? showcaseID, DateTime beginDate, DateTime endDate)
		{
			List<ShowcaseItemMetric> objects;
			string key = cacheKeyPrefix + "GetMostPopularItems_" + numberOfShowcaseItems + "_" + showcaseID + "_" + beginDate + "_" + endDate;

			List<ShowcaseItemMetric> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<ShowcaseItemMetric>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				int totalCount = 0;
				objects = new List<ShowcaseItemMetric>();
				using (Entities entity = new Entities())
				{
					var itemQuery = entity.ShowcaseItemMetric.Where(s => s.Date >= beginDate && s.Date <= endDate);
					if (showcaseID.HasValue)
						itemQuery = itemQuery.Where(s => s.ShowcaseItem.ShowcaseID == showcaseID.Value);
					var queriedList = itemQuery.GroupBy(s => new { s.ShowcaseItemID, s.ShowcaseItem.Title }).Select(s => new { s.Key.ShowcaseItemID, s.Key.Title, Count = s.Select(m => m.SessionID).Distinct().Count() }).OrderByDescending(s => s.Count).ThenBy(s => s.Title).Take(numberOfShowcaseItems).ToList();
					foreach (var item in queriedList)
					{
						ShowcaseItemMetric obj = new ShowcaseItemMetric();
						obj.ShowcaseItemID = item.ShowcaseItemID;
						obj.ShowcaseItemTitle = item.Title;
						obj.Count = item.Count;
						totalCount += item.Count;
						objects.Add(obj);
					}
				}

				foreach (ShowcaseItemMetric obj in objects)
				{
					obj.Percentage = obj.Count / (decimal)totalCount;
				}
				Cache.Store(key, objects);
			}
			return objects;
		}

		public static Dictionary<string, int> GetStatisticsForProperty(int showcaseItemID, DateTime? beginDate, DateTime? endDate, bool updateHistoricalStats = true)
		{
			Dictionary<string, int> objects;
			string key = cacheKeyPrefix + "GetStatisticsForProperty_" + showcaseItemID + "_" + beginDate + "_" + endDate;

			Dictionary<string, int> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as Dictionary<string, int>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				if (updateHistoricalStats)
					UpdateHistoricalStats(null);
				using (Entities entity = new Entities())
				{
					objects = entity.ShowcaseItemMetric.Where(s => s.ShowcaseItemID == showcaseItemID && (!beginDate.HasValue || s.Date >= beginDate) && (!endDate.HasValue || s.Date <= endDate)).GroupBy(s => new
					{
						ClickType = s.ClickType.Name
					}).Select(s => new
					{
						Key = s.Key.ClickType,
						Value = s.Count()
					}).ToDictionary(o => o.Key, o => o.Value);
				}

				Cache.Store(key, objects);
			}
			return objects;
		}

		public static void UpdateHistoricalStats(DateTime? currentDate)
		{
			string key = cacheKeyPrefix + "UpdateHistoricalStats_" + currentDate;
			bool? tmpObj = null;
			if (Cache.IsEnabled)
				tmpObj = Cache[key] as bool?;
			if (tmpObj.HasValue)
				return;
			using (Entities entity = new Entities())
			{
				entity.Showcase_UpdateHistoricalStatData(currentDate);
			}

			Cache.Store(key, true);
		}

		public static void SendPropertyStatisticEmail(ShowcaseItem property, string emailAddresses = null, string subject = null, string message = null, bool updateHistoricalStats = true)
		{
			List<ShowcaseItem> properties = new List<ShowcaseItem>();
			properties.Add(property);
			SendPropertyStatisticEmail(properties, emailAddresses, subject, message, updateHistoricalStats);
		}

		public static void SendPropertyStatisticEmail(List<ShowcaseItem> properties, string emailAddresses = null, string subject = null, string message = null, bool updateHistoricalStats = true)
		{
			if (!properties.Any())
				return;

			string agentFirstAndLast;
			string agentImage;
			string agentOfficePhone;
			string agentFax = string.Empty;
			string agentEmail;
			string agentCellPhone = string.Empty;

			if (properties.Any(a => a.AgentID.HasValue))
			{
				Classes.Media352_MembershipProvider.User userEntity = Classes.Media352_MembershipProvider.User.GetByID(properties.FirstOrDefault(a => a.AgentID.HasValue).AgentID.Value, (new[] { "UserInfo" }).ToList());
				UserInfo userInfo = userEntity.UserInfo.FirstOrDefault();
				if (userInfo == null)
					return;
				agentFirstAndLast = userInfo.FirstAndLast;
				agentImage = userInfo.Photo;
				agentOfficePhone = userInfo.OfficePhone;
				agentFax = userInfo.Fax;
				agentEmail = userEntity.Email;
				agentCellPhone = userInfo.CellPhone;
			}
			else if (properties.Any(a => a.TeamID.HasValue))
			{
				Team teamEntity = Team.GetByID(properties.FirstOrDefault(a => a.TeamID.HasValue).TeamID.Value);
				agentFirstAndLast = teamEntity.Name;
				agentImage = teamEntity.Photo;
				agentOfficePhone = teamEntity.Phone;
				agentEmail = teamEntity.Email;
			}
			else
				return;

			if (String.IsNullOrWhiteSpace(emailAddresses))
				emailAddresses = agentEmail;
			
			if (string.IsNullOrWhiteSpace(emailAddresses))
				return;	

			MailMessage email = new MailMessage
			{
				IsBodyHtml = true,
				From = new MailAddress(Globals.Settings.FromEmail)
			};
			//TODO: Remove this when done testing.  Put in place to prevent spamming of meybohm agents
			if (System.Web.HttpContext.Current.IsDebuggingEnabled && !emailAddresses.ToLower().Contains("@352media.com"))
				email.To.Add("jschroder@meybohm.com");
			else
			{
				foreach (string emailAddress in emailAddresses.Split(';'))
				{
					if (Regex.IsMatch(emailAddress, Helpers.EmailValidationExpression))
						email.To.Add(emailAddress);
				}
			}
			if (!email.To.Any())
				return;

			string propertyHtml = "<ul>";
			string basePropertyHtml = EmailTemplateService.HtmlMessageBody(EmailTemplates.PropertyItemForStatistics, null, false);

			string statsTableRowTemplate = @"<tr>
				<td>Number of [[Key]]:</td>
				<td>[[Value]]</td>
			</tr>";

			DateTime beginDate = DateTime.UtcNow.AddDays(-7);
			DateTime endDate = DateTime.UtcNow;

			bool addedProperty = false;
			foreach (ShowcaseItem property in properties)
			{
				try
				{
					Dictionary<string, int> clickCounts = GetStatisticsForProperty(property.ShowcaseItemID, beginDate, endDate, updateHistoricalStats);
					string statsSection = string.Empty;
					foreach (KeyValuePair<string, int> metric in clickCounts)
					{
						statsSection += statsTableRowTemplate.Replace("[[Key]]", metric.Key == "Visit" || metric.Key == "Share" ? metric.Key + "s" : "Clicks on " + metric.Key + " Tab")
															 .Replace("[[Value]]", metric.Value.ToString());
					}
					if (!clickCounts.Any())
						statsSection += "<tr><td colspan=\"2\">No visits this week</td></tr>";

					string msLink = property.ShowcaseID == (int)MeybohmShowcases.AikenExistingHomes || property.ShowcaseID == (int)MeybohmShowcases.AikenRentalHomes
									? "aiken/" : "augusta/";
					string searchType = property.ShowcaseID == (int)MeybohmShowcases.AikenExistingHomes || property.ShowcaseID == (int)MeybohmShowcases.AugustaExistingHomes ? (property.NewHome?"new-":"") + "search" : "rentals";
					

					string thisPropertyHtml = basePropertyHtml.Replace("[[Title]]", property.Title)
													.Replace("[[Image]]", !String.IsNullOrWhiteSpace(property.Image) && !property.Image.ToLower().StartsWith("http") ? Helpers.RootPath + Globals.Settings.UploadFolder + "images/" + property.Image : property.Image)
													.Replace("[[Address]]", property.Address.Address1 + "<br />" + property.Address.City + ", " + property.Address.State.Abb + " " + property.Address.Zip)
													.Replace("[[DateListed]]", property.DateListed.HasValue ? property.DateListed.Value.ToShortDateString() : string.Empty)
													.Replace("[[NumberOfPhotos]]", Media.GetNumberOfPhotos(property.ShowcaseItemID).ToString())
													.Replace("[[StatsFromShowcaseItemMetric]]", statsSection)
													.Replace("[[PropertyLink]]", Helpers.RootPath + msLink + searchType + "?id=" + property.ShowcaseItemID);

					if (thisPropertyHtml.IndexOf("[[SavedSearchBegin]]", StringComparison.Ordinal) > 0 && thisPropertyHtml.IndexOf("[[SavedSearchEnd]]", StringComparison.Ordinal) > 0)
						thisPropertyHtml = thisPropertyHtml.Remove(thisPropertyHtml.IndexOf("[[SavedSearchBegin]]", StringComparison.Ordinal), thisPropertyHtml.IndexOf("[[SavedSearchEnd]]", StringComparison.Ordinal) - thisPropertyHtml.IndexOf("[[SavedSearchBegin]]", StringComparison.Ordinal) + "[[SavedSearchEnd]]".Length);

					propertyHtml += thisPropertyHtml;
					addedProperty = true;
				}
				catch (Exception ex)
				{
					BaseCode.Helpers.LogException(ex);
				}
			}

			if (!addedProperty)
				return;

			propertyHtml += "</ul>";
			string propertyStatsBody = EmailTemplateService.HtmlMessageBody(EmailTemplates.PropertyStatistics, new
			{
				PersonalMessage = message,
				SiteName = Globals.Settings.SiteTitle,
				BeginDate = beginDate.ToShortDateString(),
				EndDate = endDate.ToShortDateString(),
				AgentName = agentFirstAndLast,
				AgentImage = !String.IsNullOrWhiteSpace(agentImage) ? (!agentImage.ToLower().StartsWith("http") ? Helpers.RootPath + Globals.Settings.UploadFolder + "agents/" + agentImage : agentImage) : Helpers.RootPath + Globals.Settings.UploadFolder + "images/missingFile.jpg",
				OfficePhone = agentOfficePhone,
				Fax = agentFax,
				CellPhone = agentCellPhone,
				AgentEmail = agentEmail,
				Properties = propertyHtml
			});

			if (propertyStatsBody.IndexOf("[[PersonalMessageBegin]]", StringComparison.Ordinal) > 0 && propertyStatsBody.IndexOf("[[PersonalMessageEnd]]", StringComparison.Ordinal) > 0 && String.IsNullOrWhiteSpace(message))
				propertyStatsBody = propertyStatsBody.Remove(propertyStatsBody.IndexOf("[[PersonalMessageBegin]]", StringComparison.Ordinal), propertyStatsBody.IndexOf("[[PersonalMessageEnd]]", StringComparison.Ordinal) - propertyStatsBody.IndexOf("[[PersonalMessageBegin]]", StringComparison.Ordinal) + "[[PersonalMessageEnd]]".Length);
			else
				propertyStatsBody = propertyStatsBody.Replace("[[PersonalMessageBegin]]", "").Replace("[[PersonalMessageEnd]]", "");

			email.Body = propertyStatsBody;
			email.Subject = String.IsNullOrWhiteSpace(subject) ? "Your Property Statistics from " + Globals.Settings.SiteTitle : subject;

			SmtpClient smtp = new SmtpClient();
			smtp.Send(email);
			foreach (ShowcaseItem showcaseItem in properties)
			{
				new PropertyStatisticsEmailLog { ShowcaseItemID = showcaseItem.ShowcaseItemID, Email = emailAddresses, TimeSent = DateTime.UtcNow }.Save();
			}
		}
	}
}