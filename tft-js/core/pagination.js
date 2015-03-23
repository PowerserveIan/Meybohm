(function ($) {
	$.fn.pagination = function (options) {
		var defaults = $.fn.pagination.defaults;
		var o = $.extend({}, defaults, options);
		return this.each(function () {
			var $wrapper = $(this),
				$navigation = $wrapper.find('ul'),
				$pages = $('> li', $navigation),
				pageWidth = $pages.outerWidth();
			if ($navigation.parents('.scroller').length > 0) {
				$navigation.unwrap();
				$navigation.next().remove();
				$navigation.css('width', 'auto');
			}

			if ($pages.length > o.pagesVisible) {
				var $scroller = $navigation.wrap('<div class="scroller" />').parent(),
				scrollerWidth = pageWidth * o.pagesVisible,
				$current = $('> span:visible', $pages).parent(),
				currentNumber = $current.index(),
				halfPagesVisible = parseInt(o.pagesVisible / 2);

				if (currentNumber <= halfPagesVisible)
					currentOffset = 0;
				else if (currentNumber >= $pages.length - halfPagesVisible)
					currentOffset = pageWidth * (halfPagesVisible + 1);
				else
					currentOffset = (currentNumber - halfPagesVisible) * pageWidth;

				$scroller.css({ 'float': 'left', 'width': scrollerWidth, 'overflow': 'hidden' });
				$navigation.css('width', ($pages.length * pageWidth) + 'px');
				$navigation.css("margin-left", -1 * currentOffset + "px");

				if (o.scrollable) {
					if (!o.customScrollbar) {
						$scroller.css({ 'overflow-x': 'scroll' });
						$navigation.css("margin-left", "0");
						$scroller.scrollLeft(currentOffset);
					}
					else {
						$scroller.after('<div class="slider" />');
						var $slider = $scroller.siblings(".slider");
						$slider.wrap('<div class="sliderWrapper" />');
						$slider.slider({
							slide: function (event, ui) {
								if ($navigation.width() > $scroller.width()) {
									$navigation.css("margin-left", Math.round(
									ui.value / 100 * ($scroller.width() - $navigation.width())
								) + "px");
								} else {
									$navigation.css("margin-left", 0);
								}
							}
						});
						$slider.find(".ui-slider-handle").append("<span class='grip'></span>");
						var $handleWrapper = $slider.find(".ui-slider-handle").wrap("<div class='handleWrapper' />").parent();
						$handleWrapper.find(".ui-slider-handle").mousedown(function () {
							$slider.width($handleWrapper.width());
						}).mouseup(function () {
							$slider.width("100%");
						});
						var remainder = $navigation.width() - $scroller.width();
						var proportion = remainder / $navigation.width();
						var handleSize = $scroller.width() - (proportion * $scroller.width());
						$slider.find(".ui-slider-handle").css({
							width: handleSize - 2,
							"margin-left": -handleSize / 2
						});
						$handleWrapper.width("").width($slider.width() - handleSize);

						var percentage = Math.round(currentOffset / remainder * 100);
						$slider.slider("value", percentage);
					}
				}
			}
		});
	};
	$.fn.pagination.defaults = {
		scrollable: true,
		customScrollbar: true,
		pagesVisible: 5
	};
})(jQuery);