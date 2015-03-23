<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="videos.aspx.cs" Inherits="VideoPage" Title="Videos" ViewStateMode="Disabled" %>
<%@ Reference VirtualPath="~/microsite.master" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<div class="newsPost listingContainer newsletter">
		<h1>Videos</h1>
		<asp:DataPager ID="uxTopPager" runat="server" PagedControlID="uxListView" QueryStringField="Page" class="pagination">
			<Fields>
				<asp:NextPreviousPagerField PreviousPageText="Previous" ShowNextPageButton="false" ButtonCssClass="prev" />
				<asp352:NumericPagerFieldCustom ButtonCount="99999999" />
				<asp:NextPreviousPagerField PreviousPageText="Next" ShowPreviousPageButton="false" ButtonCssClass="next" />
			</Fields>
		</asp:DataPager>
		<asp:ListView ID="uxListView" runat="server" ItemPlaceholderID="uxItemPlaceHolder" ItemType="Classes.Videos.Video">
			<LayoutTemplate>
				<ul class="videoList">
					<asp:PlaceHolder ID="uxItemPlaceHolder" runat="server"></asp:PlaceHolder>
				</ul>
			</LayoutTemplate>
			<ItemTemplate>
				<li>
					<div class="top">
						<div class="bottom">
							<h3><%#Item.Title%></h3>
							<%# Item.Url.ToLower().Contains("iframe") ? Item.Url : string.Format(@"<iframe width=""250"" height=""190"" src=""{0}"" frameborder=""0"" allowfullscreen></iframe>", Item.Url) %>
						</div>
					</div>
				</li>
			</ItemTemplate>
			<EmptyDataTemplate>
				No videos found
			</EmptyDataTemplate>
		</asp:ListView>
		<asp:DataPager ID="uxBottomPager" runat="server" PagedControlID="uxListView" QueryStringField="Page" class="pagination">
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
		$(document).ready(function () {
			if ($(".pagination").length > 0)
				$(".pagination").pagination();
		});
		// ]]>
	</script>
</asp:Content>
