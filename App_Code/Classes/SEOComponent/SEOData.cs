using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BaseCode;

namespace Classes.SEOComponent
{
	public partial class SEOData
	{
		protected override void SaveSearch()
		{
			Cache.Purge("ContentManager_CMPage_DoesFilenameExist_");
		}

		protected override void DeleteSearch()
		{
			Cache.Purge("ContentManager_CMPage_DoesFilenameExist_");
		}

		/// <summary>
		/// Use this to delete any SEO data associated with any entity that is being hard deleted (don't do this for soft deletes).
		/// To delete all links for a microsite or sub folder, leave the filename blank.
		/// </summary>
		/// <param name="folder">By default this should be "~/".  You should also append any microsite names to the end of this.</param>
		/// <param name="filename">In general, this will be the component's details page (news-press-details.aspx)</param>
		/// <param name="idField">The query string ID field used</param>
		/// <param name="id">The id of the entity</param>
		public static void DeleteSEOData(string folder, string filename, string idField, int? id)
		{
			folder = folder.Replace(" ", "-");
			if (!String.IsNullOrEmpty(filename))
			{
				string pageLinkToDelete = folder + filename + (id.HasValue ? "?" + idField + "=" + id.Value : "");
				List<SEOData> pageLinks = SEODataGetByPageURL(pageLinkToDelete);
				//There should only be one entity, but in case bad data exists, delete all links
				foreach (SEOData link in pageLinks)
				{
					link.Delete();
				}
			}
			else
			{
				//Add a trailing slash to the folder name so we don't delete any pages with the same name as the folder (improbable but possible)
				folder = folder.TrimEnd('/') + "/";
				using (Entities entity = new Entities())
				{
					entity.SEOComponent_DeleteDataMicrositeOrFolder(folder);
				}
			}
		}

		/// <summary>
		/// Gets the SEO for display - either the one specified for the page (if any) or the default setup for the site (if any)
		/// </summary>
		/// <param name="urlPath">Rooted path (starts with "~/")</param>
		/// <param name="queryStringItems"></param>
		/// <returns></returns>
		public static SEOData GetSEOForSpecificPath(string urlPath, string queryStringItems)
		{
			SEOData obj;
			int currentLanguageID = Helpers.GetCurrentLanguage().LanguageID;
			string key = cacheKeyPrefix + "GetSEOForSpecificPath_" + urlPath + "_" + queryStringItems + "_" + currentLanguageID;

			SEOData tmpClass = null;

			if (Cache.IsEnabled)
			{
				if (Cache[key] == "NULL")
					return null;
				tmpClass = Cache[key] as SEOData;
			}

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				List<SEOData> setups = new List<SEOData>();
				using (Entities entity = new Entities())
				{
					setups = entity.SEOComponent_SitePageSEOSetupForOutput((urlPath.StartsWith("~/") ? urlPath : "~/" + urlPath), queryStringItems).ToList();
				}
				//Get most recent for current language
				//If no SEO data for language, pull in for default language
				//Lastly, if still null, get most recent for no language
				obj = (setups.LastOrDefault(s => s.LanguageID == currentLanguageID) ?? setups.LastOrDefault(s => s.LanguageID == Helpers.GetDefaultLanguageID())) ?? setups.LastOrDefault(s => !s.LanguageID.HasValue);
				if (obj == null)
					Cache.Store(key, "NULL");
				else
					Cache.Store(key, obj);
			}
			return obj;
		}

		public static Dictionary<string, string> GetPagesWithFriendlyFilenames(string pageURLContains = null, int? languageID = null)
		{
			Dictionary<string, string> objects;
			string key = cacheKeyPrefix + "GetPagesWithFriendlyFilenames_" + pageURLContains + "_" + languageID;

			Dictionary<string, string> tmpClass = null;

			if (Cache.IsEnabled)
				tmpClass = Cache[key] as Dictionary<string, string>;

			if (tmpClass != null)
				objects = tmpClass;
			else
			{
				objects = new Dictionary<string, string>();
				using (Entities entity = new Entities())
				{
					objects = entity.SEOData.Where(s => !String.IsNullOrEmpty(s.FriendlyFilename) && (String.IsNullOrEmpty(pageURLContains) || s.PageURL.ToLower().Contains(pageURLContains.ToLower())) && (!languageID.HasValue || s.LanguageID == languageID)).Select(s => new
					{
						Key = s.PageURL,
						Value = s.FriendlyFilename
					}).AsEnumerable().ToDictionary(o => o.Key, o => o.Value);
				}

				Cache.Store(key, objects);
			}
			return objects;
		}

		public static string ReplaceHtmlWithFriendlyFilenames(string html, string pageURLContains = null)
		{
			Dictionary<string, string> replacePages = GetPagesWithFriendlyFilenames(pageURLContains);
			foreach (KeyValuePair<string, string> page in replacePages)
			{
				string escapedKey = Regex.Escape(page.Key.Replace("~/", "")).Replace("\\.aspx", "(\\.aspx)?");
				html = Regex.Replace(html, escapedKey + "&(amp;)?title=[^\"&\\?'\\]\\<]*(&(amp;)?)+", page.Value.Replace("~/", "") + "?", RegexOptions.IgnoreCase);
				html = Regex.Replace(html, escapedKey + "&(amp;)?title=[^\"&\\?'\\]\\<]*", page.Value.Replace("~/", ""), RegexOptions.IgnoreCase);
				html = Regex.Replace(html, escapedKey + "&(amp;)?", page.Value.Replace("~/", "") + "?", RegexOptions.IgnoreCase);
				html = Regex.Replace(html, escapedKey + "[^\"&\\?'\\]\\<]*", page.Value.Replace("~/", ""), RegexOptions.IgnoreCase);
				html = Regex.Replace(html, escapedKey + "$", page.Value.Replace("~/", ""), RegexOptions.IgnoreCase);
			}
			return html;
		}
	}
}
