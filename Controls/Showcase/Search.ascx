<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Controls_MainContent_Search" %>
<div>
	<!-- extra div for IE7 -->
	<div class="tabbedBoxContainer">
		<div class="search-bar">
			<asp:TextBox runat="server" ID="uxSearchBox" placeholder="Search all properties..."></asp:TextBox>
            <asp:Button runat="server" ID="uxSearchBtn" Text="Search" OnClick="uxSearchBtn_Click"/>
		</div>
		<ul class="tabs">
			<li>
				<a href="#" id="buy" class="current">Buy</a></li>
			<li runat="server" id="uxRentLink">
				<a href="#" id="rent">Rent</a></li>
		</ul>
		<div class="panes">
			<h2>Finding a property is easy!</h2>
			<div class="formWrapper newHomes">
				<div class="formHalf" id="buyMin">
					<label for="min_price">
						Min Price:</label>
					<select id="min_price">
						<option selected="selected" value="0">Any</option>
						<option value="100000">$100,000</option>
						<option value="150000">$150,000</option>
						<option value="200000">$200,000</option>
						<option value="250000">$250,000</option>
						<option value="300000">$300,000</option>
						<option value="400000">$400,000</option>
						<option value="750000">$750,000</option>
						<option value="1000000">$1,000,000</option>
					</select>
					<div class="clear"></div>
				</div>
				<div class="formHalf" id="buyMax">
					<label for="max_price">
						Max Price:</label>
					<select id="max_price">
						<option selected="selected" value="99999999">Any</option>
						<option value="100000">$100,000</option>
						<option value="150000">$150,000</option>
						<option value="200000">$200,000</option>
						<option value="250000">$250,000</option>
						<option value="300000">$300,000</option>
						<option value="400000">$400,000</option>
						<option value="750000">$750,000</option>
						<option value="99999999">$1,000,000+</option>
					</select>
					<div class="clear"></div>
				</div>
				<div class="clear"></div>
				<asp:PlaceHolder runat="server" ID="uxRentTab">
					<div class="formHalf" id="rentMin" style="display: none;">
						<label for="min_price">
							Min Rent:</label>
						<select id="min_price_rent">
							<option selected="selected" value="0">Any</option>
							<option value="500">$500</option>
							<option value="1000">$1,000</option>
							<option value="1500">$1,500</option>
							<option value="2000">$2,000</option>
							<option value="2500">$2,500</option>
							<option value="3000">$3,000</option>
						</select>
						<div class="clear"></div>
					</div>
					<div class="formHalf" id="rentMax" style="display: none;">
						<label for="max_price">
							Max Rent:</label>
						<select id="max_price_rent">
							<option selected="selected" value="3000">Any</option>
							<option value="500">$500</option>
							<option value="1000">$1,000</option>
							<option value="1500">$1,500</option>
							<option value="2000">$2,000</option>
							<option value="2500">$2,500</option>
							<option value="3000">$3,000</option>
						</select>
						<div class="clear"></div>
					</div>
				</asp:PlaceHolder>
				<div class="clear"></div>
				<div class="formHalf">
					<label for="beds">
						Beds:</label>
					<select id="beds">
						<option value="0">Any</option>
						<option value="1<5">1+</option>
						<option value="2<5">2+</option>
						<option value="3<5">3+</option>
						<option value="4<5">4+</option>
						<option value="5">5+</option>
					</select>
					<div class="clear"></div>
				</div>
				<div class="formHalf">
					<label for="baths">
						Baths:</label>
					<select id="baths">
						<option value="0">Any</option>
						<option value="1<5">1+</option>
						<option value="2<5">2+</option>
						<option value="3<5">3+</option>
						<option value="4<5">4+</option>
						<option value="5">5+</option>
					</select>
					<div class="clear"></div>
				</div>
				<div class="clear"></div>
				<div class="blueButtonContainer">
					<a href="#" class="buttonBlue" id="homesLink"><span id="numberHomes"></span>&nbsp;Matches<span class="blueButtonArrow">&raquo;</span></a>
					<div class="clear"></div>
				</div>
			</div>
			<a class="moreSearch" href="<%= SearchLink %>">See More Search Options &raquo;</a>
		</div>
	</div>
