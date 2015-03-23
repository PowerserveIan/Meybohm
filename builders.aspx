<%@ Page Title="Builders" Language="C#" MasterPageFile="~/microsite.master" AutoEventWireup="true" CodeFile="builders.aspx.cs" Inherits="builders" %>

<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/alphabet.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="headerBG">
		<h1>Builder Directory</h1>
	</div>
	<div class="buDirContent">
		<asp:Repeater ID="uxAlphabet" runat="server">
			<HeaderTemplate>
				<ul class="AtoZ">
			</HeaderTemplate>
			<ItemTemplate>
				<li>
					<a href="#" <%# Container.DataItem.ToString() == CurrentLetter ? " class='active'" : !m_BuilderAlphabet.Contains(Container.DataItem.ToString()) ? " class='unavailable'" : ""  %>><%# Container.DataItem.ToString() %></a></li>
			</ItemTemplate>
			<FooterTemplate></ul></FooterTemplate>
		</asp:Repeater>
		<div class="form">
			<div class="formWhole">
				<label>Search by Name:</label>
				<asp:TextBox ID="uxSearch" runat="server" CssClass="text"></asp:TextBox>
				<a href="#" class="button" id="searchButton">Search</a>
			</div>
		</div>
		<asp:DataPager ID="uxTopPager" runat="server" PagedControlID="uxListView" QueryStringField="Page" class="pagination">
			<Fields>
				<asp352:NumericPagerFieldCustom ButtonCount="99999999" />
			</Fields>
		</asp:DataPager>
		<div class="clear"></div>
		<asp:ListView ID="uxListView" runat="server" ItemPlaceholderID="uxItemPlaceHolder" ItemType="Classes.MLS.Builder">
			<LayoutTemplate>
				<ul class="buList">
					<asp:PlaceHolder ID="uxItemPlaceHolder" runat="server"></asp:PlaceHolder>
				</ul>
			</LayoutTemplate>
			<ItemTemplate>
				<li>
					<img alt="" src='<%# ResolveUrl(BaseCode.Helpers.ResizedImageUrl(Item.Image, "uploads/builders/", 100, 75, true)) %>' />
					<div class="textWrapper">
						<h3><%# Item.Name %></h3>
						<p>
							<asp:PlaceHolder runat="server" Visible="<%# !String.IsNullOrEmpty(Item.OwnerName) %>">Owner: <span><%# Item.OwnerName %></span><br /></asp:PlaceHolder>
							<asp:ListView runat="server" ID="uxSubList" DataSource="<%# Item.NeighborhoodBuilder %>" ItemPlaceholderID="uxItemPlaceHolder" ItemType="Classes.MLS.NeighborhoodBuilder">
								<LayoutTemplate>
									Featured Neighborhood(s):
											<asp:PlaceHolder ID="uxItemPlaceHolder" runat="server"></asp:PlaceHolder> 
								</LayoutTemplate>
								<ItemTemplate>
									<a href='<%# "neighborhood-details?id=" + Item.NeighborhoodID %>' class="featuredNeighborhoodLink"><%# Item.Neighborhood.Name %></a> 
								</ItemTemplate>
								<EmptyDataTemplate>
								</EmptyDataTemplate>
							</asp:ListView>
							<br />
							<a href='<%# FixExternalLink(Item.Website) %>' target="_blank"><%# Item.Website %></a>
						</p>
					</div>
					<p class="buDesc"><%# BaseCode.Helpers.ReplaceRootWithAbsolutePath(Item.Info) %></p>
				</li>
			</ItemTemplate>
			<EmptyDataTemplate>
				No builders found
			</EmptyDataTemplate>
		</asp:ListView>
		<asp:DataPager ID="uxBottomPager" runat="server" PagedControlID="uxListView" QueryStringField="Page" class="pagination">
			<Fields>
				<asp352:NumericPagerFieldCustom ButtonCount="99999999" />
			</Fields>
		</asp:DataPager>
		<div class="clear"></div>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="PageSpecificJS" runat="Server">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery-ui.custom.min.js,~/tft-js/core/pagination.js"></asp:Literal>
	<script type="text/javascript">
		$(document).ready(function () {
			$("body").addClass("theme-new");
			$("ul.AtoZ li:first-child").addClass("aLi");
			$(".leftCol").addClass("buDirect");
			if ($(".pagination").length > 0)
				$(".pagination").pagination({
					pagesVisible: 17
				});
			$("#searchButton").click(function () {
				var searchText = $("#<%= uxSearch.ClientID %>").val();
				window.location = 'builders' + (searchText != "" ? "?searchText=" + searchText : "");
				return false;
			});
			$("#<%= uxSearch.ClientID %>").keydown(function (event) {
				if (event.keyCode == 13)
					$("#searchButton").trigger("click");
				return event.keyCode != 13;
			});
			$(".AtoZ a.active, .AtoZ a.unavailable").removeAttr("href");
			$(".AtoZ a").click(function () {
				if ($(this).hasClass("active") || $(this).hasClass("unavailable"))
					return false;
				window.location = 'builders?letter=' + $(this).html();
				return false;
			});
		});
	</script>
</asp:Content>
