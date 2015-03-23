<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Subscribe.ascx.cs" Inherits="Controls_Newsletters_Subscribe" %>
<div class="smallBox">
	<label>Receive updates and new listings from Meybohm</label>
	<asp:Panel ID="uxSubscribePH" runat="server" DefaultButton="uxSubmit">
		<div class="formWrapper newListings">
			<div class="validation">
				<asp:Literal runat="server" ID="uxEmailAlreadySubscribed" Visible="false"></asp:Literal>
			</div>
			<div class="formHalf">
				<asp:TextBox ID="uxEmail" runat="server" MaxLength="382" CssClass="text" />
				<asp:RequiredFieldValidator ID="uxEmailRequired" ControlToValidate="uxEmail" runat="server" ErrorMessage="Email is required." />
				<asp:RegularExpressionValidator runat="server" ID="uxEmailValidator" ErrorMessage="The email address entered is invalid." ControlToValidate="uxEmail" CssClass="errorText" />
			</div>
			<asp:Button runat="server" ID="uxSubmit" CssClass="button" Text="Send" OnClick="uxSubmit_Click" />
		</div>
	</asp:Panel>
<asp:PlaceHolder ID="uxResponseMessagePH" runat="server" Visible="false">

			<asp:Label runat="server" ID="uxResponseMessage" />

	</asp:PlaceHolder>
</div>
