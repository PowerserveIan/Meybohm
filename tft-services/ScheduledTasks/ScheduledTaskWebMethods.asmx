<%@ WebService Language="C#" Class="ScheduledTaskWebMethods" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using Classes.Showcase;

[WebService(Namespace = "http://352media.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class ScheduledTaskWebMethods : System.Web.Services.WebService
{
	[WebMethod]
	public void SendSavedSearchAlerts()
	{
		List<PropertyChangeLog> propertyChanges = PropertyChangeLog.GetChangesInDateRange(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
		if (!propertyChanges.Any())
			return;
		List<SavedSearch> savedSearchNotifications = SavedSearch.SavedSearchGetByEnableEmailNotifications(true);
		List<int> userIDs = savedSearchNotifications.Select(s => s.UserID).Distinct().ToList();
		foreach (int userID in userIDs)
		{
			List<SavedSearch> searchesToAlert = new List<SavedSearch>();
			List<int> propertiesToAlert = new List<int>();
			List<SavedSearch> savedProperties = savedSearchNotifications.Where(s => s.UserID == userID && s.ShowcaseItemID.HasValue).OrderByDescending(s => s.SeparateEmail).ToList();
			List<SavedSearch> savedShowcaseSearch = savedSearchNotifications.Where(s => s.UserID == userID && s.FilterString != null).OrderByDescending(s => s.SeparateEmail).ToList();
			foreach (SavedSearch savedProperty in savedProperties)
			{
				if (savedProperty.ShowcaseItemID.HasValue && propertyChanges.Any(p => p.ShowcaseItemID == savedProperty.ShowcaseItemID.Value && (!savedProperty.LastAlertDate.HasValue || p.DateStamp > savedProperty.LastAlertDate)))
				{
					if ((savedProperty.DailyEmail && (!savedProperty.LastAlertDate.HasValue || savedProperty.LastAlertDate < DateTime.UtcNow.AddDays(-1))) ||
						(!savedProperty.DailyEmail && (!savedProperty.LastAlertDate.HasValue || savedProperty.LastAlertDate < DateTime.UtcNow.AddDays(-7))))
					{
						savedProperty.LastAlertDate = DateTime.UtcNow;
						savedProperty.Save();
						if (!savedProperty.SeparateEmail)
							searchesToAlert.Add(savedProperty);
					}
				}
			}
			List<WhatChanged> whatChanged = new List<WhatChanged>();
			foreach (SavedSearch savedSearch in savedShowcaseSearch)
			{
				if (String.IsNullOrWhiteSpace(savedSearch.FilterString))
					continue;
				List<ShowcaseItemForJSON> changedProperties = ShowcaseItem.GetItemsFromSearch(savedSearch.FilterString, savedSearch.ShowcaseID, propertyChanges.Select(s => s.ShowcaseItemID).Distinct().ToList());
				if (changedProperties.Any())
				{
					if ((savedSearch.DailyEmail && (!savedSearch.LastAlertDate.HasValue || savedSearch.LastAlertDate < DateTime.UtcNow.AddDays(-1))) ||
						(!savedSearch.DailyEmail && (!savedSearch.LastAlertDate.HasValue || savedSearch.LastAlertDate < DateTime.UtcNow.AddDays(-7))))
					{
						savedSearch.LastAlertDate = DateTime.UtcNow;
						savedSearch.LastAlertCount = changedProperties.Count;
						savedSearch.Save();
						if (!savedSearch.SeparateEmail)
						{
							searchesToAlert.Add(savedSearch);
							whatChanged.Add(new WhatChanged { SavedSearch = savedSearch, ChangeLog = propertyChanges.Where(p => changedProperties.Any(s => s.ShowcaseItemID == p.ShowcaseItemID)).ToList(), PropertiesThatChanged = changedProperties });
						}
					}
				}
			}
			if (searchesToAlert.Any() || propertiesToAlert.Any())
				SavedSearch.SendAlertEmails(searchesToAlert, whatChanged);
		}
	}

	[WebMethod]
	public void SendPropertyStatisticEmails()
	{
		//Send to agents first
		List<ShowcaseItem> allPropertiesWithAgent = ShowcaseItem.GetPropertiesWithAnAgentForStatsEmail();
		List<int> userIDs = allPropertiesWithAgent.Select(s => s.AgentID.Value).Distinct().ToList();
		List<PropertyStatisticsEmailLog> todaysSentEmails = PropertyStatisticsEmailLog.GetAll().Where(s => s.TimeSentClientTime >= DateTime.Now.Date).ToList();
		foreach (int userID in userIDs)
		{
			try
			{
				List<ShowcaseItem> propertiesForAgent = allPropertiesWithAgent.Where(s => s.AgentID.Value == userID).Where(s => !todaysSentEmails.Any(e => e.ShowcaseItemID == s.ShowcaseItemID && e.Email.ToLower().EndsWith("@meybohm.com"))).ToList();
				ShowcaseItemMetric.SendPropertyStatisticEmail(propertiesForAgent, updateHistoricalStats: false);
			}
			catch (Exception ex)
			{
				BaseCode.Helpers.LogException(ex);
			}
		}

		//Send to owners
		List<ShowcaseItem> allPropertiesWithOwners = ShowcaseItem.ShowcaseItemPage(0, 0, "", "", true, new ShowcaseItem.Filters { FilterShowcaseItemActive = true.ToString(), FilterShowcaseItemRented = false.ToString(), FilterShowcaseItemStatsSentToOwner = true.ToString() }, new string[] { "Address", "Address.State" }).Where(s => !String.IsNullOrWhiteSpace(s.EmailAddresses) && s.AgentID.HasValue).ToList();
		List<string> uniqueEmailAddresses = new List<string>();

		foreach (ShowcaseItem property in allPropertiesWithOwners)
		{
			foreach (string emailAddress in property.EmailAddresses.Split(';'))
			{
				if (!uniqueEmailAddresses.Any(a => a.Equals(emailAddress, StringComparison.OrdinalIgnoreCase)) && System.Text.RegularExpressions.Regex.IsMatch(emailAddress, BaseCode.Helpers.EmailValidationExpression))
					uniqueEmailAddresses.Add(emailAddress);
			}
		}

		foreach (string emailAddress in uniqueEmailAddresses)
		{
			try
			{
				List<ShowcaseItem> propertiesForOwner = allPropertiesWithOwners.Where(s => s.EmailAddresses.ToLower().Contains(emailAddress.ToLower())).Where(s => !todaysSentEmails.Any(e => e.ShowcaseItemID == s.ShowcaseItemID && !e.Email.ToLower().EndsWith("@meybohm.com"))).ToList();
				ShowcaseItemMetric.SendPropertyStatisticEmail(propertiesForOwner, emailAddress, updateHistoricalStats: false);
			}
			catch (Exception ex)
			{
				BaseCode.Helpers.LogException(ex);
			}
		}
	}

	[WebMethod]
	public void UpdateHistoricalStatData()
	{
		ShowcaseItemMetric.UpdateHistoricalStats(null);
	}
}