<%@ WebService Language="C#" Class="ContactsWebMethods" %>

using System;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Classes.Contacts;

[WebService(Namespace = "http://352media.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class ContactsWebMethods : WebService
{
	[WebMethod]
	public void SaveContactForm(int? cmMicrositeID, int contactMethodID, int contactTimeID, int contactTypeID, string email, string firstName, string lastName, string message, string phone, int? showcaseItemID)
	{
		Contact contactEntity = new Contact();
		if (showcaseItemID.HasValue)
		{
			Classes.Showcase.ShowcaseItem showcaseItemEntity = Classes.Showcase.ShowcaseItem.GetByID(showcaseItemID.Value);
			contactEntity.AgentID = showcaseItemEntity.AgentID;
			contactEntity.TeamID = showcaseItemEntity.TeamID;
		}
		contactEntity.CMMicrositeID = cmMicrositeID;
		contactEntity.ContactMethodID = contactMethodID;
		contactEntity.ContactStatusID = (int)ContactStatuses.Unread;
		contactEntity.ContactTimeID = contactTimeID;
		contactEntity.ContactTypeID = contactTypeID;
		contactEntity.Created = DateTime.UtcNow;
		contactEntity.Email = email;
		contactEntity.FirstName = firstName;
		contactEntity.LastName = lastName;
		contactEntity.Message = message;
		contactEntity.Phone = phone;
		contactEntity.ShowcaseItemID = showcaseItemID;
		contactEntity.Save();

		Contact.SendSubmissionEmail(contactEntity, BaseCode.EnumParser.Parse<ContactTypes>(contactTypeID.ToString()));
	}
}