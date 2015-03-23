<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="configuration-settings.aspx.cs" Inherits="Admin_ConfigurationSettings" Title="Admin - Contacts Configuration Settings" %>

<%@ Register TagPrefix="Controls" TagName="ConfigurationSettings" Src="~/Controls/BaseControls/ConfigurationSettings.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<Controls:ConfigurationSettings runat="server" ID="uxConfigSettings" Component="Contacts" />
</asp:Content>
