<% bool open = Request.Path.ToLower().Contains("media352-news-press"); %>
<% if (Page.User.IsInRole("Admin") || Page.User.IsInRole("Newspress Admin")){%>
<li class="parent<%= open ? " current" : "" %>"><span>News Press</span>
	<ul>
		<li>
			<a runat="server" href="~/admin/media352-news-press/admin-news-press.aspx">News Press Manager</a></li>
		<% if (Classes.Media352_NewsPress.Settings.EnableCategories)
	   { %>
		<li>
			<a runat="server" href="~/admin/media352-news-press/admin-news-press-category.aspx">News Press Category Manager</a></li>
		<% } %>
		<li>
			<a runat="server" href="~/admin/media352-news-press/configuration-settings.aspx">Settings</a></li>
	</ul>
</li>
<% } %>
