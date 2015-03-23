<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchResultSection.ascx.cs" Inherits="Controls_Showcase_SearchResultSection" %>


<h2><asp:Literal ID="uxSectionTitle" runat="server" /></h2>
<asp:ListView runat="server" ID="uxItemList" ItemPlaceholderID="itemPlaceHolder">
	<LayoutTemplate>
		<ul class="search-category clearfix">
			<asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
		</ul>
	</LayoutTemplate>
	<ItemTemplate>
		<li style="float:left">
			<a class="fancybox.iframe showcaseProject" rel="aikenHomeGallery" href="<%# ResolveClientUrl("~/" + micrositePath + "home-details?id=" + ((ShowcaseItemForJSON)Container.DataItem).ShowcaseItemID) %>">
				<img alt="<%#((ShowcaseItemForJSON)Container.DataItem).Title %>" src="<%#GetImageUrl(((ShowcaseItemForJSON)Container.DataItem).Image) %>" width="160" height="120"/>
			    <span class="showcaseAddress"><%# ((ShowcaseItemForJSON)Container.DataItem).Address %></span>
                <span><%#((ShowcaseItemForJSON)Container.DataItem).Title %></span>
			</a>
		</li>
	</ItemTemplate>
	<EmptyDataTemplate>No results found.</EmptyDataTemplate>
</asp:ListView>
<asp:Literal ID="uxCount" runat="server" /> <a runat="server" id="uxSeeMore"></a>
<div class="clear"></div>