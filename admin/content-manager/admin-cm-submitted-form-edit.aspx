<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-cm-submitted-form-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminCMSubmittedFormEdit" Title="Admin - Submitted Form Edit" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<asp:CheckBox runat="server" ID="uxIsProcessed" Text="Is Processed <span>Check this box if you have handled this form submission</span>" />
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf"><span class="label">Date Submitted </span>
				<asp:Label runat="server" ID="uxDateSubmitted"></asp:Label>
			</div>
			<div class="formWhole"><span class="label">Form HTML </span>
				<asp:Literal runat="server" ID="uxFormHTML"></asp:Literal>
			</div>
			<asp:PlaceHolder runat="server" ID="uxUploadedFilePH">
				<div class="formHalf"><span class="label">Uploaded File <span>Click to download</span></span>
					<asp:LinkButton runat="server" ID="uxDownloadFile" ToolTip="Click to Download" CssClass="button download"></asp:LinkButton>
				</div>
			</asp:PlaceHolder>
			<div class="formHalf"><span class="label">Form Recipient </span>
				<asp:Label runat="server" ID="uxFormRecipient" />
			</div>
			<div class="formHalf"><span class="label">Response Page </span>
				<asp:Label runat="server" ID="uxResponsePage"></asp:Label>
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
