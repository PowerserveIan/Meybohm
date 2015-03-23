<%@ Page Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="contact-us.aspx.cs" Inherits="contact_us" Title="Contact Us" %>

<%@ Register Src="~/Controls/Contacts/ContactForm.ascx" TagPrefix="Contacts" TagName="ContactForm" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<% if (micrositeEntity == null)
	{ %>
	<script type="text/javascript">
		var __lc = {};
		__lc.license = <%= System.Configuration.ConfigurationManager.AppSettings["LiveChat_LicenseNumber"] %>;
		(function () {
			var lc = document.createElement('script'); lc.type = 'text/javascript'; lc.async = true;
			lc.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'cdn.livechatinc.com/tracking.js';
			var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(lc, s);
		})();
	</script>
	<% } %>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="contactForm">
		<div class="contactPhone">
			<asp:PlaceHolder runat="server" ID="uxAugustaPH">
				<ul>
					<li>In Augusta</li>
					<li>
						<a href="tel:18032784663">(706) 738-HOME</a></li>
					<li><span>(4663)</span></li>
				</ul>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="uxAikenPH">
				<ul>
					<li>In Aiken</li>
					<li>
						<a href="tel:18036487653">(803) 648-SOLD</a></li>
					<li><span>(7653)</span></li>
				</ul>
			</asp:PlaceHolder>
			<ul>
				<li>Toll Free</li>
				<li>
					<a href="tel:18032784663">(800) 241-9726</a></li>
				<li><span></span></li>
			</ul>
		</div>
		<a class="allLocs" href='<%= ResolveClientUrl("~/") + MicrositePath + (!String.IsNullOrWhiteSpace(MicrositePath) ? "/" : "") + "offices" %>'>See All Meybohm Office Locations &raquo;</a>
		<div id="uxLiveChat" class="liveChat">
			<a href="http://www.livechatinc.com/?partner=lc_<%= System.Configuration.ConfigurationManager.AppSettings["LiveChat_LicenseNumber"] %>">Live Chat</a>
		</div>
		<div class="form">
			<h2>Contact Meybohm Today!</h2>
			<Contacts:ContactForm runat="server" ID="uxContactForm" />
			<div class="relatedResources">
				<ul>
					<li>Related Resources:</li>
					<li>
						<a href="relocation-guide">Relocation Guide</a></li>
					<li>
						<a href="offices">Meybohm Office Locations</a></li>
					<li>
						<a href="<%= micrositeEntity != null ? "" : "augusta/" %>staff">Agent Directory</a></li>
					<li>
						<a href="careers">Career Opportunities</a></li>
				</ul>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		$(document).ready(function () {
			$("body").addClass("theme-new");
		});
		<% if (micrositeEntity == null)
	 { %>
		
		var __lc_buttons = __lc_buttons || [];
		__lc_buttons.push({
			elementId: 'uxLiveChat',
			skill: '0'
		});
		<% } %>
	</script>
</asp:Content>
