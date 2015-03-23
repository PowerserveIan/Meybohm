using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Newsletters;

public partial class MailingListSubscriberListing : UserControl
{
	public int? MailingListID
	{
		get { return (int?)ViewState["MailingListID"]; }
		set { ViewState["MailingListID"] = value; }
	}

	public bool BlockSubscribers
	{
		get { return (Settings.EnableMailingListLimitations && uxTopPager.TotalRowCount >= Settings.MaxNumberSubscribers); }
	}

	public bool SortDirection
	{
		get
		{
			return ViewState["MLSSortDirection"] == null || Convert.ToBoolean(ViewState["MLSSortDirection"]);
		}
		set { ViewState["MLSSortDirection"] = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		ObjectDataSource uxMailingListSubscribersDataSource = new ObjectDataSource();
		uxMailingListSubscribersDataSource.SelectMethod = "MailingListSubscriberPageBySubscriberEmail";
		uxMailingListSubscribersDataSource.TypeName = "Classes.Newsletters.MailingListSubscriber";
		uxMailingListSubscribersDataSource.EnablePaging = true;
		uxMailingListSubscribersDataSource.SelectCountMethod = "SelectCount";

		uxMailingListSubscribers.DataSource = uxMailingListSubscribersDataSource;

		uxMailingListSubscribersDataSource.Selecting += uxMailingListSubscribersDataSource_Selecting;
		uxMailingListSubscribers.DataBound += uxMailingListSubscribers_DataBound;
		int result;
		if (Request.QueryString[uxTopPager.QueryStringField] == null || !Int32.TryParse(Request.QueryString[uxTopPager.QueryStringField], out result))
			uxTopPager.SetPageProperties(0, Globals.Settings.AdminPageSize, true);
		else
			uxTopPager.SetPageProperties((Convert.ToInt32(Request.QueryString[uxTopPager.QueryStringField]) - 1) * Globals.Settings.AdminPageSize, Globals.Settings.AdminPageSize, true);
		uxExport.Click += uxExport_Click;
	}

	private void uxMailingListSubscribersDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
	{
		MailingListSubscriber.Filters filterList = new MailingListSubscriber.Filters();
		filterList.FilterMailingListSubscriberActive = true.ToString();
		filterList.FilterMailingListSubscriberMailingListID = MailingListID.ToString();
		e.InputParameters["searchText"] = uxSearchText.Text.Trim();
		e.InputParameters["sortField"] = "Email";
		e.InputParameters["sortDirection"] = SortDirection;
		e.InputParameters["filterList"] = filterList;
	}

	private void uxMailingListSubscribers_DataBound(object sender, EventArgs e)
	{
		uxTopPager.Visible = uxTopPager.PageSize < uxTopPager.TotalRowCount;
	}

	public void AllCommands(object sender, CommandEventArgs e)
	{
		switch (e.CommandName)
		{
			case "EnableToggle":
				MailingListSubscriber mls = MailingListSubscriber.GetByID(Convert.ToInt32(e.CommandArgument.ToString()));
				mls.Active = false;
				mls.Save();
				break;
			case "Unsubscribe":
				if (MailingListID.HasValue)
				{
					UnsubscribeUserReturnCode returnCode = NewsletterSystem.UnsubscribeUser(MailingListID.Value, uxEmail.Text);
					if (returnCode == UnsubscribeUserReturnCode.Success)
						uxSuccessPH.Visible = true;
					else
					{
						uxFailureMessage.Visible = true;
						uxFailureMessage.Text = "User does not exist in mailing list";
					}
				}
				break;
			case "Subscribe":
				if (Page.IsValid)
					if (MailingListID.HasValue)
					{
						SubscribeUserReturnCode returnCode = NewsletterSystem.SubscribeUser(MailingListID.Value, uxEmail.Text, 1, null);
						if (returnCode == SubscribeUserReturnCode.Success)
							uxSuccessPH.Visible = true;
						else
						{
							uxFailureMessage.Visible = true;
							uxFailureMessage.Text = "User already subscribed to mailing list";
						}
					}
				break;
			case "EditSubscriber":
				if (MailingListID.HasValue)
					Response.Redirect("~/admin/newsletters/admin-mailing-list-subscriber-edit.aspx?id=" + e.CommandArgument + "&mid=" + MailingListID.Value, true);
				break;
			default:
				throw new Exception("CommandName '" + e.CommandName + "' not supported");
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		uxEmail.Attributes.Add("onFocus", "javascript:this.value='';");
		uxEmailREV.ValidationExpression = Helpers.EmailValidationExpression;
		if (!IsPostBack)
			uxMailingListSubscriberSubscriberIDSortIb.DataBind();
		uxSuccessPH.Visible = false;
		uxFailureMessage.Visible = false;
		uxExport.Visible = MailingListID.HasValue;
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		uxMailingListSubscribers.DataBind();
		Visible = MailingListID != null;
		if (BlockSubscribers)
		{
			uxSubscribeButton.Visible = false;
			uxNoMoreSubscribers.Visible = true;
			uxNoMoreSubscribers.Text = "<h3>" + Settings.MaxNumberSubscribersErrorMessage + "</h3>";
		}
		else
		{
			uxSubscribeButton.Visible = true;
			uxNoMoreSubscribers.Visible = false;
		}
	}

	public void AddConfirmation(object source, EventArgs e)
	{
		((ImageButton)source).Attributes.Add("onclick", "return confirm('Are you sure you would like to delete this item?');");
	}
	
	public string GetSortClasses(string field)
	{
		string cssClass = "sort";
		if (!SortDirection)
			return cssClass + " descending";
		return cssClass + " ascending";
	}

	protected void Sort_Command(object sender, CommandEventArgs e)
	{
		if (e.CommandName == "Sort")
		{
			SortDirection = !SortDirection;
			uxMailingListSubscriberSubscriberIDSortIb.DataBind();
		}
	}

	void uxExport_Click(object sender, EventArgs e)
	{
		CSVWriteHelper.WriteCSVToResponse(MailingListSubscriber.GetSubscriberEmailsForExport(MailingListID.Value), true, Response, "Subscribers");
	}
}