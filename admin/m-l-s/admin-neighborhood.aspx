<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminNeighborhood" CodeFile="admin-neighborhood.aspx.cs" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder">
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByMicrosite" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterNeighborhoodCMMicrositeID">
				<asp:ListItem Text="--All Markets--" Value=""></asp:ListItem>
			</asp:DropDownList>
		</div>
	</asp:PlaceHolder>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Name' && sortDirection(), descending: sortField() == 'Name' && !sortDirection()}, click: function(){listingModel.setSort('Name')}">Name</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Active' && sortDirection(), descending: sortField() == 'Active' && !sortDirection()}, click: function(){listingModel.setSort('Active')}">Active</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Featured' && sortDirection(), descending: sortField() == 'Featured' && !sortDirection()}, click: function(){listingModel.setSort('Featured')}">Featured</a>
				</th>
				<th style="width: 120px;">
					Options
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css:{odd:index() % 2 == 0}">
				<td class="first" data-bind='html:Name'>
				</td>
				<td>
					<a href='#' class="icon noText" data-bind="click: toggleActive, css: {active: Active(), inactive: !Active() }, text:activeText(),attr:{title:activeText()}"></a>
				</td>
				<td>
					<a href='#' class="icon noText" data-bind="click: toggleFeatured, css: {active: Featured(), inactive: !Featured() }, text:featuredText(),attr:{title:featuredText()}"></a>
				</td>
				<td>
					<a data-bind='attr:{href:linkToEditPage + Id + listingModel.returnString()}' class="icon edit">Edit</a>
					<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this ' + entityClassName + '?'))deleteRecord();}">Delete</a>
					<input type='hidden' data-bind="value:Id" />
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no Neighborhoods to edit. Add a Neighborhood by clicking the Add New button above.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		pageFilter.FilterNeighborhoodCMMicrositeID = ko.observable(null);
		pageFilter.FilterNeighborhoodCMMicrositeID.subscribe(UseFilters);
		appendToListing = function (index, item, thisListing) {
			thisListing.Id = item.NeighborhoodID;
			thisListing.Name = item.Name;
		}

		<% if (!String.IsNullOrEmpty(uxFilterByMicrosite.SelectedValue))
	 { %>
		listingModel.readyToReloadListing(false);
		$(document).ready(function () {
			pageFilter.FilterNeighborhoodCMMicrositeID("<%=uxFilterByMicrosite.SelectedValue%>");
			listingModel.readyToReloadListing(true);
		});<%} %>
	</script>
</asp:Content>
