<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminWhatsNearByCategory" CodeFile="admin-whats-near-by-category.aspx.cs" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder"></asp:PlaceHolder>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="sorting: 'Name'">Name</a>
				</th>
				<th>
					Default Image
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
			<tr data-bind="css:{odd:index() % 2 == 0}">
				<td class="first" data-bind='html:Name'>
				</td>
				<td>
					<!--ko if:PlaceholderImage-->
					<img data-bind='attr: { alt: Name, src: ImageSrc }' alt="" src="../../img/loading.gif" />
					<!--/ko-->
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
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no "What's Near By" Categories to edit. Add a "What's Near By" Category by clicking the Add New button above.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		appendToListing = function (index, item, thisListing) {
			thisListing.Id = item.WhatsNearByCategoryID;
			thisListing.Name = item.Name;
			thisListing.PlaceholderImage = item.PlaceholderImage;
			if (item.PlaceholderImage)
				thisListing.ImageSrc = "../../" + (item.PlaceholderImage.indexOf("http") >= 0 ? "resizer.aspx?filename=" : "<%=BaseCode.Globals.Settings.UploadFolder%>nearByLocations/") + item.PlaceholderImage + (item.PlaceholderImage.indexOf("http") >= 0 ? "&" : "?") + "width=90&height=90" + (item.PlaceholderImage.indexOf("http") >= 0 ? "&trim=1" : "&mode=crop&anchor=middlecenter");
		}
	</script>
</asp:Content>
