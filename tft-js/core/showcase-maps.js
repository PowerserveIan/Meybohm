var geocoder;
var map;
var markers = new Array();
var bounds;
var resultsArray = new Array();
var infowindow;

function handleApiReady() {
	geocoder = new google.maps.Geocoder();
	var myOptions = {
		mapTypeId: google.maps.MapTypeId.ROADMAP
	}
	map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
	if ($("input[id*=uxFilterAddress]").length > 0 && viewModel.AddressText()) {
		var slider = $("input[id*=uxFilterAddress]").parents(".attributeWrapper").find("div[id^=rangeSliderDiv] div.ui-slider");
		if (slider.length > 0)
			rangeSliderValueChanged(null, slider.parent());
		else {
			slider = $("input[id*=uxFilterAddress]").parents(".attributeWrapper").find("div[id^=sliderDiv]  div.ui-slider");
			if (slider.length > 0)
				sliderValueChanged(null, slider.parent());
		}
		SetUserAddress(true, false);
	}
	else
		viewModel.ReadyToLoadData(true);
}
function showInfoWindow(markerIndex) {
	var doOpen = false;
	if (!infowindow || infowindow.content != markers[markerIndex].html) {
		if (infowindow) infowindow.setOptions({ content: markers[markerIndex].html });
		else infowindow = new google.maps.InfoWindow({ content: markers[markerIndex].html });
		doOpen = true;
	}
	else if (infowindow.content == markers[markerIndex].html && !infowindow.map)
		doOpen = true;

	if (doOpen)
		infowindow.open(map, markers[markerIndex]);
}

function createMarker(showcaseItem, latlng, isLastAddress) {
	var marker = new google.maps.Marker({ position: latlng, map: map });
	var myHTML = '<div class="mapMarker">' +
                            '<a onclick="mapMarkerClick(\'' + showcaseItem.DetailsPageUrl + '\');return false;" href="' + showcaseItem.DetailsPageUrl + '">' + showcaseItem.Title + '</a>' +
                            '<img class="mapImage" alt="' + showcaseItem.Title + '" src="' + (showcaseItem.Image.indexOf('http') >= 0 ? showcaseItem.Image : resizerDomain + 'uploads/images/' + showcaseItem.Image + '?width=70&height=50&mode=crop&anchor=middlecenter') + '" />' +
                            '<span class="mapAddress">' + (isRental ? showcaseItem.Summary : showcaseItem.Address + '<br />' + showcaseItem.City + ", " + showcaseItem.State) +
                            '<a class="more" onclick="mapMarkerClick(\'' + showcaseItem.DetailsPageUrl + '\');return false;" href="' + showcaseItem.DetailsPageUrl + '">' + 'View Information' + '</a>' +
                            '</span>' +
                            '<div class="clear">' +
                            '</div>' +
                        '</div>';
	marker.html = myHTML;
	var markerIndex = markers.length;

	google.maps.event.addListener(marker, "click", function () {
		showInfoWindow(markerIndex);
	});

	if (isLastAddress)
		$("div.mapLoading").hide();
	return marker;
}

function mapMarkerClick(href) {
	$("ul#category a[href^='" + href + "']").click();
}

function clearMarkers() {
	$("div.mapLoading").show();
	for (var i = 0; i < markers.length; i++) {
		markers[i].setMap(null);
	}
	markers.length = 0;
	bounds = new google.maps.LatLngBounds();
	if (infowindow)
		infowindow.close();
}

