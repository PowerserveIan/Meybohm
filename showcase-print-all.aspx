<%@ Page Language="C#" AutoEventWireup="true" CodeFile="showcase-print-all.aspx.cs" Inherits="showcase_print_all" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
	<meta http-equiv="Content-language" content="en" />
	<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
	<link rel="dns-prefetch" href="//ajax.aspnetcdn.com" />
	<link media="screen, projection,print" type="text/css" runat="server" id="uxPrintCSS" href="~/css/print.css" rel="stylesheet" />
	<title>Meybohm REALTORs&reg; Home Listings</title>
	<script type="text/javascript">
		var jqueryPath = "//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js";
		document.write(unescape("%3Cscript src='" + jqueryPath + "' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<script type="text/javascript">
	  var _gaq = _gaq || [];
	  var pluginUrl = '//www.google-analytics.com/plugins/ga/inpage_linkid.js';
	  _gaq.push(['_require', 'inpage_linkid', pluginUrl]);
	  _gaq.push(['_setAccount', 'UA-18152881-1']);
	  _gaq.push(['_trackPageview']);

	  (function() {
		var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
		ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
		var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
	  })();
	</script>
</head>
<body>
	<form id="form1" runat="server">
	<div class="printAllLogo">
		<img src="~/img/print-logo.jpg" runat="server" alt="Meybohm" />
		<h1 class="printAllHeader">Meybohm REALTORs<sup>&reg;</sup> Home Listings</h1>
		<div class="clear"></div>
	</div>
	<div class="clear"></div>
	<asp:Repeater runat="server" ID="uxHomes" ItemType="ShowcaseItemForJSON">
		<HeaderTemplate>
			<ul class="printAll">
		</HeaderTemplate>
		<ItemTemplate>
			<li>
				<img alt="<%# Item.Title %>" src="<%# (Item.Image.ToLower().StartsWith("http") ? "" : ResolveClientUrl("~/uploads/images/")) + Item.Image %>" height="64" width="112" />
				<span><%# Item.Address %> - <%# Item.Title %></span><br />
				<a class="showcaseProject" href="<%# BaseCode.Helpers.RootPath + m_MarketPath + (Item.DetailsPageUrl.Contains("&title=") ? Item.DetailsPageUrl.Substring(0, Item.DetailsPageUrl.IndexOf("&title=")) : Item.DetailsPageUrl) %>"></a>
				<div class="clear"></div>
			</li>
		</ItemTemplate>
		<FooterTemplate>
			</ul>
		</FooterTemplate>
	</asp:Repeater>
	</form>
	<script type="text/javascript">
		$(document).ready(function () {
			window.print();
			window.close();
		});
	</script>
</body>
</html>
