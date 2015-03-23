// JScript File
var currentCountryIndex = 0;
var stateSelectField = '';
function Country_Change(CountrySelectField, StateSelectField, StateSelectDiv, StateOtherField, StateOtherDiv, UseShipTo) {
	//parameters are the clientIDs of the fields, not jQuery objects
	if (CountrySelectField.options[CountrySelectField.selectedIndex].text.toLowerCase() == "united states" || CountrySelectField.options[CountrySelectField.selectedIndex].text.toLowerCase() == "canada") {
		$("#" + StateOtherDiv).hide();
		$("#" + StateOtherField).val("");
		$("#" + StateSelectField).selectedIndex = 0;
		$("#" + StateSelectDiv).show();
		if (currentCountryIndex != CountrySelectField.selectedIndex) {
			stateSelectField = StateSelectField;
			currentCountryIndex = CountrySelectField.selectedIndex;
			StateAndCountryWebMethods.GetStatesByCountryID(parseInt(CountrySelectField.options[CountrySelectField.selectedIndex].value), UseShipTo, Load_States);
		}
		else
			scworking = false;
	}
	else if (CountrySelectField.options[CountrySelectField.selectedIndex].value == "") {
		//no country selected
		$("#" + StateOtherDiv).hide();
		$("#" + StateSelectField).selectedIndex = $(StateSelectField).length - 1;
		$("#" + StateSelectDiv).show();
		scworking = false;
	}
	else {
		//International
		$("#" + StateOtherDiv).show();
		$("#" + StateSelectField).selectedIndex = $(StateSelectField).length - 1;
		$("#" + StateSelectDiv).hide();
		scworking = false;
	}
}

function Load_States(results, userContext, methodName) {
	$("#" + stateSelectField).find('option:gt(0)').remove().end();
	for (var i = 0; i < results.length; i++) {
		$("#" + stateSelectField).append('<option value="' + results[i].StateID + '">' + results[i].Name + '</option>');
	}
	scworking = false;
}

function ValidateStateSelection(state, stateOther, country) {
	//parameters are already jQuery objects
	argsIsValid = true;

	if ((state != null) && (stateOther != null) && (country != null)) {
		if ((country.val() == "")) {
			argsIsValid = false;
		}
		else if (country.find("option:selected").text().toLowerCase() == "united states" || country.find("option:selected").text().toLowerCase() == "canada") {
			if (state.val() == "")
				argsIsValid = false;
			else
				argsIsValid = true;
		}
		else
			argsIsValid = true;
	}
	else
		argsIsValid = false;

	return argsIsValid;
}

function ValidateCountrySelection(country) {
	return country != null && country.val() != "";
}