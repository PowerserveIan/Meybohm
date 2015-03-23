<%@ Page Language="C#" AutoEventWireup="true" CodeFile="showcase-item.aspx.cs" Inherits="ShowcaseItemPage" MasterPageFile="~/microsite.master" Title="Home Details" %>

<%@ Import Namespace="BaseCode" %>
<%@ Import Namespace="Classes.Showcase" %>
<%@ Register TagPrefix="Contacts" TagName="ContactForm" Src="~/Controls/Contacts/ContactForm.ascx" %>
<%@ Register TagPrefix="Showcase" TagName="AddSavedSearch" Src="~/Controls/Showcase/AddSavedSearch.ascx" %>
<%@ Register TagPrefix="Showcase" TagName="Attributes" Src="~/Controls/Showcase/AttributeDisplay.ascx" %>
<%@ Register TagPrefix="Showcase" TagName="Media" Src="~/Controls/Showcase/MediaCollection.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/showcase.css,~/css/slideLock.css" id="uxCSSFiles" />
	<% if (!uxBackToShowcase.Visible)
	{ %>
	<style type="text/css">
		body {
			background: #fff;
		}
	</style>
	<% } %>
	<!--[if lte IE 6]>
	    <style type="text/css">
		    div#tabs div.leftSide {
                width: 170px;
            }
            ul.ui-tabs-nav li a {
	            float: left;
	            width: 165px;
            }
            table.attributes.featured {
                width: 165px
            }
	    </style>
	<![endif]-->
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentContentDiv">
	<div class="content inner map">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentWindow">
	<asp:PlaceHolder runat="server" ID="uxEntireBodyPH">
		<div class="showcase">
			<div class="projectPage">
				<div class="leftSide">
					<div class="printLogo">
						<img src="~/img/print-logo.jpg" runat="server" alt="Meybohm" /></div>
					<asp:PlaceHolder runat="server" ID="uxSaveSearchPH">
						<% if (Page.User.Identity.IsAuthenticated)
		 { %>
						<a href="#addSearchDiv" class="button editSavedSearch">Save this Listing</a>
						<% }
		 else
		 { %>
						<a href="#" class="button editSavedSearch" onclick="parent.$.fancybox.close();parent.$('.loginExpander').trigger('click');parent.window.scrollTo(0, parent.$('.loginExpander').offset().top);return false;">Login to save</a>
						<% } %>
					</asp:PlaceHolder>
					<div class="topArea" runat="server" id="uxTopArea">
						
					</div>
					<div class="propInfoSum">
					
							<asp:Image runat="server" ID="uxMainImageTopRight" style="display:none" Width="156" Height="97" ResizerWidth="156" ResizerHeight="97"/>
						
						
						<p class="propAddress">
							<asp:Literal runat="server" ID="uxAddress"></asp:Literal>
						</p>
						<asp:Label runat="server" ID="uxPrice" CssClass="price"></asp:Label>
						<asp:Label runat="server" ID="uxAvailableDate"></asp:Label>
						<asp:PlaceHolder runat="server" ID="uxMLSNumberPH">MLS #
							<asp:Label runat="server" ID="uxMLSNumber"></asp:Label>
						</asp:PlaceHolder>
						<asp:HyperLink runat="server" ID="uxWebsite" Target="_blank" Text="Visit Website" CssClass="visitWebsite"></asp:HyperLink>
						<asp:HyperLink runat="server" ID="uxVirtualTour" Target="_blank" Text="Virtual Tour" CssClass="virtualTour btnShowcaseDetail"></asp:HyperLink>
						<div class="clear"></div>
					</div>
					<asp:PlaceHolder runat="server" ID="uxMainAgentPH">
						<hr />
						<div class="realtorInfo">
						    <asp:Image runat="server" ID="uxMainImage3"/>
							<asp:Label runat="server" ID="uxMainAgentName" CssClass="realtorName"></asp:Label>
							<asp:Label runat="server" ID="uxMainAgentPhone" CssClass="realtorNumber"></asp:Label>
							<asp:Repeater runat="server" ID="uxMainTeamPhones" Visible="false">
								<ItemTemplate>
									<span class="realtorNumber">
										<%# ((KeyValuePair<string, string>)Container.DataItem).Key %>: 
										 <span class="value"><%# ((KeyValuePair<string, string>)Container.DataItem).Value %></span>
									</span>
								</ItemTemplate>
							</asp:Repeater>
						</div>
						<hr />
					</asp:PlaceHolder>
					<a href="#" class="printSummary btnShowcaseDetail" title="Print a summary" onclick="TrackClick(<%= (int)ClickTypes.Print %>);window.print();return false;">Print a Summary</a>
					<div class="slideWrapper linkWrapper" runat="server" id="uxLinkWrapper">
						<div class="revealSlide">
							<a class="linkToPage expand up btnShowcaseDetail" title="Link to this page" style="cursor: pointer;" onclick="TrackClick(<%= (int)ClickTypes.Share %>);">Share this page</a></div>
						<div class="linkContainer" style="display: none;">
							<div class="slide">
								<div id="LinkPopup">
									<Facebook:LikeButton runat="server" ID="uxFacebookLikeButton" width="310" />
									<asp:Label runat="server" ID="uxLinkToPage" />
								</div>
							</div>
						</div>
					</div>
					<!--end slideWrapper-->
				</div>
				<!--end leftSide-->
				<div class="rightSide">
					<div id="tabs" class="ui-tabs">
						<div class="topSection">
							<ul class="topTabs">
								<li runat="server" id="uxOverviewLI">
									<a class="prg_noimage" href="#tabs-1">Overview</a></li>
								<asp:Repeater runat="server" ID="uxCollectionsRepeater" ItemType="Classes.Showcase.MediaCollection">
									<ItemTemplate>
										<li>
											<a  class="prg_noimage" href='<%#"#tabs-" + (numberOfStaticTabs + Container.ItemIndex + 1)%>' onclick="TrackClick(<%= (int)ClickTypes.Photos %>);">
												<%#:Item.Title%></a></li>
									</ItemTemplate>
								</asp:Repeater>
								<li runat="server" id="uxMapLI">
									<a  class="prg_image" href="#tabs-2" onclick="TrackClick(<%= (int)ClickTypes.Map %>);setTimeout('handleApiReady();', 1000);">Map</a></li>
								<li  runat="server" id="uxNeighborhoodLI">
									<a class="prg_image neighborhoodTab" href="#tabs-3" onclick="TrackClick(<%= (int)ClickTypes.Neighborhood %>);">Neighborhood</a></li>
								<li runat="server" id="uxBuilderLI">
									<a class="prg_image" href="#tabs-4" onclick="TrackClick(<%= (int)ClickTypes.Builder %>);">Builder</a></li>
								<li runat="server" id="uxSchoolsLI">
									<a class="prg_image" href="#tabs-5" onclick="TrackClick(<%= (int)ClickTypes.Schools %>);">Schools</a></li>
								<li runat="server" id="uxWhatsNearByLI">
									<a class="prg_image" href="#tabs-6" onclick="TrackClick(<%= (int)ClickTypes.WhatsNearby %>);">What's Nearby</a></li>
								<li runat="server" id="uxOpenHouseLI">
									<a class="prg_image" href="#tabs-7" onclick="TrackClick(<%= (int)ClickTypes.OpenHouse %>);">Open House</a></li>
								
								<li>
									<a class="prg_image contactAgentTab" href="#tabs-<%=(numberOfStaticTabs + numberOfCollections + 1)%>" onclick="TrackClick(<%= (int)ClickTypes.Contact %>);">Contact Agent</a></li>
							</ul>
							<div class="clear"></div>
						</div>
						<div class="bottomSection">
							<div id="tabs-1">
								<div class="full">
									<% if (numberOfCollections > 0)
									{ %><a onclick="$('a[href=#tabs-<%= numberOfStaticTabs + 1 %>]').click();return false;" href="#"><% } %>
									<asp:Image runat="server" ID="uxImage" Width="430" Height="300" ResizerWidth="430" ResizerHeight="300" />
									<span id="prgmImageCount"><i class="icon-th"></i> View More Photos (0)</span>
									<% if (numberOfCollections > 0)
									{ %></a><% } %>
									<h2>Description:</h2>
									<hr />
									<p>
										<asp:Literal runat="server" ID="uxItemSummary"></asp:Literal>
									</p>
								</div>
								<div class="half">
									<h2>Information:</h2>
									<hr />
									<Showcase:Attributes runat="server" ID="uxTabsAttributes" />
								</div>

                                <div class="half paymentCalculator">
                                    <div class="toggle">
                                        <div>
                                            Estimated Mortgage Payment: $<span id="monthlyPayment2"></span>/mo
                                        </div>
                                        <a id="changeTerms" href="#">Change Terms</a>
                                    </div>
                                    <div class="wrapper">
                                        <div>
                                            <label for="purchasePrice">Purchase Price:</label>
                                            $<input id="purchasePrice" />
                                        </div>
                                        <div>
                                            <label for="downPayment">Down Payment:</label>
                                            $<input id="downPayment" />
                                        </div>
                                        <div>
                                            <label for="interestRate">Interest Rate:</label>
                                            <input id="interestRate" value="4.5" />%
                                        </div>
                                        <div>
                                            <label for="yearsCount">Number of Years:</label>
                                            <input id="yearsCount" value="30" />
                                        </div>
                                        <div>
                                            <label>Estimated Mortgage Payment:</label>
                                            $<span id="monthlyPayment"></span>
                                        </div>
                                        <div>
                                            <button id="calculate" class="button">Calculate</button>
                                        </div>
                                    </div>
                                    <div class="notice" style="display: none;">
                                        * Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempus arcu nunc, vitae porttitor purus dictum in. Quisque sed metus cursus tortor blandit eleifend et non justo. Pellentesque in enim et neque condimentum consectetur in scelerisque metus. Proin mauris dui, placerat nec pulvinar eu, condimentum sit amet odio. Integer nulla.
                                    </div>
                                </div>

                                <script src="/Content/Javascript/jquery.number.min.js"></script>

                                <script>
                                    $(function()
                                    {
                                        var type = $('table.attributes.full tbody tr td:eq(0) span').html().trim();
                                        
                                        if (type == 'Rental Price:' || type == 'Property Type:')
                                        {
                                            $('.paymentCalculator').hide();
                                            $('.contactAgentTab').text('Contact Us');
                                            $('.contactAgentTabText').text('Use the form at the right to contact customer service about this listing for more information.');
                                            $('.contactAgentHeader').hide();
                                            $('.neighborhoodTab').hide();
                                        }

                                        $('.paymentCalculator .wrapper').hide();
                                        $('.paymentCalculator .toggle #changeTerms').click(function ()
                                        {
                                            $('.paymentCalculator .toggle').hide();
                                            $('.paymentCalculator .wrapper').slideDown();
                                        });

                                        $('#purchasePrice').number(true, 2);
                                        $('#downPayment').number(true, 2);

                                        var price = $('table.attributes.full tbody tr td:eq(1)').html().replace('$', '').trim();
                                        var purchasePrice = parseFloat($.number(price, 2, '.', ''));
                                        var downPayment = 0.2 * purchasePrice;

                                        $('#purchasePrice').val(purchasePrice);
                                        $('#downPayment').val(downPayment);

                                        calculateMonthlyPayment();

                                        $('#calculate').click(function ()
                                        {
                                            calculateMonthlyPayment();

                                            return false;
                                        });
                                    });

                                    function calculateMonthlyPayment()
                                    {
                                        var purchasePrice = parseFloat($.number($('#purchasePrice').val(), 2, '.', ''));
                                        var downPayment = parseFloat($.number($('#downPayment').val(), 2, '.', ''));
                                        var interestRate = parseFloat($.number($('#interestRate').val(), 2, '.', ''));
                                        var yearsCount = parseFloat($.number($('#yearsCount').val(), 2, '.', ''));
                                            
                                        var monthlyInterest = (interestRate / 100.0) / 12.0;
                                        var monthsCount = yearsCount * 12;

                                        var monthlyPayment = (purchasePrice - downPayment) * (monthlyInterest / (1 - Math.pow((1 + monthlyInterest), -monthsCount)));
                                        monthlyPayment = Math.round(monthlyPayment);

                                        $('#monthlyPayment').html($.number(monthlyPayment, 0));
                                        $('#monthlyPayment2').html($.number(monthlyPayment, 0));
                                    }
                                </script>
								
								<div class="clear"></div>
								<asp:PlaceHolder runat="server" ID="uxBrokerReciprocityPH">
									<div class="brokerRecip">
										<img class="floatLeft" src="<%= ResolveClientUrl("~/") %>img/idx_logo.gif"><p>
											Listing information courtesy of
										<asp:Literal runat="server" ID="uxBrokerOffice"></asp:Literal>. The data relating to real estate for sale on this web site comes in part from the Broker Reciprocity Program of <%= IsAiken ? "the Aiken Board of REALTORs<sup>&reg;</sup>" : "G.A.A.R. - MLS" %>.
											Real estate listings
										held by brokerage firms other than Meybohm REALTORs<sup>&reg;</sup> are marked with the Broker Reciprocity logo and detailed information about them includes the name of the listing brokers.
										</p>
									</div>
								</asp:PlaceHolder>
							</div>
							<asp:PlaceHolder runat="server" ID="uxMapPH">
								<div id="tabs-2">
									<div class="mapContainer"></div>
									<div class="floatLeft">
										<p class="propAddress">
											<asp:Literal runat="server" ID="uxAddressInMapTab"></asp:Literal>
										</p>
										<p class="propDirections">
											<asp:Literal runat="server" ID="uxDirections"></asp:Literal>
										</p>
									</div>
									<div class="poweredBy clearfix">
										<asp:HyperLink runat="server" ID="uxMapBirdsEye" Text="Bird's Eye View" Target="_blank" CssClass="floatRight btnShowcaseDetail bing"></asp:HyperLink>
										<span class="bingPower floatRight clearfix">Powered by Bing</span>
									</div>
									<div class="poweredBy clearfix">
										<asp:HyperLink runat="server" ID="uxMapDirections" Text="Get Directions" Target="_blank" CssClass="floatRight btnShowcaseDetail google"></asp:HyperLink>
										<span class="googlePower floatRight">Powered by Google</span>
										<div class="clear"></div>
									</div>
									<div class="clear"></div>
								</div>
							</asp:PlaceHolder>
							<asp:PlaceHolder runat="server" ID="uxNeighborhoodPH">
								<div id="tabs-3">
									<div class="hoodDetailInfo">
										<div class="hoodDetailInfoWrapper">
											<span data-bind="html: viewModel.Neighborhood().Name" class="neighborhoodName"></span>
											<hr />
											<img data-bind='attr:{alt:viewModel.Neighborhood().Name,src:viewModel.NeighborhoodImageSrc()},visible:viewModel.Neighborhood().Image' alt="" src="../../img/loading.gif" height="150" width="200" class="floatLeft neighborhoodImage" />
											<div class="floatLeft">

												<p>
													<strong>Location:</strong>
													<span data-bind="html: viewModel.NeighborhoodFormattedAddress()"></span>
												</p>
												<p data-bind="visible:viewModel.Neighborhood().Phone">
													<strong>Information:</strong>
													<span data-bind="html:viewModel.Neighborhood().Phone"></span>
												</p>
												<a href="#" target="_blank" data-bind="html:viewModel.Neighborhood().Website,attr:{href:viewModel.Neighborhood().Website}"></a>
												<a href="#" class="parentWindow" data-bind="attr:{href:'neighborhood-details?id=' + viewModel.Neighborhood().NeighborhoodID},visible: viewModel.Neighborhood().Active">See All Properties &raquo;</a>
											</div>
											<div class="clear"></div>

											<div class="highlights" data-bind="visible:viewModel.Neighborhood().Amenities">
												<h3>Highlights</h3>
												<hr />
												<div data-bind="html:viewModel.Neighborhood().Amenities"></div>
											</div>
										</div>
										<div class="clear"></div>
									</div>
								</div>
							</asp:PlaceHolder>
							<asp:PlaceHolder runat="server" ID="uxBuilderPH">
								<div id="tabs-4">
									<div class="hoodDetailInfo">
										<span class="builderName" data-bind="html:viewModel.Builder().Name"></span>
										<hr />
										<div class="left">
											<img data-bind='attr:{alt:viewModel.Builder().Name,src:viewModel.BuilderImageSrc()},visible:viewModel.Builder().Image' alt="" src="../../img/loading.gif" height="150" width="200" />
											<a href="#" target="_blank" data-bind="html:viewModel.Builder().Website,attr:{href:viewModel.Builder().Website}" class="builderWebsite"></a>
										</div>
										<div class="hoodDetailInfoWrapper">
											<p data-bind="visible: viewModel.Builder().OwnerName && viewModel.Builder().OwnerName != ''"><strong>Owner:</strong> <span data-bind="	html: viewModel.Builder().OwnerName"></span></p>
											<p data-bind="html:viewModel.Builder().Info"></p>
										</div>
										<div class="clear"></div>
									</div>
								</div>
							</asp:PlaceHolder>
							<asp:PlaceHolder runat="server" ID="uxSchoolsPH">
								<div id="tabs-5">
									<ul class="schoolTab">
										<li runat="server" id="uxElementarySchoolLI">
											<asp:Image runat="server" ID="uxElementaryImage" ResizerWidth="134" ResizerHeight="102" CssClass="floatLeft" />
											<div class="floatLeft schoolInfo">
												<h3>Elementary School: 
											<asp:Label runat="server" ID="uxElementaryName"></asp:Label></h3>
												<p>
													<strong>Distance: </strong>
													<asp:Label runat="server" ID="uxElementaryDistance"></asp:Label>&nbsp;Miles
												</p>
											</div>
											<div class="clear"></div>
										</li>
										<li runat="server" id="uxMiddleSchoolLI">
											<asp:Image runat="server" ID="uxMiddleImage" ResizerWidth="134" ResizerHeight="102" CssClass="floatLeft" />
											<div class="floatLeft schoolInfo">
												<h3>Middle School: 
											<asp:Label runat="server" ID="uxMiddleName"></asp:Label></h3>
												<p>
													<strong>Distance: </strong>
													<asp:Label runat="server" ID="uxMiddleDistance"></asp:Label>&nbsp;Miles
												</p>
											</div>
											<div class="clear"></div>
										</li>
										<li runat="server" id="uxHighSchoolLI">
											<asp:Image runat="server" ID="uxHighImage" ResizerWidth="134" ResizerHeight="102" CssClass="floatLeft" />
											<div class="floatLeft schoolInfo">
												<h3>High School: 
											<asp:Label runat="server" ID="uxHighName"></asp:Label></h3>
												<p>
													<strong>Distance: </strong>
													<asp:Label runat="server" ID="uxHighDistance"></asp:Label>&nbsp;Miles
												</p>
											</div>
											<div class="clear"></div>
										</li>
									</ul>
								</div>
							</asp:PlaceHolder>
							<asp:Repeater runat="server" ID="uxWhatsNearByRepeater" ItemType="NearbyLocations">
								<HeaderTemplate>
									<div id="tabs-6">
										<ul class="nearbyTab">
								</HeaderTemplate>
								<ItemTemplate>
									<li>
										<asp:Image runat="server" AlternateText="<%# Item.Name %>" ImageUrl='<%# Helpers.ResizedImageUrl(Item.Image, Globals.Settings.UploadFolder + "nearByLocations/", 100, 75, true) %>'
											Visible="<%# !String.IsNullOrWhiteSpace(Item.Image) %>" />
										<h3>
											<asp:HyperLink runat="server" Visible="<%# !String.IsNullOrEmpty(Item.Website) %>" NavigateUrl="<%# Item.Website %>" Text="<%# Item.Name %>" Target="_blank"></asp:HyperLink>
											<asp:Literal runat="server" Visible="<%# String.IsNullOrEmpty(Item.Website) %>" Text="<%# Item.Name %>"></asp:Literal></h3>
										<p>
											<strong>Distance: </strong>
											<span><%# Item.DistanceAway.HasValue ? Math.Round(Item.DistanceAway.Value, 2).ToString() : "" %></span>&nbsp;Miles
										</p>
										<p>
											<asp:PlaceHolder runat="server" Visible="<%# !String.IsNullOrEmpty(Item.Phone) %>">
												<strong>Phone: </strong>
												<span><%# Item.Phone %></span>
											</asp:PlaceHolder>
										</p>
									</li>
								</ItemTemplate>
								<FooterTemplate>
									<div class="clear"></div>
									</ul></div>
								</FooterTemplate>
							</asp:Repeater>
							<asp:PlaceHolder runat="server" ID="uxOpenHousePH">
								<div id="tabs-7">
									<div class="contactWrapper">
										<asp:PlaceHolder runat="server" ID="uxOpenHouseAgentPH">
											<div class="floatLeft realtor">
												<span><em>Your Meybohm REALTOR<sup>&reg;</sup>
													<br />
													for this open house is:</em></span>
												<asp:Image runat="server" ID="uxOpenHouseAgentImage" ResizerWidth="134" ResizerHeight="168" />
												<asp:Label runat="server" ID="uxOpenHouseAgentName" CssClass="realtorName"></asp:Label>
												<asp:Label runat="server" ID="uxOpenHouseAgentPhone" CssClass="realtorNumber"></asp:Label>
											</div>
										</asp:PlaceHolder>
										<div class="floatLeft panelRight">
											<h3>Open House Times</h3>
											<ul class="openHouse">
												<asp:Repeater runat="server" ID="uxOpenHouseTimes" ItemType="Classes.Showcase.OpenHouse">
													<ItemTemplate>
														<li><%# Item.BeginDateClientTime.ToString("dddd M/d/yyyy h:mm tt")%>
															<%# Item.EndDate.HasValue ? " - " + Item.EndDateClientTime.Value.ToString("h:mm tt") : "" %></li>
													</ItemTemplate>
												</asp:Repeater>
											</ul>
										</div>
										<div class="clear"></div>
									</div>
								</div>
							</asp:PlaceHolder>
							<asp:Repeater runat="server" ID="uxTabsRepeater" ItemType="Classes.Showcase.MediaCollection">
								<ItemTemplate>
									<div id='<%#"tabs-" + (numberOfStaticTabs + Container.ItemIndex + 1)%>'>
										<Showcase:Media MediaType="<%#EnumParser.Parse<MediaTypes>(Item.ShowcaseMediaTypeID.ToString())%>" runat="server" MediaCollectionID="<%#Item.ShowcaseMediaCollectionID%>" Visible='<%#EnumParser.Parse<MediaTypes>(Item.ShowcaseMediaTypeID.ToString())
			!= MediaTypes.TextBlock%>' />
									</div>
								</ItemTemplate>
							</asp:Repeater>
							<div id="tabs-<%=(numberOfStaticTabs + numberOfCollections + 1)%>">
								<div class="contactWrapper">
									<asp:PlaceHolder runat="server" ID="uxAgentPH">
										<div class="floatLeft realtor">
											<span class="contactAgentHeader"><em>Your Meybohm REALTOR<sup>&reg;</sup>
												<br />
												for this listing is:</em></span>
											<asp:Image runat="server" ID="uxAgentImage" ResizerWidth="134" ResizerHeight="168" />
											<asp:Label runat="server" ID="uxAgentName" CssClass="realtorName"></asp:Label>
											<asp:Label runat="server" ID="uxAgentPhone" CssClass="realtorNumber"></asp:Label>
											<asp:Repeater runat="server" ID="uxTeamPhones" Visible="false">
												<ItemTemplate>
													<span class="realtorNumber"><%# ((KeyValuePair<string, string>)Container.DataItem).Key %>: <%# ((KeyValuePair<string, string>)Container.DataItem).Value %></span>
												</ItemTemplate>
											</asp:Repeater>
											<hr />
											<em class="questions">Questions about this listing?</em><br />
											<p class="contactAgentTabText">Use the form at the right to contact a Meybohm REALTOR about this listing for more information.</p>
										</div>
									</asp:PlaceHolder>
									<div class="floatLeft panelRight">
										<Contacts:ContactForm runat="server" ID="uxContactForm" ContactFormType="PropertyInformation" EnableClientSideSubmission="true" HideIntroText="true" />
									</div>
									<div class="clear"></div>
								</div>
							</div>
						</div>
					</div>
					<!--end tabs-->
				</div>
				<!--end rightSide-->
				<div class="clear"></div>
			</div>
			<!--end projectPage-->
		</div>
		<!--end showcase-->
		<br />
		<asp:HyperLink runat="server" ID="uxBackToShowcase" Text="Back to Listings" CssClass="button"></asp:HyperLink>
		<Showcase:AddSavedSearch runat="server" ID="uxAddSavedSearch" />
	</asp:PlaceHolder>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentSideCol">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery-ui.custom.min.js,~/tft-js/core/easing.js,~/tft-js/core/jquery.mousewheel.min.js,~/tft-js/core/knockout.js,~/tft-js/google-maps.js,~/tft-js/core/showcase-item.js"></asp:Literal>
	<asp:PlaceHolder runat="server" ID="uxMapsJSPH">
		<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
		<script type="text/javascript">
			// <![CDATA[
			var markerList = [<%= m_MapLocation %>];
			var blockInitialLoad = true;
			var maxZoomLevel = 17;
			// ]]>
		</script>
	</asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="uxNoMapsJSPH">
        <script type="text/javascript">
        var blockInitialLoad = true;
        </script>
    </asp:PlaceHolder>

	<script type="text/javascript">
		var showcaseItemID = <%= ShowcaseItemId %>;		
		var clickTypesTracked = [<%= string.Join(",", ((Dictionary<int, List<int>>)Session["HomeVisits"])[ShowcaseItemId]) %>];
		var isOwnProperty = <%= m_IsOwnProperty.ToString().ToLower() %>;
		// <![CDATA[		
		$(document).ready(function () {
			$(".prg_noimage").click(function() {
				removeLeftPropertyImage();
			});
			$(".prg_image").click(function() {
				addLeftPropertyImage();
			});

			function removeLeftPropertyImage() {
				$('#<%=uxImage.ClientID%>').show();
				$('#<%=uxMainImageTopRight.ClientID%>').hide();
				$('#<%=uxMainImage3.ClientID%>').show();
			}

			function addLeftPropertyImage() {
				$('#<%=uxImage.ClientID%>').hide();
				$('#<%=uxMainImageTopRight.ClientID%>').show();
				$('#<%=uxMainImage3.ClientID%>').hide();
			}

			if (parent.$.fancybox)
				parent.$("#fancybox-loading").hide();
			$("body").addClass("theme-new");
			$("#tabs").tabs({
				fx: { opacity: 'toggle' }<% if (Classes.Showcase.Settings.AutoplayVideos && Request.Browser.Browser == "IE")
								{ %>,
				select: function (event, ui) {
					$(ui.panel).find("embed").each(function () {
						$(this).attr("src", $(this).attr("src") + "&autoplay=1");
						var clone = $(this).clone();
						$(this).parent().append(clone);
						$(this).remove();
					});
				}<% } %>
			});

		    var imageCount = $('div.thumbnails a.item').length;
		    if(imageCount < 1)
		        $('#prgmImageCount').hide();
            else
		        $('#prgmImageCount').html('<i class="icon-th"></i> View More Photos (' + $('div.thumbnails a.item').length + ')');
		});

		var viewModel = {
			Builder: ko.observable(null),
			Neighborhood: ko.observable(null)
		}
		var builderLoaded = <%= uxBuilderPH.Visible ? "false" : "true" %>;
		var neigborhoodLoaded = <%= uxNeighborhoodPH.Visible ? "false" : "true" %>;		
		
		<% if (uxBuilderPH.Visible)
	 { %>
		$(document).ready(function () {
			


			$.ajax({
				type: "POST",
				url: '<%= ResolveClientUrl("~/showcase-item.aspx") %>/GetBuilder',
				data: '{"showcaseItemID":"<%= ShowcaseItemId %>"}',
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (data) {
					viewModel.Builder(data.d);
					viewModel.BuilderImageSrc = function () {
						if (viewModel.Builder().Image)
							return (viewModel.Builder().Image.indexOf("http") >= 0 ? viewModel.Builder().Image : "<%= ResolveClientUrl("~/" + Globals.Settings.UploadFolder) %>builders/" + viewModel.Builder().Image + "?width=200&height=150&mode=crop=anchor=middlecenter");
					};
					builderLoaded = true;
					if (neigborhoodLoaded)
						ko.applyBindings(viewModel);
				},
				error: function (jqXHR, textStatus, errorThrown) {
					$("#tabs-4").html("There was a problem loading the builder info for this home.  Please try again later.  Error: " + jqXHR.responseText);
				}
			});
		});
		<% } %>
		<% if (uxNeighborhoodPH.Visible)
	 { %>

		$(document).ready(function () {
			$.ajax({
				type: "POST",
				url: '<%= ResolveClientUrl("~/showcase-item.aspx") %>/GetNeighborhood',
				data: '{"showcaseItemID":"<%= ShowcaseItemId %>"}',
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (data) {
					viewModel.Neighborhood(data.d);
					viewModel.NeighborhoodImageSrc = ko.computed(function () {
						if (viewModel.Neighborhood().Image)
							return (viewModel.Neighborhood().Image.indexOf("http") >= 0 ? viewModel.Neighborhood().Image : "<%= ResolveClientUrl("~/" + Globals.Settings.UploadFolder) %>neighborhoods/" + viewModel.Neighborhood().Image + "?width=200&height=150&mode=crop=anchor=middlecenter");
					});
					viewModel.NeighborhoodFormattedAddress = ko.computed(function () {
						return (viewModel.Neighborhood().Address.Address1 + "<br />" + viewModel.Neighborhood().Address.City + ", " + viewModel.Neighborhood().Address.State.Abb + " " + viewModel.Neighborhood().Address.Zip).replace(/null/g, '');
					});
					neigborhoodLoaded = true;
					if (builderLoaded)
						ko.applyBindings(viewModel);
				},
				error: function (jqXHR, textStatus, errorThrown) {
					$("#tabs-3").html("There was a problem loading the neighborhood info for this home.  Please try again later.  Error: " + jqXHR.responseText);
				}
			});

		});
		<% } %>
		// ]]>
	</script>
</asp:Content>
