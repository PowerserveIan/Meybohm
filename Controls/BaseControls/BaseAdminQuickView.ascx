<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BaseAdminQuickView.ascx.cs" Inherits="Controls_BaseControls_BaseAdminQuickView" %>
<div class="widget<%= NumberColumnsWide == ColumnsWide.Two ? " twoWide" : (NumberColumnsWide == ColumnsWide.Three ? " threeWide" : "") %> <%= ComponentName.ToLower().Replace("-", "").Replace(" ", "") %>">
	<div class="top">
		<div class="bottom">
			<h3>
				<%= ComponentName %></h3>
			<ul class="popUp manage">
				<asp:PlaceHolder runat="server" ID="uxManagePH">
					<li>
						<a class="icon settings">Manage</a>
						<ul>
							<asp:Literal runat="server" ID="uxManageLinks"></asp:Literal>
						</ul>
					</li>
				</asp:PlaceHolder>
			</ul>
			<asp:PlaceHolder runat="server" ID="uxContentArea"></asp:PlaceHolder>
		</div>
	</div>
</div>
