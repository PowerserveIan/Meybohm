<% bool open = Request.Path.ToLower().Contains("contacts"); %>
<% if (Page.User.IsInRole("Admin")){%>
<li class="parent<%= open ? " current" : "" %>"><span>Contacts</span>
	<ul>
		<li<%= Request.QueryString["FilterContactContactTypeID"] == "1" ? " class='current'": "" %>>
			<a runat="server" href="~/admin/contacts/admin-contact.aspx?FilterContactContactTypeID=1">Contact Submissions</a></li>
		<li<%= Request.QueryString["FilterContactContactTypeID"] == "2" ? " class='current'": "" %>>
			<a runat="server" href="~/admin/contacts/admin-contact.aspx?FilterContactContactTypeID=2">Home Valuation Requests</a></li>
		<li<%= Request.QueryString["FilterContactContactTypeID"] == "3" ? " class='current'": "" %>>
			<a runat="server" href="~/admin/contacts/admin-contact.aspx?FilterContactContactTypeID=3">Maintenance Requests</a></li>
		<li<%= Request.QueryString["FilterContactContactTypeID"] == "4" ? " class='current'": "" %>>
			<a runat="server" href="~/admin/contacts/admin-contact.aspx?FilterContactContactTypeID=4">Property Information Requests</a></li>
		<li<%= Request.QueryString["FilterContactContactTypeID"] == "5" ? " class='current'": "" %>>
			<a runat="server" href="~/admin/contacts/admin-contact.aspx?FilterContactContactTypeID=5">Agent Requests</a></li>
		<li>
			<a runat="server" href="~/admin/contacts/configuration-settings.aspx">Settings</a></li>
	</ul>
</li>
<%} %>
