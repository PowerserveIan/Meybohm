using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using BaseCode;
//using Controls.SearchComponent;

public partial class admin_DefaultContent : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			uxComponentRepeater.DataSource = new string[] { "All"
//				, "Blog"
				, "Content Manager"
				, "Dynamic Header"
//				, "Ecommerce"
//				, "Events"
//				, "File Library"
//				, "Forum"
				, "News Press"
				, "Newsletters"
//				, "Open Payment"
//				, "Polls"
//				, "Product Catalog"
//				, "Search"
				, "Showcase" 
			};
			uxComponentRepeater.DataBind();
		}
	}

	protected void uxComponent_Command(object sender, CommandEventArgs e)
	{
		if (e.CommandArgument.ToString() == "Search" && e.CommandName != "Delete")
			throw new Exception("You forgot to uncomment the line below me, uncomment the using statement, and remove me");
//		SearchIndexer.IndexAll();
		using (Entities entity = new Entities())
		{
			entity.SiteWide_UpdateDefaultContent((e.CommandArgument.ToString() != "All" ? e.CommandArgument.ToString() : string.Empty), e.CommandName == "Delete");
		}
		Helpers.PurgeCacheItems(null);
	}
}
