using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Optimization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Classes.SiteLanguages;
using Elmah;

namespace BaseCode
{
	public static partial class Helpers
	{
		private const string Root = "[[ROOT]]";

		/// <summary>
		/// Most sites will want a more stringent email validator, however, this validation expression lets all RFC compliant addresses through
		/// </summary>
		public const string EmailValidationExpression = @"^[^,;]{1,64}@[^,;]{1,255}\.[^,;]{1,60}[^.]$";

		/// <summary>
		/// RegEx expressiong for US zipcodes (##### or #####-####)
		/// </summary>
		public const string USZipCodeValidationExpression = "^\\d{3,5}(-\\d{4})?$";

		public static string MembersAreaLink
		{
			get
			{
				return HttpContext.Current.User.IsInRole("Admin") ? "admin" : "/";
			}
		}

		public static bool CanContentManage
		{
			get
			{
				return HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("CMS Admin");
			}
		}

		/// <summary>
		/// Gets absolute path (ends with a /)
		/// </summary>
		public static string RootPath
		{
			get { return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/"; }
		}

		/// <summary>
		/// Gets relative path (ends with a /)
		/// </summary>
		public static string BaseUrl
		{
			get
			{
				string appPath = HttpContext.Current.Request.ApplicationPath;
				appPath = appPath == "/" ? "/" : appPath + "/";
				return appPath;
			}
		}

		public static DateTime ConvertUTCToClientTime(DateTime utcTime, string timezone = null)
		{
			return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.SpecifyKind(utcTime, DateTimeKind.Unspecified), TimeZoneInfo.Utc.Id, (!String.IsNullOrEmpty(timezone) ? timezone : Globals.Settings.DefaultTimeZone));
		}

		public static DateTime ConvertClientTimeToUTC(DateTime clientTime, string timezone = null)
		{
			return TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(clientTime, DateTimeKind.Unspecified), TimeZoneInfo.FindSystemTimeZoneById((!String.IsNullOrEmpty(timezone) ? timezone : Globals.Settings.DefaultTimeZone)));
		}

		public static string GetFileName(HttpContext httpContext = null)
		{
			if (httpContext == null)
				httpContext = HttpContext.Current;
			if (!String.IsNullOrEmpty(httpContext.Request.QueryString["filename"]))
				return httpContext.Request.QueryString["filename"].ToLower();
			if ((httpContext.Request.ApplicationPath == "/" ? "" : httpContext.Request.ApplicationPath + "/") != "")
				return httpContext.Request.Url.LocalPath.ToLower().Replace((httpContext.Request.ApplicationPath == "/" ? "" : httpContext.Request.ApplicationPath + "/").ToLower(), "");
			else
				return httpContext.Request.Url.LocalPath.Substring(1, httpContext.Request.Url.LocalPath.Length - 1).ToLower();
			return string.Empty;
		}

		public static readonly ICacheProvider Cache = new AspDotNetCacheProvider(new HttpContextWrapper(HttpContext.Current), Globals.Settings.EnableCaching, Globals.Settings.DefaultCacheDuration);

		/// <summary>
		/// Checks to see if the CMS is installed and then checks to see if the CMS page exists
		/// </summary>
		/// <param name="pageName"></param>
		/// <param name="seoLink"></param>
		/// <returns></returns>
		public static bool DoesFilenameExist(string pageName, string seoLink = null)
		{
			bool returnValue;
			bool? tmpValue = null;
			string key = "ContentManager_CMPage_DoesFilenameExist_" + pageName + "_" + seoLink;

			if (Cache.IsEnabled)
				tmpValue = Cache[key] as bool?;

			if (tmpValue != null)
				returnValue = tmpValue.Value;
			else
			{
				string microsite = null;
				string page = pageName;

				if (pageName.Contains("/"))
				{
					microsite = pageName.Split('/')[0];
					page = pageName.Split('/')[1];
				}

				using (Entities entity = new Entities())
				{
					returnValue = entity.SiteWide_DoesFilenameExist(page, microsite, seoLink).FirstOrDefault().Value;
				}
				Cache.Store(key, returnValue);
			}

			return returnValue;
		}

		/// <summary>
		/// Checks a URL and returns false if header status is anything other than OK.
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static bool DoesWebpageExist(string url)
		{
			try
			{
				System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
				req.Method = "HEAD";
				using (System.Net.HttpWebResponse rsp = (System.Net.HttpWebResponse)req.GetResponse())
				{
					if (rsp.StatusCode == System.Net.HttpStatusCode.OK)
					{
						return true;
					}
				}
			}
			catch (System.Net.WebException)
			{
				// Eat it because all we want to do is return false  
			}

			// Otherwise  
			return false;
		}

