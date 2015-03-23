<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RichTextEditor.ascx.cs" Inherits="Controls_BaseControls_RichTextEditor" %>
<asp:HyperLink runat="server" ID="uxToggleEditor" NavigateUrl="#"><img runat="server" src="~/admin/img/btnIcons/icon_edit.png" alt="Edit Content" /></asp:HyperLink>
<asp:Label runat="server" ID="uxEditorText"></asp:Label>
<asp:TextBox runat="server" ID="uxEditorTextBox" CssClass="tinymce" TextMode="MultiLine" Width="480" />
<asp:RequiredFieldValidator runat="server" ID="uxEditorTextBoxRFV" ControlToValidate="uxEditorTextBox" />
<asp:RegularExpressionValidator runat="server" ID="uxEditorTextBoxREV" ControlToValidate="uxEditorTextBox" Enabled="false" />
<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/tiny_mce/jquery.tinymce.js,~/tft-js/core/tinymceinit.js"></asp:Literal>
<script type="text/javascript">
	//<![CDATA[
	$(document).ready(function () {
		<% if (!HideEditorInitially)
	 { %>InitializeTinyMCE('<%= uxEditorTextBox.ClientID %>', 2);<%} %>
		$("#<%= uxToggleEditor.ClientID %>").click(function () {
			InitializeTinyMCE('<%= uxEditorTextBox.ClientID %>', 2);
			$('#<%= uxEditorText.ClientID %>').remove();
			$(this).remove();
			$("#<%= uxEditorTextBox.ClientID %>").removeClass("hidden");
			return false;
		});
	});
	//]]>
</script>
