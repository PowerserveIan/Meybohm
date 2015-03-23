<%@ Page Title="Neighborhoods" Language="C#" MasterPageFile="~/microsite.master" AutoEventWireup="true" CodeFile="neighborhoods.aspx.cs" Inherits="neighborhoods" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentContentDiv">
	<div class="content inner map full">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="hoods">
		<div class="headerBG">
			<h1>Neighborhoods in <%= ((microsite)Master).CurrentMicrosite.Name %></h1>
		</div>
		<asp:PlaceHolder runat="server" ID="uxMapContainerPH">
			<div class="mapContainer"></div>
		</asp:PlaceHolder>
		<div class="form hoodSearch">
			<div class="formWhole">
				<label>Search by name or zip code:</label>
				<asp:TextBox ID="uxSearch" runat="server" CssClass="text"></asp:TextBox>
				<a href="#" class="button" id="searchButton">Search</a>
			</div>
		</div>
		<asp:Repeater ID="uxNeighborhoods" runat="server" ItemType="Classes.MLS.Neighborhood">
			<HeaderTemplate>
				<div class="hoodDirect neighborhoodsDirectory">
					<h3>Neighborhood Directory</h3>
					<ul>
			</HeaderTemplate>
			<ItemTemplate>
				<li class="temp-block">
					<a href='<%# "neighborhood-details?id=" + Item.NeighborhoodID %>'>
						<%# Item.Name %></a>
					<asp:PlaceHolder runat="server" Visible="<%# Item.NumberHomesAvailable > 0 %>">
						<a class="homesAvail" href='<%# "neighborhood-details?id=" + Item.NeighborhoodID + "#tab-4" %>'><span><%# Item.NumberHomesAvailable %></span> Listing<%# Item.NumberHomesAvailable == 1 ? "" : "s" %> Available!</a><%# !String.IsNullOrWhiteSpace(Item.PriceRange) ? " <em>From " + Item.PriceRange + "</em>" : "" %>
					</asp:PlaceHolder>
				</li>
			</ItemTemplate>
			<FooterTemplate>
				</ul>
			<div class="clear"></div>
				</div>
			</FooterTemplate>
		</asp:Repeater>
		<asp:PlaceHolder runat="server" ID="uxNoNeighborhoods" Visible="false">
			<div class="formWhole">There are no neighborhoods that match your search criteria.  Please try again.
			</div>
		</asp:PlaceHolder>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentSideCol"></asp:Content>
<asp:Content ContentPlaceHolderID="PageSpecificJS" runat="server">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/google-maps.js"></asp:Literal>
	<script type="text/javascript">
		var markerList = [<%= m_NeighborhoodJS %>];

		$(document).ready(function () {
			$("body").addClass("theme-new");
			$("#searchButton").click(function () {
				var searchText = $("#<%= uxSearch.ClientID %>").val();
				window.location = 'neighborhoods' + (searchText != "" ? "?searchText=" + searchText : "");
				return false;
			});
			$("#<%= uxSearch.ClientID %>").keydown(function (event) {
				if (event.keyCode == 13)
					$("#searchButton").trigger("click");
				return event.keyCode != 13;
			});
		});
	</script>
</asp:Content>