<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminCMSubmittedForm" CodeFile="admin-cm-submitted-form.aspx.cs" Title="Admin - Submitted Form Manager" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<asp:PlaceHolder runat="server" ID="uxMicrositePlaceHolder" Visible="false">
		<h1>
			<asp:Literal runat="server" ID="uxMicroSiteName" /></h1>
		<div style="clear: both; padding-bottom: 10px;">
			<asp:DropDownList runat="server" ID="uxMicrositeList" AutoPostBack="true" />
		</div>
	</asp:PlaceHolder>
	<asp:Label runat="server" ID="uxMicrositeInactive" Text="The microsite you are managing has been disabled.  Please contact the admin to have it reenabled." Visible="false" />
	<asp:PlaceHolder runat="server" ID="uxHideThisIfMSAdmin">
		<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
		<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
		<%--The markup for pagination can be found in the BaseListingPage--%>
		<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder"></asp:PlaceHolder>
		<table class="listing" data-bind="visible: listings().length > 0">
			<thead>
				<tr>
					<th class="first">
						<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'DateSubmitted' && sortDirection(), descending: sortField() == 'DateSubmitted' && !sortDirection()}, click: function(){listingModel.setSort('DateSubmitted')}">Date Submitted</a>
					</th>
					<th>
						<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'FormRecipient' && sortDirection(), descending: sortField() == 'FormRecipient' && !sortDirection()}, click: function(){listingModel.setSort('FormRecipient')}">Form Recipient</a>
					</th>
					<th>
						<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'IsProcessed' && sortDirection(), descending: sortField() == 'IsProcessed' && !sortDirection()}, click: function(){listingModel.setSort('IsProcessed')}">Is Processed</a>
					</th>
					<th style="width: 120px;">
					Options
				</th>
				</tr>
			</thead>
			<tbody data-bind="foreach: listings">
				<tr data-bind="css:{odd:index() % 2 == 0}">
					<td class="first" data-bind='html:FormatDate(DateSubmittedClientTime, "MMMM d, yyyy h:mm tt")'>						
					</td>
					<td data-bind='html:FormRecipient'>
					</td>
					<td data-bind='html:IsProcessed ? "Yes" : "No"'>
					</td>
					<td>
						<a data-bind='attr:{href:linkToEditPage + Id + listingModel.returnString()}' class="icon edit">Edit</a>
						<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this ' + entityClassName + '?'))deleteRecord();}">Delete</a>
						<input type='hidden' data-bind="value:Id" />
					</td>
				</tr>
			</tbody>
		</table>
		<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no Submitted Forms.</h4>
	</asp:PlaceHolder>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		pageFilter.FilterCMSubmittedFormCMMicrositeID = ko.observable("");
		pageFilter.FilterCMSubmittedFormCMMicrositeID.subscribe(function (newValue) {
			listingModel.readyToReloadListing(false);
			listingModel.filter((newValue == '' ? new Object() : pageFilter));
			listingModel.readyToReloadListing(true);
		});
		appendToListing = function (index, item, thisListing) {
			thisListing.DateSubmittedClientTime = item.DateSubmittedClientTime;
			thisListing.FormRecipient = item.FormRecipient;
			thisListing.IsProcessed = item.IsProcessed;
			thisListing.Id = item.CMSubmittedFormID;
		}
		<% if (!String.IsNullOrEmpty(uxMicrositeList.SelectedValue)){ %>
		listingModel.readyToReloadListing(false);
		$(document).ready(function () {
			pageFilter.FilterCMSubmittedFormCMMicrositeID("<%=uxMicrositeList.SelectedValue%>");
		});<%} %>
	</script>
</asp:Content>
