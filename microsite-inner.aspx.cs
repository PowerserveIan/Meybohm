public partial class microsite_inner : BaseCMSPage
{
	protected override void SetCssAndJs()
	{
		if (((BaseMasterPage)Master).NewHomePage.HasValue && ((BaseMasterPage)Master).NewHomePage.Value)
			m_AdditionalCssFiles = uxCSSFiles;
		else
			uxCSSFiles.Visible = false;
	}
}