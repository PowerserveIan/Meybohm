/* 
 * Admin_AdminShowcaseItemEditStatus.cs
 * Powerserve 2013
 * 
 * */

#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Showcase;
using Classes.StateAndCountry;
using Settings = Classes.Showcase.Settings;

#endregion

public partial class Admin_AdminShowcaseItemEditStatus : BaseEditPage
{

    #region Fields

    public ShowcaseItem ShowcaseItemEntity { get; set; }
    public int ShowcaseItemId = 0;

    #endregion

    #region Methods

    /// <summary>
    /// Handles the basic initialization of the status edit page.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        m_Header = uxHeader;
        m_SavePanel = uxPanel;
        m_ButtonContainer = uxButtonContainer;
        m_LinkToListingPage = "admin-showcase-item.aspx";
        
        int temp;
        if (!String.IsNullOrEmpty(Request.QueryString["ShowcaseID"]) && Int32.TryParse(Request.QueryString["ShowcaseID"], out temp))
        {
            ShowcaseHelpers.SetUsersCurrentShowcaseID(temp);
        }

        if (!ShowcaseHelpers.GetCurrentShowcaseID().HasValue && (!ShowcaseHelpers.IsShowcaseAdmin() || EntityId == 0))
        {
            Response.Redirect("~/admin/showcase/admin-showcases.aspx");
        }
        else if (!ShowcaseHelpers.GetCurrentShowcaseID().HasValue && EntityId > 0)
        {
            ShowcaseItemEntity = ShowcaseItem.GetByID(EntityId);
            if (ShowcaseItemEntity == null || (!ShowcaseHelpers.IsShowcaseAdmin() && !ShowcaseUser.ShowcaseUserGetByShowcaseID(ShowcaseItemEntity.ShowcaseID).Exists(s => s.UserID == Helpers.GetCurrentUserID())))
                Response.Redirect(m_LinkToListingPage + ReturnQueryString);
            ShowcaseHelpers.SetUsersCurrentShowcaseID(ShowcaseItemEntity.ShowcaseID);
        }

        m_ClassName = (Settings.MultipleShowcases && ShowcaseHelpers.UserCanManageOtherShowcases() ? Showcases.GetByID(ShowcaseHelpers.GetCurrentShowcaseID().Value).Title + " " : "") + "Prop. Status";
        base.OnInit(e);
        m_SaveAndAddNewButton.Visible = m_AddNewButton.Visible = false;
        m_ValidationSummary.DisplayMode = ValidationSummaryDisplayMode.BulletList;

        m_CancelButton.PostBackUrl = "~/admin/showcase/admin-showcase-item-edit.aspx?id=" + EntityId;
        m_CancelButton.Text = "return to " + (Settings.MultipleShowcases && ShowcaseHelpers.UserCanManageOtherShowcases() ? Showcases.GetByID(ShowcaseHelpers.GetCurrentShowcaseID().Value).Title + " " : "") + " property details";
    }

    /// <summary>
    /// Handles the on load function of the page and sets up URLs.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        uxEditMediaCollection.NavigateUrl = "~/admin/showcase/admin-media-collection-edit.aspx?id=0&FilterMediaCollectionShowcaseItemID=" + EntityId + "&isFineProperty=1";
    }

    /// <summary>
    /// Page load functionality to handle information updating/retrieval
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        var showcaseItemFinePropertyInformation = ShowcaseItemFinePropertyInformation.Get(EntityId);
        ShowcaseItemEntity = ShowcaseItem.GetByID(EntityId);
        
        if (!IsPostBack)
        {
            uxMLSID.Text = ShowcaseItemEntity.MlsID.ToString();
            uxTitle.Text = ShowcaseItemEntity.Title;
            uxIsFine.SelectedValue = showcaseItemFinePropertyInformation.IsFine.ToString();
            uxIsFineFeatured.SelectedValue = showcaseItemFinePropertyInformation.IsFineFeatured.ToString();
            uxDescription.Text = showcaseItemFinePropertyInformation.Description;
            uxFeatures.Text = showcaseItemFinePropertyInformation.Features;

            if (!string.IsNullOrEmpty(showcaseItemFinePropertyInformation.Tags))
            {
                uxEquestrian.Checked = showcaseItemFinePropertyInformation.Tags.Contains("|Equestrian|");
                uxEstate.Checked = showcaseItemFinePropertyInformation.Tags.Contains("|Estate|");
                uxWaterfront.Checked = showcaseItemFinePropertyInformation.Tags.Contains("|Waterfront|");
                uxGolf.Checked = showcaseItemFinePropertyInformation.Tags.Contains("|Golf|");
                uxHistoric.Checked = showcaseItemFinePropertyInformation.Tags.Contains("|Historic|");
                uxAcreage.Checked = showcaseItemFinePropertyInformation.Tags.Contains("|Acreage|");
            }
        }
        else
        {
            showcaseItemFinePropertyInformation.IsFine = Convert.ToBoolean(uxIsFine.SelectedValue);
            showcaseItemFinePropertyInformation.IsFineFeatured = Convert.ToBoolean(uxIsFineFeatured.SelectedValue);
            showcaseItemFinePropertyInformation.Description = uxDescription.Text;
            showcaseItemFinePropertyInformation.Features = uxFeatures.Text;
            showcaseItemFinePropertyInformation.ShowcaseItemId = EntityId;

            StringBuilder showcaseItemTags = new StringBuilder();
            showcaseItemTags.Append(uxEquestrian.Checked ? "|Equestrian|" : "");
            showcaseItemTags.Append(uxEstate.Checked ? "|Estate|" : "");
            showcaseItemTags.Append(uxWaterfront.Checked ? "|Waterfront|" : "");
            showcaseItemTags.Append(uxGolf.Checked ? "|Golf|" : "");
            showcaseItemTags.Append(uxHistoric.Checked ? "|Historic|" : "");
            showcaseItemTags.Append(uxAcreage.Checked ? "|Acreage|" : "");

            showcaseItemFinePropertyInformation.Tags = showcaseItemTags.ToString();

            showcaseItemFinePropertyInformation.Save();
        }
    }

    #endregion 
}