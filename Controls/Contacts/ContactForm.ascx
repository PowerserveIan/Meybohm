<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContactForm.ascx.cs" Inherits="Controls_Contacts_ContactForm" %>
<%@ Register TagPrefix="Controls" TagName="Address" Src="~/Controls/State_And_Country/Address.ascx" %>
<%@ Register TagPrefix="Controls" TagName="PhoneBox" Src="~/Controls/BaseControls/PhoneBoxControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="SpamPrevention" Src="~/Controls/BaseControls/SpamPrevention.ascx" %>
<asp:Panel runat="server" ID="uxContactPH" DefaultButton="uxSubmit">
	<asp:PlaceHolder runat="server" ID="uxIntroTextPH">
		<p>
			Please fill out the following form and someone from our office will be in touch with you as soon as possible.
		</p>
		<hr />
		<div class="clear"></div>
		<h3>Your Information:</h3>
		<div class="clear"></div>
	</asp:PlaceHolder>
	<asp:ValidationSummary ID="uxContactErrorSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" CssClass="validation" HeaderText="<h3 class='failure'>Please correct the following errors:</h3>" ValidationGroup="ContactForm" />
	<div class="formHalf">
		<label for="<%= uxFirstName.ClientID %>">
			First Name<span class="asterisk">*</span>
		</label>
		<asp:TextBox runat="server" ID="uxFirstName" CssClass="text" MaxLength="255" />
		<asp:RequiredFieldValidator runat="server" ID="uxFirstNameReqFVal" ControlToValidate="uxFirstName" ErrorMessage="First name is required." ValidationGroup="ContactForm" />
		<asp:RegularExpressionValidator runat="server" ID="uxFirstNameREV" ControlToValidate="uxFirstName" ErrorMessage="First Name is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" ValidationGroup="ContactForm" />
	</div>
	<div class="formHalf">
		<label for="<%= uxLastName.ClientID %>">
			Last Name<span class="asterisk">*</span>
		</label>
		<asp:TextBox runat="server" ID="uxLastName" CssClass="text" MaxLength="255"  />
		<asp:RequiredFieldValidator runat="server" ID="uxLastNameReqFVal" ControlToValidate="uxLastName" ErrorMessage="Last name is required." ValidationGroup="ContactForm" />
		<asp:RegularExpressionValidator runat="server" ID="uxLastNameREV" ControlToValidate="uxLastName" ErrorMessage="Last Name is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" ValidationGroup="ContactForm" />
	</div>
	<Controls:Address runat="server" ID="uxAddress" AddressLabel="Property Address" Address2Label="Property Address 2" Required="true" ShowAddress2="true" ShowLatAndLong="false" />
	<div class="formWhole">
		<span class="charLimit">(1000 character limit)</span>
		<label for="<%= uxMessage.ClientID %>">
			<asp:Literal runat="server" ID="uxMessageFieldText" Text="Message"></asp:Literal><span class="asterisk">*</span></label>
		<div class="clear"></div>
		<asp:TextBox runat="server" ID="uxMessage" TextMode="MultiLine" CssClass="text" />
		<asp:RequiredFieldValidator runat="server" ID="uxMessageReqFVal" ControlToValidate="uxMessage" ErrorMessage="Message is required." ValidationGroup="ContactForm" />
		<asp:RegularExpressionValidator runat="server" ID="uxMessageREV" ControlToValidate="uxMessage" ErrorMessage="Message is too long.  It must be 1,000 characters or less." ValidationExpression="^[\s\S]{0,1000}$" ValidationGroup="ContactForm" />
	</div>
		<div class="formWhole" id="emailDiv" >
		<label for="<%= uxEmail.ClientID %>">
			Email<span class="asterisk">*</span>
		</label>
		<asp:TextBox runat="server" ID="uxEmail" CssClass="text" MaxLength="382"  />
		<asp:RequiredFieldValidator runat="server" ID="uxEmailReqFVal" ControlToValidate="uxEmail" ErrorMessage="Email is required." ValidationGroup="ContactForm" />
		<asp:RegularExpressionValidator runat="server" ID="uxEmailRegexVal" ControlToValidate="uxEmail" ErrorMessage="Email is an invalid email address." ValidationGroup="ContactForm" />
	</div>
	<div class="formHalf">
		<label for="<%= uxContactMethod.ClientID %>">
			How would you like us to contact you?<span class="asterisk">*</span>
		</label>
		<asp:DropDownList runat="server" ID="uxContactMethod" AppendDataBoundItems="true">
			<asp:ListItem Text="--Select Contact Method--" Value=""></asp:ListItem>
		</asp:DropDownList>
		<asp:RequiredFieldValidator runat="server" ID="uxContactMethodIdReqFVal" ControlToValidate="uxContactMethod" ErrorMessage="Contact Method is required." InitialValue="" ValidationGroup="ContactForm" />
	</div>

	<div class="formHalf" id="phoneDiv" style="display: none;">
		<label for='<%=uxPhone.ClientID + "_uxPhoneBox"%>'>
			Phone<span class="asterisk">*</span></label>
		<div class="formInternational formPhone">
			<Controls:PhoneBox runat="server" ID="uxPhone" Required="true" TextBoxClass="text" ShowExtension="true" ValidationGroup="ContactForm" />
		</div>
		<div class="clear"></div>
	</div>
	<div class="clear"></div>
	<div class="formWhole">
		<label for="<%= uxContactTime.ClientID %>" class="full">
			When would you like us to contact you?<span class="asterisk">*</span><br />
			<span></span>
		</label>
		<asp:RadioButtonList runat="server" ID="uxContactTime" RepeatLayout="UnorderedList" CssClass="list" />
		<asp:CustomValidator runat="server" ID="uxContactTimeRFV" ErrorMessage="Please tell us when you would like to be contacted." ClientValidationFunction="ValidateContactTime" ValidationGroup="ContactForm" />
		<div class="clear"></div>
	</div>
	<div class="formWholeBottom">
		<asp:Button runat="server" ID="uxSubmit" Text="Send" CssClass="button" ValidationGroup="ContactForm" />
		<Controls:SpamPrevention runat="server" ID="SpamPrevention" SubmitClientIDName="uxSubmit" />
		<div class="clear"></div>
	</div>
	<script type="text/javascript">
		$(document).ready(function () {
			$('#<%= uxContactMethod.ClientID %>').change(function (e) {
				if ($(this).val() == '<%= (int)Classes.Contacts.ContactMethods.Email %>') {
					$('#phoneDiv').hide();
					disablePhoneDivValidators(false);
				}
				else {
					$('#phoneDiv').show();
					disablePhoneDivValidators(true);
				}
			});

			

			function disablePhoneDivValidators(enable) {
				CustomValidatorEnable($("#<%= uxPhone.ClientID %>_uxPhoneBoxREV")[0], enable);
				CustomValidatorEnable($("#<%= uxPhone.ClientID %>_uxPhoneBoxRFV")[0], enable);
			}

			function CustomValidatorEnable(val, enable) {
				val.enabled = enable;
			}
		});

		function ValidateContactTime(sender, args) {
			args.IsValid = $("input[id*=uxContactTime]:checked").length > 0;
		}
	</script>
