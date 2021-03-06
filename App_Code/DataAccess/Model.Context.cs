﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.EntityClient;
using System.Data.Objects;

public partial class Entities : DbContext
{
	public Entities()
		: base(CreateConnectionString())
	{
		Initialize();
	}

	public Entities(string connectionString)
		: base(connectionString)
	{
		Initialize();
	}

	public Entities(EntityConnection connection)
		: base(connection,false)
	{
		Initialize();
	}

	private void Initialize()
	{
        this.Configuration.LazyLoadingEnabled = false;
		this.Configuration.ProxyCreationEnabled = false;
	}

	//Custom 352 Code
	private static string CreateConnectionString()
	{
		EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
		entityBuilder.Provider = "System.Data.SqlClient";
		entityBuilder.ProviderConnectionString = ConfigurationManager.ConnectionStrings[BaseCode.Globals.Settings.DefaultConnectionStringName].ConnectionString;
		entityBuilder.Metadata = "res://*/App_Code.DataAccess.Entity.csdl|res://*/App_Code.DataAccess.Entity.ssdl|res://*/App_Code.DataAccess.Entity.msl";
		return entityBuilder.ToString();
	}

	protected override void OnModelCreating(DbModelBuilder modelBuilder)
	{
		throw new UnintentionalCodeFirstException();
	}

