<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="error.aspx.cs" Inherits="Error" Title="Error" %>

<asp:Content ContentPlaceHolderID="PageSpecificCSS" runat="Server">
	<base href="<%= BaseCode.Helpers.RootPath %>" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<cm:ContentRegion runat="server" ID="uxMainRegion" RegionName="MainRegion" />
	Click here to return to the
	<a href="~/" runat="server">home</a>
	page.
</asp:Content>
