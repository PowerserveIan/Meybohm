<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminSellNewHome.ascx.cs" Inherits="Controls_Showcase_AdminSellNewHome" %>
<%@ Register TagPrefix="Controls" TagName="DateTimePicker" Src="~/Controls/BaseControls/DateTimePicker.ascx" %>
<div class="formHalf staticError">
	<label for="<%= uxSoldHomeCloseDate.ClientID + "_uxDate" %>">
		Close Date</label>
	<Controls:DateTimePicker runat="server" ID="uxSoldHomeCloseDate" TextBoxCssClass="text" Required="true" RequiredErrorMessage="Close Date is required to mark this home as sold." />
</div>
<div class="formHalf staticError">
	<label for="<%=uxSoldHomeSalePrice.ClientID%>">
		Sale Price
	</label>
	$<asp:TextBox runat="server" ID="uxSoldHomeSalePrice" CssClass="text numbersOnly" MaxLength="17" />
	<asp:RequiredFieldValidator runat="server" ID="uxSoldHomeSalePriceRFV" ControlToValidate="uxSoldHomeSalePrice" ErrorMessage="Sale Price is required to mark this home as sold." />
	<asp:CompareValidator runat="server" ID="uxSoldHomeSalePriceCompareVal" ControlToValidate="uxSoldHomeSalePrice" ErrorMessage="Sale Price must be a valid American currency value greater than $0." Operator="DataTypeCheck" Type="Currency" />
</div>
<div class="clear"></div>
<div class="formHalf">
	<label for="<%= uxSoldHomeListingAgent.ClientID %>">
		Seller (Listing Agent)</label>
	<asp:DropDownList runat="server" ID="uxSoldHomeListingAgent" AppendDataBoundItems="true">
		<asp:ListItem Text="--Select an Agent--" Value=""></asp:ListItem>
	</asp:DropDownList>
	<asp:HyperLink runat="server" ID="uxSoldHomeAddSeller" Text="<span>Add Seller</span>" CssClass="button add" Target="_blank" NavigateUrl="~/admin/media352-membership-provider/admin-user-edit.aspx?id=0&FilterUserHasRole=true&FilterUserRoleName=Agent"></asp:HyperLink>
</div>
<div class="formHalf staticError">
	<label for="<%=uxSoldHomeSellerPercentage.ClientID%>">
		Seller Percentage
	</label>
	<asp:TextBox runat="server" ID="uxSoldHomeSellerPercentage" CssClass="text numbersOnly integer" MaxLength="10" />%
	<asp:RangeValidator runat="server" ID="uxSoldHomeSellerPercentageRangeVal" ControlToValidate="uxSoldHomeSellerPercentage" ErrorMessage="Seller Percentage must be a numeric value between 0 and 100." Type="Double" MinimumValue="0" MaximumValue="100" />
</div>
<div class="clear"></div>
<div class="formHalf">
	<label for="<%= uxSoldHomeSellerOffice.ClientID %>">
		Seller Office</label>
	<asp:DropDownList runat="server" ID="uxSoldHomeSellerOffice" AppendDataBoundItems="true">
		<asp:ListItem Text="--Select an Office--" Value=""></asp:ListItem>
	</asp:DropDownList>
</div>
<div class="formHalf staticError">
	<label for="<%=uxSoldHomeSellerOfficePercentage.ClientID%>">
		Seller Office Percentage
	</label>
	<asp:TextBox runat="server" ID="uxSoldHomeSellerOfficePercentage" CssClass="text numbersOnly integer" MaxLength="10" />%
	<asp:RangeValidator runat="server" ID="uxSoldHomeSellerOfficePercentageRangeVal" ControlToValidate="uxSoldHomeSellerOfficePercentage" ErrorMessage="Seller Office Percentage must be a numeric value between 0 and 100." Type="Double" MinimumValue="0" MaximumValue="100" />
</div>
<div class="formHalf">
	<label for="<%= uxSoldHomeSalesAgent.ClientID %>">
		Sales Agent</label>
	<asp:DropDownList runat="server" ID="uxSoldHomeSalesAgent" AppendDataBoundItems="true">
		<asp:ListItem Text="--Select an Agent--" Value=""></asp:ListItem>
	</asp:DropDownList>
	<asp:HyperLink runat="server" ID="uxSoldHomeAddSalesAgent" Text="<span>Add Sales Agent</span>" CssClass="button add" Target="_blank" NavigateUrl="~/admin/media352-membership-provider/admin-user-edit.aspx?id=0&FilterUserHasRole=true&FilterUserRoleName=Agent"></asp:HyperLink>
</div>
<div class="formHalf staticError">
	<label for="<%=uxSoldHomeSalesAgentPercentage.ClientID%>">
		Sales Agent Percentage
	</label>
	<asp:TextBox runat="server" ID="uxSoldHomeSalesAgentPercentage" CssClass="text numbersOnly integer" MaxLength="10" />%
	<asp:RangeValidator runat="server" ID="uxSoldHomeSalesAgentPercentageRangeVal" ControlToValidate="uxSoldHomeSalesAgentPercentage" ErrorMessage="Sales Agent Percentage must be a numeric value between 0 and 100." Type="Double" MinimumValue="0" MaximumValue="100" />
</div>
