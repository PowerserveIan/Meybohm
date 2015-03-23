<% bool open = Request.Path.ToLower().Contains("newsletters"); %>
<% if (Page.User.IsInRole("Admin") || Page.User.IsInRole("Newsletter Admin")){%>
<li class="parent<%= open ? " current" : "" %>"><span>Newsletters</span>
	<ul>
		<li>
			<a runat="server" href="~/admin/newsletters/admin-newsletter.aspx">Newsletter Manager</a></li>
		<li>
			<a runat="server" href="~/admin/newsletters/admin-newsletter-metrics.aspx">Newsletter Statistics</a></li>
		<li>
			<a runat="server" href="~/admin/newsletters/admin-mailing-list.aspx">Mailing List Manager</a></li>
		<%--<li><a runat="server" href="~/admin/newsletters/admin-newsletter-category.aspx">Category Manager</a></li>--%>
		<li>
			<a runat="server" href="~/admin/newsletters/configuration-settings.aspx">Settings</a></li>
	</ul>
</li>
<% } %>
