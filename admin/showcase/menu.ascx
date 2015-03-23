<% bool open = Request.Path.ToLower().Contains("showcase"); %>
<% if (Page.User.IsInRole("Admin") || Page.User.IsInRole("Showcase Admin") || Page.User.IsInRole("Showcase Manager")){%>
<li class="parent<%= open ? " current" : "" %>"><span>Properties</span>
	<ul>
		<% if (Classes.Showcase.Settings.MultipleShowcases && (Classes.Showcase.ShowcaseHelpers.UserCanManageOtherShowcases() || Classes.Showcase.ShowcaseHelpers.GetCurrentShowcaseID() == null)){ %>
		<li>
			<a runat="server" href="~/admin/showcase/admin-showcases.aspx">Showcase Manager</a></li>
		<% } %>
		<% if (Classes.Showcase.ShowcaseHelpers.GetCurrentShowcaseID() != null){ %>
		<% if (Classes.Showcase.ShowcaseHelpers.IsCurrentShowcaseMLS()){ %>
		<li>
			<a runat="server" href="~/admin/showcase/admin-attribute-list.aspx">MLS Attribute List</a></li>
		<% } %>
		<li>
			<a runat="server" href="~/admin/showcase/admin-attribute.aspx">Attribute Manager</a></li>
		<li>
			<a runat="server" href="~/admin/showcase/admin-showcase-item.aspx">Property Manager</a></li>
		<li>
			<a runat="server" href="~/admin/showcase/configuration-settings.aspx">Settings</a></li>
		<% } %>
		<li>
			<a runat="server" href="~/admin/showcase/property-inventory.aspx">Property Inventory and Statistics</a></li>
	</ul>
</li>
<% } %>
