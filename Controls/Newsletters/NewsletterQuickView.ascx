<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsletterQuickView.ascx.cs" EnableViewState="false" Inherits="Controls_Newsletters_NewsletterQuickView" %>
<%@ Import Namespace="BaseCode" %>
<%@ Register TagName="Subscribe" TagPrefix="Newsletter" Src="~/Controls/Newsletters/Subscribe.ascx" %>
<asp:ObjectDataSource ID="uxNewsListDataSource" runat="server" SelectMethod="GetNumArticlesQuickView" TypeName="Classes.Newsletters.Newsletter">
	<SelectParameters>
		<asp:Parameter Type="Int32" Name="numArticles" />
	</SelectParameters>
</asp:ObjectDataSource>
<div class="quickview newsPost newsletter">
	<asp:ListView ID="uxNewsListView" runat="server" ItemPlaceholderID="uxItemPlaceHolder" DataSourceID="uxNewsListDataSource" DataKeyNames="NewsletterID" ItemType="Classes.Newsletters.Newsletter">
		<LayoutTemplate>
			<ul id="storyList" class="storyList">
				<asp:PlaceHolder ID="uxItemPlaceHolder" runat="server"></asp:PlaceHolder>
			</ul>
		</LayoutTemplate>
		<ItemTemplate>
			<li>
				<div class="top">
					<div class="bottom">
						<h3>
							<a class="fancybox.iframe" href="newsletter-details.aspx?id=<%#Item.NewsletterID%>&amp;title=<%#Server.UrlEncode(Item.Title)%>">
								<%#Item.Issue%>:
								<%#Item.Title.Replace("& ", "&amp; ")%></a></h3>
						<span class="byLine articleDate">
							<%#Item.DisplayDateClientTime.ToShortDateString()%></span>
						<p>
							<%#Helpers.ForceShorten(Item.Description, SummaryLength)%>
						</p>
						<a class="more fancybox.iframe" href="newsletter-details.aspx?id=<%#Item.NewsletterID%>&amp;title=<%#Server.UrlEncode(Item.Title)%>">Read More »</a>
					</div>
					<!--top-->
				</div>
				<!--end bottom-->
			</li>
		</ItemTemplate>
	</asp:ListView>
	<asp:HyperLink runat="server" ID="uxViewAll" Text="View All Newsletters" NavigateUrl="~/newsletter.aspx"></asp:HyperLink>
	<asp:PlaceHolder runat="server" ID="uxSubscribePH">
		<a href="#" class="button newsletterSubscribeLink"><span>Subscribe to Newsletter</span></a>
		<div class="newsletterSubscribe" style="display: none;">
			<Newsletter:Subscribe runat="server" ID="uxSubscribeQV" ShowCancelButton="false" />
		</div>
	</asp:PlaceHolder>
</div>
<script type="text/javascript">
	//<![CDATA[
	var newsletterFancyboxDetailsParams = {
		closeClick: false,
		height: 600,
		width: 550,
		padding: 0
	};
	$(document).ready(function() {
		$(".fancybox\\.iframe").fancybox(newsletterFancyboxDetailsParams);
		<% if (uxSubscribePH.Visible){ %>
		$(".newsletterSubscribeLink").click(function(){
			var $newsletterSubscribe = $("div.newsletterSubscribe");
			if ($newsletterSubscribe.is(':hidden')){
				$newsletterSubscribe.slideDown();
				$(this).children().html('Collapse Subscribe');
			}
			else {
				$newsletterSubscribe.slideUp();
				$(this).children().html('Subscribe to Newsletter');
			}
			 $("html, body").animate({ scrollTop: $newsletterSubscribe.offset().top - $newsletterSubscribe.height() });
			return false;
		});
		<%} %>
	});
	//]]>
</script>
