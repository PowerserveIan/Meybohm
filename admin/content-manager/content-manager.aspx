<%@ Page MasterPageFile="~/admin/admin.master" Language="c#" Inherits="ContentManager2.Admin.ContentManager" CodeFile="content-manager.aspx.cs" Title="Admin - Content Manager" %>

<%@ Import Namespace="Classes.ContentManager" %>
<%@ Register TagName="Toggle" TagPrefix="Language" Src="~/Controls/BaseControls/LanguageToggleAdmin.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<div class="title">
		<h1>Content Manager</h1>
	</div>
	<ul class="breadcrumbs clearfix">
		<li class="firstBreadcrumb">
			<a runat="server" href="~/admin" title="Home">Dashboard</a></li>
		<li class="currentBreadcrumb">Content Manager</li>
	</ul>
	<div class="blue"><span>If one or more of the icons below display an alert icon
		<img src="../img/treeview/icon-alert.gif" style="display: inline;" alt="Alert" />, your attention is needed. Hover your mouse cursor over the icon to see a description of the alert.</span>
		<div class="clear"></div>
	</div>
	<div class="paddingLeft paddingBottom paddingTop" runat="server" id="uxMicrositeLanguagePH">
		<asp:PlaceHolder runat="server" ID="uxMicrositePlaceHolder" Visible="false">
			<h2>
				<asp:Literal runat="server" ID="uxMicroSiteName" /></h2>
			<asp:DropDownList runat="server" ID="uxMicrositeList" AutoPostBack="true" />
			<asp:Button ID="uxPublish" Text="Publish" CssClass="button pushBtn" runat="server" CausesValidation="false" Visible="false" />
			<asp:Button ID="uxUnpublish" Text="Unpublish" CssClass="button pullBtn" runat="server" CausesValidation="false" Visible="false" />
			<asp:HyperLink CssClass="icon help" NavigateUrl="#publishPopup" runat="server" ID="publishPopupLink" Visible="false">Help</asp:HyperLink>
		</asp:PlaceHolder>
		<asp:Label runat="server" ID="uxMicrositeInactive" Text="The microsite you are managing has been disabled.  Please contact the admin to have it reenabled." Visible="false" />
		<Language:Toggle ID="uxLanguageToggle" runat="server" />
	</div>
	<asp:Repeater ID="uxTemplatesRepeater" runat="server" OnItemCommand="uxTemplatesRepeater_OnItemCommand" ItemType="Classes.ContentManager.CMTemplate">
		<HeaderTemplate>
			<div class="blue">
				<div style="float: right; vertical-align: middle;">
					<asp:LinkButton runat="server" Visible='<%#!ShowDeleted && (IsDeveloper || User.IsInRole("CMS Page Manager") || (User.IsInRole("Microsite Admin") && Settings.AllowMicrositeAdminToEditSitemap))%>' ID="ShowDeleted" AlternateText="Show deleted pages" OnClick="ShowDeleted_OnClick">
						<asp:Image runat="server" ID="uxNeedApprovalIcon" ImageUrl="~/admin/img/treeview/icon-alert.gif" CssClass="floatLeft" ToolTip="Editor Deleted Pages" /><span style="margin-left: 5px;">Show Deleted Pages</span></asp:LinkButton>
					<asp:LinkButton runat="server" Text="hide deleted" Visible='<%#ShowDeleted && (IsDeveloper || User.IsInRole("CMS Page Manager") || (User.IsInRole("Microsite Admin") && Settings.AllowMicrositeAdminToEditSitemap))%>' ID="HideDeleted" AlternateText="Hide deleted pages"
						OnClick="HideDeleted_OnClick" />
				</div>
				<div class="clear"></div>
			</div>
		</HeaderTemplate>
		<ItemTemplate>
			<asp:Label runat="server" Visible="False" ID="templateid" Text='<%#Item.CMTemplateID.ToString()%>' />
			<table class="listing">
				<thead>
					<tr>
						<th class="first" width="550">
							<%#Item.Name%>
						</th>
						<th colspan="4">
							<asp:LinkButton runat="server" Visible='<%#Item.Addable && (IsDeveloper || (User.IsInRole("Microsite Admin") && Settings.AllowMicrositeAdminToEditSitemap))%>' ID="ADD" Text="<span>Create New Page from Template</span>" CssClass="button add floatRight"
								CommandName="Add" CommandArgument='<%#Item.CMTemplateID%>' />
						</th>
					</tr>
				</thead>
				<asp:Repeater ID="uxPagesRepeater" runat="server" OnItemCommand="uxPagesRepeater_OnItemCommand" ItemType="Classes.ContentManager.CMPage">
					<ItemTemplate>
						<tr<%# Container.ItemIndex % 2 == 1 ? " class='odd'" : "" %>>
							<td class="first" style='color: <%#Item.Deleted ? "red" : "grey"%>;'>
								<a runat="server" href='<%# GetLinkForPageByCMPageID(Item.OriginalCMPageID.HasValue ? Item.OriginalCMPageID.Value : Item.CMPageID) %>' title='<%#UnapprovedRegions.Exists(c => c.CMPageID == Item.CMPageID || c.CMPageID == Item.OriginalCMPageID) ? "Unapproved Content" : "View the Page" %>'>
									<%#Settings.EnableMultipleLanguages && !String.IsNullOrEmpty(Item.CMPageTitleTitle) ? Item.CMPageTitleTitle : Item.Title%>
									<img runat="server" src="../img/treeview/icon-translationAlert.gif" alt="Not Translated" title="Not Translated" style="display: inline; margin: 0 0 0 5px;" visible="<%#Settings.EnableMultipleLanguages && String.IsNullOrEmpty(Item.CMPageTitleTitle)%>" /></a>
							</td>
							<td>
								<a href='<%# "content-manager-page.aspx?id=" + Item.CMPageID %>' class='<%# "icon properties" + (Item.OriginalCMPageID.HasValue || Item.NeedsApproval ? "alert" : "") %>' title='<%#Item.OriginalCMPageID.HasValue || Item.NeedsApproval ? "Unapproved Change" : "Edit Page Properties" %>'>
									properties</a>
							</td>
							<asp:PlaceHolder runat="server" ID="uxViewPH" Visible='<%#(!Item.EditorDeleted.HasValue && !Item.Deleted || (Item.EditorDeleted.HasValue && !Item.EditorDeleted.Value))%>'>
								<td>
									<a runat="server" href='<%# GetLinkForPageByCMPageID(Item.OriginalCMPageID.HasValue ? Item.OriginalCMPageID.Value : Item.CMPageID) %>' class='<%# "icon edit" + (UnapprovedRegions.Exists(c => c.CMPageID == Item.CMPageID || c.CMPageID == Item.OriginalCMPageID) ? "alert" : "") %>'
										title='<%#UnapprovedRegions.Exists(c => c.CMPageID == Item.CMPageID || c.CMPageID == Item.OriginalCMPageID) ? "Unapproved Content" : "View the Page" %>'>edit</a>
								</td>
							</asp:PlaceHolder>
							<asp:PlaceHolder runat="server" ID="uxPermaDeletePH" Visible='<%#ShowDeleted && IsDeveloper%>'>
								<td>
									<asp:LinkButton runat="server" CommandName="PermaDelete" CommandArgument='<%#Item.CMPageID%>' CssClass="icon delete" Text="Delete permanently" />
								</td>
							</asp:PlaceHolder>
							<td>
								<asp:LinkButton runat="server" CommandName="Restore" Visible='<%#(!Item.EditorDeleted.HasValue && Item.Deleted || (Item.EditorDeleted.HasValue && Item.EditorDeleted.Value)) && (IsDeveloper || (User.IsInRole("Microsite Admin") && Settings.AllowMicrositeAdminToEditSitemap))%>'
									CommandArgument='<%#Item.CMPageID%>' CssClass='<%# "icon restore" + (Item.EditorDeleted.HasValue && Item.EditorDeleted.Value ? " alert" : "")%>' Text="restore" ToolTip='<%#Item.EditorDeleted.HasValue && Item.EditorDeleted.Value ? "Editor Deleted" : "Restore the page"%>' />
								<asp:LinkButton runat="server" CommandName="Delete" Visible='<%#(!Item.EditorDeleted.HasValue && !Item.Deleted || (Item.EditorDeleted.HasValue && !Item.EditorDeleted.Value)) && (IsDeveloper || (User.IsInRole("Microsite Admin") && Settings.AllowMicrositeAdminToEditSitemap)) && Item.CanDelete%>'
									CommandArgument='<%#Item.CMPageID%>' CssClass='<%# "icon delete" + (Item.EditorDeleted.HasValue && !Item.EditorDeleted.Value ? " alert" : "")%>' Text="delete" ToolTip='<%#Item.EditorDeleted.HasValue && !Item.EditorDeleted.Value ? "Editor Restored" : "Delete the page"%>' />
							</td>
							<asp:PlaceHolder runat="server" ID="uxLogPH" Visible="<%#CMSHelpers.HasFullCMSPermission()%>">
								<td>
									<a href='<%# "content-manager-log.aspx?id=" + Item.CMPageID %>' class="icon log" title="View the page's edit log">log</a>
								</td>
							</asp:PlaceHolder>
						</tr>
					</ItemTemplate>
				</asp:Repeater>
				<asp:Literal runat="server" ID="uxNoPagesToEdit" Text="<tr><td>There are no pages for you to edit</td></tr>" Visible="false"></asp:Literal>
			</table>
		</ItemTemplate>
	</asp:Repeater>
	<div style="display: none">
		<div id="publishPopup">Publishing a microsite allows it to be viewable by non-admins. If a microsite is unpublished, only admins will be able to view it for editing purposes. You should not publish a microsite until you have updated each pages content for the
			microsite. </div>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		//<![CDATA[
		$(document).ready(function () {
			$(".help").fancybox({
				'width': 500,
				'height': 60,
				'padding': 10,
				'closeClick': false
			});
		});
		//]]>
	</script>
</asp:Content>
