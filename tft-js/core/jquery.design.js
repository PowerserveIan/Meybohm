/* * * * * Misc. jQuery application of design * * * * */

/* SHARED PAGE TEMPLATE STYLES */
$(function () {
	$("div.globalContact").hide();
	$("a.contactToggle").click(function () {
		var $globalContact = $(this).next("div.globalContact");
		if ($globalContact.is(':hidden')) {
			$globalContact.slideDown();
			$(this).children().html('Collapse Contact Us');
		}
		else {
			$globalContact.slideUp();
			$(this).children().html('Contact Us');
		}
		$("html, body").animate({ scrollTop: $globalContact.offset().top - $globalContact.height() });
		return false;
	});

	$(".masked input, .masked textarea").placeholder();

	$('a.loginExpander').click(function () {
		$('div.login').slideToggle(500);
		return false;
	});

	//	ERROR MESSAGE TEXT ON HOVER
	//	$("span.errorText").hide();
	//	$("span.errorMessage").css('cursor' , 'help').hover(
	//		function () {
	//			var thisSpan = $(this).find("span.errorText");
	//			thisSpan.show();
	//			if (thisSpan.height() + parseInt(thisSpan.css("margin-bottom")) + parseInt(thisSpan.css("margin-top")) + parseInt(thisSpan.css("padding-top")) + parseInt(thisSpan.css("padding-bottom")) + thisSpan.offset().top > $(window).scrollTop() + $(window).height())
	//				thisSpan.css("top", "-" + thisSpan.height() + "px");
	//		},
	//		function () {
	//			$(this).find("span.errorText").hide();
	//		}
	//	);
});

Sys.Net.WebRequestManager.add_invokingRequest(onInvoke);
Sys.Net.WebRequestManager.add_completedRequest(onComplete);

var canShowLoading = true;
var running = false;
function onInvoke(sender, args) {
    running = true;
    
    if (canShowLoading)
	setTimeout(function () { if (running) $("div.AJAXLoading").show(); }, 500);
}

function onComplete(sender, args) {
	$("div.AJAXLoading").hide();
	running = false;
}
var homeDetailsFancyboxParams = {
	'height': 605,
	'width': 960,
	'padding': 0,
	'scrolling': 'no',
	'fitToView': false//,
	//'mouseWheel': false
};

