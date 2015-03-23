<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminQuickView.ascx.cs" Inherits="Controls_DynamicHeader_AdminQuickView" %>
<%@ Register TagPrefix="DynamicHeader" TagName="DynamicHeader" Src="~/Controls/DynamicHeader/DynamicHeader.ascx" %>
<%@ Register TagPrefix="Controls" TagName="BaseAdminQuickView" Src="~/Controls/BaseControls/BaseAdminQuickView.ascx" %>
<Controls:BaseAdminQuickView runat="server" ID="uxAdminQuickView" ComponentName="Dynamic Header" ComponentFolderLocation="~/admin/dynamic-header/">
	<ContentAreaTemplate>
		<h4>Preview</h4>
		<asp:Literal runat="server" ID="uxUpdatedOn" Text="You need to create your Dynamic Header Image Slideshow!"></asp:Literal>
		<DynamicHeader:DynamicHeader runat="server" ID="uxDynamicHeader" ImageHeight="110" ImageWidth="270" ShowThumbnails="false" />
	</ContentAreaTemplate>
</Controls:BaseAdminQuickView>
