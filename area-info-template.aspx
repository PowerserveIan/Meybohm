<%@ Page Language="c#" MasterPageFile="~/microsite.master" Inherits="area_info_template" CodeFile="area-info-template.cs" %>

<%@ Reference Control="~/Controls/DynamicHeader/DynamicHeader.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<style>
		
		.accordionContainer h2 {
			cursor: pointer;
			display: block;
			line-height: 50px;
			height: 50px;
			margin: 0;
		}
		.accordionContainer h2:hover {
			text-decoration: underline;
		}
		.accordionContent {
			padding-bottom: 15px;
		}
	</style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentWindow">
	<cm:ContentRegion ID="uxMainRegion" runat="server" RegionName="MainRegion" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		$(document).ready(function () {
			$(".accordionContent").hide();
			$(".accordionContainer h2").click(function () {
				$(this).parent(".accordionContainer").find(".accordionContent").slideToggle(200);
			});
		});
	</script>
</asp:Content>
