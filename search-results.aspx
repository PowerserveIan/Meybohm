<%@ Page Title="Search" Language="C#" MasterPageFile="microsite.master" AutoEventWireup="true" CodeFile="search-results.aspx.cs" Inherits="search_results" %>

<%@ Import Namespace="BaseCode" %>
<%@ Import Namespace="Classes.Showcase" %>
<%@ Register TagPrefix="Controls" TagName="ResultSection" Src="~/Controls/Showcase/SearchResultSection.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
    <link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/showcase.css" id="uxCSSFiles" />
	<style>
		ul.breadcrumbs {
			width: 100%;
			margin-bottom: 10px;
		}
		div.box{
			padding-top: 10px;
		}
	</style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentContentDiv">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentSideCol">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="search-results">
		<h3 class="search-info">
			You searched for “<span><%= SearchText %></span>” and here’s what we found.
		</h3>
		<div class="search-bar">
			<input id="uxSearchResultsText" type="text"  placeholder="Search all properties..." />
			<input id="uxSearchResultsButton" type="submit" value="Search"/>
		</div>
		<div class="clear"></div>
		<div class="search-results-items">
            <asp:Literal ID="uxNoResults" runat="server" />
            <Controls:ResultSection ID="uxHomesTop" runat="server" SectionTitle="Homes" />
			<br/>
            <Controls:ResultSection ID="uxLotsLandTop" runat="server" SectionTitle="Lots/Land" />
			<br/>
            <Controls:ResultSection ID="uxHomesBottom" runat="server" SectionTitle="Homes" />
			<br/>
            <Controls:ResultSection ID="uxLotsLandBottom" runat="server" SectionTitle="Lots/Land" />
		</div>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
    <script type="text/javascript">
        $(document).ready(function () {
            $("a.fancybox\\.iframe").fancybox(homeDetailsFancyboxParams);

            function replaceQueryString(url,param,value) {
                var re = new RegExp("([?|&])" + param + "=.*?(&|$)","i");
                if (url.match(re))
                    return url.replace(re,'$1' + param + "=" + value + '$2');
                else
                    return url + '&' + param + "=" + value;
            }

            $("#uxSearchResultsButton").click(function (e) {
                e.preventDefault();
                window.location.href = replaceQueryString(window.location.href, 'q', encodeURIComponent($('#uxSearchResultsText').val()));
            });
            $("#uxSearchResultsText").keypress(function(event){
                if(event.keyCode == 13){
                    $("#uxSearchResultsButton").trigger("click");
                }
            });
        });
        $.fn.placeholder();
    </script>
</asp:Content>

