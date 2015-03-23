using System;
using System.Collections.Generic;
using BaseCode;

namespace Classes.ContentManager
{
	public partial class CMPageTitle
	{
		private string m_Culture = String.Empty;

		public string Culture
		{
			get { return m_Culture; }
			set { m_Culture = value; }
		}

		public static List<CMPageTitle> CMPageTitleGetByCMPageIDAndLanguageID(int cmPageID, int languageID)
		{
			return CMPageTitleGetByCMPageIDAndLanguageID(cmPageID, languageID, true);
		}

		public static List<CMPageTitle> CMPageTitleGetByCMPageIDAndLanguageID(int cmPageID, int languageID, bool sortAscending)
		{
			Filters filterList = new Filters();
			filterList.FilterCMPageTitleCMPageID = cmPageID.ToString();
			filterList.FilterCMPageTitleLanguageID = languageID.ToString();
			return CMPageTitlePage(0, 0, "", "Title", false, filterList);
		}

		public static void CreateCMPageTitlesFromCMPages()
		{
			using (Entities entity = new Entities())
			{
				entity.CMS_CreateCMPageTitlesFromCMPages(Helpers.GetDefaultLanguageID());
			}
			Helpers.PurgeCacheItems("ContentManager");
		}
	}
}