<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-office-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminOfficeEdit" %>

<%@ Register TagPrefix="Controls" TagName="Address" Src="~/Controls/State_And_Country/Address.ascx" %>
<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="PhoneBox" Src="~/Controls/BaseControls/PhoneBoxControl.ascx" %>
<%@ Register TagPrefix="SEOComponent" TagName="CurrentPageSEO" Src="~/Controls/SEOComponent/SEO_Data_Entry.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<ul class="inputList checkboxes horizontal">
				<li>
					<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Active" /></li>
				<li>
					<asp:CheckBox runat="server" ID="uxHasNewHomes" Text="Has New Homes" /></li>
				<li>
					<asp:CheckBox runat="server" ID="uxHasRentals" Text="Has Rentals" /></li>
			</ul>
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%= uxMlsID.ClientID %>">
					MLS ID<span class="asterisk">*</span><br />
					<span>Must be less than 50 characters</span></label>
				<asp:TextBox runat="server" ID="uxMlsID" MaxLength="50" CssClass="text" />
				<asp:RequiredFieldValidator runat="server" ID="uxMlsIDReqFVal" ControlToValidate="uxMlsID" ErrorMessage="MLS ID is required." />
				<asp:RegularExpressionValidator runat="server" ID="uxMlsIDRegexVal" ControlToValidate="uxMlsID" ErrorMessage="MLS ID is too long.  It must be 50 characters or less." ValidationExpression="^[\s\S]{0,50}$" />
			</div>
			<div class="formHalf">
				<label for="<%= uxName.ClientID %>">
					Name<span class="asterisk">*</span><br />
					<span>Must be less than 255 characters</span></label>
				<asp:TextBox runat="server" ID="uxName" MaxLength="255" CssClass="text" />
				<asp:RequiredFieldValidator runat="server" ID="uxNameReqFVal" ControlToValidate="uxName" ErrorMessage="Name is required." />
				<asp:RegularExpressionValidator runat="server" ID="uxNameRegexVal" ControlToValidate="uxName" ErrorMessage="Name is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
			</div>
			<Controls:Address runat="server" ID="uxAddress" Required="true" AutoCalculateCoordinates="true" />
			<div class="formWhole">
				<label for="<%= uxImage.ClientID %>">
					Image</label>
				<Controls:FileUpload runat="server" ID="uxImage" Required="False" UploadToLocation="~/uploads/offices" />
			</div>
			<div class="formHalf">
				<label for="<%= uxPhone.ClientID %>">
					Phone<span class="asterisk">*</span></label>
				<Controls:PhoneBox runat="server" ID="uxPhone" TextBoxClass="text" />
			</div>
			<div class="formHalf">
				<label for="<%= uxFax.ClientID %>">
					Fax</label>
				<Controls:PhoneBox runat="server" ID="uxFax" TextBoxClass="text" />
			</div>
		</div>
		<div class="clear"></div>
		<SEOComponent:CurrentPageSEO ID="uxSEOData" runat="server" SitePageLinkSetupType="PageFormatter" PageLinkFormatter="~/office-details.aspx?id={0}" />
		<!-- button container -->
		<div class="buttons">
			<%--The markup for the buttons is in the BaseEditPage--%>
			<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
		</div>
	</asp:Panel>
</asp:Content>
