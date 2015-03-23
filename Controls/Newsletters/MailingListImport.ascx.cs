using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using BaseCode;
using Classes.Newsletters;

public partial class Admin_Newsletters_MailingListImport : UserControl
{
	private string m_FileFolder = string.Empty;
	private string m_FileToImport = string.Empty;

	public bool BlockSubscribers { get; set; }

	public int? MailingListID
	{
		get { return (int?)ViewState["MailingListID"]; }
		set { ViewState["MailingListID"] = value; }
	}

	public string FileToImport
	{
		get { return m_FileToImport; }
		set { m_FileToImport = value; }
	}

	public string FileFolder
	{
		get { return m_FileFolder; }
		set { m_FileFolder = value; }
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxFileImportPH.Visible = !BlockSubscribers;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		uxImport.Click += uxImport_Click;
		uxRedo.Click += uxRedo_Click;
		uxImportMore.Click += uxImportMore_Click;
	}

	private void uxImportMore_Click(object sender, EventArgs e)
	{
		uxSuccessPH.Visible = false;
		uxFileImportPH.Visible = true;
	}

	private void uxRedo_Click(object sender, EventArgs e)
	{
		uxErrorsPH.Visible = false;
		uxFileImportPH.Visible = true;
	}

	private void ValidateFileUpload()
	{
		if (uxFileUpload.HasFile)
		{
			if (!uxFileUpload.FileName.EndsWith(".csv"))
				uxFileUploadCV.IsValid = false;
		}
		else
		{
			uxFileUploadCV.IsValid = false;
			uxFileUploadCV.ErrorMessage = "*No file selected, please choose a .csv file to import.";
		}
	}

