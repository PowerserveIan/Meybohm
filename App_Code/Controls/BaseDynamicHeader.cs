using System.Web.UI;

/// <summary>
/// Sucky hack to allow access to properties of the DynamicHeader control in App_Code files
/// </summary>
public class BaseDynamicHeader : UserControl
{
	/// <summary>
	/// Determines which collection of images to load.  Must match exactly as entered into the backend through the dynamic image collection manager.
	/// </summary>
	public string CollectionName { get; set; }
}