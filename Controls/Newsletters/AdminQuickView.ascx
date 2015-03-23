<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminQuickView.ascx.cs" Inherits="Controls_Newsletters_AdminQuickView" %>
<%@ Register TagPrefix="Controls" TagName="BaseAdminQuickView" Src="~/Controls/BaseControls/BaseAdminQuickView.ascx" %>
<Controls:BaseAdminQuickView runat="server" ID="uxAdminQuickView" ComponentName="Newsletters" ComponentFolderLocation="~/admin/newsletters/">
	<ContentAreaTemplate>
		<h4>Last Mailout</h4>
		<asp:PlaceHolder runat="server" ID="uxLastMailoutPH">
			<table class="subhead">
				<tbody>
					<tr>
						<td>
							Date
						</td>
						<td>
							<asp:Literal runat="server" ID="uxLastSentDate"></asp:Literal>
							<br />
							<asp:HyperLink ID="uxViewLink" runat="server" Text="View" CssClass="fancybox.iframe detailbox" NavigateUrl="~/newsletter-details.aspx?iframe&adminView=true&mailoutId=" />
							|
							<asp:HyperLink ID="uxStatsLink" runat="server" Text="Statistics" CssClass="fancybox.iframe statbox" NavigateUrl="~/admin/newsletters/admin-mailout-metrics.aspx?iframe&mailoutId=" />
						</td>
					</tr>
					<tr>
						<td>
							Mailing Lists
						</td>
						<td>
							<asp:Literal runat="server" ID="uxMailingLists"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td>
							Sent
						</td>
						<td>
							<asp:Literal runat="server" ID="uxSendCount"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td>
							Not Sent
						</td>
						<td>
							<asp:Literal runat="server" ID="uxNotSentCount"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td>
							Opened
						</td>
						<td>
							<asp:Literal runat="server" ID="uxOpenCount"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td>
							Link Clicks
						</td>
						<td>
							<asp:Literal runat="server" ID="uxClickCount"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td>
							Forwards
						</td>
						<td>
							<asp:Literal runat="server" ID="uxForwardCount"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td width="150">
							Unsubscriptions
						</td>
						<td>
							<asp:Literal runat="server" ID="uxUnsubscribeCount"></asp:Literal>
						</td>
					</tr>
				</tbody>
			</table>
			<script type="text/javascript">
				//<![CDATA[
				$(document).ready(function () {
					$("a.statbox").fancybox({
						'width': 600,
						'height': 400,
						'closeClick': false
					});
					$("a.detailbox").fancybox({
						'width': 550,
						'height': 600,
						'padding': 0,
						'closeClick': false
					});
				});//]]>
			</script>
		</asp:PlaceHolder>
		<asp:Literal runat="server" ID="uxNoNewslettersSent" Text="You have not sent any newsletters yet!" Visible="false"></asp:Literal>
	</ContentAreaTemplate>
</Controls:BaseAdminQuickView>
