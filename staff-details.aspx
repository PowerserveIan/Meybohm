<%@ Page Title="Staff Details" Language="C#" MasterPageFile="~/microsite.master" AutoEventWireup="true" CodeFile="staff-details.aspx.cs" Inherits="staff_details" %>

<%@ Register Src="~/Controls/Contacts/ContactForm.ascx" TagPrefix="Contacts" TagName="ContactForm" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/realtor.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentContentDiv">
	<div class="content inner realtorSearch">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="headerBG">
		<h1>Meybohm REALTOR<sup>&reg;</sup> Information</h1>
	</div>
	<div class="realtorInfo">
		<div class="realtorInfoInner clearfix">
			<div class="realInfoLeft">
				<asp:Image runat="server" ID="uxImage" ResizerWidth="120" ResizerHeight="180" />
				<h3>
					<asp:Literal runat="server" ID="uxFirstAndLast"></asp:Literal></h3>
				<asp:Label runat="server" ID="uxJobTitle"></asp:Label>
				<asp:Label runat="server" ID="uxLocation"></asp:Label>
				<asp:Repeater runat="server" ID="uxDesignations" ItemType="Classes.Media352_MembershipProvider.UserDesignation">
					<ItemTemplate>
						<asp:Image runat="server" AlternateText="<%# Item.Designation.Name %>" ToolTip="<%# Item.Designation.Name %>" ImageUrl='<%# "~/" + BaseCode.Globals.Settings.UploadFolder + "designations/" + Item.Designation.Icon + "?width=25&height=25&mode=crop&anchor=middlecenter" %>'
							Height="25" Width="25" CssClass="designationIcon" />
					</ItemTemplate>
				</asp:Repeater>
				<asp:Label runat="server" ID="uxHomePhone"></asp:Label>
				<asp:Label runat="server" ID="uxCellPhone"></asp:Label>
				<asp:Label runat="server" ID="uxOfficePhone"></asp:Label>
				<asp:Label runat="server" ID="uxFax"></asp:Label>
				<asp:HyperLink runat="server" ID="uxWebsite" Target="_blank"></asp:HyperLink>
				<div class="clear"></div>
			</div>
			<div class="realInfoRight">
				<asp:HyperLink runat="server" ID="uxViewListings" CssClass="btn arrow large viewMyListings"><span>View My Listings</span></asp:HyperLink>
				<asp:PlaceHolder runat="server" ID="uxBioPH">
					<div class="quickFact">
						<h4>Quick Fact</h4>
						<p>
							<asp:Literal runat="server" ID="uxBiography"></asp:Literal>
						</p>
					</div>
				</asp:PlaceHolder>
			</div>
		</div>
	</div>
	<asp:PlaceHolder runat="server" ID="uxTestimonialsPH">
		<div class="realDetTest">
			<ul class="testimonial">
				<asp:Repeater runat="server" ID="uxTestimonials" ItemType="Classes.Media352_MembershipProvider.UserTestimonial">
					<ItemTemplate>
						<li>
							<p>
								<%# Item.Testimonial %>
							</p>
							<span>- <%# Item.GiverNameAndLocation %></span>
						</li>
					</ItemTemplate>
				</asp:Repeater>
			</ul>
		</div>
	</asp:PlaceHolder>
	<div class="realDetContact">
		<div class="realDetContactInner">
			<asp:PlaceHolder runat="server" ID="uxContactMeHeaderPH">
				<h2>Contact Me</h2>
				<cm:ContentRegion runat="server" ID="uxContactRegion" RegionName="Contact" />
			</asp:PlaceHolder>
			<Contacts:ContactForm runat="server" ID="uxContactForm" HideIntroText="true" ContactFormType="Agent" />
		</div>
	</div>
	<div class="realDetBackToResults">
		<asp:HyperLink runat="server" CssClass="button" ID="uxBackToSearchResults">Back to Search Results</asp:HyperLink>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="PageSpecificJS" runat="Server">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/jquery.jcarousel.min.js"></asp:Literal>
	<% if (uxTestimonialsPH.Visible && uxTestimonials.Items.Count > 1)
	{ %>
	<script type="text/javascript">
		$(document).ready(function () {
			$(".testimonial").jcarousel({
				buttonNextHTML: '<div class="testNext"><a>Next Testimonial</a></div>',
				buttonPrevHTML: '<div class="testPrev"><a>Previous Testimonial</a></div>',
				size: $(".testimonial li").length,
				scroll: 1,
				visible: 1
			});
		});
	</script>
	<% } %>
</asp:Content>

