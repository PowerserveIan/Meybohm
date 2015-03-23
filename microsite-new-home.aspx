<%@ Page Language="c#" MasterPageFile="~/microsite.master" Inherits="microsite_new_home" CodeFile="microsite-new-home.aspx.cs" %>

<%@ Reference Control="~/Controls/DynamicHeader/DynamicHeader.ascx" %>
<%@ Register TagPrefix="Showcase" TagName="Search" Src="~/Controls/Showcase/Search.ascx" %>
<%@ Register TagPrefix="Newsletter" TagName="Subscribe" Src="~/Controls/Newsletters/Subscribe.ascx" %>
<%@ Register TagPrefix="MLS" TagName="FeaturedNeighborhoods" Src="~/Controls/MLS/FeaturedNeighborhoods.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentBreadCrumbs"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentContentDiv">
	<div class="content">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentWindow">
	<MLS:FeaturedNeighborhoods runat="server" ID="uxFeaturedNeighborhoods" />
	<cm:ContentRegion ID="uxMainRegion" runat="server" RegionName="MainRegion" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentSideCol">
	<div class="rightCol">
		<Showcase:Search runat="server" ID="uxSearchWidget" NewHomes="true" />
		<Newsletter:Subscribe runat="server" ID="uxSubscribe" />
		<cm:ContentRegion ID="uxSideRegion" runat="server" RegionName="SideRegion" />
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		$(document).ready(function () {
			<%--TODO REMOVE THIS HACK BY FIXING CSS--%>
			$(".leftCol .box:first, .leftCol .top:first, .leftCol .bottom:first").removeClass("box").removeClass("top").removeClass("bottom");
		});

	</script>
</asp:Content>
