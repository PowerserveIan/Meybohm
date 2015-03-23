<%@ Page Title="Saved Searches" Language="C#" MasterPageFile="~/microsite.master" AutoEventWireup="true" CodeFile="saved-searches.aspx.cs" Inherits="saved_searches" %>

<%@ Register TagPrefix="Showcase" TagName="AddSavedSearch" Src="~/Controls/Showcase/AddSavedSearch.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<span data-bind="visible: numberPages() > 1" class="pagination">
		<a data-bind="css:{aspNetDisabled: pageNumber() == 1}, click: function(){if (pageNumber() &gt; 1)pageNumber(pageNumber()-1);}" class="prev">Previous</a>
		<ul data-bind="foreach: pages" class="clearfix">
			<li>
				<span data-bind="visible: listingModel.pageNumber() == number(), text: number"></span>
				<a data-bind="visible: listingModel.pageNumber() != number(), click: changePage, text: number" href="#"></a>
			</li>
		</ul>
		<a data-bind="css:{aspNetDisabled: pageNumber() == numberPages()}, click: function(){if (pageNumber() < numberPages())pageNumber(pageNumber()+1);}" class="next">Next</a>
	</span>
	<div class="savedSearchWrap">
		<table class="listing" data-bind="visible: listings().length > 0">
			<thead>
				<tr>
					<th class="first">
						Image
					</th>
					<th>
						<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'Name' && sortDirection(), descending: sortField() == 'Name' && !sortDirection()}, click: function(){listingModel.setSort('Name')}">Name</a>
					</th>
					<th>
						<a href="#" class="sort date" data-bind="css:{ascending: sortField() == 'Created' && sortDirection(), descending: sortField() == 'Created' && !sortDirection()}, click: function(){listingModel.setSort('Created')}">Date</a>
					</th>
					<th>
						<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'EnableEmailNotifications' && sortDirection(), descending: sortField() == 'EnableEmailNotifications' && !sortDirection()}, click: function(){listingModel.setSort('EnableEmailNotifications')}">
							Notifications</a>
					</th>
					<th>
						<span class="savedSearchOptions">Options</span>
					</th>
				</tr>
			</thead>
			<tbody data-bind="foreach: listings">
				<tr data-bind="css:{odd:index() % 2 == 0}">
					<td class="first">
						<img data-bind="attr:{alt:Name,src:ImageSrc}" alt="" src="<%= ResolveClientUrl("~/img/") %>loading.gif" width="120" height="90" />
					</td>
					<td>
						<a href="#" data-bind="html:Name,attr:{href:ShowcaseItemID ? 'home-details?id=' + ShowcaseItemID : SearchPageUrl + '?' + FilterString},css:{'fancybox.iframe showcaseProject': ShowcaseItemID}"></a>
					</td>
					<td data-bind="html:FormatDate(CreatedClientTime, 'MM/d/yyyy')">
					</td>
					<td data-bind="html: EnableEmailNotifications ? 'Yes' : 'No'">
					</td>
					<td>
						<a href="#addSearchDiv" class="icon edit editSavedSearch" data-bind="click: function(){UpdateEditedSearch(Id);}">Edit</a>
						<a href='#' class="icon delete or" data-bind="click: function(){if(confirm('Are you sure you want to delete this Saved Search?'))deleteRecord();}">Delete</a>
						<input type='hidden' data-bind="value:Id" />
					</td>
				</tr>
			</tbody>
		</table>
	</div>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">You have not saved any searches yet.</h4>
	<span data-bind="visible: numberPages() > 1" class="pagination">
		<a data-bind="css:{aspNetDisabled: pageNumber() == 1}, click: function(){if (pageNumber() &gt; 1)pageNumber(pageNumber()-1);}" class="prev">Previous</a>
		<ul data-bind="foreach: pages" class="clearfix">
			<li>
				<span data-bind="visible: listingModel.pageNumber() == number(), text: number"></span>
				<a data-bind="visible: listingModel.pageNumber() != number(), click: changePage, text: number" href="#"></a>
			</li>
		</ul>
		<a data-bind="css:{aspNetDisabled: pageNumber() == numberPages()}, click: function(){if (pageNumber() < numberPages())pageNumber(pageNumber()+1);}" class="next">Next</a>
	</span>
	<Showcase:AddSavedSearch runat="server" ID="uxAddSavedSearch" Editing="true" />
	<script type="text/javascript">
		var columnNumberToMakeLink = 0;
		var defaultPageNumber = 1;
		var defaultPageSize = 10;
		var defaultSearchText = '';
		var defaultSortField = defaultBaseSortField = 'Created';
		var defaultSortDirection = false;
		var defaultPageUrl = '<%= ResolveClientUrl("~/saved-searches.aspx") %>';

	  

		

	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="PageSpecificJS" runat="Server">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/knockout.js,~/tft-js/core/jquery.dateformat.js,~/tft-js/core/pagination.js,~/tft-js/core/admin-listing.js"></asp:Literal>
	<script type="text/javascript">
		
		pageFilter.FilterSavedSearchNewHomeSearch = ko.observable("");
		pageFilter.FilterSavedSearchNewHomeSearch.subscribe(UseFilters);
		listingModel.readyToReloadListing(false);
		$(document).ready(function () {
			pageFilter.FilterSavedSearchNewHomeSearch("<%= true %>");
			listingModel.readyToReloadListing(true);
		});
		

		appendToListing = function (index, item, thisListing) {
			thisListing.Id = item.SavedSearchID;
			thisListing.ImageSrc = "<%= ResolveClientUrl("~/") %>" + (item.Image ? (item.Image.indexOf("http") >= 0 ? "resizer.aspx?filename=" : "<%=BaseCode.Globals.Settings.UploadFolder%>images/") + item.Image + (item.Image.indexOf("http") >= 0 ? "&" : "?") + "width=120&height=90" + (item.Image.indexOf("http") >= 0 ? "&trim=1" : "&mode=crop&anchor=middlecenter") : "img/genericSearch.jpg");
		};
		$(document).ready(function () {
			$("body").addClass("theme-new");
			$(".showcaseProject").fancybox(homeDetailsFancyboxParams);
		});
	
	</script>

</asp:Content>
