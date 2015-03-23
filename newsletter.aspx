<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="newsletter.aspx.cs" Inherits="NewsletterPage" Title="Newsletter" ViewStateMode="Disabled" %>
<%@ Reference VirtualPath="~/microsite.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/newsShared.css,~/css/newsletter.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<div class="newsPost listingContainer newsletter">
		<div class="headerBG"><h1>Newsletters</h1></div>
		<asp:HyperLink ID="uxSubscribeLink" runat="server" CssClass="button fancybox.iframe floatRight subscribe" NavigateUrl="newsletter-subscribe-popup.aspx">Subscribe</asp:HyperLink>
		<asp:DataPager ID="uxTopPager" runat="server" PagedControlID="uxNewsListView" QueryStringField="Page" class="pagination">
			<Fields>
				<asp:NextPreviousPagerField PreviousPageText="Previous" ShowNextPageButton="false" ButtonCssClass="prev" />
				<asp352:NumericPagerFieldCustom ButtonCount="99999999" />
				<asp:NextPreviousPagerField PreviousPageText="Next" ShowPreviousPageButton="false" ButtonCssClass="next" />
			</Fields>
		</asp:DataPager>
		<asp:ListView ID="uxNewsListView" runat="server" ItemPlaceholderID="uxItemPlaceHolder" ItemType="Classes.Newsletters.Newsletter">
			<LayoutTemplate>
				<ul class="storyList">
					<asp:PlaceHolder ID="uxItemPlaceHolder" runat="server"></asp:PlaceHolder>
				</ul>
			</LayoutTemplate>
			<ItemTemplate>
				<li>
					<div class="top">
						<div class="bottom">
							<h3>
								<a class="fancybox.iframe detailsLink" href="newsletter-details.aspx?id=<%#Item.NewsletterID%>&amp;title=<%#Server.UrlEncode(Item.Title)%><%#String.IsNullOrEmpty(Request.QueryString.ToString().Replace("Id=" + Request.QueryString["Id"], "").Replace("id=" + Request.QueryString["Id"], "")) ? "" : "&" + Request.QueryString.ToString().Replace("Id=" + Request.QueryString["Id"], "").Replace("id=" + Request.QueryString["Id"], "")%>">
									<%#:Item.Issue%>:
									<%#:Item.Title%></a></h3>
							<span class="byLine articleDate">
								<%#Item.DisplayDateClientTime.ToShortDateString()%></span>
							<p>
								<%#:Item.Description%>
							</p>
							<a class="more fancybox.iframe detailsLink" href="newsletter-details.aspx?id=<%#Item.NewsletterID%>&amp;title=<%#Server.UrlEncode(Item.Title)%><%#String.IsNullOrEmpty(Request.QueryString.ToString().Replace("Id=" + Request.QueryString["Id"], "").Replace("id=" + Request.QueryString["Id"], "")) ? "" : "&" + Request.QueryString.ToString().Replace("Id=" + Request.QueryString["Id"], "").Replace("id=" + Request.QueryString["Id"], "")%>">
								Read More »</a>
						</div>
						<!--end bottom-->
					</div>
					<!--end top-->
				</li>
			</ItemTemplate>
			<EmptyDataTemplate>
				<br />
				<h2>There are currently no newsletters</h2>
			</EmptyDataTemplate>
		</asp:ListView>
		<asp:DataPager ID="uxBottomPager" runat="server" PagedControlID="uxNewsListView" QueryStringField="Page" class="pagination">
			<Fields>
				<asp:NextPreviousPagerField PreviousPageText="Previous" ShowNextPageButton="false" ButtonCssClass="prev" />
				<asp352:NumericPagerFieldCustom ButtonCount="99999999" />
				<asp:NextPreviousPagerField PreviousPageText="Next" ShowPreviousPageButton="false" ButtonCssClass="next" />
			</Fields>
		</asp:DataPager>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery-ui.custom.min.js,~/tft-js/core/pagination.js"></asp:Literal>
	<script type="text/javascript">
		// <![CDATA[
		var fancyboxDetailsParams = {
			'closeClick': false,
			'height': 600,
			'width': 650,
			'padding': 0
		};
		var fancyboxSubscribeParams = {
			'closeClick': false,
			'height': 555,
			'width': 650,
			'padding': 0
		};
		$(document).ready(function () {
			$(".detailsLink").fancybox(fancyboxDetailsParams);
			$("#<%=uxSubscribeLink.ClientID%>").fancybox(fancyboxSubscribeParams);

			newsletterID = "<%=m_NewsletterID%>";
			if (newsletterID != null && newsletterID != "") {
				anIframe = $("h3 a.fancybox\\.iframe:first");
				var oldLink = anIframe.attr('href');
				anIframe.attr('href', 'newsletter-details.aspx?Id=<%=m_NewsletterID%>');
				anIframe.fancybox(fancyboxDetailsParams).trigger("click");
				anIframe.attr('href', oldLink);
			}
			<%if (m_Subscribe.HasValue)
	 {%>
			else if (<%=m_Subscribe.ToString().ToLower()%>)
				$("#<%=uxSubscribeLink.ClientID%>").fancybox(fancyboxSubscribeParams).trigger("click");
			<%
	 }%>
		});
		// ]]>
	</script>
</asp:Content>
