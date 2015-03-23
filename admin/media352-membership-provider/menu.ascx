<% bool open = Request.Path.ToLower().Contains("media352-membership-provider"); %>
<% if (Page.User.IsInRole("Admin")){%>
<li class="parent<%= open ? " current" : "" %>"><span>Site Users</span>
	<ul>
		<li<%= Request.QueryString["FilterUserHasRole"] == "false" ? " class='current'": "" %>>
			<a runat="server" href="~/admin/media352-membership-provider/admin-user.aspx?FilterUserHasRole=false">Customer Manager</a></li>
		<li<%= Request.QueryString["FilterUserHasRole"] == "true" && String.IsNullOrEmpty(Request.QueryString["FilterUserRoleName"]) ? " class='current'": "" %>>
			<a runat="server" href="~/admin/media352-membership-provider/admin-user.aspx?FilterUserHasRole=true">Staff Manager</a></li>
		<li<%= Request.QueryString["FilterUserRoleName"] == "Agent" ? " class='current'": "" %>>
			<a runat="server" href="~/admin/media352-membership-provider/admin-user.aspx?FilterUserHasRole=true&FilterUserRoleName=Agent">Agent Manager</a></li>
		<% if (Classes.Media352_MembershipProvider.Settings.SecurityQuestionRequired)
	   { %>
		<li>
			<a runat="server" href="~/admin/media352-membership-provider/admin-security-question.aspx">Security Question Manager</a></li>
		<% } %>
		<li>
			<a runat="server" href="~/admin/media352-membership-provider/admin-team.aspx">Team Manager</a></li>
		<li>
			<a runat="server" href="~/admin/media352-membership-provider/admin-designation.aspx">Designation Manager</a></li>
		<li>
			<a runat="server" href="~/admin/media352-membership-provider/configuration-settings.aspx">Settings</a></li>
	</ul>
</li>
<% } %>
