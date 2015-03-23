<%@ WebService Language="C#" Class="ContentManagerWebMethods" %>

using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Classes.ContentManager;

[WebService(Namespace = "http://352media.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class ContentManagerWebMethods : WebService
{
	[WebMethod]
	public string LoadCMSContent(int? cmPageRegionID)
	{
		if (cmPageRegionID.HasValue)
			return BaseCode.Helpers.ReplaceRootWithRelativePath(CMPageRegion.GetByID(cmPageRegionID.Value).Content, HttpContext.Current.Request.UrlReferrer.ToString().ToLower().Replace(BaseCode.Helpers.RootPath.ToLower(), "").Split('/').Length - 1);
		return string.Empty;
	}
}