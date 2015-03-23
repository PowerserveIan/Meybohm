<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminShowcaseItem" CodeFile="admin-showcase-item.aspx.cs" Title="Admin - Showcase Item Manager" %>

<%@ Import Namespace="BaseCode" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder">
		<asp:PlaceHolder runat="server" ID="uxOfficeFilterPH">
			<div class="column">
				<asp:DropDownList runat="server" ID="uxFilterByOfficeID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterShowcaseItemOfficeID">
					<asp:ListItem Text="--All Offices--" Value=""></asp:ListItem>
				</asp:DropDownList>
			</div>
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="uxRentalStatusPH">
			<div class="column">
				<asp:DropDownList runat="server" ID="uxFilterByRentalStatus" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterShowcaseItemRented">
					<asp:ListItem Text="--All Rental Statuses--" Value=""></asp:ListItem>
					<asp:ListItem Text="Rented" Value="true"></asp:ListItem>
					<asp:ListItem Text="Unrented" Value="false"></asp:ListItem>
				</asp:DropDownList>
			</div>
		</asp:PlaceHolder>
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByAgentID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterShowcaseItemAgentID">
				<asp:ListItem Text="--All Agents--" Value=""></asp:ListItem>
			</asp:DropDownList>
		</div>
		<div class="column">
			<asp:DropDownList runat="server" ID="uxFilterByNeighborhoodID" AppendDataBoundItems="true" data-bind="value: pageFilter.FilterShowcaseItemNeighborhoodID">
				<asp:ListItem Text="--All Neighborhoods--" Value=""></asp:ListItem>
			</asp:DropDownList>
		</div>
	</asp:PlaceHolder>
	<div class="blue">
		<h4>Note that your Showcase items will not show up on the front end until
			<asp:Literal runat="server" ID="uxFilterMessage" Text=" you have applied filters to the Showcase item and"></asp:Literal>
			media items have been added to the Showcase item.</h4>
		<div class="clear"></div>
	</div>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<asp:PlaceHolder runat="server" ID="uxNonRentalPH">
					<th class="first">
						<a href="#" class="sort" data-bind="sorting: 'Title'">Title</a>
					</th>
					<th>
						<a href="#" class="sort" data-bind="sorting: 'MlsID'">MLS ID</a>
					</th>
				</asp:PlaceHolder>
				<asp:PlaceHolder runat="server" ID="uxRentalPH">
					<th class="first">
						<a href="#" class="sort" data-bind="sorting: 'Address.Address1'">Address</a>
					</th>
					<th>
						<a href="#" class="sort" data-bind="sorting: 'Agent.Name'">Listing Agent</a>
					</th>
				</asp:PlaceHolder>
				<th>
					Summary
				</th>
				<th>
					Image
				</th>
				<th>
					<a href="#" class="sort" data-bind="sorting: 'Neighborhood.Name'">Neighborhood</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="sorting: 'Active'">Active</a>
				</th>
				<th>
					<a href="#" class="sort" data-bind="sorting: 'Featured'">Featured</a>
				</th>
				<th style="width: 120px;">
					Options
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css: { odd: index() % 2 == 0 }">
				<asp:PlaceHolder runat="server" ID="uxNonRentalPH2">
					<td class="first" data-bind='html: Title'>
					</td>
					<td data-bind='html: MlsID'>
					</td>
				</asp:PlaceHolder>
				<asp:PlaceHolder runat="server" ID="uxRentalPH2">
					<td class="first" data-bind='html: Address1'>
					</td>
					<td data-bind='html: AgentName'>
					</td>
				</asp:PlaceHolder>
				<td data-bind='html: Summary'>
				</td>
				<td>
					<img data-bind='attr: { alt: Title, src: ImageSrc }' alt="" src="../../img/loading.gif" />
				</td>
				<td data-bind='html: Neighborhood != null ? Neighborhood.Name : ""'>
				</td>
				<td>
					<a href='#' class="icon noText" data-bind="click: toggleActive, css: { active: Active(), inactive: !Active() }, text: activeText(), attr: { title: activeText() }"></a>
				</td>
				<td>
					<a href='#' class="icon noText" data-bind="click: toggleFeatured, css: { active: Featured(), inactive: !Featured() }, text: featuredText(), attr: { title: featuredText() }"></a>
				</td>
				<td>
					<a data-bind='attr: { href: linkToEditPage + Id + listingModel.returnString() }' class="icon edit">Edit</a>
					<a href='#' class="icon delete or" data-bind="click: function () { if (confirm('Are you sure you want to delete this ' + entityClassName + '?')) deleteRecord(); }">Delete</a>
					<input type='hidden' data-bind="value: Id" /><br />
					<a data-bind='attr: { href: "admin-media-collection.aspx?FilterMediaCollectionShowcaseItemID=" + Id }' class="icon view">View Collections</a>
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no items to edit.<asp:Literal runat="server" ID="uxAddMessage" Text=" Add an item by clicking the Add New button above."></asp:Literal></h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		pageFilter.FilterShowcaseItemAgentID = ko.observable("");
		pageFilter.FilterShowcaseItemNeighborhoodID = ko.observable("");
		pageFilter.FilterShowcaseItemOfficeID = ko.observable("");
		pageFilter.FilterShowcaseItemRented = ko.observable("");
		pageFilter.FilterShowcaseItemAgentID.subscribe(UseFilters);
		pageFilter.FilterShowcaseItemNeighborhoodID.subscribe(UseFilters);
		pageFilter.FilterShowcaseItemOfficeID.subscribe(UseFilters);
		pageFilter.FilterShowcaseItemRented.subscribe(UseFilters);
		UseFilters();

		appendToListing = function (index, item, thisListing) {
			thisListing.Address1 = item.Address ? item.Address.Address1 : '';
			thisListing.AgentName = item.Agent ? item.Agent.Name : '';
			thisListing.ImageSrc = "../../" + (item.Image.indexOf("http") >= 0 ? "resizer.aspx?filename=" : "<%=BaseCode.Globals.Settings.UploadFolder%>images/") + item.Image + (item.Image.indexOf("http") >= 0 ? "&" : "?") + "width=90&height=90" + (item.Image.indexOf("http") >= 0 ? "&trim=1" : "&mode=crop&anchor=middlecenter");
		}

		<% if (!String.IsNullOrEmpty(uxFilterByOfficeID.SelectedValue) || !String.IsNullOrEmpty(uxFilterByAgentID.SelectedValue) || !String.IsNullOrEmpty(uxFilterByNeighborhoodID.SelectedValue) || !String.IsNullOrEmpty(uxFilterByRentalStatus.SelectedValue))
	 { %>
		listingModel.readyToReloadListing(false);
		$(document).ready(function () {
			pageFilter.FilterShowcaseItemOfficeID("<%=uxFilterByOfficeID.SelectedValue%>");
			pageFilter.FilterShowcaseItemAgentID("<%=uxFilterByAgentID.SelectedValue%>");
			pageFilter.FilterShowcaseItemNeighborhoodID("<%=uxFilterByNeighborhoodID.SelectedValue%>");
			pageFilter.FilterShowcaseItemRented("<%=uxFilterByRentalStatus.SelectedValue%>");
			listingModel.readyToReloadListing(true);
		});<%} %>
	</script>
</asp:Content>
