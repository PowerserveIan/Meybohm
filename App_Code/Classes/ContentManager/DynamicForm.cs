using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using BaseCode;

namespace Classes.ContentManager
{
	public class DynamicForm
	{
		private const string DynamicSubmitName = "dynamicsubmit";
		public static string PopupScriptName = "PopupScript";
		public const string UploadedFilesLocation = "~/uploads/cmsforms/";

		/// <summary>
		/// Updates the HTML, updating the names on the form fields. Fields without a name are autoassigned a name,
		/// those with a duplicate name are appended with a numeric index.
		/// </summary>
		/// <param name="html">HTML from editor such as RadEditor</param>
		/// <returns>Prepared HTML</returns>
		public static string PrepareDynamicForm(string html)
		{
			const string inputTemplate = @"<{0} name=""{1}"" {2} {3}>";

			StringBuilder preparedHtml = new StringBuilder();
			List<string> namedInputs = new List<string>();

			List<string> split = new List<string>(Regex.Split(html, @"(<\w+\s?.*?/?>)", RegexOptions.Multiline | RegexOptions.IgnoreCase));

			foreach (string s in split)
			{
				Match m = Regex.Match(s, @"<(\w+)\s?(.*?)(name=['""]([\w\s]*)['""])+(.*?)/?>", RegexOptions.IgnoreCase);
				if (!m.Success) //Regex doesnt have an agressive option for the name match to balance the lazy, so two matches are needed
					m = Regex.Match(s, @"<(\w+)\s?(.*?)(name=['""]([\w\s]*)['""])*(.*?)/?>", RegexOptions.IgnoreCase);
				if (m.Success)
				{
					var i =
						new
							{
								name = String.IsNullOrEmpty(m.Groups[4].Value) ? m.Groups[1].Value : m.Groups[4].Value,
								attributes = m.Groups[2].Value + " " + m.Groups[5].Value,
								type = m.Groups[1].Value
							};

					// for input and textarea tags
					if (i.type.Equals("input", StringComparison.OrdinalIgnoreCase)
						|| i.type.Equals("textarea", StringComparison.OrdinalIgnoreCase))
					{
						Match mtype = Regex.Match(i.attributes, @"type=['""](\w+)['""]", RegexOptions.IgnoreCase);
						if (!mtype.Groups[1].Value.Equals("submit", StringComparison.OrdinalIgnoreCase))
						{
							int countAppendage = 0;
							if (!i.attributes.Contains("type=\"radio\""))
							{
								while (namedInputs.Contains(i.name + EmptyIfZero(countAppendage)))
									countAppendage++;
							}

							namedInputs.Add(i.name + EmptyIfZero(countAppendage));
							string determinedClassName = DetermineClassName(i.type, mtype.Groups[1].Value, i.attributes);
							preparedHtml.Append(String.Format(inputTemplate, i.type, i.name + EmptyIfZero(countAppendage),
								(mtype.Groups[1].Value.Equals("reset", StringComparison.OrdinalIgnoreCase) ? Regex.Replace(determinedClassName, "style=['\"](.*?)['\"]", "") : determinedClassName),
								i.type.Equals("textarea") ? "" : "/"));
						}
						else
							preparedHtml.Append(String.Format(inputTemplate, i.type, DynamicSubmitName,
								Regex.Replace(DetermineClassName(i.type, "submit", i.attributes), "style=['\"](.*?)['\"]", ""), (i.attributes.Contains("value") ? "" : "value=\"Submit\"") + "/"));
					}
					else
						preparedHtml.Append(s);
				}
				else
					preparedHtml.Append(s);
			}
			return preparedHtml.ToString();
		}

