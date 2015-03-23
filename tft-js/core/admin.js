(function ($) {
	$.fn.menuScroll = function () {

		return this.each(function () {
			var $menu = $(this),
				$subNav = $('> li > ul', $menu),
				$wrapper = $menu.parent(),
				$items = $('> li', $menu),
				$navigation = $menu.siblings('div'),
				$leftArrow = $navigation.find('a.scrollLeft'),
				$rightArrow = $navigation.find('a.scrollRight'),
				scrollAmount = 200,
				totalWidth = 0,
				maxWidth = $menu.width(),
				visibleWidth = maxWidth - $navigation.outerWidth();

			$items.each(function () {
				totalWidth += $(this).outerWidth(true);
			});

			if (totalWidth > maxWidth) {
				$wrapper.addClass('scrolling');
				$menu.wrap('<div class="navScroll" />');

				var overflow = totalWidth - visibleWidth,
					$scroller = $menu.parent(),
					currentOffset = $('> li.current', $menu).length > 0 ? Math.floor($('> li.current', $menu).offset().left - $('div.content div.wrapper').offset().left + $('> li.current', $menu).outerWidth() - 1) : 0,
					currentPage = Math.floor(currentOffset / visibleWidth);

				if (currentOffset > visibleWidth) {
					if (overflow > visibleWidth) {
						$scroller.scrollLeft(currentPage * visibleWidth);
						$subNav.css('left', currentPage * visibleWidth + 1 + 'px');
					} else {
						$scroller.scrollLeft(currentOffset - visibleWidth);
						$subNav.css('left', currentOffset - visibleWidth + 1 + 'px');
					}
				}

				function arrowState() {
					if ($scroller.scrollLeft() == 0) {
						$leftArrow.addClass('disabled');
						$rightArrow.removeClass('disabled');
					} else if ($scroller.scrollLeft() >= overflow) {
						$leftArrow.removeClass('disabled');
						$rightArrow.addClass('disabled');
					} else {
						$leftArrow.removeClass('disabled');
						$rightArrow.removeClass('disabled');
					}
				}
				arrowState();

				function scrollNav(dir) {
					if (overflow > visibleWidth) {
						var currentScroll = $scroller.scrollLeft();

						scrollAmount = visibleWidth * dir + currentScroll;

						if (scrollAmount <= totalWidth) {
							if (parseInt($subNav.css('left')) + visibleWidth * dir > 0) {
								$subNav.filter(':not(:animated)').animate({
									left: '+=' + visibleWidth * dir
								}, 500);
							} else {
								$subNav.filter(':not(:animated)').animate({
									left: 1
								}, 500);
							}
							$scroller.filter(':not(:animated)').animate({
								scrollLeft: '+=' + visibleWidth * dir
							}, 500, function () { arrowState(); });
						}
					} else {
						if (dir > 0) {
							$subNav.filter(':not(:animated)').animate({
								left: overflow + 1
							}, 500);
							$scroller.filter(':not(:animated)').animate({
								scrollLeft: overflow
							}, 500, function () {
								$leftArrow.removeClass('disabled');
								$rightArrow.addClass('disabled');
							});
						} else {
							$subNav.filter(':not(:animated)').animate({
								left: 1
							}, 500);
							$scroller.filter(':not(:animated)').animate({
								scrollLeft: 0
							}, 500, function () {
								$leftArrow.addClass('disabled');
								$rightArrow.removeClass('disabled');
							});
						}
					}
				}

				$leftArrow.click(function () { scrollNav(-1); return false });
				$rightArrow.click(function () { scrollNav(1); return false });
			}
		});
	};
})(jQuery);
var changeMade = false;
var suppressOnBeforeUnload = false;
function MasterRunAtDocReady() {
	changeMade = false;
	suppressOnBeforeUnload = false;
	$(function () {
		if ($("ul.popUp.menu").length > 0 && $("ul.popUp.menu").attr("style") == null) {
			$('ul.popUp.menu').menuScroll();
			$("ul.popUp a.current").each(function ($childMenu) {
				$(this).parent().addClass("current");
				$(this).removeClass("current");
			});

			var orgHeight = $('ul.popUp.menu').outerHeight(),
				firstTier = orgHeight,
				newHeight = $('ul.popUp.menu > li.parent.current > ul').outerHeight() + orgHeight;
			if (newHeight > orgHeight) {
				$('ul.popUp.menu').height(newHeight);
				orgHeight = newHeight;
			}
			
			var hoverTimer;
			$('ul.popUp.menu').hover(
				function () {
					clearTimeout(hoverTimer);
				},
				function () {
					hoverTimer = setTimeout(function () {
					$('ul.popUp.menu > li.current').removeClass('unhover');
					}, 600);
				}
			);
			$('ul.popUp.menu').hover(
				function () {
					$(this).toggleClass('hovered');
				}
			);
			function drops_show() {
				$(this).addClass('dropped').removeClass('delay');
				if ($(this).hasClass('parent')) {
					var firstHeight = $(this).find('> ul').outerHeight() + orgHeight;
					if (!$(this).hasClass('current')) {
						$('ul.popUp.menu > li.current').addClass('unhover');
						$('ul.popUp.menu > li.current > ul').addClass('hidden');
					} else {
						$(this).removeClass('unhover');
					}
				}
				$('ul.popUp.menu').height(firstHeight);
			}
			function drops_hide() {
				if (!$(this).hasClass('current') || ($(this).hasClass('current') && $(this).siblings('.dropped').length == 0))
					$('ul.popUp.menu > li.current > ul').removeClass('hidden');
				$(this).removeClass('dropped').addClass('delay');
				if (!$('ul.popUp.menu').hasClass('hovered')) {
					$('ul.popUp.menu').height(orgHeight);
					$('ul.popUp.menu > li.current').removeClass('unhover');
				}
			}
			$('ul.popUp > li').addClass('delay');
			$('ul.popUp > li').hoverIntent({
				sensitivity: 5,
				over: drops_show,
				timeout: 600,
				out: drops_hide
			});

			$('ul.popUp.menu ul li.parent').hover(
			function () {
				firstTier = $('ul.popUp.menu').height();
				$('ul.popUp.menu').height(firstTier + $(this).find('> ul').outerHeight());
			},
			function () {
				$('ul.popUp.menu').height(firstTier);
			});
		}
		$('a.top').click(function () {
			scroll(0, 0);
			return false;
		});

		$("span.tooltip span").hide();
		$("span.tooltip").hover(
		function () {
			$(this).addClass('hovered').find("span").show();
		},
		function () {
			$(this).removeClass('hovered').find("span").hide();
		});
		$("span.tooltip span").hover(
		function () {
			$(this).show();
		},
		function () {
			$(this).hide();
		});

		$(":not(span) input.button").each(function () {
			$(this).wrap("<span class='" + $(this).attr('class') + "' />");
			$(this).removeClass($(this).attr("class"));
		});

		if ($("ul.breadcrumbs").height() > 28)
			$("ul.breadcrumbs").addClass("small");

		setTimeout(function () {
			$(".masked input, .masked textarea").placeholder();
		}, 5);

		$(".formHalf").each(function () {
			if ($(this).next(".formHalf:not(.staticError)").length > 0 && $(this).offset().left < $(this).next(".formHalf").offset().left) {
				var heightDiff = $(this).next(".formHalf").height() - $(this).height();
				if (heightDiff >= 16 && !$(this).find("label:first,span.label:first").hasClass("fiveLine, fourLine, threeLine, twoLine"))
					$(this).find("label:first,span.label:first").addClass((heightDiff >= 48 ? "fourLine" : (heightDiff >= 32 ? "threeLine" : "twoLine")));
				else if (heightDiff <= -16 && !$(this).next(".formHalf").find("label:first,span.label:first").hasClass("fiveLine, fourLine, threeLine, twoLine"))
					$(this).next(".formHalf").find("label:first,span.label:first").addClass((heightDiff <= -48 ? "fourLine" : (heightDiff <= -32 ? "threeLine" : "twoLine")));
			}
		});

		$("span.button").click(function (e) {
			if ($(e.target).not("input").length > 0)
				$(this).find("input").click();
		});

		$("#uxGiveFeedbackLink").fancybox({
			height: 630,
			padding: 0,
			width: 550
		});

		function textToLink(text) {
			var exp = /(\b(https?|ftp|file):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/ig;
			return text.replace(exp, "<a href='$1' target='_blank'>$1</a>");
		}
		var $tweetContent = $('.tweet span');
		if ($tweetContent.length > 0)
			$tweetContent.html(textToLink($tweetContent.html()));

		$(".formWrapper input, .formWrapper textarea, .formWrapper select").change(function () {
			changeMade = true;
		});
		$("input[type=submit],table.listing a.button").click(function () {
			suppressOnBeforeUnload = true;
		});
		window.onbeforeunload = confirmAdminEditExit;
		function confirmAdminEditExit() {
			if (!suppressOnBeforeUnload && changeMade)
				return "You have unsaved changes that will be lost.  Do you wish to continue?";
		}

		if ($('div.fixedBottom')[0]){
			var $w = $(window),
				$fixed = $('.fixedBottom'),
				$content = $('div.whiteContent'),
				$footer = $('div.footer'),
				didScroll = false,
				didResize = false;
			$content.css('padding-bottom', parseInt($content.css('padding-bottom')) + $fixed.outerHeight());
			$w.scroll(function(){
				didScroll = true;
			});
			$w.resize(function(){
				didResize = true;
			});
			setInterval(function(){
				if (didResize || didScroll) {
					didResize = false;
					didScroll = false;
					fixedLocation();
				}
			}, 125);
			function fixedLocation(){
				var scrollPosition = $w.scrollTop(),
					windowHeight = $w.height(),
					footerScroll = $footer.offset().top;
				if (scrollPosition > (footerScroll - windowHeight)) {
					$fixed.addClass('pinned');
				}
				else {
					$fixed.removeClass('pinned');
				}
			}
			fixedLocation();
		}

		$(".numbersOnly").keydown(function (e) {
			var key = e.charCode || e.keyCode || 0;
			return key == 8 || key == 9 || (e.ctrlKey && key == 86) || key == 46 || (key >= 37 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == 110 || key == 188 || key == 190;
		});
	});
}

MasterRunAtDocReady();

Sys.Net.WebRequestManager.add_invokingRequest(onInvoke);
Sys.Net.WebRequestManager.add_completedRequest(onComplete);
var running = false;
function onInvoke(sender, args) {
	running = true;
	setTimeout(function () { if (running) $("div.AJAXLoading").show(); }, 500);
}

function onComplete(sender, args) {
	$("div.AJAXLoading").hide();
	running = false;
}