	private void uxImport_Click(object sender, EventArgs e)
	{
		ValidateFileUpload();
		if (Page.IsValid)
		{
			FileFolder = Server.MapPath("~") + "/admin/newsletters/Mailing_Lists";
			string forConcurrency = DateTime.UtcNow.ToShortDateString().Replace('\\', '-').Replace('/', '-') + " " + DateTime.UtcNow.ToLongTimeString().Replace(" ", "").Replace(":", "-");
			uxFileUpload.SaveAs(FileFolder + "/" + forConcurrency + uxFileUpload.FileName);
			FileToImport = forConcurrency + uxFileUpload.FileName;

			DataSet ds = new DataSet();
			StreamReader reader = new StreamReader(FileFolder + "/" + FileToImport);
			ds.Tables.Add(CsvParser.Parse(reader));
			ds.Tables[0].TableName = "MailingLists";

			ds.Tables["MailingLists"].Columns[0].ColumnName = "SubscriberEmail";
			ds.Tables["MailingLists"].Columns[1].ColumnName = "Format";
			ds.Tables["MailingLists"].Columns[2].ColumnName = "Active";

			ds.Tables.Add("NotImported");
			ds.Tables["NotImported"].Columns.Add("SubscriberEmail");
			ds.Tables["NotImported"].Columns.Add("Reason");

			if (ds.Tables["MailingLists"].Rows.Count > 1) //Contains more than Header Row
			{
				if (MailingListID != null)
				{
					List<string> listSubscribersEmails = MailingListSubscriber.GetMailingListSubscriberEmails((int)MailingListID);
					List<NewsletterFormat> listFormats = NewsletterFormat.GetAll();

					string emailRegEx = Helpers.EmailValidationExpression;
					Regex regEx = new Regex(emailRegEx);
					//Check each row in the .csv file
					foreach (DataRow drCurrent in ds.Tables["MailingLists"].Rows)
					{
						//Ignore Header Row
						if (drCurrent == ds.Tables["MailingLists"].Rows[0])
							continue;
						//Check to make sure email is valid
						if (!regEx.IsMatch(drCurrent["SubscriberEmail"].ToString().Trim()))
						{
							DataRow dr = ds.Tables["NotImported"].NewRow();
							dr["SubscriberEmail"] = drCurrent["SubscriberEmail"].ToString();
							dr["Reason"] = "Invalid email address format";
							ds.Tables["NotImported"].Rows.Add(dr);
						}
						else
						{
							//Check to see if the subscriber is already in the DB
							bool subscriberExists = listSubscribersEmails.Exists(s=>s.Equals(drCurrent["SubscriberEmail"].ToString(), StringComparison.OrdinalIgnoreCase));

							if (subscriberExists)
							{
								DataRow dr = ds.Tables["NotImported"].NewRow();
								dr["SubscriberEmail"] = drCurrent["SubscriberEmail"].ToString();
								dr["Reason"] = "Already in mailing list";
								ds.Tables["NotImported"].Rows.Add(dr);
							}
							else //Subscriber is not in DB
							{
								//Check if the Newsletter Format is already in the DB
								NewsletterFormat currentFormat = listFormats.Find(f=>f.Name.Equals(drCurrent["Format"].ToString(), StringComparison.OrdinalIgnoreCase));
								MailingListSubscriber mlSubscriber = new MailingListSubscriber();
								Subscriber newSubscriber = new Subscriber();

								if (currentFormat != null)
								{
									mlSubscriber.MailingListID = (int)MailingListID;
									if (drCurrent["Active"].ToString() == "1" || drCurrent["Active"].ToString().ToUpper() == "TRUE" || drCurrent["Active"].ToString().ToUpper() == "T")
										mlSubscriber.Active = true;
									else if (drCurrent["Active"].ToString() == "0" || drCurrent["Active"].ToString().ToUpper() == "FALSE" || drCurrent["Active"].ToString().ToUpper() == "F")
										mlSubscriber.Active = false;
									else
									{
										DataRow dr = ds.Tables["NotImported"].NewRow();
										dr["SubscriberEmail"] = drCurrent["SubscriberEmail"].ToString();
										dr["Reason"] = "Active column is set to " + drCurrent["Active"] + ", which is not a valid True/False format";
										ds.Tables["NotImported"].Rows.Add(dr);
									}

									newSubscriber.Email = drCurrent["SubscriberEmail"].ToString();
									newSubscriber.Deleted = false;
									newSubscriber.DefaultNewsletterFormatID = currentFormat.NewsletterFormatID;
									mlSubscriber.NewsletterFormatID = currentFormat.NewsletterFormatID;

									if (!Settings.EnableMailingListLimitations || (Settings.EnableMailingListLimitations && listSubscribersEmails.Count < Settings.MaxNumberSubscribers))
									{
										newSubscriber.CustomInsert();
										//Subscriber has now been added to the DB
										mlSubscriber.SubscriberID = newSubscriber.SubscriberID;
										mlSubscriber.EntityID = Guid.NewGuid();
										mlSubscriber.Save();
										listSubscribersEmails.Add(newSubscriber.Email);
										//MailingListSubscriber has now been added to the DB 
									}
									else //Over the limit for Number of Subscribers
									{
										//add row to error table
										DataRow dr = ds.Tables["NotImported"].NewRow();
										dr["SubscriberEmail"] = drCurrent["SubscriberEmail"].ToString();
										dr["Reason"] = "You are over the limit for number of subscribers per mailing list";
										ds.Tables["NotImported"].Rows.Add(dr);
									}
								}
								else //Format is not in DB
								{
									//add row to error table
									DataRow dr = ds.Tables["NotImported"].NewRow();
									dr["SubscriberEmail"] = drCurrent["SubscriberEmail"].ToString();
									dr["Reason"] = drCurrent["Format"] + " is not a valid format";
									ds.Tables["NotImported"].Rows.Add(dr);
								}
							}
						}
					}
				}
			}
			if (ds.Tables["NotImported"].Rows.Count > 0)
			{
				uxErrorsPH.Visible = true;
				int numRowsInserted = ds.Tables["MailingLists"].Rows.Count - ds.Tables["NotImported"].Rows.Count;
				uxNumErrorsLbl.Text = numRowsInserted + " row(s) were successfully inserted, and <br />" + ds.Tables["NotImported"].Rows.Count + " row(s) out of " + ds.Tables["MailingLists"].Rows.Count + " total rows were not inserted:";
				uxErrorsRepeater.DataSource = ds.Tables["NotImported"];
				uxErrorsRepeater.DataBind();
			}
			else if (ds.Tables["MailingLists"].Rows.Count <= 1)
			{
				uxSuccessLbl.Text = "Nothing to import from CSV file, please check to ensure it has column headers followed by rows of email addresses<br />";
				uxSuccessPH.Visible = true;
			}
			else
			{
				uxSuccessLbl.Text = "Success!  " + ds.Tables["MailingLists"].Rows.Count + " rows were inserted";
				uxSuccessPH.Visible = true;
				uxFileImportPH.Visible = false;
			}
			uxFileImportPH.Visible = false;

			Page.DataBind();
		}
	}
}