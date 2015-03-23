<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-news-press-category-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminNewsPressCategoryEdit" Title="Admin - Newspress Category Add/Edit" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Is Active" />
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%=uxName.ClientID%>">
					Name<span class="asterisk">*</span>
				</label>
				<asp:TextBox CssClass="text" ID="uxName" MaxLength="100" runat="server" />
				<asp:RequiredFieldValidator runat="server" ID="uxNameReqFVal" ControlToValidate="uxName" ErrorMessage="Name is required." />
				<asp:RegularExpressionValidator runat="server" ID="uxNameRegexVal" ControlToValidate="uxName" ErrorMessage="Name is too long.  It must be 100 characters or less." ValidationExpression="^[\s\S]{0,100}$" />
				<asp:CustomValidator ID="uxNameUniqueValidator" runat="server" ControlToValidate="uxName" ErrorMessage="Category name is already in use, please choose another." />
			</div>
		</div>
		<div class="clear"></div>
		<!-- button container -->
		<div class="buttons">
			<%--The markup for the buttons is in the BaseEditPage--%>
			<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
		</div>
	</asp:Panel>
</asp:Content>