		public static ContentPlaceHolder FindContentPlaceHolder(Control parentControl, string id)
		{
			ContentPlaceHolder thePlaceHolder = (ContentPlaceHolder)parentControl.FindControl(id);
			if (thePlaceHolder != null)
				return thePlaceHolder;
			foreach (Control ctrl in parentControl.Controls)
			{
				if (ctrl is ContentPlaceHolder || ctrl is System.Web.UI.HtmlControls.HtmlContainerControl)
				{
					thePlaceHolder = FindContentPlaceHolder(ctrl, id);
					if (thePlaceHolder != null)
						return thePlaceHolder;
				}
			}
			return thePlaceHolder;
		}

		public static AspNetHostingPermissionLevel GetCurrentTrustLevel()
		{
			const string key = "Helpers_GetCurrentTrustLevel";

			if (Globals.Settings.EnableCaching && HttpContext.Current != null && HttpContext.Current.Cache[key] != null)
				return (AspNetHostingPermissionLevel)HttpContext.Current.Cache[key];
			foreach (AspNetHostingPermissionLevel trustLevel in
					new[] {
                AspNetHostingPermissionLevel.Unrestricted,
                AspNetHostingPermissionLevel.High,
                AspNetHostingPermissionLevel.Medium,
                AspNetHostingPermissionLevel.Low,
                AspNetHostingPermissionLevel.Minimal 
            })
			{
				try
				{
					new AspNetHostingPermission(trustLevel).Demand();
				}
				catch (System.Security.SecurityException)
				{
					continue;
				}

				if (Globals.Settings.EnableCaching && HttpContext.Current != null)
					HttpContext.Current.Cache.Insert(key, trustLevel, null,
													 DateTime.Now.AddSeconds(Globals.Settings.DefaultCacheDuration), TimeSpan.Zero);
				return trustLevel;
			}

			return AspNetHostingPermissionLevel.None;
		}

		/// <summary>
		/// Used to determine if at least one object is selected.  Should be used by custom validators to check list controls.
		/// </summary>
		/// <param name="listObject"></param>
		/// <returns></returns>
		public static bool IsAListItemSelected(ListControl listObject)
		{
			return listObject.Items.Cast<ListItem>().Any(li => li.Selected);
		}

		public static void GetCSSCode(System.Web.UI.HtmlControls.HtmlGenericControl combinedLink)
		{
			combinedLink.Visible = false;
			Page page = (Page)HttpContext.Current.CurrentHandler;
			if (page.Header == null)
				return;

			foreach (Control c in page.Header.Controls)
			{
				if (c is System.Web.UI.HtmlControls.HtmlLink && ((System.Web.UI.HtmlControls.HtmlLink)c).Href.Contains(combinedLink.Attributes["Href"]))
					return;
			}
			System.Web.UI.HtmlControls.HtmlLink link = new System.Web.UI.HtmlControls.HtmlLink();
			link.Href = combinedLink.Attributes["Href"];
			link.Attributes["media"] = combinedLink.Attributes["media"];
			link.Attributes["type"] = combinedLink.Attributes["type"];
			link.Attributes["rel"] = combinedLink.Attributes["rel"];
			GetCSSCode(link);
		}

		private static List<string> BundlesAlreadyRendered
		{
			get
			{
				if (HttpContext.Current.Items["BundlesAlreadyRendered"] == null)
					HttpContext.Current.Items["BundlesAlreadyRendered"] = new List<string>();
				return (List<string>)HttpContext.Current.Items["BundlesAlreadyRendered"];
			}
			set
			{
				HttpContext.Current.Items["BundlesAlreadyRendered"] = value;
			}
		}

