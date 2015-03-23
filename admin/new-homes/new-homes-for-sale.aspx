<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminNewHomesForSale" CodeFile="new-homes-for-sale.aspx.cs" Title="Admin - New Homes For Sale" %>

<%@ Import Namespace="BaseCode" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:Button runat="server" ID="uxDownloadReport" Text="Export to CSV" CssClass="button export" data-bind="visible: listings().length > 0" />
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder">
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByShowcaseID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterShowcaseItemShowcaseID">
				<asp:ListItem Text="--All Markets--" Value=""></asp:ListItem>
			</asp:DropDownList>
		</div>
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByBuilderID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterShowcaseItemBuilderID">
				<asp:ListItem Text="--All Builders--" Value=""></asp:ListItem>
			</asp:DropDownList>
		</div>
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByNeighborhoodID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterShowcaseItemNeighborhoodID">
				<asp:ListItem Text="--All Neighborhoods--" Value=""></asp:ListItem>
			</asp:DropDownList>
		</div>
	</asp:PlaceHolder>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Address.Address1' && sortDirection(), descending: sortField() == 'Address.Address1' && !sortDirection()}, click: function(){listingModel.setSort('Address.Address1')}">
						Address</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Builder.Name' && sortDirection(), descending: sortField() == 'Builder.Name' && !sortDirection()}, click: function(){listingModel.setSort('Builder.Name')}">
						Builder</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Agent.Email' && sortDirection(), descending: sortField() == 'Agent.Email' && !sortDirection()}, click: function(){listingModel.setSort('Agent.Email')}">Agent</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'NumberBedrooms' && sortDirection(), descending: sortField() == 'NumberBedrooms' && !sortDirection()}, click: function(){listingModel.setSort('NumberBedrooms')}">
						BR</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'NumberBathrooms' && sortDirection(), descending: sortField() == 'NumberBathrooms' && !sortDirection()}, click: function(){listingModel.setSort('NumberBathrooms')}">
						BA</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'ListPrice' && sortDirection(), descending: sortField() == 'ListPrice' && !sortDirection()}, click: function(){listingModel.setSort('ListPrice')}">List Price</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'NumberOfVisits' && sortDirection(), descending: sortField() == 'NumberOfVisits' && !sortDirection()}, click: function(){listingModel.setSort('NumberOfVisits')}">Visits</a>
				</th>
				<th style="width: 70px;">
					Options
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: neighborhoods">
			<tr>
				<td colspan="2" style="font-size: 25px;" data-bind="html: Name"></td>
				<td colspan="4" data-bind="html: Overview"></td>
				<td colspan="2" data-bind="html: CombinedVisits()"></td>
			</tr>
			<!-- ko foreach: NeighborhoodListings-->
			<tr data-bind="css:{odd:$index % 2 == 0}">
				<td class="first" data-bind='html:Address.Address1'>
				</td>
				<td data-bind='html:Builder ? Builder.Name : ""'>
				</td>
				<td data-bind='html:AgentInfo ? AgentInfo.FirstAndLast : ""'>
				</td>
				<td data-bind='html:NumberBedrooms'>					
				</td>
				<td data-bind='html:NumberBathrooms'>
				</td>
				<td data-bind='html:"$" + ListPrice.toLocaleString()'>
				</td>
				<td data-bind='html:NumberOfVisits'>
				</td>
				<td>
					<a data-bind='attr:{href:linkToEditPage + Id + "&ShowcaseID=" + ShowcaseID}' class="icon edit">Edit</a>
				</td>
			</tr>
			<!-- /ko -->		
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no homes that meet the search criteria.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		pageFilter.FilterShowcaseItemShowcaseID = ko.observable("");
		pageFilter.FilterShowcaseItemNeighborhoodID = ko.observable("");
		pageFilter.FilterShowcaseItemBuilderID = ko.observable("");
		pageFilter.FilterShowcaseItemShowcaseID.subscribe(UseFilters);
		pageFilter.FilterShowcaseItemNeighborhoodID.subscribe(UseFilters);
		pageFilter.FilterShowcaseItemBuilderID.subscribe(UseFilters);
		UseFilters();

		appendToListing = function (index, item, thisListing) {
			thisListing.Id = item.ShowcaseItemID;
			thisListing.Address = item.Address;
			thisListing.AgentInfo = item.AgentInfo;
			thisListing.Builder = item.Builder;
			thisListing.Neighborhood = item.Neighborhood;
			thisListing.NumberBedrooms = item.NumberBedrooms;
			thisListing.NumberBathrooms = item.NumberBathrooms;
			thisListing.ListPrice = item.ListPrice;
			thisListing.NumberOfVisits = item.NumberOfVisits;
			thisListing.ShowcaseID = item.ShowcaseID;
		}

		listingModel.neighborhoods = ko.observableArray([]);
		var neighborhood = function (neighborhood) {
			var self = this;
			this.Name = neighborhood.Name;
			this.Overview = neighborhood.Overview;
			this.NeighborhoodListings = ko.observableArray([]);
			this.CombinedVisits = ko.dependentObservable({
				read: function () {
					var total = 0;
					for (var i = 0; i < self.NeighborhoodListings().length; i++) {
						total += self.NeighborhoodListings()[i].NumberOfVisits;
					}
					return total;
				}
			});
		}
		afterLoad = function (data) {
			listingModel.neighborhoods.removeAll();
			var tempNeighborhoods = [];
			for (var i = 0; i < listingModel.listings().length; i++) {
				if (!listingModel.listings()[i].Neighborhood)
					continue;
				var currentNeighborhood = null;
				for (var j = 0; j < tempNeighborhoods.length; j++) {
					if (listingModel.listings()[i].Neighborhood.Name == tempNeighborhoods[j].Name)
						currentNeighborhood = tempNeighborhoods[j];
				}
				if (!currentNeighborhood) {
					currentNeighborhood = new neighborhood(listingModel.listings()[i].Neighborhood);
					tempNeighborhoods.push(currentNeighborhood);
				}
				currentNeighborhood.NeighborhoodListings.push(listingModel.listings()[i]);
			}
			tempNeighborhoods.sort(function (a, b) {
				return (a.Name < b.Name ? -1 : (a.Name > b.Name ? 1 : 0));
			});
			listingModel.neighborhoods(tempNeighborhoods);
		}
	</script>
</asp:Content>
