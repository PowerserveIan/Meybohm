<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-mailing-list-subscriber-edit.aspx.cs" EnableViewState="true" EnableEventValidation="false" Inherits="Admin_MailingListSubscriberEdit" Title="Admin - Mailing List Subscriber Add/Edit" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<asp:CheckBox runat="server" ID="uxSubscribed" Checked="true" Text="Is Subscribed" />
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf"><span class="label">Email Address </span>
				<asp:Literal ID="uxEmail" runat="server"></asp:Literal>
			</div>
			<div class="formHalf"><span class="label">Format </span>
				<asp:RadioButtonList RepeatLayout="UnorderedList" ID="uxNewsletterFormat" runat="server" CssClass="inputList radiobuttons">
					<asp:ListItem Text="HTML" Value="1"></asp:ListItem>
					<asp:ListItem Text="Text" Value="2"></asp:ListItem>
				</asp:RadioButtonList>
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
