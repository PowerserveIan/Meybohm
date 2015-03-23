using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.ContentManager;
using Classes.SiteLanguages;
using Settings = Classes.ContentManager.Settings;

public partial class Controls_ContentManager_AdminQuickView : UserControl
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxAdminQuickView.ComponentVersionNumber = Settings.VersionNumber;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			PlaceHolder uxContentArea = (PlaceHolder)uxAdminQuickView.FindControl("uxContentArea");
			Repeater uxTranslationsRepeater = (Repeater)uxContentArea.Controls[0].FindControl("uxTranslationsRepeater");
			if (Settings.EnableApprovals)
			{
				if (CMPage.GetAllPagesNeedingApproval().Count > 0 || SMItem.GetAllSMItemsNeedingApproval(null).Count > 0 || CMPageRegion.CMPageRegionGetByNeedsApproval(true).Count > 0)
					uxContentArea.Controls[0].FindControl("uxApprovalRequiredRow").Visible = true;
			}

			if (CMSubmittedForm.CMSubmittedFormGetByIsProcessed(false).Count > 0)
				uxContentArea.Controls[0].FindControl("uxSubmittedFormsRow").Visible = true;

			if (Settings.EnableMultipleLanguages)
			{
				uxTranslationsRepeater.DataSource = Language.LanguageGetByActive(true);
				uxTranslationsRepeater.DataBind();
			}

			uxContentArea.Controls[0].FindControl("uxNoItemsRequiringAttention").Visible = !(uxContentArea.Controls[0].FindControl("uxApprovalRequiredRow").Visible || uxContentArea.Controls[0].FindControl("uxSubmittedFormsRow").Visible || uxTranslationsRepeater.Items.Count > 0);
		}
	}
}