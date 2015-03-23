<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminMediaCollection" CodeFile="admin-media-collection.aspx.cs" Title="Admin - Media Collection Manager" %>

<%@ Import Namespace="Classes.Showcase" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<asp:PlaceHolder runat="server" ID="uxCustomBreadCrumbsPH">
		<li>
			<a runat="server" href="~/admin/showcase/admin-showcase-item.aspx">
				<asp:Literal runat="server" ID="uxShowcaseName"></asp:Literal>
				Property Manager</a></li>
	</asp:PlaceHolder>
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder"></asp:PlaceHolder>
	<div class="blue" data-bind="visible: listings().length > 0">
		<h4>To change the display order of your media collections, click Edit Display Orders and then change the values in the textboxes below. The lowest number will display first.</h4>
		<a href="#" class="button edit" data-bind="click:function(){listingModel.displayOrderEditableChanged(true);},visible:!listingModel.displayOrderEditableChanged()"><span>Edit Display Orders</span></a>
		<a href="#" class="button save" data-bind="click:function(){listingModel.saveDisplayOrders();},visible:listingModel.displayOrderEditableChanged()"><span>Save Display Orders</span></a>
		<div class="clear"></div>
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
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'ShowcaseMediaTypeID' && sortDirection(), descending: sortField() == 'ShowcaseMediaTypeID' && !sortDirection()}, click: function(){listingModel.setSort('ShowcaseMediaTypeID')}">Media Type</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Active' && sortDirection(), descending: sortField() == 'Active' && !sortDirection()}, click: function(){listingModel.setSort('Active')}">Active</a>
				</th>
				<th style="width: 215px;">
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
				<td data-bind="html:Title">
				</td>
				<td data-bind="html:MediaTypeName">
				</td>
				<td>
					<a href='#' class="icon noText" data-bind="click: toggleActive, css: {active: Active(), inactive: !Active() }, text:activeText(),attr:{title:activeText()}"></a>
				</td>
				<td>
					<a data-bind='attr:{href:linkToEditPage + Id + listingModel.returnString()}' class="icon edit">Edit</a>
					<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this ' + entityClassName + '?'))deleteRecord();}">Delete</a>
					<input type='hidden' data-bind="value:Id" />
					<!-- ko if:ShowcaseMediaTypeID != textBlockTypeID-->
					<a data-bind='attr:{href:"admin-media.aspx?FilterMediaCollectionShowcaseItemID=" + pageFilter.FilterMediaCollectionShowcaseItemID() + "&FilterMediaShowcaseMediaCollectionID=" + Id}' class="icon view or">View Media</a>
					<!-- /ko -->
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no items to edit. Add an item by clicking the Add New button above.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		var textBlockTypeID = <%= (int)MediaTypes.TextBlock %>;
		pageFilter.FilterMediaCollectionShowcaseItemID = ko.observable(<%= ShowcaseItemId %>);
		listingModel.filter(pageFilter);
		listingModel.additionalDisplayOrderFilter = ko.dependentObservable(function () {
			return ', "showcaseItemID": ' + pageFilter.FilterMediaCollectionShowcaseItemID();
		}, listingModel);
		appendToListing = function (index, item, thisListing) {
			thisListing.Id = item.ShowcaseMediaCollectionID;
			thisListing.MediaTypeName = item.MediaTypeName;
			thisListing.ShowcaseMediaTypeID = item.ShowcaseMediaTypeID;
			thisListing.Title = item.Title;
		}
	</script>
</asp:Content>
