<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tft-user.aspx.cs" Inherits="tft_user" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
	<link rel="SHORTCUT ICON" href="//www.352media.com/favicon.ico" />
	<title>352 Login as User</title>
</head>
<body>
	<form id="form1" runat="server">
	<h1>Click one of the buttons below to login as that user</h1>
	<asp:Label runat="server" ID="uxCurrentlyLoggedInAs"></asp:Label><br />
	<asp:Repeater runat="server" ID="uxUsers">
		<ItemTemplate>
			<asp:Button runat="server" CommandArgument="<%# ((Classes.Media352_MembershipProvider.User)Container.DataItem).Name %>" Text='<%# "Log in as " + ((Classes.Media352_MembershipProvider.User)Container.DataItem).Name %>' OnCommand="Login_Command" CssClass="button"
				Visible="<%# !((Classes.Media352_MembershipProvider.User)Container.DataItem).Name.Equals(Page.User.Identity.Name, StringComparison.OrdinalIgnoreCase) %>" /><br />
		</ItemTemplate>
	</asp:Repeater>
	</form>
</body>
</html>
