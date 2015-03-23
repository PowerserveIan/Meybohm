<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminShowcaseAttribute" CodeFile="admin-attribute.aspx.cs" Title="Admin - Attribute Manager" %>

<%@ Import Namespace="Classes.Showcase" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder"></asp:PlaceHolder>
	<div class="blue">
		<h4>Setting an attribute to inactive will remove it from the list of filters, but the attribute and its value will still display in the home's popup page.</h4>
		<div data-bind="visible: listings().length > 0">
			<hr />
			<h4>To change the display order of your images, click Edit Display Orders and then change the values in the textboxes below. The lowest number will display first.</h4>
			<a href="#" class="button edit" data-bind="click:function(){listingModel.displayOrderEditableChanged(true);},visible:!listingModel.displayOrderEditableChanged()"><span>Edit Display Orders</span></a>
			<a href="#" class="button save" data-bind="click:function(){listingModel.saveDisplayOrders();},visible:listingModel.displayOrderEditableChanged()"><span>Save Display Orders</span></a>
			<div class="clear"></div>
		</div>
	</div>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'DisplayOrder' && sortDirection(), descending: sortField() == 'DisplayOrder' && !sortDirection()}, click: function(){listingModel.setSort('DisplayOrder')}">Display Order</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Title' && sortDirection(), descending: sortField() == 'Title' && !sortDirection()}, click: function(){listingModel.setSort('Title')}">Title</a>
				</th>
				<th id="uxFilterTHPlaceHolder" runat="server">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'ShowcaseFilterID' && sortDirection(), descending: sortField() == 'ShowcaseFilterID' && !sortDirection()}, click: function(){listingModel.setSort('ShowcaseFilterID')}">Filter Type</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Numeric' && sortDirection(), descending: sortField() == 'Numeric' && !sortDirection()}, click: function(){listingModel.setSort('Numeric')}">Numeric</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Active' && sortDirection(), descending: sortField() == 'Active' && !sortDirection()}, click: function(){listingModel.setSort('Active')}">Active</a>
				</th>
				<th style="width: 175px;">
					Options
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css:{odd:index() % 2 == 0},style:{cursor:(!listingModel.hideDisplayOrder()&&listingModel.displayOrderEditable()?'move':'')}">
				<td class="first">
					<input type="text" class="text small displayOrder" maxlength="3" data-bind="value: displayOrder,visible: listingModel.displayOrderEditable(),css:{error:displayOrderInvalid()}" />
					<span data-bind="text: displayOrder,visible: !listingModel.displayOrderEditable()"></span>
				</td>
				<td data-bind='html:Title'>
				</td>
				<asp:PlaceHolder runat="server" ID="uxFilterPH">
					<td data-bind='html:FilterType'>
					</td>
				</asp:PlaceHolder>
				<td data-bind='html:Numeric ? "Yes" : "No"'>
				</td>
				<td>
					<a href='#' class="icon noText" data-bind="click: toggleActive, css: {active: Active(), inactive: !Active() }, text:activeText(),attr:{title:activeText()}"></a>
				</td>
				<td>
					<a data-bind='attr:{href:linkToEditPage + Id + listingModel.returnString()}' class="icon edit">Edit</a>
					<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this ' + entityClassName + '?'))deleteRecord();}">Delete</a>
					<input type='hidden' data-bind="value:Id" />
					<!-- ko if:!Numeric || ShowcaseFilterID == distanceFilterID || ShowcaseFilterID == distanceRangeFilterID-->
					<a data-bind='attr:{href:"admin-attribute-value.aspx?FilterShowcaseAttributeValueShowcaseAttributeID=" + Id}' class="icon view or">View</a>
					<!-- /ko -->
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no Attributes to edit. Add an Attribute by clicking the Add New button above.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		var distanceFilterID = <%= (int)FilterTypes.Distance %>;
		var distanceRangeFilterID = <%= (int)FilterTypes.DistanceRange %>;
		appendToListing = function (index, item, thisListing) {
			thisListing.FilterType = item.FilterType;
			thisListing.Id = item.ShowcaseAttributeID;
			thisListing.Numeric = item.Numeric;
			thisListing.ShowcaseFilterID = item.ShowcaseFilterID;
			thisListing.Title = item.Title;
		}
	</script>
</asp:Content>
