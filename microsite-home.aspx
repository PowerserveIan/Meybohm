<%@ Page Language="c#" MasterPageFile="~/microsite.master" Inherits="microsite_home" CodeFile="microsite-home.aspx.cs" %>

<%@ Reference Control="~/Controls/DynamicHeader/DynamicHeader.ascx" %>
<%@ Register TagPrefix="Showcase" TagName="Search" Src="~/Controls/Showcase/Search.ascx" %>
<%@ Register Src="~/Controls/Newsletters/Subscribe.ascx" TagPrefix="Newsletter" TagName="Subscribe" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentBreadCrumbs"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentContentDiv">
	<div class="content">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentWindow">
	<cm:ContentRegion ID="uxMainRegion" runat="server" RegionName="MainRegion" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentSideCol">
	<div class="rightCol">
		<Showcase:Search runat="server" ID="uxSearchWidget" />
		<Newsletter:Subscribe runat="server" ID="uxSubscribe" />
		<cm:ContentRegion ID="uxSideRegion" runat="server" RegionName="SideRegion" />

        <div class="virtualMagazine aiken">
            <a target="_blank" title="Aiken Virtual magazine" href="http://www.scribd.com/doc/241101591" style="margin-right:10px;">
                <img src="/img/VirtualMagAikenSeptemberOctober.jpg" />
            </a>
            <a target="_blank" title="FINE Virtual magazine" href="http://issuu.com/meybohmrealtors/docs/virtual_magazine_fall_2014">
                <img src="/img/Fall2014.png" />
            </a>
        </div>

        <div class="virtualMagazine augusta" >
            <a target="_blank" title="Augusta Virtual magazine" href="http://issuu.com/meybohmrealtors/docs/mm_october_2014" style="margin-right:10px;">
                <img src="/img/VirtualMagAugustaSeptemberOctober.png" />
            </a>
            <a target="_blank" title="FINE Virtual magazine" href="http://issuu.com/meybohmrealtors/docs/virtual_magazine_fall_2014">
                <img src="/img/Fall2014.png" />
            </a>
        </div>
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