</asp:Panel>
<asp:Panel runat="server" ID="uxSuccessPH" Visible="false">
	<h2>Thank You!</h2>
	<p>
		We will be in contact with you as soon as possible. Thank you for contacting Meybohm REALTORs<sup>&reg;</sup>.
	</p>
</asp:Panel>
<% if (EnableClientSideSubmission)
   { %>
<script type="text/javascript">
	$(document).ready(function () {
		$("#<%= uxSubmit.ClientID %>").click(function () {
			if (Page_ClientValidate("ContactForm"))				
				ContactsWebMethods.SaveContactForm(<%= m_CurrentMicrositeID.HasValue ? m_CurrentMicrositeID.ToString() : "null" %>, $('#<%= uxContactMethod.ClientID %>').val(), $("input[id*=uxContactTime]:checked").val(), <%= (int)ContactFormType %>, $("#<%= uxEmail.ClientID %>").val(), $("#<%= uxFirstName.ClientID %>").val(), $("#<%= uxLastName.ClientID %>").val(), $("#<%= uxMessage.ClientID %>").val(), $("#<%=uxPhone.ClientID + "_uxPhoneBox"%>").val(), <%= ShowcaseItemID.HasValue ? ShowcaseItemID.ToString() : "null" %>, contact_success_method);
			return false;
		});

		function contact_success_method(results, userContext, methodName){
			$("#<%= uxContactPH.ClientID %>").hide();
			$("#<%= uxSuccessPH.ClientID %>").show();
		}
	});
</script>
<% } %>