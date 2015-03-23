<%@ Page Language="C#" AutoEventWireup="true" CodeFile="newsletter-details.aspx.cs" Inherits="NewsletterDetail" ViewStateMode="Disabled" %>

<!DOCTYPE html>
<!--[if lte IE 6 ]> <html lang="en" class="ie ie6"> <![endif]-->
<!--[if gt IE 6 ]> <html lang="en" class="ie"> <![endif]-->
<!--[if !IE]><!-->
<html lang="en">
<!--<![endif]-->
<head id="Head1" runat="server">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/newsShared.css,~/css/newsletter.css,~/css/forms.css,~/css/typography.css" id="uxCSSFiles" />
	<script type="text/javascript">
		var jqueryPath = "//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js";
		document.write(unescape("%3Cscript src='" + jqueryPath + "' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<script type="text/javascript">		!window.jQuery && document.write('<script type="text/javascript" src="tft-js/core/jquery.min.js"><\/script>')</script>
</head>
<body>
	<form id="form1" runat="server">
	<div id="detailContainer" class="detailContainer">
		<asp:Literal ID="uxHtmlPreview" runat="server" />
	</div>
	<br />
	<a id="dummylink" style="display: none;" onclick="parent.$.fancybox.close();"></a>
		<asp:HyperLink runat="server" ID="uxBackButton" Text="Back to Listings" CssClass="button"></asp:HyperLink>
	</form>
	<script type="text/javascript">
		// <![CDATA[
		$(document).ready(function () {
			$("a").not("#dummylink").click(function () {
				if (parent.$.fancybox != null) {
					$("#dummylink").trigger('click');
					if ($(this).attr("id") != '<%= uxBackButton.ClientID %>')
						parent.location = $(this).attr("href");
					return false;
				}
			});
		});
		// ]]>
	</script>
</body>
</html>
