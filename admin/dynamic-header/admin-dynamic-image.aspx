<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminDynamicImage" CodeFile="admin-dynamic-image.aspx.cs" Title="Admin - Dynamic Slide Manager" %>

<%@ Import Namespace="BaseCode" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (Title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder">
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByCollection" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterDynamicCollectionID">
				<asp:ListItem Text="--All Images--" Value=""></asp:ListItem>
				<asp:ListItem Text="--Images Not Assigned to Collection--" Value="NULL"></asp:ListItem>
			</asp:DropDownList>
		</div>
	</asp:PlaceHolder>
	<div class="blue" data-bind="visible: !listingModel.hideDisplayOrder()">
		<h4>To change the display order of your slides, click Edit Display Orders and then change the values in the textboxes below.<br />
			The lowest number will display first.</h4>
		<a href="#" class="button edit" data-bind="click:function(){listingModel.displayOrderEditableChanged(true);},visible:!listingModel.displayOrderEditableChanged()"><span>Edit Display Orders</span></a>
		<a href="#" class="button save" data-bind="click:function(){listingModel.saveDisplayOrders();},visible:listingModel.displayOrderEditableChanged()"><span>Save Display Orders</span></a>
		<div class="clear"></div>
	</div>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first" data-bind="visible: !listingModel.hideDisplayOrder()">
					<a href="#" class="sort" data-bind="sorting: 'DisplayOrder'">Display Order</a>
				</th>
				<th data-bind="css:{first:listingModel.hideDisplayOrder()}">
					<a href="#" class="sort" data-bind="sorting: 'Title'">Slide Title</a>
				</th>
				<th>
					Slide
				</th>
				<th>
					<a href="#" class="sort" data-bind="sorting: 'Active'">Active</a>
				</th>
				<th style="width: 120px;">
					Options
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css:{odd:index() % 2 == 0},style:{cursor:(!listingModel.hideDisplayOrder()&&listingModel.displayOrderEditable()?'move':'')}">
				<td data-bind="visible:!listingModel.hideDisplayOrder(),css:{first:!listingModel.hideDisplayOrder()}">
					<input type="text" class="text small displayOrder" maxlength="3" data-bind="value: displayOrder,visible: listingModel.displayOrderEditable(),css:{error:displayOrderInvalid()}" />
					<span data-bind="text: displayOrder,visible: !listingModel.displayOrderEditable()"></span>
				</td>
				<td data-bind="css:{first:listingModel.hideDisplayOrder()}, html:Title">
				</td>
				<td>
					<img data-bind='attr:{alt:Title,src:ImageSrc}' alt="" src="../../img/loading.gif" />
				</td>
				<td>
					<a href='#' class="icon noText" data-bind="click: toggleActive, css: {active: Active(), inactive: !Active() }, text:activeText(),attr:{Title:activeText()}"></a>
				</td>
				<td>
					<a data-bind='attr:{href:linkToEditPage + Id + listingModel.returnString()}' class="icon edit">Edit</a>
					<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this slide?'))deleteRecord();}">Delete</a>
					<input type='hidden' data-bind="value:Id" />
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no Slides to edit. Add a Slide by clicking the Add New button above.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		pageFilter.FilterDynamicCollectionID = ko.observable("");
		listingModel.hideDisplayOrder = ko.dependentObservable(function () {
			return pageFilter.FilterDynamicCollectionID() == '' || pageFilter.FilterDynamicCollectionID() == 'NULL' || (listingModel.totalCount() == 0 && listingModel.searchText() == "");
		}, listingModel);
		listingModel.additionalDisplayOrderFilter = ko.dependentObservable(function () {
			return ', "dynamicCollectionID": ' + pageFilter.FilterDynamicCollectionID();
		}, listingModel);
		var displayOrderHidden = listingModel.hideDisplayOrder();
		pageFilter.FilterDynamicCollectionID.subscribe(function (newValue) {
			listingModel.readyToReloadListing(false);
			listingModel.displayOrderEditable(false);
			listingModel.filter((newValue == '' ? new Object() : pageFilter));
			if (displayOrderHidden && (newValue != '' && newValue != 'NULL')) {
				listingModel.sortField("DisplayOrder").sortDirection(true);
				columnNumberToMakeLink = 2;
			}
			else if (listingModel.sortField() == "DisplayOrder" && (newValue == '' || newValue == 'NULL')) {
				listingModel.sortField("Title").sortDirection(true);				
				columnNumberToMakeLink = 1;
			}
			listingModel.pageNumber(1);
			listingModel.readyToReloadListing(true);
			displayOrderHidden = pageFilter.FilterDynamicCollectionID() == '' || pageFilter.FilterDynamicCollectionID() == 'NULL';
		});
		appendToListing = function (index, item, thisListing) {
			thisListing.ImageSrc = "../../<%=Globals.Settings.UploadFolder%>images/" + item.Name + "?width=90&height=90";
		}
		<% if (!String.IsNullOrEmpty(uxFilterByCollection.SelectedValue)){ %>
		listingModel.readyToReloadListing(false);
		$(document).ready(function () {
			pageFilter.FilterDynamicCollectionID("<%=uxFilterByCollection.SelectedValue%>");
		});<%} %>
	</script>
</asp:Content>
