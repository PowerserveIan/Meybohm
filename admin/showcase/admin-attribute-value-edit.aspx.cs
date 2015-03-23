using System;
using BaseCode;
using Classes.Showcase;

public partial class Admin_AdminShowcaseAttributeValueEdit : BaseEditPage
{
	protected int ShowcaseAttributeId
	{
		get
		{
			int tempID;
			if (Request.QueryString["FilterShowcaseAttributeValueShowcaseAttributeID"] != null)
				if (Int32.TryParse(Request.QueryString["FilterShowcaseAttributeValueShowcaseAttributeID"], out tempID))
					return tempID;

			return 0;
		}
	}

	public ShowcaseAttributeValue ShowcaseAttributeValueEntity { get; set; }

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-attribute-value.aspx";
		m_ClassName = "Attribute Value";
		m_CustomBreadCrumbsPH = uxCustomBreadCrumbsPH;
		base.OnInit(e);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		ShowcaseAttribute attributeEntity = null;
		if (!IsPostBack)
		{
			if (EntityId > 0)
			{
				ShowcaseAttributeValueEntity = ShowcaseAttributeValue.GetByID(EntityId);
				if (ShowcaseAttributeValueEntity == null)
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);

				attributeEntity = ShowcaseAttribute.GetByID(ShowcaseAttributeValueEntity.ShowcaseAttributeID);
				if (attributeEntity == null || (!ShowcaseHelpers.IsShowcaseAdmin() && !ShowcaseUser.ShowcaseUserGetByShowcaseID(attributeEntity.ShowcaseID).Exists(s => s.UserID == Helpers.GetCurrentUserID())))
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);
				LoadData();
			}
			else
				NewRecord = true;

			if (attributeEntity == null)
				attributeEntity = ShowcaseAttribute.GetByID(EntityId > 0 ? ShowcaseAttributeValueEntity.ShowcaseAttributeID : ShowcaseAttributeId);

			if (!String.IsNullOrEmpty(attributeEntity.MLSAttributeName) && NewRecord)
				Response.Redirect(m_LinkToListingPage + ReturnQueryString);
			bool isDistanceAttribute = attributeEntity.ShowcaseFilterID == (int)FilterTypes.Distance ||
									   attributeEntity.ShowcaseFilterID == (int)FilterTypes.DistanceRange;

			uxDistanceDescription.Visible = uxValueRangeVal.Visible = isDistanceAttribute;
			uxTextDescription.Visible = uxValueRegexVal.Visible = !isDistanceAttribute;

			if (isDistanceAttribute)
				uxValue.TextMode = System.Web.UI.WebControls.TextBoxMode.SingleLine;

			uxLinkToValueManager.NavigateUrl = m_LinkToListingPage + ReturnQueryString;
			uxLinkToValueManager.Text = @"<b>" + attributeEntity.Title + @"</b> Value Manager";

			if (Settings.MultipleShowcases && ShowcaseHelpers.UserCanManageOtherShowcases())
				uxShowcaseName.Text = @"<b>" + Showcases.GetByID(attributeEntity.ShowcaseID).Title + @"</b> ";
			else
				uxShowcaseName.Visible = false;

			m_SaveAndAddNewButton.Visible = m_AddNewButton.Visible = String.IsNullOrEmpty(attributeEntity.MLSAttributeName);
		}
	}

	protected override void Save()
	{
		if (IsValid)
		{
			ShowcaseAttributeValueEntity = EntityId > 0 ? ShowcaseAttributeValue.GetByID(EntityId) : new ShowcaseAttributeValue();
			if (NewRecord)
			{
				ShowcaseAttributeValueEntity.DisplayOrder = (short)(Helpers.GetMaxDisplayOrder("ShowcaseAttributeValue", "ShowcaseAttributeValueID", "ShowcaseAttributeID", ShowcaseAttributeId) + 1);
				ShowcaseAttributeValueEntity.ShowcaseAttributeID = ShowcaseAttributeId;
			}
			ShowcaseAttributeValueEntity.DisplayInFilters = uxDisplayInFilters.Checked;
			ShowcaseAttributeValueEntity.Value = uxValue.Text;
			ShowcaseAttributeValueEntity.Save();
			EntityId = ShowcaseAttributeValueEntity.ShowcaseAttributeValueID;
			m_ClassTitle = ShowcaseAttributeValueEntity.Value;
		}
	}

	protected override void LoadData()
	{
		uxDisplayInFilters.Checked = ShowcaseAttributeValueEntity.DisplayInFilters;
		uxValue.Text = ShowcaseAttributeValueEntity.Value;
	}
}