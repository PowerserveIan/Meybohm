using System.Web.UI;

/// <summary>
/// Sucky hack to allow access to properties of the ContentRegion control in App_Code files
/// </summary>
public class BaseContentRegion : UserControl
{
	private string m_RegionName = "MainRegion";

	/// <summary>
	/// The region name
	/// </summary>
	public string RegionName
	{
		get { return m_RegionName; }
		set { m_RegionName = value; }
	}
}