<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-whats-near-by-category-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminWhatsNearByCategoryEdit"%>

<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<asp:CheckBox runat="server" id="uxActive" Checked="true" Text="Active" />
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%= uxName.ClientID %>">
					Name<span class="asterisk">*</span><br /><span>Must be less than 255 characters</span></label>
				<asp:TextBox runat="server" id="uxName" MaxLength="255" CssClass="text" />
				<asp:RequiredFieldValidator runat="server" id="uxNameReqFVal" ControlToValidate="uxName" ErrorMessage="Name is required."  />
				<asp:RegularExpressionValidator runat="server" id="uxNameRegexVal" ControlToValidate="uxName" ErrorMessage="Name is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$"  />
			</div>
			<div class="formWhole">
				<label for="<%= uxPlaceholderImage.ClientID %>">
					Default Image</label>
				<Controls:FileUpload runat="server" ID="uxPlaceholderImage" Required="False" UploadToLocation="~/uploads/nearByLocations" />
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
