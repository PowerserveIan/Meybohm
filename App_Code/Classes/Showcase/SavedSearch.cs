using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using BaseCode;
using Classes.Media352_MembershipProvider;

namespace Classes.Showcase
{
	public class WhatChanged
	{
		public SavedSearch SavedSearch { get; set; }
		public List<PropertyChangeLog> ChangeLog { get; set; }
		public List<ShowcaseItemForJSON> PropertiesThatChanged { get; set; }
	}

	public partial class SavedSearch
	{
		private static List<string> m_ChangeLogAttributesToIgnore = new List<string>(new[] { "City", "County", "Elementary School", "Middle School", "High School", "Subdivision" });


		public string SearchPageUrl { get { return "../" + (ShowcaseHelpers.IsAikenShowcase(ShowcaseID) ? "aiken/" : "augusta/") + (NewHomeSearch ? "new-search" : (ShowcaseID == (int)MeybohmShowcases.AikenExistingHomes || ShowcaseID == (int)MeybohmShowcases.AugustaExistingHomes ? "search" : "rentals")); } }

		public static List<SavedSearch> SavedSearchPageByActiveCommunitiesWithTotalCount(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, out int totalCount, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			List<SavedSearch> objects = SavedSearchPageByActiveCommunities(startRowIndex, maximumRows, searchText, sortField, sortDirection, filterList, includeList);
			totalCount = m_ItemCount;
			return objects;
		}

		public static List<SavedSearch> SavedSearchPageByActiveCommunities(int startRowIndex, int maximumRows, string searchText, string sortField, bool sortDirection, Filters filterList = new Filters(), IEnumerable<string> includeList = null)
		{
			sortField = (!string.IsNullOrWhiteSpace(sortField) ? sortField : "SavedSearchID");

			string cachingFilterText = GetCacheFilterText(filterList.GetFilterList(), searchText, includeList);

			List<SavedSearch> objects;
			string baseKey = cacheKeyPrefix + "SavedSearchPageByActiveCommunities_" + cachingFilterText;
			string key = baseKey + "_" + sortField + "_" + sortDirection + "_" + startRowIndex + "_" + maximumRows;
			string countKey = baseKey + "_count";

			List<SavedSearch> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<SavedSearch>();
				tmpList = Cache[key] as List<SavedSearch>;
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
					IQueryable<SavedSearch> itemQuery = SetupQuery(entity.SavedSearch, "SavedSearch", null, searchText, m_LikeSearchProperties, sortField, sortDirection, includeList);
					if (!String.IsNullOrEmpty(filterList.FilterSavedSearchUserID))
					{
						int userID = Convert.ToInt32(filterList.FilterSavedSearchUserID);
						itemQuery = itemQuery.Where(s => s.UserID == userID);
					}
					if (!String.IsNullOrEmpty(filterList.FilterSavedSearchShowcaseID))
					{
						int showcaseID = Convert.ToInt32(filterList.FilterSavedSearchShowcaseID);
						if (showcaseID == (int)MeybohmShowcases.AikenExistingHomes)
							itemQuery = itemQuery.Where(s => s.ShowcaseID == showcaseID || s.ShowcaseID == (int)MeybohmShowcases.AikenRentalHomes);
						else if (showcaseID == (int)MeybohmShowcases.AikenExistingHomes && filterList.FilterSavedSearchNewHomeSearch.Equals("true",StringComparison.OrdinalIgnoreCase))
							itemQuery = itemQuery.Where(s => (s.ShowcaseID == showcaseID && s.NewHomeSearch )|| s.ShowcaseID == (int)MeybohmShowcases.AikenLand);
						else if (showcaseID == (int)MeybohmShowcases.AugustaExistingHomes)
							itemQuery = itemQuery.Where(s => s.ShowcaseID == showcaseID || s.ShowcaseID == (int)MeybohmShowcases.AugustaRentalHomes);
						else if (showcaseID == (int)MeybohmShowcases.AugustaExistingHomes && filterList.FilterSavedSearchNewHomeSearch.Equals("true", StringComparison.OrdinalIgnoreCase))
							itemQuery = itemQuery.Where(s =>( s.ShowcaseID == showcaseID && s.NewHomeSearch )|| s.ShowcaseID == (int)MeybohmShowcases.AugustaLand);
					}
					itemQuery = itemQuery.Where(s => !s.ShowcaseItemID.HasValue || s.ShowcaseItem.Active);
					objects = maximumRows <= 0 ? itemQuery.ToList() : itemQuery.Skip(maximumRows * (pageNumber - 1)).Take(maximumRows).ToList();
					m_ItemCount = tmpInt.HasValue ? tmpInt.Value : (maximumRows <= 0 || (pageNumber == 1 && objects.Count < maximumRows) ? objects.Count : itemQuery.Count());
				}

