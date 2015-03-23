<% bool open = Request.Path.ToLower().Contains("content-manager"); %>
<% if (Page.User.IsInRole("Admin") || Page.User.IsInRole("CMS Admin") || Page.User.IsInRole("CMS Content Integrator") || Page.User.IsInRole("CMS Page Manager") || (Classes.ContentManager.Settings.EnableMicrosites && Page.User.IsInRole("Microsite Admin"))){%>
<li class="parent<%= open ? " current" : "" %>"><span>Content Manager</span>
	<ul>
		<% if ((Page.User.IsInRole("Admin") || Page.User.IsInRole("CMS Admin")) && Classes.ContentManager.Settings.EnableApprovals){ %>
		<li>
			<a runat="server" href="~/admin/content-manager/approval-alerts.aspx">Manage Approvals</a></li>
		<% } %>
		<li>
			<a runat="server" href="~/admin/content-manager/content-manager.aspx">
				<% if (Page.User.IsInRole("Admin") || Page.User.IsInRole("CMS Admin") || Page.User.IsInRole("CMS Content Integrator") || (Classes.ContentManager.Settings.AllowMicrositeAdminToEditSitemap && Page.User.IsInRole("Microsite Admin"))){%>Create &amp;<% } %>
				Edit Pages</a></li>
		<% if (Page.User.IsInRole("Admin") || Page.User.IsInRole("CMS Admin") || Page.User.IsInRole("CMS Content Integrator") || (Classes.ContentManager.Settings.AllowMicrositeAdminToEditSitemap && Page.User.IsInRole("Microsite Admin"))){%>
		<li>
			<a runat="server" href="~/admin/content-manager/sitemap.aspx">Manage Site Map</a></li>
		<% } %>
		<% if (Classes.ContentManager.Settings.EnableMicrosites && (Page.User.IsInRole("Admin") || Page.User.IsInRole("CMS Admin"))){%>
		<li>
			<a runat="server" href="~/admin/content-manager/admin-cm-microsite.aspx">Microsite Manager</a></li>
		<% } %>
		<% if (!Page.User.IsInRole("CMS Page Manager"))
	   { %>
		<li>
			<a runat="server" href="~/admin/content-manager/admin-cm-submitted-form.aspx">Submitted Form Manager</a></li>
		<% }if ((Page.User.IsInRole("Admin") || Page.User.IsInRole("CMS Admin")) && Classes.ContentManager.Settings.EnableCMPageRoles){%>
		<li>
			<a runat="server" href="~/admin/content-manager/admin-cm-role.aspx">CM Role Manager</a></li>
		<% }if ((Page.User.IsInRole("Admin") || Page.User.IsInRole("CMS Admin")) && Classes.ContentManager.Settings.CMSConfigSettingsExist){%>
		<li>
			<a runat="server" href="~/admin/content-manager/configuration-settings.aspx">Settings</a></li>
		<% } %>
	</ul>
</li>
<% } %>
