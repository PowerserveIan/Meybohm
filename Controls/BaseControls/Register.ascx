<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Register.ascx.cs" Inherits="Controls_BaseControls_Register" %>
<%@ Register Src="~/Controls/BaseControls/ProfileControl.ascx" TagPrefix="User" TagName="Profile" %>
<%@ Register Src="~/Controls/Media352_MembershipProvider/UserLoginInformation.ascx" TagPrefix="User" TagName="UserLoginInformation" %>
<%@ Reference Control="~/Controls/BaseControls/ProfileControl.ascx" %>
<asp:PlaceHolder runat="server" ID="uxStep1PH">
	<h1>Register with Meybohm REALTORs<sup>&reg;</sup></h1>
	<hr />
	<p>
		Registration with Meybohm REALTORs<sup>&reg;</sup> allows you to...
	</p>
	<ul class="regUl">
		<li>Save Searches</li>
		<li>Receive Email Alerts</li>
	</ul>
	<a class="button">Learn More &raquo;</a>
	<h2 class="fbLoginHeading">Connect with Facebook or Register below:</h2>
	<div class="fb-login-button" autologoutlink="true" data-show-faces="false" data-width="400" data-max-rows="1">Connect with Facebook</div>
	<br />
    <br />
	<asp:ValidationSummary ID="uxProfileErrorSummary" CssClass="validation" runat="server" ValidationGroup="CreateUser" DisplayMode="BulletList" ShowSummary="true" />
	<div class="requiredFields">* required fields</div>
	<h3>Registration Info</h3>
	<asp:Panel ID="uxLoginInfo" runat="server" DefaultButton="uxCreateUser">
		<asp:Label runat="server" CssClass="validation" EnableViewState="False" ID="uxErrorMessage" Visible="false" />
		<div class="clear"></div>
		<User:UserLoginInformation runat="server" ID="uxUserLoginInformation" ValidationGroup="CreateUser" />
        <br />
		<User:Profile ID="uxUserProfile" runat="server" UseProfileStandalone="false" UseCurrentLoggedInUser="false" ProfileValidationGroup="CreateUser" />
		<asp:Button runat="server" ID="uxCreateUser" CssClass="button largeButton" Text="Register" ValidationGroup="CreateUser" />
	</asp:Panel>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="uxStepCompletePH" Visible="false"><span class="confirmationMessage">Congratulations! Your account has been successfully created.</span>
	<p>
	In a few moments, you should receive an email with instructions on how to activate your account.</p>
</asp:PlaceHolder>
