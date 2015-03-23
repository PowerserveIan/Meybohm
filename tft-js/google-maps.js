var map;
var markers = [];
var infowindow;
var bounds;
var mapLoaded = function () {
	if (!markerList)
		return;
	for (var i = 0; i < markerList.length; i++) {
		createMarker(markerList[i].Latitude, markerList[i].Longitude, markerList[i].Content);
	}
	if (typeof (maxZoomLevel) != "undefined")
		setMapZoom(maxZoomLevel);
}

function appendBootstrap() {
	var script = document.createElement("script");
	script.type = "text/javascript";
	script.src = "http://maps.google.com/maps/api/js?sensor=false&callback=handleApiReady";
	document.body.appendChild(script);
}

function handleApiReady() {
	var myOptions = {
		mapTypeId: google.maps.MapTypeId.ROADMAP
	}
	map = new google.maps.Map($(".mapContainer")[0], myOptions);
	bounds = new google.maps.LatLngBounds();
	mapLoaded();
}

function createMarker(latitude, longitude, content) {
	var latlng = new google.maps.LatLng(latitude, longitude);
	var marker = new google.maps.Marker({ position: latlng, map: map });
	marker.html = content;
	var markerIndex = markers.length;

	google.maps.event.addListener(marker, "click", function () {
		showInfoWindow(markerIndex);
	});

	markers.push(marker);
	bounds.extend(marker.position);
	map.fitBounds(bounds);
}

var zoom_counter = 0;
function setMapZoom(maxZoomLevel) {
	if (map.getZoom() > maxZoomLevel)
		map.setZoom(maxZoomLevel);
	while (zoom_counter < 4 && map.getZoom() != maxZoomLevel) {
		zoom_counter++;
		setTimeout("setMapZoom(" + maxZoomLevel + ");", 1000);
	}
}

function showInfoWindow(markerIndex) {
	if (!infowindow || infowindow.content != markers[markerIndex].html) {
		if (infowindow)
			infowindow.close();
		infowindow = new google.maps.InfoWindow({ content: markers[markerIndex].html });
		infowindow.open(map, markers[markerIndex]);
	}
	else if (infowindow.content == markers[markerIndex].html && !infowindow.map)
		infowindow.open(map, markers[markerIndex]);
}

var markerItem = function (lat, long, content) {
	this.Latitude = lat;
	this.Longitude = long;
	this.Content = content;
}

$(document).ready(function () {
	if (typeof(blockInitialLoad) == "undefined" || !blockInitialLoad)
		appendBootstrap();
});