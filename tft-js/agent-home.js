var viewModel = {
	ContactRequest: ko.observable(null),
	PropertyRequests: ko.observableArray([]),
	PropertyPageSize: ko.observable(5),
	PropertySortField: ko.observable("Created"),
	PropertySortDirection: ko.observable(false),
	PropertyTotalCount: ko.observable(0),
	PropertyLoading: ko.observable(true),
	AgentRequests: ko.observableArray([]),
	AgentPageSize: ko.observable(5),
	AgentSortField: ko.observable("Created"),
	AgentSortDirection: ko.observable(false),
	AgentTotalCount: ko.observable(0),
	AgentLoading: ko.observable(true),
	PropertyListings: ko.observableArray([]),
	PropertyListingsPageSize: ko.observable(5),
	PropertyListingsSortField: ko.observable("NumberOfVisits"),
	PropertyListingsSortDirection: ko.observable(false),
	PropertyListingsTotalCount: ko.observable(0),
	PropertyListingsLoading: ko.observable(true),
	EmailsLoading: ko.observable(true),
	Emails: ko.observableArray([]),
	EmailsToPullBack: 10
};
viewModel.SetPropertySort = function (field) {
	if (this.PropertySortField() == field)
		this.PropertySortDirection(!this.PropertySortDirection());
	this.PropertySortField(field);
};
viewModel.SetAgentSort = function (field) {
	if (this.AgentSortField() == field)
		this.AgentSortDirection(!this.AgentSortDirection());
	this.AgentSortField(field);
};
viewModel.SetPropertyListingsSort = function (field) {
	if (this.PropertyListingsSortField() == field)
		this.PropertyListingsSortDirection(!this.PropertyListingsSortDirection());
	this.PropertyListingsSortField(field);
};
function FormatDate(dateJSON, format) {
	if (!dateJSON)
		return "";
	var date = new Date(parseInt(dateJSON.substr(6)));
	return $.format.date(date, format)
}
$(document).ready(function () {
	var newsletterDetailsParams = {
		height: 600,
		width: 550,
		padding: 0
	};
	var contactRequestParams = {
		height: 600,
		width: 550
	};
	var shareListingParams = {
		height: 600,
		width: 550
	};
	var sendStatsParams = {
		height: 600,
		width: 550,
		beforeLoad: function(){
			$("#sendSharedListings").hide();
			$("#sendStats").show();
			$("#statsShowcaseItemID").val($(this.element).parents("tr").find("td.first input[type=hidden]").val());
		}
	};
	$(".newsletterLink").fancybox(newsletterDetailsParams);
	$(".showcaseProject").fancybox(homeDetailsFancyboxParams);
	$(".contactRequest").fancybox(contactRequestParams);
	$(".shareListings").fancybox(shareListingParams).click(function(){
		if ($("#propertyListing input[type=checkbox]:checked").length == 0) {
			alert("Please select at least one property to share");
			return false;
		}
		$("#sendSharedListings").show();
		$("#sendStats").hide();
	});
	$(".sendStats").fancybox(sendStatsParams);

	ko.applyBindings(viewModel);

	if ($("#propertyLoading").length > 0)
		ko.dependentObservable(function () {
			$.ajax({
				type: "POST",
				url: defaultPageUrl + '/GetPropertyRequests',
				data: '{pageSize:"' + viewModel.PropertyPageSize() + '",sortField:"' + viewModel.PropertySortField() + '",sortDirection:' + viewModel.PropertySortDirection() + '}',
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (data) {
					if (data.d.Items.length > 0) {
						viewModel.PropertyRequests(data.d.Items);
						viewModel.PropertyLoading(false);
						viewModel.PropertyTotalCount(data.d.TotalCount);
					}
					else
						$("#propertyLoading").html('There are no property information requests');
				},
				error: function (jqXHR, textStatus, errorThrown) {
					$("#propertyLoading").html('There was an error loading the listing: ' + jqXHR.responseText);
				}
			});
		}, viewModel);

	ko.dependentObservable(function () {
		$.ajax({
			type: "POST",
			url: defaultPageUrl + '/GetAgentRequests',
			data: '{pageSize:"' + viewModel.AgentPageSize() + '",sortField:"' + viewModel.AgentSortField() + '",sortDirection:' + viewModel.AgentSortDirection() + '}',
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) {
				if (data.d.Items.length > 0) {
					viewModel.AgentRequests(data.d.Items);
					viewModel.AgentLoading(false);
					viewModel.AgentTotalCount(data.d.TotalCount);
				}
				else
					$("#agentLoading").html('There are no agent contact requests');
			},
			error: function (jqXHR, textStatus, errorThrown) {
				$("#agentLoading").html('There was an error loading the listing: ' + jqXHR.responseText);
			}
		});
	}, viewModel);

	ko.dependentObservable(function () {
		$.ajax({
			type: "POST",
			url: defaultPageUrl + '/GetProperties',
			data: '{pageSize:"' + viewModel.PropertyListingsPageSize() + '",sortField:"' + viewModel.PropertyListingsSortField() + '",sortDirection:' + viewModel.PropertyListingsSortDirection() + '}',
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) {
				if (data.d.Items.length > 0) {
					viewModel.PropertyListings(data.d.Items);
					viewModel.PropertyListingsLoading(false);
					viewModel.PropertyListingsTotalCount(data.d.TotalCount);
				}
				else
					$("#propertyListingsLoading").html('You do not have any property listings');
			},
			error: function (jqXHR, textStatus, errorThrown) {
				$("#propertyListingsLoading").html('There was an error loading the listing: ' + jqXHR.responseText);
			}
		});
	}, viewModel);

	ko.dependentObservable(function () {
		$.ajax({
			type: "POST",
			url: defaultPageUrl + '/GetEmails',
			data: '{pageSize:"' + viewModel.EmailsToPullBack + '"}',
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) {
				if (data.d.Items.length > 0) {
					viewModel.Emails(data.d.Items);
					viewModel.EmailsLoading(false);
				}
				else
					$("#emailLoading").html('There are no emails in your inbox');
			},
			error: function (jqXHR, textStatus, errorThrown) {
				$("#emailLoading").html('There was an error loading your emails: ' + jqXHR.responseText);
			}
		});
	}, viewModel);

	$("#selectAllProperties").click(function () {
		$("#propertyListing input[type=checkbox]").attr("checked", "checked");
		return false;
	});

	$("#selectNoProperties").click(function () {
		$("#propertyListing input[type=checkbox]").removeAttr("checked");
		return false;
	});

	$("#sendSharedListings").click(function () {
		if (!Page_ClientValidate("SendEmails"))
			return false;
		var sharedListings = [];
		$("#propertyListing input[type=checkbox]:checked").each(function () {
			var id = parseInt($(this).siblings("input[type=hidden]").val());
			for (var i = 0; i < viewModel.PropertyListings().length; i++) {
				if (id == viewModel.PropertyListings()[i].ShowcaseItemID)
					sharedListings.push(viewModel.PropertyListings()[i]);
			}
		});
		for (var i = 0; i < sharedListings.length; i++) {
			try{
				sharedListings[i].DateListed = new Date(parseInt(sharedListings[i].DateListed.substr(6)));
				sharedListings[i].DateListedClientTime = new Date(parseInt(sharedListings[i].DateListedClientTime.substr(6)));
			}
			catch (ex){}
		}
		$.ajax({
			type: "POST",
			url: defaultPageUrl + '/ShareListings',
			data: '{emails:"' + $("[id$=uxShareListingEmails]").val() + '",subject:' + ko.toJSON($("[id$=uxShareListingEmailSubject]").val()) + ',message:' + ko.toJSON($("[id$=uxShareListingPersonalMessage]").val()) + ',listingItems:' + ko.toJSON(sharedListings) + '}',
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) {
				$.fancybox.close();
			}
		});
		return false;
	});
	$("#sendStats").click(function () {
		if (!Page_ClientValidate("SendEmails"))
			return false;
		var showcaseItemID = parseInt($("#statsShowcaseItemID").val());
		$.ajax({
			type: "POST",
			url: defaultPageUrl + '/SendStats',
			data: '{emails:"' + $("[id$=uxShareListingEmails]").val() + '",subject:' + ko.toJSON($("[id$=uxShareListingEmailSubject]").val()) + ',message:' + ko.toJSON($("[id$=uxShareListingPersonalMessage]").val()) + ',showcaseItemID:"' + showcaseItemID + '"}',
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) {
				$.fancybox.close();
			}
		});
		return false;
	});
});

function SetContactStatusToRead(contactID) {
	$.ajax({
		type: "POST",
		url: defaultPageUrl + '/UpdateContactRequest',
		data: '{contactID:"' + contactID + '",statusID:"' + readStatusID + '"}',
		contentType: "application/json; charset=utf-8",
		dataType: "json"
	});
}
function UpdateContactStatus(contactID, statusID) {
	$.ajax({
		type: "POST",
		url: defaultPageUrl + '/UpdateContactRequest',
		data: '{contactID:"' + contactID + '",statusID:"' + $("select[id$=uxContactStatusID]").val() + '"}',
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (data) {
			$.fancybox.close();
		}
	});
}