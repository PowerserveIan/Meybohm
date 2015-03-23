<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="change-password.aspx.cs" Inherits="changepass" Title="Change Password" %>

<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/members.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<div class="formWrapper">
		<asp:Label runat="server" ID="uxLinkExpired" Visible="false" Text="Your change password link has expired.  Please resubmit your <a href='lost-password.aspx'>lost password</a> request."></asp:Label>
		<asp:PlaceHolder runat="server" ID="uxStep1PH">
			<asp:ValidationSummary runat="server" ID="uxValidationSummary" ValidationGroup="ChangePassword" CssClass="validation" />
			<h1>Change Your Password </h1>
			<asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" Display="None" ErrorMessage="The Confirm New Password must match the New Password entry." ValidationGroup="ChangePassword" />
			<asp:Label ID="FailureText" runat="server" EnableViewState="False" ForeColor="Red" />
			<div class="clear"></div>
			<asp:PlaceHolder runat="server" ID="uxCurrentPasswordPH">
				<div class="formHalf">
					<asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Password:
						<asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword" ErrorMessage="Password is required." ValidationGroup="ChangePassword" />
					</asp:Label>
					<asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password" CssClass="text" MaxLength="14"/>
				</div>
				<div class="clear"></div>
			</asp:PlaceHolder>
			<div class="formHalf">
				<asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">New Password:
					<asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword" ErrorMessage="New Password is required." ValidationGroup="ChangePassword" />
					<asp:RegularExpressionValidator runat="server" ID="NewPasswordRegexVal" ControlToValidate="NewPassword" ErrorMessage="New Password must be between 6 and 14 characters long." ValidationGroup="ChangePassword" />
				</asp:Label>
				<asp:TextBox ID="NewPassword" runat="server" TextMode="Password" CssClass="text" MaxLength="14"/>
			</div>
			<div class="formHalf">
				<asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirm New Password:
					<asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword" ErrorMessage="Confirm New Password is required." ValidationGroup="ChangePassword" />
					<asp:RegularExpressionValidator runat="server" ID="ConfirmNewPasswordRegexVal" ControlToValidate="ConfirmNewPassword" ErrorMessage="Confirm New Password must be between 6 and 14 characters long." ValidationGroup="ChangePassword"
						 />
				</asp:Label>
				<asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password" CssClass="text" MaxLength="14"/>
			</div>
			<div class="formWhole">
				<asp:Button ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword" Text="Change Password" ValidationGroup="ChangePassword" />
				<asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
			</div>
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="uxSuccessPH" Visible="false">
			<h1>Change Password Complete</h1>
			<h2>Your password has been changed!</h2>
			<asp:HyperLink runat="server" ID="Continue" Text="Continue" CssClass="button" />
		</asp:PlaceHolder>
	</div>
	<!--end formWrapper-->
</asp:Content>
