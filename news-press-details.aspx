<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="news-press-details.aspx.cs" Inherits="NewsPressDetail" ViewStateMode="Disabled" %>

<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/newsShared.css,~/css/newsPress.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<span class="button back hide"><asp:HyperLink ID="uxBackButtonTop" runat="server" Text="Back to Listings"></asp:HyperLink></span>
	<div class="newsPost detailContainer newsPress">
		<asp:PlaceHolder runat="server" ID="uxItemFoundPH">
			<h3>
				<asp:Literal runat="server" ID="uxTitle"></asp:Literal></h3>
			<div class="byLine">
				<asp:Label ID="uxDate" runat="server" CssClass="articleDate" />
				<asp:Label ID="uxAuthor" runat="server" CssClass="articleAuthor" /></div>
			<div class="articleBody">
				<asp:Literal runat="server" ID="uxStoryHTML"></asp:Literal>
			</div>
		</asp:PlaceHolder>
		<asp:Label runat="server" ID="uxNoItemFoundMsg" Visible="false" Text="An Error has occured: News Press item not found.<br />" />
	</div>
	<span class="button back"><asp:HyperLink ID="uxBackButton" runat="server" Text="Back to Listings"></asp:HyperLink></span>
	<asp:PlaceHolder runat="server" ID="uxNextPreviousPH">
		<div class="newsPressPopupWrapper">
			<div id="upPreviousWrapper" style="margin-left: 400px;" runat="server">
				<div id="upPrevious">
					<div class="wrapper">
						<h2>Previous Article</h2>
						<h6>
							<asp:Literal runat="server" ID="uxPreviousCategoryTitle"></asp:Literal>
							<span class="num">
								<asp:Literal runat="server" ID="uxPreviousNumberArticles"></asp:Literal></span></h6>
						<h3>
							<asp:HyperLink runat="server" ID="uxPreviousLink"></asp:HyperLink></h3>
						<p class="refer">
							<asp:HyperLink runat="server" ID="uxPreviousReadMoreLink" CssClass="more">Read More &raquo;</asp:HyperLink></p>
						<a href="#" id="previousClose" title="Close">x</a></div>
				</div>
			</div>
			<div id="upNextWrapper" style="margin-left: 400px;" runat="server">
				<div id="upNext">
					<div class="wrapper">
						<h2>Next Article</h2>
						<h6>
							<asp:Literal runat="server" ID="uxNextCategoryTitle"></asp:Literal>
							<span class="num">
								<asp:Literal runat="server" ID="uxNextNumberArticles"></asp:Literal></span></h6>
						<h3>
							<asp:HyperLink runat="server" ID="uxNextLink"></asp:HyperLink></h3>
						<p class="refer">
							<asp:HyperLink runat="server" ID="uxNextReadMoreLink" CssClass="more">Read More &raquo;</asp:HyperLink></p>
						<a href="#" id="nextClose" title="Close">x</a></div>
				</div>
			</div>
		</div>
	</asp:PlaceHolder>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		// <![CDATA[
		$(document).ready(function () {
			var previousHidden = false;
			var nextHidden = false;
			var popupsVisible = isScrolledIntoView($("#<%=uxBackButton.ClientID %>"));

			if (popupsVisible)
				toggleOtherArticles();

			$(window).scroll(function () {
				if (!(nextHidden && previousHidden)) {
					if (popupsVisible != isScrolledIntoView($("#<%=uxBackButton.ClientID %>"))) {
						popupsVisible = !popupsVisible;
						toggleOtherArticles();
					}
				}
			});

			function isScrolledIntoView(elem) {
				var docViewTop = $(window).scrollTop();
				var docViewBottom = docViewTop + $(window).height();
				var elemTop = $(elem).offset().top;

				return elemTop <= docViewBottom;
			}

			function toggleOtherArticles() {
				if (!previousHidden)
					$("#<%=upPreviousWrapper.ClientID %>").animate(
							{ marginLeft: parseInt($("#<%=upPreviousWrapper.ClientID %>").css('marginLeft'), 10) == 0 ? $("#<%=upPreviousWrapper.ClientID %>").outerWidth() + 10 : 0 });
				if (!nextHidden)
					$("#<%=upNextWrapper.ClientID %>").animate(
							{ marginLeft: parseInt($("#<%=upNextWrapper.ClientID %>").css('marginLeft'), 10) == 0 ? $("#<%=upNextWrapper.ClientID %>").outerWidth() + 10 : 0 });
			}


			$("a#previousClose").click(function () {
				previousHidden = true;
				$("#<%=upPreviousWrapper.ClientID %>").animate(
					{ marginLeft: parseInt($("#<%=upPreviousWrapper.ClientID %>").css('marginLeft'), 10) == 0 ? $("#<%=upPreviousWrapper.ClientID %>").outerWidth() + 10 : 0 }).hide();
				return false;
			});

			$("a#nextClose").click(function () {
				nextHidden = true;
				$("#<%=upNextWrapper.ClientID %>").animate(
					{ marginLeft: parseInt($("#<%=upNextWrapper.ClientID %>").css('marginLeft'), 10) == 0 ? $("#<%=upNextWrapper.ClientID %>").outerWidth() + 10 : 0 }).hide();
				return false;
			});
		});
		// ]]>
	</script>
</asp:Content>
