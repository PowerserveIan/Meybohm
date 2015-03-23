<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SpamPrevention.ascx.cs" Inherits="Controls_BaseControls_SpamPrevention" %>

<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/slideLock.css" id="uxCSSFiles" />
<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery-ui.custom.min.js,~/tft-js/jquery.slideLock.js"></asp:Literal>
<script type="text/javascript">// <![CDATA[
	$(document).ready(function () {
		$("form").slideLock({
			// set the options - all are given, not all are required
			labelText: "Slide to Unlock:",
			noteText: "Proves you're a human.",
			lockText: "Locked",
			unlockText: "Unlocked",
			onCSS: "#333",
			offCSS: "#aaa",
			iconURL: "<%= ResolveClientUrl("~/img/") %>arrow_right.png",
			submitID: "[id*=<%= SubmitClientIDName %>]"
		});
		$("#dynamicsubmit").click(function () {
			if ($(this).attr("disabled") == "disabled")
				alert("Please slide the lock to continue");
		});
	});
	// ]]>
</script>