		/// <summary>
		/// Method to get Combined or non-combined CSS references
		/// </summary>
		public static void GetCSSCode(System.Web.UI.HtmlControls.HtmlLink combinedLink)
		{
			Page page = (Page)HttpContext.Current.CurrentHandler;
			if (page.Header == null)
			{
				combinedLink.Visible = false;
				return;
			}
			if (HttpContext.Current.Request.IsSecureConnection)
				combinedLink.Href = combinedLink.Href.Replace("http://", "https://");
			combinedLink.Href = combinedLink.Href.Replace(" ", "").TrimEnd(',');
			combinedLink.EnableViewState =
			combinedLink.Visible = false;
			string foldersFromRoot = combinedLink.Href.Split(',')[0].Substring(0, combinedLink.Href.Split(',')[0].LastIndexOf('/') + 1);
			string bundleName = foldersFromRoot;
			foreach (string cssFile in combinedLink.Href.Split(','))
			{
				bundleName += cssFile.Substring(cssFile.LastIndexOf('/') + 1).Replace(".css", "").Replace(".", "");
			}
			if (BundleTable.Bundles.FirstOrDefault(b => b.Path == bundleName) == null)
			{
				StyleBundle cssBundle = new StyleBundle(bundleName);
				foreach (string cssFile in combinedLink.Href.Split(','))
				{
					if (!String.IsNullOrWhiteSpace(cssFile))
						cssBundle.Include(cssFile);
				}
				BundleTable.Bundles.Add(cssBundle);
			}
			if (!BundlesAlreadyRendered.Contains(bundleName))
			{
				//This is a hacky work around for the media query
				string bundleText = Styles.Render(bundleName).ToHtmlString();
				string fixedBundleText = string.Empty;
				foreach (string link in bundleText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
				{
					fixedBundleText += link.Replace("rel=\"stylesheet\"", "rel=\"stylesheet\" media=\"" + combinedLink.Attributes["media"] + "\"");
				}
				page.Header.Controls.AddAt(page.Header.Controls.IndexOf(combinedLink), new Literal { Text = fixedBundleText });
				BundlesAlreadyRendered.Add(bundleName);
			}
		}

		/// <summary>
		/// Method to get Combined or non-combined JS references
		/// </summary>
		/// <param name="fileReferences">Comma separated list of rooted JS files (ex. "~/tft-js/core/Validation.js,~/tft-js/core/jquery.fancybox.js")</param>
		/// <returns></returns>
		public static string GetJSCode(string fileReferences)
		{
			string bundleName = "~/tft-js/";
			foreach (string jsFile in fileReferences.Split(','))
			{
				bundleName += jsFile.Substring(jsFile.LastIndexOf('/') + 1).Replace(".js", "").Replace(".", "");
			}
			if (BundleTable.Bundles.FirstOrDefault(b => b.Path == bundleName) == null)
			{
				ScriptBundle jsBundle = new ScriptBundle(bundleName);
				foreach (string jsFile in fileReferences.Split(','))
				{
					if (!String.IsNullOrWhiteSpace(jsFile))
						jsBundle.Include(jsFile);
				}
				BundleTable.Bundles.Add(jsBundle);
			}
			if (!BundlesAlreadyRendered.Contains(bundleName))
			{
				BundlesAlreadyRendered.Add(bundleName);
				return Scripts.Render(bundleName).ToHtmlString();
			}
			return string.Empty;
		}

		public static void GetJSCode(Literal uxJavaScripts, bool useLiteralLocation = false)
		{
			string scriptsName = uxJavaScripts.Text;
			uxJavaScripts.Text = GetJSCode(uxJavaScripts.Text);
			Page page = (Page)HttpContext.Current.CurrentHandler;
			if (page != null)
			{
				uxJavaScripts.EnableViewState = false;
				if (!useLiteralLocation)
				{
					page.ClientScript.RegisterStartupScript(page.GetType(), "Scripts_" + scriptsName, uxJavaScripts.Text);
					uxJavaScripts.Visible = false;
				}
			}
		}

		public static void SetTinyMCESessionVariables()
		{
			if (((Page)HttpContext.Current.Handler).User.Identity.IsAuthenticated)
			{
				HttpContext.Current.Session["mc_isLoggedIn"] = "true";
				HttpContext.Current.Session["mc_user"] = ((Page)HttpContext.Current.Handler).User.Identity.Name;
				HttpContext.Current.Session["mc_groups"] = string.Empty;
				HttpContext.Current.Session["imagemanager.preview.urlprefix"] =
				HttpContext.Current.Session["filemanager.preview.urlprefix"] =
				HttpContext.Current.Session["imagemanager.urlprefix"] =
				HttpContext.Current.Session["filemanager.urlprefix"] = RootPath;
				HttpContext.Current.Session["imagemanager.preview.wwwroot"] =
				HttpContext.Current.Session["filemanager.preview.wwwroot"] = HttpContext.Current.Request.PhysicalApplicationPath.Replace("\\", "/").TrimEnd('/');
			}
		}

		/// <summary>
		/// Used on Admin Add pages to get the maximum currently used display order
		/// </summary>
		/// <param name="tableName">Classname of the current listing page</param>
		/// <param name="tableIDField">ID Field name of the current listing page</param>
		/// <param name="parentTableIDField">Nullable ID Field name of the parent object of the listing page
		/// (ex: OptionID for OptionValue listing page)</param>
		/// <param name="parentTableID">Nullable ID of the parent object of the listing page</param>
		/// <returns></returns>
		public static short GetMaxDisplayOrder(string tableName, string tableIDField, string parentTableIDField, int? parentTableID)
		{
			return GetMaxDisplayOrder(tableName, tableIDField, parentTableIDField, parentTableID, null, null);
		}

		/// <summary>
		/// Used on Admin Add pages to get the maximum currently used display order
		/// </summary>
		/// <param name="tableName">Classname of the current listing page</param>
		/// <param name="tableIDField">ID Field name of the current listing page</param>
		/// <param name="parentTableIDField">Nullable ID Field name of the parent object of the listing page
		/// (ex: OptionID for OptionValue listing page)</param>
		/// <param name="parentTableID">Nullable ID of the parent object of the listing page</param>
		/// <param name="additionalIDField">Nullable ID field name to filter down what to get the display order from</param>
		/// <param name="additionalID">Nullable ID to filter down what to get the display order from</param>
		/// <returns></returns>
		public static short GetMaxDisplayOrder(string tableName, string tableIDField, string parentTableIDField, int? parentTableID, string additionalIDField, int? additionalID)
		{
			short obj;
			using (Entities entity = new Entities())
			{
				obj = entity.SiteWide_GetMaxDisplayOrder(tableName, tableIDField, parentTableID, parentTableIDField, additionalIDField, additionalID).FirstOrDefault().Value;
			}

			return obj;
		}

		public static string RenderHtmlAsString(Control ctrl)
		{
			StringBuilder sb = new StringBuilder();
			using (StringWriter sw = new StringWriter(sb))
			{
				using (HtmlTextWriter textWriter = new HtmlTextWriter(sw))
				{
					ctrl.RenderControl(textWriter);
				}
			}
			return sb.ToString();
		}

		public static List<XElement> GetRssFeedAsXmlList(string feedUrl, int numberToTake)
		{
			List<XElement> objects;
			string key = "BaseCode_GetRssFeedAsXmlList_" + feedUrl + "_" + numberToTake;

			List<XElement> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<XElement>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				XElement feed = XElement.Load(feedUrl);
				if (feed.Elements("channel").Any())
					objects = (from item in feed.Elements("channel").Elements("item")
							   select item).ToList();
				else
				{
					objects = new List<XElement>();
					objects.Add(feed);
				}
				objects = objects.Take(numberToTake).ToList();
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static string StripNonAlphaCharacters(string toStrip, string characterToReplaceWith = "", bool allowNumeric = true, bool allowCharacters = true, bool allowSpaces = true)
		{
			if (!string.IsNullOrEmpty(toStrip))
			{
				string regExToUse = string.Empty;

				if (allowNumeric)
					regExToUse = "0-9";

				if (allowCharacters)
					regExToUse += "a-zA-Z";

				if (allowSpaces)
					regExToUse += " ";

				if (regExToUse.Length > 0)
					regExToUse = "[^" + regExToUse + "]";

				Regex regex = new Regex(regExToUse);

				return regex.Replace(toStrip, characterToReplaceWith);
			}

			return string.Empty;
		}

		/// <summary>
		/// Takes an exception and logs it via ELMAH
		/// </summary>
		/// <param name="e"></param>
		public static void LogException(Exception e)
		{
			if (HttpContext.Current != null)
			{
				try
				{
					ErrorSignal.FromCurrentContext().Raise(e);
				}
				catch (Exception)
				{
				}
			}
			else
			{
				//You won't be emailed but the error will be logged in the database
				ErrorLog.GetDefault(null).Log(new Error(e));
				try
				{
					MailMessage email = new MailMessage();
					email.From = new MailAddress(Globals.Settings.FromEmail);
					ErrorMailHtmlFormatter formatter = new ErrorMailHtmlFormatter();
					StringWriter sw = new StringWriter();
					formatter.Format(sw, new Error(e));
					email.To.Add("zfloyd@352media.com");
					email.IsBodyHtml = true;
					email.Body = sw.ToString();
					email.Subject = "ELMAH Error Logged on " + Globals.Settings.SiteTitle + " - No HttpContext";
					SmtpClient smtp = new SmtpClient();
					smtp.Send(email);
				}
				catch (Exception)
				{
				}
			}
		}

		/// <summary>
		/// Sends permanent redirection headers (301)
		/// </summary>
		public static void PermanentRedirect(string url, HttpContext context)
		{
			if (url.EndsWith("Default.aspx", StringComparison.OrdinalIgnoreCase))
				url = url.Replace("Default.aspx", string.Empty);

			context.Response.Clear();
			context.Response.StatusCode = 301;
			context.Response.AppendHeader("location", url);
			context.Response.End();
		}

		/// <summary>
		/// This adds in an html line break wherever a user hits enter which should indicate a new paragraph.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string MaintainLineBreaks(string input)
		{
			return input.Replace(Environment.NewLine, "<br />").Replace("\n", "<br />");
		}

		/// <summary>
		/// Recursive function used to find a parent reference of a specified type.  Very useful for finding a control inside of a repeater item on a command event
		/// </summary>
		/// <param name="current"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Control GetParentRef(Control current, string type)
		{
			bool match = false;
			Control parent = new Control();
			while (!match)
			{
				//loops through parent structure until there is a match with the desired type

				if (current.Parent != null)
				{
					parent = current.Parent;
					match = parent.GetType().ToString() == type;
				}
				else
				{
					throw new Exception("Desired control not found");
				}
				if (!match)
					current = parent;
			}
			return parent;
		}

		public static int GetCurrentUserID()
		{
			int returnValue = 0;

			if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
			{
				//Use the applications instantiated membership provider
				Media352_MembershipProvider membership = (Media352_MembershipProvider)Membership.Provider;
				MembershipUser user = membership.GetUser(HttpContext.Current.User.Identity.Name, false);
				if (user != null)
					returnValue = (int)user.ProviderUserKey;
			}

			return returnValue;
		}

		/// <summary>
		/// Written By Charles Cook:
		/// With Guest Author: Jared Allen
		/// 
		/// This method removes html which is not allowed through a whitelist.
		/// Any tag which is not within the whitelist is HTML Encoded.
		/// Any attribute not within the attribute whitelist is removed as well.
		/// Attribute values may be either a URL or single word.  This supports
		/// simple attributes such as href="http://example.com" and align="center".
		/// 
		/// Note: Nothing is unhackable! (yet this can't be hacked by us)
		/// </summary>
		/// <param name="html">HTML to clean</param>
		/// <returns></returns>
		public static string HtmlEncodeUnsafe(string html)
		{
			bool hasMatch = true;

			List<string> safe = new List<string>(new[]
			                                     	{
			                                     		"a", "p", "b", "em", "strong", "sup", "sub", "br", "strike", "br", "ol", "li", "ul", "lu", "u"
			                                     	});

			List<string> safeAttributes = new List<string>(new[]
			                                               	{
			                                               		"align", "href", "target"
			                                               	});

			MatchCollection matches = Regex.Matches(html,
													@"</?(\w+)((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>",
													RegexOptions.IgnoreCase);

			while (hasMatch)
			{
				hasMatch = false;
				foreach (Match m in matches)
				{
					if (!safe.Contains(m.Groups[1].Value.ToLower()))
					{
						hasMatch = true;
						html = html.Replace(m.Groups[0].Value, HttpContext.Current.Server.HtmlEncode(m.Groups[0].Value));
					}
					else if (m.Groups.Count > 2)
					{
						if (!String.IsNullOrEmpty(m.Groups[2].Value))
						{
							string attributeValue;
							bool invalid = false;
							if (m.Groups[2].Value.ToLower().Contains("="))
							{
								string attribute = m.Groups[2].Value.ToLower().Split('=')[0];
								if (!safeAttributes.Contains(attribute.Trim()))
								{
									invalid = true;
								}
								else
								{
									attributeValue = m.Groups[2].Value.Replace(attribute + "=", "");
									attributeValue = attributeValue.Replace("\"", "").Replace("'", "");

									if (Regex.Match(attributeValue, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?")
											.Captures.Count == 0)
										if (Regex.Match(attributeValue.Trim(), @"^\\w+$").Length != 0)
										{
											invalid = true;
										}
								}

								if (invalid)
								{
									hasMatch = true;
									html = html.Replace(m.Groups[2].Value, "");
								}
							}
							else
							{
								hasMatch = true;
								html = html.Replace(m.Groups[2].Value, "");
							}
						}
					}
				}
				matches = Regex.Matches(html,
										@"</?(\w+)((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.IgnoreCase);
			}
			return html;
		}

		public static string WriteLocalPathWithQuery(
			this NameValueCollection collection,
			Uri url)
		{
			if (collection.Count == 0)
				return url.LocalPath;

			StringBuilder sb = new StringBuilder(url.LocalPath);
			return WriteQueryString(collection, sb);
		}

		public static string WriteLocalPathWithQuery(
			this NameValueCollection collection,
			Uri url,
			string pageName)
		{
			if (collection.Count == 0)
				return url.LocalPath;

			StringBuilder sb = new StringBuilder(
				Regex.Replace(url.LocalPath, @"[^/]*?\.\w+", pageName, RegexOptions.IgnoreCase | RegexOptions.Multiline));

			return WriteQueryString(collection, sb);
		}

		public static string WriteQueryString(NameValueCollection collection, StringBuilder sb)
		{
			sb.Append("?");

			for (int i = 0; i < collection.Keys.Count; i++)
			{
				if (!String.IsNullOrEmpty(collection[i]))
				{
					sb.Append(
						String.Format("{0}={1}",
									  collection.Keys[i], HttpContext.Current.Server.UrlEncode(collection[i]))
						);
					sb.Append("&");
				}
			}
			return sb.ToString().TrimEnd('&').TrimEnd('?');
		}

		/// <summary>
		/// Will return the querystring with values you've passed in.  If you leave the value of an unacceptableValue blank, then the querystring name and value will come through.
		/// </summary>
		/// <param name="unacceptableValues"></param>
		/// <returns></returns>
		public static string GetQueryStringWithAcceptableValues(Dictionary<string, string> unacceptableValues = null, Dictionary<string, string> acceptableValues = null)
		{
			string queryString = "?";
			if (unacceptableValues != null)
				foreach (KeyValuePair<string, string> value in unacceptableValues)
				{
					if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString[value.Key]) && !HttpContext.Current.Request.QueryString[value.Key].Equals(value.Value, StringComparison.OrdinalIgnoreCase))
						queryString += value.Key + "=" + HttpContext.Current.Request.QueryString[value.Key] + "&";
				}
			if (acceptableValues != null)
				foreach (KeyValuePair<string, string> value in acceptableValues)
				{
					if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString[value.Key]) && HttpContext.Current.Request.QueryString[value.Key].Equals(value.Value, StringComparison.OrdinalIgnoreCase))
						queryString += value.Key + "=" + HttpContext.Current.Request.QueryString[value.Key] + "&";
				}
			return queryString.Replace("?&", "?").TrimEnd('&').TrimEnd('?');
		}

