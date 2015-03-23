using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.SEOComponent;
using System.IO;
using BaseCode;

public partial class Controls_SEOComponent_SEO_Data_Entry : UserControl
{
	private readonly bool m_EnableMultipleLanguages = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ContentManager_enableMultipleLanguages"]) ? Convert.ToBoolean(ConfigurationManager.AppSettings["ContentManager_enableMultipleLanguages"]) : false;

	public enum SitePageLinkSetupTypeEnum
	{
		PageFormatter,
		SEODataID,
		PageUrl
	}

	protected bool m_Approved = true;
	protected string m_ValidationGroupName = String.Empty;
	protected string m_PageLinkFormatter = string.Empty;
	protected string m_PageUrl = string.Empty;
	protected bool m_ShowFriendlyFilename = true;
	private bool m_RequireAspxExtension = Globals.Settings.RequireASPXExtensions;
	protected SitePageLinkSetupTypeEnum m_SitePageLinkSetupType = SitePageLinkSetupTypeEnum.PageFormatter;

	/// <summary>
	/// Toggles display of the friendly filename field
	/// </summary>
	public bool ShowFriendlyFilename
	{
		get { return m_ShowFriendlyFilename; }
		set { m_ShowFriendlyFilename = value; }
	}

	/// <summary>
	/// Gets/sets the validationgroup name used by the control.  It will need to be used by the control to set the validationgroups as well as which validation to check
	/// </summary>
	public string ValidationGroupName
	{
		get { return m_ValidationGroupName; }
		set { m_ValidationGroupName = value; }
	}

	public String Title
	{
		get { return uxTitle.Text; }
		set { uxTitle.Text = value; }
	}

	public String Description
	{
		get { return uxDescription.Text; }
		set { uxDescription.Text = value; }
	}

	public String Keywords
	{
		get { return uxKeywords.Text; }
		set { uxKeywords.Text = value; }
	}

	public String FriendlyFilename
	{
		get { return uxFriendlyFilename.Text; }
		set { uxFriendlyFilename.Text = value; }
	}

	public bool Approved
	{
		get { return m_Approved; }
		set { m_Approved = value; }
	}

	public bool RequireAspxExtension
	{
		get { return m_RequireAspxExtension; }
		set { m_RequireAspxExtension = value; }
	}

	/// <summary>
	/// gets/sets how the sitepagelink is dealt with (add/edit/delete/find)
	/// </summary>
	public SitePageLinkSetupTypeEnum SitePageLinkSetupType
	{
		get { return m_SitePageLinkSetupType; }
		set { m_SitePageLinkSetupType = value; }
	}

	/// <summary>
	/// gets/sets the pagelink url format to be used (formatted like it would be used for string.format function)
	/// </summary>
	/// <example><![CDATA[product.aspx?id={0}]]></example>
	public string PageLinkFormatter
	{
		get { return m_PageLinkFormatter; }
		set { m_PageLinkFormatter = value; }
	}

	/// <summary>
	/// gets/sets the list of strings used to populate the pagelinkformatter
	/// </summary>
	public List<string> PageLinkFormatterElements
	{
		get
		{
			if (ViewState["PageLinkFormatterElements"] == null)
				ViewState["PageLinkFormatterElements"] = new List<string>();

			return ViewState["PageLinkFormatterElements"] as List<string>;
		}
		set { ViewState["PageLinkFormatterElements"] = value; }
	}

	/// <summary>
	/// Gets/sets the SEODataID field that the SEO data belongs to
	/// </summary>
	public int? SEODataID
	{
		get { return ViewState["SEODataID"] == null ? (int?)null : Convert.ToInt32(ViewState["SEODataID"]); }
		set { ViewState["SEODataID"] = value; }
	}

	/// <summary>
	/// Gets/sets the object used for loading/saving data
	/// </summary>
	public SEOData SEODataEntity { get; set; }

	public string PageUrl
	{
		get { return m_PageUrl.StartsWith("~/") ? m_PageUrl : "~/" + m_PageUrl; }
		set { m_PageUrl = value; }
	}

