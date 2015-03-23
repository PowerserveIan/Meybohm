using System.Collections.Generic;
using System.Linq;

namespace Classes.ContentManager
{
	public partial class SMItem
	{
		/// <summary>
		/// Value pulled from the Languages table (will be null for a site that is not multilingual)
		/// Only populated by GetAllSMItemsNeedingApproval
		/// </summary>
		public string Culture
		{
			get
			{
				if (this.Language == null)
					return string.Empty;
				return this.Language.Culture;
			}
		}

		/// <summary>
		/// Value pulled from the Languages table (will be null for a site that is not multilingual)
		/// Only populated by GetAllSMItemsNeedingApproval
		/// </summary>
		public string CultureName
		{
			get
			{
				if (this.Language == null)
					return string.Empty;
				return this.Language.CultureName;
			}
		}

		public static List<SMItem> GetAllSMItemsNeedingApproval(int? languageID)
		{
			List<SMItem> objects;
			string key = cacheKeyPrefix + "GetAllSMItemsNeedingApproval_" + languageID;

			List<SMItem> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<SMItem>;

			if (tmpList != null)
				objects = tmpList;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.SMItem.Include("Languages").Where(s => (s.NeedsApproval || s.OriginalSMItemID.HasValue || s.EditorDeleted) && (!languageID.HasValue || s.LanguageID == languageID)).ToList();
				}

				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}