		private static string DetermineClassName(string elementname, string inputtype, string attributes)
		{
			if (String.IsNullOrEmpty(inputtype))
				inputtype = elementname.Equals("input", StringComparison.OrdinalIgnoreCase) ? "text" : elementname;

			Match m = Regex.Match(attributes, @"class=['""](.*?)['""]", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			if (!m.Success)
				attributes += String.Format(" class=\"{0}\"", inputtype);
			else
				attributes = Regex.Replace(attributes, @"class=['""](.*?)['""]", String.Format(@"class=""{0}""",
																							   m.Groups[1].Value.ToLower().StartsWith("required")
																								? ("required " + inputtype)
																								: m.Groups[1].Value),
										   RegexOptions.IgnoreCase | RegexOptions.Multiline);

			return attributes;
		}

		private static string EmptyIfZero(int countAppendage)
		{
			return (countAppendage > 0 ? Convert.ToString(countAppendage) : "");
		}

		/// <summary>
		/// This method checks the Content Manager page for form fields.
		/// If it finds form fields it will submit them to the form recipient.
		/// </summary>
		public static void ParseRequestForFormFields(string regionName)
		{
			if (String.IsNullOrEmpty(HttpContext.Current.Request.Form[DynamicSubmitName])) return;
			int? micrositeID = null;
			bool globalContact = false;
			CMPage cmPage = CMSHelpers.GetCurrentRequestCMSPage();

			// determine legit fields
			List<CMPageRegion> prs = new List<CMPageRegion>();
			int? userID = Helpers.GetCurrentUserID();
			if (userID == 0)
				userID = null;
			if (cmPage != null)
			{
				CMRegion cmRegion = CMRegion.CMRegionPage(0, 1, "", "", true, new CMRegion.Filters { FilterCMRegionName = regionName }).FirstOrDefault();
				if (cmRegion != null)
				{
					CMPageRegion currentRegion = CMPageRegion.LoadContentRegion(new CMPageRegion.Filters { FilterCMPageRegionCMRegionID = cmRegion.CMRegionID.ToString(), FilterCMPageRegionUserID = userID.ToString(), FilterCMPageRegionCMPageID = cmPage.CMPageID.ToString(), FilterCMPageRegionNeedsApproval = false.ToString() });
					if (currentRegion != null)
						prs.Add(currentRegion);
				}
			}

			//Also get Global areas that might contain forms
			List<CMPage> globalAreas = CMSHelpers.GetCachedCMPages().Where(c => !c.CMTemplateID.HasValue && c.FileName.Equals(regionName)).ToList();
			List<CMPage> temp = new List<CMPage>();
			temp.AddRange(globalAreas);
			foreach (CMPage globalPage in temp)
			{
				CMRegion cmRegion = CMRegion.CMRegionGetByName(regionName).FirstOrDefault();
				if (cmRegion != null)
				{
					CMPageRegion region = CMPageRegion.LoadContentRegion(new CMPageRegion.Filters { FilterCMPageRegionCMRegionID = cmRegion.CMRegionID.ToString(), FilterCMPageRegionUserID = userID.ToString(), FilterCMPageRegionCMPageID = globalPage.CMPageID.ToString(), FilterCMPageRegionNeedsApproval = false.ToString() });
					if (region != null)
					{
						prs.Clear();
						prs.Add(region);
					}
					else
						globalAreas.Remove(globalPage);
				}
				else
					globalAreas.Remove(globalPage);
			}
			if (prs.Count > 0)
			{
				bool hasFields = false;

				List<string> validFields = new List<string>();
				List<string> checkBoxes = new List<string>();

				foreach (CMPageRegion pr in prs)
				{
					MatchCollection ms = Regex.Matches(pr.Content, @"<(input|textarea|select) (.|\n)*?name=""?((\w|\d|\s|\-|\(|\))+)""?(.|\n)*?/?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
					if (ms.Count > 0 && globalAreas.Exists(c => c.CMPageID == pr.CMPageID))
					{
						cmPage = globalAreas.Find(c => c.CMPageID == pr.CMPageID);
						globalContact = true;
					}
					foreach (Match m in ms)
					{
						if (!m.ToString().Contains("type=\"radio\"") || !validFields.Contains(m.Groups[3].Value))
							validFields.Add(m.Groups[3].Value);
						if (m.ToString().Contains("type=\"checkbox\""))
							checkBoxes.Add(m.Groups[3].Value);
					}
				}

				validFields.Remove("dynamicsubmit");

				CMSubmittedForm newForm = new CMSubmittedForm();
				newForm.IsProcessed = false;
				newForm.DateSubmitted = DateTime.UtcNow;
				newForm.FormRecipient = cmPage.FormRecipient;
				newForm.ResponsePageID = cmPage.ResponsePageID;
				newForm.CMMicrositeID = cmPage.CMMicrositeID;

				if (HttpContext.Current.Request.Files.Count > 0)
				{
					if (Regex.IsMatch(HttpContext.Current.Request.Files[0].FileName, "(\\.(doc)|(docx)|(pdf)|(jpg)|(jpeg)|(bmp)|(png)|(gif)|(ppt)|(pptx)|(xls)|(xlsx))$"))
					{
						HttpContext.Current.Request.Files[0].SaveAs(HttpContext.Current.Server.MapPath(UploadedFilesLocation + HttpContext.Current.Request.Files[0].FileName));
						newForm.UploadedFile = HttpContext.Current.Request.Files[0].FileName;
					}
					else
					{
						Page page = (Page)HttpContext.Current.Handler;
						page.ClientScript.RegisterStartupScript(page.GetType(), "InvalidFileExt", "alert('Invalid file extension.  Valid extensions are: doc,docx,pdf,jpg,jpeg,bmp,png,gif,ppt,pptx,xls,xlsx');", true);
						return;
					}
				}

				if (validFields.Count > 0)
				{
					StringBuilder formData = new StringBuilder();
					validFields.ForEach(s =>
											{
												if (HttpContext.Current.Request.Form[s] != null)
												{
													formData.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", s, HttpContext.Current.Request.Form[s].ToString()));
													if (!hasFields)
														hasFields = true;
												} // if the item is not posted, no harm, just dont include it
												else if (checkBoxes.Contains(s))
												{
													formData.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", s, "off"));
													if (!hasFields)
														hasFields = true;
												}
											});
					if (hasFields)
					{
						string body = EmailTemplateService.HtmlMessageBody(EmailTemplates.CMSFormPost, new { PageName = cmPage.FileName, FormFields = formData.ToString() });

						newForm.FormHTML = body;
						newForm.Save();
						if (globalContact && !String.IsNullOrEmpty(Settings.GlobalContactEmailAddress))
						{
							MailMessage message = new MailMessage();
							message.To.Add(Settings.GlobalContactEmailAddress);
							message.IsBodyHtml = true;
							message.Body = body;
							message.Subject = Globals.Settings.SiteTitle + "- Form Submission From " + cmPage.FileName;
							if (!String.IsNullOrEmpty(newForm.UploadedFile))
								message.Attachments.Add(new Attachment(HttpContext.Current.Server.MapPath(UploadedFilesLocation + newForm.UploadedFile)));
							SmtpClient client = new SmtpClient();
							client.Send(message);
						}
						else if (!String.IsNullOrEmpty(cmPage.FormRecipient))
						{
							cmPage.FormRecipient.Split(',').ToList().ForEach(recipient =>
																				{
																					MailMessage message = new MailMessage();
																					message.To.Add(recipient);
																					message.IsBodyHtml = true;
																					message.Body = body;
																					message.Subject = Globals.Settings.SiteTitle + "- Form Submission From " + cmPage.FileName;
																					if (!String.IsNullOrEmpty(newForm.UploadedFile))
																						message.Attachments.Add(new Attachment(HttpContext.Current.Server.MapPath(UploadedFilesLocation + newForm.UploadedFile)));
																					SmtpClient client = new SmtpClient();
																					client.Send(message);
																				});
						}
					}
					if (hasFields)
					{
						if (globalContact)
						{
							Page page = (Page)HttpContext.Current.Handler;
							page.ClientScript.RegisterStartupScript(page.GetType(), PopupScriptName, @"$(document).ready(function(){
	if ($('a#contactDummyLink').length == 0)
		$('div.contactSuccess').parent().parent().prepend('<a href=""#contactSuccess"" style=""display:none;"" id=""contactDummyLink"">success</a>');
	$('a#contactDummyLink').fancybox();
	$('a#contactDummyLink').trigger('click');
	setTimeout(function(){$.fancybox.close();}, 4000);
});", true);
						}
						else if (cmPage.ResponsePageID != null)
							HttpContext.Current.Response.Redirect(CMSHelpers.GetCachedCMPages().Where(p => p.CMPageID == cmPage.ResponsePageID.Value).Single().FileName);
						else
						{
							Page page = (Page)HttpContext.Current.Handler;
							//If you change the key or type of the script below, 
							//you must also change it on the _RadEditor.cs file or your page will not load correctly
							//in the if statement with Page.ClientScript.IsStartupScriptRegistered(Page.GetType(), "PopupScript")
							page.ClientScript.RegisterStartupScript(page.GetType(), PopupScriptName, "alert('Thank you. Your form has been submitted successfully.');", true);
						}
					}
				}
			}
		}
	}
}