<%@ Page Language="c#" MasterPageFile="~/microsite.master" Inherits="microsite_inner" CodeFile="microsite-inner.aspx.cs" %>

<%@ Reference Control="~/Controls/DynamicHeader/DynamicHeader.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/micrositenew.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentWindow">

	<cm:ContentRegion ID="uxMainRegion" runat="server" RegionName="MainRegion" />				
</asp:Content>
