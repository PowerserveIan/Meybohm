<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminContact" CodeFile="admin-contact.aspx.cs" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder">
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByMicrosite" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterContactCMMicrositeID">
				<asp:ListItem Text="--All Markets--" Value=""></asp:ListItem>
			</asp:DropDownList>
		</div>
		<asp:PlaceHolder runat="server" ID="uxPropertySpecificPH">			
			<div class="column">
				<asp:DropDownList runat="server" ID="uxFilterByPropertyType" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterShowcaseID">
					<asp:ListItem Text="--All Property Types--" Value=""></asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="column">
				<asp:DropDownList runat="server" ID="uxFilterByShowcaseItemID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterContactShowcaseItemID">
					<asp:ListItem Text="--All Properties--" Value=""></asp:ListItem>
				</asp:DropDownList>
			</div>
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="uxAgentPH">
			<div class="column">
				<asp:DropDownList runat="server" ID="uxFilterByAgentID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterContactAgentID">
					<asp:ListItem Text="--All Agents--" Value=""></asp:ListItem>
				</asp:DropDownList>
			</div>
		</asp:PlaceHolder>
	</asp:PlaceHolder>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'LastName' && sortDirection(), descending: sortField() == 'LastName' && !sortDirection()}, click: function(){listingModel.setSort('LastName')}">Last Name</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'FirstName' && sortDirection(), descending: sortField() == 'FirstName' && !sortDirection()}, click: function(){listingModel.setSort('FirstName')}">First Name</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Email' && sortDirection(), descending: sortField() == 'Email' && !sortDirection()}, click: function(){listingModel.setSort('Email')}">Email</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Created' && sortDirection(), descending: sortField() == 'Created' && !sortDirection()}, click: function(){listingModel.setSort('Created')}">Submitted</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'ContactStatusID' && sortDirection(), descending: sortField() == 'ContactStatusID' && !sortDirection()}, click: function(){listingModel.setSort('ContactStatusID')}">Status</a>
				</th>
				<th data-bind="visible:pageFilter.FilterContactContactTypeID() == propertyContactType">
					Property Address
				</th>
				<th style="width: 120px;">
					Options
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css:{odd:index() % 2 == 0}">
				<td class="first" data-bind='html:LastName'>
				</td>
				<td data-bind='html:FirstName'>
				</td>
				<td data-bind='html:Email'>
				</td>
				<td data-bind='html:FormatDate(CreatedClientTime, "MMMM d, yyyy")'>
				</td>
				<td data-bind='html:ContactStatusName'>
				</td>
				<td data-bind="visible:pageFilter.FilterContactContactTypeID() == propertyContactType">
					<a href="#" data-bind="html:FormattedAddress,attr:{href:'../showcase/admin-showcase-item-edit.aspx?id=' + ShowcaseItemID}"></a>
				</td>
				<td>
					<a data-bind='attr:{href:linkToEditPage + Id + listingModel.returnString()}' class="icon edit">Edit</a>
					<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this ' + entityClassName + '?'))deleteRecord();}">Delete</a>
					<input type='hidden' data-bind="value:Id" />
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no Contacts to edit.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		var propertyContactType = <%= (int)Classes.Contacts.ContactTypes.PropertyInformation %>;		
		pageFilter.FilterContactAgentID = ko.observable("");
		pageFilter.FilterContactContactTypeID = ko.observable('<%= !String.IsNullOrEmpty(Request.QueryString["FilterContactContactTypeID"]) ? Request.QueryString["FilterContactContactTypeID"] : ((int)Classes.Contacts.ContactTypes.ContactUs).ToString() %>');
		pageFilter.FilterContactCMMicrositeID = ko.observable("");
		pageFilter.FilterContactShowcaseItemID = ko.observable("");
		pageFilter.FilterShowcaseID = ko.observable("");
		pageFilter.FilterContactAgentID.subscribe(UseFilters);
		pageFilter.FilterContactCMMicrositeID.subscribe(UseFilters);
		pageFilter.FilterContactShowcaseItemID.subscribe(UseFilters);
		pageFilter.FilterShowcaseID.subscribe(UseFilters);
		UseFilters();

		appendToListing = function (index, item, thisListing) {
			thisListing.Id = item.ContactID;
			thisListing.ContactStatusName = item.ContactStatusName;
			thisListing.CreatedClientTime = item.CreatedClientTime;
			thisListing.Email = item.Email;
			thisListing.FirstName = item.FirstName;
			thisListing.LastName = item.LastName;
			thisListing.ShowcaseItemID = item.ShowcaseItemID;
			thisListing.FormattedAddress = item.ShowcaseItem && item.ShowcaseItem.Address ? item.ShowcaseItem.Address.FormattedAddress : null;
		}
		<% if (!String.IsNullOrEmpty(uxFilterByMicrosite.SelectedValue) || !String.IsNullOrEmpty(uxFilterByShowcaseItemID.SelectedValue) || !String.IsNullOrEmpty(uxFilterByAgentID.SelectedValue) || !String.IsNullOrEmpty(uxFilterByPropertyType.SelectedValue))
	 { %>
		listingModel.readyToReloadListing(false);
		$(document).ready(function () {
			pageFilter.FilterContactAgentID("<%=uxFilterByAgentID.SelectedValue%>");
			pageFilter.FilterContactCMMicrositeID("<%=uxFilterByMicrosite.SelectedValue%>");
			pageFilter.FilterContactShowcaseItemID("<%=uxFilterByShowcaseItemID.SelectedValue%>");
			pageFilter.FilterShowcaseID("<%=uxFilterByPropertyType.SelectedValue%>");
			listingModel.readyToReloadListing(true);
		});<%} %>
	</script>
</asp:Content>
