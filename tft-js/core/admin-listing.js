var listing = function (index, item) {
	$.extend(this, item);
	var itemTypeSplit = item.__type.split('.');
	if (item.__type && itemTypeSplit.length > 0)
		this.Id = item[itemTypeSplit[itemTypeSplit.length - 1] + "ID"];
	this.oldDisplayOrder = ko.observable((item.DisplayOrder != null ? item.DisplayOrder : null));
	this.Active = ko.observable((item.Active != null ? item.Active : null));
	this.Featured = ko.observable((item.Featured != null ? item.Featured : null));
	this.index = ko.observable(index);
	var self = this;
	appendToListing(index, item, this);	
	this.activeText = ko.dependentObservable({
		read: function () {
			return self.Active() ? "Yes" : "No";
		}
	});
	this.toggleActive = function () {
		$.ajax({
			type: "POST",
			url: defaultPageUrl + '/ToggleActive',
			data: '{id:' + this.Id + '}',
			contentType: "application/json; charset=utf-8"
		});
		this.Active(!this.Active());
		return false;
	};
	this.featuredText = ko.dependentObservable({
		read: function () {
			return self.Featured() ? "Yes" : "No";
		}
	});
	this.toggleFeatured = function () {
		$.ajax({
			type: "POST",
			url: defaultPageUrl + '/ToggleFeatured',
			data: '{id:' + this.Id + '}',
			contentType: "application/json; charset=utf-8"
		});
		this.Featured(!this.Featured());
		return false;
	};
	this.deleteRecord = function () {
		$.ajax({
			type: "POST",
			url: defaultPageUrl + '/DeleteRecord',
			data: '{id:"' + this.Id + '"}',
			contentType: "application/json; charset=utf-8"
		});
		listingModel.listings.remove(this);
		listingModel.totalCount(listingModel.totalCount() - 1);
		return false;
	};
	this.displayOrderInvalid = ko.observable(false);
	this.displayOrder = ko.dependentObservable({
		read: this.oldDisplayOrder,
		write: function (value) {
			self.displayOrderInvalid(isNaN(value) || value % 1 != 0 || value < 1);
			if (!self.displayOrderInvalid()) {
				value = parseInt(value);
				if (self.oldDisplayOrder() < value) {
					for (var i = 0; i < listingModel.listings().length; i++) {
						var currentDisplayOrder = parseInt(listingModel.listings()[i].displayOrder());
						if (currentDisplayOrder <= value && currentDisplayOrder > self.oldDisplayOrder() && listingModel.listings()[i].Id != self.Id)
							listingModel.listings()[i].oldDisplayOrder(currentDisplayOrder - 1);
					}
				}
				else if (self.oldDisplayOrder() > value) {
					for (var i = 0; i < listingModel.listings().length; i++) {
						var currentDisplayOrder = parseInt(listingModel.listings()[i].displayOrder());
						if (currentDisplayOrder >= value && currentDisplayOrder < self.oldDisplayOrder() && listingModel.listings()[i].Id != self.Id)
							listingModel.listings()[i].oldDisplayOrder(currentDisplayOrder + 1);
					}
				}
			}
			self.oldDisplayOrder(value);
			if (!self.displayOrderInvalid()) {
				listingModel.listings.sort(function (a, b) {
					return (a.displayOrder() < b.displayOrder() ? -1 : (a.displayOrder() > b.displayOrder() ? 1 : 0));
				});
				// This technique of adding an index to an observable array should not be needed anymore b/c knockout 2.1.0 gives access to $index in the foreach binding.
				// Keeping it in here for now.
				for (var i = 0; i < listingModel.listings().length; i++) {
					listingModel.listings()[i].index(i + 1);
				}
			}
		}
	});
}
var appendToListing = function (index, item, thisListing) {
}

var page = function (number) {
	this.number = ko.observable(number);
	this.changePage = function () {
		if (this.number != listingModel.pageNumber())
			listingModel.pageNumber(this.number());
	}
}

var pageFilter = {};	

