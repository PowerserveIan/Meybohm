<%@ Page Language="C#" MasterPageFile="~/microsite.master" AutoEventWireup="true" CodeFile="agent-home.aspx.cs" Inherits="MembersHome" Title="Agent Dashboard" %>

<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentContentDiv">
	<div class="content inner full agentHome padded">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<asp:HyperLink runat="server" ID="uxHelpDesk" Target="_blank" CssClass="floatRight button helpdesk">HelpDesk</asp:HyperLink>
	<div class="clear"></div>
	<div class="formThird">
		<div class="widget">
			<h2>
				<asp:Literal runat="server" ID="uxOffice"></asp:Literal></h2>
			<div class="widgetInner">
				<asp:Literal runat="server" ID="uxOfficeAddress"></asp:Literal><br />
				<strong>Phone:</strong>
				<asp:Literal runat="server" ID="uxOfficePhone"></asp:Literal><br />
				<strong>Fax:</strong>
				<asp:Literal runat="server" ID="uxOfficeFax"></asp:Literal><br />
			</div>
		</div>
	</div>
	<div class="formThird">
		<div class="widget">
			<h2>Company Announcements</h2>
			<div class="widgetInner">
				<newspress:quickView runat="server" ID="uxCompanyAnnouncements" Category="CompanyAnnouncements" />
			</div>
		</div>
	</div>
	<div class="formThird">
		<div class="widget">
			<h2>Listing Stats</h2>
			<div class="widgetInner">
				<strong>Total Listings:</strong>
				<asp:Literal runat="server" ID="uxTotalListings"></asp:Literal>
				<asp:Repeater runat="server" ID="uxListingTypes">
					<ItemTemplate>
						<br />
						<%# ((KeyValuePair<string, int>)Container.DataItem).Key %>: <%# ((KeyValuePair<string, int>)Container.DataItem).Value %>
					</ItemTemplate>
				</asp:Repeater>
			</div>
		</div>
	</div>
	<div class="clear"></div>
	<div class="formWhole">
		<h2>Your Email Inbox</h2>
		<hr />
		<table class="listing paddingBottom" data-bind="visible: !viewModel.EmailsLoading()">
			<thead>
				<tr>
					<th class="first">
						Subject
					</th>
					<th>
						Sender
					</th>
					<th>
						Date
					</th>
					<%--<th class="view"></th>--%>
				</tr>
			</thead>
			<tbody data-bind="foreach: Emails">
				<tr data-bind="css:{odd:$index % 2 == 0}">
					<td class="first" data-bind="html:Subject">
					</td>
					<td data-bind="html:Sender">
					</td>
					<td data-bind="html:FormatDate(DateTimeReceived, 'MM/d/yyyy h:mm tt')">
					</td>
					<%--<td>
						<a href="#" class="icon view button" data-bind="attr:{href: 'https://mail.meybohm.com/owa/?ae=Item&id=Rg' + Id + 'J'}" target="_blank">View</a>
					</td>--%>
				</tr>
			</tbody>
		</table>
		<div class="loadMessage" id="emailLoading" data-bind="visible: viewModel.EmailsLoading()"><span>Loading...</span></div>
		<br />
		<a href="https://mail.meybohm.com/" class="button" target="_blank">Open Your Inbox</a>
	</div>
	<asp:PlaceHolder runat="server" ID="uxPropertyContactsPH">
		<div class="formWhole">
			<h2>Property Specific Contact Requests</h2>
			<hr />
			<a href="#" class="floatRight" data-bind="visible: viewModel.PropertyTotalCount() > viewModel.PropertyPageSize(), click: function(){viewModel.PropertyPageSize(999999);}">View All <span data-bind="	html:'(' + viewModel.PropertyTotalCount() + ')'"></span></a>
			<div class="clear"></div>
			<table class="listing paddingBottom" data-bind="visible: !viewModel.PropertyLoading()">
				<thead>
					<tr>
						<th class="first date">
							<a href="#" class="sort" data-bind="css:{ascending: PropertySortField() == 'Created' && PropertySortDirection(), descending: PropertySortField() == 'Created' && !PropertySortDirection()}, click: function(){viewModel.SetPropertySort('Created')}">Date</a>
						</th>
						<th class="property">
							<a href="#" class="sort" data-bind="css:{ascending: PropertySortField() == 'ShowcaseItem.Title' && PropertySortDirection(), descending: PropertySortField() == 'ShowcaseItem.Title' && !PropertySortDirection()}, click: function(){viewModel.SetPropertySort('ShowcaseItem.Title')}">
								Property</a>
						</th>
						<th class="name">
							<a href="#" class="sort" data-bind="css:{ascending: PropertySortField() == 'LastName' && PropertySortDirection(), descending: PropertySortField() == 'LastName' && !PropertySortDirection()}, click: function(){viewModel.SetPropertySort('LastName')}">Name</a>
						</th>
						<th class="email">
							<a href="#" class="sort" data-bind="css:{ascending: PropertySortField() == 'Email' && PropertySortDirection(), descending: PropertySortField() == 'Email' && !PropertySortDirection()}, click: function(){viewModel.SetPropertySort('Email')}">Email</a>
						</th>
						<th class="phone">
							<a href="#" class="sort" data-bind="css:{ascending: PropertySortField() == 'Phone' && PropertySortDirection(), descending: PropertySortField() == 'Phone' && !PropertySortDirection()}, click: function(){viewModel.SetPropertySort('Phone')}">Phone</a>
						</th>
						<th class="view"></th>
					</tr>
				</thead>
				<tbody data-bind="foreach: PropertyRequests">
					<tr data-bind="css:{odd:$index % 2 == 0}">
						<td class="first" data-bind="html:FormatDate(Created, 'MM/d/yyyy')">
						</td>
						<td>
							<a href="#" class="fancybox.iframe showcaseProject" data-bind="html:ShowcaseItem.Address.FormattedAddress,attr:{href:'home-details?id=' + ShowcaseItemID}"></a>
						</td>
						<td data-bind="html:FirstName + ' ' + LastName">
						</td>
						<td data-bind="html:Email">
						</td>
						<td data-bind="html:Phone">
						</td>
						<td>
							<a href="#viewContact" class="icon view contactRequest button" data-bind="click: function(){viewModel.ContactRequest($data);if (unreadStatusID == viewModel.ContactRequest().ContactStatusID)SetContactStatusToRead(viewModel.ContactRequest().ContactID);}">View</a>
						</td>
					</tr>
				</tbody>
			</table>
			<div class="loadMessage" id="propertyLoading" data-bind="visible: viewModel.PropertyLoading()"><span>Loading...</span></div>
		</div>
	</asp:PlaceHolder>
	<div class="formWhole">
		<h2>Agent Requests</h2>
		<hr />
		<a href="#" class="floatRight" data-bind="visible: viewModel.AgentTotalCount() > viewModel.AgentPageSize(), click: function(){viewModel.AgentPageSize(999999);}">View All <span data-bind="	html:'(' + viewModel.AgentTotalCount() + ')'"></span></a>
		<div class="clear"></div>
		<table class="listing paddingBottom" data-bind="visible: !viewModel.AgentLoading()">
			<thead>
				<tr>
					<th class="first date">
						<a href="#" class="sort" data-bind="css:{ascending: AgentSortField() == 'Created' && AgentSortDirection(), descending: AgentSortField() == 'Created' && !AgentSortDirection()}, click: function(){viewModel.SetAgentSort('Created')}">Date</a>
					</th>
					<th class="name">
						<a href="#" class="sort" data-bind="css:{ascending: AgentSortField() == 'LastName' && AgentSortDirection(), descending: AgentSortField() == 'LastName' && !AgentSortDirection()}, click: function(){viewModel.SetAgentSort('LastName')}">Name</a>
					</th>
					<th class="email">
						<a href="#" class="sort" data-bind="css:{ascending: AgentSortField() == 'Email' && AgentSortDirection(), descending: AgentSortField() == 'Email' && !AgentSortDirection()}, click: function(){viewModel.SetAgentSort('Email')}">Email</a>
					</th>
					<th class="phone">
						<a href="#" class="sort" data-bind="css:{ascending: AgentSortField() == 'Phone' && AgentSortDirection(), descending: AgentSortField() == 'Phone' && !AgentSortDirection()}, click: function(){viewModel.SetAgentSort('Phone')}">Phone</a>
					</th>
					<th class="view"></th>
				</tr>
			</thead>
			<tbody data-bind="foreach: AgentRequests">
				<tr data-bind="css:{odd:$index % 2 == 0}">
					<td class="first" data-bind="html:FormatDate(CreatedClientTime, 'MM/d/yyyy')">
					</td>
					<td data-bind="html:FirstName + ' ' + LastName">
					</td>
					<td data-bind="html:Email">
					</td>
					<td data-bind="html:Phone">
					</td>
					<td>
						<a href="#viewContact" class="icon view contactRequest button" data-bind="click: function(){viewModel.ContactRequest($data);if (unreadStatusID == viewModel.ContactRequest().ContactStatusID)SetContactStatusToRead(viewModel.ContactRequest().ContactID);}">View</a>
					</td>
				</tr>
			</tbody>
		</table>
		<div class="loadMessage" id="agentLoading" data-bind="visible: viewModel.AgentLoading()"><span>Loading...</span></div>
	</div>
	<div class="formWhole">
		<h2>My Property Listings</h2>
		<hr />
		<div class="selectOptions">
			<a href="#" id="selectAllProperties">Select All</a>
			<span>| </span>
			<a href="#" id="selectNoProperties">Select None</a>
			<a href="#" class="floatRight" data-bind="visible: viewModel.PropertyListingsTotalCount() > viewModel.PropertyListingsPageSize(), click: function(){viewModel.PropertyListingsPageSize(999999);}">View All <span data-bind="	html:'(' + viewModel.PropertyListingsTotalCount() + ')'">
			</span></a>
			<span class="floatRight" data-bind="visible: viewModel.PropertyListingsTotalCount() > viewModel.PropertyListingsPageSize()">&nbsp;|&nbsp;</span>
			<a href="#shareListing" class="floatRight shareListings">Share Selected Listings</a>
			<div class="clear"></div>
		</div>
		<table class="listing paddingBottom" id="propertyListing" data-bind="visible: !viewModel.PropertyListingsLoading()">
			<thead>
				<tr>
					<th class="first select">
					</th>
					<th class="date">
						<a href="#" class="sort" data-bind="css:{ascending: PropertyListingsSortField() == 'DateListed' && PropertyListingsSortDirection(), descending: PropertyListingsSortField() == 'DateListed' && !PropertyListingsSortDirection()}, click: function(){viewModel.SetPropertyListingsSort('DateListed')}">
							List Date</a>
					</th>
					<th class="address">
						Address
					</th>
					<th class="mlsid">
						<a href="#" class="sort" data-bind="css:{ascending: PropertyListingsSortField() == 'MlsID' && PropertyListingsSortDirection(), descending: PropertyListingsSortField() == 'MlsID' && !PropertyListingsSortDirection()}, click: function(){viewModel.SetPropertyListingsSort('MlsID')}">
							MLS ID</a>
					</th>
					<th class="price">
						<a href="#" class="sort" data-bind="css:{ascending: PropertyListingsSortField() == 'ListPrice' && PropertyListingsSortDirection(), descending: PropertyListingsSortField() == 'ListPrice' && !PropertyListingsSortDirection()}, click: function(){viewModel.SetPropertyListingsSort('ListPrice')}">
							Price</a>
					</th>
					<th class="visits">
						<a href="#" class="sort" data-bind="css:{ascending: PropertyListingsSortField() == 'NumberOfVisits' && PropertyListingsSortDirection(), descending: PropertyListingsSortField() == 'NumberOfVisits' && !PropertyListingsSortDirection()}, click: function(){viewModel.SetPropertyListingsSort('NumberOfVisits')}">
							Visits</a>
					</th>
					<th class="options">
						Options
					</th>
				</tr>
			</thead>
			<tbody data-bind="foreach: PropertyListings">
				<tr data-bind="css:{odd:$index % 2 == 0}">
					<td class="first">
						<input type="checkbox" />
						<input type="hidden" data-bind="value: ShowcaseItemID" />
					</td>
					<td data-bind="html:FormatDate(DateListedClientTime, 'MM/d/yyyy')">
					</td>
					<td data-bind="html:Address.Address1 + ', ' + Address.City + ', ' + Address.State.Name + ', ' + Address.Zip">
					</td>
					<td data-bind="html:MlsID">
					</td>
					<td data-bind="html:'$' + ListPrice.toLocaleString()">
					</td>
					<td data-bind="html:NumberOfVisits">
					</td>
					<td>
						<a href="#" class="fancybox.iframe showcaseProject button" data-bind="attr:{href:'home-details?id=' + ShowcaseItemID}">View</a>
						<a href="#shareListing" class="sendStats button">Send Stats</a>
					</td>
				</tr>
			</tbody>
		</table>
		<div class="loadMessage" id="propertyListingsLoading" data-bind="visible: viewModel.PropertyListingsLoading()"><span>Loading...</span></div>
	</div>
	<div class="formWhole"></div>
	<div class="formThird">
		<div class="widget">
			<h2>Recent Company Newsletters</h2>
			<div class="widgetInner">
				<asp:Repeater runat="server" ID="uxNewsletters" ItemType="Classes.Newsletters.Newsletter">
					<ItemTemplate>
						<span><%# Item.DisplayDateClientTime.ToString("MM/d/yyyy") %></span>
						<asp:HyperLink runat="server" CssClass="newsletterLink fancybox.iframe" Text="<%# Item.Title %>" NavigateUrl='<%# "~/newsletter-details.aspx?id=" + Item.NewsletterID %>'></asp:HyperLink>
					</ItemTemplate>
				</asp:Repeater>
				<br />
				<br />
				<asp:HyperLink runat="server" Text="View All" NavigateUrl="~/newsletter.aspx" CssClass="textLinkBtn"></asp:HyperLink>
			</div>
		</div>
	</div>
	<div class="formThird">
		<div class="widget">
			<h2>Training Videos</h2>
			<div class="widgetInner">
				<asp:Literal runat="server" ID="uxVideoUrl"></asp:Literal>
				<asp:Label runat="server" ID="uxVideoTitle"></asp:Label>
				<br />
				<br />
				<a href="videos" class="textLinkBtn">View All
				</a>
			</div>
		</div>
	</div>
	<div class="formThird">
		<div class="widget">
			<h2>HR Updates</h2>
			<div class="widgetInner">
				<newspress:quickView runat="server" ID="uxHRUpdates" Category="HRUpdates" />
			</div>
		</div>
	</div>
	<div style="display: none;">
		<div id="viewContact" class="agentHomeBox">
			<!-- ko if: viewModel.ContactRequest() -->
			<h2 data-bind="html: (viewModel.ContactRequest().ContactTypeID == <%= (int)Classes.Contacts.ContactTypes.PropertyInformation %> ? 'Property Information' : 'Agent') + ' Request'"></h2>
			<div class="formHalf">
				<span class="label">Submitted On</span>
				<span data-bind="html:FormatDate(viewModel.ContactRequest().CreatedClientTime, 'MM/d/yyyy h:mm tt')"></span>
			</div>
			<div class="formHalf">
				<span class="label">Contact Time</span>
				<span data-bind="html:viewModel.ContactRequest().ContactTime ? viewModel.ContactRequest().ContactTime.Name : ''"></span>
			</div>
			<div class="formHalf">
				<span class="label">Contact Method</span>
				<span data-bind="html:viewModel.ContactRequest().ContactMethod ? viewModel.ContactRequest().ContactMethod.Name : ''"></span>
			</div>
			<div class="formHalf">
				<span class="label">First Name</span>
				<span data-bind="html:viewModel.ContactRequest().FirstName"></span>
			</div>
			<div class="formHalf">
				<span class="label">Last Name</span>
				<span data-bind="html:viewModel.ContactRequest().LastName"></span>
			</div>
			<div class="formHalf">
				<span class="label">Email</span>
				<span data-bind="html:viewModel.ContactRequest().Email"></span>
			</div>
			<div class="formHalf">
				<span class="label threeLine">Phone</span>
				<span data-bind="html:viewModel.ContactRequest().Phone"></span>
			</div>
			<div class="formWhole">
				<span class="label">Message</span>
				<span data-bind="html:viewModel.ContactRequest().Message"></span>
			</div>
			<div class="formHalf">
				<label for="<%= uxContactStatusID.ClientID %>">
					Status</label>
				<asp:DropDownList runat="server" ID="uxContactStatusID" data-bind="value: viewModel.ContactRequest().ContactStatusID" />
				<div class="clear"></div>
				<br />
				<a href="#" class="button" onclick="UpdateContactStatus(viewModel.ContactRequest().ContactID);return false;">Update Status</a>
			</div>
			<!-- /ko -->
		</div>
	</div>
	<div style="display: none;">
		<div id="shareListing" class="agentHomeBox">
			<div class="formWhole">
				<label for="<%= uxShareListingEmails.ClientID %>">
					Send to Email Addresses<br />
					<em><small>Separate addresses with a semi-colon (;)</small></em></label>
				<asp:TextBox runat="server" ID="uxShareListingEmails" CssClass="text" MaxLength="500" />
				<asp:RequiredFieldValidator runat="server" ID="uxShareListingEmailsRFV" ControlToValidate="uxShareListingEmails" ErrorMessage="You must enter at least one email address." ValidationGroup="SendEmails" />
			</div>
			<div class="formWhole">
				<label for="<%= uxShareListingEmailSubject.ClientID %>">Email Subject</label>
				<asp:TextBox runat="server" ID="uxShareListingEmailSubject" CssClass="text" MaxLength="500" />
			</div>
			<div class="formWhole">
				<label for="<%= uxShareListingPersonalMessage.ClientID %>">Personal Message</label>
				<asp:TextBox runat="server" ID="uxShareListingPersonalMessage" CssClass="text" TextMode="MultiLine" />
			</div>
			<div class="clear"></div>
			<br />
			<a href="#" class="button" id="sendSharedListings">Send Emails</a>
			<a href="#" class="button" id="sendStats">Send Emails</a>
			<input type="hidden" id="statsShowcaseItemID" />
		</div>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentSideCol"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/json2.js,~/tft-js/core/knockout.js,~/tft-js/core/jquery.dateformat.js,~/tft-js/agent-home.js"></asp:Literal>
	<script type="text/javascript">
		var defaultPageUrl = '<%= ResolveClientUrl("~/agent-home.aspx") %>';
		var readStatusID = <%= (int)Classes.Contacts.ContactStatuses.Read %>;
		var unreadStatusID = <%= (int)Classes.Contacts.ContactStatuses.Unread %>;
	</script>
</asp:Content>
