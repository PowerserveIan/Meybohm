<%--Delete this line (comment tags as well) if you do not want the Code generator to overwrite your menu--%>
<% bool open = Request.Path.ToLower().Contains("videos"); %>
<% if (Page.User.IsInRole("Admin")){%>
<li class="parent<%= open ? " current" : "" %>"><span>Videos</span>
	<ul>
		<li>
			<a runat="server" href="~/admin/videos/admin-video.aspx">Manage Video</a></li>
	</ul>
</li>
<%} %>
