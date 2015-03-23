using System;
using System.Collections.Generic;
using Classes.ContentManager;
using Classes.Showcase;

public partial class contact_us : BasePage
{
	protected CMMicrosite micrositeEntity;
	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		micrositeEntity = CMSHelpers.GetCurrentRequestCMSMicrosite();
		if (micrositeEntity != null)
			MasterPageFile = "~/microsite.master";
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (micrositeEntity != null)
		{
			uxAugustaPH.Visible = micrositeEntity.Name == "Augusta";
			uxAikenPH.Visible = micrositeEntity.Name == "Aiken";
		}
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Contacts";
		ComponentAdminPage = "contacts/admin-contact.aspx?FilterContactContactTypeID=1";
	}
}