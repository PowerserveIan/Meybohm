<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminNewsletters" CodeFile="admin-newsletter.aspx.cs" Title="Admin - Newsletter Manager" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder">
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByMicrosite" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterNewsletterCMMicrositeID">
				<asp:ListItem Text="--All Markets--" Value=""></asp:ListItem>
			</asp:DropDownList>
		</div>
	</asp:PlaceHolder>
	<asp:PlaceHolder runat="server" ID="uxEmailLimitationsPH" Visible="false">
		<div class="blue">
			<asp:Literal runat="server" ID="uxEmailsRemaining"></asp:Literal>
			<div class="clear"></div>
		</div>
	</asp:PlaceHolder>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Title' && sortDirection(), descending: sortField() == 'Title' && !sortDirection()}, click: function(){listingModel.setSort('Title')}">Title</a>
				</th>
				<th style="width: 110px;">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'CreatedDate' && sortDirection(), descending: sortField() == 'CreatedDate' && !sortDirection()}, click: function(){listingModel.setSort('CreatedDate')}">Creation Date</a>
				</th>
				<th style="width: 65px;">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Issue' && sortDirection(), descending: sortField() == 'Issue' && !sortDirection()}, click: function(){listingModel.setSort('Issue')}">Issue</a>
				</th>
				<th style="width: 180px;">
					Options
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css:{odd:index() % 2 == 0}">
				<td class="first" data-bind="html:Title">
				</td>
				<td data-bind='html:FormatDate(CreatedDateClientTime, "M/d/yyyy")'>
				</td>
				<td data-bind="html:Issue">
				</td>
				<td>
					<a data-bind='attr:{href:linkToEditPage + Id + listingModel.returnString()}' class="icon edit">Edit</a>
					<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this ' + entityClassName + '?'))deleteRecord();}">Delete</a>
					<input type='hidden' data-bind="value:Id" />
					<asp:PlaceHolder runat="server" ID="uxMailoutEnabledPH">
						<a data-bind='attr:{href:"admin-mailout-edit.aspx?NewsletterId=" + Id}' class="icon email or">Send</a>
					</asp:PlaceHolder>
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no Newsletters to edit. Add a Newsletter by clicking the Add New button above..</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		pageFilter.FilterNewsletterCMMicrositeID = ko.observable(null);
		pageFilter.FilterNewsletterCMMicrositeID.subscribe(UseFilters);

		appendToListing = function (index, item, thisListing) {
			thisListing.CreatedDateClientTime = item.CreatedDateClientTime;
			thisListing.Id = item.NewsletterID;
			thisListing.Issue = item.Issue;
			thisListing.Title = item.Title;
		}

		<% if (!String.IsNullOrEmpty(uxFilterByMicrosite.SelectedValue))
	 { %>
		listingModel.readyToReloadListing(false);
		$(document).ready(function () {
			pageFilter.FilterNewsletterCMMicrositeID("<%=uxFilterByMicrosite.SelectedValue%>");
			listingModel.readyToReloadListing(true);
		});<%} %>
	</script>
</asp:Content>
