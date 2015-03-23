<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="404.aspx.cs" Inherits="_404" Title="Page Not Found" %>

<asp:Content ContentPlaceHolderID="PageSpecificCSS" runat="Server">
	<base href="<%= BaseCode.Helpers.RootPath %>" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<cm:ContentRegion runat="server" ID="uxMainRegion" RegionName="MainRegion" />
	<span class="fourohfour">404</span>
	<h1>We're sorry!</h1>
	<p>This page seems to be missing or it never existed.</p>
	<p><a href="~/" runat="server" class="button">Return Home</a></p>
</asp:Content>