</div>
<script type="text/javascript">
	$(document).ready(function () {
		var updateBeds = function(minBeds, maxBeds) {
			var selectedBedValue =  $("#beds").val();
			var bedArray = ["0", "1", "2", "3", "4", "5"];
			var displayBedArray = ["Any", "1+", "2+", "3+", "4+", "5+"];
			$("#beds").empty();
			for(var i=0;i<bedArray.length;i++) {
				if(i==0||(parseInt(bedArray[i])>=selectedBedValue&&parseInt(bedArray[i])>=minBeds)&&(parseInt(bedArray[i])<=maxBeds)){
					$("#beds").append($("<option></option>").val(bedArray[i]).html(displayBedArray[i]));
				}
			}
			$("#beds").val(selectedBedValue);
		};
		var updateBaths = function(minBaths, maxBaths) {
			var selectedBedValue =  $("#baths").val();
			var bathArray = ["0", "1", "2", "3", "4", "5"];
			var displayBathArray = ["Any", "1+", "2+", "3+", "4+", "5+"];
			$("#baths").empty();
			for(var i=0;i<bathArray.length;i++) {
				if(i==0||(parseInt(bathArray[i])>=selectedBedValue&&parseInt(bathArray[i])>=minBaths)&&(parseInt(bathArray[i])<=maxBaths)){
					$("#baths").append($("<option></option>").val(bathArray[i]).html(displayBathArray[i]));
				}
			}
			$("#baths").val(selectedBedValue);
		};
		var updatePrices = function(minPrice, maxPrice) {
			var minSelectedValue = $("#min_price").val();
			var maxSelectedValue =$("#max_price").val()==""?"99999999":$("#max_price").val();
    		
			var priceArray = ["0", "100000", "150000", "200000", "250000", "300000", "400000", "750000", "1000000"];
			var displayPriceArray = ["Any", "$100,000", "$150,000", "$200,000", "$250,000", "$300,000", "$400,000", "$750,000", "$1,000,000"];
			var maxPriceArray = ["99999999", "100000", "150000", "200000", "250000", "300000", "400000", "750000", "99999999"];
			var maxDisplayPriceArray = ["Any", "$100,000", "$150,000", "$200,000", "$250,000", "$300,000", "$400,000", "$750,000", "$1,000,000+"];
			$("#min_price").empty();
			$("#max_price").empty();
			$("#min_price").append($("<option></option>").val(priceArray[0]).html(displayPriceArray[0]));
			$("#max_price").append($("<option></option>").val(maxPriceArray[0]).html(maxDisplayPriceArray[0]));
			for(var i=1;i<priceArray.length;i++) {
				if((parseInt(priceArray[i])<=maxSelectedValue&&parseInt(priceArray[i])<=maxPrice)){
					$("#min_price").append($("<option></option>").val(priceArray[i]).html(displayPriceArray[i]));
				}
			}
			$("#min_price").val(minSelectedValue);

			for(var n=1;n<maxPriceArray.length;n++) {
				if(parseInt(priceArray[n])>=minSelectedValue&&parseInt(maxPriceArray[n])>=minPrice){
					$("#max_price").append($("<option></option>").val(maxPriceArray[n]).html(maxDisplayPriceArray[n]));
				}
			}
			$("#max_price").val(maxSelectedValue);
		};
		var updateRentalPrices = function(minPrice, maxPrice) {
			var minSelectedValue = $("#min_price_rent").val();
			var maxSelectedValue = $("#max_price_rent").val();
    		
			var priceArray = ["0", "500", "1000", "1500", "2000", "2500", "3000"];
			var displayPriceArray = ["Any", "$500", "$1,000", "$2,000", "$2,500", "$3,000"];
			var maxPriceArray = ["", "500", "1000", "1500", "2000", "2500", "3000"];
			var maxDisplayPriceArray =["Any", "$500", "$1,000","$1,500", "$2,000", "$2,500", "$3,000"];
			$("#min_price_rent").empty();
			$("#max_price_rent").empty();
			$("#min_price_rent").append($("<option></option>").val(priceArray[0]).html(displayPriceArray[0]));
			for(var i=1;i<priceArray.length;i++) {
				if((parseInt(priceArray[i])>=minSelectedValue||(i+1<priceArray.length&&parseInt(priceArray[i+1])>minPrice))&&(parseInt(priceArray[i])<=maxSelectedValue&&parseInt(priceArray[i])<=maxPrice)){
					$("#min_price_rent").append($("<option></option>").val(priceArray[i]).html(displayPriceArray[i]));
				}
			}
			$("#min_price_rent").val(minSelectedValue);
			$("#max_price_rent").append($("<option></option>").val(maxPriceArray[0]).html(maxDisplayPriceArray[0]));
			for(var n=1;n<maxPriceArray.length;n++) {
				if((parseInt(maxPriceArray[n])<=maxSelectedValue||(n+1<maxPriceArray.length&&parseInt(maxPriceArray[n+1])<maxPrice))&&parseInt(maxPriceArray[n])>=minPrice){
					$("#max_price_rent").append($("<option></option>").val(maxPriceArray[n]).html(maxDisplayPriceArray[n]));
				}
			}
			$("#max_price_rent").val(maxSelectedValue);
		};
		var updateQuicksearchDropdownOptions = function(minPrice, maxPrice, minBeds, maxBeds, minBaths, maxBaths,minRentalPriceResult,maxRentalPriceResult) {
		    
			//updatePrices(0, 999999);
			//updateRentalPrices(minRentalPriceResult,maxRentalPriceResult);
			//updateBeds(minBeds, maxBeds);
			//updateBaths(minBaths, maxBaths);
		};
	    
		$("#<%= uxSearchBox.ClientID%>").keypress(function(event){
			if(event.keyCode == 13){
				$("#<%= uxSearchBtn.ClientID%>").trigger("click");
			}
		});
        
		canShowLoading = false;
        
		$("#buy").click(function () {
			if ($(this).hasClass("current")||($("#buy").hasClass("Prog_disabled")||$("#rent").hasClass("Prog_disabled")))
				return false;
			$("#buyMin,#buyMax").show();
			$("#rentMin,#rentMax").hide();
			$(this).addClass("current");
			$("#rent").removeClass("current");
			UpdateQuickSearchResults();
			return false;
		});
		<% if (!NewHomes){%>
		$("#rent").click(function () {
			if ($(this).hasClass("current")||($("#buy").hasClass("Prog_disabled")||$("#rent").hasClass("Prog_disabled")))
				return false;
			$("#buyMin,#buyMax").hide();
			$("#rentMin,#rentMax").show();
			$(this).addClass("current");
			$("#buy").removeClass("current");
			UpdateQuickSearchResults();
			return false;
		});<%} %>
		$("#min_price, #max_price, #min_price_rent, #max_price_rent, #beds, #baths").change(function () {
			UpdateQuickSearchResults();
			//updatePrices( $("#min_price").val(),  $("#max_price").val());
			//updateRentalPrices($("#min_price_rent").val(),$("#max_price_rent").val());
			//updateBeds($("#beds").val(), 12);
			//updateBaths($("#baths").val(), 12);
		});
		
		function UpdateQuickSearchResults() {
			
			var buying = $("#buy").hasClass("current");
			$("#buy").addClass("Prog_disabled");
			$("#rent").addClass("Prog_disabled");
			
			var residentialString = '<%= !NewHomes ? (IsAiken ? Classes.Showcase.Settings.AikenExistingPropertyTypeAttributeID : Classes.Showcase.Settings.AugustaExistingPropertyTypeAttributeID) + ":Residential|" : "" %>';
			var filtersString = '';
			var showcaseID = buying ? <%= m_ShowcaseID %> : <%= m_RentShowcaseID %>;
			var priceFilterID = buying ? <%= m_PriceFilterID %> : <%= m_RentPriceFilterID %>;
			var bedroomFilterID = buying ? <%= m_BedroomFilterID %> : <%= m_RentBedroomFilterID %>;
			var bathroomFilterID = buying ? <%= m_BathroomFilterID %> : <%= m_RentBathroomFilterID %>;
			var listingLink = buying ? "<%= SearchLink %>" : "rentals";
			
			if (buying)
				filtersString = residentialString;
			if (buying && ($("#min_price").val() != "0" || $("#max_price").val() != "99999998"|| $("#max_price").val() != "99999999"))
				filtersString += priceFilterID + ":>" + $("#min_price").val() + "<"+$("#max_price").val() + "|";
			if (!buying && ($("#min_price_rent").val() != "0" || $("#max_price_rent").val() != ""))
				filtersString += priceFilterID + ":>" + $("#min_price_rent").val() +"<"+ $("#max_price_rent").val() + "|";
			if ($("#beds").val() != "")
				filtersString += bedroomFilterID + ":>" + $("#beds").val() + "|";
			if ($("#baths").val() != "")
				filtersString += bathroomFilterID + ":>" + $("#baths").val();
			
			filtersString = filtersString.replace(/\|*$/, "");
			$("#numberHomes").html("Loading");
			$("#homesLink").attr("href", listingLink + (filtersString == '' ? '' : '?Filters=' + filtersString));
			$(".moreSearch").attr("href", listingLink);
			
			ShowcaseWebMethods.GetItemCountForQuickSearch(filtersString, showcaseID, <%= NewHomes.ToString().ToLower() %>, quickSearch_success);
			ShowcaseWebMethods.GetUpdatedMinMaxForQuickSearch(filtersString, showcaseID, updateQuickSearchDropdowns_success);
		}

    	
		function updateQuickSearchDropdowns_success(results, userContext, methodName) {
			var maxPriceResult = 0;
			var minPriceRsult = 0;
			var minBedResult = 0;
			var maxBedResult = 0;
			var minBathRsult = 0;
			var maxBathResult = 0;
			var maxRentalPriceResult = 0;
			var minRentalPriceResult = 0;
			for(var i = 0;i<results.length;i++) {
				if (results[i].Title == "Baths") {
					minBathRsult = results[i].MinValue;
					maxBathResult = results[i].MaxValue;
				} 
				else if (results[i].Title == "Full Baths") {
					minBathRsult = results[i].MinValue;
					maxBathResult = results[i].MaxValue;
				}else if (results[i].Title == "Bedrooms") {
					minBedResult = results[i].MinValue;
					maxBedResult = results[i].MaxValue;
				} else if (results[i].Title == "List Price") {
					minPriceRsult = results[i].MinValue;
					maxPriceResult = results[i].MaxValue;
				}
				else if (results[i].Title == "Price") {
					minPriceRsult = results[i].MinValue;
					maxPriceResult = results[i].MaxValue;
				}
				else if (results[i].Title == "Rental Price") {
					minRentalPriceResult = results[i].MinValue;
					maxRentalPriceResult = results[i].MaxValue;
				}
			}
			updateQuicksearchDropdownOptions(minPriceRsult,maxPriceResult, minBedResult, maxBedResult, minBathRsult, maxBathResult,minRentalPriceResult,maxRentalPriceResult);//minprice,maxprice,minbed,maxbed,minbath,maxbath
		}

		function quickSearch_success(results, userContext, methodName) {
			$("#numberHomes").html(results);
			$("#buy").removeClass("Prog_disabled");
			$("#rent").removeClass("Prog_disabled");
		}

		UpdateQuickSearchResults();
	});	
    
</script>