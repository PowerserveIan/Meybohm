namespace Classes.ContentManager
{
	public partial class SMItemUser
	{
		public static void DeleteAll(int? micrositeID, int? languageID)
		{
			using (Entities entity = new Entities())
			{
				entity.CMS_DeleteAllSMItemUser(micrositeID, languageID);
			}
		}
	}
}