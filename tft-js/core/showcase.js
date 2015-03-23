var ulWidth = $(".categoryWrapper ul#category").width();
var quicksandDuration = 1000;
var quicksandEasing = 'easeInOutQuad';
var isQuicksandBusy = false;
var changesMadeWhileQuicksandIsBusy = false;
var loaded = false;
var currentFilter;

var filterItem = function(attributeID, type, value) {
    this.AttributeID = attributeID;
    this.Type = type;
    this.Value = ko.observable(value);
    var self = this;
    this.Value.subscribe(function(newValue) {
        viewModel.FiltersChanged(newValue != self.Value());
    });
};

function GetFilterByID(attributeID) {
    for (var i = 0; i < viewModel.FilterList().length; i++) {
        if (viewModel.FilterList()[i].AttributeID == attributeID)
            return viewModel.FilterList()[i];
    }
    return null;
}

var viewModel = {
    FilterList: ko.observableArray([]),
    FiltersChanged: ko.observable(false),
    AddressText: ko.observable($("input[id*=uxFilterAddress]").length > 0 ? $("input[id*=uxFilterAddress]").val() : ""),
    AddressLat: ko.observable(null),
    AddressLong: ko.observable(null),
    MinDistance: ko.observable(null),
    MaxDistance: ko.observable(null),
    AgentID: ko.observable(agentID),
    OpenHouse: ko.observable(GetQueryStringParams("OpenHouse") == "true" ? true : $("select[id*=uxOpenHouse]").length > 0 ? $("select[id*=uxOpenHouse]").val() : null),
    SearchText: ko.observable($("input[id*=uxSearchText]").length > 0 ? $("input[id*=uxSearchText]").val() : ""),
    PageNumber: ko.observable(1),
    PageSize: ko.observable(numberOfItemsToShowPerPage),
    SortDirection: ko.observable(defaultSortDirection),
    SortField: ko.observable(defaultSortField),
    TotalRowCount: ko.observable(0),
    Loading: ko.observable(false),
    Animating: ko.observable(false),
    Listings: ko.observableArray([]),
    OldListings: ko.observableArray([]),
    ReadyToLoadData: ko.observable(false),
    isPagingBusy: ko.observable(false)
};
viewModel.Filter = ko.computed(function() {
    var allFilters = "";
    for (var i = 0; i < viewModel.FilterList().length; i++) {
        if (viewModel.FilterList()[i].Value() != "")
            allFilters += viewModel.FilterList()[i].AttributeID + ":" + (viewModel.FilterList()[i].Type == "CheckBoxList" ? viewModel.FilterList()[i].Value().replace(/\,/g, "|" + viewModel.FilterList()[i].AttributeID + ":") : viewModel.FilterList()[i].Value()) + "|";
    }
    return allFilters.replace(/\|*$/, "");
});
viewModel.CurrentHeight = ko.computed(function() {
    if (viewModel.TotalRowCount() <= 4 || viewModel.PageSize() == 4)
        return 212;
    else if (viewModel.TotalRowCount() <= 9 || viewModel.PageSize() == 9)
        return 135;
    else
        return 97;
});
viewModel.CurrentWidth = ko.computed(function() {
    if (viewModel.TotalRowCount() <= 4 || viewModel.PageSize() == 4)
        return 298;
    else if (viewModel.TotalRowCount() <= 9 || viewModel.PageSize() == 9)
        return 190;
    else
        return 136;
});
viewModel.LIOuterHeight = ko.computed(function() {
    if (viewModel.TotalRowCount() <= 4 || viewModel.PageSize() == 4)
        return 315;
    else if (viewModel.TotalRowCount() <= 9 || viewModel.PageSize() == 9)
        return 238;
    else
        return 209;
});
viewModel.CategoryWrapperHeight = ko.computed(function() {
    if (viewModel.TotalRowCount() <= 20 || viewModel.PageSize() <= 20 || viewModel.LastIndexVisible() == 0)
        return "auto";
    var liHeight = $(".categoryWrapper li").outerHeight(true);
    if (!liHeight)
        liHeight = viewModel.LIOuterHeight();
    var numberRowsVisible = parseInt(Math.ceil((viewModel.LastIndexVisible() - viewModel.FirstIndexVisible()) / 4));
    return numberRowsVisible * liHeight + "px";
});
viewModel.FirstIndexVisible = ko.computed(function() {
    return (viewModel.PageNumber() - 1) * viewModel.PageSize();
});
viewModel.LastIndexVisible = ko.computed(function() {
    return viewModel.PageNumber() * viewModel.PageSize() > viewModel.TotalRowCount() ? viewModel.TotalRowCount() : viewModel.PageNumber() * viewModel.PageSize();
});
viewModel.NextEnabled = ko.computed(function() {
    return (viewModel.LastIndexVisible() > viewModel.TotalRowCount() ? viewModel.TotalRowCount() : viewModel.LastIndexVisible()) != viewModel.TotalRowCount();
});
viewModel.NextClicked = function() {
    if (!viewModel.NextEnabled() || viewModel.Animating() || viewModel.isPagingBusy())
        return false;
    viewModel.isPagingBusy(true);
    viewModel.Animating(true);
    if (window.pageYOffset > $(".controlsTop .next").offset().top)
        window.scrollTo(0, $(".controlsTop .next").offset().top);
    $(".categoryWrapper ul#category").animate({ left: -ulWidth }, 500, function() {
        $(".categoryWrapper ul#category").animate({ left: ulWidth }, 0, function() {
            viewModel.isPagingBusy(false);
            viewModel.PageNumber(viewModel.PageNumber() + 1);
            if (viewModel.OldListings().length < viewModel.TotalRowCount() && (viewModel.LastIndexVisible() > viewModel.OldListings().length)) {
                LoadNextSet();
            } else {
                if (showMap) {
                    clearMarkers();
                    for (var i = 0; i < resultsArray[viewModel.PageNumber() - 1].length; i++)
                        codeAddress(resultsArray[viewModel.PageNumber() - 1][i], i == resultsArray[viewModel.PageNumber() - 1].length - 1);
                }
                var previousPageLastIndexVisible = (viewModel.PageNumber() - 1) * viewModel.PageSize();
                for (var i = previousPageLastIndexVisible - 1; i >= 0; i--) {
                    $(".categoryWrapper ul#category li:eq(" + i + ")").hide();
                }
                for (var i = previousPageLastIndexVisible; i < (viewModel.OldListings().length > previousPageLastIndexVisible + 1 + viewModel.PageSize() ? previousPageLastIndexVisible + 1 + viewModel.PageSize() : viewModel.OldListings().length); i++) {
                    $(".categoryWrapper ul#category li:eq(" + i + ")").show();
                }
                $(".categoryWrapper ul#category").animate({ left: 0 }, 500);
                LazyLoadNextPage();
            }
            viewModel.Animating(false);
        });
    });
    return false;
};
viewModel.PrevEnabled = ko.computed(function() {
    return viewModel.PageNumber() != 1;
});
viewModel.PrevClicked = function() {
    if (!viewModel.PrevEnabled() || viewModel.Animating() || viewModel.isPagingBusy())
        return false;
    viewModel.isPagingBusy(true);
    viewModel.Animating(true);
    if (window.pageYOffset > $(".controlsTop .previous").offset().top)
        window.scrollTo(0, $(".controlsTop .previous").offset().top);
    $(".categoryWrapper ul#category").animate({ left: ulWidth }, 500, function() {
        $(".categoryWrapper ul#category").animate({ left: -ulWidth }, 0, function() {
            viewModel.isPagingBusy(false);
            viewModel.PageNumber(viewModel.PageNumber() - 1);
            if (showMap) {
                clearMarkers();
                for (var i = 0; i < resultsArray[viewModel.PageNumber() - 1].length; i++)
                    codeAddress(resultsArray[viewModel.PageNumber() - 1][i], i == resultsArray[viewModel.PageNumber() - 1].length - 1);
            }
            $(".categoryWrapper ul#category li").slice(viewModel.LastIndexVisible() - viewModel.PageSize(), viewModel.LastIndexVisible()).show();
            $(".categoryWrapper ul#category li").slice(viewModel.LastIndexVisible(), viewModel.OldListings().length).hide();
            $(".categoryWrapper ul#category").animate({ left: 0 }, 500);
            viewModel.Animating(false);
        });
    });
    return false;
};
viewModel.ShowingResultsMessage = ko.computed(function() {
    return 'Showing results ' + (viewModel.FirstIndexVisible() + 1) + '-' + (viewModel.LastIndexVisible() > viewModel.TotalRowCount() ? viewModel.TotalRowCount() : viewModel.LastIndexVisible()) + " of " + viewModel.TotalRowCount();
});
viewModel.WrapperClass = ko.computed(function() {
    if (viewModel.TotalRowCount() <= 4)
        return "display4";
    if (viewModel.TotalRowCount() <= 9)
        return "display9";
    if (viewModel.TotalRowCount() <= 20)
        return "display16";
    if (viewModel.TotalRowCount() <= 25 || viewModel.PageSize() == 25)
        return "display25";
    else if (viewModel.TotalRowCount() <= 50 || viewModel.PageSize() == 50)
        return "display50";
    else
        return "display100";
});
// Watch the filters for changes
viewModel.AddressText.subscribe(function() {
    if (!viewModel.MaxDistance())
        viewModel.MaxDistance(parseInt($("input[id*=uxFilterAddress]").parents(".attributeWrapper").find("a.ui-slider-handle:last").attr("aria-valuetext")));
    SetUserAddress(true, true);
});
viewModel.AddressLat.subscribe(function() {
    viewModel.FiltersChanged(true);
});
viewModel.AddressLong.subscribe(function() {
    viewModel.FiltersChanged(true);
});
viewModel.MinDistance.subscribe(function() {
    if (viewModel.AddressLat() != null && viewModel.AddressLong() != null)
        viewModel.FiltersChanged(true);
});
viewModel.MaxDistance.subscribe(function() {
    if (viewModel.AddressLat() != null && viewModel.AddressLong() != null)
        viewModel.FiltersChanged(true);
});
viewModel.SearchText.subscribe(function(newValue) {
    if (viewModel.Filter() != "")
        ResetAllFilters(newValue);
    else
        viewModel.FiltersChanged(true);
});
viewModel.Filter.subscribe(function() {
    viewModel.FiltersChanged(true);
});
viewModel.OpenHouse.subscribe(function() {
    viewModel.FiltersChanged(true);
});
viewModel.PageSize.subscribe(function() {
    viewModel.FiltersChanged(true);
});
viewModel.SortDirection.subscribe(function() {
    viewModel.FiltersChanged(true);
});
viewModel.SortField.subscribe(function() {
    viewModel.FiltersChanged(true);
});
viewModel.QueryString = ko.computed(function() {
    return ((viewModel.Filter() == '' ? '' :
        'Filters=' + encodeURIComponent(viewModel.Filter())) +
        (viewModel.AddressText() == '' ? '' :
            '&Address=' + encodeURIComponent(viewModel.AddressText()) +
                '&Distance=' + (viewModel.MinDistance() ? viewModel.MinDistance() + encodeURIComponent(':') : "") + viewModel.MaxDistance()) +
        (!viewModel.AgentID() || viewModel.AgentID() == '' ? '' : '&AgentID=' + viewModel.AgentID()) +
        (!viewModel.OpenHouse() ? '' : '&OpenHouse=' + viewModel.OpenHouse()) +
        (viewModel.SearchText() == '' ? '' : '&SearchText=' + encodeURIComponent(viewModel.SearchText())) +
        (viewModel.SortField() == 'ListPrice' ? '' : '&SortField=' + encodeURIComponent(viewModel.SortField())) +
        (!viewModel.SortDirection() ? '' : '&SortDirection=' + viewModel.SortDirection()) +
        (parseInt(viewModel.PageSize()) == 20 ? '' : '&PageSize=' + viewModel.PageSize())).replace(/%20/g, '+');
});
viewModel.LinkToThisPage = ko.computed(function() {
    return CleanupLink(filterLessUrl + (filterLessUrl.indexOf('?') > -1 ? '&' : '?') + viewModel.QueryString());
});
ko.computed(function() {
    if (viewModel.FiltersChanged() && viewModel.ReadyToLoadData()) {
        if (isQuicksandBusy) changesMadeWhileQuicksandIsBusy = true;
        else LoadNewSet();
    }
});

