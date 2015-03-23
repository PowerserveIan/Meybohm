<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" Title="Login" %>

<%@ Register TagPrefix="Generic" TagName="GenericLogin" Src="~/Controls/BaseControls/GenericLoginControl.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="formWrapper">
		<Generic:GenericLogin ID="uxLogin" runat="server" />
	</div>
</asp:Content>