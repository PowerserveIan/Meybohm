<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LanguageToggle.ascx.cs" Inherits="Controls_BaseControls_LanguageToggle" ViewStateMode="Enabled" %>
<%@ Import Namespace="System.Threading" %>
<asp:Repeater runat="server" ID="uxLanguages" ItemType="Classes.SiteLanguages.Language">
	<HeaderTemplate>
		<ul class="langToggle">
	</HeaderTemplate>
	<ItemTemplate>
		<li class='<%#Item.CultureName + (Item.CultureName == Thread.CurrentThread.CurrentCulture.Name ? " currentLanguage" : "")%>'>
			<asp:PlaceHolder runat="server" Visible="<%#Item.CultureName != Thread.CurrentThread.CurrentCulture.Name%>">
				<a href='<%# "?language=" + Item.CultureName %>'>
					<span><%#Item.Culture%></span>
				</a>
			</asp:PlaceHolder>
			<asp:Label runat="server" ID="uxCurrentLanguage" Text="<%#Item.Culture%>" Visible="<%#Item.CultureName == Thread.CurrentThread.CurrentCulture.Name%>" />
		</li>
	</ItemTemplate>
	<FooterTemplate>
		</ul>
	</FooterTemplate>
</asp:Repeater>
