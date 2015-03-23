<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProfileControl.ascx.cs" Inherits="Controls_BaseControls_ProfileControl" %>
<%@ Register TagPrefix="Controls" TagName="Address" Src="~/Controls/State_And_Country/Address.ascx" %>
<%@ Register TagPrefix="Controls" TagName="PhoneBox" Src="~/Controls/BaseControls/PhoneBoxControl.ascx" %>
<asp:ValidationSummary ID="uxProfileErrorSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" CssClass="validation" />
<asp:Label runat="server" CssClass="confirmationMessage" ID="uxSuccessMessage" Visible="false" Text="Profile updated successfully" />
<h3>Profile Info</h3>
<div class="formHalf">
	<label for="<%=uxFirstName.ClientID%>">
		First Name<em>*</em></label>
	<asp:TextBox ID="uxFirstName" runat="server" CssClass="text" MaxLength="255" />
	<asp:RequiredFieldValidator runat="server" ID="uxFirstNameRFV" ControlToValidate="uxFirstName" ErrorMessage="First Name is required." />
</div>
<div class="formHalf">
	<label for="<%=uxLastName.ClientID%>">
		Last Name<em>*</em></label>
	<asp:TextBox ID="uxLastName" runat="server" CssClass="text" MaxLength="255" />
	<asp:RequiredFieldValidator runat="server" ID="uxLastNameRFV" ControlToValidate="uxLastName" ErrorMessage="Last Name is required." />
</div>
<div class="formHalf">
	<label for="<%=uxPreferredCMMicrositeID.ClientID%>">
		Preferred Market<em>*</em></label>
	<asp:DropDownList runat="server" ID="uxPreferredCMMicrositeID" AppendDataBoundItems="true">
		<asp:ListItem Text="--Select a Market--" Value=""></asp:ListItem>
	</asp:DropDownList>
	<asp:RequiredFieldValidator runat="server" ID="uxPreferredCMMicrositeIDRFV" ControlToValidate="uxPreferredCMMicrositeID" ErrorMessage="Preferred Market is required." InitialValue="" />
</div>
<div class="formHalf">
	<label for="<%=uxPreferredLanguageID.ClientID%>">
		Preferred Language<em>*</em></label>
	<asp:DropDownList runat="server" ID="uxPreferredLanguageID" AppendDataBoundItems="true">
		<asp:ListItem Text="--Select a Language--" Value=""></asp:ListItem>
	</asp:DropDownList>
	<asp:RequiredFieldValidator runat="server" ID="uxPreferredLanguageIDRFV" ControlToValidate="uxPreferredLanguageID" ErrorMessage="Preferred Language is required." InitialValue="" />
</div>
<Controls:Address runat="server" ID="uxAddress" AddressLabel="Street 1" Address2Label="Street 2" ShowAddress2="true" ShowLatAndLong="false" />
<asp:PlaceHolder runat="server" ID="uxPhonePH">
	<div class="formHalf">
		<label for='<%=uxPhone.ClientID + "_uxPhoneBox_text"%>'>
			Phone</label>
		<Controls:PhoneBox runat="server" ID="uxPhone" Required="false" TextBoxClass="text" ShowExtension="true" />
	</div>
</asp:PlaceHolder>
<asp:LinkButton ID="uxSaveButton" CssClass="btnAdmin button" runat="server"><span>Update</span></asp:LinkButton>
<div class="clear"></div>
