$(document).ready(function () {
	IE6UpdatePanel();
});

function IE6UpdatePanel() {
	$("input").each(function () {
		if ($(this).attr("type") == "checkbox" && !$(this).hasClass("checkbox"))
			$(this).addClass("checkbox");
		else if ($(this).attr("type") == "radio" && !$(this).hasClass("radio"))
			$(this).addClass("radio");
		else if ($(this).attr("type") == "submit" && !$(this).hasClass("submit"))
			$(this).addClass("submit");
		else if ($(this).attr("type") == "reset" && !$(this).hasClass("reset"))
			$(this).addClass("reset");
		else if ($(this).attr("type") == "button" && !$(this).hasClass("button"))
			$(this).addClass("button");
		if ($(this).next().is("label"))
			$(this).next().addClass("inputLabel");
	});

	$("li").hover(function () {
		$(this).addClass("hover");
	}, function () {
		$(this).removeClass("hover");
	});
}