<% bool open = Request.Path.ToLower().Contains("m-l-s"); %>
<% if (Page.User.IsInRole("Admin") || Page.User.IsInRole("New Homes Admin")){%>
<li class="parent<%= open ? " current" : "" %>"><span>MLS</span>
	<ul>
		<% if (Page.User.IsInRole("Admin")){%>
		<li>
			<a runat="server" href="~/admin/m-l-s/admin-builder.aspx">Manage Builder</a></li><%} %>
		<li>
			<a runat="server" href="~/admin/m-l-s/admin-neighborhood.aspx">Manage Neighborhood</a></li>
		<% if (Page.User.IsInRole("Admin")){%>
		<li>
			<a runat="server" href="~/admin/m-l-s/admin-office.aspx">Manage Office</a></li><%} %>
	</ul>
</li>
<%} %>
