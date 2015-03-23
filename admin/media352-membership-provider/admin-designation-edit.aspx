<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-designation-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminDesignationEdit"%>
<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%= uxName.ClientID %>">
					Name<span class="asterisk">*</span><br /><span>Must be less than 255 characters</span></label>
				<asp:TextBox runat="server" id="uxName" MaxLength="255" CssClass="text" />
				<asp:RequiredFieldValidator runat="server" id="uxNameReqFVal" ControlToValidate="uxName" ErrorMessage="Name is required."  />
				<asp:RegularExpressionValidator runat="server" id="uxNameRegexVal" ControlToValidate="uxName" ErrorMessage="Name is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$"  />
			</div>
			<div class="formWhole"><span class="label">Icon<span class="asterisk">*</span><span class="tooltip"> <span>	Must be of type .gif, .jpg, .jpeg, .png<br />
				<br />
				Suggested resizing tool:
				<a href="http://faststone.org/FSViewerDetail.htm" target="_blank">FastStone</a></span> </span><span>Optimal image size is 25x25</span></span>
				<Controls:FileUpload runat="server" ID="uxIcon" ImageWidth="25" ImageHeight="25" AllowedFileTypes=".gif,.jpg,.jpeg,.png" RequiredErrorMessage="Icon is required." Required="true" UploadToLocation="~/uploads/designations" />
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
