<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DateTimePicker.ascx.cs" Inherits="Controls_BaseControls_DateTimePicker" %>

<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/timepicker.css" id="uxCSSFiles" />
<asp:TextBox runat="server" ID="uxDate" />
<asp:CustomValidator runat="server" ID="uxDateREV" ControlToValidate="uxDate" ValidateEmptyText="false" ErrorMessage="Invalid date format." ClientValidationFunction="ValidateDate" />
<asp:RequiredFieldValidator runat="server" ID="uxDateRFV" ControlToValidate="uxDate" ErrorMessage="Date is required." />
<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery-ui-timepicker-addon.js"></asp:Literal>
<% if (Enabled){ %>
<script type="text/javascript">
	var minDate_<%=ClientID %> = <% if (MinDate.HasValue){ %>new Date(<%= MinDate.Value.Year %>, <%= MinDate.Value.Month - 1 %>, <%= MinDate.Value.Day %>)<%}else{ %>null<%} %>;
	var maxDate_<%=ClientID %> = <% if (MaxDate.HasValue){ %>new Date(<%= MaxDate.Value.Year %>, <%= MaxDate.Value.Month - 1 %>, <%= MaxDate.Value.Day %>)<%}else{ %>null<%} %>;
	var dotsToRoot_<%=ClientID %> = "<%= m_DotsToRoot %>";

	var defaultDateOptions_<%= ClientID %> = {
		<% if (PickerStyle == Picker.DateOnly || PickerStyle == Picker.DateTime){ %>
		changeMonth: true,
		changeYear: true,
		minDate: minDate_<%=ClientID %>,
		maxDate: maxDate_<%=ClientID %>,
		showOn: 'both',
		buttonImage: dotsToRoot_<%=ClientID %> + 'img/datepicker.gif',
		buttonImageOnly: true<% if (PickerStyle == Picker.DateTime){ %>,
		ampm: true<% } %>
		<% } else{%>
		ampm: true,
		showOn: 'both',
		buttonImage: dotsToRoot_<%=ClientID %> + 'img/timepicker.gif',
		buttonImageOnly: true<% } %>
	};
	
	function ValidateDate_<%=ClientID%>(source, args) {
		var value = $("#<%= uxDate.ClientID %>").val();
		if (value.replace(/^\\s+|\\s+$/, '').length != 0 && isNaN(Date.parse(value)) && isNaN(Date.parse('1/1/2010 ' + value)))
			args.IsValid = false;
	}
	$(document).ready(function () {			
		<% if (PickerStyle == Picker.DateOnly){ %>$("#<%= uxDate.ClientID %>").datepicker(defaultDateOptions_<%= ClientID %>);
		<%}else if (PickerStyle == Picker.DateTime){ %>$("#<%= uxDate.ClientID %>").datetimepicker(defaultDateOptions_<%= ClientID %>);
		<% } else{ %>$("#<%= uxDate.ClientID %>").timepicker(defaultDateOptions_<%= ClientID %>);<% } %>
	});
</script><%} %>