	public DbSet<Classes.StateAndCountry.Address> Address { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.Application> Application { get; set; }
	public DbSet<Classes.MLS.Builder> Builder { get; set; }
	public DbSet<Classes.MLS.BuilderMicrosite> BuilderMicrosite { get; set; }
	public DbSet<Classes.Showcase.ClickType> ClickType { get; set; }
	public DbSet<Classes.ContentManager.CMMicrosite> CMMicrosite { get; set; }
	public DbSet<Classes.ContentManager.CMMicrositeUser> CMMicrositeUser { get; set; }
	public DbSet<Classes.ContentManager.CMPage> CMPage { get; set; }
	public DbSet<Classes.ContentManager.CMPageRegion> CMPageRegion { get; set; }
	public DbSet<Classes.ContentManager.CMPageRole> CMPageRole { get; set; }
	public DbSet<Classes.ContentManager.CMPageTitle> CMPageTitle { get; set; }
	public DbSet<Classes.ContentManager.CMRegion> CMRegion { get; set; }
	public DbSet<Classes.ContentManager.CMSubmittedForm> CMSubmittedForm { get; set; }
	public DbSet<Classes.ContentManager.CMTemplate> CMTemplate { get; set; }
	public DbSet<Classes.Contacts.Contact> Contact { get; set; }
	public DbSet<Classes.Contacts.ContactMethod> ContactMethod { get; set; }
	public DbSet<Classes.Contacts.ContactStatus> ContactStatus { get; set; }
	public DbSet<Classes.Contacts.ContactTime> ContactTime { get; set; }
	public DbSet<Classes.Contacts.ContactType> ContactType { get; set; }
	public DbSet<Classes.StateAndCountry.Country> Country { get; set; }
	public DbSet<Classes.DynamicHeader.DynamicCollection> DynamicCollection { get; set; }
	public DbSet<Classes.DynamicHeader.DynamicImage> DynamicImage { get; set; }
	public DbSet<Classes.DynamicHeader.DynamicImageCollection> DynamicImageCollection { get; set; }
	public DbSet<Classes.Showcase.Filter> Filter { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.JobTitle> JobTitle { get; set; }
	public DbSet<Classes.SiteLanguages.Language> Language { get; set; }
	public DbSet<Classes.Newsletters.MailingList> MailingList { get; set; }
	public DbSet<Classes.Newsletters.MailingListSubscriber> MailingListSubscriber { get; set; }
	public DbSet<Classes.Newsletters.Mailout> Mailout { get; set; }
	public DbSet<Classes.Newsletters.MailoutMailingList> MailoutMailingList { get; set; }
	public DbSet<Classes.Showcase.Media> Media { get; set; }
	public DbSet<Classes.Showcase.MediaCollection> MediaCollection { get; set; }
	public DbSet<Classes.Showcase.MediaType> MediaType { get; set; }
	public DbSet<Classes.MLS.Neighborhood> Neighborhood { get; set; }
	public DbSet<Classes.MLS.NeighborhoodBuilder> NeighborhoodBuilder { get; set; }
	public DbSet<Classes.Newsletters.Newsletter> Newsletter { get; set; }
	public DbSet<Classes.Newsletters.NewsletterAction> NewsletterAction { get; set; }
	public DbSet<Classes.Newsletters.NewsletterActionType> NewsletterActionType { get; set; }
	public DbSet<Classes.Newsletters.NewsletterDesign> NewsletterDesign { get; set; }
	public DbSet<Classes.Newsletters.NewsletterFormat> NewsletterFormat { get; set; }
	public DbSet<Classes.Media352_NewsPress.NewsPress> NewsPress { get; set; }
	public DbSet<Classes.Media352_NewsPress.NewsPressCategory> NewsPressCategory { get; set; }
	public DbSet<Classes.Media352_NewsPress.NewsPressNewsPressCategory> NewsPressNewsPressCategory { get; set; }
	public DbSet<Classes.MLS.Office> Office { get; set; }
	public DbSet<Classes.Showcase.PropertyChangeLog> PropertyChangeLog { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.Role> Role { get; set; }
	public DbSet<Classes.Showcase.SavedSearch> SavedSearch { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.SecurityQuestion> SecurityQuestion { get; set; }
	public DbSet<Classes.SEOComponent.SEOData> SEOData { get; set; }
	public DbSet<Classes.Showcase.ShowcaseAttribute> ShowcaseAttribute { get; set; }
	public DbSet<Classes.Showcase.ShowcaseAttributeHeader> ShowcaseAttributeHeader { get; set; }
	public DbSet<Classes.Showcase.ShowcaseAttributeValue> ShowcaseAttributeValue { get; set; }
	public DbSet<Classes.Showcase.ShowcaseItem> ShowcaseItem { get; set; }
	public DbSet<Classes.Showcase.ShowcaseItemAttributeValue> ShowcaseItemAttributeValue { get; set; }
	public DbSet<Classes.Showcase.ShowcaseItemMetric> ShowcaseItemMetric { get; set; }
	public DbSet<Classes.Showcase.ShowcaseItemRental> ShowcaseItemRental { get; set; }
	public DbSet<Classes.Showcase.Showcases> Showcases { get; set; }
	public DbSet<Classes.Showcase.ShowcaseSiteSettings> ShowcaseSiteSettings { get; set; }
	public DbSet<Classes.Showcase.ShowcaseUser> ShowcaseUser { get; set; }
	public DbSet<Classes.ConfigurationSettings.SiteComponent> SiteComponent { get; set; }
	public DbSet<Classes.ConfigurationSettings.SiteSettings> SiteSettings { get; set; }
	public DbSet<Classes.ConfigurationSettings.SiteSettingsDataType> SiteSettingsDataType { get; set; }
	public DbSet<Classes.ContentManager.SMItem> SMItem { get; set; }
	public DbSet<Classes.ContentManager.SMItemUser> SMItemUser { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.StaffType> StaffType { get; set; }
	public DbSet<Classes.StateAndCountry.State> State { get; set; }
	public DbSet<Classes.Newsletters.Subscriber> Subscriber { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.Team> Team { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.User> User { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.UserInfo> UserInfo { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.UserLanguageSpoken> UserLanguageSpoken { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.UserOffice> UserOffice { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.UserRole> UserRole { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.UserTeam> UserTeam { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.UserTestimonial> UserTestimonial { get; set; }
	public DbSet<Classes.WhatsNearBy.WhatsNearByCategory> WhatsNearByCategory { get; set; }
	public DbSet<Classes.WhatsNearBy.WhatsNearByLocation> WhatsNearByLocation { get; set; }
	public DbSet<Classes.WhatsNearBy.WhatsNearByLocationCategory> WhatsNearByLocationCategory { get; set; }
	public DbSet<Classes.Videos.Video> Video { get; set; }
	public DbSet<Classes.Showcase.OpenHouse> OpenHouse { get; set; }
	public DbSet<Classes.NewHomes.SoldHome> SoldHome { get; set; }
	public DbSet<Classes.Showcase.ShowcaseItemMetricHistorical> ShowcaseItemMetricHistorical { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.Designation> Designation { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.UserDesignation> UserDesignation { get; set; }
	public DbSet<Classes.Rets.RetsStatus> RetsStatus { get; set; }
	public DbSet<Classes.Rets.RetsTask> RetsTask { get; set; }
	public DbSet<Classes.Rets.RetsTaskStatus> RetsTaskStatus { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.UserOpenAuthProvider> UserOpenAuthProvider { get; set; }
	public DbSet<Classes.Rets.RetsUpdatePageTracker> RetsUpdatePageTracker { get; set; }
	public DbSet<Classes.Media352_MembershipProvider.TeamLanguageSpoken> TeamLanguageSpoken { get; set; }
	public DbSet<Classes.Redirects.Redirect> Redirect { get; set; }
	public DbSet<Classes.Showcase.PropertyStatisticsEmailLog> PropertyStatisticsEmailLog { get; set; }
	public DbSet<Classes.Showcase.ShowcaseItemFinePropertyInformation> ShowcaseItemFinePropertyInformation { get; set; }

	public virtual int CMS_CreateCMPageTitlesFromCMPages(Nullable<int> languageID)
	{
		var languageIDParameter = languageID.HasValue ?
			new ObjectParameter("LanguageID", languageID) :
			new ObjectParameter("LanguageID", typeof(int));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CMS_CreateCMPageTitlesFromCMPages", languageIDParameter);
	}

	public virtual int CMS_CreatePageForMicrosite(Nullable<int> cMPageID, Nullable<bool> update)
	{
		var cMPageIDParameter = cMPageID.HasValue ?
			new ObjectParameter("CMPageID", cMPageID) :
			new ObjectParameter("CMPageID", typeof(int));
		var updateParameter = update.HasValue ?
			new ObjectParameter("Update", update) :
			new ObjectParameter("Update", typeof(bool));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CMS_CreatePageForMicrosite", cMPageIDParameter, updateParameter);
	}

	public virtual int CMS_DeleteAllSMItemUser(Nullable<int> micrositeID, Nullable<int> languageID)
	{
		var micrositeIDParameter = micrositeID.HasValue ?
			new ObjectParameter("MicrositeID", micrositeID) :
			new ObjectParameter("MicrositeID", typeof(int));
		var languageIDParameter = languageID.HasValue ?
			new ObjectParameter("LanguageID", languageID) :
			new ObjectParameter("LanguageID", typeof(int));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CMS_DeleteAllSMItemUser", micrositeIDParameter, languageIDParameter);
	}

	public virtual ObjectResult<Nullable<bool>> CMS_IsMenuItemParentByPageRoles(Nullable<int> sMItemID, Nullable<int> userID)
	{
		var sMItemIDParameter = sMItemID.HasValue ?
			new ObjectParameter("SMItemID", sMItemID) :
			new ObjectParameter("SMItemID", typeof(int));
		var userIDParameter = userID.HasValue ?
			new ObjectParameter("UserID", userID) :
			new ObjectParameter("UserID", typeof(int));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("CMS_IsMenuItemParentByPageRoles", sMItemIDParameter, userIDParameter);
	}

	public virtual ObjectResult<Classes.ContentManager.CMPageRegion> CMS_LoadContentRegion(string filterCMPageRegionCMPageID, string filterCMPageRegionCMRegionID, string filterCMPageRegionLanguageID, string filterCMPageRegionUserID, string filterCMPageRegionCreated, Nullable<bool> filterCMPageRegionNeedsApproval, Nullable<int> defaultLanguageID)
	{
		var filterCMPageRegionCMPageIDParameter = filterCMPageRegionCMPageID != null ?
			new ObjectParameter("FilterCMPageRegionCMPageID", filterCMPageRegionCMPageID) :
			new ObjectParameter("FilterCMPageRegionCMPageID", typeof(string));
		var filterCMPageRegionCMRegionIDParameter = filterCMPageRegionCMRegionID != null ?
			new ObjectParameter("FilterCMPageRegionCMRegionID", filterCMPageRegionCMRegionID) :
			new ObjectParameter("FilterCMPageRegionCMRegionID", typeof(string));
		var filterCMPageRegionLanguageIDParameter = filterCMPageRegionLanguageID != null ?
			new ObjectParameter("FilterCMPageRegionLanguageID", filterCMPageRegionLanguageID) :
			new ObjectParameter("FilterCMPageRegionLanguageID", typeof(string));
		var filterCMPageRegionUserIDParameter = filterCMPageRegionUserID != null ?
			new ObjectParameter("FilterCMPageRegionUserID", filterCMPageRegionUserID) :
			new ObjectParameter("FilterCMPageRegionUserID", typeof(string));
		var filterCMPageRegionCreatedParameter = filterCMPageRegionCreated != null ?
			new ObjectParameter("FilterCMPageRegionCreated", filterCMPageRegionCreated) :
			new ObjectParameter("FilterCMPageRegionCreated", typeof(string));
		var filterCMPageRegionNeedsApprovalParameter = filterCMPageRegionNeedsApproval.HasValue ?
			new ObjectParameter("FilterCMPageRegionNeedsApproval", filterCMPageRegionNeedsApproval) :
			new ObjectParameter("FilterCMPageRegionNeedsApproval", typeof(bool));
		var defaultLanguageIDParameter = defaultLanguageID.HasValue ?
			new ObjectParameter("DefaultLanguageID", defaultLanguageID) :
			new ObjectParameter("DefaultLanguageID", typeof(int));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Classes.ContentManager.CMPageRegion>("CMS_LoadContentRegion", filterCMPageRegionCMPageIDParameter, filterCMPageRegionCMRegionIDParameter, filterCMPageRegionLanguageIDParameter, filterCMPageRegionUserIDParameter, filterCMPageRegionCreatedParameter, filterCMPageRegionNeedsApprovalParameter, defaultLanguageIDParameter);
	}

	public virtual ObjectResult<Classes.ContentManager.CMPageRegion> CMS_LoadContentRegion(string filterCMPageRegionCMPageID, string filterCMPageRegionCMRegionID, string filterCMPageRegionLanguageID, string filterCMPageRegionUserID, string filterCMPageRegionCreated, Nullable<bool> filterCMPageRegionNeedsApproval, Nullable<int> defaultLanguageID, MergeOption mergeOption)
	{
		var filterCMPageRegionCMPageIDParameter = filterCMPageRegionCMPageID != null ?
			new ObjectParameter("FilterCMPageRegionCMPageID", filterCMPageRegionCMPageID) :
			new ObjectParameter("FilterCMPageRegionCMPageID", typeof(string));
		var filterCMPageRegionCMRegionIDParameter = filterCMPageRegionCMRegionID != null ?
			new ObjectParameter("FilterCMPageRegionCMRegionID", filterCMPageRegionCMRegionID) :
			new ObjectParameter("FilterCMPageRegionCMRegionID", typeof(string));
		var filterCMPageRegionLanguageIDParameter = filterCMPageRegionLanguageID != null ?
			new ObjectParameter("FilterCMPageRegionLanguageID", filterCMPageRegionLanguageID) :
			new ObjectParameter("FilterCMPageRegionLanguageID", typeof(string));
		var filterCMPageRegionUserIDParameter = filterCMPageRegionUserID != null ?
			new ObjectParameter("FilterCMPageRegionUserID", filterCMPageRegionUserID) :
			new ObjectParameter("FilterCMPageRegionUserID", typeof(string));
		var filterCMPageRegionCreatedParameter = filterCMPageRegionCreated != null ?
			new ObjectParameter("FilterCMPageRegionCreated", filterCMPageRegionCreated) :
			new ObjectParameter("FilterCMPageRegionCreated", typeof(string));
		var filterCMPageRegionNeedsApprovalParameter = filterCMPageRegionNeedsApproval.HasValue ?
			new ObjectParameter("FilterCMPageRegionNeedsApproval", filterCMPageRegionNeedsApproval) :
			new ObjectParameter("FilterCMPageRegionNeedsApproval", typeof(bool));
		var defaultLanguageIDParameter = defaultLanguageID.HasValue ?
			new ObjectParameter("DefaultLanguageID", defaultLanguageID) :
			new ObjectParameter("DefaultLanguageID", typeof(int));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Classes.ContentManager.CMPageRegion>("CMS_LoadContentRegion", mergeOption, filterCMPageRegionCMPageIDParameter, filterCMPageRegionCMRegionIDParameter, filterCMPageRegionLanguageIDParameter, filterCMPageRegionUserIDParameter, filterCMPageRegionCreatedParameter, filterCMPageRegionNeedsApprovalParameter, defaultLanguageIDParameter);
	}

	public virtual ObjectResult<Nullable<bool>> CMS_PageNeedsApproval(Nullable<int> cMPageID, Nullable<int> languageID)
	{
		var cMPageIDParameter = cMPageID.HasValue ?
			new ObjectParameter("CMPageID", cMPageID) :
			new ObjectParameter("CMPageID", typeof(int));
		var languageIDParameter = languageID.HasValue ?
			new ObjectParameter("LanguageID", languageID) :
			new ObjectParameter("LanguageID", typeof(int));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("CMS_PageNeedsApproval", cMPageIDParameter, languageIDParameter);
	}

	public virtual int CMS_PopulateNewMicrosite(Nullable<int> cMMicrositeID)
	{
		var cMMicrositeIDParameter = cMMicrositeID.HasValue ?
			new ObjectParameter("CMMicrositeID", cMMicrositeID) :
			new ObjectParameter("CMMicrositeID", typeof(int));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CMS_PopulateNewMicrosite", cMMicrositeIDParameter);
	}

	public virtual int CMS_UpdateMicrositesWithDefaultContent(string fileName)
	{
		var fileNameParameter = fileName != null ?
			new ObjectParameter("FileName", fileName) :
			new ObjectParameter("FileName", typeof(string));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CMS_UpdateMicrositesWithDefaultContent", fileNameParameter);
	}

	public virtual int CMS_UpdateSMItemsOnParentItemDelete(Nullable<int> sMItemID, Nullable<bool> editorDelete)
	{
		var sMItemIDParameter = sMItemID.HasValue ?
			new ObjectParameter("SMItemID", sMItemID) :
			new ObjectParameter("SMItemID", typeof(int));
		var editorDeleteParameter = editorDelete.HasValue ?
			new ObjectParameter("EditorDelete", editorDelete) :
			new ObjectParameter("EditorDelete", typeof(bool));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CMS_UpdateSMItemsOnParentItemDelete", sMItemIDParameter, editorDeleteParameter);
	}

	public virtual ObjectResult<Membership_CustomerListForCSV> Membership_GetCustomerListForCSV()
	{
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Membership_CustomerListForCSV>("Membership_GetCustomerListForCSV");
	}

	public virtual ObjectResult<MailingListSubscribersWithEmails> Newsletter_GetMailingListSubscribersWithEmailsByListOfMailingListIDs(string mailingListIDs, Nullable<int> existingMailoutId)
	{
		var mailingListIDsParameter = mailingListIDs != null ?
			new ObjectParameter("MailingListIDs", mailingListIDs) :
			new ObjectParameter("MailingListIDs", typeof(string));
		var existingMailoutIdParameter = existingMailoutId.HasValue ?
			new ObjectParameter("ExistingMailoutId", existingMailoutId) :
			new ObjectParameter("ExistingMailoutId", typeof(int));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MailingListSubscribersWithEmails>("Newsletter_GetMailingListSubscribersWithEmailsByListOfMailingListIDs", mailingListIDsParameter, existingMailoutIdParameter);
	}

	public virtual int SEOComponent_DeleteDataMicrositeOrFolder(string folder)
	{
		var folderParameter = folder != null ?
			new ObjectParameter("Folder", folder) :
			new ObjectParameter("Folder", typeof(string));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SEOComponent_DeleteDataMicrositeOrFolder", folderParameter);
	}

	public virtual ObjectResult<Classes.SEOComponent.SEOData> SEOComponent_SitePageSEOSetupForOutput(string uRLPath, string queryItems)
	{
		var uRLPathParameter = uRLPath != null ?
			new ObjectParameter("URLPath", uRLPath) :
			new ObjectParameter("URLPath", typeof(string));
		var queryItemsParameter = queryItems != null ?
			new ObjectParameter("QueryItems", queryItems) :
			new ObjectParameter("QueryItems", typeof(string));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Classes.SEOComponent.SEOData>("SEOComponent_SitePageSEOSetupForOutput", uRLPathParameter, queryItemsParameter);
	}

	public virtual ObjectResult<Classes.SEOComponent.SEOData> SEOComponent_SitePageSEOSetupForOutput(string uRLPath, string queryItems, MergeOption mergeOption)
	{
		var uRLPathParameter = uRLPath != null ?
			new ObjectParameter("URLPath", uRLPath) :
			new ObjectParameter("URLPath", typeof(string));
		var queryItemsParameter = queryItems != null ?
			new ObjectParameter("QueryItems", queryItems) :
			new ObjectParameter("QueryItems", typeof(string));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Classes.SEOComponent.SEOData>("SEOComponent_SitePageSEOSetupForOutput", mergeOption, uRLPathParameter, queryItemsParameter);
	}

	public virtual ObjectResult<ShowcaseItemForJSON> Showcase_GetFilteredShowcaseItemIDs(Nullable<int> pageSize, Nullable<int> pageNumber, string filter, Nullable<bool> ascending, string orderBy, string filterShowcaseItemActive, string filterShowcaseItemShowcaseID, string filterShowcaseItemNeighborhoodID, string filterShowcaseItemAgentID, Nullable<decimal> addressLat, Nullable<decimal> addressLong, Nullable<int> minDistance, Nullable<int> maxDistance, Nullable<bool> openHouse, string searchText, Nullable<bool> enableFilters, Nullable<bool> newHomesOnly, Nullable<bool> showLotsLand, string showcaseItemIDsToFilterThrough)
	{
		var pageSizeParameter = pageSize.HasValue ?
			new ObjectParameter("PageSize", pageSize) :
			new ObjectParameter("PageSize", typeof(int));
		var pageNumberParameter = pageNumber.HasValue ?
			new ObjectParameter("PageNumber", pageNumber) :
			new ObjectParameter("PageNumber", typeof(int));
		var filterParameter = filter != null ?
			new ObjectParameter("Filter", filter) :
			new ObjectParameter("Filter", typeof(string));
		var ascendingParameter = ascending.HasValue ?
			new ObjectParameter("Ascending", ascending) :
			new ObjectParameter("Ascending", typeof(bool));
		var orderByParameter = orderBy != null ?
			new ObjectParameter("OrderBy", orderBy) :
			new ObjectParameter("OrderBy", typeof(string));
		var filterShowcaseItemActiveParameter = filterShowcaseItemActive != null ?
			new ObjectParameter("FilterShowcaseItemActive", filterShowcaseItemActive) :
			new ObjectParameter("FilterShowcaseItemActive", typeof(string));
		var filterShowcaseItemShowcaseIDParameter = filterShowcaseItemShowcaseID != null ?
			new ObjectParameter("FilterShowcaseItemShowcaseID", filterShowcaseItemShowcaseID) :
			new ObjectParameter("FilterShowcaseItemShowcaseID", typeof(string));
		var filterShowcaseItemNeighborhoodIDParameter = filterShowcaseItemNeighborhoodID != null ?
			new ObjectParameter("FilterShowcaseItemNeighborhoodID", filterShowcaseItemNeighborhoodID) :
			new ObjectParameter("FilterShowcaseItemNeighborhoodID", typeof(string));
		var filterShowcaseItemAgentIDParameter = filterShowcaseItemAgentID != null ?
			new ObjectParameter("FilterShowcaseItemAgentID", filterShowcaseItemAgentID) :
			new ObjectParameter("FilterShowcaseItemAgentID", typeof(string));
		var addressLatParameter = addressLat.HasValue ?
			new ObjectParameter("AddressLat", addressLat) :
			new ObjectParameter("AddressLat", typeof(decimal));
		var addressLongParameter = addressLong.HasValue ?
			new ObjectParameter("AddressLong", addressLong) :
			new ObjectParameter("AddressLong", typeof(decimal));
		var minDistanceParameter = minDistance.HasValue ?
			new ObjectParameter("MinDistance", minDistance) :
			new ObjectParameter("MinDistance", typeof(int));
		var maxDistanceParameter = maxDistance.HasValue ?
			new ObjectParameter("MaxDistance", maxDistance) :
			new ObjectParameter("MaxDistance", typeof(int));
		var openHouseParameter = openHouse.HasValue ?
			new ObjectParameter("OpenHouse", openHouse) :
			new ObjectParameter("OpenHouse", typeof(bool));
		var searchTextParameter = searchText != null ?
			new ObjectParameter("SearchText", searchText) :
			new ObjectParameter("SearchText", typeof(string));
		var enableFiltersParameter = enableFilters.HasValue ?
			new ObjectParameter("EnableFilters", enableFilters) :
			new ObjectParameter("EnableFilters", typeof(bool));
		var newHomesOnlyParameter = newHomesOnly.HasValue ?
			new ObjectParameter("NewHomesOnly", newHomesOnly) :
			new ObjectParameter("NewHomesOnly", typeof(bool));
		var showLotsLandParameter = showLotsLand.HasValue ?
			new ObjectParameter("ShowLotsLand", showLotsLand) :
			new ObjectParameter("ShowLotsLand", typeof(bool));
		var showcaseItemIDsToFilterThroughParameter = showcaseItemIDsToFilterThrough != null ?
			new ObjectParameter("ShowcaseItemIDsToFilterThrough", showcaseItemIDsToFilterThrough) :
			new ObjectParameter("ShowcaseItemIDsToFilterThrough", typeof(string));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ShowcaseItemForJSON>("Showcase_GetFilteredShowcaseItemIDs", pageSizeParameter, pageNumberParameter, filterParameter, ascendingParameter, orderByParameter, filterShowcaseItemActiveParameter, filterShowcaseItemShowcaseIDParameter, filterShowcaseItemNeighborhoodIDParameter, filterShowcaseItemAgentIDParameter, addressLatParameter, addressLongParameter, minDistanceParameter, maxDistanceParameter, openHouseParameter, searchTextParameter, enableFiltersParameter, newHomesOnlyParameter, showLotsLandParameter, showcaseItemIDsToFilterThroughParameter);
	}

	public virtual ObjectResult<Nullable<bool>> SiteWide_DoesFilenameExist(string pageName, string microsite, string sEOLink)
	{
		var pageNameParameter = pageName != null ?
			new ObjectParameter("PageName", pageName) :
			new ObjectParameter("PageName", typeof(string));
		var micrositeParameter = microsite != null ?
			new ObjectParameter("Microsite", microsite) :
			new ObjectParameter("Microsite", typeof(string));
		var sEOLinkParameter = sEOLink != null ?
			new ObjectParameter("SEOLink", sEOLink) :
			new ObjectParameter("SEOLink", typeof(string));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("SiteWide_DoesFilenameExist", pageNameParameter, micrositeParameter, sEOLinkParameter);
	}

	public virtual ObjectResult<DynamicSitemap_URLAndTitle> SiteWide_GetDynamicSitemap()
	{
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DynamicSitemap_URLAndTitle>("SiteWide_GetDynamicSitemap");
	}

	public virtual ObjectResult<Nullable<short>> SiteWide_GetMaxDisplayOrder(string tableName, string tableIDField, Nullable<int> parentTableID, string parentTableIDField, string additionalIDField, Nullable<int> additionalID)
	{
		var tableNameParameter = tableName != null ?
			new ObjectParameter("TableName", tableName) :
			new ObjectParameter("TableName", typeof(string));
		var tableIDFieldParameter = tableIDField != null ?
			new ObjectParameter("TableIDField", tableIDField) :
			new ObjectParameter("TableIDField", typeof(string));
		var parentTableIDParameter = parentTableID.HasValue ?
			new ObjectParameter("ParentTableID", parentTableID) :
			new ObjectParameter("ParentTableID", typeof(int));
		var parentTableIDFieldParameter = parentTableIDField != null ?
			new ObjectParameter("ParentTableIDField", parentTableIDField) :
			new ObjectParameter("ParentTableIDField", typeof(string));
		var additionalIDFieldParameter = additionalIDField != null ?
			new ObjectParameter("AdditionalIDField", additionalIDField) :
			new ObjectParameter("AdditionalIDField", typeof(string));
		var additionalIDParameter = additionalID.HasValue ?
			new ObjectParameter("AdditionalID", additionalID) :
			new ObjectParameter("AdditionalID", typeof(int));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<short>>("SiteWide_GetMaxDisplayOrder", tableNameParameter, tableIDFieldParameter, parentTableIDParameter, parentTableIDFieldParameter, additionalIDFieldParameter, additionalIDParameter);
	}

	public virtual int SiteWide_UpdateDefaultContent(string componentName, Nullable<bool> delete)
	{
		var componentNameParameter = componentName != null ?
			new ObjectParameter("ComponentName", componentName) :
			new ObjectParameter("ComponentName", typeof(string));
		var deleteParameter = delete.HasValue ?
			new ObjectParameter("Delete", delete) :
			new ObjectParameter("Delete", typeof(bool));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SiteWide_UpdateDefaultContent", componentNameParameter, deleteParameter);
	}

	public virtual int StateAndCountry_ToggleShipTo(Nullable<bool> toggleState, Nullable<bool> shipTo)
	{
		var toggleStateParameter = toggleState.HasValue ?
			new ObjectParameter("ToggleState", toggleState) :
			new ObjectParameter("ToggleState", typeof(bool));
		var shipToParameter = shipTo.HasValue ?
			new ObjectParameter("ShipTo", shipTo) :
			new ObjectParameter("ShipTo", typeof(bool));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("StateAndCountry_ToggleShipTo", toggleStateParameter, shipToParameter);
	}

	public virtual ObjectResult<NearbyLocations> WhatsNearBy_GetLocationsNearCoordinates(Nullable<decimal> latitude, Nullable<decimal> longitude, Nullable<int> distanceAway)
	{
		var latitudeParameter = latitude.HasValue ?
			new ObjectParameter("Latitude", latitude) :
			new ObjectParameter("Latitude", typeof(decimal));
		var longitudeParameter = longitude.HasValue ?
			new ObjectParameter("Longitude", longitude) :
			new ObjectParameter("Longitude", typeof(decimal));
		var distanceAwayParameter = distanceAway.HasValue ?
			new ObjectParameter("DistanceAway", distanceAway) :
			new ObjectParameter("DistanceAway", typeof(int));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<NearbyLocations>("WhatsNearBy_GetLocationsNearCoordinates", latitudeParameter, longitudeParameter, distanceAwayParameter);
	}

	public virtual int OpenHouse_AddRecurringOpenHouses(string dateString, Nullable<int> showcaseItemID)
	{
		var dateStringParameter = dateString != null ?
			new ObjectParameter("DateString", dateString) :
			new ObjectParameter("DateString", typeof(string));
		var showcaseItemIDParameter = showcaseItemID.HasValue ?
			new ObjectParameter("ShowcaseItemID", showcaseItemID) :
			new ObjectParameter("ShowcaseItemID", typeof(int));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("OpenHouse_AddRecurringOpenHouses", dateStringParameter, showcaseItemIDParameter);
	}

	public virtual int Showcase_UpdateHistoricalStatData(Nullable<System.DateTime> currentDate)
	{
		var currentDateParameter = currentDate.HasValue ?
			new ObjectParameter("CurrentDate", currentDate) :
			new ObjectParameter("CurrentDate", typeof(System.DateTime));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Showcase_UpdateHistoricalStatData", currentDateParameter);
	}

	public virtual ObjectResult<CUSTOM_ELMAH_GetRETSErrors_Result> CUSTOM_ELMAH_GetRETSErrors()
	{
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CUSTOM_ELMAH_GetRETSErrors_Result>("CUSTOM_ELMAH_GetRETSErrors");
	}

	public virtual ObjectResult<CUSTOM_ELMAH_GetRETSErrors_New_Result> CUSTOM_ELMAH_GetRETSErrors_New()
	{
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CUSTOM_ELMAH_GetRETSErrors_New_Result>("CUSTOM_ELMAH_GetRETSErrors_New");
	}

	public virtual ObjectResult<Showcase_GetFilteredShowcaseAttributeIDs_Result> Showcase_GetFilteredShowcaseAttributeIDs(string filter, string filterShowcaseItemActive, string filterShowcaseItemShowcaseID, string filterShowcaseItemNeighborhoodID, string filterShowcaseItemAgentID, Nullable<decimal> addressLat, Nullable<decimal> addressLong, Nullable<int> minDistance, Nullable<int> maxDistance, Nullable<bool> openHouse, string searchText, Nullable<bool> enableFilters, Nullable<bool> newHomesOnly, Nullable<bool> showLotsLand, string showcaseItemIDsToFilterThrough)
	{
		var filterParameter = filter != null ?
			new ObjectParameter("Filter", filter) :
			new ObjectParameter("Filter", typeof(string));
		var filterShowcaseItemActiveParameter = filterShowcaseItemActive != null ?
			new ObjectParameter("FilterShowcaseItemActive", filterShowcaseItemActive) :
			new ObjectParameter("FilterShowcaseItemActive", typeof(string));
		var filterShowcaseItemShowcaseIDParameter = filterShowcaseItemShowcaseID != null ?
			new ObjectParameter("FilterShowcaseItemShowcaseID", filterShowcaseItemShowcaseID) :
			new ObjectParameter("FilterShowcaseItemShowcaseID", typeof(string));
		var filterShowcaseItemNeighborhoodIDParameter = filterShowcaseItemNeighborhoodID != null ?
			new ObjectParameter("FilterShowcaseItemNeighborhoodID", filterShowcaseItemNeighborhoodID) :
			new ObjectParameter("FilterShowcaseItemNeighborhoodID", typeof(string));
		var filterShowcaseItemAgentIDParameter = filterShowcaseItemAgentID != null ?
			new ObjectParameter("FilterShowcaseItemAgentID", filterShowcaseItemAgentID) :
			new ObjectParameter("FilterShowcaseItemAgentID", typeof(string));
		var addressLatParameter = addressLat.HasValue ?
			new ObjectParameter("AddressLat", addressLat) :
			new ObjectParameter("AddressLat", typeof(decimal));
		var addressLongParameter = addressLong.HasValue ?
			new ObjectParameter("AddressLong", addressLong) :
			new ObjectParameter("AddressLong", typeof(decimal));
		var minDistanceParameter = minDistance.HasValue ?
			new ObjectParameter("MinDistance", minDistance) :
			new ObjectParameter("MinDistance", typeof(int));
		var maxDistanceParameter = maxDistance.HasValue ?
			new ObjectParameter("MaxDistance", maxDistance) :
			new ObjectParameter("MaxDistance", typeof(int));
		var openHouseParameter = openHouse.HasValue ?
			new ObjectParameter("OpenHouse", openHouse) :
			new ObjectParameter("OpenHouse", typeof(bool));
		var searchTextParameter = searchText != null ?
			new ObjectParameter("SearchText", searchText) :
			new ObjectParameter("SearchText", typeof(string));
		var enableFiltersParameter = enableFilters.HasValue ?
			new ObjectParameter("EnableFilters", enableFilters) :
			new ObjectParameter("EnableFilters", typeof(bool));
		var newHomesOnlyParameter = newHomesOnly.HasValue ?
			new ObjectParameter("NewHomesOnly", newHomesOnly) :
			new ObjectParameter("NewHomesOnly", typeof(bool));
		var showLotsLandParameter = showLotsLand.HasValue ?
			new ObjectParameter("ShowLotsLand", showLotsLand) :
			new ObjectParameter("ShowLotsLand", typeof(bool));
		var showcaseItemIDsToFilterThroughParameter = showcaseItemIDsToFilterThrough != null ?
			new ObjectParameter("ShowcaseItemIDsToFilterThrough", showcaseItemIDsToFilterThrough) :
			new ObjectParameter("ShowcaseItemIDsToFilterThrough", typeof(string));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Showcase_GetFilteredShowcaseAttributeIDs_Result>("Showcase_GetFilteredShowcaseAttributeIDs", filterParameter, filterShowcaseItemActiveParameter, filterShowcaseItemShowcaseIDParameter, filterShowcaseItemNeighborhoodIDParameter, filterShowcaseItemAgentIDParameter, addressLatParameter, addressLongParameter, minDistanceParameter, maxDistanceParameter, openHouseParameter, searchTextParameter, enableFiltersParameter, newHomesOnlyParameter, showLotsLandParameter, showcaseItemIDsToFilterThroughParameter);
	}

	public virtual ObjectResult<Showcase_GetFilteredShowcaseAttributeMinMax_Result> Showcase_GetFilteredShowcaseAttributeMinMax(string filter, string filterShowcaseItemActive, string filterShowcaseItemShowcaseID, string filterShowcaseItemNeighborhoodID, string filterShowcaseItemAgentID, Nullable<decimal> addressLat, Nullable<decimal> addressLong, Nullable<int> minDistance, Nullable<int> maxDistance, Nullable<bool> openHouse, string searchText, Nullable<bool> enableFilters, Nullable<bool> newHomesOnly, Nullable<bool> showLotsLand, string showcaseItemIDsToFilterThrough)
	{
		var filterParameter = filter != null ?
			new ObjectParameter("Filter", filter) :
			new ObjectParameter("Filter", typeof(string));
		var filterShowcaseItemActiveParameter = filterShowcaseItemActive != null ?
			new ObjectParameter("FilterShowcaseItemActive", filterShowcaseItemActive) :
			new ObjectParameter("FilterShowcaseItemActive", typeof(string));
		var filterShowcaseItemShowcaseIDParameter = filterShowcaseItemShowcaseID != null ?
			new ObjectParameter("FilterShowcaseItemShowcaseID", filterShowcaseItemShowcaseID) :
			new ObjectParameter("FilterShowcaseItemShowcaseID", typeof(string));
		var filterShowcaseItemNeighborhoodIDParameter = filterShowcaseItemNeighborhoodID != null ?
			new ObjectParameter("FilterShowcaseItemNeighborhoodID", filterShowcaseItemNeighborhoodID) :
			new ObjectParameter("FilterShowcaseItemNeighborhoodID", typeof(string));
		var filterShowcaseItemAgentIDParameter = filterShowcaseItemAgentID != null ?
			new ObjectParameter("FilterShowcaseItemAgentID", filterShowcaseItemAgentID) :
			new ObjectParameter("FilterShowcaseItemAgentID", typeof(string));
		var addressLatParameter = addressLat.HasValue ?
			new ObjectParameter("AddressLat", addressLat) :
			new ObjectParameter("AddressLat", typeof(decimal));
		var addressLongParameter = addressLong.HasValue ?
			new ObjectParameter("AddressLong", addressLong) :
			new ObjectParameter("AddressLong", typeof(decimal));
		var minDistanceParameter = minDistance.HasValue ?
			new ObjectParameter("MinDistance", minDistance) :
			new ObjectParameter("MinDistance", typeof(int));
		var maxDistanceParameter = maxDistance.HasValue ?
			new ObjectParameter("MaxDistance", maxDistance) :
			new ObjectParameter("MaxDistance", typeof(int));
		var openHouseParameter = openHouse.HasValue ?
			new ObjectParameter("OpenHouse", openHouse) :
			new ObjectParameter("OpenHouse", typeof(bool));
		var searchTextParameter = searchText != null ?
			new ObjectParameter("SearchText", searchText) :
			new ObjectParameter("SearchText", typeof(string));
		var enableFiltersParameter = enableFilters.HasValue ?
			new ObjectParameter("EnableFilters", enableFilters) :
			new ObjectParameter("EnableFilters", typeof(bool));
		var newHomesOnlyParameter = newHomesOnly.HasValue ?
			new ObjectParameter("NewHomesOnly", newHomesOnly) :
			new ObjectParameter("NewHomesOnly", typeof(bool));
		var showLotsLandParameter = showLotsLand.HasValue ?
			new ObjectParameter("ShowLotsLand", showLotsLand) :
			new ObjectParameter("ShowLotsLand", typeof(bool));
		var showcaseItemIDsToFilterThroughParameter = showcaseItemIDsToFilterThrough != null ?
			new ObjectParameter("ShowcaseItemIDsToFilterThrough", showcaseItemIDsToFilterThrough) :
			new ObjectParameter("ShowcaseItemIDsToFilterThrough", typeof(string));
		return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Showcase_GetFilteredShowcaseAttributeMinMax_Result>("Showcase_GetFilteredShowcaseAttributeMinMax", filterParameter, filterShowcaseItemActiveParameter, filterShowcaseItemShowcaseIDParameter, filterShowcaseItemNeighborhoodIDParameter, filterShowcaseItemAgentIDParameter, addressLatParameter, addressLongParameter, minDistanceParameter, maxDistanceParameter, openHouseParameter, searchTextParameter, enableFiltersParameter, newHomesOnlyParameter, showLotsLandParameter, showcaseItemIDsToFilterThroughParameter);
	}
}