		public static NameValueCollection ChangeField(this NameValueCollection collection,
													  string key, string value)
		{
			return ChangeField(collection, key, value, true);
		}

		public static NameValueCollection ChangeField(this NameValueCollection collection,
													  string key, string value, bool allow)
		{
			if (allow)
			{
				if (collection[key] != null)
					collection[key] = value;
				else
					collection.Add(key, value);
			}
			else //remove the value all together
			{
				if (!String.IsNullOrEmpty(collection[key]))
					collection.Remove(key);
			}
			return collection;
		}

		public static NameValueCollection Duplicate(this NameValueCollection source)
		{
			NameValueCollection collection = new NameValueCollection();
			foreach (string key in source)
				collection.Add(key, source[key]);
			return collection;
		}

		/// <summary>
		/// Retrieves the base folder of a passed in control
		/// </summary>
		/// <param name="currentControl"></param>
		/// <returns></returns>
		public static string GetControlFolder(Control currentControl)
		{
			//Get the URL to the control's folder
			string thisFolderPath = currentControl.ResolveClientUrl(".");
			if (!string.IsNullOrEmpty(thisFolderPath))
			{
				thisFolderPath = currentControl.ResolveUrl("~/" + thisFolderPath + "/");
				HttpContext.Current.Trace.Warn(currentControl.ID, "FolderPath: " + thisFolderPath);
			}
			return thisFolderPath;
		}

