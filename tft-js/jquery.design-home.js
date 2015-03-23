$(document).ready(function () {
	var $landingMap = $("div#lmc");

	$landingMap.find("a.left").mouseenter(function () {
		var $this = $(this);
		var $augusta = $("div#augustaMap");
		$augusta.stop().fadeIn('100');
		$this.mouseleave(function () {
			$augusta.stop().fadeOut('100');
		});
	});
	$landingMap.find("a.right").mouseenter(function () {
		var $this = $(this);
		var $aiken = $("div#aikenMap");
		$aiken.stop().fadeIn('100');
		$this.mouseleave(function () {
			$aiken.stop().fadeOut('100');
		});
	});


	if ($('html').hasClass('ie7')) {
		$landingMap.find("a.left").mouseenter(function () {
			$this = $(this);
			$this.parent().addClass('augusta');
			$this.mouseleave(function () {
				$this.parent().removeClass('augusta');
			});
		});
		$landingMap.find("a.right").mouseenter(function () {
			$this = $(this);
			$this.parent().addClass('aiken');
			$this.mouseleave(function () {
				$this.parent().removeClass('aiken');
			});
		});
	};
});