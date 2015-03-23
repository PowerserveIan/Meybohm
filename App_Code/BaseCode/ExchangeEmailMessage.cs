using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using BaseCode;
using Microsoft.Exchange.WebServices.Data;

namespace BaseCode
{
	public class ExchangeEmailMessage
	{
		protected const string cacheKeyPrefix = "ExchangeEmailMessage_";

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), BaseCode.Globals.Settings.EnableCaching, BaseCode.Globals.Settings.DefaultCacheDuration);

		public DateTime DateTimeReceived { get; set; }
		public string Id { get; set; }
		public bool IsRead { get; set; }
		public string Sender { get; set; }
		public string Subject { get; set; }

		public ExchangeEmailMessage()
		{
		}

		public static List<ExchangeEmailMessage> GetEmailsFromExchangeServer(string emailAddress, int pageSize, out int totalCount)
		{
			List<ExchangeEmailMessage> objects;
			totalCount = 0;
			string key = cacheKeyPrefix + "GetEmailsFromExchangeServer_" + emailAddress + "_" + pageSize;
			string countKey = key + "_count";

			List<ExchangeEmailMessage> tmpList = null;
			int? tmpInt = null;

			if (Cache.IsEnabled)
			{
				if (Cache.IsEmptyCacheItem(key))
					return new List<ExchangeEmailMessage>();
				tmpList = Cache[key] as List<ExchangeEmailMessage>;
				tmpInt = Cache[countKey] as int?;
			}

			if (tmpList != null && tmpInt.HasValue)
			{
				objects = tmpList;
				totalCount = tmpInt.Value;
			}
			else
			{
				ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
				service.Credentials = new WebCredentials(ConfigurationManager.AppSettings["ExchangeServer_AdminUserName"].ToString(), ConfigurationManager.AppSettings["ExchangeServer_AdminPassword"].ToString());
				service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.PrincipalName, emailAddress);
				//service.AutodiscoverUrl(userName);
				service.Url = new Uri("https://mail.meybohm.com/EWS/Exchange.asmx");
				//service.UseDefaultCredentials = true;
				ItemView view = new ItemView(pageSize);
				view.Traversal = ItemTraversal.Shallow;
				view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties);
				FindItemsResults<Item> emails = service.FindItems(WellKnownFolderName.Inbox, view);
				objects = new List<ExchangeEmailMessage>();
				foreach (Item email in emails.Items)
				{
					objects.Add(new ExchangeEmailMessage { DateTimeReceived = email.DateTimeReceived, Id = email.Id.UniqueId, IsRead = ((EmailMessage)email).IsRead, Sender = ((EmailMessage)email).From.Name, Subject = email.Subject });
				}
				totalCount = emails.TotalCount;

				Cache.Store(key, objects);
				Cache.Store(countKey, totalCount);
			}
			return objects;
		}
	}
}