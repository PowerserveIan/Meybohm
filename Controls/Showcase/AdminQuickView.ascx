<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminQuickView.ascx.cs" Inherits="Controls_Showcase_AdminQuickView" %>
<%@ Import Namespace="Classes.Showcase" %>
<%@ Register TagPrefix="Controls" TagName="BaseAdminQuickView" Src="~/Controls/BaseControls/BaseAdminQuickView.ascx" %>
<Controls:BaseAdminQuickView runat="server" ID="uxAdminQuickView" ComponentName="Showcase" ComponentFolderLocation="~/admin/showcase/">
	<ContentAreaTemplate>
		<h4>Most Popular Showcase Items (last 30 days)</h4>
		<table class="subhead">
			<!--<asp:Repeater runat="server" ID="uxMostPopularItems" ItemType="Classes.Showcase.ShowcaseItemMetric">
				<HeaderTemplate>
					<thead>
						<tr>
							<th>
								Title
							</th>
							<th style="text-align: right;">
								# of Clicks
							</th>
							<th style="text-align: right;">
								% of Total Clicks
							</th>
						</tr>
					</thead>
					<tbody>
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
						<td>
							<%#Item.ShowcaseItemTitle%>
						</td>
						<td style="text-align: right;">
							<%#Item.Count%>
						</td>
						<td style="text-align: right;">
							<%#Item.Percentage.ToString("P")%>
						</td>
					</tr>
				</ItemTemplate>
				<FooterTemplate></tbody></FooterTemplate>
			</asp:Repeater>-->
			<asp:Literal runat="server" ID="uxNoItemsClicked" Text="<tbody><tr><td>No users have accessed your Showcase Items yet.</td></tr>" Visible="false"></asp:Literal>
		</table>
	</ContentAreaTemplate>
</Controls:BaseAdminQuickView>
