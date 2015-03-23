<% bool open = Request.Path.ToLower().Contains("whats-near-by"); %>
<% if (Page.User.IsInRole("Admin")){%>
<li class="parent<%= open ? " current" : "" %>"><span>What's Near By</span>
	<ul>
		<li>
			<a runat="server" href="~/admin/whats-near-by/admin-whats-near-by-category.aspx">Manage Categories</a></li>
		<li>
			<a runat="server" href="~/admin/whats-near-by/admin-whats-near-by-location.aspx">Manage Locations</a></li>
	</ul>
</li>
<%} %>
