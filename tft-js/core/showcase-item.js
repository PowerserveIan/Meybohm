$(document).ready(function () {
	$("div#tabs div.rightSide").css("overflow-y", "auto");

	$("a.linkToPage").click(function () {
		$("div.linkContainer").toggle(300);
	});

	$('.projectPage').mousewheel(function (event, delta) {
		return false;
	});
	$('.leftSide').mousewheel(function (event, delta) {
		event.stopPropagation();
		return true;
	});
	$(".parentWindow").click(function () {
		if (parent.window) {
			parent.window.location = $(this).attr("href");
			return false;
		}
	});
});

function TrackClick(clickType) {
	if (!isOwnProperty && $.inArray(clickType, clickTypesTracked) == -1) {
		ShowcaseWebMethods.TrackClick(showcaseItemID, clickType);
		clickTypesTracked.push(clickType);
	}
}