<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-redirect-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminRedirectEdit"%>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="formWrapper">
			<div class="formWhole">
				<label for="<%= uxOldUrl.ClientID %>">
					Old Url<span class="asterisk">*</span><br /><span>Must be less than 2000 characters</span></label>
				<asp:TextBox runat="server" id="uxOldUrl" TextMode="MultiLine" CssClass="text" />
				<asp:RequiredFieldValidator runat="server" id="uxOldUrlReqFVal" ControlToValidate="uxOldUrl" ErrorMessage="Old Url is required."  />
				<asp:RegularExpressionValidator runat="server" id="uxOldUrlRegexVal" ControlToValidate="uxOldUrl" ErrorMessage="Old Url is too long.  It must be 2000 characters or less." ValidationExpression="^[\s\S]{0,2000}$"  />
			</div>
			<div class="formWhole">
				<label for="<%= uxNewUrl.ClientID %>">
					New Url<span class="asterisk">*</span><br /><span>Must be less than 2000 characters</span></label>
				<asp:TextBox runat="server" id="uxNewUrl" TextMode="MultiLine" CssClass="text" />
				<asp:RequiredFieldValidator runat="server" id="uxNewUrlReqFVal" ControlToValidate="uxNewUrl" ErrorMessage="New Url is required."  />
				<asp:RegularExpressionValidator runat="server" id="uxNewUrlRegexVal" ControlToValidate="uxNewUrl" ErrorMessage="New Url is too long.  It must be 2000 characters or less." ValidationExpression="^[\s\S]{0,2000}$"  />
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
