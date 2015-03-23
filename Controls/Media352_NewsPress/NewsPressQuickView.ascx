<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsPressQuickView.ascx.cs" EnableViewState="false" Inherits="Controls_Media352_NewsPress_NewsPressQuickView" %>
<%@ Import Namespace="BaseCode" %>
<asp:ObjectDataSource ID="uxNewsListDataSource" runat="server" SelectMethod="GetNumArticlesQuickView" TypeName="Classes.Media352_NewsPress.NewsPress">
	<SelectParameters>
		<asp:Parameter Type="Int32" Name="numArticles" />
		<asp:Parameter Type="Int32" Name="categoryID" />
	</SelectParameters>
</asp:ObjectDataSource>
<div class="quickNews quickview newsPost newsPress">
	<asp:ListView ID="uxNewsListView" runat="server" ItemPlaceholderID="uxItemPlaceHolder" DataSourceID="uxNewsListDataSource" DataKeyNames="NewsPressID" ItemType="Classes.Media352_NewsPress.NewsPress">
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
							<a href='<%# "news-press-details.aspx?id=" + Item.NewsPressID + "&amp;title=" + Server.UrlEncode(Item.Title) %>'>
								<%#Helpers.ForceShorten(Item.Title.Replace("& ", "&amp; "), TitleLength)%></a></h3>
						<p>
							<%#Helpers.ForceShorten(Item.Summary, SummaryLength)%><br />
						</p>
						<span class="byLine articleDate">
							<%#String.Format("{0:d}", Item.Date)%></span>
						<a class="more" href='<%# "news-press-details.aspx?id=" + Item.NewsPressID + "&amp;title=" + Server.UrlEncode(Item.Title) %>'>Read More »</a>
						<div class="clear"></div>
					</div>
					<!--end top-->
				</div>
				<!--end bottom-->
			</li>
		</ItemTemplate>
	</asp:ListView>
	<a class="readMoreLink homeNewsBlogBtmLink textLinkBtn" href="news-press">View all<strong> »</strong></a>
</div>
