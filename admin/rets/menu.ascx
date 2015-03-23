<% bool open = Request.Path.ToLower().Contains("rets"); %>
<% if (Page.User.IsInRole("Admin")){%>
<li class="parent<%= open ? " current" : "" %>"><span>Rets</span>
	<ul>
		<li<%= Request.Path.ToLower().Contains("rets") ? " class='current'": "" %>><a runat="server" href="~/admin/rets/admin-rets-task-status.aspx">Rets Tasks</a></li>
			<li>
			<a runat="server" href="~/admin/rets/configuration-settings.aspx">Settings</a></li>
	</ul>
</li>
<%} %>
