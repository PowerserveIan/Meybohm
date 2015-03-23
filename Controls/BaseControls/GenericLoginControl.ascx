<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenericLoginControl.ascx.cs" Inherits="Controls_BaseControls_GenericLoginControl" %>
<%@ Register Src="~/Controls/Media352_MembershipProvider/ExternalLoginProviders.ascx" TagPrefix="Membership" TagName="ExternalLoginProviders" %>
<asp:Label runat="server" ID="uxAccessDeniedLabel" Text="You are not authorized to view that page, please login below.<br />" ForeColor="Red" Font-Bold="true" />
<asp:ValidationSummary ID="uxValidationSummary" runat="server" CssClass="errorBox validation" Font-Bold="true" HeaderText="<strong>Corrections are required.</strong><br />Please review them below and then send the form." DisplayMode="BulletList" ForeColor="Red" />
<asp:LoginView ID="uxLoginView" runat="server">
	<LoggedInTemplate>
		You are logged in as <asp:LoginName ID="uxLoginName" runat="server" />,
		<a href="logout.aspx">log out here</a>.
	</LoggedInTemplate>
	<AnonymousTemplate>
	</AnonymousTemplate>
</asp:LoginView>
<asp:Panel ID="uxLoginPanel" runat="server" DefaultButton="uxLogin$LoginButton" CssClass="loginContainer">
	<asp:Login ID="uxLogin" RenderOuterTable="false" runat="server">
		<LayoutTemplate>
			<div class="meybohmLogin">
				<div runat="server" id="uxUserNameDiv">
					<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
					<asp:TextBox ID="UserName" runat="server" CssClass="text" />
					<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." />
				</div>
				<div runat="server" id="uxPasswordDiv" style="padding-top: 8px;">
					<asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
					<asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="text" />
					<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." />
				</div>
				<div class="formWhole">
					<asp:CheckBox ID="RememberMe" runat="server" Text="Keep me logged in after my session ends" />
				</div>
				<asp:Label ID="FailureText" runat="server" EnableViewState="False" CssClass="failureText"></asp:Label>
				<asp:Button ID="LoginButton" runat="server" CssClass="button loginButton" CommandName="Login" Text="Log In" />
				<div style="display: block; width: 97%;" id="mbrArea">
					<asp:HyperLink ID="CreateUserLink" runat="server" Style="float: left;" NavigateUrl="~/register.aspx">Register Here</asp:HyperLink>
					<asp:HyperLink ID="PasswordRecoveryLink" runat="server" Style="float: right;" NavigateUrl="~/lost-password.aspx">Lost Password?</asp:HyperLink>
					<div class="clear"></div>
					<br />
					<Membership:ExternalLoginProviders runat="server" ID="uxExternalLoginProviders" />
					<p>One-click sign in if you've already connected your Facebook account, or quickly sign up for Meybohm with Facebook.</p>
				</div>
			</div>
		</LayoutTemplate>
	</asp:Login>
</asp:Panel>
