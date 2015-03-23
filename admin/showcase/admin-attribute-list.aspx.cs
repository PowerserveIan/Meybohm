using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.Showcase;

public partial class Admin_AttributeList : Page
{
	protected override void OnInit(System.EventArgs e)
	{
		base.OnInit(e);
		uxSave.Click += uxSave_Click;
	}

	protected override void OnLoad(System.EventArgs e)
	{
		base.OnLoad(e);
		int? showcaseID = ShowcaseHelpers.GetCurrentShowcaseID();
		if (!showcaseID.HasValue)
			Response.Redirect("~/admin/showcase/admin-showcases.aspx");
		if (!IsPostBack)
		{
			List<ShowcaseAttribute> allAttributes = ShowcaseAttribute.ShowcaseAttributeGetByShowcaseID(showcaseID.Value, "Title").Where(s=>!String.IsNullOrEmpty(s.MLSAttributeName)).ToList();
			uxAttributes.DataSource = allAttributes;
			uxAttributes.DataTextField = "Title";
			uxAttributes.DataValueField = "ShowcaseAttributeID";
			uxAttributes.DataBind();

			List<ShowcaseAttribute> importedAttributes = allAttributes.Where(a => a.ImportItemAttribute).ToList();
			foreach (ListItem li in uxAttributes.Items)
			{
				if (importedAttributes.Any(a => a.ShowcaseAttributeID == Convert.ToInt32(li.Value)))
					li.Selected = true;
			}
		}
	}

	void uxSave_Click(object sender, System.EventArgs e)
	{
		if (IsValid)
		{
			List<ShowcaseAttribute> allAttributes = ShowcaseAttribute.ShowcaseAttributeGetByShowcaseID(ShowcaseHelpers.GetCurrentShowcaseID().Value, "Title").Where(s => !String.IsNullOrEmpty(s.MLSAttributeName)).ToList();
			List<ShowcaseAttribute> importedAttributes = allAttributes.Where(a => a.ImportItemAttribute).ToList();
			foreach (ListItem li in uxAttributes.Items)
			{
				ShowcaseAttribute attribute = importedAttributes.Find(npc => npc.ShowcaseAttributeID == Convert.ToInt32(li.Value));
				if (attribute != null)
				{
					if (!li.Selected)
					{
						attribute.ImportItemAttribute = false;
						attribute.Save();
					}
				}
				else
				{
					if (li.Selected)
					{
						attribute = allAttributes.Find(npc => npc.ShowcaseAttributeID == Convert.ToInt32(li.Value));
						attribute.ImportItemAttribute = true;
						attribute.Save();
					}
				}
			}
			uxAfterSavePH.Visible = true;
		}
	}

}