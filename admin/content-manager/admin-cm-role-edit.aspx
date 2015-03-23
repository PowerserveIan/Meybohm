<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-cm-role-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminCMRoleEdit" Title="Admin - CM Role Add/Edit" %>

<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/jquery.flexbox.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%=uxName.ClientID%>">
					Name<span class="asterisk">*</span>
				</label>
				<asp:TextBox CssClass="text" ID="uxName" MaxLength="50" runat="server" />
				<asp:RegularExpressionValidator runat="server" ID="uxNameRegexVal" ControlToValidate="uxName"
					ErrorMessage="Only numbers, letters, underscore(_), and/or spaces are allowed in the Name field. Please ensure that the name chosen is less than 200 chars." ValidationExpression="^[\w\d\s_]{0,50}$" />
				<asp:RequiredFieldValidator runat="server" ID="uxNameReqFVal" ControlToValidate="uxName" ErrorMessage="Name is required." />
				<asp:CustomValidator runat="server" ID="uxNameUniqueCV" ControlToValidate="uxName" ErrorMessage="A role already exists by that name, please choose another." />
			</div>
			<asp:PlaceHolder runat="server" ID="uxUsersInRolePlaceHolder">
				<div class="formWhole"><span class="label">Users in Role</span>
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
							<asp:Repeater runat="server" ID="uxUsersInRole" ItemType="Classes.Media352_MembershipProvider.UserRole">
								<ItemTemplate>
									<tr>
										<td>
											<asp:LinkButton ID="uxDelete" runat="server" OnCommand="UserRoleItem_Command" CommandName="Delete" CommandArgument='<%#Item.UserID%>' CausesValidation="false" OnClientClick="return confirm('Are you sure you want to remove this user from the role?');" CssClass="button delete"
												Text="<span>Delete</span>" />
										</td>
										<td>
											<%#Item.User.Name%>
										</td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
							<tr>
								<td>
									<asp:Button ID="uxAddUserToRole" runat="server" OnCommand="UserRoleItem_Command" CommandName="Add" ToolTip="Add New" ValidationGroup="Amounts" Text="Add New" CssClass="button add" />
								</td>
								<td>
									<div id="uxUserList"></div>
									<asp:CustomValidator runat="server" ID="uxAddUserToRoleReqVal" Text="<br /><span class='errorMessage'><span class='asterisk'>*</span><span class='errorText'>You must select a user.</span></span>" ErrorMessage="You must select a user." />
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</asp:PlaceHolder>
		</div>
		<div class="clear"></div>
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
			$("#uxUserList").flexbox('admin-cm-role-edit.aspx?id=<%= RoleEntity.RoleID %>&timestamp=<%=DateTime.Now.Ticks %>', {
				resultTemplate: '<td>{name}</td><td>{email}</td>',
				watermark: 'Choose a User',
				width: 346,
				flexboxWidth: 356,
				header: '<table style="width: 100%" cellspacing="0" cellpadding="0"><tr><td>User Name</td><td>Email</td></tr>',
				footer: '</table>',
				useTable: true
			});
		});
	</script>
</asp:Content>
