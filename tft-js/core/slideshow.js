(function ($) {
	$.fn.slideShow = function (options) {
		var defaults = $.fn.slideShow.defaults;
		var o = $.extend({}, defaults, options);

		var userInputCounter = 0;
		var currentImageDuration = 0;
		var direction = true;
		var initialState = true;
		var working = false;
		var numberWidth, currentButton, intervalID, imageCountdownIntervalID, imageCountdownWidthDecrementer;
		var $ul = this;
		var $buttonPanel;
		var $dynamicHeader = $("<div />").addClass("dynamicHeader").addClass("clearfix");
		var $postDynamicHeader;
		if (o.overlayDiv) {
			$("div." + o.overlayDiv).after($dynamicHeader);
			$postDynamicHeader = $("div." + o.overlayDiv).next();
		}
		this.wrap($dynamicHeader);
		$dynamicHeader = this.parent();
		var $dynamicHeaderContainer = $("<div />").addClass("dynamicHeaderContainer");
		this.wrap($dynamicHeaderContainer);
		$dynamicHeaderContainer = this.parent();

		if (o.showNavigation && this.find("li").length > 1) {
			var $navigation = $('<div />').addClass("slideShowNavigation").appendTo(o.overlayDiv ? $postDynamicHeader : $dynamicHeader);
			var $leftArrow = $('<a />').addClass("leftArrow").attr("href", "#").html("&laquo;").click(function () {
				PreviousImage();
				return false;
			}).appendTo($navigation);
			var $pause = $('<a />').addClass("pause").attr("href", "#").html("Pause").click(function () {
				clearInterval(intervalID);
				$(this).hide();
				$play.show();
				return false;
			}).appendTo($navigation);
			var $play = $('<a />').addClass("play").attr("href", "#").html("Play").click(function () {
				$(this).hide();
				$pause.show();
				intervalID = setInterval(AutoScrollImages, 1000);
				return false;
			}).appendTo($navigation);
			var $rightArrow = $('<a />').addClass("rightArrow").attr("href", "#").html("&raquo;").click(function () {
				NextImage();
				return false;
			}).appendTo($navigation);
			$('<div />').addClass("clear").appendTo($navigation);
			if (o.playByDefault)
				$play.hide();
			else
				$pause.hide();
		}

		if (o.showThumbnails || o.genericThumbnail) {
			var $thumbnailContainer = $('<div />').addClass("slideShowThumbs").appendTo(o.overlayDiv ? $postDynamicHeader : $dynamicHeader);
			var $thumbnails = this.clone().removeClass("images").addClass("thumbs");
			$thumbnails.find("li").hide();
			$thumbnails.find("li .caption").remove();
			if (o.transition == "Slide")
				initialState = false;
			$thumbnails.find("li").click(function () {
				if ($ul.find("li").index($ul.find("li.displayed")) != ($thumbnails.find("li").index($(this)) + $ul.find("li.previous").length))
					SwapImages($ul.find("li:eq(" + ($thumbnails.find("li").index($(this)) + $ul.find("li.previous").length) + ")"));
				return false;
			});

			$thumbnails.find("li img").each(function () {
				if (o.showThumbnails) {
					if (o.usingResizer)
						$(this).attr("src", $(this).attr("src").replace("width=" + $(this).css("width").replace("px", "") + "&height=" + $(this).css("height").replace("px", ""), "width=" + o.thumbnailWidth + "&height=" + o.thumbnailHeight)).attr("width", o.thumbnailWidth).attr("height", o.thumbnailHeight).load(function () {
							$(this).parents("li").show();
						});
					else
						$(this).attr("width", o.thumbnailWidth).attr("height", o.thumbnailHeight);
				}
				$(this).parent().attr("href", "#");
				if (o.genericThumbnail) {
					$(this).parents("li").show();
					$(this).parents("a").addClass("thumb");
					$(this).remove();
				}
			});
			$thumbnails = $thumbnails.appendTo($thumbnailContainer);
		}
		else if (o.showNumbersAsThumbnails && this.find("li").length > 1) {
			$buttonPanel = $('<div />').addClass("csButtonPanel").attr("style", "display: block;").appendTo(o.overlayDiv ? $postDynamicHeader : $dynamicHeader);
			this.find("li").each(function () {
				var btnIndex = $ul.find("li").index($(this));
				var $buttonContainer = $('<div />').addClass("csBtnContain").click(function () {
					if (btnIndex != ($ul.find("li").index($ul.find("li.displayed")) - $ul.find("li.previous").length)) {
						direction = btnIndex > $ul.find("li").index($ul.find("li.displayed"));
						SwapImages($ul.find("li:eq(" + (btnIndex + $ul.find("li.previous").length) + ")"));
					}
				}).appendTo($buttonPanel);
				$('<div />').addClass("csBtnBase").appendTo($buttonContainer);
				$('<div />').addClass("csBtnAni").appendTo($buttonContainer);
				$('<div />').addClass("csBtnClkHov").appendTo($buttonContainer);
				$('<div />').addClass("csBtnNum").html(btnIndex + 1).appendTo($buttonContainer);
			});
		}
		$dynamicHeaderContainer.css({ "width": o.slideWidth, "height": o.slideHeight });
		if (o.transition == "Crossfade") {
			this.find("li").css({ "position": "absolute", "top": "0", "left": "0" });
			this.find("li:first").css({ "z-index": "3" });
		} else if (o.transition == "Slide") {
			var $prev = $ul.find("li:last").clone().addClass("previous displayed");
			$ul.find("li:first").clone().addClass("last").appendTo($ul);
			$prev.prependTo($ul);
			var $hideSlide = $("<div />").addClass("hideSlide").css({ "width": o.slideWidth, "height": o.slideHeight, "overflow": "hidden", "position": "absolute", "top": "0", "left": "0" });
			if (o.slideWidth > 960) {
				$dynamicHeaderContainer.css({ "position": "absolute", "margin-left": (-o.slideWidth / 2) + "px", "left": "50%" });
				var $dynamicHeaderContainerContainer = $("<div />").addClass("headerContainer").css({ "position": "relative", "height": o.slideHeight, "overflow": "hidden" });
				$dynamicHeaderContainer.wrap($dynamicHeaderContainerContainer);
			}
			$ul.wrap($hideSlide);
			$ul.find("li").css({ "float": "left", "position": "relative" });
			$ul.addClass("clearfix").css({ "position": "absolute", "top": "0", "width": ($ul.find("li").length * o.slideWidth) + "px", "left": -o.slideWidth });
		}
		this.find("li").show();
		if (o.hasVideos)
			$ul.find("a[href*='.flv']").css({ "display": "block", "width": o.slideWidth + "px", "height": o.slideHeight + "px" }).flowplayer({ src: "motion/flowplayer-3.2.7.swf", wmode: 'transparent' }, { clip: { autoPlay: false, autoBuffering: true, onResume: function () { if (o.playByDefault) clearInterval(intervalID); }, onFinish: function () { if (o.playByDefault) setTimeout(function () { NextImage(); intervalID = setInterval(AutoScrollImages, 1000); }, 1000); } } });

		if (o.playByDefault && this.find("li").length > 1)
			intervalID = setInterval(AutoScrollImages, 1000);

		function SwapImages(liToShow) {
			if (!working) {
				working = true;
				if (o.transition == "Crossfade") {
					if (initialState) {
						working = false;
						liToShow.css({ "z-index": "3" });
					}
					else {
						liToShow.css({ "z-index": "2" });
						$ul.find("li.displayed").fadeOut("slow", function () {
							liToShow.css({ "z-index": "3" });
							$(this).css("z-index", "");
							$(this).show();
							working = false;
						});
					}
				} else if (o.transition == "Slide") {
					var temp = liToShow;
					var slidesJumped = ($ul.find("li").index($ul.find("li.displayed")) - $ul.find("li").index(liToShow)) * (direction ? -1 : 1);
					if (!initialState) {
						$ul.animate({ "left": (direction ? '-=' : '+=') + (o.slideWidth * slidesJumped) }, 500, function () {
							if (temp.hasClass("last"))
								$ul.animate({ "left": -o.slideWidth }, 0);
							else if (temp.hasClass("previous"))
								$ul.animate({ "left": -o.slideWidth * ($ul.find("li").index($ul.find("li:last")) - 1) }, 0);
							working = false;
						});
						if (liToShow.hasClass("last"))
							liToShow = $ul.find("li:eq(1)");
						else if (liToShow.hasClass("previous"))
							liToShow = $ul.find("li:eq(" + ($ul.find("li").index($ul.find("li:last")) - 1) + ")");
					}
					else
						working = false;
				}
				$ul.find("li.displayed").removeClass("displayed");
				liToShow.addClass("displayed");
				currentImageDuration = liToShow.find("input[id$=uxDuration]").val();
				if (o.showThumbnails || o.genericThumbnail) {
					$(".slideShowThumbs ul.thumbs li.displayed").removeClass("displayed");
					$(".slideShowThumbs ul.thumbs li:eq(" + ($ul.find("li").index(liToShow) - $ul.find("li.previous").length) + ")").addClass("displayed");
				}
				if (o.showNumbersAsThumbnails) {
					if (currentButton != null) {
						currentButton.find("div.csBtnBase").css("background-position", "0px -18px");
						currentButton.find("div.csBtnAni").css("width", 0);
					}
					currentButton = $buttonPanel.find("div.csBtnContain:eq(" + ($ul.find("li").index(liToShow) - $ul.find("li.previous").length) + ")");
					currentButton.find("div.csBtnBase").css("background-position", "0px -54px");
					currentButton.find("div.csBtnAni").css("background-position", "0px 0px").css("width", "16px");
					numberWidth = 16;
					imageCountdownWidthDecrementer = 16 / (currentImageDuration * 10);
					clearInterval(imageCountdownIntervalID);
					imageCountdownIntervalID = setInterval(ImageCountdown, 100);
				}
				userInputCounter = 0;
				initialState = false;
				if (o.hasVideos) {
					if (liToShow.find("a[href*='.flv']").length > 0)
					{
						clearInterval(intervalID);
						if (liToShow.find("a[href*='.flv']").flowplayer(0).getState() == 1)
							liToShow.find("a[href*='.flv']").flowplayer(0).play();
					}
				}
			}
		}

		function NextImage() {
			userInputCounter = 0;
			direction = true;
			var visibleLI = $ul.find("li.displayed");
			if (visibleLI.next().length > 0)
				SwapImages(visibleLI.next());
			else
				SwapImages($ul.find("li:first"));
		}

		function PreviousImage() {
			userInputCounter = 0;
			direction = false;
			var visibleLI = $ul.find("li.displayed");
			if (visibleLI.prev().length > 0)
				SwapImages(visibleLI.prev());
			else
				SwapImages($ul.find("li:last"));
		}

		function AutoScrollImages() {
			userInputCounter++;
			if (userInputCounter > parseInt(currentImageDuration))
				NextImage();
		}

		function ImageCountdown() {
			numberWidth -= imageCountdownWidthDecrementer;
			currentButton.find("div.csBtnAni").css("width", numberWidth + "px");
		}
	};

	$.fn.slideShow.defaults = {
		genericThumbnail: false,
		hasVideos: false,
		slideHeight: 300,
		slideWidth: 300,
		overlayDiv: null,
		playByDefault: true,
		showNavigation: true,
		showNumbersAsThumbnails: false,
		showThumbnails: true,
		thumbnailHeight: 70,
		thumbnailWidth: 70,
		transition: "Crossfade",
		usingResizer: true
	};
})(jQuery);
