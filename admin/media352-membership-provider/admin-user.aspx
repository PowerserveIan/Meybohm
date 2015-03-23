<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminUser" CodeFile="admin-user.aspx.cs" Title="Admin - User Manager" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:Button runat="server" ID="uxDownloadCustomers" Text="Export Customers to CSV" CssClass="button export" />
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder"></asp:PlaceHolder>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first" runat="server" id="uxUserNameIsEmailHeaderPH">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Name' && sortDirection(), descending: sortField() == 'Name' && !sortDirection()}, click: function(){listingModel.setSort('Name')}">User Name</a>
				</th>
				<th runat="server" id="uxEmailHeader">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Email' && sortDirection(), descending: sortField() == 'Email' && !sortDirection()}, click: function(){listingModel.setSort('Email')}">Email</a>
				</th>
				<th runat="server" id="uxUserInfoHeaderFirstName">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'UserInfo.FirstName' && sortDirection(), descending: sortField() == 'UserInfo.FirstName' && !sortDirection()}, click: function(){listingModel.setSort('UserInfo.FirstName')}">First Name</a>
				</th>
				<th runat="server" id="uxUserInfoHeaderLastName">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'UserInfo.LastName' && sortDirection(), descending: sortField() == 'UserInfo.LastName' && !sortDirection()}, click: function(){listingModel.setSort('UserInfo.LastName')}">Last Name</a>
				</th>
				<th style="width: 120px;">
					Options
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css:{odd:index() % 2 == 0}">
				<asp:PlaceHolder runat="server" ID="uxUserNameNotEmailPH">
					<td class="first" data-bind="html:Name">
					</td>
				</asp:PlaceHolder>
				<td runat="server" id="uxEmailTD" data-bind="html:Email">
				</td>
				<asp:PlaceHolder runat="server" ID="uxComplexPH">
					<td data-bind="html:FirstName">
					</td>
					<td data-bind="html:LastName">
					</td>
				</asp:PlaceHolder>
				<td>
					<a data-bind='attr:{href:linkToEditPage + Id + listingModel.returnString()}' class="icon edit">Edit</a>
					<!-- ko if:Name != currentUserName-->
					<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this ' + entityClassName + '?'))deleteRecord();}">Delete</a>
					<!-- /ko -->
					<input type='hidden' data-bind="value:Id" />
					<!-- ko if:Locked() || FailedPasswordAttempts > 5-->
					<a href='#' class="icon unlock or" data-bind="click: function(){if (confirm('Are you sure you want to unlock this user?'))toggleLocked();},visible:Locked">Unlock</a>
					<!-- /ko -->
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">There are no Users to edit. Add a User by clicking the Add New button above.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		<% if (Staff.HasValue)
	 { %>
		pageFilter.FilterUserHasRole = ko.observable("<%= Staff.ToString().ToLower() %>");
		<% if (!String.IsNullOrEmpty(RoleName))
	 { %>pageFilter.FilterUserRoleName = ko.observable("<%= RoleName %>");<% } %>
		listingModel.filter(pageFilter);
		<% } %>
		var currentUserName = "<%= User.Identity.Name %>";
		appendToListing = function (index, item, thisListing) {
			thisListing.Email = item.Email;
			thisListing.FailedPasswordAttempts = ko.observable(item.FailedPasswordAttempts);
			thisListing.FirstName = item.FirstName;
			thisListing.Id = item.UserID;
			thisListing.LastName = item.LastName;
			thisListing.Locked = ko.observable(item.Locked);
			thisListing.Name = item.Name;
			thisListing.toggleLocked = function () {
				$.ajax({
					type: "POST",
					url: defaultPageUrl + '/ToggleLocked',
					data: '{id:' + this.Id + '}',
					contentType: "application/json; charset=utf-8"
				});
				this.Locked(!this.Locked());
				this.FailedPasswordAttempts(0);
				return false;
			};
		}
	</script>
</asp:Content>
