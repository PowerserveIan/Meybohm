<% bool open = Request.Path.ToLower().Contains("dynamic-header");%>
<% if (Page.User.IsInRole("Admin") || Page.User.IsInRole("DynamicImage Admin")){%>
<li class="parent<%= open ? " current" : "" %>"><span>Dynamic Header</span>
	<ul>
		<li>
			<a runat="server" href="~/admin/dynamic-header/admin-dynamic-image.aspx">Slide Manager</a></li>
		<li>
			<a runat="server" href="~/admin/dynamic-header/admin-dynamic-collection.aspx">Collection Manager</a></li>
		<li>
			<a runat="server" href="~/admin/dynamic-header/configuration-settings.aspx">Settings</a></li>
	</ul>
</li>
<% } %>
