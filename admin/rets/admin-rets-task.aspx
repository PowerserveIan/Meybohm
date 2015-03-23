<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminRetsTask" CodeFile="admin-rets-task.aspx.cs" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder"></asp:PlaceHolder>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first">
					<a href="#" class="sort" data-bind="css:{ascending: sortField() == 'TaskName' && sortDirection(), descending: sortField() == 'TaskName' && !sortDirection()}, click: function(){listingModel.setSort('TaskName')}">Task Name</a>
				</th>
				<th style="width: 180px;">
					Last Scheduled Task Successful
				</th>
				<th style="width: 180px;" class="childOnly">
					Last Successful Attempt
				</th>
				<th style="width: 180px;" class="childOnly">
					Last Attempt
				</th>
				<th style="width: 120px;">
					Options
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css:{odd:index() % 2 == 0}">
				<td class="first" data-bind='html:TaskName'>
				</td>
				<td>
					<img alt="" data-bind="attr:{src: imgSrc}" />
				</td>
				<td class="childOnly" data-bind='visible:showChildOnly, html:LastSuccessfulTime!=null?FormatDate(LastSuccessfulTime, "MMMM d, yyyy h:MM tt"):""'></td>
				<td class="childOnly" data-bind='visible:showChildOnly, html:LastAttemptTime!=null?FormatDate(LastAttemptTime, "MMMM d, yyyy h:MM tt"):""'></td>
				<td>
					<a data-bind='attr:{href:linkToEditPage + Id + listingModel.returnString()}' class="icon edit view">View</a>
					<input type='hidden' data-bind="value:Id" />
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">No tasks exist matching the filter criteria.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		appendToListing = function(index, item, thisListing) {
			thisListing.Id = item.RetsTaskID;
			thisListing.TaskName = item.TaskName;
			thisListing.LastRunSuccessful = item.LastRunSuccessful;
			thisListing.LastSuccessfulTime = item.LastSuccessfulTime;
			thisListing.LastAttemptTime = item.LastAttemptTime;
			thisListing.imgSrc = item.LastRunSuccessful ? '../img/Active.png' : '../img/inactive.png';
			thisListing.showChildOnly = <%= (!string.IsNullOrEmpty(ParentID)).ToString().ToLower()%>;
		};
		<% if (string.IsNullOrEmpty(ParentID))
	 {%>
		$(document).ready(function() {
			$('.childOnly').hide();
		});<% } %>
	</script>
</asp:Content>
