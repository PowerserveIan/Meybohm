<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="lost-password.aspx.cs" Inherits="Lostpass" Title="Forgot Account Information" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="formMaster">
		<div class="formWrapper">
			<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="PasswordRecovery1" CssClass="validation" />
			<asp:Panel ID="uxUserInfoPanel" runat="server" DefaultButton="uxUserInfoSubmit">
				<h1>Forgot Your Account Information?</h1>
				<div class="formWhole">
					<asp:Label ID="uxEmailLabel" runat="server" AssociatedControlID="uxEmail">Email Address:<span class="asterisk">*</span></asp:Label><br />
					<asp:TextBox ID="uxEmail" runat="server" TabIndex="1" CssClass="text" />
					<asp:RequiredFieldValidator ID="uxEmailRequired" runat="server" ControlToValidate="uxEmail" ErrorMessage="Email Address is required." ValidationGroup="PasswordRecovery1" />
					<asp:RegularExpressionValidator runat="server" ID="uxEmailRegexVal" ControlToValidate="uxEmail" ErrorMessage="Email is an invalid email address." ValidationGroup="PasswordRecovery1" />
				</div>
				<div class="formWhole" style="color: red;">
					<asp:Literal ID="uxUserInfoFailureText" runat="server" EnableViewState="False" Visible="false"></asp:Literal>
				</div>
				<div class="formWhole">
					<asp:Button ID="uxUserInfoSubmit" runat="server" CommandName="Submit" ValidationGroup="PasswordRecovery1" Text="Get Account Info" CssClass="button" />
				</div>
			</asp:Panel>
			<asp:Panel ID="uxQuestionPanel" runat="server" DefaultButton="uxQuestionSubmit" Visible="false">
				<h1>Identity Confirmation</h1>
				<p>
					Answer the following question to receive your account information.
				</p>
				<p>
					<asp:Label ID="uxQuestionLabel" runat="server"><strong>Question: </strong></asp:Label>
					<asp:Label ID="uxQuestion" runat="server" />
				</p>
				<div class="formHalf">
					<asp:Label ID="uxAnswerLabel" runat="server" AssociatedControlID="uxAnswer">Answer:<span class="asterisk">*</span></asp:Label>
					<asp:TextBox runat="server" ID="uxAnswer" CssClass="text" TabIndex="1" MaxLength="50" />
					<asp:RequiredFieldValidator ID="uxAnswerRequired" runat="server" ControlToValidate="uxAnswer" ErrorMessage="Answer is required." Display="None" ValidationGroup="PasswordRecovery1" /></div>
				<div class="clear"></div>
				<div class="formWhole" style="color: red;">
					<asp:Literal ID="uxQuestionFailureText" runat="server" EnableViewState="False" Visible="false"></asp:Literal>
				</div>
				<asp:Button ID="uxQuestionSubmit" runat="server" CommandName="Submit" ValidationGroup="PasswordRecovery1" Text="Get Account Info" CssClass="inputBtnSubmitForm button" />
				<div class="clear"></div>
			</asp:Panel>
			<asp:PlaceHolder runat="server" ID="uxSuccessPH" Visible="false">
				<h1>Success</h1>
				<p>
					Instructions for changing your password were sent to
					<asp:Label runat="server" ID="uxUserNameLabelSuccess" Style="margin: 0;" />
				</p>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="uxStaffPH" Visible="false">Based on your email address, we have determined that you are an employee.  Please contact
				<asp:HyperLink runat="server" ID="uxStaffEmailAddress" Target="_blank"></asp:HyperLink>&nbsp;
				to reset/recover your password.
			</asp:PlaceHolder>
		</div>
		<!--end formWrapper-->
	</div>
	<!--end formMaster-->
</asp:Content>
