<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="approval-alerts.aspx.cs" Inherits="Admin_ContentManager_ApprovalAlerts" Title="Admin - Manage Approvals" %>

<%@ Import Namespace="Classes.ContentManager" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<style type="text/css">
		.deleted {
			color: Red;
			text-decoration: line-through !important;
		}

		.restored {
			color: Green;
		}
	</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<div class="title">
		<h1>CMS Approval Alerts</h1>
	</div>
	<div class="contentPadding">
		<p>
			<span style="color: #069;">Blue</span> means the page has been edited.<br />
			<span class="deleted">Strike-through</span> means the page has been deleted. Approving will remove the page from view on the frontend.<br />
			<span class="restored">Green</span> means an Editor has restored a page that was previously deleted.<br />
		</p>
		<div class="selectAllDiv">
			<asp:Label runat="server" ID="uxNothingNeedingApproval" Text="Nothing currently requires approval" Visible="false" />
			<asp:CheckBox runat="server" ID="uxSelectAll" Text="Select/Deselect All" />
		</div>
		<asp:Repeater runat="server" ID="uxUnapprovedGlobalAreas" ItemType="Classes.ContentManager.CMPageRegion">
			<HeaderTemplate>
				<div class="formWrapper">
					<h2>Unapproved Global Areas</h2>
					<em>The following sitewide content areas have unapproved changes</em><br />
					<table class="contentApproval">
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
						<asp:CheckBox runat="server" ID="uxCheckBox" />
						<asp:HiddenField runat="server" ID="uxRegionID" Value="<%#Item.CMPageRegionID%>" />
					</td>
					<td>
						<asp:HyperLink ID="uxGlobalAreaLink" runat="server" Target="_blank" NavigateUrl='<%#"~/" + (CMPage.GetByID(Item.CMPageID).CMMicrositeID.HasValue ? CMMicrosite.GetByID(CMPage.GetByID(Item.CMPageID).CMMicrositeID.Value).Name.Replace(" ", "-") + "/" : "") + CMPage.GetByID(Item.GlobalAreaCMPageID.Value).FileName%>'> <%#CMPage.GetByID(Item.CMPageID).Title%></asp:HyperLink>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</table> </div>
			</FooterTemplate>
		</asp:Repeater>
		<asp:Repeater runat="server" ID="uxSiteSectionRepeater">
			<ItemTemplate>
				<h2>
					<asp:Literal runat="server" ID="uxSectionLabel"></asp:Literal></h2>
				<asp:Repeater runat="server" ID="uxContentRepeater" ItemType="Classes.ContentManager.CMPage">
					<HeaderTemplate>
						<h4>Content Approval</h4>
						<table class="contentApproval">
					</HeaderTemplate>
					<ItemTemplate>
						<tr>
							<td>
								<asp:CheckBox runat="server" ID="uxCheckBox" />
								<asp:HiddenField runat="server" ID="uxPageID" Value="<%#Item.CMPageID%>" />
								<asp:HiddenField runat="server" ID="uxLanguage" Value="<%#!String.IsNullOrEmpty(Item.CultureName) ? Item.CultureName : Classes.SiteLanguages.Settings.DefaultLanguageCulture%>" />
							</td>
							<td>
								<asp:HyperLink runat="server" ID="uxContentLink" Target="_blank" NavigateUrl='<%#"~/" + (Item.CMMicrositeID.HasValue ? CMMicrosite.GetByID(Item.CMMicrositeID.Value).Name.Replace(" ", "-") + "/" : "") + Item.FileName + (!String.IsNullOrEmpty(Item.CultureName) || Settings.EnableMultipleLanguages ? "?language=" + (!String.IsNullOrEmpty(Item.CultureName) ? Item.CultureName : Classes.SiteLanguages.Settings.DefaultLanguageCulture) : "")%>'> <%#!String.IsNullOrEmpty(Item.CMPageTitleTitle) ? Item.CMPageTitleTitle : Item.Title%></asp:HyperLink>
							</td>
						</tr>
					</ItemTemplate>
					<FooterTemplate>
						</table>
					</FooterTemplate>
				</asp:Repeater>
				<asp:Repeater runat="server" ID="uxPropertiesRepeater" ItemType="Classes.ContentManager.CMPage">
					<HeaderTemplate>
						<div class="formHalfContentTitle">
							<h4>Page Approval</h4>
						</div>
						<table class="contentApproval">
					</HeaderTemplate>
					<ItemTemplate>
						<tr>
							<td>
								<asp:CheckBox runat="server" ID="uxCheckBox" />
								<asp:HiddenField runat="server" ID="uxPageID" Value="<%#Item.CMPageID%>" />
								<asp:HiddenField runat="server" ID="uxLanguage" Value="<%#!String.IsNullOrEmpty(Item.CultureName) ? Item.CultureName : Classes.SiteLanguages.Settings.DefaultLanguageCulture%>" />
							</td>
							<td>
								<asp:HyperLink runat="server" ID="uxPagePropertiesLink" Target="_blank" CssClass='<%#Item.EditorDeleted.HasValue ? (Item.EditorDeleted.Value ? "deleted" : "restored") : ""%>' NavigateUrl='<%#"~/admin/content-manager/content-manager-page.aspx?id=" + Item.CMPageID + (!String.IsNullOrEmpty(Item.CultureName) || Settings.EnableMultipleLanguages ? "&amp;language=" + (!String.IsNullOrEmpty(Item.CultureName) ? Item.CultureName : Classes.SiteLanguages.Settings.DefaultLanguageCulture) : "")%>'> <%#!String.IsNullOrEmpty(Item.CMPageTitleTitle) ? Item.CMPageTitleTitle : Item.Title%></asp:HyperLink>
							</td>
						</tr>
					</ItemTemplate>
					<FooterTemplate>
						</table>
					</FooterTemplate>
				</asp:Repeater>
				<asp:Repeater runat="server" ID="uxSiteMapRepeater" ItemType="Classes.ContentManager.SMItem">
					<HeaderTemplate>
						<div class="formHalfContentTitle">
							<h4>Sitemap Approval</h4>
						</div>
						<table class="contentApproval">
					</HeaderTemplate>
					<ItemTemplate>
						<tr>
							<td>
								<asp:CheckBox runat="server" ID="uxCheckBox" />
								<asp:HiddenField runat="server" ID="uxMicrositeID" Value="<%#Convert.ToInt32(((RepeaterItem)(Container.Parent).Parent).DataItem)%>" />
								<asp:HiddenField runat="server" ID="uxLanguage" Value="<%#!String.IsNullOrEmpty(Item.CultureName) ? Item.CultureName : Classes.SiteLanguages.Settings.DefaultLanguageCulture%>" />
							</td>
							<td>
								<asp:HyperLink runat="server" ID="uxSiteMapApprovalLink" Target="_blank" Text='<%#(String.IsNullOrEmpty(Item.Culture) ? "" : Item.Culture + " ") + "Sitemap"%>' NavigateUrl='<%#"~/admin/content-manager/sitemap.aspx" + ((((RepeaterItem)(Container.Parent).Parent).ItemType == ListItemType.Item || ((RepeaterItem)(Container.Parent).Parent).ItemType == ListItemType.AlternatingItem) && Convert.ToInt32(((RepeaterItem)(Container.Parent).Parent).DataItem) == 0 ? "" : "?micrositeid=" + Convert.ToInt32(((RepeaterItem)(Container.Parent).Parent).DataItem)) + (!String.IsNullOrEmpty(Item.CultureName) || Settings.EnableMultipleLanguages ? ((((RepeaterItem)(Container.Parent).Parent).ItemType == ListItemType.Item || ((RepeaterItem)(Container.Parent).Parent).ItemType == ListItemType.AlternatingItem) && Convert.ToInt32(((RepeaterItem)(Container.Parent).Parent).DataItem) == 0 ? "?" : "&amp;") + "language=" + (String.IsNullOrEmpty(Item.CultureName) ? Classes.SiteLanguages.Settings.DefaultLanguageCulture : Item.CultureName) : "")%>'></asp:HyperLink>
							</td>
						</tr>
					</ItemTemplate>
					<FooterTemplate>
						</table>
					</FooterTemplate>
				</asp:Repeater>
			</ItemTemplate>
		</asp:Repeater>
	</div>
	<div class="buttons">
		<asp:Button runat="server" ID="uxApprove" Text="Approve All Selected Changes" OnCommand="Approval_Command" CommandName="Approve" CssClass="button approve" />
		<asp:Button runat="server" ID="uxDeny" Text="Deny All Selected Changes" OnCommand="Approval_Command" CommandName="Deny" CssClass="button delete" />
		<asp:CustomValidator runat="server" ID="uxSelectOneValidator" Text="You should select at least one changed item" OnServerValidate="uxSelectOneValidator_ServerValidate" ClientValidationFunction="SelectOne"></asp:CustomValidator>
		<div class="clear"></div>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		//<![CDATA[
		function SelectOne(sender, args) {
			args.IsValid = $("input[id$=uxCheckBox]:checked").length > 0;
		}

		$(document).ready(function () {
			$("#<%=uxSelectAll.ClientID%>").click(function () {
				$("input[id$=uxCheckBox]").attr("checked", $(this).is(":checked"));
			});
		});
		//]]>
	</script>
</asp:Content>
