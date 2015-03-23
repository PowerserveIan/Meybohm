$(document).ready(function () {
	if (eval("typeof ValidatorHookupEvent == 'function'")) {
		$("input, select").each(function () {
			ValidatorHookupEvent($(this)[0], "onblur", "ValidatorOnChange(event);");
		});
	}
	/**
	* Re-assigns a couple of the ASP.NET validation JS functions to
	* provide a more flexible approach
	*/
	function UpgradeASPNETValidation() {
		// Hi-jack the ASP.NET error display only if required
		if (typeof (Page_ClientValidate) != "undefined") {
			ValidatorUpdateDisplay = NicerValidatorUpdateDisplay;
		}
	}

	/**
	* Extends the classic ASP.NET validation to add a class to the parent span when invalid
	*/
	function NicerValidatorUpdateDisplay(val) {
		for (i = 0; i < Page_Validators.length; i++) {
			if (Page_Validators[i] == val && !val.isvalid) {
				$("#" + Page_Validators[i].controltovalidate).addClass('error');
				$("#" + Page_Validators[i].controltovalidate).parents('div.formHalf, div.formThird, div.formWhole').addClass('errorWrapper').removeClass('validWrapper');
			}
			else if (Page_Validators[i] == val && val.isvalid) {
				/*Check all previous validators to see if they set the error class*/
				previouslyValid = true;
				for (j = 0; j < i; j++) {
					if (Page_Validators[j].controltovalidate == Page_Validators[i].controltovalidate && !Page_Validators[j].isvalid) {
						previouslyValid = false;
						break;
					}
				}
				if (previouslyValid) {
					$("#" + Page_Validators[i].controltovalidate).removeClass('error');
					$("#" + Page_Validators[i].controltovalidate).parents('div.formHalf, div.formThird, div.formWhole').removeClass('errorWrapper').addClass('validWrapper');
				}
			}
		}
		if (typeof (val.display) == "string") {
			if (val.display == "None")
				return;
			if (val.display == "Dynamic") {
				val.style.display = val.isvalid ? "none" : "inline";
				return;
			}
		}
		val.style.visibility = val.isvalid ? "hidden" : "visible";
	}

	UpgradeASPNETValidation();

	$("span.validator").each(function () {
		if ($(this).find("span.asterisk").length == 0)
			$(this).html("<span class='errorMessage'><span class='asterisk'>*</span><span class='errorText'>" + $(this).html() + "</span></span>");
	});
});