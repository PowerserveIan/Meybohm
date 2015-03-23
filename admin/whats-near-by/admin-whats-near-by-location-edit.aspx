<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-whats-near-by-location-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminWhatsNearByLocationEdit" %>

<%@ Register TagPrefix="Controls" TagName="Address" Src="~/Controls/State_And_Country/Address.ascx" %>
<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="PhoneBox" Src="~/Controls/BaseControls/PhoneBoxControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="RichTextEditor" Src="~/Controls/BaseControls/RichTextEditor.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Active" />
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%= uxName.ClientID %>">
					Name<span class="asterisk">*</span><br />
					<span>Must be less than 255 characters</span></label>
				<asp:TextBox runat="server" ID="uxName" MaxLength="255" CssClass="text" />
				<asp:RequiredFieldValidator runat="server" ID="uxNameReqFVal" ControlToValidate="uxName" ErrorMessage="Name is required." />
				<asp:RegularExpressionValidator runat="server" ID="uxNameRegexVal" ControlToValidate="uxName" ErrorMessage="Name is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
			</div>
			<div class="formWhole">
				<label for="<%= uxDescription.ClientID %>">
					Description</label>
				<Controls:RichTextEditor runat="server" ID="uxDescription" FieldName="Description" />
			</div>
			<Controls:Address runat="server" ID="uxAddress" Required="true" />
			<div class="formWhole">
				<label for="<%= uxImage.ClientID %>">
					Image</label>
				<Controls:FileUpload runat="server" ID="uxImage" Required="False" AllowExternalImageLink="true" UploadToLocation="~/uploads/nearByLocations" />
			</div>
			<div class="formHalf">
				<label for="<%= uxPhone.ClientID %>_uxPhoneBox">
					Phone</label>
				<Controls:PhoneBox runat="server" ID="uxPhone" Required="False" TextBoxClass="text" />
			</div>
			<div class="formHalf">
				<label for="<%= uxWebsite.ClientID %>">
					Website</label>
				<asp:TextBox runat="server" ID="uxWebsite" CssClass="text" />
				<asp:RegularExpressionValidator runat="server" ID="uxWebsiteRegexVal" ControlToValidate="uxWebsite" ErrorMessage="Website is too long.  It must be 1000 characters or less." ValidationExpression="^[\s\S]{0,1000}$" />
			</div>
		</div>
		<div class="rightCol">
			<div class="formHalf"><span class="label">Categories<span class="asterisk">*</span></span>
				<asp:CheckBoxList ID="uxCategory" runat="server" CssClass="inputList checkboxes" RepeatLayout="UnorderedList" />
				<asp:CustomValidator runat="server" ID="uxCategoryRequired" OnServerValidate="uxCategory_ServerValidate" ErrorMessage="You must select at least one category." ClientValidationFunction="ValidateCategories" />
			</div>
		</div>
		<div class="clear"></div>
		<!-- button container -->
		<div class="buttons">
			<%--The markup for the buttons is in the BaseEditPage--%>
			<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
		</div>
	</asp:Panel>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		function ValidateCategories(source, args) {
			args.IsValid = $("input[id*=uxCategory]:checked").length > 0;
		}
	</script>
</asp:Content>