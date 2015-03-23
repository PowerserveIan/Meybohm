<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-cm-microsite-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminCMMicrositeEdit" Title="Admin - Microsite Add/Edit" %>

<%@ Import Namespace="Classes.ContentManager" %>
<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="PhoneBox" Src="~/Controls/BaseControls/PhoneBoxControl.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/jquery.flexbox.css" id="uxCSSFiles" />
	<style type="text/css">
		.ie .formRightColumn span.ffb-arrow {
			left: 331px;
		}
	</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Is Active<span>Inactive Microsites will not show up on the front end or be accessible by Microsite Admins</span>" />
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%=uxName.ClientID%>">
					Name<span class="asterisk">*</span>
				</label>
				<asp:TextBox CssClass="text" ID="uxName" MaxLength="200" runat="server" />
				<asp:RegularExpressionValidator runat="server" ID="uxNameRegexVal" ControlToValidate="uxName" ErrorMessage="Only numbers, letters, underscore(_), and/or spaces are allowed in the Name field. Please ensure that the name chosen is less than 200 chars."
					ValidationExpression="^[\w\d\s_]{0,200}$" />
				<asp:RequiredFieldValidator runat="server" ID="uxNameReqFVal" ControlToValidate="uxName" ErrorMessage="Name is required." />
				<asp:CustomValidator runat="server" ID="uxNameUniqueCV" ControlToValidate="uxName" ErrorMessage="The name chosen is already in use. Please modify and resumbit." />
			</div>
			<div class="formHalf">
				<label for="<%=uxLocation.ClientID%>">
					Location
				</label>
				<asp:TextBox CssClass="text" ID="uxLocation" MaxLength="255" runat="server" />
			</div>
			<div class="formWhole">
				<label for="<%=uxDescription.ClientID%>">
					Description
				</label>
				<asp:TextBox CssClass="text" ID="uxDescription" runat="server" TextMode="MultiLine" />
				<asp:RegularExpressionValidator runat="server" ID="uxDescriptionRegexVal" ControlToValidate="uxDescription" ErrorMessage="Description is too long.  It must be 1000 characters or less." ValidationExpression="^[\s\S]{0,1000}$" />
			</div>
			<div class="formWhole"><span class="label" id="uxImageLabel">Image<span class="tooltip"><span>Must be of type .gif, .jpg, .jpeg, .png
				<br />
				<br />
				Suggested resizing tool:
				<a href="http://faststone.org/FSViewerDetail.htm" target="_blank">FastStone</a></span></span> <span>Recommended image size is 450x450</span> </span>
				<Controls:FileUpload runat="server" ID="uxImage" ImageWidth="450" ImageHeight="450" AllowedFileTypes=".gif,.jpg,.jpeg,.png" />
			</div>
			<div class="formHalf">
				<label for="<%= uxPhone.ClientID %>">
					Phone<span class="asterisk">*</span></label>
				<Controls:PhoneBox runat="server" ID="uxPhone" Required="True" TextBoxClass="text" />
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
							<asp:Repeater runat="server" ID="uxMicrositeUsers" ItemType="Classes.ContentManager.CMMicrositeUser">
								<ItemTemplate>
									<tr>
										<td>
											<asp:LinkButton ID="uxDelete" runat="server" OnCommand="ManagersItem_Command" CommandName="Delete" CommandArgument='<%#Item.UserID%>' OnClientClick="return confirm('Are you sure you want to remove this Manager?');" CssClass="button delete"
												Text="<span>Delete</span>" CausesValidation="false" />
										</td>
										<td>
											<%#Item.User.Name%>
										</td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
							<tr>
								<td>
									<asp:Button ID="uxManagerAdd" runat="server" OnCommand="ManagersItem_Command" CommandName="Add" ToolTip="Add New" ValidationGroup="Amounts" Text="Add New" CssClass="button add" />
								</td>
								<td>
									<div id="uxUserList"></div>
									<asp:CustomValidator runat="server" ID="uxManagerAddReqVal" Text="<br /><span class='errorMessage'><span class='asterisk'>*</span><span class='errorText'>You must select a user.</span></span>" ErrorMessage="You must select a user." ValidationGroup="Amounts" />
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
			$("#uxUserList").flexbox('admin-cm-microsite-edit.aspx?id=<%= EntityId %>&timestamp=<%=DateTime.Now.Ticks %>', {
				resultTemplate: '<td>{name}</td><td>{email}</td>',
				watermark: 'Choose a User',
				header: '<table style="width: 100%" cellspacing="0" cellpadding="0"><tr><td>User Name</td><td>Email</td></tr>',
				footer: '</table>',
				width: 346,
				flexboxWidth: 356,
				useTable: true
			});
		});
	</script>
</asp:Content>