				Cache.Store(key, objects);
				Cache.Store(countKey, m_ItemCount);
			}
			return objects;
		}

		public static void SendAlertEmails(SavedSearch savedSearch, WhatChanged whatChanged = null)
		{
			SendAlertEmails(new List<SavedSearch>(new[] { savedSearch }), new List<WhatChanged>(new[] { whatChanged }));
		}

		public static void SendAlertEmails(List<SavedSearch> searches, List<WhatChanged> whatChanged = null)
		{
			Classes.Media352_MembershipProvider.User userEntity = Classes.Media352_MembershipProvider.User.GetByID(searches.FirstOrDefault().UserID, (new[] { "UserInfo" }).ToList());
			string toAddress = userEntity.Email;
			string propertyLinks = string.Empty, searchLinks = string.Empty;

			string basePropertyHtml = EmailTemplateService.HtmlMessageBody(EmailTemplates.SavedSearchProperty, null, false);
			string baseFilterHtml = EmailTemplateService.HtmlMessageBody(EmailTemplates.SavedSearchFilter, null, false);
			string baseFilterPropertyHtml = EmailTemplateService.HtmlMessageBody(EmailTemplates.SavedSearchFilterProperty, null, false);

			foreach (SavedSearch search in searches)
			{
				string msLink = search.ShowcaseID == (int)MeybohmShowcases.AikenExistingHomes ||  search.ShowcaseID == (int)MeybohmShowcases.AikenRentalHomes
								? "aiken/" : "augusta/";
				string searchType = search.ShowcaseID == (int)MeybohmShowcases.AikenRentalHomes || search.ShowcaseID == (int)MeybohmShowcases.AugustaRentalHomes ? "rentals" : "search";
				if (!string.IsNullOrEmpty(search.FilterString))
				{
					string propertiesText = string.Empty;
					WhatChanged whatChangedEntity = whatChanged.Find(f => f.SavedSearch.SavedSearchID == search.SavedSearchID);
					if (whatChangedEntity != null)
					{
						foreach (ShowcaseItemForJSON property in whatChangedEntity.PropertiesThatChanged)
						{
							string whatChangedText = string.Empty;
							List<PropertyChangeLog> propertyChanges = whatChangedEntity.ChangeLog.Where(s => s.ShowcaseItemID == property.ShowcaseItemID && !m_ChangeLogAttributesToIgnore.Any(a => a == s.Attribute)).ToList();
							if (propertyChanges.Any())
							{
								if (propertyChanges.Any(c => c.Attribute == "Home Added"))
									whatChangedText += "<img alt=\"Listing Updated\" src=\"[[ROOT]]img/exclamation.png\" />&nbsp; Home Added";
								else
								{
									foreach (PropertyChangeLog change in propertyChanges)
									{
										whatChangedText += "<img alt=\"Listing Updated\" src=\"[[ROOT]]img/exclamation.png\" />&nbsp; " + change.Attribute + " Updated,";
									}
								}
							}
							else
								whatChangedText = "<img alt=\"Listing Updated\" src=\"[[ROOT]]img/exclamation.png\" />&nbsp; Home Added";

							propertiesText += baseFilterPropertyHtml.Replace("[[Address]]", property.Address + "<br />" + property.City + ", " + property.State + " " + property.Zipcode)
																		.Replace("[[Title]]", property.Title.Replace(" Bedrooms:", "<br />Bedrooms:"))
																		.Replace("[[Image]]", !String.IsNullOrWhiteSpace(property.Image) && !property.Image.ToLower().StartsWith("http") ? Helpers.RootPath + Globals.Settings.UploadFolder + "images/" + property.Image : property.Image)
																		.Replace("[[WhatChanged]]", whatChangedText.TrimEnd(',').Replace(",", "<br />"));
						}
					}

					searchLinks += baseFilterHtml.Replace("[[Url]]", Helpers.RootPath + msLink + searchType + "?" + search.FilterString)
												 .Replace("[[Name]]", search.Name)
												 .Replace("[[Properties]]", propertiesText);
				}

				if (search.ShowcaseItemID > 0)
				{
					ShowcaseItem property = ShowcaseItem.GetByID((int)search.ShowcaseItemID, new string[] { "Address", "Address.State" });
					if (!property.Active)
						continue;
					propertyLinks = string.IsNullOrEmpty(propertyLinks) ? string.Format(@"<tr style=""background-color: #f5f5f5; padding: 10px 0; width: 100%;"">
	<td style=""text-align: center; padding: 10px 0; font-family: Arial, sans-serif; font-size: 16px; color: #999; line-height: 1.4; vertical-align: top;"" colspan=""2"">
		Updated Properties: &nbsp;{0}		
	</td>
</tr>
<tr>
	<td colspan=""2"">&nbsp;</td>
</tr>", string.Join(", ", searches.Where(s=>s.ShowcaseItemID.HasValue).Select(s=>s.Name))) : propertyLinks;
					propertyLinks += basePropertyHtml.Replace("[[Title]]", property.Title.Replace(" Bedrooms:", "<br />Bedrooms:"))
												.Replace("[[Image]]", !String.IsNullOrWhiteSpace(property.Image) && !property.Image.ToLower().StartsWith("http") ? Helpers.RootPath + Globals.Settings.UploadFolder + "images/" + property.Image : property.Image)
												.Replace("[[Address]]", property.Address.Address1 + "<br />" + property.Address.City + ", " + property.Address.State.Abb + " " + property.Address.Zip)
												.Replace("[[DateListed]]", property.DateListed.HasValue ? property.DateListed.Value.ToShortDateString() : string.Empty)
												.Replace("[[NumberOfPhotos]]", Media.GetNumberOfPhotos(property.ShowcaseItemID).ToString())
												.Replace("[[WhatChanged]]", "<img alt=\"Listing Updated\" src=\"[[ROOT]]img/exclamation.png\" />&nbsp; " + PropertyChangeLog.PropertyChangeLogPage(0, 1, "", "DateStamp", false, new PropertyChangeLog.Filters { FilterPropertyChangeLogShowcaseItemID = search.ShowcaseItemID.ToString() }).FirstOrDefault().Attribute.Replace("PhotoURL", "Photos") + " Updated")
												.Replace("[[PropertyLink]]", Helpers.RootPath + msLink + searchType + "?id=" + property.ShowcaseItemID);
				}
			}

			UserInfo userInfo = userEntity.UserInfo.FirstOrDefault();

			if (string.IsNullOrWhiteSpace(toAddress) || userInfo == null || (string.IsNullOrEmpty(propertyLinks) && string.IsNullOrEmpty(searchLinks)))
				return;
			MailMessage email = new MailMessage
			{
				IsBodyHtml = true,
				From = new MailAddress(Globals.Settings.FromEmail)
			};
			email.To.Add(toAddress);
			email.Body = EmailTemplateService.HtmlMessageBody(EmailTemplates.SavedSearch, new
			{
				FirstAndLastName = userInfo.FirstAndLast,
				PropertyLinks = propertyLinks,
				SearchLinks = searchLinks
			});
			email.Subject = Globals.Settings.SiteTitle + " - Saved Search";

			SmtpClient smtp = new SmtpClient();
			smtp.Send(email);
		}
	}
}