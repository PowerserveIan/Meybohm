<%@ Page Language="c#" MasterPageFile="~/frontend.master" Inherits="BaseCMPage" CodeFile="base-cm-page.aspx.cs" %>

<%@ Reference Control="~/Controls/DynamicHeader/DynamicHeader.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentWindow">
	<cm:ContentRegion ID="uxMainRegion" runat="server" RegionName="MainRegion" />
</asp:Content>
