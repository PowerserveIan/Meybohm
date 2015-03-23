<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="NewHomesSold" CodeFile="new-homes-sold.aspx.cs" Title="Admin - New Homes Sold" %>

<%@ Register TagPrefix="Controls" TagName="DateTimePicker" Src="~/Controls/BaseControls/DateTimePicker.ascx" %>
<%@ Register TagPrefix="Showcase" TagName="AdminSellNewHome" Src="~/Controls/Showcase/AdminSellNewHome.ascx" %>
<%@ Register Src="~/Controls/State_And_Country/Address.ascx" TagPrefix="Controls" TagName="Address" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder">
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByShowcaseID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterShowcaseID">
				<asp:ListItem Text="--All Markets--" Value=""></asp:ListItem>
			</asp:DropDownList>
			<asp:DropDownList runat="server" ID="uxFilterByBuilderID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterBuilderID">
				<asp:ListItem Text="--All Builders--" Value=""></asp:ListItem>
			</asp:DropDownList>
		</div>
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByAgentID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterSoldHomeListingAgentID">
				<asp:ListItem Text="--All Agents--" Value=""></asp:ListItem>
			</asp:DropDownList>
			<div class="date">
				<label for="<%= uxBeginDate.ClientID %>_uxDate">
					Begin Date</label>
				<Controls:DateTimePicker runat="server" ID="uxBeginDate" TextBoxCssClass="text" />
			</div>
		</div>
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByNeighborhoodID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterNeighborhoodID">
				<asp:ListItem Text="--All Neighborhoods--" Value=""></asp:ListItem>
			</asp:DropDownList>
			<div class="date">
				<label for="<%= uxEndDate.ClientID %>_uxDate">
					End Date</label>
				<Controls:DateTimePicker runat="server" ID="uxEndDate" TextBoxCssClass="text" />
			</div>
		</div>
	</asp:PlaceHolder>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'MlsID' && sortDirection(), descending: sortField() == 'MlsID' && !sortDirection()}, click: function(){listingModel.setSort('MlsID')}">MLS ID</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'ListingAgent.Email' && sortDirection(), descending: sortField() == 'ListingAgent.Email' && !sortDirection()}, click: function(){listingModel.setSort('ListingAgent.Email')}">Agent</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'ShowcaseItem.Neighborhood.Name' && sortDirection(), descending: sortField() == 'ShowcaseItem.Neighborhood.Name' && !sortDirection()}, click: function(){listingModel.setSort('ShowcaseItem.Neighborhood.Name')}">
						Neighborhood</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'ShowcaseItem.Builder.Name' && sortDirection(), descending: sortField() == 'ShowcaseItem.Builder.Name' && !sortDirection()}, click: function(){listingModel.setSort('ShowcaseItem.Builder.Name')}">
						Builder</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'CloseDate' && sortDirection(), descending: sortField() == 'CloseDate' && !sortDirection()}, click: function(){listingModel.setSort('CloseDate')}">Date Closed</a>
				</th>
				<th style="width: 120px;">
					Options
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css:{odd:index() % 2 == 0}">
				<td class="first" data-bind="html:ShowcaseItem.MlsID">
				</td>
				<td data-bind="html:ListingAgentFirstAndLast">
				</td>
				<td data-bind="html:ShowcaseItem.Neighborhood != null ? ShowcaseItem.Neighborhood.Name : ''">
				</td>
				<td data-bind="html:ShowcaseItem.Builder != null ? ShowcaseItem.Builder.Name : ''">
				</td>
				<td data-bind="html:FormatDate(CloseDate, 'MM/d/yyyy')">
				</td>
				<td>
					<a data-bind="click: function(){EditSaleRecord(ShowcaseItem.MlsID);}" class="icon edit editSale" href="#sellHome">Edit</a>
					<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this ' + entityClassName + '?'))deleteRecord();}">Delete</a>
					<input type='hidden' data-bind="value:Id" />
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no sales to edit. Add a sale by clicking the Add New button above.</h4>
	<div style="display: none;">
		<div id="sellHome">
			<div class="formWrapper">
				<h1>Record New Home Sale</h1>
				<div class="formWhole">
					<asp:TextBox runat="server" ID="uxHomeSearchByMLS" Placeholder="Enter MLS # or Address Line 1" CssClass="text" Width="50%" />
					<a href="#" class="button search" id="searchByMLS"><span>Find</span></a>
					<span style="display: none; color: #f00" id="notFound">
						<br />
						Could not find a property with that MLS ID #.  Please check the number and try again.</span>
				</div>
				<a href="#" id="hideAddress" style="display: none;">Hide Address Information</a>
				<div id="addressPanel" style="display: none;">
					<Controls:Address runat="server" ID="uxAddress" ReadOnly="true" ShowAddress2="true" ShowLatAndLong="false" />
				</div>
				<div class="clear"></div>
				<div id="saleDetails">
					<Showcase:AdminSellNewHome runat="server" ID="uxSoldHomeControl" />
					<asp:HiddenField runat="server" ID="uxShowcaseItemID" />
					<div class="clear"></div>
					<asp:Button runat="server" ID="uxSaveSoldHome" Text="Save Sale" CssClass="button save" Style="display: none;" />
				</div>
			</div>
			<div class="clear"></div>
		</div>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		pageFilter.FilterShowcaseID = ko.observable("");
		pageFilter.FilterSoldHomeListingAgentID = ko.observable("");
		pageFilter.FilterNeighborhoodID = ko.observable("");
		pageFilter.FilterBuilderID = ko.observable("");
		pageFilter.FilterBeginDate = ko.observable("");
		pageFilter.FilterEndDate = ko.observable("");
		pageFilter.FilterShowcaseID.subscribe(UseFilters);
		pageFilter.FilterSoldHomeListingAgentID.subscribe(UseFilters);
		pageFilter.FilterNeighborhoodID.subscribe(UseFilters);
		pageFilter.FilterBuilderID.subscribe(UseFilters);
		pageFilter.FilterBeginDate.subscribe(ValidateDates);
		pageFilter.FilterEndDate.subscribe(ValidateDates);
		UseFilters();

		listingModel.startDateValid = ko.observable(true);
		listingModel.endDateValid = ko.observable(true);

		appendToListing = function (index, item, thisListing) {
			thisListing.Id = item.SoldHomeID;
			thisListing.ShowcaseItem = item.ShowcaseItem;
			thisListing.ListingAgentFirstAndLast = item.ListingAgentFirstAndLast;
			thisListing.CloseDate = item.CloseDate;
			thisListing.ShowcaseItemID = item.ShowcaseItemID;
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

		function EditSaleRecord(id) {
			$("#<%= uxHomeSearchByMLS.ClientID %>").val(id);
			$("#searchByMLS").trigger("click");
		}

		$(document).ready(function () {
			$("#<%= m_AddButton.ClientID %>, .editSale").fancybox({ afterClose: function () { ClearEntireForm(); $("#<%= uxHomeSearchByMLS.ClientID %>").val(""); } });
			$("#<%= uxHomeSearchByMLS.ClientID %>").keydown(function (e) {
				var key = e.charCode || e.keyCode || 0;
				if (key == 13)
					$("#searchByMLS").trigger("click");
				return key != 13;
			});
			$("#searchByMLS").click(function () {
				if ($("#<%= uxHomeSearchByMLS.ClientID %>").val() == "") {
					ClearEntireForm();
					return false;
				}
				$.ajax({
					type: "POST",
					url: defaultPageUrl + '/GetSoldHomeInfo',
					data: '{"mlsIDOrAddress":"' + $("#<%= uxHomeSearchByMLS.ClientID %>").val() + '"}',
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: function (data) {
						if (data.d) {
							$("#notFound").hide();
							$("#<%= uxShowcaseItemID.ClientID %>").val(data.d.ShowcaseItemID);
							$("[id$=uxAddress_uxAddress]").val(data.d.Address.Address1);
							$("[id$=uxAddress_uxAddress2]").val(data.d.Address.Address2);
							$("[id$=uxAddress_uxCity]").val(data.d.Address.City);
							$("[id$=uxAddress_uxStateID]").val(data.d.Address.StateID);
							$("[id$=uxAddress_uxZip_uxZipcode]").val(data.d.Address.Zip);
							$("#addressPanel").slideDown(200, function () {
								$("#hideAddress, #<%= uxSaveSoldHome.ClientID %>").show();
							});
							if (data.d.SoldHome) {
								$("[id$=uxSoldHomeCloseDate_uxDate]").val(FormatDate(data.d.SoldHome.CloseDate, 'MM/d/yyyy'));
								$("[id$=uxSoldHomeSalePrice]").val(data.d.SoldHome.SalePrice);
								$("[id$=uxSoldHomeListingAgent]").val(data.d.SoldHome.ListingAgentID);
								$("[id$=uxSoldHomeSellerPercentage]").val(data.d.SoldHome.SellerPercentage);
								$("[id$=uxSoldHomeSellerOffice]").val(data.d.SoldHome.SellerOfficeID);
								$("[id$=uxSoldHomeSellerOfficePercentage]").val(data.d.SoldHome.SellerOfficePercentage);
								$("[id$=uxSoldHomeSalesAgent]").val(data.d.SoldHome.SalesAgentID);
								$("[id$=uxSoldHomeSalesAgentPercentage]").val(data.d.SoldHome.SalesAgentPercentage);
							}
							else
								ClearNewHomeSaleForm();
						}
						else {
							ClearEntireForm();
							$("#notFound").show();
						}
					}
				});
				return false;
			});
			$("#hideAddress").click(function () {
				$("#addressPanel").slideToggle(200, function () {
					if ($("#addressPanel").is(":visible"))
						$("#hideAddress").html("Hide Address Information");
					else
						$("#hideAddress").html("Show Address Information");
				});
				return false;
			});

			function ClearEntireForm() {
				$("#sellHome select, #sellHome input[type=text]:not(#<%= uxHomeSearchByMLS.ClientID %>)").val("");
				$("#notFound, #addressPanel, #hideAddress, #<%= uxSaveSoldHome.ClientID %>").hide();
			}

			function ClearNewHomeSaleForm() {
				$("#saleDetails select, #saleDetails input[type=text]").val("");
			}
		});
	</script>
</asp:Content>
