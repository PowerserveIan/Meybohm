<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminShowcaseItem" CodeFile="property-inventory.aspx.cs" Title="Admin - Property Inventory and Statistics" %>

<%@ Register TagPrefix="Controls" TagName="DateTimePicker" Src="~/Controls/BaseControls/DateTimePicker.ascx" %>
<asp:Content ContentPlaceHolderID="PageSpecificCSS" runat="server">
	<style>
		#viewStats span.label {
			display: inline-block;
		}

		#viewStats ul {
			list-style-type: none;
			margin: 0;
		}

		.realtor {
			text-align: center;
			padding-right: 25px;
		}

		.realtor span {
			display: block;
		}

		.realtorName {
			font-size: 15px;
		}

		.realtorNumber {
			color: #851521;
			font-weight: bold;
		}
	</style>
</asp:Content>
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
			<asp:DropDownList runat="server" ID="uxFilterByPropertyType" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterPropertyType">
				<asp:ListItem Text="--All Home Types--" Value=""></asp:ListItem>
			</asp:DropDownList>
			<div class="date">
				<label for="<%= uxBeginDate.ClientID %>_uxDate">
					List Date Begin</label>
				<Controls:DateTimePicker runat="server" ID="uxBeginDate" TextBoxCssClass="text" />
			</div>
		</div>
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByOfficeID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterShowcaseItemOfficeID">
				<asp:ListItem Text="--All Offices--" Value=""></asp:ListItem>
			</asp:DropDownList>
			<asp:TextBox runat="server" ID="uxPriceRangeMin" CssClass="text numbersOnly" Placeholder="Minimum Price" data-bind="value: pageFilter.FilterListPriceMin" />
			<div class="date">
				<label for="<%= uxEndDate.ClientID %>_uxDate">
					List Date End</label>
				<Controls:DateTimePicker runat="server" ID="uxEndDate" TextBoxCssClass="text" />
			</div>
		</div>
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByAgentID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterShowcaseItemAgentID">
				<asp:ListItem Text="--All Agents--" Value=""></asp:ListItem>
			</asp:DropDownList>
			<asp:TextBox runat="server" ID="uxPriceRangeMax" CssClass="text numbersOnly" Placeholder="Maximum Price" data-bind="value: pageFilter.FilterListPriceMax" />
		</div>
	</asp:PlaceHolder>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'MlsID' && sortDirection(), descending: sortField() == 'MlsID' && !sortDirection()}, click: function(){listingModel.setSort('MlsID')}">MLS ID</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Showcase.Title' && sortDirection(), descending: sortField() == 'Showcase.Title' && !sortDirection()}, click: function(){listingModel.setSort('Showcase.Title')}">Market</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Agent.Email' && sortDirection(), descending: sortField() == 'Agent.Email' && !sortDirection()}, click: function(){listingModel.setSort('Agent.Email')}">Agent</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'DateListed' && sortDirection(), descending: sortField() == 'DateListed' && !sortDirection()}, click: function(){listingModel.setSort('DateListed')}">List Date</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Address.Address1' && sortDirection(), descending: sortField() == 'Address.Address1' && !sortDirection()}, click: function(){listingModel.setSort('Address.Address1')}">Address</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'ListPrice' && sortDirection(), descending: sortField() == 'ListPrice' && !sortDirection()}, click: function(){listingModel.setSort('ListPrice')}">Price</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'PropertyType' && sortDirection(), descending: sortField() == 'PropertyType' && !sortDirection()}, click: function(){listingModel.setSort('PropertyType')}">Property Type</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'NumberOfVisits' && sortDirection(), descending: sortField() == 'NumberOfVisits' && !sortDirection()}, click: function(){listingModel.setSort('NumberOfVisits')}">Visits</a>
				</th>
				<th style="width: 120px;">
					Options
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css:{odd:index() % 2 == 0}">
				<td class="first" data-bind='html:MlsID'>
				</td>
				<td data-bind='html:ShowcaseID == aikenExistingShowcaseID ? "Aiken" : "Augusta"'>
				</td>
				<td data-bind='html:AgentInfo ? AgentInfo.FirstAndLast : ""'>
				</td>
				<td data-bind="html:DateListed ? FormatDate(DateListed, 'MM/d/yyyy') : ''">
				</td>
				<td data-bind='html:Address.Address1'>
				</td>
				<td data-bind='html:"$" + ListPrice.toLocaleString()'>
				</td>
				<td data-bind='html:PropertyType'>
				</td>
				<td data-bind='html:NumberOfVisits'>
				</td>
				<td>
					<a data-bind='click: function(){UpdateStats($data);}' class="icon edit" href="#viewStats">View Stats</a>
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no properties that meet your search criteria.</h4>
	<div style="display: none;">
		<div id="viewStats">
			<div class="formWrapper" style="width: 375px;">
				<!-- ko if: statsModel.CurrentProperty() -->
				<!-- ko if: statsModel.CurrentProperty().AgentInfo -->
				<div class="realtor floatLeft">
					<img alt="" src="../../img/loading.gif" data-bind='attr:{alt:(statsModel.CurrentProperty().AgentInfo ? statsModel.CurrentProperty().AgentInfo.FirstAndLast : ""),src:statsModel.AgentPhotoSrc()}, visible: statsModel.AgentPhotoSrc()' width="134" height="168" />
					<span class="realtorName" data-bind="html: statsModel.CurrentProperty().AgentInfo ? statsModel.CurrentProperty().AgentInfo.FirstAndLast : ''"></span>
					<span class="realtorNumber" data-bind="html: statsModel.CurrentProperty().AgentInfo ? statsModel.CurrentProperty().AgentInfo.CellPhone : ''"></span>
				</div>
				<!-- /ko -->				
				<div class="floatLeft">
					<span class="label">MLS ID #:</span>
					<span data-bind="html: statsModel.CurrentProperty().MlsID"></span>
					<br />
					<span class="label">Address:</span>
					<span data-bind="html: statsModel.CurrentProperty().Address.Address1"></span>
					<br />
					<span class="label">MLS Market:</span>
					<span data-bind="html: statsModel.CurrentProperty().ShowcaseID == aikenExistingShowcaseID ? 'Aiken' : 'Augusta'"></span>
					<br />
					<span class="label">List Date:</span>
					<span data-bind="html: statsModel.CurrentProperty().DateListed ? FormatDate(statsModel.CurrentProperty().DateListed, 'MM/d/yyyy') : ''"></span>
					<br />
					<span class="label">Price:</span>
					<span data-bind="html: '$' + statsModel.CurrentProperty().ListPrice.toLocaleString()"></span>
				</div>
				<!-- /ko -->
				<div class="clear"></div>
				<hr />
				<ul data-bind="foreach: statsModel.PropertyStats">
					<li>Number of <span class="label" data-bind="html: key"></span>&nbsp;Clicks: <strong data-bind="html: value"></strong>
					</li>
				</ul>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		var aikenExistingShowcaseID = <%= (int)Classes.Showcase.MeybohmShowcases.AikenExistingHomes %>;
		pageFilter.FilterShowcaseItemAgentID = ko.observable("");
		pageFilter.FilterShowcaseItemShowcaseID = ko.observable("");
		pageFilter.FilterShowcaseItemOfficeID = ko.observable("");
		pageFilter.FilterPropertyType = ko.observable("");
		pageFilter.FilterListPriceMin = ko.observable("");
		pageFilter.FilterListPriceMax = ko.observable("");
		pageFilter.FilterBeginDate = ko.observable("<%= uxBeginDate.SelectedDate.HasValue ? uxBeginDate.SelectedDate.Value.ToShortDateString() : "" %>");
		pageFilter.FilterEndDate = ko.observable("<%= uxEndDate.SelectedDate.HasValue ? uxEndDate.SelectedDate.Value.ToShortDateString() : "" %>");

		pageFilter.FilterShowcaseItemAgentID.subscribe(UseFilters);
		pageFilter.FilterShowcaseItemShowcaseID.subscribe(UseFilters);
		pageFilter.FilterShowcaseItemOfficeID.subscribe(UseFilters);
		pageFilter.FilterPropertyType.subscribe(UseFilters);
		pageFilter.FilterListPriceMin.subscribe(UseFilters);
		pageFilter.FilterListPriceMax.subscribe(UseFilters);
		pageFilter.FilterBeginDate.subscribe(ValidateDates);
		pageFilter.FilterEndDate.subscribe(ValidateDates);
		UseFilters();

		listingModel.startDateValid = ko.observable(true);
		listingModel.endDateValid = ko.observable(true);

		appendToListing = function (index, item, thisListing) {
			thisListing.Id = item.ShowcaseItemID;
			thisListing.MlsID = item.MlsID;
			thisListing.ShowcaseID = item.ShowcaseID;
			thisListing.AgentInfo = item.AgentInfo;
			thisListing.DateListed = item.DateListed;
			thisListing.Address = item.Address;
			thisListing.ListPrice = item.ListPrice;
			thisListing.NumberOfVisits = item.NumberOfVisits;
			thisListing.PropertyType = item.PropertyType;
		}

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

		listingModel.statsModel = ko.observable({});
		listingModel.statsModel.CurrentProperty = ko.observable(null);
		listingModel.statsModel.PropertyStats = ko.observableArray([]);
		listingModel.statsModel.AgentPhotoSrc = ko.dependentObservable({
			read: function () {
				var agentInfo = listingModel.statsModel.CurrentProperty() ? listingModel.statsModel.CurrentProperty().AgentInfo : null;
				return agentInfo && agentInfo.Photo && agentInfo.Photo != '' ? "../../" + (agentInfo.Photo.indexOf("http") >= 0 ? "resizer.aspx?filename=" : "<%=BaseCode.Globals.Settings.UploadFolder%>agents/") + agentInfo.Photo + (agentInfo.Photo.indexOf("http") >= 0 ? "&" : "?") + "width=134&height=168" + (agentInfo.Photo.indexOf("http") >= 0 ? "&trim=1" : "&mode=crop&anchor=middlecenter") : "";
			}
		});

		function UpdateStats(listingItem){			
			listingModel.statsModel.CurrentProperty(listingItem);
			$.ajax({
				type: "POST",
				url: defaultPageUrl + '/GetPropertyStats',
				data: '{"showcaseItemID":"' + listingItem.Id + '", "beginDateStr":"' + pageFilter.FilterBeginDate() + '", "endDateStr":"' + pageFilter.FilterEndDate() + '"}',
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (data) {
					listingModel.statsModel.PropertyStats.removeAll();
					for (var key in data.d) {
						if (data.d.hasOwnProperty(key))
							listingModel.statsModel.PropertyStats.push({ key: key, value: data.d[key] }); 
					}
				}
			});
		}

		$(document).ready(function() {
			$(".edit").fancybox();
		});
	</script>
</asp:Content>
