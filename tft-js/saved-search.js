$(document).ready(function () {
	$(".editSavedSearch").fancybox({
		minHeight: 600,
		padding: 0,
		fitToView: false,
		scrolling: 'no', 
		afterClose: function () {
			$("#savedSearchWrapper").show();
			$("#savedSearchSaveMessage").hide();
			$("#uxSavedSearchName").val("");
			$("input[id*=uxSavedSearchEmailNotifications]:checked, input[id*=uxSavedSearchSeparateEmail]:checked, input[id*=uxSavedSearchDailyEmail]:checked").removeAttr("checked");
			$("#separateEmail, #dailyEmail").hide();
		}
	});
	$("input[id*=uxSavedSearchEmailNotifications]").click(function () {
		if ($(this).val() == 'true')
			$("#separateEmail, #dailyEmail").show();
		else
			$("#separateEmail, #dailyEmail").hide();
	});
	$("#saveSavedSearch").click(function () {
		if (!Page_ClientValidate("SavedSearch"))
			return false;
		var enableEmailNotifications = $("input[id*=uxSavedSearchEmailNotifications]:checked").val();
		var separateEmail = $("input[id*=uxSavedSearchSeparateEmail]:checked").length > 0 && $("input[id*=uxSavedSearchSeparateEmail]:checked").val();
		var dailyEmail = $("input[id*=uxSavedSearchDailyEmail]:checked").length > 0 && $("input[id*=uxSavedSearchDailyEmail]:checked").val();
		var showcaseID = parseInt($("input[id$=uxShowcaseID]").val());
		var newProperties = $("input[id$=uxNewProperties]").val();
		var showcaseItemID = $("input[id$=uxShowcaseItemID]").length > 0 && $("input[id$=uxShowcaseItemID]").val() != "" ? parseInt($("input[id$=uxShowcaseItemID]").val()) : null;
		var savedSearchID = $("input[id$=uxSavedSearchID]").length > 0 && $("input[id$=uxSavedSearchID]").val() != "" ? parseInt($("input[id$=uxSavedSearchID]").val()) : null;
		ShowcaseWebMethods.SaveSearch(showcaseID, $("#savedSearchUrl").val(), showcaseItemID, $("#uxSavedSearchName").val(), enableEmailNotifications, separateEmail, dailyEmail, savedSearchID,newProperties, saved_search_success);
		return false;
	});

	function saved_search_success(results, userContext, methodName) {
		$("#savedSearchWrapper").hide();
		$("#savedSearchSaveMessage").show();		
	}
});

function ValidateEmailNotifications(sender, args) {
	args.IsValid = $("input[id*=uxSavedSearchEmailNotifications]:checked").length > 0;
}
function ValidateSeparateEmail(sender, args) {
	args.IsValid = $("input[id*=uxSavedSearchEmailNotifications]:checked").val() == 'false' || $("input[id*=uxSavedSearchSeparateEmail]:checked").length > 0;
}
function ValidateDailyEmail(sender, args) {
	args.IsValid = $("input[id*=uxSavedSearchEmailNotifications]:checked").val() == 'false' || $("input[id*=uxSavedSearchDailyEmail]:checked").length > 0;
}