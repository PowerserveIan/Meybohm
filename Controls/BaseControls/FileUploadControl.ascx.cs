using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Collections.Generic;

public partial class Controls_BaseControls_FileUploadControl : UserControl
{
	protected string m_AllowedFileTypes;
	private bool m_ExternalLinkIsImage = true;
	private int m_FileNameLengthLimit = 50;
	private int m_ImageHeight = 100;
	private int m_ImageWidth = 100;
	private int m_MaxFileSize = 10000000;
	private string m_UploadToLocation = "~/" + Globals.Settings.UploadFolder + "images/";
	private string m_UploadButtonText = "Upload Files";
	private string m_ValidationGroup = string.Empty;

    public bool IsMultipleEnabled { get; set; }
	public bool AllowExternalImageLink { get; set; }

	public bool ExternalLinkIsImage { get { return m_ExternalLinkIsImage; } set { m_ExternalLinkIsImage = value; } }

	/// <summary>
	/// Comma separated list of allowed file types (ex: ".gif,.jpg,.jpeg")
	/// </summary>
	public string AllowedFileTypes
	{
		get
		{
			if (!String.IsNullOrEmpty(m_AllowedFileTypes))
			{
				string tempFileTypes = string.Empty;
				foreach (string type in m_AllowedFileTypes.Split(','))
				{
					tempFileTypes += "(" + type.TrimStart('.').ToLower() + ")|";
				}
				return tempFileTypes.Trim().TrimEnd('|');
			}
			return "";
		}
		set
		{
			m_AllowedFileTypes = value;
		}
	}

	/// <summary>
	/// The image will be resized to correct size (proportions constrained) and then saved
	/// </summary>
	//public bool AutomaticallyResizeImage { get; set; }

	/// <summary>
	/// Whether or not a file must be supplied to pass validation
	/// </summary>
	public bool Required
	{
		get { return uxFileUploadRFV.Enabled; }
		set { uxFileUploadRFV.Enabled = value; }
	}

	/// <summary>
	/// Error message that will be displayed if a file is not supplied
	/// </summary>
	public string RequiredErrorMessage
	{
		set { uxFileUploadRFV.ErrorMessage = value; }
	}

	/// <summary>
	/// Text that will be displayed if a file is not supplied
	/// </summary>
	public string RequiredText
	{
		set { uxFileUploadRFV.Text = value; }
	}

	/// <summary>
	/// Turns on/off client side validation
	/// </summary>
	public bool EnableClientScript
	{
		get { return uxFileUploadRFV.EnableClientScript; }
		set { uxFileUploadRFV.EnableClientScript = value; }
	}

	/// <summary>
	/// Relative path location where file should be uploaded (start with "~/")
	/// </summary>
	public string UploadToLocation
	{
		get { return m_UploadToLocation; }
		set { m_UploadToLocation = (value.StartsWith("~/") ? value : "~/" + value).TrimEnd('/') + "/"; }
	}

	public string FileName
	{
		get { return ViewState["FileName"] != null ? ViewState["FileName"].ToString() : ""; }
		set { ViewState["FileName"] = value; }
	}

    public string CaptionText
    {
        get { return ViewState["CaptionText"] != null ? ViewState["CaptionText"].ToString() : ""; }
        set { ViewState["CaptionText"] = value; }
    }

    public List<string> ListFileName
    {
        get { return ViewState["ListFileName"] != null ? (List<string>)ViewState["ListFileName"] : new List<string>(); }
        set { ViewState["ListFileName"] = value; }
    }

    public List<string> ListFileCaption
    {
        get { return ViewState["ListFileCaption"] != null ? (List<string>)ViewState["ListFileCaption"] : new List<string>(); }
        set { ViewState["ListFileCaption"] = value; }
    }

	/// <summary>
	/// If the file is an image, this is the width of the displayed image
	/// </summary>
	public int ImageWidth
	{
		get { return m_ImageWidth; }
		set { m_ImageWidth = value; }
	}

	/// <summary>
	/// If the file is an image, this is the height of the displayed image
	/// </summary>
	public int ImageHeight
	{
		get { return m_ImageHeight; }
		set { m_ImageHeight = value; }
	}

	/// <summary>
	/// Limit the filename to a certain length for db constraints
	/// </summary>
	public int FileNameLengthLimit
	{
		get { return m_FileNameLengthLimit; }
		set { m_FileNameLengthLimit = value; }
	}

	/// <summary>
	/// Returns the file extension of the uploaded file
	/// </summary>
	public string FileExtension
	{
		get { return ViewState["FileExtension"] != null ? ViewState["FileExtension"].ToString() : ""; }
		set { ViewState["FileExtension"] = value; }
	}

	/// <summary>
	/// Returns the file size of the uploaded file
	/// </summary>
	public long FileSize
	{
		get { return !String.IsNullOrEmpty(FileName) && File.Exists(Server.MapPath(UploadToLocation + FileName)) ? new FileInfo(Server.MapPath(UploadToLocation + FileName)).Length : 0; }
	}

