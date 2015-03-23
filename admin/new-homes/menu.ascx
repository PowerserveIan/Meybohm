<% bool open = Request.Path.ToLower().Contains("new-homes"); %>
<% if (Page.User.IsInRole("Admin") || Page.User.IsInRole("New Homes Admin")){%>
<li class="parent<%= open ? " current" : "" %>"><span>New Homes</span>
	<ul>
		<li>
			<a runat="server" href="~/admin/new-homes/new-homes-for-sale.aspx">New Homes For Sale</a></li>
		<li>
			<a runat="server" href="~/admin/new-homes/new-homes-sold.aspx">New Homes Sold</a></li>
		<li>
			<a runat="server" href="~/admin/new-homes/sales-report.aspx">Sales Reports</a></li>		
	</ul>
</li>
<% } %>
