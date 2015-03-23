<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminMedia" CodeFile="admin-media.aspx.cs" Title="Admin - Media Manager" %>

<%@ Import Namespace="BaseCode" %>
<%@ Import Namespace="Classes.Showcase" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<asp:PlaceHolder runat="server" ID="uxCustomBreadCrumbsPH">
		<li>
			<a runat="server" href="~/admin/showcase/admin-showcase-item.aspx">
				<asp:Literal runat="server" ID="uxShowcaseName"></asp:Literal>
				Property Manager</a></li>
		<li>
			<asp:HyperLink runat="server" ID="uxLinkToMediaCollectionManager"></asp:HyperLink></li>
	</asp:PlaceHolder>
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder"></asp:PlaceHolder>
	<div class="blue" data-bind="visible: listings().length > 0">
		<h4>To change the display order of your media items, click Edit Display Orders and then change the values in the textboxes below. The lowest number will display first.</h4>
		<a href="#" class="button edit" data-bind="click:function(){listingModel.displayOrderEditableChanged(true);},visible:!listingModel.displayOrderEditableChanged()"><span>Edit Display Orders</span></a>
		<a href="#" class="button save" data-bind="click:function(){listingModel.saveDisplayOrders();},visible:listingModel.displayOrderEditableChanged()"><span>Save Display Orders</span></a>
		<div class="clear"></div>
	</div>
	<table class="listing" data-bind="visible: showData()">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="sorting: 'DisplayOrder'">Display Order</a>
				</th>
				<th>
					Caption
				</th>
				<th>
					Thumbnail
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
			<td class="first">
				<input type="text" class="text small displayOrder" maxlength="3" data-bind="value: displayOrder,visible: listingModel.displayOrderEditable(),css:{error:displayOrderInvalid()}" />
				<span data-bind="text: displayOrder,visible: !listingModel.displayOrderEditable()"></span>
			</td>
			<td data-bind='html:Caption'>
			</td>
			<td>
				<!-- ko if:Thumbnail != null && Thumbnail != ""-->
				<img data-bind='attr:{alt:Caption,src:ThumbnailSrc}' alt="" src="../../img/loading.gif" />
				<!-- /ko -->
				<!-- ko if:MediaTypeID == imageMediaTypeID-->
				<img data-bind='attr:{alt:Caption,src:ImageSrc}' alt="" src="../../img/loading.gif" />
				<!-- /ko -->
			</td>
			<td>
				<a href='#' class="icon noText" data-bind="click: toggleActive, css: {active: Active(), inactive: !Active() }, text:activeText(),attr:{title:activeText()}"></a>
			</td>
			<td>
				<a data-bind='attr:{href:linkToEditPage + Id + listingModel.returnString()}' class="icon edit">Edit</a>
				<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this ' + entityClassName + '?'))deleteRecord();}">Delete</a>
				<input type='hidden' data-bind="value:Id" />
			</td>
		</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0 && !listingModel.isLoading()">There are no items to edit. Add an item by clicking the Add New button above.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		var imageMediaTypeID = <%= (int)MediaTypes.Image %>;
		pageFilter.FilterMediaShowcaseMediaCollectionID = ko.observable(<%= MediaCollectionId %>);
		listingModel.filter(pageFilter);
		listingModel.additionalDisplayOrderFilter = ko.dependentObservable(function () {
			return ', "collectionID": ' + pageFilter.FilterMediaShowcaseMediaCollectionID();
		}, listingModel);
		appendToListing = function (index, item, thisListing) {
			thisListing.Id = item.ShowcaseMediaID;
			thisListing.ThumbnailSrc = "http://img.youtube.com/vi/" + item.Thumbnail + "/2.jpg";
			thisListing.ImageSrc = "../../" + (item.URL.indexOf("http") >= 0 ? "resizer.aspx?filename=" : "<%=BaseCode.Globals.Settings.UploadFolder%>images/") + item.URL + (item.URL.indexOf("http") >= 0 ? "&" : "?") + "width=90&height=90" + (item.URL.indexOf("http") >= 0 ? "&trim=1" : "&mode=crop&anchor=middlecenter");
		}
	</script>
</asp:Content>
