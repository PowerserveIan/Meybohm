using System.Collections.Generic;
using System.Linq;
using Classes.SEOComponent;

namespace Classes.Newsletters
{
	public partial class Newsletter
	{
		protected override void DeleteSEO()
		{
			SEOData.DeleteSEOData("~/", "newsletter-details.aspx", "Id", NewsletterID);
		}

		public static List<Newsletter> GetNumArticlesQuickView(int numArticles)
		{
			List<Newsletter> objects;
			string key = cacheKeyPrefix + "GetNumArticlesQuickView_" + numArticles;

			List<Newsletter> tmpList = null;

			if (Cache.IsEnabled)
				tmpList = Cache[key] as List<Newsletter>;

			if (tmpList != null)
				objects = tmpList.Select(entity => new Newsletter(entity)).ToList();
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.Newsletter.Where(n => n.Active && n.Featured && !n.Deleted && !n.Archived).OrderByDescending(n => n.DisplayDate).Take(numArticles).ToList();
				}
				Cache.Store(key, objects);
			}

			return objects;
		}
	}
}