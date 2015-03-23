<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true"
    CodeFile="newsletter-subscribe.aspx.cs" Inherits="NewsletterSubscribe" Title="Newsletter - Subscribe" %>
<%@ Register TagName="Subscribe" TagPrefix="Newsletter" Src="~/Controls/Newsletters/Subscribe.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/newsShared.css,~/css/newsletter.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
    <Newsletter:Subscribe runat="server" ID="uxSubscribe" />
</asp:Content>
