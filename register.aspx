<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="register.aspx.cs" Inherits="register" Title="Register" %>

<%@ Register Src="~/Controls/BaseControls/Register.ascx" TagPrefix="User" TagName="Register" %>
<%@ Register Src="~/Controls/BaseControls/SpamPrevention.ascx" TagPrefix="Controls" TagName="SpamPrevention" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="formWrapper">
		<User:Register runat="server" ID="uxRegister" />
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<Controls:SpamPrevention runat="server" ID="SpamPrevention" AlwaysShow="true" SubmitClientIDName="uxCreateUser" />
</asp:Content>