	/// <summary>
	/// Maximum file size allowed for upload (bytes).
	/// </summary>
	public int MaxFileSize
	{
		get { return m_MaxFileSize; }
		set { m_MaxFileSize = value; }
	}

	public bool ReadOnly { get; set; }

	public string ValidationGroup
	{
		get { return m_ValidationGroup; }
		set { m_ValidationGroup = value; }
	}

	public string UploadButtonText
	{
		get { return m_UploadButtonText; }
		set { m_UploadButtonText = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		if (!ReadOnly)
		{
			KillCache();
			CheckForUploadedFile();
		}
		base.OnInit(e);
		uxFileUploadRFV.ClientValidationFunction = "ValidateRequiredFile_" + ClientID;
		uxFileUploadRFV.ServerValidate += uxFileUploadRFV_ServerValidate;
		uxFileUploadRFV.ValidationGroup = ValidationGroup;
		uxFileUploadRFV.Enabled = Required && !ReadOnly;
		uxExternalImagePH.Visible = AllowExternalImageLink;

	}

	void CheckForUploadedFile()
	{
		if (IsPostBack && Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
		{
			HttpFileCollection files = Request.Files; // Load File collection into HttpFileCollection variable.
			string[] arr1 = files.AllKeys;  // This will get names of all files into a string array.
			if (!Directory.Exists(Server.MapPath("~/" + Globals.Settings.UploadFolder + "temp/")))
				Directory.CreateDirectory(Server.MapPath("~/" + Globals.Settings.UploadFolder + "temp/"));
			for (int loop1 = 0; loop1 < arr1.Length; loop1++)
			{
				if (!System.Text.RegularExpressions.Regex.IsMatch(files[loop1].FileName, "." + AllowedFileTypes + "$", System.Text.RegularExpressions.RegexOptions.IgnoreCase) || files[loop1].ContentLength > MaxFileSize)
					continue;
				//Stupid IE fix
				string filename = files[loop1].FileName.Contains("\\") ? files[loop1].FileName.Substring(files[loop1].FileName.LastIndexOf("\\") + 1) : files[loop1].FileName;
				if (File.Exists("~/" + Globals.Settings.UploadFolder + "temp/" + filename))
				{
					int appendToFilename = 1;
					while (File.Exists("~/" + Globals.Settings.UploadFolder + "temp/" + filename + "-" + appendToFilename))
					{ appendToFilename++; }
					filename += "-" + appendToFilename;
				}
				files[loop1].SaveAs(Server.MapPath("~/" + Globals.Settings.UploadFolder + "temp/") + filename);
				Response.ContentType = "text/plain";
				Response.Write(@"{""name"":""" + filename + @""",""type"":""" + files[loop1].ContentType + @""",""size"":""" + files[loop1].ContentLength + @"""}");
			}

			Response.End();
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		Helpers.GetCSSCode(uxCSSFiles);
		Helpers.GetJSCode(uxJavaScripts);
		if (!String.IsNullOrEmpty(FileName))
		{
			if (IsUploadExternal())
			{
				if (IsImage())
				{
					uxImage.ImageUrl = "~/resizer.aspx?filename=" + Server.UrlEncode(FileName) + "&width=" + ImageWidth + "&height=" + ImageHeight + "&trim=1";
					uxImage.AlternateText = FileName;
				}
				else
					uxFancyLink.Text = FileName;

				uxFancyLink.NavigateUrl = FileName;
			}
			else if (File.Exists(GetFilePath()))
			{
				if (IsImage())
				{
					uxImage.ImageUrl = "~/resizer.aspx?filename=" + UploadToLocation.Replace("~/", "").TrimEnd('/') + "/" + FileName + "&width=" + ImageWidth + "&height=" + ImageHeight + "&trim=1";
					uxImage.AlternateText = FileName;
				}
				else
					uxFancyLink.Text = FileName;
				uxFancyLink.NavigateUrl = UploadToLocation + "/" + FileName;
			}
			else
			{
				uxFileNotFound.Text = FileName + " could not be found.";
				uxFileNotFound.Visible = true;
			}
			uxUploader.Attributes["style"] = "display: none;";
			markForDeletion.Attributes.Remove("style");
			uxDeletePH.Attributes.Remove("style");
		}
		else
		{
			markForDeletion.Attributes["style"] =
			uxDeletePH.Attributes["style"] = "display: none;";
			uxUploader.Attributes.Remove("style");
		}
		uxUploadButtonText.Text = UploadButtonText;
	}

	private string AppendDate(string fileName)
	{
		int start = fileName.LastIndexOf(".");
		string ext = (start > 0) ? fileName.Substring(start) : "";
		string file = fileName.Substring(0, fileName.Length - ext.Length);
		string temp = (DateTime.UtcNow + DateTime.UtcNow.Ticks.ToString().Substring(0, 2))
			.Replace("/", "").Replace(" ", "").Replace(":", "").Replace("\\", "").Replace(",", "");
		fileName = file + temp;
		if ((fileName.Length + ext.Length) > FileNameLengthLimit)
			fileName = fileName.Substring(0, FileNameLengthLimit - (1 + ext.Length)) + ext;
		else
			fileName = fileName + ext;
		return fileName;
	}

	public void CommitChanges()
	{
		if (Page.IsValid && !ReadOnly)
		{
			if ((!String.IsNullOrEmpty(uxFileUploaded.Value) || !String.IsNullOrEmpty(uxFileDeleted.Value)) && !String.IsNullOrEmpty(FileName) && File.Exists(GetFilePath()))
				File.Delete(GetFilePath());

			if (!String.IsNullOrEmpty(uxFileUploaded.Value))
			{
                List<string> listFileName = new List<string>();
                List<string> listFileCaption = new List<string>();

                string[] arrFileNames = uxFileUploaded.Value.Split(',');
                string[] arrFileCaptions = uxFileCaption.Value.Split(',');
                
                //Loop through each File Name, if it's not null or empty string, save it/modify it and add it to the list.
                for (int index = 0; index < arrFileNames.Length; index++)
                {
                    if (!String.IsNullOrEmpty(arrFileNames[index]))
                    {
                        FileName = arrFileNames[index];
                        CaptionText = arrFileCaptions[index];

                        string oldFileName = FileName;
                        if (File.Exists(Server.MapPath(UploadToLocation) + FileName))
                            FileName = AppendDate(FileName);
                        if (File.Exists(Server.MapPath("~/" + Globals.Settings.UploadFolder + "temp/") + oldFileName))
                        {
                            try
                            {
                                File.Move(Server.MapPath("~/" + Globals.Settings.UploadFolder + "temp/") + oldFileName, Server.MapPath(UploadToLocation) + "/" + FileName);
                            }
                            catch(IOException exception)
                            {
                                //Made to catch IO Exception, File ALready Exists.
                                Helpers.LogException(exception);
                            }
                        }
                        else
                            Helpers.LogException(new Exception("An uploaded file was deleted before it could be fully saved."));
                        uxFileDeleted.Value = uxFileUploaded.Value = string.Empty;
                        uxFileNotFound.Visible = false;

                        //Add the FileName to the List
                        listFileName.Add(FileName);
                        listFileCaption.Add(arrFileCaptions[index]);
                    }
                }

                this.ListFileName = listFileName;
                this.ListFileCaption = listFileCaption;
			}
            else if (AllowExternalImageLink && !String.IsNullOrEmpty(uxImageExternal.Text))
            {
                FileName = uxImageExternal.Text;
            }
            else if (!String.IsNullOrEmpty(uxFileDeleted.Value))
            {
                FileName = string.Empty;
            }
		}
	}

	private string GetFilePath()
	{
		if (IsUploadExternal())
			return FileName;
		if (!Directory.Exists(Context.Server.MapPath(UploadToLocation)))
			Directory.CreateDirectory(Context.Server.MapPath(UploadToLocation));

		return Server.MapPath(UploadToLocation + "/" + FileName);
	}

	protected bool IsImage()
	{
		if (ExternalLinkIsImage && IsUploadExternal())
			return true;
		if (!String.IsNullOrEmpty(FileName))
		{
			string tempFile = FileName.ToLower();
			return tempFile.EndsWith(".jpg") || tempFile.EndsWith(".jpeg") || tempFile.EndsWith(".gif") || tempFile.EndsWith(".png") || tempFile.EndsWith(".bmp");
		}
		return !String.IsNullOrEmpty(m_AllowedFileTypes) && (m_AllowedFileTypes.ToLower().Contains(".jpg") || m_AllowedFileTypes.ToLower().Contains(".jpeg") || m_AllowedFileTypes.ToLower().Contains(".gif") || m_AllowedFileTypes.ToLower().Contains(".png") || m_AllowedFileTypes.ToLower().Contains(".bmp"));
	}

	protected bool IsUploadExternal()
	{
		return AllowExternalImageLink && FileName.ToLower().StartsWith("http");
	}

	void uxFileUploadRFV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = (!String.IsNullOrEmpty(FileName) && String.IsNullOrEmpty(uxFileDeleted.Value)) || !String.IsNullOrEmpty(uxFileUploaded.Value) || (AllowExternalImageLink && !String.IsNullOrEmpty(uxImageExternal.Text));
	}

	private void KillCache()
	{
		if (!String.IsNullOrEmpty(Request.Form["killCache"]))
		{
			Cache.Remove("Resizer_" + Request.Form["fileName"]);
			Response.End();
		}
	}
}