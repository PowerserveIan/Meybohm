using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.ContentManager;
using Classes.SiteLanguages;
using DifferenceEngine;
using Settings = Classes.ContentManager.Settings;

/* LICENSING INFORMATION FOR DiffEngine By Michael Potter
 * 
 *  [...] Seriously, use it any way you want. Let me know if you find any improvements (I am sure there are many). If you are exposing the source in another project it would be nice to drop in a comment that points back here. Not necessary, but nice. [...]

 *  Date Seen : 20050519
 *  http://codeproject.com/csharp/DiffEngine.asp?df=100&forumid=42386&fr=26
 *  Re: Great Stuff! - Copywrite?
 * Thanks,

Copywrite - let me check my files. Ahh... yes... here it is... If you use this code and make a million dollars you are required to come to St. Louis and buy me a beer.

Seriously, use it any way you want. Let me know if you find any improvements (I am sure there are many). If you are exposing the source in another project it would be nice to drop in a comment that points back here. Not necessary, but nice.


I have used NAnt - works very well when sharing source with developers across the net. 
 */

/// <summary>
/// 	Written By: Charles Cook
/// </summary>
public partial class ContentManagerDiff : Page
{
	protected CMPage cmPLeft;
	protected CMPageRegion cmPRLeft;
	protected CMPageRegion cmPRRight;
	protected CMPage cmPRight;
	protected CMRegion cmRLeft;
	protected CMRegion cmRRight;
	protected PlaceHolder content;
	protected List<DifferenceRow> ds;
	protected int leftId;
	protected int rightId;
	protected double time;

	protected int LanguageID
	{
		get
		{
			if (ViewState["LanguageID"] == null)
			{
				Language currLanguage = Helpers.GetCurrentLanguage();
				return currLanguage.LanguageID;
			}
			return (int)ViewState["LanguageID"];
		}
		set { ViewState["LanguageID"] = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		rDiffReport.DataBinding += rDiffReport_DataBinding;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			leftId = int.Parse(Request.QueryString["leftid"]);
			rightId = int.Parse(Request.QueryString["rightid"]);
		}
		catch (FormatException)
		{
		}
		catch (ArgumentNullException)
		{
		}

		if (!IsPostBack)
		{
			ds = new List<DifferenceRow>();

			if (ValidIDs())
			{
				DataBind();
			}
		}
	}

	private void AddRow(string action, string source, string destination)
	{
		ds.Add(new DifferenceRow
		       	{
		       		Action = action,
		       		DestinationText = destination,
		       		SourceText = source
		       	});
	}

	private bool ValidIDs()
	{
		if (leftId == 0) return false;
		if (rightId == 0) return false;
		if (rightId == leftId) return false;

		if (CMPageRegion.GetByID(leftId) == null) return false;
		if (CMPageRegion.GetByID(rightId) == null) return false;

		return true;
	}

	private void rDiffReport_DataBinding(object sender, EventArgs e)
	{
		DiffList_Text sLF = null;
		DiffList_Text dLF = null;

		cmPRLeft = CMPageRegion.GetByID(leftId);
		cmPRRight = CMPageRegion.GetByID(rightId);
		cmRLeft = CMRegion.GetByID(cmPRLeft.CMRegionID);
		cmRRight = CMRegion.GetByID(cmPRRight.CMRegionID);

		if (Settings.EnableMultipleLanguages)
		{
			cmPLeft = CMPage.GetByCMPageIDAndLanguageID(cmPRLeft.CMPageID, LanguageID);
			cmPRight = CMPage.GetByCMPageIDAndLanguageID(cmPRRight.CMPageID, LanguageID);
		}
		else
		{
			cmPLeft = CMPage.GetByID(cmPRLeft.CMPageID);
			cmPRight = CMPage.GetByID(cmPRRight.CMPageID);
		}

		sLF = new DiffList_Text(cmPRLeft.Content);
		dLF = new DiffList_Text(cmPRRight.Content);

		DiffEngine de = new DiffEngine();
		time = de.ProcessDiff(sLF, dLF, DiffEngineLevel.Medium);
		ArrayList rep = de.DiffReport();

		foreach (DiffResultSpan drs in rep)
		{
			int i;
			switch (drs.Status)
			{
				case DiffResultSpanStatus.DeleteSource:
					for (i = 0; i < drs.Length; i++)
						AddRow("DeleteSource", ((string)sLF.GetByIndex(drs.SourceIndex + i)), "");
					break;
				case DiffResultSpanStatus.NoChange:
					for (i = 0; i < drs.Length; i++)
						AddRow("NoChange", ((string)sLF.GetByIndex(drs.SourceIndex + i)), ((string)dLF.GetByIndex(drs.DestIndex + i)));
					break;
				case DiffResultSpanStatus.AddDestination:
					for (i = 0; i < drs.Length; i++)
						AddRow("AddDestination", "", ((string)dLF.GetByIndex(drs.DestIndex + i)));
					break;
				case DiffResultSpanStatus.Replace:
					for (i = 0; i < drs.Length; i++)
						AddRow("Replace", ((string)sLF.GetByIndex(drs.SourceIndex + i)), ((string)dLF.GetByIndex(drs.DestIndex + i)));
					break;
			}
		}
	}

	#region Nested type: DifferenceRow

	protected class DifferenceRow
	{
		public string Action { get; set; }
		public string SourceText { get; set; }
		public string DestinationText { get; set; }
	}

	#endregion
}