		/// <summary>
		/// Returns an array with the names of all local Themes
		/// </summary>
		public static string[] GetThemes()
		{
			if (HttpContext.Current.Cache["SiteThemes"] != null)
				return (string[])HttpContext.Current.Cache["SiteThemes"];

			string themesDirPath = HttpContext.Current.Server.MapPath("~/App_Themes");
			// get the array of themes folders under /app_themes
			string[] themes = Directory.GetDirectories(themesDirPath);
			for (int i = 0; i <= themes.Length - 1; i++)
				themes[i] = Path.GetFileName(themes[i]);
			// cache the array with a dependency to the folder
			CacheDependency dep = new CacheDependency(themesDirPath);
			HttpContext.Current.Cache.Insert("SiteThemes", themes, dep);
			return themes;
		}

		public static bool IsNumeric(string num)
		{
			double retNum;
			return Double.TryParse(Convert.ToString(num), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
		}

		/// <summary>
		/// Converts the input plain-text to HTML version, replacing carriage returns
		/// and spaces with <br /> and &nbsp;
		/// </summary>
		public static string ConvertToHtml(string content)
		{
			content = HttpUtility.HtmlEncode(content);
			content = content.Replace("  ", "&nbsp;&nbsp;").Replace(
				"\t", "&nbsp;&nbsp;&nbsp;").Replace("\n", "<br />");
			return content;
		}

		/// <summary>
		/// Remove from the ASP.NET cache all items whose key starts with the input prefix.  
		/// No prefix will clear entire site's cache.
		/// </summary>
		public static void PurgeCacheItems(string prefix)
		{
			if (HttpContext.Current == null)
				return;

			if (prefix == null)
				prefix = string.Empty;
			prefix = prefix.ToLower();
			List<string> itemsToRemove = new List<string>();

			IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Key.ToString().ToLower().StartsWith(prefix))
					itemsToRemove.Add(enumerator.Key.ToString());
			}

