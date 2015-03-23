<%@ Page Title="Staff Directory" Language="C#" MasterPageFile="~/microsite.master" AutoEventWireup="true" CodeFile="staff.aspx.cs" Inherits="staff" %>

<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/realtor.css,~/css/alphabet.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentContentDiv">
	<div class="content inner realtorSearch">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="headerBG">
		<h1>Find Your Meybohm REALTOR<sup>&reg;</sup></h1>
	</div>
	<div class="realtorSearchContent">
		<div class="form">
			<p>Please select from the following options to search for a Meybohm REALTOR<sup>&reg;</sup>. Search based on your preferences, or search by name.</p>
			<div class="formWhole">
				<label for="<%= uxMarket.ClientID %>">Location of Interest:</label>
				<asp:DropDownList runat="server" ID="uxMarket" AppendDataBoundItems="true">
					<asp:ListItem Text="--Select a Market--" Value=""></asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="formWhole">
				<label for="<%= uxLanguage.ClientID %>">Language:</label>
				<asp:DropDownList runat="server" ID="uxLanguage" AppendDataBoundItems="true">
					<asp:ListItem Text="--Select a Language--" Value=""></asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="formWhole">
				<label for="<%= uxStaffType.ClientID %>">Staff Type:</label>
				<asp:DropDownList runat="server" ID="uxStaffType" AppendDataBoundItems="true">
					<asp:ListItem Text="--All Staff Types--" Value="-1"></asp:ListItem>
				</asp:DropDownList>
				<span class="andOr">(and/or)</span>
			</div>
			<div class="formWhole">
				<label for="<%= uxName.ClientID %>">Name:</label>
				<asp:TextBox runat="server" ID="uxName" CssClass="text" />
			</div>
			<div class="formWhole">
				<a href="#" class="button" id="uxSearch">Search</a>
			</div>
		</div>
		<asp:Repeater ID="uxAlphabet" runat="server">
			<HeaderTemplate>
				<ul class="AtoZ" id="alphabetPaging">
			</HeaderTemplate>
			<ItemTemplate>
				<li>
					<a href="#" <%# Container.DataItem.ToString() == CurrentLetter ? " class='active'" : !m_StaffAlphabet.Contains(Container.DataItem.ToString()) ? " class='unavailable'" : ""  %>><%# Container.DataItem.ToString() %></a></li>
			</ItemTemplate>
			<FooterTemplate></ul></FooterTemplate>
		</asp:Repeater>
		<div class="clear"></div>
		<asp:DataPager ID="uxTopPager" runat="server" PagedControlID="uxListView" QueryStringField="Page" class="pagination pagTop">
			<Fields>
				<asp352:NumericPagerFieldCustom ButtonCount="99999999" />
			</Fields>
		</asp:DataPager>
		<div class="clear"></div>
		<asp:ListView ID="uxListView" runat="server" ItemPlaceholderID="uxItemPlaceHolder" ItemType="Classes.Media352_MembershipProvider.UserInfo">
			<LayoutTemplate>
				<ul class="realtorList">
					<asp:PlaceHolder ID="uxItemPlaceHolder" runat="server"></asp:PlaceHolder>
				</ul>
			</LayoutTemplate>
			<ItemTemplate>
				<li class="realtorSearchInfoItem clearfix">
					<img alt="" src='<%# ResolveUrl(BaseCode.Helpers.ResizedImageUrl(Item.Photo, BaseCode.Globals.Settings.UploadFolder + "agents/", 100, 150, true)) %>' />
					<div class="realtorSearchInfoWrap">
						<h3>
							<a href="staff-details?id=<%# Item.UserID %>"><%# Item.FirstAndLast %></a></h3>
						<asp:Label runat="server" CssClass="realtorTitle" Visible="<%# Item.JobTitle != null %>"><%# Item.JobTitle != null ? Item.JobTitle.Name : string.Empty %></asp:Label>
						<span class="realtorLoc"><%# Item.PreferredCMMicrositeID.HasValue ? (Item.PreferredCMMicrositeID.Value == 2 ? "Augusta, GA" : "Aiken, SC") : "" %></span>
						<asp:Label runat="server" CssClass="realtorPhone" Visible="<%# !String.IsNullOrWhiteSpace(GetPrimaryPhoneNumber(Item)) %>"><%# GetPrimaryPhoneNumber(Item) %></asp:Label>
					</div>
					<div class="realtorSearchActionWrap">
						<asp:HyperLink runat="server" CssClass="btn arrow viewList" Visible="<%# Item.ShowListingLink %>" NavigateUrl='<%# "~/" + GetPreferredMicrosite(Item) + "/" + (Item.StaffTypeID == (int)Classes.Media352_MembershipProvider.StaffTypes.RentalRealtor ? "rentals" : ((microsite)Master).NewHomes ? "new-search" : "search") + "?AgentID=" + Item.UserID %>'><span>View Listings</span></asp:HyperLink>
						<a href="staff-details?id=<%# Item.UserID + SearchQueryString.Replace("?", "&") %>" class="btn arrow"><span>Contact <%# Item.StaffTypeID == (int)Classes.Media352_MembershipProvider.StaffTypes.REALTOR || Item.StaffTypeID == (int)Classes.Media352_MembershipProvider.StaffTypes.RentalRealtor ? "Agent" : "Staff" %></span></a>
					</div>
				</li>
			</ItemTemplate>
			<EmptyDataTemplate>
				<h3>No staff found</h3>
			</EmptyDataTemplate>
		</asp:ListView>
		<asp:DataPager ID="uxBottomPager" runat="server" PagedControlID="uxListView" QueryStringField="Page" class="pagination paginationBottom">
			<Fields>
				<asp352:NumericPagerFieldCustom ButtonCount="99999999" />
			</Fields>
		</asp:DataPager>
		<div class="clear"></div>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="PageSpecificJS" runat="Server">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery-ui.custom.min.js,~/tft-js/core/pagination.js"></asp:Literal>
	<script type="text/javascript">
		$(document).ready(function () {
			if ($(".pagination").length > 0)
				$(".pagination").pagination({
					pagesVisible: 10
				});

			$("#<%= uxName.ClientID %>").keydown(function (event) {
				if (event.keyCode == 13)
					$("#uxSearch").trigger("click");
				return event.keyCode != 13;
			});
			$("#uxSearch").click(function () {
				var searchText = $("#<%= uxName.ClientID %>").val();
				var marketID = $("#<%= uxMarket.ClientID %>").val();
				var languageID = $("#<%= uxLanguage.ClientID %>").val();
				var staffTypeID = $("#<%= uxStaffType.ClientID %>").val();
				window.location = ('staff?' + (searchText != "" ? "searchText=" + searchText : "") + (marketID != "" ? "&marketID=" + marketID : "") + (languageID != "" ? "&languageID=" + languageID : "") + (staffTypeID != "" ? "&staffTypeID=" + staffTypeID : "")).replace('?&', '?').replace(/\?$/, "");
				return false;
			});
			$(".AtoZ a").click(function () {
				if ($(this).hasClass("active") || $(this).hasClass("unavailable"))
					return false;
				var marketID = $("#<%= uxMarket.ClientID %>").val();
				var languageID = $("#<%= uxLanguage.ClientID %>").val();
				var staffTypeID = $("#<%= uxStaffType.ClientID %>").val();
				window.location = ('staff?letter=' + $(this).html() + (marketID != "" ? "&marketID=" + marketID : "") + (languageID != "" ? "&languageID=" + languageID : "") + (staffTypeID != "" ? "&staffTypeID=" + staffTypeID : "")).replace('?&', '?').replace(/\?$/, "");
				return false;
			});
		});
	</script>
</asp:Content>

