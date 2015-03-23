using System;
using System.Collections.Generic;
using System.Linq;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class CMPage
	{
		private string m_CMPageTitleTitle;
		private string m_CultureName;

		/// <summary>
		/// Value pulled from the CMPageTitle table (will be null for a site that is not multilingual)
		/// </summary>
		public string CMPageTitleTitle
		{
			get
			{
				if (m_CMPageTitleTitle == null && this.CMPageTitle.Any())
					m_CMPageTitleTitle = this.CMPageTitle.FirstOrDefault().Title;
				return m_CMPageTitleTitle;
			}
			set { m_CMPageTitleTitle = value; }
		}

		/// <summary>
		/// Value pulled from the Languages table (will be null for a site that is not multilingual)
		/// Only populated by GetAllPagesNeedingApproval
		/// </summary>
		public string CultureName
		{
			get
			{
				if (m_CultureName == null && this.CMPageTitle.Any())
					m_CultureName = this.CMPageTitle.FirstOrDefault().Language.CultureName;
				return m_CultureName;
			}
			set { m_CultureName = value; }
		}

		public static CMPage GetByCMPageIDAndLanguageID(int cmPageId, int languageId)
		{
			CMPage obj;
			string key = cacheKeyPrefix + "GetByCMPageIDAndLanguageID_" + cmPageId + "_" + languageId;

			CMPage tmpClass = null;

			if (Cache.IsEnabled)
				tmpClass = Cache[key] as CMPage;

			if (tmpClass != null)
				obj = tmpClass;
			else
			{
				using (Entities entity = new Entities())
				{
					obj = entity.CMPage.Include("CMPageTitle").FirstOrDefault(p => p.CMPageID == cmPageId && p.CMPageTitle.Any(t => t.LanguageID == languageId));
				}
			}

			return obj;
		}

		public static List<CMPage> CMPageGetByCMTemplateIDAndLanguageID(int? templateId)
		{
			return CMPageGetByCMTemplateIDAndLanguageID(templateId, null);
		}

		public static List<CMPage> CMPageGetByCMTemplateIDAndLanguageID(int? templateId, int? languageID)
		{
			List<CMPage> objects;
			string key = cacheKeyPrefix + "GetByCMTemplateIDAndLanguageID" + (templateId.HasValue ? "_" + templateId.Value : "") + (languageID.HasValue ? "_" + languageID.Value : "");

			List<CMPage> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<CMPage>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				objects = new List<CMPage>();
				using (Entities entity = new Entities())
				{
					var query = from page in entity.CMPage
								where page.CMTemplateID == templateId
								select new
								{
									CMSPage = page,
									CMSTitle = (from title in page.CMPageTitle where title.LanguageID == languageID select title.Title).FirstOrDefault()
								};
					foreach (var page in query)
					{
						CMPage pageEntity = page.CMSPage;
						if (pageEntity.OriginalCMPageID.HasValue && String.IsNullOrEmpty(page.CMSTitle))
							pageEntity.CMPageTitleTitle = query.FirstOrDefault(p => p.CMSPage.CMPageID == pageEntity.OriginalCMPageID.Value).CMSTitle;
						else
							pageEntity.CMPageTitleTitle = page.CMSTitle;
						objects.Add(pageEntity);
					}
				}

				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<CMPage> CMPagesGetAllWithCMPageTitle(int languageID)
		{
			List<CMPage> objects;
			string key = cacheKeyPrefix + "GetAllWithTitleByLanguageID" + languageID;

			List<CMPage> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<CMPage>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				objects = new List<CMPage>();
				using (Entities entity = new Entities())
				{
					var query = from page in entity.CMPage
								select new
								{
									CMSPage = page,
									CMSTitle = (from title in page.CMPageTitle where title.LanguageID == languageID select title.Title).FirstOrDefault()
								};
					foreach (var page in query)
					{
						CMPage pageEntity = page.CMSPage;
						pageEntity.CMPageTitleTitle = page.CMSTitle;
						objects.Add(pageEntity);
					}
				}

				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<CMPage> CMPageGetByFileNameAndLanguageID(string fileName, int languageID)
		{
			List<CMPage> objects;
			string key = cacheKeyPrefix + "GetByCMFileNameAndLanguageID_" + fileName + "_" + languageID;

			List<CMPage> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<CMPage>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.CMPage.Include("CMPageTitle").Where(p => p.FileName == fileName && p.CMPageTitle.Any(t => t.LanguageID == languageID)).ToList();
				}

				Cache.Store(key, objects);
			}

			return objects;
		}

		public static List<CMPage> GetAllPagesNeedingApproval(int? languageID = null)
		{
			List<CMPage> objects;
			string key = cacheKeyPrefix + "GetAllPagesNeedingApproval_" + languageID;

			List<CMPage> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<CMPage>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				List<CMPage> newPages;
				using (Entities entity = new Entities())
				{
					objects = entity.CMPage.Include("CMPageTitle").Include("CMPageTitle.Languages").Where(p => p.CMTemplateID.HasValue && (p.NeedsApproval || p.OriginalCMPageID.HasValue || p.EditorDeleted.HasValue)).ToList();
					newPages = entity.CMPage.Where(p => p.OriginalCMPageID.HasValue).ToList();
				}
				
				//Only care about the unapproved version of pages, discard the live versions
				List<CMPage> temp = new List<CMPage>();
				temp.AddRange(objects);
				foreach (CMPage pageEntity in temp)
				{
					objects.RemoveAll(s => s.OriginalCMPageID == pageEntity.CMPageID && s.CultureName == pageEntity.CultureName);
					if (newPages.Exists(s => s.OriginalCMPageID == pageEntity.CMPageID) && objects.Exists(s => s == pageEntity))
					{
						objects.Find(s => s == pageEntity).OriginalCMPageID = pageEntity.CMPageID;
						objects.Find(s => s == pageEntity).CMPageID = newPages.Find(s => s.OriginalCMPageID == pageEntity.CMPageID).CMPageID;
					}
				}

				temp = new List<CMPage>();
				temp.AddRange(objects);
				//Remove null cultures where the default culture exists
				foreach (CMPage pageEntity in temp)
				{
					objects.RemoveAll(s => String.IsNullOrEmpty(s.CultureName) && s.CMPageID == pageEntity.CMPageID && pageEntity.CultureName == SiteLanguages.Settings.DefaultLanguageCulture);
				}
				objects = objects.OrderBy(s => s.CMPageID).ToList();
				Cache.Store(key, objects);
			}

			return objects;
		}

		public static bool PageNeedsApproval(int cmPageID, int? languageID)
		{
			bool needsApproval;
			string key = cacheKeyPrefix + "PageNeedsApproval_" + cmPageID + "_" + languageID;

			bool? temp = null;
			if (Cache.IsEnabled)
				temp = Cache[key] as bool?;

			if (temp.HasValue)
				needsApproval = temp.Value;
			else
			{
				using (Entities entity = new Entities())
				{
					needsApproval = entity.CMS_PageNeedsApproval(cmPageID, languageID).FirstOrDefault().Value;
				}
				
				Cache.Store(key, needsApproval);
			}
			return needsApproval;
		}

		public static bool PagesNeedTranslating(int languageID)
		{
			bool obj;
			string key = cacheKeyPrefix + "GetPagesThatNeedTranslatingByLanguageID_" + languageID;

			bool? tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as bool?;

			if (tmpList.HasValue)
				obj = tmpList.Value;
			else
			{
				using (Entities entity = new Entities())
				{
					var query = entity.CMPage.Where(c => c.CMTemplateID.HasValue && !c.CMPageTitle.Any(t => t.LanguageID == languageID));
					if (languageID == Helpers.GetDefaultLanguageID())
						query = query.Where(c=>String.IsNullOrEmpty(c.Title));
					obj = query.Any();
				}				

				Cache.Store(key, obj);
			}

			return obj;
		}
	}
}