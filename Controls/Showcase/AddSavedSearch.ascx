<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddSavedSearch.ascx.cs" Inherits="Controls_Showcase_AddSavedSearch" %>

<div style="display: none;">
	<div id="addSearchDiv" class="addSavedSearch">
		<div class="formWrapper" id="savedSearchWrapper">
			<h2><%= Editing ? "Edit" : "Save" %><%= ShowcaseItemID.HasValue ? " this Listing" : " this Filter Set" %></h2>
			<hr />
			<div class="formWhole">
				<label for="<%= uxSavedSearchName.ClientID %>"><%= ShowcaseItemID.HasValue ? "Nickname this property" : "Name your search" %><span class="asterisk">*</span></label>
				<asp:TextBox runat="server" ID="uxSavedSearchName" CssClass="text" MaxLength="500" ClientIDMode="Static" />
				<asp:RequiredFieldValidator runat="server" ID="uxSavedSearchNameRFV" ControlToValidate="uxSavedSearchName" ErrorMessage="Please name your search." ValidationGroup="SavedSearch" />
			</div>
			<div class="formWhole">
				<span class="label">Email notifications for this <%= ShowcaseItemID.HasValue ? "property" : "search" %><span class="asterisk">*</span></span>
				<asp:RadioButtonList runat="server" ID="uxSavedSearchEmailNotifications">
					<asp:ListItem Text="On" Value="true"></asp:ListItem>
					<asp:ListItem Text="Off" Value="false"></asp:ListItem>
				</asp:RadioButtonList>
				<asp:CustomValidator runat="server" ID="uxSavedSearchEmailNotificationsRFV" ClientValidationFunction="ValidateEmailNotifications" ErrorMessage="Do you want email notifications for this search?" ValidationGroup="SavedSearch" />
			</div>
			<div class="formWhole" id="separateEmail" style="display: none;">
				<span class="label">Email notifications for this <%= ShowcaseItemID.HasValue ? "property" : "search" %> as a separate email<span class="asterisk">*</span></span>
				<asp:RadioButtonList runat="server" ID="uxSavedSearchSeparateEmail">
					<asp:ListItem Text="Yes" Value="true"></asp:ListItem>
					<asp:ListItem Text="No, include with my other searches" Value="false"></asp:ListItem>
				</asp:RadioButtonList>
				<asp:CustomValidator runat="server" ID="uxSavedSearchSeparateEmailRFV" ClientValidationFunction="ValidateSeparateEmail" ErrorMessage="Do you want email notifications for this search as a separate email?" ValidationGroup="SavedSearch" />
			</div>
			<div class="formWhole" id="dailyEmail" style="display: none;">
				<span class="label">I would like to receive this email<span class="asterisk">*</span></span>
				<asp:RadioButtonList runat="server" ID="uxSavedSearchDailyEmail">
					<asp:ListItem Text="Daily" Value="true"></asp:ListItem>
					<asp:ListItem Text="Weekly" Value="false"></asp:ListItem>
				</asp:RadioButtonList>
				<asp:CustomValidator runat="server" ID="uxSavedSearchDailyEmailRFV" ClientValidationFunction="ValidateDailyEmail" ErrorMessage="When do you want to receive this email?" ValidationGroup="SavedSearch" />
			</div>
			<a href="#" id="saveSavedSearch" class="button">Save this <%= ShowcaseItemID.HasValue ? "listing" : "search" %></a>
			<input id="savedSearchUrl" type="hidden" <% if (!Editing && !ShowcaseItemID.HasValue){ %>data-bind="value: viewModel.QueryString()" <% } %>/>
			<asp:HiddenField runat="server" ID="uxSavedSearchID" />
			<asp:HiddenField runat="server" ID="uxShowcaseItemID" />
			<asp:HiddenField runat="server" ID="uxShowcaseID" />
				<asp:HiddenField runat="server" ID="uxNewProperties" />
		</div>
		<span style="display: none;" id="savedSearchSaveMessage">Your search was saved.</span>
	</div>
</div>
<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/saved-search.js"></asp:Literal>
<% if (Editing){ %>
<script type="text/javascript">
	function UpdateEditedSearch(id) {
		var currentItem;
		for (var i = 0; i < listingModel.listings().length; i++) {
			if (listingModel.listings()[i].Id == id) {
				currentItem = listingModel.listings()[i];
				break;
			}
		}
		$("#<%= uxSavedSearchName.ClientID %>").val(currentItem.Name);
		$("input[id*=uxSavedSearchEmailNotifications]").each(function () {
			if ($(this).val() == currentItem.EnableEmailNotifications.toString())
				$(this).attr("checked", "checked");
			else
				$(this).removeAttr("checked");
		});
		
			$("#<%= uxNewProperties.ClientID %>").val(currentItem.NewHomeSearch);
	
		if (currentItem.EnableEmailNotifications)
			$("#separateEmail, #dailyEmail").show();
		$("input[id*=uxSavedSearchSeparateEmail]").each(function () {
			if ($(this).val() == currentItem.SeparateEmail.toString())
				$(this).attr("checked", "checked");
			else
				$(this).removeAttr("checked");
		});
		$("input[id*=uxSavedSearchDailyEmail]").each(function () {
			if ($(this).val() == currentItem.DailyEmail.toString())
				$(this).attr("checked", "checked");
			else
				$(this).removeAttr("checked");
		});
		$("#<%= uxSavedSearchID.ClientID %>").val(currentItem.Id);
	}
</script>
<% } %>