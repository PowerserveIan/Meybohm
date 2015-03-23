<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="configuration-settings.aspx.cs" Inherits="Admin_ConfigurationSettings" Title="Admin - Showcase Configuration Settings" %>

<%@ Register TagPrefix="Controls" TagName="ConfigurationSettings" Src="~/Controls/BaseControls/ConfigurationSettings.ascx" %>
<%@ Register TagPrefix="Controls" TagName="DateTimePicker" Src="~/Controls/BaseControls/DateTimePicker.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<Controls:ConfigurationSettings runat="server" ID="uxConfigSettings" Component="Showcase" LetPageHandleSaving="true" />
</asp:Content>
