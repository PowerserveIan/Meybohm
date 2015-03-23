<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LanguageToggleAdmin.ascx.cs"
	Inherits="Controls_BaseControls_LanguageToggleAdmin" %>
<asp:DropDownList ID="Language" runat="server" OnSelectedIndexChanged="ToggleLanguage"
	CssClass="LangList" AutoPostBack="true">
</asp:DropDownList>