			foreach (string itemToRemove in itemsToRemove)
				HttpContext.Current.Cache.Remove(itemToRemove);
		}

		public static int CurrentAge(DateTime birthDate)
		{
			return birthDate.AddYears((DateTime.UtcNow.Year - birthDate.Year)) > DateTime.UtcNow ? DateTime.UtcNow.Year - birthDate.Year - 1 : DateTime.UtcNow.Year - birthDate.Year;
		}

		public static string RemoveIgnoredWords(string word)
		{
			string[] iw = @"about,after,all,also,an,and,another,any,are,as,at,be,because,been,before,
							being,between,both,but,by,came,can,come,could,did,do,each,for,from,get,
							got,has,had,he,have,her,here,him,himself,his,how,if,in,into,is,it,like,
							make,many,me,might,more,most,much,must,my,never,now,of,on,only,or,other,
							our,out,over,said,same,see,should,since,some,still,such,take,than,that,
							the,their,them,then,there,these,they,this,those,through,to,too,under,up,
							very,was,way,we,well,were,what,where,which,while,who,with,would,you,your,a,
							b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,$,1,2,3,4,5,6,7,8,9,0,_".Split(new[] { ',' });
			foreach (string s in iw)
			{
				word = word.Replace(" " + s + " ", " ");
			}
			return word;
		}

		/// <summary>
		/// Method to replace RadEditor HTML rootpath with a Root marker
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string ReplaceAbsolutePathWithRoot(string html)
		{
			string dots = string.Empty;
			for (int i = 0; i < (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Split('/').Length - 2); i++)
			{
				dots += "../";
			}
			html = html.Replace(RootPath.TrimEnd('/') + "/", Root);
			if (!String.IsNullOrEmpty(dots))
				html = html.Replace(dots, Root);
			return html;
		}

		public static string ReplaceRootWithRelativePath(string html, int numberOfFoldersToRoot)
		{
			if (String.IsNullOrEmpty(html))
				return "";

			string dots = string.Empty;
			for (int i = 0; i < numberOfFoldersToRoot; i++)
			{
				dots += "../";
			}
			if (String.IsNullOrEmpty(dots))
				dots = HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
			return html.Replace(Root, dots);
		}

		public static string ReplaceRootWithAbsolutePath(string html)
		{
			if (!String.IsNullOrEmpty(html))
				return html.Replace(Root, RootPath.TrimEnd('/') + "/");//Trim is to ensure that the root never has a double slash
			return "";
		}

		public static string ForceShorten(string strData, int datalength)
		{
			// truncates string data that is too long and adds '...' to the end
			string strConvert;
			if (strData != null)
			{
				if (strData.Trim().Length > datalength)
				{
					strConvert = strData.Substring(0, datalength - 1);
					int placeholder = strConvert.LastIndexOf(" ");
					if (placeholder == -1)
					{
						placeholder = strData.IndexOf(" ");
						if (placeholder == -1)
							placeholder = datalength - 1;
						strConvert = strData.Substring(0, placeholder);
						strConvert += "...";
					}
					else
					{
						strConvert = strConvert.Substring(0, placeholder);
						strConvert += "...";
					}
				}
				else
				{
					strConvert = strData;
				}
				return (strConvert);
			}
			return (" ");
		}

		public static string StripHtml(string html, bool allowHarmlessTags)
		{
			if (String.IsNullOrEmpty(html))
				return string.Empty;

			if (allowHarmlessTags)
				return Regex.Replace(html, "", string.Empty);

			return Regex.Replace(html, "<[^>]*>", string.Empty);
		}

		public static Language GetCurrentLanguage()
		{
			Language obj;
			string key = "SiteLanguages_GetCurrentLanguage_" + Thread.CurrentThread.CurrentCulture.Name;

			Language tmpClass = null;

			if (Cache.IsEnabled)
				tmpClass = Cache[key] as Language;

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				List<Language> currentLanguage = Language.LanguageGetByCultureName(Thread.CurrentThread.CurrentCulture.Name);
				if (currentLanguage.Count() == 0)
					currentLanguage = Language.LanguageGetByCultureName("en-US");
				//Add English if it doesn't exist
				if (currentLanguage.Count == 0)
					currentLanguage.Add(CreateDefaultLanguage());
				obj = currentLanguage.FirstOrDefault();
				Cache.Store(key, obj);
			}

			return obj;
		}

		public static int GetDefaultLanguageID()
		{
			int languageID;
			const string key = "SiteLanguages_GetDefaultLanguageID";

			int? tmpLanguageID = null;

			if (Cache.IsEnabled)
				tmpLanguageID = Cache[key] as int?;

			if (tmpLanguageID != null)
				languageID = tmpLanguageID.Value;
			else
			{
				using (Entities entity = new Entities())
				{
					Language obj = entity.Language.Where(l => l.CultureName == Settings.DefaultLanguageCulture).FirstOrDefault() ??
								   CreateDefaultLanguage();
					languageID = obj.LanguageID;
				}
				Cache.Store(key, languageID);
			}
			return languageID;
		}

		public static Language CreateDefaultLanguage()
		{
			Language english = new Language { Culture = "English", CultureName = "en-US", Active = true };
			english.Save();
			return english;
		}

		public static byte[] CompressViewState(byte[] data)
		{
			var ms = new MemoryStream();
			var stream = new GZipStream
				(ms, CompressionMode.Compress);
			stream.Write(data, 0, data.Length);
			stream.Close();
			return ms.ToArray();
		}

		public static byte[] DecompressViewState(byte[] data)
		{
			var ms = new MemoryStream();
			ms.Write(data, 0, data.Length);
			ms.Position = 0;
			var stream = new GZipStream(ms, CompressionMode.Decompress);
			var temp = new MemoryStream();
			var buffer = new byte[1024];
			while (true)
			{
				int read = stream.Read(buffer, 0, buffer.Length);
				if (read <= 0)
					break;
				temp.Write(buffer, 0, buffer.Length);
			}
			stream.Close();
			return temp.ToArray();
		}


		#region Nested type: ArrayListCompare

		public static class ArrayListCompare<T>
		{
			public static bool Equal(List<T> a, List<T> b)
			{
				if (a.Count != b.Count)
					return false;

				for (int i = 0; i < a.Count; i++)
				{
					T arrayElementA = a[i];
					T arrayElementB = b[i];

					if (!arrayElementA.Equals(arrayElementB))
						return false;
				}

				return true;
			}

			public static bool Equal(List<T> a, List<T> b, bool sortArrays)
			{
				if (sortArrays)
				{
					a.Sort();
					b.Sort();
				}

				return Equal(a, b);
			}
		}

		#endregion

		#region Nested type: CryptorRijndael

		public class CryptorRijndael
		{
			private static string passPhrase = "3169CA58-36BD-4898-92FF-88F9B3B84E20"; // can be any string
			private static string saltValue = "34E80D8C-4398-4A87-847C-5BBF2D909F5C"; // can be any string
			private static string hashAlgorithm = Globals.Settings.HashAlgorithm; // either "MD5"  or "SHA1"
			private static int passwordIterations = 6; // can be any number
			private static string initVector = "C%1E8aAF^286*3Fd"; // must be 16 bytes (Characters)
			private static int keySize = 192; // can be 192 or 128

			public static string Encrypt(string Val)
			{
				return Encrypt(Val, saltValue);
			}

			/// <summary>
			/// To encrypt plain text
			/// </summary>
			/// <param name="val"></param>
			/// <param name="salt"></param>
			/// <returns></returns>
			public static string Encrypt(string val, string salt)
			{
				byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
				byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
				byte[] plainTextBytes = Encoding.UTF8.GetBytes(val);

				PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

				byte[] keyBytes = password.GetBytes(keySize / 8);

				RijndaelManaged symmetricKey = new RijndaelManaged();
				symmetricKey.Mode = CipherMode.CBC;
				symmetricKey.Padding = PaddingMode.PKCS7;

				ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
				MemoryStream memoryStream = new MemoryStream();

				CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
				cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
				cryptoStream.FlushFinalBlock();

				byte[] cipherTextBytes = memoryStream.ToArray();
				memoryStream.Close();
				cryptoStream.Close();
				string cipherText = Convert.ToBase64String(cipherTextBytes);
				return cipherText;
			}

			public static string Decrypt(string cipherText)
			{
				return Decrypt(cipherText, saltValue);
			}

			/// <summary>
			/// To decrypt ciphered text
			/// </summary>
			/// <param name="cipherText"></param>
			/// <param name="salt"></param>
			/// <returns></returns>
			public static string Decrypt(string cipherText, string salt)
			{
				if (String.IsNullOrEmpty(cipherText))
					return "";
				byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
				byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
				byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

				PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

				byte[] keyBytes = password.GetBytes(keySize / 8);

				RijndaelManaged symmetricKey = new RijndaelManaged();
				symmetricKey.Mode = CipherMode.CBC;
				symmetricKey.Padding = PaddingMode.PKCS7;

				ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
				MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
				CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

				byte[] plainTextBytes = new byte[cipherTextBytes.Length];
				int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

				memoryStream.Close();
				cryptoStream.Close();

				string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
				return plainText;
			}

			/// <summary>
			/// Fantastic method ;-)))
			/// </summary>
			/// <param name="strOriginal"></param>
			/// <returns></returns>
			public static string Reverse(string strOriginal)
			{
				StringBuilder sb = new StringBuilder();
				for (int i = strOriginal.Length - 1; i >= 0; i--)
					sb.Append(strOriginal[i]);

				return sb.ToString();
			}
		}

		#endregion

		#region Nested type: PageView

		public class PageView
		{
			#region PageAnchors enum

			public enum PageAnchors
			{
				top,
				center
			}

			#endregion

			public static void Anchor(Page thispage, string locationString)
			{
				Anchor(thispage, PageAnchors.center, locationString);
			}

			public static void Anchor(Page thispage, PageAnchors location, string locationString = null)
			{
				string actualLocation = String.IsNullOrEmpty(locationString) ? location.ToString() : locationString;
				thispage.ClientScript.RegisterStartupScript(thispage.GetType(), "viewtarget", @"window.scrollTo(0, $('#" + actualLocation + "').offset().top);", true);
			}
		}

		#endregion
	}
}