<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminShowcaseAttributeValue" CodeFile="admin-attribute-value.aspx.cs" Title="Admin - Attribute Value Manager" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<asp:PlaceHolder runat="server" ID="uxCustomBreadCrumbsPH">
		<li>
			<a runat="server" href="~/admin/showcase/admin-attribute.aspx">
				<asp:Literal runat="server" ID="uxShowcaseName"></asp:Literal>Attribute Manager</a></li>
	</asp:PlaceHolder>
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder"></asp:PlaceHolder>
	<div class="blue" data-bind="visible: listings().length > 0 || $('.blue h4:visible').length > 0">
		<div data-bind="visible: listings().length > 0">
			<h4>To change the display order of your attribute values, click Edit Display Orders and then change the values in the textboxes below. The lowest number will display first.</h4>
			<a href="#" class="button edit" data-bind="click:function(){listingModel.displayOrderEditableChanged(true);},visible:!listingModel.displayOrderEditableChanged()"><span>Edit Display Orders</span></a>
			<a href="#" class="button save" data-bind="click:function(){listingModel.saveDisplayOrders();},visible:listingModel.displayOrderEditableChanged()"><span>Save Display Orders</span></a>
		</div>
		<asp:Literal runat="server" ID="uxDistanceFilterText" Text="<h4>The slider on the frontend will populate based on the minimum and maximum values.  You do not need to specify each individual distance.</h4>"></asp:Literal>
		<div class="clear"></div>
	</div>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'DisplayOrder' && sortDirection(), descending: sortField() == 'DisplayOrder' && !sortDirection()}, click: function(){listingModel.setSort('DisplayOrder')}">Display Order</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Value' && sortDirection(), descending: sortField() == 'Value' && !sortDirection()}, click: function(){listingModel.setSort('Value')}">Value</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="css: { ascending: sortField() == 'DisplayInFilters' && sortDirection(), descending: sortField() == 'DisplayInFilters' && !sortDirection() }, click: function () { listingModel.setSort('DisplayInFilters') }">Display in Filters</a>
				</th>
				<th style="width: 120px;">
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
				<td data-bind='html:Value'>
				</td>
				<td>
					<a href='#' class="icon noText" data-bind="click: toggleDisplayInFilters, css: { active: DisplayInFilters(), inactive: !DisplayInFilters() }, text: displayInFiltersText(), attr: { title: displayInFiltersText() }"></a>
				</td>
				<td>
					<a data-bind='attr:{href:linkToEditPage + Id + listingModel.returnString()}' class="icon edit">Edit</a>
					<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this ' + entityClassName + '?'))deleteRecord();}">Delete</a>
					<input type='hidden' data-bind="value:Id" />
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no Attribute Values to edit.<asp:Literal runat="server" ID="uxAddMessage" Text=" Add an Attribute Value by clicking the Add New button above."></asp:Literal></h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		pageFilter.FilterShowcaseAttributeValueShowcaseAttributeID = ko.observable(<%= ShowcaseAttributeId %>);
		listingModel.filter(pageFilter);
		listingModel.additionalDisplayOrderFilter = ko.dependentObservable(function () {
			return ', "attributeID": ' + pageFilter.FilterShowcaseAttributeValueShowcaseAttributeID();
		}, listingModel);
		appendToListing = function (index, item, thisListing) {
			thisListing.Id = item.ShowcaseAttributeValueID;
			thisListing.Value = item.Value;
			thisListing.DisplayInFilters = ko.observable(item.DisplayInFilters);
			thisListing.displayInFiltersText = ko.dependentObservable({
				read: function () {
					return thisListing.DisplayInFilters() ? "Yes" : "No";
				}
			});
			thisListing.toggleDisplayInFilters = function () {
				$.ajax({
					type: "POST",
					url: defaultPageUrl + '/ToggleDisplayInFilters',
					data: '{id:' + this.Id + '}',
					contentType: "application/json; charset=utf-8"
				});
				thisListing.DisplayInFilters(!thisListing.DisplayInFilters());
				return false;
			};
		}
	</script>
</asp:Content>
