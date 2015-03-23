<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminQuickView.ascx.cs" Inherits="Controls_Media352_MembershipProvider_AdminQuickView" %>
<%@ Import Namespace="Classes.Media352_MembershipProvider" %>
<%@ Register TagPrefix="Controls" TagName="BaseAdminQuickView" Src="~/Controls/BaseControls/BaseAdminQuickView.ascx" %>
<Controls:BaseAdminQuickView runat="server" ID="uxAdminQuickView" ComponentName="Site Users" ComponentFolderLocation="~/admin/media352-membership-provider/">
	<ContentAreaTemplate>
		<h4>Users Requiring Approval</h4>
		<table class="subhead">
			<asp:Repeater runat="server" ID="uxUsersRequiringApproval" ItemType="Classes.Media352_MembershipProvider.User">
				<HeaderTemplate>
					<thead>
						<tr>
							<th>
								Approve User
							</th>
							<th>
								User Name
							</th>
						</tr>
					</thead>
					<tbody>
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
						<td>
							<asp:LinkButton ID="uxApprove" runat="server" OnCommand="Item_Command" CommandName="Approve" CommandArgument='<%#Item.UserID%>' OnClientClick="return confirm('Are you sure you want to approve this user?');" CssClass="icon unlock noText"
								Text="Approve" />
						</td>
						<td>
							<asp:HyperLink runat="server" ID="uxUserLink" NavigateUrl='<%#"~/admin/media352-membership-provider/admin-user-edit.aspx?id=" + Item.UserID%>' Text="<%#Item.Name%>"></asp:HyperLink>
						</td>
					</tr>
				</ItemTemplate>
			</asp:Repeater>
			<asp:Literal runat="server" ID="uxNoUnapprovedUsers" Text="<tbody><tr><td>There are no users needing approval.</td></tr>" Visible="false"></asp:Literal>
			</tbody>
		</table>
		<h4>The <%#NumberOfMostRecentUsers%> most recent users to join your site</h4>
		<table class="subhead">
			<tbody>
				<asp:Repeater runat="server" ID="uxMostRecentUsers" ItemType="Classes.Media352_MembershipProvider.User">
					<ItemTemplate>
						<tr>
							<td>
								<asp:HyperLink runat="server" ID="uxUserLink" NavigateUrl='<%#"~/admin/media352-membership-provider/admin-user-edit.aspx?id=" + Item.UserID%>' Text="<%#Item.Name%>"></asp:HyperLink>
							</td>
						</tr>
					</ItemTemplate>
					<FooterTemplate>
						</table>
					</FooterTemplate>
				</asp:Repeater>
				<asp:Literal runat="server" ID="uxNoUsersOnSite" Text="<tr><td>No users have joined your site.</td></tr>" Visible="false"></asp:Literal>
			</tbody>
		</table>
	</ContentAreaTemplate>
</Controls:BaseAdminQuickView>
