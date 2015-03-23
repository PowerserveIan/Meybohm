<%@ Page Language="C#" MasterPageFile="~/microsite.master" AutoEventWireup="true" CodeFile="showcase.aspx.cs" Inherits="Showcase" Title="Showcase" ViewStateMode="Disabled" %>

<%@ Import Namespace="BaseCode" %>
<%@ Import Namespace="Classes.Showcase" %>
<%@ Register TagPrefix="Showcase" TagName="AddSavedSearch" Src="~/Controls/Showcase/AddSavedSearch.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/showcase.css" id="uxCSSFiles" />
	<style type="text/css">
		.radSliderNoBar .rslSelectedregion {
			display: none !important;
		}

		.grid_10.leftCol {
			width: 940px;
		}

		div.leftCol ul.langToggle {
			float: none;
			position: absolute;
			top: -50px;
			right: 10px;
		}
	</style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentContentDiv">
	<div class="content inner map showcaseBox">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentSideCol">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class='showcase<%=Settings.FilterDisplayStyle == FiltersDisplays.Left ? "" : ""%>'>
		<div class="showcaseWrapper">
			<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder">
				<asp:HyperLink runat="server" ID="uxToggleFilters" CssClass="filterShow" NavigateUrl="#" title="Click to toggle filter options">Filter Results</asp:HyperLink>
				<% if (Settings.HideFiltersInSlideout)
	   { %>
				<div class="filterSlideHide">
					<%} %>
					<div class='filterWrapper side <%=Settings.FilterDisplayStyle == FiltersDisplays.Left ? "" : ""%>'>
						<div class="FilterWrapperTop">
							<div class="FilterWrapperBottom">
								<div class="filterHeader">
									<h1><%= IsExistingHomesShowcase ? "Homes" : ((bool)((BaseMasterPage)Master).NewHomePage) ? "New Homes" : IsRentalsShowcase ? "Rentals" : "Lots/Land" %>
										in <%= ((microsite)Master).CurrentMicrosite.Name %></h1>
									<a href="<%= ((microsite)Master).OtherMicrositePath + (IsExistingHomesShowcase ? "search" : ((bool)((BaseMasterPage)Master).NewHomePage) ? "new-search" : IsRentalsShowcase ? "rentals" : "search-lots-land") %>">Click here for <%= ((microsite)Master).OtherMicrosite.Name %> properties »</a>
								</div>
								<div class="divide1">&nbsp;</div>
								<asp:PlaceHolder runat="server" ID="uxAgentPH">
									<div class="filter">
										<div class="filters first attributes realtor">
											<h2>Your Meybohm REALTOR<sup>&reg;</sup>
												<br />
												for these listings is:</h2>
											<asp:Image runat="server" ID="uxAgentImage" ResizerWidth="134" ResizerHeight="168" />
											<asp:Label runat="server" ID="uxAgentName" CssClass="realtorName"></asp:Label>
											<asp:Label runat="server" ID="uxAgentPhone" CssClass="realtorNumber"></asp:Label>
											<a href="<%= IsExistingHomesShowcase ? "search" : "new-search" %>">Search all properties »</a>
											<div class="clear"></div>
										</div>
										<div class="clear"></div>
									</div>
									<div class="divide1">&nbsp;</div>
								</asp:PlaceHolder>
								<div id="filter" class="filter">
									<div class="filters attributes first">
										<label for="<%= uxSearchText.ClientID %>">
											Search
											<asp:Literal runat="server" ID="uxAgentName2"></asp:Literal>
											By <% if (!IsRentalsShowcase)
				 { %>MLS Number, <% } %>Keyword or Address</label>
										<asp:TextBox runat="server" ID="uxSearchText" CssClass="text" MaxLength="50" data-bind="value: viewModel.SearchText" />
										<a href="#" class="button searchButton" id="searchButton" onclick="return false;"><span>Search</span></a>
									</div>
									<div class="editSavedSearchWrap">
										<% if (Page.User.Identity.IsAuthenticated)
			 { %>
										<a href="#addSearchDiv" class="button editSavedSearch">Add Filter Set to Saved Searches</a>
										<% }
			 else
			 { %>
										<a href="#" class="button" onclick="$('.loginExpander').trigger('click');return false;">Login to save searches</a>
										<% } %>
										<div class="clear"></div>
									</div>
									<div class="filterHeader small">
										<h1 class="floatLeft">Filters</h1>
										<a href="#" title="Reset All Filters" class="resetAllFilters floatRight" data-bind="visible: viewModel.Filter() != '' || viewModel.SearchText() != '' || viewModel.AddressLat() || viewModel.AddressLong() || viewModel.MinDistance() || viewModel.MaxDistance()">Reset
											All Filters</a>
										<div class="clear"></div>
									</div>
									<asp:Repeater runat="server" ID="uxFilterRepeater" ItemType="Classes.Showcase.ShowcaseAttribute">
										<ItemTemplate>
											<asp:PlaceHolder runat="server" ID="uxMoreThanMaxVisibleFiltersTop" Visible="<%#Settings.FilterDisplayStyle == FiltersDisplays.Top && Container.ItemIndex == Settings.NumberFiltersVisible%>">
												<div class="clear"></div>
												<div class="filterHeader small">
													<h1 class="floatLeft">More Filters:</h1>
												</div>
												<div class="slideWrapper filters">
													<div class="revealSlide">
														<a href="#moreFilters" class="expand up button">Show all filters</a>
													</div>
													<div class="filterContainer" style="display: none;">
														<div id="moreFilters" class="slide">
											</asp:PlaceHolder>
											<div class="filters attributes">
												<%
													if (Settings.FilterDisplayStyle == FiltersDisplays.Left)
													{%>
												<span class='filterMinimize<%#Container.ItemIndex < Settings.NumberFiltersVisible ? " open" : ""%>'>
													<a style="cursor: pointer;">Toggle filter</a>
												</span>
												<%
													}%>
												<asp:PlaceHolder runat="server" ID="uxRangeSliderResetPH" Visible="<%#EnumParser.Parse<FilterTypes>(Item.ShowcaseFilterID.ToString()) == FilterTypes.RangeSlider%>"><span class="filterTitle">
													<%#Item.Title + ":"%>
													<span class='rangeClear<%#Container.ItemIndex >= Settings.NumberFiltersVisible ? " hidden" : ""%>'>
														<a title="Reset Values">Reset Values</a></span> </span></asp:PlaceHolder>
												<asp:PlaceHolder runat="server" ID="uxNonRangeSliderResetPH" Visible="<%#EnumParser.Parse<FilterTypes>(Item.ShowcaseFilterID.ToString()) != FilterTypes.RangeSlider%>"><span class="filterTitle">
													<%#Item.Title + ":"%>
												</span></asp:PlaceHolder>
												<div class="clear"></div>
												<div class='attributeWrapper' style='<%#Settings.FilterDisplayStyle == FiltersDisplays.Left && Container.ItemIndex >= Settings.NumberFiltersVisible ? "display: none;": ""%>'>
													<asp:HiddenField runat="server" ID="uxAttributeID" Value="<%#Item.ShowcaseAttributeID.ToString()%>" />
													<asp:RadioButtonList RepeatLayout="UnorderedList" CssClass="filterRadio" runat="server" ID="uxFilterRadioButtonList" AppendDataBoundItems="true" DataSource="<%#Item.ShowcaseAttributeValues%>" OnDataBound="FilterList_DataBound"
														DataTextField="Value" DataValueField="Value" Visible="<%#EnumParser.Parse<FilterTypes>(Item.ShowcaseFilterID.ToString()) == FilterTypes.RadioButtonList%>">
														<asp:ListItem Text="All" Value="all" Selected="True"></asp:ListItem>
													</asp:RadioButtonList>
													<asp:PlaceHolder runat="server" ID="uxRadioButtonGridPH" Visible="<%#Item.ShowcaseFilterID == (int)FilterTypes.RadioButtonGrid%>">
														<asp:Repeater runat="server" ID="uxHeaders" DataSource="<%# ShowcaseAttributeHeader.ShowcaseAttributeHeaderGetByShowcaseAttributeID(Item.ShowcaseAttributeID) %>" ItemType="Classes.Showcase.ShowcaseAttributeHeader">
															<HeaderTemplate>
																<ul class="filterTitle">
															</HeaderTemplate>
															<ItemTemplate>
																<li>
																	<%# Item.Text %></li>
															</ItemTemplate>
															<FooterTemplate>
																</ul>
																<div class="clear"></div>
															</FooterTemplate>
														</asp:Repeater>
														<%--<asp:Repeater runat="server" ID="uxRadioButtonGrid" OnItemDataBound="uxRadioButtonGrid_ItemDataBound" DataSource='<%#ShowcaseAttributeValue.ShowcaseAttributeValueGetByShowcaseAttributeID(Item.ShowcaseAttributeID).OrderBy(v => v.DisplayOrder).ToList()%>' ItemType="Classes.Showcase.ShowcaseAttributeValue">
															<ItemTemplate>
																<asp:Label runat="server" ID="uxAttributeValue" CssClass="attributeHalf" Text="<%# Item.Value %>"></asp:Label>
																<asp:RadioButtonList RepeatLayout="UnorderedList" runat="server" ID="uxRadioButtons" CssClass="filterHalf" DataTextField="Text" DataValueField="Text" DataSource="<%# ShowcaseAttributeHeader.ShowcaseAttributeHeaderGetByShowcaseAttributeID(Item.ShowcaseAttributeID) %>">
																</asp:RadioButtonList>
															</ItemTemplate>
														</asp:Repeater>--%>
													</asp:PlaceHolder>
													<asp:CheckBoxList runat="server" CssClass="filterCheckbox" RepeatLayout="UnorderedList" ID="uxFilterCheckBoxList" AppendDataBoundItems="true" DataSource="<%#Item.ShowcaseAttributeValues%>" OnDataBound="FilterList_DataBound"
														DataTextField="Value" DataValueField="Value" Visible="<%#EnumParser.Parse<FilterTypes>(Item.ShowcaseFilterID.ToString()) == FilterTypes.CheckBoxList%>">
														<asp:ListItem Text="All" Value="all" Selected="True" Enabled="false"></asp:ListItem>
													</asp:CheckBoxList>
													<asp:ListBox CssClass="filterListbox" runat="server" ID="uxFilterListBox" AppendDataBoundItems="true" DataSource="<%#Item.ShowcaseAttributeValues%>" OnDataBound="FilterList_DataBound" DataTextField="Value" DataValueField="Value"
														Visible="<%#EnumParser.Parse<FilterTypes>(Item.ShowcaseFilterID.ToString()) == FilterTypes.ListBox%>" SelectionMode="Multiple">
														<asp:ListItem Text="All" Value="all" Selected="True"></asp:ListItem>
													</asp:ListBox>
													<asp:DropDownList CssClass="filterDropdown" runat="server" ID="uxFilterDropDown" AppendDataBoundItems="true" DataSource="<%#Item.ShowcaseAttributeValues%>" OnDataBound="FilterList_DataBound" DataTextField="Value" DataValueField="Value"
														Visible="<%#EnumParser.Parse<FilterTypes>(Item.ShowcaseFilterID.ToString()) == FilterTypes.DropDown%>">
														<asp:ListItem Text="All" Value="all" Selected="True"></asp:ListItem>
													</asp:DropDownList>
													<asp:PlaceHolder runat="server" Visible="<%#EnumParser.Parse<FilterTypes>(Item.ShowcaseFilterID.ToString()) == FilterTypes.Distance || EnumParser.Parse<FilterTypes>(Item.ShowcaseFilterID.ToString()) == FilterTypes.DistanceRange %>">
														<div class="masked">
															<label for='<%# uxFilterRepeater.ClientID + "_ctl" + Container.ItemIndex.ToString().PadLeft(2, '0') + "_uxFilterAddress" %>'>
																Address or Zipcode</label>
															<asp:TextBox runat="server" ID="uxFilterAddress" CssClass="text" placeholder="Address or Zipcode" data-bind="value: viewModel.AddressText" />
														</div>
													</asp:PlaceHolder>
													<asp:PlaceHolder runat="server" ID="uxFilterSliderPH" Visible="<%#(EnumParser.Parse<FilterTypes>(Item.ShowcaseFilterID.ToString()) == FilterTypes.Slider  || EnumParser.Parse<FilterTypes>(Item.ShowcaseFilterID.ToString()) == FilterTypes.Distance) && Item.ShowcaseAttributeValues.Count > 0%>">
														<div class="sliderWrapper" id="sliderDiv<%# Container.ItemIndex %>">
															<asp:DropDownList runat="server" ID="uxFilterSlider" DataSource="<%#Item.ShowcaseAttributeValues%>" OnDataBound="FilterList_DataBound" DataTextField="Value" DataValueField="Value" Style="display: none;">
															</asp:DropDownList>
														</div>
														<script type="text/javascript">
															$(document).ready(function () {
																$("#sliderDiv<%# Container.ItemIndex %> select").selectToUISlider({
																	tooltip: false,
																	sliderOptions: {
																		stop: sliderValueChanged
																	}
																}).hide();
															});
														</script>
													</asp:PlaceHolder>
													<asp:PlaceHolder runat="server" ID="uxFilterRangeSliderPH" Visible="<%#IsRangeSlider(Item.ShowcaseFilterID)%>">
														<table style="width: 100%;">
															<tr>
																<td class="rangeSlider">
																	<div class="sliderWrapper rangeWrapper" id="rangeSliderDiv<%# Container.ItemIndex %>">
																		<asp:DropDownList runat="server" ID="uxFilterRangeSlider" DataSource="<%# GetRangeSliderValues(Item, IsRangeSlider(Item.ShowcaseFilterID)) %>" OnDataBound="FilterList_DataBound"
																			Style="display: none;">
																		</asp:DropDownList>
																		<asp:DropDownList runat="server" ID="uxFilterRangeSlider2" DataSource="<%# GetRangeSliderValues(Item, IsRangeSlider(Item.ShowcaseFilterID)) %>" OnDataBound="FilterList_DataBound"
																			Style="display: none;">
																		</asp:DropDownList>
																	</div>
																	<script type="text/javascript">
																		$(document).ready(function () {
																			$("#rangeSliderDiv<%# Container.ItemIndex %> select").selectToUISlider({
																				customSlideFunction: "rangeSliderUpdateText",
																				tooltip: false,
																				sliderOptions: {
																					stop: rangeSliderValueChanged
																				}
																			}).hide();
																		});
																	</script>
																</td>
															</tr>
															<tr>
																<td>
																	<span class="rangeAmount">Current Range:
																		<asp:Literal runat="server" ID="uxMinimumAmount"></asp:Literal>
																		-
																		<asp:Literal runat="server" ID="uxMaximumAmount"></asp:Literal></span>
																</td>
															</tr>
														</table>
													</asp:PlaceHolder>
													<asp:PlaceHolder runat="server" Visible="<%#EnumParser.Parse<FilterTypes>(Item.ShowcaseFilterID.ToString()) == FilterTypes.RangeTextBoxes %>">
														<div class="masked">
															<asp:TextBox runat="server" ID="uxMinimumRange" CssClass="text floatLeft numbersOnly" Placeholder="Min" Width="40%" MaxLength="11" /><span class="floatLeft"> - </span>
															<asp:TextBox runat="server" ID="uxMaximumRange" CssClass="text floatLeft numbersOnly" Placeholder="Max" Width="40%" MaxLength="11" />
															<div class="clear"></div>
														</div>
													</asp:PlaceHolder>
													<div class="clear"></div>
												</div>
												<!--end attributeWrapper-->
											</div>
											<%# (Container.ItemIndex + 1 > Settings.NumberFiltersVisible ? Container.ItemIndex + 1 - Settings.NumberFiltersVisible : Container.ItemIndex + 1) % numberOfFiltersPerRow == 0 ? "<div class='clear'></div>" : ""%>
											<asp:PlaceHolder runat="server" ID="uxMoreThan4FiltersBottom" Visible="<%#Settings.FilterDisplayStyle == FiltersDisplays.Top && Container.ItemIndex + 1 > Settings.NumberFiltersVisible && Container.ItemIndex + 1 == m_FilterCount%>">
												<div class="clear"></div>
												</div> </div> </div>
												<!--end slideWrapper-->
											</asp:PlaceHolder>
										</ItemTemplate>
									</asp:Repeater>
									<asp:PlaceHolder runat="server" ID="uxOpenHouseFilterPH">
										<div class="filters attributes">
											<span class="filterMinimize">
												<a style="cursor: pointer;">Toggle filter</a>
											</span>
											<span class="filterTitle">Open House:</span>
											<div class="clear"></div>
											<div class="attributeWrapper" style="display: none;">
												<asp:DropDownList runat="server" ID="uxOpenHouse" CssClass="filterDropdown" data-bind="value: viewModel.OpenHouse">
													<asp:ListItem Text="All" Value=""></asp:ListItem>
													<asp:ListItem Text="Yes" Value="true"></asp:ListItem>
													<asp:ListItem Text="No" Value="false"></asp:ListItem>
												</asp:DropDownList>
											</div>
											<!--end attributeWrapper-->
										</div>
									</asp:PlaceHolder>
								</div>
								<div class="clear"></div>
							</div>
							<!--/filter wrapper bottom-->
						</div>
						<!--/filter wrapper top-->
					</div>
					<!--/filter wrapper side-->
					<!--end filterWrapper-->
					<% if (Settings.HideFiltersInSlideout)
		{ %>
				</div>
				<%} %>
			</asp:PlaceHolder>
			<div class="showcaseCategory" data-bind="className: 'showcaseCategory ' + viewModel.WrapperClass()">
				<%	if (showMap)
		{%>
				<div class="showcaseDisplayWrapper">
					<div class="mapLoading" style="display: none;">
						<div class="loadOverlay"></div>
						<div class="loadMessage"><span>Loading...</span></div>
					</div>

					<div class="showcaseDisplay" id="map_canvas"></div>
					<a class="previous resultsMap" title="Previous" href="#" data-bind="css:{disabled:!viewModel.PrevEnabled()},click:function(){viewModel.PrevClicked();}">Prev Page</a>
					<a class="next resultsMap" title="Next" href="#" data-bind="css:{disabled:!viewModel.NextEnabled()},click:function(){viewModel.NextClicked();}">Next Page</a>
					<div class="clear"></div>
				</div>
				<div class="clear"></div>
				<a href="#" class="toggleMap">Toggle Map</a>
				<a href="#" data-bind="attr:{href:CleanupLink('showcase-print-all.aspx?ShowcaseID=<%= m_ShowcaseID %>&' + viewModel.QueryString() + (viewModel.PageNumber() > 1 ? '&PageNumber=' + viewModel.PageNumber() : ''))}" target="_blank" class="printSummary btnShowcaseDetail floatRight">
					Print <%= IsRentalsShowcase ? "All" : "Results" %></a>
				<div class="slideWrapper linkWrapper resultsLink floatRight">
					<div class="revealSlide">
						<a class="linkToPage expand up btnShowcaseDetail" title="Share my results" href="#">Share my results</a>
					</div>
					<div class="linkContainer" style="display: none;">
						<div class="slide">
							<div id="LinkPopup">
								<asp:Label runat="server" ID="uxLinkToThisPage" data-bind="html: viewModel.LinkToThisPage" />
							</div>
						</div>
					</div>
				</div>
				<!--end slideWrapper-->
				<%
		}%>
				<div class="clear"></div>
				<div class="showcaseTile">
					<div class="showcaseResults">
						<div class="pageSizeSelector">
							<span>Results:</span>
							<ul>
								<li>
									<label for="uxNumResults_20">20</label>
									<input type="radio" value="20" id="uxNumResults_20" data-bind="checked: viewModel.PageSize" />
								</li>
								<li>
									<label for="uxNumResults_48">48</label>
									<input type="radio" value="48" id="uxNumResults_48" data-bind="checked: viewModel.PageSize" />
								</li>
								<li>
									<label for="uxNumResults_100">100</label>
									<input type="radio" value="100" id="uxNumResults_100" data-bind="checked: viewModel.PageSize" />
								</li>
							</ul>
							<div class="clear"></div>
						</div>
						<div class="sortFields">
							<span>Sort By:</span>
							<ul>
								<li>
									<label for="uxSortBy_ListPrice">List Price</label>
									<input type="radio" value="ListPrice" id="uxSortBy_ListPrice" data-bind="checked: viewModel.SortField" />
								</li>
								<li>
									<label for="uxSortBy_DateListed">Date Listed</label>
									<input type="radio" value="DateListed" id="uxSortBy_DateListed" data-bind="checked: viewModel.SortField" />
								</li>
								<li>
									<a href="#" class="sortAD" data-bind="html: viewModel.SortDirection() ? 'Ascending' : 'Descending', click: function(){viewModel.SortDirection(!viewModel.SortDirection()); return false;}"></a>
								</li>
							</ul>
							<div class="clear"></div>
						</div>
						<div class="clear"></div>
						<div class="controlsBottom controlsTop">
							<a class="previous" title="Previous" href="#" data-bind="css:{disabled:!viewModel.PrevEnabled()},click:function(){viewModel.PrevClicked();}">Prev Page</a>

							<a class="next" title="Next" href="#" data-bind="css:{disabled:!viewModel.NextEnabled()},click:function(){viewModel.NextClicked();}">Next Page</a>
						</div>
						<div class="clear"></div>
						<p id="numberResults" data-bind="html: viewModel.ShowingResultsMessage, visible: viewModel.TotalRowCount() > 0">
						</p>
						<asp:Label runat="server" ID="uxRentPricePerMonth" Text="All prices are per month" data-bind="visible: viewModel.TotalRowCount() > 0"></asp:Label>
						<span data-bind="visible: viewModel.TotalRowCount() == 0">There are no items to display</span>
						<div class="clear"></div>
					</div>
					<div class="categoryWrapper" data-bind="style:{height:viewModel.CategoryWrapperHeight()}">
						<ul id="category" data-bind="template: { name: 'listingTemplate', foreach: viewModel.OldListings }, afterRenderWireups:viewModel.OldListings">
						</ul>
						<ul id="category2" data-bind="template: { name: 'listingTemplate', foreach: viewModel.Listings }">
						</ul>
						<div class="clear"></div>
					</div>
					<!--end categoryWrapper-->
					<div class="controlsBottom">
						<a class="previous" title="Previous" href="#" data-bind="css:{disabled:!viewModel.PrevEnabled()},click:function(){viewModel.PrevClicked();}">Prev Page</a>

						<a class="next" title="Next" href="#" data-bind="css:{disabled:!viewModel.NextEnabled()},click:function(){viewModel.NextClicked();}">Next Page</a>
					</div>
					<div class="clear"></div>
				</div>
				<!--End showcaseTile-->
				<div class="ko_AJAXLoading" data-bind="visible: viewModel.Loading()">
					<div class="loadOverlay"></div>
					<div class="loadMessage"><span>Loading...</span></div>
				</div>
			</div>
			<!--end showcaseCategory-->
		</div>
		<!--end showcaseWrapper-->
	</div>
	<!--end showcase-->
	<a style="display: none;" href="showcase-listing.aspx?ShowcaseID=<%= m_ShowcaseID %>">Showcase Listing</a>
	<Showcase:AddSavedSearch runat="server" ID="uxAddSavedSearch" />
	<%--These global javascript variables need to be loaded before the JS files since they are referenced in showcase.js--%>
	<script type="text/javascript">
		// <![CDATA[		
		var agentID = <%= m_AgentID %>;
		var filterLessUrl = '<%= m_FilterlessUrl %>';
		var numberOfItemsToShowPerPage = <%=m_PageSize%>;
		var showcaseID = <%=m_ShowcaseID%>;
		var showcaseItemID = '<%= m_ShowcaseItemID %>';
		var showMap = <%= showMap.ToString().ToLower() %>;
		var resizerDomain = '<%= Globals.Settings.EnableParallelization ? Globals.Settings.ResizerSubdomain : "../" %>';
		var collapseFiltersAtStart = <%= Settings.HideFiltersInSlideout.ToString().ToLower() %>;
		var defaultSortField = '<%= m_SortField %>';
		var defaultSortDirection = <%= m_SortDirection.ToString().ToLower() %>;
		var isRental = <%= (m_ShowcaseID == (int)MeybohmShowcases.AikenRentalHomes || m_ShowcaseID == (int)MeybohmShowcases.AugustaRentalHomes).ToString().ToLower() %>;
		// ]]>
	</script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery-ui.custom.min.js,~/tft-js/core/selectToUISlider.jQuery.js,~/tft-js/core/easing.js,~/tft-js/core/knockout.js,~/tft-js/core/showcase.js,~/tft-js/core/jquery.quicksand.js,~/tft-js/core/jquery.mousewheel.min.js"></asp:Literal>
	<script type="text/html" id="listingTemplate">
		<li style="float: left;" data-bind="attr:{'data-id': ShowcaseItemID}">
			<a class="fancybox.iframe showcaseProject showcaseId<%= m_ShowcaseID %>" rel="homeGallery" href="#" data-bind="attr:{href: CleanupLink(DetailsPageUrl + (DetailsPageUrl.indexOf('?') > -1 ? '&' : '?') + viewModel.QueryString()), 'data-id': ShowcaseItemID}">
				<img alt="" src="<%= ResolveClientUrl("~/") %>img/loading.gif" data-bind="attr:{alt: Title, src: GetImageSrc(Image), width: viewModel.CurrentWidth(), height: viewModel.CurrentHeight()}" />
                <span class="showcaseAddress" data-bind="html: Address"></span>
                <span data-bind="html: Title"></span>
			</a>
			<a title="Open House Available" class="openHouse icon" data-bind="visible: HasOpenHouse"></a>
			<a title="Virtual Tour Available" class="virtualTour icon" data-bind="visible: VirtualTourURL && VirtualTourURL != ''"></a>
		</li>
	</script>
	<script type="text/javascript">
	    var HideList = <%= SetupAttributeFiltersForJavaScriptHiding() %>;
	    var DisplayList = <%= SetupAttributeFiltersForJavaScriptShowing() %>;
	    var filterCheckList = <%= ((int)Classes.Showcase.FilterTypes.CheckBoxList).ToString()%>;
	    var filterDropDown = <%= ((int)Classes.Showcase.FilterTypes.DropDown).ToString()%>;
	    var filterRadioList = <%= ((int)Classes.Showcase.FilterTypes.RadioButtonList).ToString()%>;
	    var filterRadioGrid = <%= ((int)Classes.Showcase.FilterTypes.RadioButtonGrid).ToString()%>;
	    var filterList = <%= ((int)Classes.Showcase.FilterTypes.ListBox).ToString()%>;
        
		// <![CDATA[
		<%= m_FilterKnockoutScript %>
		$(document).ready(function(){	
			$("body").addClass("theme-new");			
			ko.applyBindings(viewModel);			
			
			<% if (showMap)
	  { %>appendBootstrap();
			<% }
	  else
	  { %>viewModel.ReadyToLoadData(true);<% } %>
		});
		// ]]>
	</script>
</asp:Content>
