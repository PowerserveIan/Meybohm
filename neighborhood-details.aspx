<%@ Page Title="Neighborhood Details" Language="C#" MasterPageFile="~/microsite.master" AutoEventWireup="true" CodeFile="neighborhood-details.aspx.cs" Inherits="neighborhoods_details" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentContentDiv">
	<div class="content inner map full">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="hoods">
		<div class="headerBG">
			<h1>
				<asp:Label runat="server" ID="uxName"></asp:Label></h1>
		</div>
		<div class="hoodDetailInfo">
			<div class="hoodDetailImageWrapper"><asp:Image runat="server" ID="uxImage" AlternateText="" /></div>
			<div class="hoodDetailInfoWrapper">
				<a href="#" class="getDirections">Get Directions</a>
				<h4>Location:</h4>
				<p>
					<span class="address">
						<span class="address"><asp:Label runat="server" ID="uxAddress"></asp:Label></span>
						<span class="cityState"><asp:Label runat="server" ID="uxCity"></asp:Label>,</span>
						<span class="cityState"><asp:Label runat="server" ID="uxState"></asp:Label>&nbsp;</span>
                        <span class="cityState"><asp:Label runat="server" ID="uxZip"></asp:Label></span>
					</span>
				</p>
				<asp:PlaceHolder runat="server" ID="uxPriceRangePH">
					<p>
						Price Range:<br />
						<span>
							<asp:Label runat="server" ID="uxPriceRange"></asp:Label></span>
					</p>
				</asp:PlaceHolder>
				<asp:HyperLink runat="server" ID="uxWebsite" Target="_blank"></asp:HyperLink>
				<div id="directionsContainer" style="display: none;">
					<asp:PlaceHolder runat="server" ID="uxDirectionsPH">
						<br />
						<h4>Directions:</h4>
						<p>
							<asp:Literal runat="server" ID="uxDirections"></asp:Literal>
						</p>
					</asp:PlaceHolder>
					<div class="poweredBy clearfix">
						<asp:HyperLink runat="server" ID="uxMapBirdsEye" Text="Bird's Eye View" Target="_blank" CssClass="floatRight btnShowcaseDetail bing"></asp:HyperLink>
						<span class="bingPower floatRight clearfix">Powered by Bing</span>
					</div>
					<div class="poweredBy clearfix">
						<asp:HyperLink runat="server" ID="uxMapDirections" Text="Get Directions" Target="_blank" CssClass="floatRight btnShowcaseDetail google"></asp:HyperLink>
						<span class="googlePower floatRight">Powered by Google</span>
						<div class="clear"></div>
					</div>
				</div>
			</div>
			<div class="clear"></div>
			<asp:PlaceHolder runat="server" ID="uxHomesAvailablePH">
				<h3>
					<a id="homesAvailable" href="#tab-4"><asp:Label runat="server" ID="uxNumberHomesAvailable"></asp:Label>&nbsp;Listing<asp:Literal runat="server" ID="uxHomesAvailableS">s</asp:Literal>
					Available!</a></h3>
			</asp:PlaceHolder>
		</div>
		<div id="hoodTabs">
			<ul class="hoodTabsMenu">
				<li>
					<a href="#tab-0">Overview</a></li>
				<li runat="server" id="uxAmenitiesLI">
					<a href="#tab-1">Amenities</a></li>
				<li runat="server" id="uxWhatsNearbyLI">
					<a href="#tab-2">What's Nearby</a></li>
				<li runat="server" id="uxBuildersLI">
					<a href="#tab-3">Builders</a></li>
				<li runat="server" id="uxHomesForSaleLI">
					<a href="#tab-4">Homes for Sale</a></li>
			</ul>
			<div id="hoodTabsPanels">
				<div id="tab-0" class="hoodTabsPanel">
					<h4>Overview</h4>
					<asp:Literal runat="server" ID="uxOverview"></asp:Literal>
				</div>
				<asp:PlaceHolder runat="server" ID="uxAmenitiesPH">
					<div id="tab-1" class="hoodTabsPanel">
						<h4>Amenities</h4>
						<asp:Literal runat="server" ID="uxAmenities"></asp:Literal>
					</div>
				</asp:PlaceHolder>
				<asp:Repeater runat="server" ID="uxNearbyCategories">
					<HeaderTemplate>
						<div id="tab-2" class="hoodTabsPanel">
							<div class="panelTextWrapper top">
								<h4>Nearby</h4>
					</HeaderTemplate>
					<ItemTemplate>
						<div class="nearbyTab">
							<h5 style="<%# Container.DataItem == null? "display:none;" : String.Empty %>"><%# Container.DataItem %></h5>
							<asp:Repeater runat="server" ID="uxNearbyLocations" ItemType="NearbyLocations" DataSource="<%# m_NearByLocations.Where(n=>Container.DataItem == null ? String.IsNullOrWhiteSpace(n.CategoryNames) : n.CategoryNames != null && n.CategoryNames.Split(',').Contains(Container.DataItem.ToString())) %>">
								<ItemTemplate>
									<div class="item">
										<asp:Image runat="server" AlternateText="<%# Item.Name %>" ImageUrl='<%# BaseCode.Helpers.ResizedImageUrl(Item.Image, BaseCode.Globals.Settings.UploadFolder + "nearByLocations/", 100, 75, true) %>'
											Visible="<%# !String.IsNullOrWhiteSpace(Item.Image) %>" />
										<h3><%# Item.Name %></h3>
										<p>
											<strong>Distance: </strong>
											<span><%# Math.Round(Item.DistanceAway.Value, 2) %></span>&nbsp;Miles
										</p>
										<p>
											<asp:PlaceHolder runat="server" Visible="<%# !String.IsNullOrEmpty(Item.Phone) %>">
												<strong>Phone: </strong>
												<span><%# Item.Phone %></span>
											</asp:PlaceHolder>
										</p>
										<p>
											<asp:HyperLink runat="server" Visible="<%# !String.IsNullOrEmpty(Item.Website) %>" NavigateUrl="<%# Item.Website %>" Text="<%# Item.Website %>" Target="_blank"></asp:HyperLink>
										</p>
									</div>
								</ItemTemplate>
							</asp:Repeater>

							<div class="clear"></div>
						</div>
						<div class="clear"></div>
					</ItemTemplate>
					<FooterTemplate>
						</div>
						</div>
					</FooterTemplate>
				</asp:Repeater>
				<asp:Repeater runat="server" ID="uxBuilders" ItemType="Classes.MLS.Builder">
					<HeaderTemplate>
						<div id="tab-3" class="hoodTabsPanel">
							<h4>Builders</h4>
							<div class="panelTextWrapper top">
								<ul class="buildersList">
					</HeaderTemplate>
					<ItemTemplate>
						<li class="floatLeft">
							<img alt="" src='<%# ResolveUrl(BaseCode.Helpers.ResizedImageUrl(Item.Image, "uploads/builders/", 200, 150, true)) %>' />
								<h3><%# Item.Name %></h3>
								<p>
									Owner: <span><%# Item.OwnerName %></span> 
									<a href='<%# Item.Website %>' target="_blank"><%# Item.Website %></a>
								</p>
								<p class="buDesc"><%# BaseCode.Helpers.ReplaceRootWithAbsolutePath(Item.Info) %></p>
						</li>
					</ItemTemplate>
					<FooterTemplate>
						</ul>
						<div class="clear"></div>
						</div>
							</div>
					</FooterTemplate>
				</asp:Repeater>
				<asp:Repeater runat="server" ID="uxHomes" ItemType="ShowcaseItemForJSON">
					<HeaderTemplate>
						<div id="tab-4" class="hoodTabsPanel">
							<h4>Homes for Sale</h4>
							<div class="panelTextWrapper top">
								<ul class="homesForSale">
					</HeaderTemplate>
					<ItemTemplate>
						<li class="floatLeft">
							<a class="fancybox.iframe showcaseProject" href="<%# Item.DetailsPageUrl %>">
								<span class="image">
									<asp:Image runat="server" AlternateText="<%# Item.Title %>" ImageUrl='<%# BaseCode.Helpers.ResizedImageUrl(Item.Image, "uploads/images/", 200, 150, true, false) %>' Visible="<%# !String.IsNullOrWhiteSpace(Item.Image) %>"
										Width="200" Height="150" /></span>
								<span class="title"><%# Item.Title %></span>
							</a>
						</li>
					</ItemTemplate>
					<FooterTemplate>
						</ul>
						<div class="clear"></div>
						</div>
							</div>
					</FooterTemplate>
				</asp:Repeater>
			</div>
		</div>
		<div class="allHoods">
			<a href="neighborhoods">All Neighborhoods</a>
		</div>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentSideCol"></asp:Content>
<asp:Content ContentPlaceHolderID="PageSpecificJS" runat="Server">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery-ui.custom.min.js"></asp:Literal>
	<script type="text/javascript">
		$(document).ready(function () {
			$("body").addClass("theme-new");

			$("#hoodTabs").tabs({
				fx: { opacity: 'toggle' }
			});
			$(".showcaseProject").fancybox(homeDetailsFancyboxParams);
			$(".getDirections").click(function () {
				$("#directionsContainer").slideToggle(200);
				$(this).remove();
				return false;
			});
			$("#homesAvailable").click(function () {
				$(".hoodTabsMenu a[href=#tab-4]").click();
				return false;
			});
		});
	</script>
</asp:Content>

