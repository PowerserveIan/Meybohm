<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-attribute-value-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminShowcaseAttributeValueEdit" Title="Admin - Attribute Value Add/Edit" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<asp:PlaceHolder runat="server" ID="uxCustomBreadCrumbsPH">
		<li>
			<a runat="server" href="~/admin/showcase/admin-attribute.aspx">
				<asp:Literal runat="server" ID="uxShowcaseName"></asp:Literal>Attribute Manager</a></li>
		<li>
			<asp:HyperLink runat="server" ID="uxLinkToValueManager"></asp:HyperLink></li>
	</asp:PlaceHolder>
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<ul class="inputList checkboxes horizontal">
				<li>
					<asp:CheckBox runat="server" ID="uxDisplayInFilters" Text="Display In Filters" Checked="true" /></li>				
			</ul>
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formWhole">
				<label for="<%=uxValue.ClientID%>">
					Value <span class="asterisk">*</span>
					<asp:Label runat="server" ID="uxDistanceDescription" Text="Must be an integer (in miles)" />
					<asp:Label runat="server" ID="uxTextDescription" Text="Must be less than 255 characters" />
				</label>
				<asp:TextBox runat="server" ID="uxValue" MaxLength="255" CssClass="text" />
				<asp:RegularExpressionValidator runat="server" ID="uxValueRegexVal" ControlToValidate="uxValue" ErrorMessage="Value is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
				<asp:CompareValidator runat="server" ID="uxValueRangeVal" ControlToValidate="uxValue" ErrorMessage="Value must be an integer greater than or equal to 0." Type="Integer" Operator="GreaterThanEqual" ValueToCompare="0" />
				<asp:RequiredFieldValidator runat="server" ID="uxValueReqFVal" ControlToValidate="uxValue" ErrorMessage="Value is required." />
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
