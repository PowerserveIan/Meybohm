<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-newsletter-metrics.aspx.cs" Inherits="Admin_Newsletters_NewsletterMetrics" Title="Admin - Newsletter Metrics" %>

<%@ Register TagPrefix="EmailSender" TagName="Progress" Src="~/Controls/Newsletters/NewsletterProgressWindow.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<EmailSender:Progress runat="server" ID="uxProgressWindow" Visible="false" />
	<asp:PlaceHolder runat="server" ID="uxCustomBreadCrumbsPH">
		<li>
			<a href="admin-newsletter.aspx">Newsletter Listing</a></li>
	</asp:PlaceHolder>
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:PlaceHolder ID="uxResendSuccessMessage" runat="server" Visible="false">
		<h3 class="success">Your mailout was successfully re-sent.</h3>
	</asp:PlaceHolder>
	<asp:Repeater ID="uxBadEmailsRepeater" runat="server" Visible="false" ItemType="Classes.Newsletters.Subscriber">
		<HeaderTemplate>
			<h3 class='failure'>Emails were not sent to the following addresses due to a formatting error:</h3>
			<ul>
		</HeaderTemplate>
		<ItemTemplate>
			<li>
				<asp:Label runat="server" ID="uxErrorText" ForeColor="#CC0000" Text='<%#Item.Email + " is not a valid email address"%> ' />
			</li>
		</ItemTemplate>
		<FooterTemplate>
			</ul>
		</FooterTemplate>
	</asp:Repeater>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder" Visible="false"></asp:PlaceHolder>
	<asp:ListView runat="server" ID="uxNewsletterRpt" ItemPlaceholderID="uxItemPlaceHolder" ItemType="Classes.Newsletters.Newsletter">
		<LayoutTemplate>
			<asp:PlaceHolder runat="server" ID="uxItemPlaceHolder"></asp:PlaceHolder>
		</LayoutTemplate>
		<ItemTemplate>
			<div class="newsStatHeader">
				<span class="floatRight"><b>Creation Date:</b> <%#Item.CreatedDateClientTime.ToShortDateString()%></span>
				<h2><%#Item.Title%> <span>(<b>Issue:</b> <%#Item.Issue%>)</span></h2>
				<asp:HiddenField runat="server" ID="uxNewsletterID" Value="<%#Item.NewsletterID%>" />
				<div class="clear"></div>
			</div>
			<asp:ListView ID="uxMailoutLV" runat="server" ItemPlaceholderID="uxItemPlaceholder" ItemType="Classes.Newsletters.Mailout">
				<LayoutTemplate>
					<table class="listing">
						<tr>
							<th width="200">
								Mailout Date
							</th>
							<th>
								Mailing Lists
							</th>
							<th width="40">
								Sent
							</th>
							<th width="60">
								Not Sent
							</th>
							<th width="60">
								Opened*
							</th>
							<th width="70">
								Link Clicks
							</th>
							<th width="70">
								Forwards
							</th>
							<th width="110">
								Unsubscriptions
							</th>
						</tr>
						<tr runat="server" id="uxItemPlaceholder">
						</tr>
					</table>
				</LayoutTemplate>
				<ItemTemplate>
					<tr>
						<td>
							<%#Item.Timestamp.ToString("dddd M/d/yyyy h:mm tt")%><br />
							<asp:HyperLink runat="server" Text="View" CssClass="fancybox.iframe detailbox" NavigateUrl='<%#"~/newsletter-details.aspx?iframe&adminView=true&mailoutId=" + Item.MailoutID%>' />
							|
							<asp:HyperLink runat="server" Text="Statistics" CssClass="fancybox.iframe statbox" NavigateUrl='<%#"~/admin/newsletters/admin-mailout-metrics.aspx?iframe&mailoutId=" + Item.MailoutID%>' />
							<asp:PlaceHolder runat="server" ID="uxResendPnl" Visible='<%#Item.NotSentCount > 0%>'>|
								<asp:LinkButton runat="server" ID="uxResendLnk" Text="Resend" OnCommand="Resend_Command" CommandName="Resend" CommandArgument='<%#Item.MailoutID.ToString()%>' OnClientClick="return ConfirmResend();" />
							</asp:PlaceHolder>
						</td>
						<td>
							<%#String.Join(", ", Item.MailingListNames.ToArray())%>
						</td>
						<td>
							<%#Item.SendCount%>
						</td>
						<td>
							<%#Item.NotSentCount%>
						</td>
						<td>
							<%#Item.OpenCount%>
						</td>
						<td>
							<%#Item.ClickCount%>
						</td>
						<td>
							<%#Item.ForwardCount%>
						</td>
						<td>
							<%#Item.UnsubscribeCount%>
						</td>
					</tr>
				</ItemTemplate>
				<EmptyDataTemplate>
					<div class="paddingLeft">
						<em>This newsletter has not been sent yet.</em>
					</div>
				</EmptyDataTemplate>
			</asp:ListView>
		</ItemTemplate>
		<EmptyDataTemplate>
			<br />
			There are no Newsletters, create one
			<a href="admin-newsletter-edit.aspx?id=0">here</a>.
		</EmptyDataTemplate>
	</asp:ListView>
	* Cannot track the opening of a Newsletter for users receiving the TEXT format
</asp:Content>
<asp:Content ContentPlaceHolderID="PageSpecificJS" runat="server">
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
		});

		function ConfirmResend() {
			var confirmed = confirm('Are you sure you want to resend this mailout to subscribers who have not yet received it?');
			if (confirmed)
				$('.floatingBox').show();
			return confirmed;
		}
		//]]>
	</script>
</asp:Content>
