<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminShowcases" CodeFile="admin-showcases.aspx.cs" Title="Admin - Showcase Manager" %>

<%@ Import Namespace="Classes.Showcase" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder"></asp:PlaceHolder>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Title' && sortDirection(), descending: sortField() == 'Title' && !sortDirection()}, click: function(){listingModel.setSort('Title')}">Title</a>
				</th>
				<th runat="server" id="uxActiveHeaderPlaceHolder">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Active' && sortDirection(), descending: sortField() == 'Active' && !sortDirection()}, click: function(){listingModel.setSort('Active')}">Active</a>
				</th>
				<th runat="server" id="uxManagedByHeaderPlaceHolder">
					Managed By
				</th>
				<th style="width: 250px;">
					Options
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css:{odd:index() % 2 == 0}">
				<td class="first" data-bind='html:Title'>
				</td>
				<asp:PlaceHolder runat="server" ID="uxShowcaseAdminOnlyPH">
					<td>
						<a href='#' class="icon noText" data-bind="click: toggleActive, css: {active: Active(), inactive: !Active() }, text:activeText(),attr:{title:activeText()}"></a>
					</td>
					<td data-bind='html:ManagersString'>
					</td>
				</asp:PlaceHolder>
				<td>
					<a data-bind='attr:{href:linkToEditPage + Id + listingModel.returnString()}' class="icon edit">Edit</a>
					<asp:PlaceHolder runat="server" ID="uxShowcaseAdminOnly2PH">
						<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this ' + entityClassName + '?'))deleteRecord();}">Delete</a>
					</asp:PlaceHolder>
					<input type='hidden' data-bind="value:Id" />
					<a data-bind='attr:{href:"admin-showcase-item.aspx?ShowcaseID=" + Id}' class="icon view or">Modify Showcase</a>
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no Showcases to edit. Add a Showcase by clicking the Add New button above.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		appendToListing = function (index, item, thisListing) {
			thisListing.Id = item.ShowcaseID;
			thisListing.ManagersString = item.ManagersString;
			thisListing.Title = item.Title;
		}
	</script>
</asp:Content>
