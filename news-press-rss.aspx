<%@ Page Language="C#" AutoEventWireup="true" CodeFile="news-press-rss.aspx.cs" Inherits="NewsPressRSS" ContentType="text/xml" %>

<%@ Import Namespace="BaseCode" %>
<asp:Repeater ID="uxRSSRepeater" runat="server" ItemType="Classes.Media352_NewsPress.NewsPress">
	<HeaderTemplate>
		<rss xmlns:content="http://purl.org/rss/1.0/modules/content/" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:atom="http://www.w3.org/2005/Atom" version="2.0">
	    <channel>
		    <title><![CDATA[News & Press Releases - RSS Feed - <%#Globals.Settings.SiteTitle%>]]></title>
		    <link><![CDATA[<%#Helpers.RootPath%>]]></link>
		    <description>352 Media News</description>
		    <language>en-us</language>
		    <atom:link href="<%#Helpers.RootPath%>news-press-rss.aspx" rel="self" type="application/rss+xml" />
	</HeaderTemplate>
	<ItemTemplate>
		<item>
           <title><![CDATA[<%#Item.Title%>]]></title>
            <description><![CDATA[<%#Item.Summary%>]]></description>
            <link><![CDATA[<%#Helpers.RootPath + "news-press-details.aspx?id=" + Item.NewsPressID + "&title=" + Server.UrlEncode(Item.Title)%>]]></link>
            <guid><![CDATA[<%#Helpers.RootPath + "news-press-details.aspx?id=" + Item.NewsPressID + "&title=" + Server.UrlEncode(Item.Title)%>]]></guid>
            <pubDate><%#String.Format("{0:R}", Item.Date)%></pubDate>
        </item>
	</ItemTemplate>
	<FooterTemplate>
		</channel> </rss>
	</FooterTemplate>
</asp:Repeater>
