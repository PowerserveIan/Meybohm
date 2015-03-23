<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="SalesReport" CodeFile="sales-report.aspx.cs" Title="Admin - New Homes Sales Report" %>

<%@ Register TagPrefix="Controls" TagName="DateTimePicker" Src="~/Controls/BaseControls/DateTimePicker.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:Button runat="server" ID="uxDownloadReport" Text="Export to CSV" CssClass="button export" data-bind="visible: listings().length > 0" />
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder">
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByOfficeID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterSoldHomeSellerOfficeID">
				<asp:ListItem Text="--All Offices--" Value=""></asp:ListItem>
			</asp:DropDownList>
			<div class="date">
				<label for="<%= uxBeginDate.ClientID %>_uxDate">
					Begin Date</label>
				<Controls:DateTimePicker runat="server" ID="uxBeginDate" TextBoxCssClass="text" />
			</div>
		</div>
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByAgentID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterSoldHomeListingAgentID">
				<asp:ListItem Text="--All Agents--" Value=""></asp:ListItem>
			</asp:DropDownList>
			<div class="date">
				<label for="<%= uxEndDate.ClientID %>_uxDate">
					End Date</label>
				<Controls:DateTimePicker runat="server" ID="uxEndDate" TextBoxCssClass="text" />
			</div>
		</div>
	</asp:PlaceHolder>
	<div class="tableInfo" data-bind="visible: listings().length > 0">
		<span data-bind='html: "<strong>$" + listingModel.totalSales().toLocaleString() + "</strong> total sales"'></span>
	</div>
	<div class="clear"></div>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'ShowcaseItem.Office.Name' && sortDirection(), descending: sortField() == 'ShowcaseItem.Office.Name' && !sortDirection()}, click: function(){listingModel.setSort('ShowcaseItem.Office.Name')}">
						Office</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'ListingAgent.Email' && sortDirection(), descending: sortField() == 'ListingAgent.Email' && !sortDirection()}, click: function(){listingModel.setSort('ListingAgent.Email')}">Agent</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'ShowcaseItem.Address.Address1' && sortDirection(), descending: sortField() == 'ShowcaseItem.Address.Address1' && !sortDirection()}, click: function(){listingModel.setSort('ShowcaseItem.Address.Address1')}">
						Address</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'ShowcaseItem.Neighborhood.Name' && sortDirection(), descending: sortField() == 'ShowcaseItem.Neighborhood.Name' && !sortDirection()}, click: function(){listingModel.setSort('ShowcaseItem.Neighborhood.Name')}">
						Neighborhood</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'CloseDate' && sortDirection(), descending: sortField() == 'CloseDate' && !sortDirection()}, click: function(){listingModel.setSort('CloseDate')}">Date Closed</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'SalePrice' && sortDirection(), descending: sortField() == 'SalePrice' && !sortDirection()}, click: function(){listingModel.setSort('SalePrice')}">Sale Price</a>
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css:{odd:index() % 2 == 0}">
				<td class="first" data-bind="html:OfficeName">
				</td>
				<td data-bind="html:AgentFirstAndLast">
				</td>
				<td data-bind="html:Address">
				</td>
				<td data-bind="html:NeighborhoodName">
				</td>
				<td data-bind="html:FormatDate(CloseDate, 'MM/d/yyyy')">
				</td>
				<td data-bind="html:'$' + SalePrice.toLocaleString()">
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no sales that match the search criteria.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		pageFilter.FilterSoldHomeSellerOfficeID = ko.observable("");
		pageFilter.FilterSoldHomeListingAgentID = ko.observable("");
		pageFilter.FilterBeginDate = ko.observable("");
		pageFilter.FilterEndDate = ko.observable("");
		pageFilter.FilterSoldHomeSellerOfficeID.subscribe(UseFilters);
		pageFilter.FilterSoldHomeListingAgentID.subscribe(UseFilters);
		pageFilter.FilterBeginDate.subscribe(ValidateDates);
		pageFilter.FilterEndDate.subscribe(ValidateDates);
		UseFilters();

		listingModel.startDateValid = ko.observable(true);
		listingModel.endDateValid = ko.observable(true);

		appendToListing = function (index, item, thisListing) {
			thisListing.Address = item.Address;
			thisListing.AgentFirstAndLast = item.AgentFirstAndLast;
			thisListing.CloseDate = item.CloseDate;
			thisListing.NeighborhoodName = item.NeighborhoodName;
			thisListing.OfficeName = item.OfficeName;
			thisListing.SalePrice = item.SalePrice;
		}

		listingModel.totalSales = ko.observable(0);
		afterLoad = function (data) {
			listingModel.totalSales(data.d.TotalSales);
		};

		function ValidateDates() {
			if ((!isNaN(Date.parse(pageFilter.FilterBeginDate())) && !isNaN(Date.parse(pageFilter.FilterEndDate())) && Date.parse(pageFilter.FilterBeginDate()) <= Date.parse(pageFilter.FilterEndDate())) || pageFilter.FilterBeginDate() == "" || pageFilter.FilterEndDate() == "") {
				listingModel.startDateValid(true);
				listingModel.endDateValid(true);
				UseFilters();
			}
			else if (!isNaN(Date.parse(pageFilter.FilterBeginDate())) && pageFilter.FilterBeginDate() != "")
				listingModel.startDateValid(false);
			else if ((!isNaN(Date.parse(pageFilter.FilterEndDate())) && pageFilter.FilterEndDate() != "") || Date.parse(pageFilter.FilterBeginDate()) < Date.parse(pageFilter.FilterEndDate()))
				listingModel.endDateValid(false);
		}	
	</script>
</asp:Content>
