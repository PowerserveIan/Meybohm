<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-security-question-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminSecurityQuestionEdit" Title="Admin - Security Question Add/Edit" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Is Active" />
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formWhole">
				<label for="<%=uxQuestion.ClientID%>">
					Question<span class="asterisk">*</span> <span>Must be less than 50 characters</span></label>
				<asp:TextBox runat="server" ID="uxQuestion" TextMode="MultiLine" MaxLength="50" />
				<asp:RegularExpressionValidator runat="server" ID="uxQuestionRegexVal" ControlToValidate="uxQuestion" ErrorMessage="Question is too long.  It must be 50 characters or less." ValidationExpression="^[\s\S]{0,50}$" />
				<asp:RequiredFieldValidator runat="server" ID="uxQuestionReqFVal" ControlToValidate="uxQuestion" ErrorMessage="Question is required." />
				<asp:CustomValidator ID="uxQuestionCV" runat="server" ControlToValidate="uxQuestion" ErrorMessage="Question already exists." />
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
