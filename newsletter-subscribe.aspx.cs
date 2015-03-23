public partial class NewsletterSubscribe : BasePage
{
	protected override void SetCssAndJs()
	{
		m_AdditionalCssFiles = uxCSSFiles;
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Newsletter";
		ComponentAdminPage = "newsletters/admin-newsletter.aspx";
	}
}