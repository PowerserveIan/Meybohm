<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminQuickView.ascx.cs" Inherits="Controls_ContentManager_AdminQuickView" %>
<%@ Import Namespace="Classes.ContentManager" %>
<%@ Import Namespace="Classes.SiteLanguages" %>
<%@ Register TagPrefix="Controls" TagName="BaseAdminQuickView" Src="~/Controls/BaseControls/BaseAdminQuickView.ascx" %>
<Controls:BaseAdminQuickView runat="server" ID="uxAdminQuickView" ComponentName="Content Manager" ComponentFolderLocation="~/admin/content-manager/">
	<ContentAreaTemplate>
		<h4>Items Requiring Attention</h4>
		<table>
			<tbody>
				<tr runat="server" id="uxApprovalRequiredRow" visible="false">
					<td>
						<span class='icon warning'>Items require your approval</span>
					</td>
					<td>
						<asp:HyperLink runat="server" ID="uxApprovalRequired" NavigateUrl="~/admin/content-manager/approval-alerts.aspx" Text="view"></asp:HyperLink>
					</td>
				</tr>
				<tr runat="server" id="uxSubmittedFormsRow" visible="false" class="odd">
					<td>
						<span class='icon warning'>There are submitted forms that require your attention</span>
					</td>
					<td>
						<asp:HyperLink runat="server" ID="uxSubmittedForms" NavigateUrl="~/admin/content-manager/admin-cm-submitted-form.aspx" Text="view"></asp:HyperLink>
					</td>
				</tr>
				<asp:Repeater runat="server" ID="uxTranslationsRepeater" ItemType="Classes.SiteLanguages.Language">
					<ItemTemplate>
						<tr runat="server" visible="<%#CMPage.PagesNeedTranslating(Item.LanguageID)%>" class='<%# Container.ItemIndex % 2 == 1 ? "" : "odd" %>'>
							<td>
								<span class='icon warning'>
									<%#"There are " + Item.Culture + " pages that require translations"%></span>
							</td>
							<td>
								<asp:HyperLink runat="server" ID="uxTranslationLink" NavigateUrl='<%#"~/admin/content-manager/content-manager.aspx?language=" + Item.CultureName%>' Text="view"></asp:HyperLink>
							</td>
						</tr>
					</ItemTemplate>
				</asp:Repeater>
				<asp:Literal runat="server" ID="uxNoItemsRequiringAttention" Text="<tr><td colspan='2'>There are no items that require your attention.</td></tr>" Visible="false"></asp:Literal>
			</tbody>
		</table>
	</ContentAreaTemplate>
</Controls:BaseAdminQuickView>
