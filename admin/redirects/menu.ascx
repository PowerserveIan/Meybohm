<% bool open = Request.Path.ToLower().Contains("redirects/"); %>
<% if (Page.User.IsInRole("Admin")){%>
<li class="parent<%= open ? " current" : "" %>"><span>Redirects</span>
	<ul>
		<li>
			<a runat="server" href="~/admin/redirects/admin-redirect.aspx">Manage 301 Redirects</a></li>
	</ul>
</li>
<%} %>
