<%@ Page Language="C#" MasterPageFile="~/microsite.master" AutoEventWireup="true" CodeFile="maintenance-request.aspx.cs" Inherits="maintenance_request" Title="Maintenance Request" %>

<%@ Register Src="~/Controls/Contacts/ContactForm.ascx" TagPrefix="Contacts" TagName="ContactForm" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="contactForm fullForm">
		<div class="form">
			<div class="headerBG"><h1>Maintenance Request</h1></div>		
			<Contacts:ContactForm runat="server" ID="uxContactForm" ContactFormType="MaintenanceRequest" MessageFieldText="Please describe the maintenance requested" ShowAddressFields="true" />
		</div>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		$(document).ready(function () {
			$("body").addClass("theme-new");
		});
	</script>
</asp:Content>
