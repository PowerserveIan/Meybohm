using System.Web.UI.WebControls;

public partial class Controls_BaseControls_AdminBar : System.Web.UI.UserControl
{
	public PlaceHolder BottomBarPH { get { return uxBottomBarPH; } }
	public LinkButton ClearCacheLink { get { return uxClearCaches; } }
	public HyperLink EditPagePropertiesLink { get { return uxEditPageProperties; } }
	public HyperLink ComponentAdminLink { get { return uxComponentAdmin; } }
	public HyperLink LogoutLink { get { return uxLogoutBB;}}
	public PlaceHolder CMSPlaceholder { get { return uxCMSPH; } }
	public Repeater PageRegionsRepeater { get { return uxPageRegions; } }
}