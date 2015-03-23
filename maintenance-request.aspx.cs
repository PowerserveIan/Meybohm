public partial class maintenance_request : BasePage
{
	public override void SetComponentInformation()
	{
		ComponentName = "Maintenance Requests";
		ComponentAdminPage = "contacts/admin-contact.aspx?FilterContactContactTypeID=3";
	}
}