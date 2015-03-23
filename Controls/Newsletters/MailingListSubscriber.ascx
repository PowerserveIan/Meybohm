<%@ Control Language="C#" AutoEventWireup="true" Inherits="MailingListSubscriberListing" CodeFile="MailingListSubscriber.ascx.cs" %>
<%@ Import Namespace="Classes.Newsletters" %>
<div class="sectionTitle">
	<div class="bottom">
		<h2>Subscribers</h2>
	</div>
</div>
<div class="formWrapper fullWidth">
	<div class="sectionTitle padded">
		<asp:Panel runat="server" ID="uxSearchPnl" DefaultButton="uxSearch" CssClass="bottom">
			<asp:TextBox ID="uxSearchText" runat="server" CssClass="text" />
			<asp:Button ID="uxSearch" runat="server" CausesValidation="false" Text="Search" CssClass="button" />
		</asp:Panel>
	</div>
	<asp:DataPager ID="uxTopPager" runat="server" PagedControlID="uxMailingListSubscribers" class="pagination">
		<Fields>
			<asp:NextPreviousPagerField PreviousPageText="Previous" ShowNextPageButton="false" ButtonCssClass="prev" />
			<asp352:NumericPagerFieldCustom ButtonCount="99999999" />
			<asp:NextPreviousPagerField PreviousPageText="Next" ShowPreviousPageButton="false" ButtonCssClass="next" />
		</Fields>
	</asp:DataPager>
	<div class="blue">
		<asp:ValidationSummary runat="server" ID="ErrorSummary" DisplayMode="BulletList" CssClass="validation" ValidationGroup="Subscriber" HeaderText="<h3 class='failure'>Please correct the following errors:</h3>" />
		<asp:PlaceHolder ID="uxSuccessPH" runat="server">
			<h3 class="success">The Subscriber has been successfully <u>updated</u>.</h3>
		</asp:PlaceHolder>
		<asp:Label runat="server" ID="uxFailureMessage" ForeColor="Red"></asp:Label>
		<label for="<%=uxEmail.ClientID%>" class="subscriberEmailLabel">
			Email:</label><div class="clear"></div>
		<asp:TextBox ID="uxEmail" runat="server" MaxLength="382" CssClass="text" Width="62%" />
		<asp:Button ID="uxSubscribeButton" runat="server" Text="Subscribe" OnCommand="AllCommands" CommandName="Subscribe" ToolTip="Subscribe New Email Address" CssClass="button add" ValidationGroup="Subscriber" />
		<asp:Button ID="uxUnsubscribeButton" runat="server" Text="Unsubscribe" OnCommand="AllCommands" CommandName="Unsubscribe" ToolTip="Unsubscribe Email Address" CssClass="button delete" ValidationGroup="Subscriber" />
		<asp:Button ID="uxExport" runat="server" Text="Export Mailing List" CssClass="button export floatRight" CausesValidation="false" />
		<div class="clear"></div>
		<asp:RequiredFieldValidator ID="uxEmailRFV" runat="server" Display="None" ErrorMessage="Please enter an email address." ControlToValidate="uxEmail" ValidationGroup="Subscriber" />
		<asp:RegularExpressionValidator ID="uxEmailREV" runat="server" Display="None" ErrorMessage="Valid email address required." ControlToValidate="uxEmail" ValidationGroup="Subscriber" />
		<asp:Label runat="server" ID="uxNoMoreSubscribers" Visible="false" />
		<div class="clear"></div>
	</div>
	<table class="listing">
		<thead>
			<tr>
				<th class="first">
					Format
				</th>
				<th>
					<asp:LinkButton ID="uxMailingListSubscriberSubscriberIDSortIb" runat="server" CommandArgument="Email" CommandName="Sort" OnCommand="Sort_Command" CssClass='<%#GetSortClasses("Email")%>'>Subscriber Email</asp:LinkButton>
				</th>
				<th style="width: 120px;">
					Actions
				</th>
			</tr>
		</thead>
		<asp:ListView ID="uxMailingListSubscribers" runat="server" ItemPlaceholderID="uxItemPlaceHolder" ItemType="Classes.Newsletters.MailingListSubscriber">
			<LayoutTemplate>
				<asp:PlaceHolder ID="uxItemPlaceHolder" runat="server"></asp:PlaceHolder>
			</LayoutTemplate>
			<ItemTemplate>
				<tr>
					<td class="first">
						<%#(Item.NewsletterFormatID == null) ? "" : NewsletterFormat.GetByID((int)Item.NewsletterFormatID).Name%>
					</td>
					<td>
						<%#Subscriber.GetByID(Item.SubscriberID).Email%>
					</td>
					<td>
						<asp:LinkButton ID="uxEditSubscriber" OnCommand="AllCommands" Text='edit' CommandName="EditSubscriber" CommandArgument='<%#Item.MailingListSubscriberID%>' runat="server" CssClass="icon edit" />
						<asp:LinkButton ID="uxEnableToggle" OnCommand="AllCommands" Text='remove' CommandName="EnableToggle" CommandArgument='<%#Item.MailingListSubscriberID%>' runat="server" CssClass="icon delete or" />
					</td>
				</tr>
			</ItemTemplate>
		</asp:ListView>
	</table>
</div>
<div class="clear"></div>
<script type="text/javascript">
	$(document).ready(function () {
		if ($("span.pagination").length > 0)
			$("span.pagination").pagination();
	});
</script>
