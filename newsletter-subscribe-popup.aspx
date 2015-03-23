<%@ Page Language="C#" AutoEventWireup="true" CodeFile="newsletter-subscribe-popup.aspx.cs" Inherits="NewsletterSubscribePopup" %>

<%@ Register TagName="Subscribe" TagPrefix="Newsletter" Src="~/Controls/Newsletters/Subscribe.ascx" %>
<!DOCTYPE html>
<!--[if lte IE 6 ]> <html lang="en" class="ie ie6"> <![endif]-->
<!--[if gt IE 6 ]> <html lang="en" class="ie"> <![endif]-->
<!--[if !IE]><!-->
<html lang="en">
<!--<![endif]-->
<head runat="server">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/newsShared.css,~/css/newsletter.css,~/css/forms.css,~/css/typography.css" id="uxCSSFiles" />
	<script type="text/javascript">
		var jqueryPath = "//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js";
		document.write(unescape("%3Cscript src='" + jqueryPath + "' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<script type="text/javascript">		!window.jQuery && document.write('<script type="text/javascript" src="tft-js/core/jquery.min.js"><\/script>')</script>
</head>
<body class="newsletter">
	<form id="form1" runat="server">
	<div class="headerBG"><h1>Subscribe to Newsletter</h1></div>
	<Newsletter:Subscribe runat="server" ID="uxSubscribe" />
	</form>
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/Validation.js" />
</body>
</html>
