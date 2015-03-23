<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-team-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminTeamEdit" %>

<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<asp:CheckBox runat="server" ID="uxDisplayInDirectory" Text="Show in Staff/Agent Directory" />
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%= uxMLSID.ClientID %>">MLS ID</label>
				<asp:TextBox runat="server" ID="uxMLSID" CssClass="text" ReadOnly="true" />
			</div>
			<div class="clear"></div>
			<div class="formHalf">
				<label for="<%= uxName.ClientID %>">
					Name<span class="asterisk">*</span><br />
					<span>Must be less than 255 characters</span></label>
				<asp:TextBox runat="server" ID="uxName" MaxLength="255" CssClass="text" />
				<asp:RequiredFieldValidator runat="server" ID="uxNameReqFVal" ControlToValidate="uxName" ErrorMessage="Name is required." />
				<asp:RegularExpressionValidator runat="server" ID="uxNameRegexVal" ControlToValidate="uxName" ErrorMessage="Name is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
			</div>
			<div class="formHalf">
				<label for="<%=uxEmail.ClientID%>">
					Email<span class="asterisk">*</span>
				</label>
				<asp:TextBox CssClass="text" runat="server" ID="uxEmail" MaxLength="382" />
				<asp:RequiredFieldValidator runat="server" ID="uxEmailReqFVal" ControlToValidate="uxEmail" ErrorMessage="Email is required." />
				<asp:RegularExpressionValidator runat="server" ID="uxEmailRegexVal2" ControlToValidate="uxEmail" ErrorMessage="Email is an invalid email address." />
			</div>
			<div class="clear"></div>
			<div class="formHalf">
				<span class="label">Additional Languages Spoken<br />
					<em>(all Agents speak English)</em></span>
				<asp:CheckBoxList runat="server" ID="uxLanguages" CssClass="inputList checkboxes"></asp:CheckBoxList>
			</div>
			<div class="formHalf">
				<label for="<%=uxCMMicrositeID.ClientID%>">
					Market<em>*</em></label>
				<asp:DropDownList runat="server" ID="uxCMMicrositeID" AppendDataBoundItems="true">
					<asp:ListItem Text="--Select a Market--" Value=""></asp:ListItem>
				</asp:DropDownList>
				<asp:RequiredFieldValidator runat="server" ID="uxCMMicrositeIDRFV" ControlToValidate="uxCMMicrositeID" ErrorMessage="Market is required." InitialValue="" />
			</div>
			<div class="formWhole">
				<label for="<%= uxPhoto.ClientID %>">
					Team Photo</label>
				<Controls:FileUpload runat="server" ID="uxPhoto" AllowedFileTypes=".gif,.jpg,.jpeg,.png" ImageHeight="134" ImageWidth="102" Required="False" UploadToLocation="~/uploads/agents" />
			</div>
			<asp:PlaceHolder runat="server" ID="uxAfterSavePH" Visible="false">
				<table class="listing paddingBottom" id="users">
					<thead>
						<tr>
							<th>Action</th>
							<th>User Name</th>
						</tr>
					</thead>
					<tbody>
						<asp:Repeater runat="server" ID="uxUsers" ItemType="Classes.Media352_MembershipProvider.UserTeam">
							<ItemTemplate>
								<tr>
									<td>
										<asp:LinkButton ID="uxDelete" runat="server" OnCommand="Team_Command" CommandName="Delete" CommandArgument='<%#Item.UserTeamID%>' OnClientClick="return confirm('Are you sure you want to remove the user from this team?');" CssClass="button delete"
											Text="<span>Delete</span>" CausesValidation="false" />
									</td>
									<td><%# Item.User.Name %></td>
								</tr>
							</ItemTemplate>
						</asp:Repeater>
						<tr>
							<td>
								<asp:LinkButton runat="server" CssClass="button add" Text="<span>Add</span>" OnCommand="Team_Command" CommandName="Add" ValidationGroup="AddTeam" />
							</td>
							<td>
								<asp:DropDownList runat="server" ID="uxUser" AppendDataBoundItems="true">
									<asp:ListItem Text="--Select a User--" Value=""></asp:ListItem>
								</asp:DropDownList>
								<asp:RequiredFieldValidator runat="server" ID="uxUserRFV" ControlToValidate="uxUser" ErrorMessage="You must select a user." InitialValue="" ValidationGroup="AddTeam" />
							</td>
						</tr>
					</tbody>
				</table>
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
