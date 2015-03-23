<%@ Page Language="C#" MasterPageFile="~/microsite.master" AutoEventWireup="true" CodeFile="home-valuation.aspx.cs" Inherits="home_valuation" Title="Home Valuation Request" %>

<%@ Register Src="~/Controls/Contacts/ContactForm.ascx" TagPrefix="Contacts" TagName="ContactForm" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="contactForm fullForm">
		<div class="form">
			<div class="headerBG"><h1>Free Home Value Estimation</h1>			</div>
			<Contacts:ContactForm runat="server" ID="uxContactForm" ContactFormType="HomeValuationRequest" MessageFieldText="Comments" ShowAddressFields="true" />
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
