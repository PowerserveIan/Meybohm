<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true"
	CodeFile="newsletter-unsubscribe.aspx.cs" Inherits="NewsletterUnsubscribe" Title="Newsletter - Unsubscribe" %>

<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/newsShared.css,~/css/newsletter.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<asp:PlaceHolder ID="uxSubscribePH" runat="server">
		<asp:ValidationSummary ID="uxValidationSummary1" runat="server" CssClass="validation" />
		<p>
			To unsubscribe from our newsletter(s), follow the instructions below. Unsubscribing
			will remove your email from our mailing lists and you will no longer receive the
			newsletter(s) you unsubscribe from.
		</p>
		<p>
			Enter the email address you used to subscribe to newsletter(s)*
			<asp:TextBox ID="uxEmail" runat="server" MaxLength="382"/>
			<asp:RequiredFieldValidator ID="uxEmailRequired" ControlToValidate="uxEmail" runat="server"
				ErrorMessage="Email is required." Display="None" ForeColor="Red" />
			<asp:RegularExpressionValidator runat="server" ID="uxEmailValidator" ErrorMessage="<br />The email address entered is invalid."
				ForeColor="Red" Display="None" ControlToValidate="uxEmail" />
		</p>
		<asp:PlaceHolder runat="server" ID="uxMailingListsPH">
			<p>
				Choose the newsletters you'd like to unsubscribe from*
				<asp:CheckBoxList ID="uxMailingList" runat="server">
				</asp:CheckBoxList>
				<asp:CustomValidator ID="uxMailingListRequired" runat="server" ErrorMessage="You must select a mailing list."
					ForeColor="Red" Display="None" />
			</p>
		</asp:PlaceHolder>
		<p>
			<asp:Button runat="server" ID="uxSubmit" Text="Unsubscribe" OnClick="uxSubmit_Click" />
			<asp:Button runat="server" ID="uxCancel" Text="Cancel" OnClick="uxCancel_Click" CausesValidation="false" />
		</p>
	</asp:PlaceHolder>
	<asp:PlaceHolder ID="uxResponseMessagePH" runat="server" Visible="false">
		<p>
			<asp:Label runat="server" ID="uxResponseMessage"/>
			<br />
			<asp:HyperLink runat="server" ID="uxReturnToListing" NavigateUrl="~/newsletter.aspx">Return to Newsletters</asp:HyperLink>
		</p>
	</asp:PlaceHolder>
</asp:Content>
