<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminQuickView.ascx.cs" Inherits="Controls_Media352_NewsPress_AdminQuickView" %>
<%@ Register TagPrefix="Controls" TagName="BaseAdminQuickView" Src="~/Controls/BaseControls/BaseAdminQuickView.ascx" %>
<Controls:BaseAdminQuickView runat="server" ID="uxAdminQuickView" ComponentName="News Press" ComponentFolderLocation="~/admin/media352-news-press/">
	<ContentAreaTemplate>
		<h4>Most Recent Article</h4>
		<asp:PlaceHolder runat="server" ID="uxUpdateCallout" Visible="false">
			<asp:Literal runat="server" ID="uxCallOutText" Text="You need to add articles more frequently!"></asp:Literal>
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="uxArticlePH">
			<div class="qv-np-articleLinkList">
				<div class="qv-np-articleLink"><span class="qv-np-articleTitle">
					<asp:HyperLink runat="server" ID="uxTitleLink"></asp:HyperLink></span> <span class="qv-np-articleDate">
						<asp:Literal runat="server" ID="uxDate"></asp:Literal></span>
					<p class="qv-np-articleSummary">
						<asp:Literal runat="server" ID="uxSummary"></asp:Literal></p>
				</div>
				<!--end articleLink-->
			</div>
			<!--end articleLinkList-->
		</asp:PlaceHolder>
	</ContentAreaTemplate>
</Controls:BaseAdminQuickView>