function LoadNewSet() {
    viewModel.isPagingBusy(false);
    viewModel.PageNumber(1);
    LoadNextSet();
}

function ReplaceQuotes(text) {
    return text.replace(/\"/g, "");
}

ko.bindingHandlers.afterRenderWireups = {
    update: function(element, valueAccessor, allBindingsAccessor) {
        ko.utils.unwrapObservable(valueAccessor());
        if (!viewModel.ReadyToLoadData())
            return;
        $(element).find("a.fancybox\\.iframe").fancybox(homeDetailsFancyboxParams);
        if (showMap) {
            $(element).find("li:visible").unbind("mouseenter").mouseenter(function() {
                showInfoWindow($(element).find("li:visible").index($(this)));
            });
        }
    }
};
ko.bindingHandlers.className = {
    init: function(element, valueAccessor) {
        ko.bindingHandlers.className.setClassName(element, valueAccessor);
    },
    update: function(element, valueAccessor) {
        ko.bindingHandlers.className.setClassName(element, valueAccessor);
    },
    setClassName: function(element, valueAccessor) {
        var className = ko.utils.unwrapObservable(valueAccessor());
        var wrap = $(element);
        wrap.removeClass();
        wrap.addClass(className);
    }
};

function GetImageSrc(src) {
    return src.indexOf('http') >= 0 ? src : resizerDomain + 'uploads/images/' + (src == '' ? "missingFile.jpg" : src) + '?width=' + viewModel.CurrentWidth() + '&height=' + viewModel.CurrentHeight() + '&mode=crop&anchor=middlecenter';
}

function LoadNextSet() {
    currentFilter = ko.toJSON([viewModel.PageSize(), viewModel.Filter(), viewModel.AddressLat(), viewModel.AddressLong(), viewModel.MinDistance(), viewModel.MaxDistance(), viewModel.OpenHouse(), viewModel.SearchText(), viewModel.SortField(), viewModel.SortDirection()]);
    viewModel.Loading(true);
    ShowcaseWebMethods.LoadMoreItems(viewModel.PageSize(), viewModel.PageNumber(), viewModel.Filter(), showcaseID, viewModel.AddressLat(), viewModel.AddressLong(), viewModel.MinDistance(), viewModel.MaxDistance(), viewModel.AgentID(), viewModel.OpenHouse(), viewModel.SearchText(), viewModel.SortField(), viewModel.SortDirection(), success_method);

    function success_method(results, userContext, methodName) {

        UpdateAttributes();

        if (results.length == 0) {
            viewModel.TotalRowCount(0);
            if (showMap) {
                if (viewModel.AddressLat() == null && viewModel.AddressLong() == null) $("div#map_canvas").hide();
            }
        } else {
            viewModel.TotalRowCount(results[0].TotalRowCount);
            if (showMap) {
                $("div#map_canvas").show();
            }
        }
        viewModel.Loading(false);
        if (showMap) {
            if (map != null) {
                clearMarkers();
                resultsArray[viewModel.PageNumber() - 1] = results;
                if (results.length == 0)
                    $("div.mapLoading").hide();
            }
        }

        viewModel.Listings(results);

        if (showMap) {
            setTimeout(function() { $("div.mapLoading").hide(); }, 30000);
        }

        if (viewModel.FiltersChanged()) {
            var listingIds = ko.utils.arrayMap(viewModel.Listings(), function(item) { return item.ShowcaseItemID; }).toString();
            var oldListingIds = ko.utils.arrayMap(viewModel.OldListings(), function(item) { return item.ShowcaseItemID; }).toString();
            if (listingIds != oldListingIds) {
                isQuicksandBusy = true;
                $(".categoryWrapper ul#category").quicksand($(".categoryWrapper ul#category2 li"), { duration: viewModel.PageNumber() > 1 ? 0 : quicksandDuration, easing: quicksandEasing }, function() {
                    FinishLoadingItems();
                });
            }
        } else {
            if (viewModel.OldListings().length > 0) {
                var temp = viewModel.Listings();
                viewModel.Listings(viewModel.OldListings());
                for (var i = 0; i < temp.length; i++) {
                    viewModel.Listings.push(temp[i]);
                }
                if (viewModel.Listings().length > 0 || viewModel.OldListings().length > 0) isQuicksandBusy = true;
                $(".categoryWrapper ul#category").quicksand($(".categoryWrapper ul#category2 li"), { duration: viewModel.PageNumber() > 1 ? 0 : quicksandDuration, easing: quicksandEasing }, function() {
                    FinishLoadingItems();
                });
            } else {
                FinishLoadingItems();
            }
        }

        function FinishLoadingItems() {
            isQuicksandBusy = false;
            if (viewModel.PageNumber() > 1)
                $(".categoryWrapper ul#category").animate({ left: 0 }, 500);

            $(".categoryWrapper ul#category").html("");
            viewModel.OldListings(viewModel.Listings());
            if (!changesMadeWhileQuicksandIsBusy) LazyLoadNextPage();

            if (!loaded) {
                loaded = true;
                if (showcaseItemID != null && showcaseItemID != "") {
                    var found = false;
                    for (var i = 0; i < viewModel.OldListings().length; i++) {
                        if (viewModel.OldListings()[i].ShowcaseItemID == showcaseItemID) {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        $("ul#category").html($(".categoryWrapper ul#category").html() + "<a style=\"display: none;\" class=\"fancybox.iframe\" data-id=\"" + showcaseItemID + "\" href=\"showcase-item.aspx?id=" + showcaseItemID + "&\">");
                    $("ul#category a[data-id=" + parseInt(showcaseItemID) + "]").fancybox(homeDetailsFancyboxParams).trigger("click");
                }
            }

            if (changesMadeWhileQuicksandIsBusy) {
                changesMadeWhileQuicksandIsBusy = false;
                LoadNewSet();
            } else viewModel.FiltersChanged(false);
        }

        if (showMap) {
            for (var i = 0; i < results.length; i++) {
                codeAddress(results[i], i == results.length - 1);
            }
        }
    }
}

function LazyLoadNextPage() {
    if (!viewModel.NextEnabled())
        return;
    var thisFilter = ko.toJSON([viewModel.PageSize(), viewModel.Filter(), viewModel.AddressLat(), viewModel.AddressLong(), viewModel.MinDistance(), viewModel.MaxDistance(), viewModel.OpenHouse(), viewModel.SearchText(), viewModel.SortField(), viewModel.SortDirection()]);
    ShowcaseWebMethods.LoadMoreItems(viewModel.PageSize(), viewModel.PageNumber() + 1, viewModel.Filter(), showcaseID, viewModel.AddressLat(), viewModel.AddressLong(), viewModel.MinDistance(), viewModel.MaxDistance(), viewModel.AgentID(), viewModel.OpenHouse(), viewModel.SearchText(), viewModel.SortField(), viewModel.SortDirection(), lazy_success_method);
    running = false;

    function lazy_success_method(results, userContext, methodName) {
        if (thisFilter == currentFilter) {
            if (showMap && map != null)
                resultsArray[viewModel.PageNumber()] = results;
            var tempListings = viewModel.OldListings();
            for (var i = 0; i < results.length; i++) {
                tempListings.push(results[i]);
            }
            viewModel.OldListings(tempListings);
        }
    }
}

var totalHeight;
$(document).ready(function() {
    var isIE6 = ($.browser.msie && parseInt($.browser.version.substr(0, 1)) == 6);
    $("div.AJAXLoading").remove();

    if (collapseFiltersAtStart) {

        function filterLocation() {
            if (window.innerWidth < 1100) {
                $("a.filterShow, div.filterWrapper, div.filterSlideHide").addClass('top');
            } else {
                $("a.filterShow, div.filterWrapper, div.filterSlideHide").removeClass('top');
            }
        }

        filterLocation();
        window.onresize = function() { filterLocation(); };
        var $filters = $("div.filterWrapper");
        $filters.addClass('slideOut');
        totalHeight = parseInt($filters.height()) + parseInt($filters.css("padding-top").replace("px", "")) + parseInt($filters.css("padding-bottom").replace("px", ""));
        if ($filters.hasClass("top"))
            $filters.animate({ top: -totalHeight }, 0);
        else
            $filters.animate({ right: -$filters.width() }, 0);
        $filters.parent().height(totalHeight);
        $("a.filterShow").click(function() {
            if ($(this).hasClass("expanded")) {
                if ($(this).hasClass("top"))
                    $filters.animate({ top: -totalHeight }, 300);
                else
                    $filters.animate({ right: -$filters.width() }, 300);
                $(this).removeClass("expanded");
            } else {
                if ($(this).hasClass("top"))
                    $filters.animate({ top: 0 }, 400);
                else
                    $filters.animate({ right: 0 }, 400);
                $(this).addClass("expanded");
            }
            return false;
        });
    }

    $("a.linkToPage").click(function() {
        $(this).parents(".resultsLink").find("div.linkContainer").toggle(300);
        return false;
    });

    $("a.expand").click(function() {
        $("div.slide").parents("div.filterContainer").toggle(100, function() {
            if (collapseFiltersAtStart) {
                totalHeight = parseInt($filters.height()) + parseInt($filters.css("padding-top").replace("px", "")) + parseInt($filters.css("padding-bottom").replace("px", ""));
                $filters.parent().height(totalHeight);
            }
        });
        var filterButton = $("div.revealSlide .expand");
        filterButton.toggleClass("up");

        return false;
    });

    $("span.filterMinimize a").click(function() {
        var sideFilterButton = $(this).parents("span.filterMinimize");
        sideFilterButton.toggleClass("open");
        $(this).parents("div.filters").find("div.attributeWrapper").toggle("blind");
        $(this).parents("div.filters").find("span.rangeClear").toggleClass("hidden");
        return false;
    });

    $("input[id*=uxFilterAddress], input[id$=uxSearchText]").keydown(function(event) {
        if (event.keyCode == 13)
            $(this).trigger("change").trigger("blur");
        return event.keyCode != 13;
    });

    //Begin Filters	

    function EscapeFilter(filterValue) {
        return filterValue.replace(/\,/g, "[COMMA]").replace(/\:/g, "[COLON]").replace(/\|/g, "[PIPE]").replace(/\</g, "[LESSTHAN]").replace(/\>/g, "[GREATERTHAN]");
    }

    function CleanupFilter(filterValue) {
        return filterValue.replace(/\,*$/, "").replace(/\,\,/, ",").replace(/^\,/, "");
    }

    $("div.filters [id*=uxFilterRadioButtonList]").click(function(e) {
        if ($(this).val() != "") {
            var attributeID = $(this).parents("div.filters").find("[id*=uxAttributeID]");
            var currentFilter = GetFilterByID(attributeID.val());
            if ($(this).val() != "all")
                currentFilter.Value(EscapeFilter($(this).val()));
            else
                currentFilter.Value("");
            e.stopPropagation();
        }
    });

    $("div.filters [id*=uxRadioButtons]").click(function(e) {
        var attributeID = $(this).parents("div.filters").find("[id*=uxAttributeID]");
        var currentFilter = GetFilterByID(attributeID.val());
        var attributeValue = $(this).parents("ul.filterHalf").prev("span.attributeHalf").html();
        if ($(this).parent().is(":last-child"))  //No Preference
            currentFilter.Value(CleanupFilter(currentFilter.Value().replace(EscapeFilter($(this).parents("ul.filterHalf").prev("span.attributeHalf").html() + "[No]"), "")
                .replace(EscapeFilter($(this).parents("ul.filterHalf").prev("span.attributeHalf").html() + "[Yes]"), "")));
        else {
            var yes = $(this).parent().is(":first-child");
            if (currentFilter.Value() == '')
                currentFilter.Value(EscapeFilter(attributeValue + (yes ? "[Yes]" : "[No]")));
            else if (currentFilter.Value().indexOf(EscapeFilter(attributeValue)) != -1) {
                if (yes)
                    currentFilter.Value(currentFilter.Value().replace(EscapeFilter(attributeValue + "[No]"), EscapeFilter(attributeValue + "[Yes]")));
                else
                    currentFilter.Value(currentFilter.Value().replace(EscapeFilter(attributeValue + "[Yes]"), EscapeFilter(attributeValue + "[No]")));
            } else
                currentFilter.Value(CleanupFilter(currentFilter.Value() + "," + EscapeFilter(attributeValue + (yes ? "[Yes]" : "[No]"))));
        }

        e.stopPropagation();
    });

    $("div.filters input[id*=uxFilterCheckBoxList]").click(function() {
        var attributeID = $(this).parents("div.filters").find("[id*=uxAttributeID]");
        var currentFilter = GetFilterByID(attributeID.val());
        if ($(this).is(':checked')) {
            if ($(this).siblings().text() == "All" || $(this).siblings().text() == "N/A") {
                $(this).parents("li").siblings().find(":checkbox:checked").each(function() {
                    $(this).attr('checked', false);
                    currentFilter.Value("");
                });
                $(this).attr('disabled', true);
            } else {
                tempFilters = EscapeFilter($(this).siblings().text()) + ",";
                $(this).parents("li").siblings().find(":checkbox:checked").each(function() {
                    if ($(this).siblings().text() == "All" || $(this).siblings().text() == "N/A") {
                        $(this).attr('checked', false);
                        $(this).removeAttr('disabled');
                    } else
                        tempFilters += EscapeFilter($(this).siblings().text()) + ",";
                });
                currentFilter.Value(CleanupFilter(tempFilters));
            }
        } else
            currentFilter.Value(CleanupFilter(currentFilter.Value().replace(EscapeFilter($(this).siblings().text()), "")));
        if (currentFilter.Value() == "")
            $(this).parents("li").siblings().find("label:contains(All)").siblings("input").attr('checked', true).attr('disabled', true);
    });

    $("div.filters [id*=uxFilterDropDown]").change(function() {
        var attributeID = $(this).parents("div.filters").find("[id*=uxAttributeID]");
        var currentFilter = GetFilterByID(attributeID.val());
        if ($(this).val() == "all")
            currentFilter.Value("");
        else
            currentFilter.Value(EscapeFilter($(this).val()));
    });

    $("div.filters [id*=uxFilterListBox]").change(function() {
        var attributeID = $(this).parents("div.filters").find("[id*=uxAttributeID]");
        var currentFilter = GetFilterByID(attributeID.val());
        var all = false;
        var listBoxValue = "";
        for (var i = 0; i < $(this).val().length; i++) {
            if ($(this).val()[i] == "all") {
                all = true;
                break;
            }
            listBoxValue += EscapeFilter($(this).val()[i]) + ",";
        }
        if (all)
            currentFilter.Value("");
        else
            currentFilter.Value(listBoxValue.replace(/\,*$/, ""));
    });

    $(".numbersOnly").blur(function() {
        var attributeID = $(this).parents("div.filters").find("[id*=uxAttributeID]");
        var currentFilter = GetFilterByID(attributeID.val());
        var min = $(this).parent().find("[id$=uxMinimumRange]").val().replace(/,/g, '');
        var max = $(this).parent().find("[id$=uxMaximumRange]").val().replace(/,/g, '');
        currentFilter.Value((min == '' ? "" : ">" + min) + (max == '' ? "" : "<" + max));
    });

    $(".rangeClear").click(function() {
        var rangeSlider = $(this).parents(".filters").find("div[id^=rangeSliderDiv]");
        if (rangeSlider.length > 0) {
            var min = rangeSlider.find("select:first option:first").val();
            var max = rangeSlider.find("select:last option:last").val();
            rangeSlider.find("div.ui-slider").slider("values", 0, 0);
            rangeSlider.find("div.ui-slider").slider("values", 1, max);
            rangeSlider.find("div.ui-slider a.ui-slider-handle:first").attr("aria-valuetext", min).attr("aria-valuenow", "0");
            rangeSlider.find("div.ui-slider a.ui-slider-handle:last").attr("aria-valuetext", max).attr("aria-valuenow", rangeSlider.find("div.ui-slider a.ui-slider-handle:last").attr("aria-valuemax"));
            $(this).parents("div.filters").find(".rangeAmount").html("Current Range: " + min + ' - ' + max);
        }
        var attributeID = $(this).parents("div.filters").find("[id*=uxAttributeID]");
        var currentFilter = GetFilterByID(attributeID.val());
        currentFilter.Value("");
    });

    $(".resetAllFilters").click(function() {
        ResetAllFilters("");
        return false;
    });

    $(".numbersOnly").keydown(function(e) {
        var key = e.charCode || e.keyCode || 0;
        return key == 8 || key == 9 || key == 46 || (key >= 37 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == 110 || key == 188 || key == 190;
    });
    $(".numbersOnly").keyup(function(e) {
        var key = e.charCode || e.keyCode || 0;
        if ($(this).val() != '' && (key == 8 || key == 9 || key == 46 || (key >= 37 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == 110 || key == 188 || key == 190)) {
            var price = parseFloat($(this).val().replace(/,/g, '')).toString();
            //add commas
            for (var i = 0; i < Math.floor((price.length - (1 + i)) / 3); i++)
                price = price.substring(0, price.length - (4 * i + 3)) + ',' + price.substring(price.length - (4 * i + 3));
            price = price.replace(',.', '.');
            if (price == 'NaN')
                price = '';
            $(this).val(price);
        } else if (key == 13)
            $(this).blur();
    });
});

function ResetAllFilters(newSearchText) {
    viewModel.ReadyToLoadData(false);
    for (var i = 0; i < viewModel.FilterList().length; i++) {
        viewModel.FilterList()[i].Value("");
    }
    $("div[id^=rangeSliderDiv]").each(function() {
        var rangeSlider = $(this).parents(".filters").find("div[id^=rangeSliderDiv]");
        var min = rangeSlider.find("select:first option:first").val();
        var max = rangeSlider.find("select:last option:last").val();
        rangeSlider.find("div.ui-slider").slider("values", 0, 0);
        rangeSlider.find("div.ui-slider").slider("values", 1, max);
        rangeSlider.find("div.ui-slider a.ui-slider-handle:first").attr("aria-valuetext", min).attr("aria-valuenow", "0");
        rangeSlider.find("div.ui-slider a.ui-slider-handle:last").attr("aria-valuetext", max).attr("aria-valuenow", rangeSlider.find("div.ui-slider a.ui-slider-handle:last").attr("aria-valuemax"));
        $(this).parents("div.filters").find(".rangeAmount").html("Current Range: " + min + ' - ' + max);
    });
    $("select[id$=uxFilterSlider]").each(function() {
        var slider = $(this).parents(".filters").find("div[id^=sliderDiv]");
        var min = $(this).find("select:first option:first").val();
        slider.find("div.ui-slider").slider("values", 0, 0);
        slider.find("div.ui-slider a.ui-slider-handle:first").attr("aria-valuetext", min);
        $(this).val($(this).find("option:first").val());
    });
    $("div.filters [id*=uxFilterListBox]").each(function() {
        $(this).val("all");
    });
    $("div.filters [id*=uxFilterDropDown]").each(function() {
        $(this).val("all");
    });
    $("div.filters [id*=uxFilterRadioButtonList]").each(function() {
        if ($(this).val() == "all")
            $(this).attr('checked', true);
        else
            $(this).attr('checked', false);
    });

    $("div.filters [id*=uxRadioButtons]").each(function() {
        if ($(this).parent().is(":last-child"))
            $(this).attr('checked', true);
        else
            $(this).attr('checked', false);
    });

    $("div.filters :checkbox").each(function() {
        if ($(this).siblings().text() == "All") {
            $(this).attr('checked', true);
            $(this).attr('disabled', true);
        } else
            $(this).attr('checked', false);
    });

    $(".numbersOnly").each(function() {
        $(this).val("");
    });

    viewModel.AddressText("");
    viewModel.AddressLat(null);
    viewModel.AddressLong(null);
    viewModel.MinDistance(null);
    viewModel.MaxDistance(null);
    viewModel.SearchText(newSearchText);
    viewModel.ReadyToLoadData(true);
    if (newSearchText != "")
        viewModel.FiltersChanged(true);
}

function CleanupLink(link) {
    return link.replace("?&", "?").replace(/\?*$/, "").replace(/\&$/, "").replace(/\&&/, "&").replace(/%20/g, '+');
}

function sliderValueChanged(event, ui) {
    var sliderElement = $(ui.handle);
    var sliderValue;
    if (sliderElement.length == 0) {
        sliderElement = $(ui).find("select");
        sliderValue = sliderElement.val();
    } else
        sliderValue = sliderElement.attr("aria-valuetext");
    if (sliderElement.parents("div.attributeWrapper").find("input[id*=uxFilterAddress]").length > 0) {
        viewModel.MaxDistance(parseInt(sliderValue));
        return;
    }
    var attributeID = sliderElement.parents("div.filters").find("[id*=uxAttributeID]");
    var currentFilter = GetFilterByID(attributeID.val());
    if (sliderValue == '0')
        currentFilter.Value("");
    else
        currentFilter.Value(sliderValue);
}

function rangeSliderUpdateText(event, ui) {
    var firstHandle = $(ui.handle).prev().length > 0 ? $(ui.handle).prev() : $(ui.handle);
    $(ui.handle).parents("div.filters").find(".rangeAmount").html("Current Range: " + firstHandle.attr("aria-valuetext") + ' - ' + firstHandle.next().attr("aria-valuetext"));
}

function rangeSliderUpdateFilter(ui) {
    var sliderElement = $(ui.handle);
    var sliderValue1;
    var sliderValue2;
    if (sliderElement.length == 0) {
        sliderElement = $(ui).find("select:first");
        sliderValue1 = sliderElement.val();
        sliderValue2 = sliderElement.next("select").val();
    } else {
        sliderValue1 = sliderElement.prev().length > 0 ? sliderElement.prev().attr("aria-valuetext") : sliderElement.attr("aria-valuetext");
        sliderValue2 = sliderElement.prev().length > 0 ? sliderElement.attr("aria-valuetext") : sliderElement.next().attr("aria-valuetext");
    }

    if (sliderElement.parents(".attributeWrapper").find("input[id*=uxFilterAddress]").length > 0) {
        viewModel.ReadyToLoadData(false);
        viewModel.MinDistance(parseInt(sliderValue1));
        viewModel.MaxDistance(parseInt(sliderValue2));
        viewModel.ReadyToLoadData(true);
        return;
    }
    var attributeID = sliderElement.parents("div.filters").find("[id*=uxAttributeID]");
    var currentFilter = GetFilterByID(attributeID.val());
    currentFilter.Value((sliderValue1 == sliderElement.parent().prev("select").find("option:first").val() ? "" : ">" + sliderValue1) +
        (sliderValue2 == sliderElement.parent().prev("select").find("option:last").val() ? "" : "<" + sliderValue2));
}

function rangeSliderValueChanged(event, ui) {
    rangeSliderUpdateText(event, ui);
    rangeSliderUpdateFilter(ui);
}


function UpdateAttributes() {
    var thisFilter = ko.toJSON([viewModel.PageSize(), viewModel.Filter(), viewModel.AddressLat(), viewModel.AddressLong(), viewModel.MinDistance(), viewModel.MaxDistance(), viewModel.OpenHouse(), viewModel.SearchText(), viewModel.SortField(), viewModel.SortDirection()]);
    ShowcaseWebMethods.GetUpdatedAttributes(viewModel.Filter(), showcaseID, viewModel.AddressLat(), viewModel.AddressLong(), viewModel.MinDistance(), viewModel.MaxDistance(), viewModel.AgentID(), viewModel.OpenHouse(), viewModel.SearchText(), viewModel.SortField(), viewModel.SortDirection(), UpdateAttributes_success_method, FailedCallback);
    running = false;

    function FailedCallback(error) {
        alert(error);
    }
    function UpdateAttributes_success_method(results, userContext, methodName) {
        var lastId = 0;
        var currentElement = null;
        var selectedElement = null;
        var currentIsSelect = false;
        var currentIsCheckList = false;
        var currentIsRadioList = false;
        var currentIsRadioGrid = false;

        //reset everything
        for (var i = 0; i < HideList.length; i++) {
            if (HideList[i][1] == filterDropDown || HideList[i][1] == filterList) {
                currentElement = $('input[value="' + HideList[i][0] + '"]').siblings('select');
                $(currentElement).find('option').not(':first').not(':selected').remove();
            }
            else if (HideList[i][1] == filterCheckList) {
                currentElement = $('input[value="' + HideList[i][0] + '"]').siblings('ul');
                $(currentElement).find('li').not(':first').each(function () {
                    if (!$(this).find('input:checkbox').is(':checked'))
                        $(this).hide();
                });
            }
            else if (HideList[i][1] == filterRadioList) {
                currentElement = $('input[value="' + HideList[i][0] + '"]').siblings('ul');
                $(currentElement).find('li').not(':first').each(function () {
                    if (!$(this).find('input:radio').is(':checked'))
                        $(this).hide();
                });
            }
        }

        currentElement = null;

        for (var i = 0; i < results.length; i++) {
            if (lastId != results[i].ShowcaseAttributeID) {
                currentIsSelect = currentIsCheckList = currentIsRadioList = currentIsRadioGrid = false;

                if (DisplayList[results[i].ShowcaseAttributeID] == filterDropDown || DisplayList[results[i].ShowcaseAttributeID] == filterList) {
                    currentIsSelect = true;
                    currentElement = $('input[value="' + results[i].ShowcaseAttributeID + '"]').siblings('select');
                } else if (DisplayList[results[i].ShowcaseAttributeID] == filterCheckList || DisplayList[results[i].ShowcaseAttributeID] == filterRadioList) {
                    currentElement = $('input[value="' + results[i].ShowcaseAttributeID + '"]').siblings('ul');
                    if ($(currentElement).find('input:radio') && $(currentElement).find('input:radio')[0])
                        currentIsRadioList = true;
                    else
                        currentIsCheckList = true;
                }

                if (currentIsSelect && selectedElement != null) {
                    $(currentElement).val($(selectedElement).val());
                    selectedElement = null;
                }

                lastId = results[i].ShowcaseAttributeID;

                if (currentIsSelect) selectedElement = $(currentElement).find('option:selected');
            }

            if (currentIsSelect) {
                if ($(selectedElement).val() == 'all')
                    $(currentElement).append($('<option></option>').val(results[i].Value).html(results[i].Value));
                else if ($(selectedElement).val() == results[i].Value) { }
                else if ($(selectedElement).val() > results[i].Value)
                    $('<option></option>').val(results[i].Value).html(results[i].Value).insertBefore($(selectedElement));
                else if ($(selectedElement).val() < results[i].Value)
                    $(currentElement).append($('<option></option>').val(results[i].Value).html(results[i].Value));

                if ((((i + 1) == results.length) || (lastId != results[i + 1].ShowcaseAttributeID)) && selectedElement != null) {
                    $(currentElement).val($(selectedElement).val());
                    selectedElement = null;
                }
            } else if (currentIsCheckList) {
                $(currentElement).find('li').find('input:checkbox[value="' + results[i].Value + '"]').each(function () {
                    $(this).parent().parent('li').show();
                });
            } else if (currentIsRadioList) {
                $(currentElement).find('li').find('input:radio[value="' + results[i].Value + '"]').each(function () {
                    $(this).parent('li').show();
                });
            }
        }
    }
}

function GetQueryStringParams(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}
