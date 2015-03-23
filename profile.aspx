<%@ Page Language="C#" MasterPageFile="~/microsite.master" AutoEventWireup="true" CodeFile="profile.aspx.cs" Inherits="profile" Title="Edit Profile" %>

<%@ Register Src="~/Controls/BaseControls/ProfileControl.ascx" TagPrefix="User" TagName="Profile" %>
<%@ Register Src="~/Controls/Media352_MembershipProvider/StaffProfile.ascx" TagPrefix="Staff" TagName="Profile" %>
<%@ Register Src="~/Controls/Media352_MembershipProvider/UserLoginInformation.ascx" TagPrefix="User" TagName="UserLoginInformation" %>

<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/members.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	
	<div class="formMaster">
			<h1>Edit Profile</h1>
			<asp:Label runat="server" CssClass="confirmationMessage" ID="uxSuccessMessage" Visible="false" Text="Profile updated successfully" />
			<User:UserLoginInformation runat="server" ID="uxUserLoginInformation" UserNameReadOnly="true" />
			<User:Profile ID="uxUserProfile" runat="server" UseProfileStandalone="false"  />
			<hr />
			<Staff:Profile ID="uxStaffProfile" runat="server" />
			<div class="clear"></div>
			<asp:Button ID="uxSave" CssClass="button save" runat="server" Text="Update" />
			<a href="saved-searches" class="floatRight manageSavedSearchLink">Manage my Saved Search and Cyber Alerts</a>
			<div class="clear"></div>
	</div>
	<!--end formMaster-->
</asp:Content>