function codeAddress(showcaseItem, isLastAddress) {
	if (geocoder) {
		if (showcaseItem != null) {
			if (showcaseItem.GeocodedLat && showcaseItem.GeocodedLong) {
				var latlng = new google.maps.LatLng(showcaseItem.GeocodedLat, showcaseItem.GeocodedLong);
				markers[markers.length] = createMarker(showcaseItem, latlng, isLastAddress);
				bounds.extend(markers[markers.length - 1].position);
				map.fitBounds(bounds);
				setTimeout("setMapZoom(17);", 100);
			}
			else {
				var address = $.trim((showcaseItem.Address + " " + showcaseItem.City + " " + showcaseItem.State + " " + showcaseItem.Country + " " + showcaseItem.Zipcode).replace(/null/g, ""));
				if (address != "") {
					geocoder.geocode({ 'address': address }, function (results, status) {
						if (status == google.maps.GeocoderStatus.OK) {
							markers[markers.length] = createMarker(showcaseItem, results[0].geometry.location, isLastAddress);
							bounds.extend(markers[markers.length - 1].position);
							map.fitBounds(bounds);
							setTimeout("setMapZoom(17);", 100);
						}
					});
				}
			}
			if (isLastAddress)
				$("div.mapLoading").hide();
		}
	}
}

function appendBootstrap() {
	var script = document.createElement("script");
	script.type = "text/javascript";
	script.src = "http://maps.google.com/maps/api/js?sensor=false&callback=handleApiReady";
	document.body.appendChild(script);
}

var handlingMarkers = false;
var settingFilters = false;
function SetUserAddress(setFilters, handleMarkers) {
	handlingMarkers = handleMarkers;
	settingFilters = setFilters;
	if (handleMarkers) {
		for (var i = 0; i < markers.length; i++) {
			if (markers[i].position.lat() == viewModel.AddressLat() && markers[i].position.lng() == viewModel.AddressLong()) {
				markers[i].setMap(null);
				break;
			}
		}
		bounds = new google.maps.LatLngBounds();
		for (var i = 0; i < markers.length; i++) {
			bounds.extend(markers[i].position);
		}
	}
	if (viewModel.AddressText() != '') {
		geocoder.geocode({ 'address': viewModel.AddressText() }, function (results, status) {
			if (status == google.maps.GeocoderStatus.OK) {
				if (handlingMarkers) {
					markers[markers.length] = new google.maps.Marker({ position: results[0].geometry.location, map: map,
						icon: new google.maps.MarkerImage("http://www.geocodezip.com/mapIcons/marker_blue.png")
					});
					bounds.extend(markers[markers.length - 1].position);
					map.fitBounds(bounds);
					setTimeout("setMapZoom(17);", 100);
				}
				viewModel.ReadyToLoadData(false);
				viewModel.AddressLat(results[0].geometry.location.lat());
				viewModel.AddressLong(results[0].geometry.location.lng());
				viewModel.ReadyToLoadData(true);
			}
		});
	}
	else {
		viewModel.ReadyToLoadData(false);
		viewModel.AddressLat(null);
		viewModel.AddressLong(null);
		viewModel.ReadyToLoadData(true);
	}
}

var counter = 0;
function setMapZoom(maxZoomLevel) {
	if (map.getZoom() > maxZoomLevel)
		map.setZoom(maxZoomLevel);
	while (counter < 4 && map.getZoom() != maxZoomLevel) {
		counter++;
		setTimeout("setMapZoom(" + maxZoomLevel + ");", 1000);
	}
}

$(document).ready(function () {
	var originalMapHeight = $(".showcaseDisplayWrapper").height();
	var paddingTop = parseInt($(".showcaseDisplayWrapper").css("padding-top").replace("px", ""));
	var paddingBottom = parseInt($(".showcaseDisplayWrapper").css("padding-bottom").replace("px", ""));
	$("a.toggleMap").click(function () {
		if ($(this).hasClass("collapsed")) {
			$(this).removeClass("collapsed");
			$(".showcaseDisplayWrapper").show().animate({ "height": originalMapHeight }, 400).animate({ "padding-top": paddingTop, "padding-bottom": paddingBottom }, 0);
			$("#map_canvas").animate({ "height": originalMapHeight }, 400);
		}
		else {
			$(this).addClass("collapsed");
			$(".showcaseDisplayWrapper").animate({ "padding-top": 0, "padding-bottom": 0 }, 0).animate({ "height": 0 }, 400, function () { $(this).hide(); });
			$("#map_canvas").animate({ "height": 0 }, 400);
		}
		return false;
	});
});