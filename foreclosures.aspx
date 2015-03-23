<%@ Page Language="C#" MasterPageFile="~/microsite.master" AutoEventWireup="true" CodeFile="foreclosures.aspx.cs" Inherits="foreclosures" Title="Foreclosures" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<h1>Foreclosures in <%= ((microsite)Master).CurrentMicrosite.Name %></h1>
	<script src="http://agents.realtytrac.com/foreclosuresearchwidget.ashx" type="text/javascript"></script>
	<script type="text/javascript">renderForeclosureSearchWidget(114596158, 0, '<%= Zipcode %>');</script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		var suppressOnBeforeUnload = false;
		window.onbeforeunload = confirmForeclosureExit;
		function confirmForeclosureExit(e) {
			if (!suppressOnBeforeUnload)
				return 'You are being taken away from Meybohm.com to an external website.  Do you want to continue?';
		}
		$(document).ready(function () {
			$("a, input[type=submit]").click(function () {
				suppressOnBeforeUnload = true;
			});
		});
	</script>
</asp:Content>
