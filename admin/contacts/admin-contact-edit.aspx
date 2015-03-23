<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-contact-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminContactEdit" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="formWrapper">
			<asp:PlaceHolder runat="server" ID="uxShowcaseItemPH">
				<div class="formWhole">
					<span class="label">Property Address</span>
					<asp:Label runat="server" ID="uxShowcaseItemTitle"></asp:Label>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="uxAgentPH">
				<div class="formWhole">
					<span class="label">Agent Name</span>
					<asp:Label runat="server" ID="uxAgentName"></asp:Label>
				</div>
			</asp:PlaceHolder>
			<div class="formHalf">
				<span class="label">Submitted On</span>
				<asp:Label runat="server" ID="uxTimestamp"></asp:Label>
			</div>
			<div class="formHalf">
				<label for="<%= uxContactStatusID.ClientID %>">
					Status</label>
				<asp:DropDownList runat="server" ID="uxContactStatusID" />
			</div>
			<div class="formHalf">
				<span class="label">Contact Time</span>
				<asp:Label runat="server" ID="uxContactTime"></asp:Label>
			</div>
			<div class="formHalf">
				<span class="label">Contact Method</span>
				<asp:Label runat="server" ID="uxContactMethod"></asp:Label>
			</div>
			<div class="formHalf">
				<span class="label">First Name</span>
				<asp:Label runat="server" ID="uxFirstName"></asp:Label>
			</div>
			<div class="formHalf">
				<span class="label">Last Name</span>
				<asp:Label runat="server" ID="uxLastName"></asp:Label>
			</div>
			<div class="formHalf">
				<span class="label">Email</span>
				<asp:Label runat="server" ID="uxEmail"></asp:Label>
			</div>
			<div class="formHalf">
				<span class="label threeLine">Phone</span>
				<asp:Label runat="server" ID="uxPhone"></asp:Label>
			</div>
			<div class="formWhole">
				<span class="label">Message</span>
				<asp:Label runat="server" ID="uxMessage"></asp:Label>
			</div>
			<asp:PlaceHolder runat="server" ID="uxAddressPH">
				<hr />
				<div class="formHalf">
					<span class="label">Address 1</span>
					<asp:Label runat="server" ID="uxAddress1"></asp:Label>
				</div>
				<div class="formHalf">
					<span class="label">Address 2</span>
					<asp:Label runat="server" ID="uxAddress2"></asp:Label>
				</div>
				<div class="formHalf">
					<span class="label">City</span>
					<asp:Label runat="server" ID="uxCity"></asp:Label>
				</div>
				<div class="formHalf">
					<span class="label">State</span>
					<asp:Label runat="server" ID="uxState"></asp:Label>
				</div>
				<div class="formHalf">
					<span class="label">Zip</span>
					<asp:Label runat="server" ID="uxZip"></asp:Label>
				</div>
			</asp:PlaceHolder>
		</div>
		<div class="clear"></div>
		<!-- button container -->
		<div class="buttons">
			<%--The markup for the buttons is in the BaseEditPage--%>
			<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
		</div>
	</asp:Panel>
</asp:Content>
