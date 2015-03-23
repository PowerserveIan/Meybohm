<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="configuration-settings.aspx.cs" Inherits="Admin_ConfigurationSettings" Title="Configuration Settings" %>

<%@ Register TagPrefix="Controls" TagName="ConfigurationSettings" Src="~/Controls/BaseControls/ConfigurationSettings.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<Controls:ConfigurationSettings runat="server" ID="uxConfigSettings" Component="SiteWide">
		<AdditionalSettingsTemplate>
			<div class="formWhole">
				<label>
					Time Zone<span class="asterisk">*</span></label>
				<asp:DropDownList runat="server" ID="uxTimeZoneID" CssClass="dynamic" AppendDataBoundItems="true">
					<asp:ListItem Text="--Select TimeZone--" Value="0"></asp:ListItem>
				</asp:DropDownList>
				<asp:RequiredFieldValidator runat="server" InitialValue="0" ID="uxTimeZoneIDRFV" ControlToValidate="uxTimeZoneID" ErrorMessage="TimeZone is required" />
			</div>
		</AdditionalSettingsTemplate>
	</Controls:ConfigurationSettings>
</asp:Content>
