<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="AdminRetsTaskStatus" CodeFile="admin-rets-task-status.aspx.cs" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, and search panel) can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<%--The markup for pagination can be found in the BaseListingPage--%>
	<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder"></asp:PlaceHolder>
	<table class="listing" data-bind="visible: listings().length > 0">
		<thead>
			<tr>
				<th class="first" width="25">
					Time
				</th>
				<th width="150">
					Error Message
				</th>

				<th>
					Method
				</th>
				<th>
					MlsID
				</th>
				<th>
					City
				</th>
				<th>
					Showcase
				</th>
				<th>
					Step
				</th>
			</tr>
		</thead>
		<tbody data-bind="foreach: listings">
			<tr data-bind="css:{odd:index() % 2 == 0}">
				<td class="first" data-bind='html:FormatDate(ErrorTimeUtc, "M/d/yyyy h:MM tt")'>
				</td>
				<td data-bind='html:ErrorMessage'>
				</td>
				<td data-bind='html:Method'>
				</td>
				<td data-bind='html:MlsID'>
				</td>
				<td data-bind='html:City'>
				</td>
				<td data-bind='html:Showcase'>
				</td>
				<td data-bind='html:Step'>
				</td>
			</tr>
		</tbody>
	</table>
	<h4 class="indent paddingTop" data-bind="visible: listings().length == 0">This task has not run yet.</h4>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		appendToListing = function (index, item, thisListing) {
			thisListing.ErrorMessage = item.ErrorMessage;
			thisListing.ErrorData = item.ErrorData;
			thisListing.ErrorTimeUtc = item.ErrorTimeUtc;
			thisListing.Method = item.Method;
			thisListing.MlsID = item.MlsID;
			thisListing.City = item.City;
			thisListing.Showcase = item.Showcase;
			thisListing.Step = item.Step;
			columnNumberToMakeLink = 0;
		}
	</script>
</asp:Content>
