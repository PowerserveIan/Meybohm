<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="news-press.aspx.cs" Inherits="NewsPressPage" Title="News and Press Releases" ViewStateMode="Disabled" %>

<%@ Import Namespace="BaseCode" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/newsShared.css,~/css/newsPress.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="newsPost listingContainer newsPress">
		<div class="headerBG"><h1>News &amp; Press Releases</h1></div>
		<div class="newsPressWrap">
		<div class="archiveToggleContainer">
			<asp:Label runat="server" ID="uxCurrentNewsLbl" CssClass="newsTab">Current News</asp:Label><asp:HyperLink ID="uxCurrentNews" runat="server" CssClass="newsTab">Current News</asp:HyperLink>
			<asp:Label runat="server" ID="uxNewsArchivesLbl" CssClass="newsTab">News Archives</asp:Label><asp:HyperLink ID="uxNewsArchives" runat="server" CssClass="newsTab">News Archives</asp:HyperLink>
			<asp:HyperLink ID="uxRssFeed" CssClass="last rss" runat="server" ImageUrl="~/img/RSSFeed.jpg" Text="News Press RSS Feed"></asp:HyperLink>
			<div class="clear"></div>
		</div>
		<asp:DataPager ID="uxTopPager" runat="server" PagedControlID="uxNewsListView" QueryStringField="Page" class="pagination">
			<Fields>
				<asp:NextPreviousPagerField PreviousPageText="Previous" ShowNextPageButton="false" ButtonCssClass="prev" />
				<asp352:NumericPagerFieldCustom ButtonCount="99999999" />
				<asp:NextPreviousPagerField PreviousPageText="Next" ShowPreviousPageButton="false" ButtonCssClass="next" />
			</Fields>
		</asp:DataPager>
		<div class="clear"></div>
		<asp:PlaceHolder runat="server" ID="uxCategoryPlaceHolder">
			<div class="categoryChooser">
				<label for="<%=uxCategories.ClientID%>">
					Please select a category:</label>
				<asp:DropDownList ID="uxCategories" runat="server" AppendDataBoundItems="true">
					<asp:ListItem Selected="True" Value="all" Text="All Articles"></asp:ListItem>
				</asp:DropDownList>
			</div>
		</asp:PlaceHolder>
		<asp:ListView ID="uxNewsListView" EnableViewState="false" runat="server" ItemPlaceholderID="uxItemPlaceHolder" DataKeyNames="NewsPressID" ItemType="Classes.Media352_NewsPress.NewsPress">
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
								<a href="news-press-details.aspx?id=<%#Item.NewsPressID%>&amp;title=<%#Server.UrlEncode(Item.Title)%><%#String.IsNullOrEmpty(Request.QueryString.ToString()) ? "" : "&" + Request.QueryString%>">
									<%#:Item.Title%></a></h3>
							<span class="byLine articleDate">
								<%#String.Format("{0:d}", Item.Date)%></span>
							<p>
								<%#:Item.Summary%></p>
							<Facebook:LikeButton runat="server" ID="uxFacebookLike" UrlToLike='<%# Helpers.RootPath + "news-press-details.aspx?id=" + Item.NewsPressID + "&amp;title=" + Server.UrlEncode(Item.Title) %>' />
							<a class="more" href="news-press-details.aspx?id=<%#Item.NewsPressID%>&amp;title=<%#Server.UrlEncode(Item.Title)%><%#String.IsNullOrEmpty(Request.QueryString.ToString()) ? "" : "&" + Request.QueryString%>">
								Read More »</a>
						</div>
						<!--end top-->
					</div>
					<!--end bottom-->
				</li>
			</ItemTemplate>
			<EmptyDataTemplate>
				No articles found
			</EmptyDataTemplate>
		</asp:ListView>
		<asp:DataPager ID="uxBottomPager" runat="server" PagedControlID="uxNewsListView" QueryStringField="Page" class="pagination">
			<Fields>
				<asp:NextPreviousPagerField PreviousPageText="Previous" ShowNextPageButton="false" ButtonCssClass="prev" />
				<asp352:NumericPagerFieldCustom ButtonCount="99999999" />
				<asp:NextPreviousPagerField PreviousPageText="Next" ShowPreviousPageButton="false" ButtonCssClass="next" />
			</Fields>
		</asp:DataPager>
		<div class="clear"></div>
	</div>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery-ui.custom.min.js,~/tft-js/core/pagination.js"></asp:Literal>
	<script type="text/javascript">
		// <![CDATA[
		$(document).ready(function () {
			$("#<%=uxCategories.ClientID%>").change(function () {
				var archived = '<%=m_Archived%>';
				if (archived != "")
					archived = "archived=" + archived + '&';
				window.location = 'news-press?' + archived + 'Category=' + $(this).val();
			});
		});
		// ]]>
	</script>
</asp:Content>