	public int? LanguageID
	{
		get { return ViewState["LanguageID"] == null ? (int?)null : Convert.ToInt32(ViewState["LanguageID"]); }
		set { ViewState["LanguageID"] = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxFriendlyFilenameUniqueVal.ServerValidate += uxFriendlyFilenameUniqueVal_ServerValidate;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		uxTitleRegexVal.ValidationGroup =
		uxPageLinkURLCV.ValidationGroup =
		uxFriendlyFilenameREV.ValidationGroup =
		uxFriendlyFilenameUniqueVal.ValidationGroup = ValidationGroupName;
		uxFriendlyFilenamePH.Visible = ShowFriendlyFilename;
		uxAspxAppended.Visible = RequireAspxExtension;
	}


	#region Saving Related Methods

	public bool SaveData(int? seoDataID)
	{
		SEODataID = seoDataID;
		SEODataEntity = seoDataID.HasValue ? SEOData.GetByID(seoDataID.Value) : new SEOData();
		return SEODataEntity != null && SaveData();
	}

	private bool SaveData()
	{
		string tempURL = PageUrl == "~/" ? GetURLFromPageLinkFormatter() : PageUrl;
		if (tempURL.Length > 0)
		{
			SEODataEntity.PageURL = tempURL;
			if (SEODataEntity.IsNewRecord)
				SEODataEntity.DateCreated = DateTime.UtcNow;
			SEODataEntity.DateLastUpdated = DateTime.UtcNow;
			SEODataEntity.Title = Title;
			SEODataEntity.Description = Description;
			SEODataEntity.Keywords = Keywords;
			if (ShowFriendlyFilename && !String.IsNullOrEmpty(FriendlyFilename))
				SEODataEntity.FriendlyFilename = FriendlyFilename.Replace(".aspx", "") + (RequireAspxExtension ? ".aspx" : "");
			else if (ShowFriendlyFilename)
				SEODataEntity.FriendlyFilename = null;
			SEODataEntity.LanguageID = LanguageID;
			SEODataEntity.Approved = Approved;
			SEODataEntity.Save();
			if (ShowFriendlyFilename && !String.IsNullOrEmpty(FriendlyFilename))
				Helpers.PurgeCacheItems("ContentManager_CMPage_DoesFilenameExist_");
			return true;
		}
		return false;
	}

	public bool SaveControlData()
	{
		GetPageLink();
		return SaveData();
	}

	#endregion

	#region Loading Related Methods

	/// <summary>
	/// Sets the control's SitePageSEOSetup object to the passed in SitePageSEOSetup object, then loads the data
	/// </summary>
	/// <param name="itemEntity"></param>
	public void LoadData(SEOData itemEntity)
	{
		SEODataEntity = itemEntity;
		if (SEODataEntity != null)
		{
			//Get the SitePageLink object for the specified SitePageSEOSetup object
			SitePageLinkSetupTypeEnum tempSetupType = SitePageLinkSetupType;
			SitePageLinkSetupType = SitePageLinkSetupTypeEnum.SEODataID;
			SEODataID = SEODataEntity.SEODataID;
			GetPageLink();
			SitePageLinkSetupType = tempSetupType;

			LoadData();
		}
	}

	/// <summary>
	/// Loads the object's data into the fields...assumes the object exists
	/// </summary>
	private void LoadData()
	{
		Title = SEODataEntity.Title;
		Description = SEODataEntity.Description;
		Keywords = SEODataEntity.Keywords;
		if (!String.IsNullOrEmpty(SEODataEntity.FriendlyFilename))
			FriendlyFilename = SEODataEntity.FriendlyFilename.Replace(".aspx", "");
	}

	/// <summary>
	/// Use this when the proper properties are filled out and the setup type is specified
	/// </summary>
	public void LoadControlData()
	{
		GetPageLink();

		if (SEODataEntity != null)
			LoadData();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="isNewSEO">True if this is used to load the control for a brand new page(new page link) otherwise false</param>
	public void LoadControlData(bool isNewSEO)
	{
		if (isNewSEO)
			SEODataEntity = new SEOData();
		else
			LoadControlData();
	}


	#endregion

	/// <summary>
	/// Fills the object using the specified data depending on the type.  If none is found, creates a new one
	/// </summary>
	private void GetPageLink()
	{
		switch (SitePageLinkSetupType)
		{
			case SitePageLinkSetupTypeEnum.PageFormatter:
			case SitePageLinkSetupTypeEnum.PageUrl:
				List<SEOData> currentItemList = SitePageLinkSetupType == SitePageLinkSetupTypeEnum.PageFormatter ? SEOData.SEODataGetByPageURL(GetURLFromPageLinkFormatter()) : SEOData.SEODataGetByPageURL(PageUrl);
				if (currentItemList.Exists(c => c.Approved == Approved && (!m_EnableMultipleLanguages || c.LanguageID == LanguageID)))
				{
					SEODataEntity = currentItemList.FirstOrDefault(c => c.Approved == Approved && (!m_EnableMultipleLanguages || c.LanguageID == LanguageID));

					//If more than 1, delete them
					for (int iLoop = currentItemList.Count(c => c.Approved == Approved && (!m_EnableMultipleLanguages || c.LanguageID == LanguageID)) - 1; iLoop > 0; iLoop--)
						currentItemList.Where(c => c.Approved == Approved && (!m_EnableMultipleLanguages || c.LanguageID == LanguageID)).ToList()[iLoop].Delete();
				}
				else
					SEODataEntity = new SEOData();
				break;
			case SitePageLinkSetupTypeEnum.SEODataID:
				SEODataEntity = SEODataID.HasValue ? SEOData.GetByID(SEODataID.Value) : new SEOData();
				break;
		}
	}

	private string GetURLFromPageLinkFormatter()
	{
		string toReturn = string.Empty;

		if (!string.IsNullOrEmpty(m_PageLinkFormatter))
		{
			int numOfPlaceHolders = m_PageLinkFormatter.Length - m_PageLinkFormatter.Replace("{", "").Length;

			if (numOfPlaceHolders == PageLinkFormatterElements.Count)
				//there are matching numbers of elements
				toReturn = string.Format(m_PageLinkFormatter, PageLinkFormatterElements.ToArray());
		}
		return toReturn;
	}

	/// <summary>
	/// Validates whether the pagelinkurl exists already or not
	/// </summary>
	/// <param name="source"></param>
	/// <param name="args"></param>
	protected void uxPageLinkURLCV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		if (SEODataEntity == null)
			LoadControlData(true);

		if (SEODataEntity.IsNewRecord)
			args.IsValid = true;
		else
		{
			string pageLinkFormatter = GetURLFromPageLinkFormatter();
			List<SEOData> itemList = SEOData.SEODataGetByPageURL(!String.IsNullOrEmpty(pageLinkFormatter) ? pageLinkFormatter : SEODataEntity.PageURL);

			args.IsValid = itemList.Count == 0 || (itemList.Count == 1 && itemList[0].SEODataID == SEODataEntity.SEODataID);
		}
	}

	void uxFriendlyFilenameUniqueVal_ServerValidate(object source, ServerValidateEventArgs args)
	{
		if (String.IsNullOrEmpty(FriendlyFilename))
		{
			args.IsValid = true;
			return;
		}
		try
		{
			FileInfo f = new FileInfo(Server.MapPath("~\\") + FriendlyFilename);
			args.IsValid = !f.Exists;
		}
		catch (Exception)
		{
			args.IsValid = true;
		}
		if (args.IsValid)
			args.IsValid = !Helpers.DoesFilenameExist(FriendlyFilename + (RequireAspxExtension ? ".aspx" : ""), PageUrl == "~/" ? GetURLFromPageLinkFormatter() : PageUrl);
	}
}