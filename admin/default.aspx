<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" Title="Admin - Dashboard" %>

<%--<%@ Register TagPrefix="Blog" TagName="AdminQuickView" Src="~/Controls/Blog/AdminQuickView.ascx" %>--%>
<%@ Register TagPrefix="CMS" TagName="AdminQuickView" Src="~/Controls/ContentManager/AdminQuickView.ascx" %>
<%@ Register TagPrefix="DynamicHeader" TagName="AdminQuickView" Src="~/Controls/DynamicHeader/AdminQuickView.ascx" %>
<%--<%@ Register TagPrefix="Ecommerce" TagName="AdminQuickView" Src="~/Controls/Ecommerce/AdminQuickView.ascx" %>--%>
<%--<%@ Register TagPrefix="Events" TagName="AdminQuickView" Src="~/Controls/Events/AdminQuickView.ascx" %>--%>
<%--<%@ Register TagPrefix="FileLibrary" TagName="AdminQuickView" Src="~/Controls/FileLibrary/AdminQuickView.ascx" %>--%>
<%--<%@ Register TagPrefix="Forum" TagName="AdminQuickView" Src="~/Controls/Forum/AdminQuickView.ascx" %>--%>
<%@ Register TagPrefix="SiteUsers" TagName="AdminQuickView" Src="~/Controls/Media352_MembershipProvider/AdminQuickView.ascx" %>
<%@ Register TagPrefix="Newsletters" TagName="AdminQuickView" Src="~/Controls/Newsletters/AdminQuickView.ascx" %>
<%--<%@ Register TagPrefix="Newspress" TagName="AdminQuickView" Src="~/Controls/Media352_NewsPress/AdminQuickView.ascx" %>--%>
<%--<%@ Register TagPrefix="OpenPayment" TagName="AdminQuickView" Src="~/Controls/OpenPayments/AdminQuickView.ascx" %>--%>
<%--<%@ Register TagPrefix="Polls" TagName="AdminQuickView" Src="~/Controls/Polls/AdminQuickView.ascx" %>--%>
<%--<%@ Register TagPrefix="ProductCatalog" TagName="AdminQuickView" Src="~/Controls/ProductCatalog/AdminQuickView.ascx" %>--%>
<%--<%@ Register TagPrefix="Search" TagName="AdminQuickView" Src="~/Controls/SearchComponent/AdminQuickView.ascx" %>--%>
<%@ Register TagPrefix="Showcase" TagName="AdminQuickView" Src="~/Controls/Showcase/AdminQuickView.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<div class="title">
		<h1>Dashboard</h1>
	</div>
	<div class="dash clearfix">
<%--		<Blog:AdminQuickView runat="server" ID="uxBlogQV" />--%>
		<CMS:AdminQuickView runat="server" ID="uxCMSQV" />
		<DynamicHeader:AdminQuickView runat="server" ID="uxDynamicHeaderQV" />
<%--		<Events:AdminQuickView runat="server" ID="uxEventsQV" />--%>
<%--		<FileLibrary:AdminQuickView runat="server" ID="uxFileLibraryQV" />--%>
<%--		<Forum:AdminQuickView runat="server" ID="uxForumQV" />--%>
		<SiteUsers:AdminQuickView runat="server" ID="uxSiteUsersQV" />
		<Newsletters:AdminQuickView runat="server" ID="uxNewsletterQV" />
<%--		<Newspress:AdminQuickView runat="server" ID="uxNewsPressQV" />--%>
<%--		<Polls:AdminQuickView runat="server" ID="uxPollsQV" />--%>
		<div class="clear"></div>
<%--		<Ecommerce:AdminQuickView runat="server" ID="uxEcommerceQV" NumberOfMostPopularItems="5" />--%>
<%--		<OpenPayment:AdminQuickView runat="server" ID="uxOpenPaymentQV" />--%>
<%--		<ProductCatalog:AdminQuickView runat="server" ID="uxProductCatalogQV" />--%>
<%--		<Search:AdminQuickView runat="server" ID="uxSearchQV" />--%>
		<Showcase:AdminQuickView runat="server" ID="uxShowcaseQV" />
	</div>
	<!--end dash-->
</asp:Content>
