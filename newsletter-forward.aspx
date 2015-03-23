<%@ Page Title="Forward Newsletter" Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="newsletter-forward.aspx.cs" Inherits="NewsletterForward" %>

<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/newsShared.css,~/css/newsletter.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<asp:ValidationSummary ID="uxValidationErrors" runat="server" CssClass="validation" />
	<asp:PlaceHolder ID="uxSendSuccess" runat="server" Visible="false">
		<p>
			The newsletter was sent successfully.</p>
	</asp:PlaceHolder>
	<asp:CustomValidator ID="uxSendFailure" runat="server" Text="<var></var>" ErrorMessage="An error occurred when sending the newsletter. Please try again later." />
	<table cellpadding="3" cellspacing="3" border="0">
		<tr>
			<td>
				Your email address *
			</td>
			<td>
				<asp:TextBox ID="uxFromEmail" runat="server" MaxLength="255" Width="250" />
				<asp:RequiredFieldValidator ID="uxFromEmailRequired" ControlToValidate="uxFromEmail" runat="server" ErrorMessage="Your email is required" Display="None" ForeColor="red" />
				<asp:RegularExpressionValidator runat="server" ID="uxFromEmailValidator" ErrorMessage="Your email address is invalid." ForeColor="red" Display="None" ControlToValidate="uxFromEmail" />
			</td>
		</tr>
		<tr>
			<td>
				Your name *
			</td>
			<td>
				<asp:TextBox ID="uxFromName" runat="server" MaxLength="255" Width="250" />
				<asp:RequiredFieldValidator ID="uxFromNameRequired" ControlToValidate="uxFromName" runat="server" ErrorMessage="Your name is required." Display="None" ForeColor="red" />
			</td>
		</tr>
		<tr>
			<td>
				Friend's email address *
			</td>
			<td>
				<asp:TextBox ID="uxToEmail" runat="server" MaxLength="255" Width="250" />
				<asp:RequiredFieldValidator ID="uxToEmailRequired" ControlToValidate="uxToEmail" runat="server" ErrorMessage="Friend's email is required." Display="None" ForeColor="red" />
				<asp:RegularExpressionValidator runat="server" ID="uxToEmailValidator" ErrorMessage="Friend's email address is invalid." ForeColor="red" Display="None" ControlToValidate="uxToEmail" />
			</td>
		</tr>
		<tr>
			<td>
				Friend's name *
			</td>
			<td>
				<asp:TextBox ID="uxToName" runat="server" MaxLength="255" Width="250" /><br />
				<asp:RequiredFieldValidator ID="uxToNameRequired" ControlToValidate="uxToName" runat="server" ErrorMessage="Friend's name is required." Display="None" ForeColor="red" />
			</td>
		</tr>
	</table>
	<asp:Button ID="uxSubmit" runat="server" Text="Forward Newsletter" CausesValidation="true" OnClick="uxSubmit_Click" /><br class="clear" />
	<div id="detailContainer">
		<asp:Literal ID="uxHtmlPreview" runat="server" />
	</div>
</asp:Content>
