<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-neighborhood-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminNeighborhoodEdit" %>

<%@ Register TagPrefix="Controls" TagName="Address" Src="~/Controls/State_And_Country/Address.ascx" %>
<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="PhoneBox" Src="~/Controls/BaseControls/PhoneBoxControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="RichTextEditor" Src="~/Controls/BaseControls/RichTextEditor.ascx" %>
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
					<asp:CheckBox runat="server" ID="uxFeatured" Text="Featured" /></li>
				<li>
					<asp:CheckBox runat="server" ID="uxShowLotsLand" Text="Show Lots/Land on Neighborhood Details" /></li>
			</ul>
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
			<div class="formHalf">
				<label for="<%= uxCMMicrositeID.ClientID %>">
					Market<span class="asterisk">*</span></label>
				<asp:DropDownList runat="server" ID="uxCMMicrositeID" AppendDataBoundItems="true">
					<asp:ListItem Text="--Select a Market--" Value=""></asp:ListItem>
				</asp:DropDownList>
				<asp:RequiredFieldValidator runat="server" ID="uxCMMicrositeIDRFV" ControlToValidate="uxCMMicrositeID" ErrorMessage="Market is required." InitialValue="" />
			</div>
			<Controls:Address runat="server" ID="uxAddress" AddressLabel="Address/Location" Required="true" />
			<div class="formWhole">
				<label for="<%= uxImage.ClientID %>">
					Image<span>Optimal image size is 232x162</span></label>
				<Controls:FileUpload runat="server" ID="uxImage" Required="False" AllowExternalImageLink="true" UploadToLocation="~/uploads/neighborhoods" />
			</div>
			<div class="formHalf">
				<label for="<%= uxPhone.ClientID %>">
					Phone</label>
				<Controls:PhoneBox runat="server" ID="uxPhone" TextBoxClass="text" Required="false" />
			</div>
			<div class="formHalf">
				<label for="<%= uxWebsite.ClientID %>">
					Website</label>
				<asp:TextBox runat="server" ID="uxWebsite" MaxLength="1000" CssClass="text" />
				<asp:RegularExpressionValidator runat="server" ID="uxWebsiteRegexVal" ControlToValidate="uxWebsite" ErrorMessage="Website is too long.  It must be 1000 characters or less." ValidationExpression="^[\s\S]{0,1000}$" />
			</div>
			<div class="formHalf">
				<label for="<%= uxPriceRange.ClientID %>">
					Price Range<br />
					<span>Must be less than 255 characters</span></label>
				<asp:TextBox runat="server" ID="uxPriceRange" MaxLength="255" CssClass="text" />
				<asp:RegularExpressionValidator runat="server" ID="uxPriceRangeRegexVal" ControlToValidate="uxPriceRange" ErrorMessage="Price Range is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
			</div>
			<div class="formWhole">
				<label for="<%= uxDirections.ClientID %>">
					Directions<br />
					<span>Must be less than 1000 characters</span></label>
				<Controls:RichTextEditor runat="server" ID="uxDirections" FieldName="Overview" MaxLength="1000" HideEditorInitially="true" />
			</div>
			<div class="formWhole">
				<label for="<%= uxOverview.ClientID %>">
					Overview<br />
					<span>Must be less than 2500 characters</span></label>
				<Controls:RichTextEditor runat="server" ID="uxOverview" FieldName="Overview" MaxLength="2500" HideEditorInitially="true" />
			</div>
			<div class="formWhole">
				<label for="<%= uxAmenities.ClientID %>">
					Amenities<br />
					<span>Must be less than 1000 characters</span></label>
				<Controls:RichTextEditor runat="server" ID="uxAmenities" FieldName="Overview" MaxLength="1000" HideEditorInitially="true" />
			</div>
		</div>
		<div class="clear"></div>
		<SEOComponent:CurrentPageSEO ID="uxSEOData" runat="server" SitePageLinkSetupType="PageFormatter" PageLinkFormatter="~/neighborhood-details.aspx?id={0}" />
		<!-- button container -->
		<div class="buttons">
			<%--The markup for the buttons is in the BaseEditPage--%>
			<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
		</div>
	</asp:Panel>
</asp:Content>
