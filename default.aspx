<%@ Page Language="c#" MasterPageFile="~/frontend.master" Inherits="_Default" CodeFile="default.aspx.cs" %>

<asp:Content runat="server" ID="breadCrumb" ContentPlaceHolderID="ContentBreadCrumbs" />
<asp:Content runat="server" ContentPlaceHolderID="ContentWindow">
	<h3 class="mapHeading">Please select your area of interest from the map below</h3>
	<div id="lmc" class="landingMapContainer">
		<a class="left" href="augusta/">Augusta</a>
		<a class="right" href="aiken/">Aiken</a>
		<div id="augustaMap" class="landingMap"></div>
		<div id="aikenMap" class="landingMap"></div>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/jquery.design-home.js"></asp:Literal>
	<script type="text/javascript">
		$(".content .wrapper .roundedBox").removeClass("roundedBox");
	</script>
</asp:Content>
