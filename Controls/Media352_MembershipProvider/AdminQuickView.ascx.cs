using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Classes.Media352_MembershipProvider;

public partial class Controls_Media352_MembershipProvider_AdminQuickView : UserControl
{
	private int m_NumberOfMostRecentUsers = 5;

	public int NumberOfMostRecentUsers
	{
		get { return m_NumberOfMostRecentUsers; }
		set { m_NumberOfMostRecentUsers = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxAdminQuickView.ComponentVersionNumber = Settings.VersionNumber;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
			BindData();
	}

	private void BindData()
	{
		PlaceHolder uxContentArea = (PlaceHolder)uxAdminQuickView.FindControl("uxContentArea");
		Repeater uxUsersRequiringApproval = (Repeater)uxContentArea.Controls[0].FindControl("uxUsersRequiringApproval");
		Repeater uxMostRecentUsers = (Repeater)uxContentArea.Controls[0].FindControl("uxMostRecentUsers");
		List<User> unapprovedUsers = User.UserGetByIsApproved(false);
		if (unapprovedUsers.Count > 0)
		{
			uxUsersRequiringApproval.DataSource = unapprovedUsers;
			uxUsersRequiringApproval.DataBind();
		}
		else
		{
			uxUsersRequiringApproval.Visible = false;
			((Literal)uxContentArea.Controls[0].FindControl("uxNoUnapprovedUsers")).Visible = true;
		}
		List<User> newestUsers = User.UserPage(0, NumberOfMostRecentUsers, "", "Created", false);
		if (newestUsers.Count > 0)
		{
			uxMostRecentUsers.DataSource = newestUsers;
			uxMostRecentUsers.DataBind();
		}
		else
		{
			uxMostRecentUsers.Visible = false;
			((Literal)uxContentArea.Controls[0].FindControl("uxNoUsersOnSite")).Visible = true;
		}
	}

	protected void Item_Command(object sender, CommandEventArgs e)
	{
		if (e.CommandName == "Approve")
		{
			User currentUser = User.GetByID(Convert.ToInt32(e.CommandArgument.ToString()));
			if (currentUser != null)
			{
				currentUser.IsApproved = true;
				currentUser.Save();
			}
			BindData();
		}
	}
}