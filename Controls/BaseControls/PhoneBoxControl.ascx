<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PhoneBoxControl.ascx.cs" Inherits="Controls_BaseControls_PhoneBoxControl" %>
<asp:TextBox runat="server" ID="uxPhoneBox" MaxLength="21" />
<asp:CheckBox runat="server" ID="uxInternationalNumber" Text="International Number?" CssClass="labelReset" />
<div class="clear"></div>
<asp:CustomValidator ID="uxPhoneBoxREV" runat="server" ControlToValidate="uxPhoneBox" OnServerValidate="uxPhoneBoxREV_ServerValidate" ClientValidationFunction="ClientValidate" ErrorMessage="Invalid phone number." />
<asp:RequiredFieldValidator runat="server" ID="uxPhoneBoxRFV" ControlToValidate="uxPhoneBox" ErrorMessage="Phone number is required."  />
<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery.maskedinput.js"></asp:Literal>
<script type="text/javascript">
	//<![CDATA[
	function ClientValidate_<%=ClientID%>(source, args) {
		var inputValue = $("#<%=uxPhoneBox.ClientID%>").val();
		var regexpression;
		if ($("#<%=uxInternationalNumber.ClientID%>").is(":checked")) 
			regexpression = new RegExp("^(\\+)?((\\s)?(\\()?(\\))?([0-9x])?(\\-)?)+$");			
		else 
			regexpression = new RegExp("^\\(\\d{3}\\)\\d{3}-\\d{4}");
		
		args.IsValid = inputValue == "" || regexpression.test(inputValue);
	}

	function ToggleMask_<%=ClientID %>(){
		if ($("#<%= uxInternationalNumber.ClientID %>").is(":checked"))
			$("#<%= uxPhoneBox.ClientID %>").unmask();
		else
			$("#<%= uxPhoneBox.ClientID %>").mask("(999)999-9999<% if (ShowExtension){ %>? x99999<%} %>");
	}

	$(document).ready(function() {
		$("#<%= uxInternationalNumber.ClientID %>").click(function(){
			ToggleMask_<%=ClientID %>();
		});

		ToggleMask_<%=ClientID %>();
	});
	//]]>
</script>