var listingModel = {
	listings: ko.observableArray([]),
	pages: ko.observableArray([]),
	pageNumber: ko.observable(defaultPageNumber),
	pageSize: ko.observable(defaultPageSize),
	searchText: ko.observable(defaultSearchText),	
	sortField: ko.observable(defaultSortField),
	sortDirection: ko.observable(defaultSortDirection),
	filter: ko.observable({}),
	totalCount: ko.observable(0),
	displayOrderEditable: ko.observable(false),
	readyToReloadListing: ko.observable(true),
	isLoading: ko.observable(false)
}
listingModel.additionalDisplayOrderFilter = ko.dependentObservable(function () {
	return "";
}, listingModel);
listingModel.displayOrderEditableChanged = ko.dependentObservable({
	read: listingModel.displayOrderEditable,
	write: function (value) {
		listingModel.readyToReloadListing(!(listingModel.numberPages() > 1 || listingModel.searchText() != ""));
		listingModel.displayOrderEditable(value);
		if (!listingModel.hideDisplayOrder() && listingModel.displayOrderEditable()) {
			if (listingModel.numberPages() > 1 || listingModel.searchText() != "")
				listingModel.pageSize(99999999);
			listingModel.searchText("");
			listingModel.readyToReloadListing(true);
			$(".listing tbody").sortable({
				start: function (event, ui) {
					var start_pos = ui.item.index();
					ui.item.data('start_pos', start_pos);
					$(".listing tbody tr:eq(" + start_pos + ") input[type=text]").blur();
				},
				change: function (event, ui) {
					for (var i = 0; i < listingModel.listings().length; i++) {
						if (listingModel.listings()[i].Id == ui.item.find("td:last input[type=hidden]").val()) {
							listingModel.listings()[i].oldDisplayOrder(ui.placeholder.index() + (ui.item.data('start_pos') < ui.placeholder.index() ? 0 : 1));
						}
					}
				},
				update: function (event, ui) {
					ui.item.parents("table").find("tr").each(function () {
						var itemPosition = $(this).parents("tbody").find("tr").index($(this)) + 1;
						for (var i = 0; i < listingModel.listings().length; i++) {
							if (listingModel.listings()[i].Id == $(this).find("td:last input[type=hidden]").val()) {
								listingModel.listings()[i].oldDisplayOrder(itemPosition);
								listingModel.listings()[i].index(itemPosition);
							}
						}
					});
				}
			});
		}
		else
			$(".listing tbody").sortable("destroy");
	}
});
listingModel.hideDisplayOrder = ko.dependentObservable(function () {
	return false;
}, listingModel);
listingModel.numberPages = ko.dependentObservable(function () {
	return Math.ceil(this.totalCount() / this.pageSize());
}, listingModel);
listingModel.pageSize.subscribe(function (newValue) {
	if (listingModel.pageNumber() > 1) {
		listingModel.readyToReloadListing(false);
		listingModel.pageNumber(1);
		listingModel.readyToReloadListing(true);
	}
});
listingModel.returnString = ko.dependentObservable(function () {
	var filterString = "";
	for (var propName in pageFilter) {
		if (pageFilter[propName]() != null && pageFilter[propName]() != '')
			filterString += "&" + propName + "=" + pageFilter[propName]();
	}
	return (this.pageNumber() > 1 ? "&Page=" + this.pageNumber() : "") + (this.pageSize() != defaultPageSize ? "&PageSize=" + this.pageSize() : "") + (this.searchText() != '' ? "&SearchText=" + this.searchText() : "") + (this.sortField() != defaultBaseSortField ? "&SortField=" + this.sortField() : "") + (this.sortDirection() != defaultSortDirection ? "&SortDirection=" + this.sortDirection() : "") + filterString;
}, listingModel);
listingModel.saveDisplayOrders = function () {
	var allValid = true;
	var displayOrders = {};
	for (var i = 0; i < listingModel.listings().length; i++) {
		if (listingModel.listings()[i].displayOrderInvalid())
			allValid = false;
		displayOrders[listingModel.listings()[i].Id] = listingModel.listings()[i].displayOrder();
	}
	if (allValid) {
		$.ajax({
			type: "POST",
			url: defaultPageUrl + '/UpdateDisplayOrder',
			data: '{"displayOrders": ' + JSON.stringify(displayOrders) + listingModel.additionalDisplayOrderFilter() + '}',
			contentType: "application/json; charset=utf-8"
		});
		listingModel.displayOrderEditable(false);
	}
	else
		alert("You have invalid display orders");
};
listingModel.showData = ko.dependentObservable(function () {
	return listingModel.listings().length > 0 && !listingModel.isLoading();
});
listingModel.throttledSearchText = ko.computed(listingModel.searchText).extend({ throttle: 300 });
listingModel.throttledSearchText.subscribe(function (newValue) {
	listingModel.pageNumber(1);
});
listingModel.setSort = function (field) {
	if (this.sortField() == field)
		this.sortDirection(!this.sortDirection());
	this.sortField(field);
};
var afterLoad = function (data) {
}
function UseFilters() {
	var newPageFilter = {};
	for (var propName in pageFilter) {
		if (pageFilter[propName]() != null && pageFilter[propName]() != '')
			newPageFilter[propName] = ko.observable(pageFilter[propName]() == 'NULL' ? "" : pageFilter[propName]());
	}
	// If a filter is being used, then set the filtered listing to the first page
	listingModel.pageNumber(1);
	listingModel.filter(newPageFilter);
}
function FormatDate(dateJSON, format) {
	var date = new Date(parseInt(dateJSON.substr(6)));
	return $.format.date(date, format)
}
ko.bindingHandlers.sorting = {
	init: function (element, valueAccessor) {
		var thisModel = ko.dataFor(element);
		thisModel.sortField.subscribe(function (newValue) {
			HandleSorting(newValue, thisModel.sortDirection());
		});
		thisModel.sortDirection.subscribe(function (newValue) {
			HandleSorting(thisModel.sortField(), newValue);
		});

		function HandleSorting(sortField, sortDirection) {
			$(element).toggleClass("descending", sortField == valueAccessor() && !sortDirection)
					  .toggleClass("ascending", sortField == valueAccessor() && sortDirection);
		}
		HandleSorting(thisModel.sortField(), thisModel.sortDirection());

		$(element).click(function () {
			thisModel.setSort(valueAccessor());
			return false;
		});
	}
};
$(function () {
	var next = $("table.listing").next("h4");
	$("table.listing").wrap('<div class="wrap"></div>');
	var curr = next;
	next = next.next(".pagination");
	$(".wrap").append(curr);
	$(".wrap").append(next);
	$(".wrap").append($(".AJAXLoading"));
	$("table.listing").after('<div class="loadMessage" data-bind="visible: listingModel.isLoading()"><span>Loading...</span></div>');
	ko.applyBindings(listingModel);

	ko.dependentObservable(function () {
		if (listingModel.readyToReloadListing()) {
			listingModel.isLoading(true);
			$.ajax({
				type: "POST",
				url: defaultPageUrl + '/PageListing',
				data: '{pageNumber:' + listingModel.pageNumber() + ',pageSize:' + listingModel.pageSize() + ',searchText:"' + listingModel.throttledSearchText() + '",sortField:"' + listingModel.sortField() + '",sortDirection:' + listingModel.sortDirection() + ', filterList:' + ko.toJSON(listingModel.filter()) + '}',
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (data) {
					var index = 0;
					var mappedListings = $.map(data.d.Items, function (item) {
						index++;
						return new listing(index, item)
					});
					listingModel.totalCount(data.d.TotalCount);
					if (listingModel.totalCount() == 0 && listingModel.pageNumber() > 1)
						listingModel.pageNumber(1);
					listingModel.listings(mappedListings);
					listingModel.pages.removeAll();
					var numberPages = listingModel.numberPages();
					for (var i = 1; i <= numberPages && i <= 500; i++) {
						listingModel.pages.push(new page(i));
					}
					afterLoad(data);
					listingModel.isLoading(false);
					if (columnNumberToMakeLink != 0)
						$("table.listing tbody tr").each(function () {
							var theTD = $(this).find("td:visible:eq(" + (columnNumberToMakeLink - 1) + ")");
							var duplicateEdit = $(this).find("td:last a.edit");
							if (duplicateEdit.length == 0)
								duplicateEdit = $(this).find("td:last a.view");
							if (duplicateEdit.length == 0)
								duplicateEdit = $(this).find("a.edit");
							if (duplicateEdit.length != 0)
								theTD.html('<a title="Edit this item" href="' + duplicateEdit.attr("href") + '">' + theTD.html() + '</a>');
						});
					if ($("span.pagination").length > 0)
						$("span.pagination").pagination();
				},
				error: function (jqXHR, textStatus, errorThrown) {
					$(".listing").next("h4").html('There was an error loading the listing: ' + jqXHR.responseText);
					listingModel.isLoading(false);
				}
			});
		}
	}, listingModel);

	$('div.filters a.toggle').click(function () {
		if ($(this).hasClass('up')) {
			$(this).html('show filtering options').removeClass('up').addClass('down');
			$(this).siblings('div.toggleArea').slideUp();
		} else {
			$(this).html('hide filtering options').removeClass('down').addClass('up');
			$(this).siblings('div.toggleArea').slideDown();
		}
		return false;
	});
	$('div.toggleArea a.hide').click(function () {
		$(this).parents().find('div.toggleArea').slideUp();
		$('div.filters a.toggle.up').html('show filtering options').removeClass('up').addClass('down');
		return false;
	});
	$("input[id$=uxSearchText]").keydown(function (e) {
		var key = e.charCode || e.keyCode || 0;
		return key != 13;
	});
});