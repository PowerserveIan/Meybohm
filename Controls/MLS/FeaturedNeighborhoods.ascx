<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FeaturedNeighborhoods.ascx.cs" Inherits="Controls_MLS_FeaturedNeighborhoods" %>
<%@ Reference VirtualPath="~/microsite.master" %>
<div class="featuredNeighborhoods">
	<div class="featuredNeighborhoodsWrap">
		<span class="featuredTab">Featured Neighborhoods</span>
		<a href="<%= m_CurrentMicrositePath %>neighborhoods" class="searchAll">Search All <%= m_CurrentMicrosite.Name %> Neighborhoods</a>
		<div class="clear"></div>
		<div class="featuredNeighborhoodsContainer">
			<asp:Repeater runat="server" ID="uxNeighborhoods" ItemType="Classes.MLS.Neighborhood">
				<HeaderTemplate>
					<ul class="neighborhoodList">
				</HeaderTemplate>
				<ItemTemplate>
					<li>
						<a class="thumb" href='<%# m_CurrentMicrositePath + "neighborhood-details?id=" + Item.NeighborhoodID %>'>
							<asp:Image runat="server" Visible="<%# !String.IsNullOrEmpty(Item.Image) %>" AlternateText="<%# Item.Name %>" ImageUrl='<%# string.Format("~/{0}width=134&height=75{1}", (!String.IsNullOrWhiteSpace(Item.Image) ? (Item.Image.ToLower().StartsWith("http") ? "resizer.aspx?filename=" : BaseCode.Globals.Settings.UploadFolder + "neighborhoods/") + Item.Image + (Item.Image.ToLower().StartsWith("http") ? "&" : "?") : ""), (!String.IsNullOrWhiteSpace(Item.Image) && Item.Image.ToLower().StartsWith("http") ? "&trim=1" : "&mode=crop&anchor=middlecenter")) %>' />
							<span>
								<span class="location"><%# Item.Name %></span>
								<br />
								<span class="price"><%# Item.PriceRange %></span>
							</span>
						</a>
					</li>
				</ItemTemplate>
				<FooterTemplate></ul></FooterTemplate>
			</asp:Repeater>
			<div class="clear"></div>
		</div>
	</div>
</div>
<!--end featuredNeighborhoods-->
<div class="clear"></div>
<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/jquery.jcarousel.min.js"></asp:Literal>
<% if (m_UseCarousel)
   { %>
<script type="text/javascript">
	$(document).ready(function () {
		$(".neighborhoodList").jcarousel({
			buttonNextHTML: '<div class="nextWrap"><a class="next"></a></div>',
			buttonPrevHTML: '<div class="backWrap"><a class="back"></a></div>',
			size: $(".neighborhoodList li").length,
			visible: 3
		});
	});
</script>
<%} %>
