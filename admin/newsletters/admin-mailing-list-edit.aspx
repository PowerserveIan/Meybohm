<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-mailing-list-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_MailingListEdit" Title="Admin - Mailing List Add/Edit" %>

<%@ Register TagPrefix="Newsletters" TagName="MailingListSubscribers" Src="~/Controls/Newsletters/MailingListSubscriber.ascx" %>
<%@ Register TagName="Import" TagPrefix="admin" Src="~/Controls/Newsletters/MailingListImport.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Is Active" />
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%=uxName.ClientID%>">
					Mailing List Name<span class="asterisk">*</span>
				</label>
				<asp:TextBox CssClass="text" runat="server" ID="uxName" MaxLength="50" />
				<asp:RequiredFieldValidator runat="server" ID="uxNameReqFVal" ControlToValidate="uxName" ErrorMessage="Name is required." />
				<asp:RegularExpressionValidator runat="server" ID="uxNameRegexVal" ControlToValidate="uxName" ErrorMessage="Name is too long.  It must be 50 characters or less." ValidationExpression="^[\s\S]{0,50}$" />
				<asp:CustomValidator ID="uxNameCV" runat="server" ControlToValidate="uxName" ErrorMessage="Name already taken." OnServerValidate="uxNameCV_ServerValidate" />
			</div>
		</div>
		<div class="clear"></div>
		<!-- button container -->
		<div class="buttons">
			<%--The markup for the buttons is in the BaseEditPage--%>
			<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
		</div>
	</asp:Panel>
	<Newsletters:MailingListSubscribers ID="uxMailingListSubscribers" runat="server" />
	<admin:Import runat="server" ID="uxImport"></admin:Import>
</asp:Content>
