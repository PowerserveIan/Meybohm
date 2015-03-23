<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ZipcodeControl.ascx.cs" Inherits="Controls_BaseControls_ZipcodeControl" %>
<asp:TextBox runat="server" ID="uxZipcode" MaxLength="12"/>
<asp:CheckBox runat="server" ID="uxInternationalNumber" Text="International Postal Code?" CssClass="labelReset" />
<div class="clear"></div>
<asp:CustomValidator ID="uxZipcodeREV" runat="server" ControlToValidate="uxZipcode" OnServerValidate="uxZipcodeREV_ServerValidate" ClientValidationFunction="ClientValidate" ErrorMessage="Invalid Postal Code." />
<asp:RequiredFieldValidator runat="server" ID="uxZipcodeRFV" ControlToValidate="uxZipcode" ErrorMessage="Postal Code is required." />
<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery.maskedinput.js"></asp:Literal>
<script type="text/javascript">
	//<![CDATA[
	function ClientValidate_<%=ClientID%>(source, args) {
		var inputValue = $("#<%=uxZipcode.ClientID%>").val();
		var regexpression;
		if ($("#<%=uxInternationalNumber.ClientID%>").is(":checked"))
			regexpression = new RegExp("^\\w{0,12}$");
		else 
			regexpression = new RegExp("^\\d{5,}");
			
		args.IsValid = inputValue == "" || regexpression.test(inputValue);
	}

	function ToggleMask_<%=ClientID %>(){
		if ($("#<%= uxInternationalNumber.ClientID %>").is(":checked"))
			$("#<%= uxZipcode.ClientID %>").unmask();
		else
			$("#<%= uxZipcode.ClientID %>").mask("99999?-9999");
	}

	$(document).ready(function() {
		$("#<%= uxInternationalNumber.ClientID %>").click(function(){
			ToggleMask_<%=ClientID %>();
		});

		ToggleMask_<%=ClientID %>();
	});
	//]]>
</script>
