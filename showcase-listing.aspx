<%@ Page Language="C#" AutoEventWireup="true" CodeFile="showcase-listing.aspx.cs" Title="Showcase Listing" Inherits="ShowcaseListing" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<asp:DataPager ID="uxTopPager" runat="server" PagedControlID="uxShowcaseItemsListView" QueryStringField="Page" class="pagination">
		<Fields>
			<asp:NextPreviousPagerField PreviousPageText="Previous" ShowNextPageButton="false" ButtonCssClass="prev" />
			<asp352:NumericPagerFieldCustom ButtonCount="99999999" />
			<asp:NextPreviousPagerField PreviousPageText="Next" ShowPreviousPageButton="false" ButtonCssClass="next" />
		</Fields>
	</asp:DataPager>
	<div class="clear"></div>
	<asp:ListView ID="uxShowcaseItemsListView" runat="server" ItemPlaceholderID="uxItemPlaceHolder" ItemType="Classes.Showcase.ShowcaseItem">
		<LayoutTemplate>
			<asp:PlaceHolder ID="uxItemPlaceHolder" runat="server"></asp:PlaceHolder>
		</LayoutTemplate>
		<ItemTemplate>
			<a href="home-details?id=<%#Item.ShowcaseItemID%>&amp;title=<%#Item.Title%>"><%#Item.Title%></a><br />
		</ItemTemplate>
		<EmptyDataTemplate>
			No Showcase Items found
		</EmptyDataTemplate>
	</asp:ListView>
	</form>
</body>
</html>
