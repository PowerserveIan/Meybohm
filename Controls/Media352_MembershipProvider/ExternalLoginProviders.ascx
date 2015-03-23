<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ExternalLoginProviders.ascx.cs" Inherits="Controls_BaseControls_ExternalLoginProviders" %>

<asp:Repeater runat="server" ID="uxProviders" ItemType="DotNetOpenAuth.AspNet.IAuthenticationClient">
	<HeaderTemplate>
		<div class="open-auth-providers">
	</HeaderTemplate>
	<ItemTemplate>
		<asp:LinkButton runat="server" ToolTip='<%# "Log in using your " + Item.ProviderName + " account." %>' CommandArgument="<%# Item.ProviderName %>" OnCommand="ExternalLogin_Command" CssClass="btn btn-block social-login">
			<i class="social-media-icon-<%# Item.ProviderName %>"></i><span>Log in with <%# Item.ProviderName %></span>
		</asp:LinkButton>
	</ItemTemplate>
	<FooterTemplate>
		</div>
	</FooterTemplate>
</asp:Repeater>
