<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-showcases-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminShowcasesEdit" Title="Admin - Showcase Add/Edit" %>

<%@ Register TagPrefix="SEOComponent" TagName="CurrentPageSEO" Src="~/Controls/SEOComponent/SEO_Data_Entry.ascx" %>
<%@ Import Namespace="Classes.Showcase" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/jquery.flexbox.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<asp:PlaceHolder runat="server" ID="uxAdminOnlyPlaceHolder">
			<div class="blue padded optionsList">
				<ul class="inputList checkboxes horizontal">
					<li>
						<asp:CheckBox runat="server" ID="uxActive" Text="Is Active" Checked="true" /></li>
					<li>
						<asp:CheckBox runat="server" ID="uxMLSData" Text="Uses MLS Data" />
					</li>
				</ul>
				<div class="clear"></div>
			</div>
		</asp:PlaceHolder>
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%=uxTitle.ClientID%>">
					Title<span class="asterisk">*</span>
				</label>
				<asp:TextBox CssClass="text" runat="server" ID="uxTitle" MaxLength="50" />
				<asp:RegularExpressionValidator runat="server" ID="uxTitleRegexVal" ControlToValidate="uxTitle" ErrorMessage="Title is too long.  It must be 50 characters or less." ValidationExpression="^[\s\S]{0,50}$" />
				<asp:RequiredFieldValidator runat="server" ID="uxTitleReqFVal" ControlToValidate="uxTitle" ErrorMessage="Title is required." />
			</div>
			<asp:PlaceHolder runat="server" ID="uxManagedByPlaceHolder">
				<div class="formWhole"><span class="label">Managed By </span>
					<table class="listing">
						<thead>
							<tr>
								<th>
									Action
								</th>
								<th>
									Username
								</th>
							</tr>
						</thead>
						<tbody>
							<asp:Repeater runat="server" ID="uxMicrositeUsers" ItemType="Classes.Showcase.ShowcaseUser">
								<ItemTemplate>
									<tr>
										<td>
											<asp:LinkButton ID="uxDelete" runat="server" OnCommand="ManagersItem_Command" CommandName="Delete" CommandArgument='<%#Item.UserID%>' OnClientClick="return confirm('Are you sure you want to delete this showcase manager?');" CssClass="button delete"
												Text="<span>Delete</span>" />
										</td>
										<td>
											<%#Item.UserName%>
										</td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
							<tr>
								<td>
									<asp:Button ID="uxManagerAdd" runat="server" OnCommand="ManagersItem_Command" CommandName="Add" ToolTip="Add New" ValidationGroup="Users" Text="Add New" CssClass="button add" />
								</td>
								<td>
									<div id="uxUserList"></div>
									<asp:CustomValidator runat="server" ID="uxManagerAddReqVal" Text="<br /><span class='errorMessage'><span class='asterisk'>*</span><span class='errorText'>You must select a user.</span></span>" ErrorMessage="You must select a user." ValidationGroup="Users" />
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="uxShowcasePlaceHolder" Visible="false">
				<div class="formWhole">
					<asp:LinkButton runat="server" ID="uxEditShowcase" Text="<span>Modify this Showcase</span>" CssClass="button edit" CausesValidation="false"></asp:LinkButton>
				</div>
			</asp:PlaceHolder>
		</div>
		<div class="clear"></div>
		<SEOComponent:CurrentPageSEO ID="uxSEOData" runat="server" SitePageLinkSetupType="PageFormatter" PageLinkFormatter="~/showcase.aspx?showcaseid={0}" />
		<!-- button container -->
		<div class="buttons">
			<%--The markup for the buttons is in the BaseEditPage--%>
			<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
		</div>
	</asp:Panel>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery.flexbox.js"></asp:Literal>
	<script type="text/javascript">
		$(document).ready(function () {
			$("#uxUserList").flexbox('admin-showcases-edit.aspx?id=<%= EntityId %>', {
				resultTemplate: '<td>{name}</td><td>{email}</td>',
				watermark: 'Choose a User',
				header: '<table style="width: 100%" cellspacing="0" cellpadding="0"><tr><td>User Name</td><td>Email</td></tr>',
				footer: '</table>',
				width: 336,
				flexboxWidth: 430,
				useTable: true
			});
		});
	</script>
</asp:Content>
