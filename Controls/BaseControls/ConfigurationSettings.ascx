<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ConfigurationSettings.ascx.cs" Inherits="ConfigurationSettingsControl" %>
<%@ Import Namespace="BaseCode" %>
<%@ Import Namespace="Classes.ConfigurationSettings" %>
<%@ Register TagPrefix="Controls" TagName="DateTimePicker" Src="~/Controls/BaseControls/DateTimePicker.ascx" %>
<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
	<div class="title">
		<h1>
			<asp:Literal runat="server" ID="uxComponentName"></asp:Literal>
			Settings</h1>
	</div>
	<ul class="breadcrumbs clearfix">
		<li class="firstBreadcrumb">
			<a runat="server" href="~/admin/" title="Home">Dashboard</a></li>
		<li class="currentBreadcrumb">
			<asp:Literal runat="server" ID="uxComponentName2"></asp:Literal>
			Settings</li>
	</ul>
	<a id='<%=Helpers.PageView.PageAnchors.center.ToString()%>'></a>
	<!-- required fields -->
	<div class="requiredFields"><span class="asterisk">*</span> required fields</div>
	<asp:CustomValidator ID="ClientSideValidationEventCapturer" runat="server" Display="None" ClientValidationFunction="ClearSuccess" />
	<asp:ValidationSummary ID="ErrorSummary" runat="server" CssClass="validation" DisplayMode="BulletList" HeaderText="<h3 class='failure'>Please correct the following errors:</h3>" />
	<asp:PlaceHolder ID="SuccessMessage" runat="server">
		<h3 class="success">The
			<asp:Literal runat="server" ID="uxComponentName3"></asp:Literal>
			Settings have been successfully <u>updated</u>.</h3>
	</asp:PlaceHolder>
	<asp:ListView ID="uxCheckBoxSettings" runat="server" ItemPlaceholderID="uxItemPH" ItemType="Classes.ConfigurationSettings.SiteSettings">
		<LayoutTemplate>
			<div class="blue padded optionsList">
				<ul class="inputList checkboxes horizontal">
					<asp:PlaceHolder runat="server" ID="uxItemPH"></asp:PlaceHolder>
				</ul>
				<div class="clear"></div>
			</div>
		</LayoutTemplate>
		<ItemTemplate>
			<li>
				<asp:CheckBox ID="uxSettingCheckBox" runat="server" Text='<%#Item.Description + (!String.IsNullOrEmpty(Item.EmbeddedDescription) ?
								"<span>" + Item.EmbeddedDescription + "</span>" : "")%>' Checked="<%#Convert.ToBoolean(Item.Value)%>" />
				<asp:HiddenField runat="server" Visible="false" ID="uxSettingID" Value='<%#Item.SiteSettingsID%>' />
				<asp:HiddenField runat="server" Visible="false" ID="uxDataType" Value='<%#Item.Type%>' />
				<asp:HiddenField runat="server" Visible="false" ID="uxSettingName" Value='<%#Item.Setting%>' />
			</li>
		</ItemTemplate>
	</asp:ListView>
	<div class="formWrapper">
		<asp:ListView ID="uxSettingsListView" runat="server" ItemPlaceholderID="uxItemPH" ItemType="Classes.ConfigurationSettings.SiteSettings">
			<LayoutTemplate>
				<asp:PlaceHolder runat="server" ID="uxItemPH"></asp:PlaceHolder>
			</LayoutTemplate>
			<ItemTemplate>
				<div runat="server" id="uxColumnContainer" class="formHalf">
					<%--label for done by javascript below--%>
					<label>
						<%#Item.Description%><%#Item.IsRequired ? "<span class='asterisk'>*</span>" : ""%>
						<%#!String.IsNullOrEmpty(Item.EmbeddedDescription) ?
								"<span class='tooltip'><span>" + Item.EmbeddedDescription + "</span></span>"
								: ""%>
						<asp:Label runat="server" ID="uxValueRequiredToPassValidation" Visible="false" />
					</label>
					<asp:HiddenField runat="server" Visible="false" ID="uxSettingID" Value='<%#Item.SiteSettingsID%>' />
					<asp:HiddenField runat="server" Visible="false" ID="uxDataType" Value='<%#Item.Type%>' />
					<asp:HiddenField runat="server" Visible="false" ID="uxSettingName" Value='<%#Item.Setting%>' />
					<asp:HiddenField runat="server" Visible="false" ID="uxOptions" Value='<%#Item.Options%>' />
					<asp:DropDownList ID="uxSettingDDL" runat="server" Visible="false" AppendDataBoundItems="true" CssClass="dynamic">
					</asp:DropDownList>
					<asp:PlaceHolder runat="server" ID="uxRadEditorPH" Visible="false">
						<asp:Label runat="server" ID="uxSettingHTMLText" Text="<%#Helpers.ReplaceRootWithAbsolutePath(Item.Value)%>"></asp:Label>
						<div class="richEditor" style="display: none;">
							<asp:TextBox runat="server" ID="uxSettingHTML" CssClass="tinymce" TextMode="MultiLine" Width="480" Text="<%#Helpers.ReplaceRootWithAbsolutePath(Item.Value)%>" />
						</div>
						<asp:HiddenField runat="server" ID="uxSettingHTMLEdited" Value="False" />
						<asp:HyperLink runat="server" ID="uxToggleRadEditor" CssClass="toggleEditor" NavigateUrl="#"><img runat="server" src="~/admin/img/btnIcons/icon_edit.png" alt="Edit Content" /></asp:HyperLink>
					</asp:PlaceHolder>
					<asp:TextBox runat="server" ID="uxSettingValue" MaxLength="20" Text='<%#Item.Value%>' CssClass="text"/>
					<Controls:DateTimePicker runat="server" ID="uxSettingDate" TextBoxCssClass="text" Required="false" Visible="false" />
					<asp:CheckBoxList runat="server" ID="uxSettingList" Visible="false" CssClass="chbox_list">
					</asp:CheckBoxList>
					<asp:RegularExpressionValidator runat="server" Enabled="false" ID="uxSettingValueEmailVal" ErrorMessage='<%#Item.Description + " is invalid."%>'
						ControlToValidate="uxSettingValue" ValidationExpression="<%#Helpers.EmailValidationExpression%>" />
					<asp:RegularExpressionValidator runat="server" Enabled="false" ID="uxSettingValueMaxLengthVal" ControlToValidate="uxSettingValue" ErrorMessage='<%#Item.Description + " is too long.  It must be 2000 characters or less."%>' ValidationExpression="^[\s\S]{0,2000}$" />
					<asp:RequiredFieldValidator Enabled='<%#Item.IsRequired && Item.Type != "list"%>' runat="server" ID="uxSettingRFV" ControlToValidate='<%#Item.Type == "date" ? "uxSettingDate" : (Item.Type == "html" ? "uxSettingHTML" : "uxSettingValue")%>'
						ErrorMessage='<%#Item.Description + " is a required field."%>' />
					<asp:RangeValidator runat="server" ID="uxSettingRangeVal" Enabled="false" ControlToValidate="uxSettingValue" Type="Integer" ErrorMessage='<%#Item.Description + " must be numeric and greater than or equal to zero."%>' MinimumValue="0" MaximumValue="99999999" />
					<asp:CustomValidator runat="server" ID="uxSettingListRFV" Enabled='<%#Item.IsRequired && Item.Type == "list"%>' OnServerValidate="uxSettingListRFV_ServerValidate" ErrorMessage='<%#"\"" + Item.Description + "\" requires at least one option to be selected."%>' />
				</div>
			</ItemTemplate>
		</asp:ListView>
		<asp:PlaceHolder runat="server" ID="uxMoreSettings"></asp:PlaceHolder>
	</div>
	<div class="clear"></div>
	<div class="buttons">
		<asp:Button ID="uxSave" runat="server" Text="Save Settings" CssClass="button save" OnClientClick="if (editorLoaded)tinyMCE.triggerSave(false,true);" />
		<div class="clear"></div>
	</div>
</asp:Panel>
<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/tiny_mce/jquery.tinymce.js,~/tft-js/core/tinymceinit.js"></asp:Literal>
<script type="text/javascript">
	//<![CDATA[
	$(document).ready(function () {
		$("[id$=uxSettingHTML]").each(function () {
			InitializeTinyMCE($(this).attr("id"), <%= Request.AppRelativeCurrentExecutionFilePath.Split('/').Length - 2 %>);
		});
		$(".toggleEditor").click(function () {
			$(this).siblings("[id$=uxSettingHTMLText]").remove();
			$(this).siblings(".richEditor").show();
			$(this).siblings("[id$=uxSettingHTMLEdited]").val("True");
			$(this).remove();
			return false;
		});
		$("div.formHalf, div.formWhole").each(function () {
			if ($(this).find("label:first").siblings("input:first").length > 0)
				$(this).find("label:first").attr("for", $(this).find("label:first").siblings("input:first").attr("id"));
			else if ($(this).find("label:first").siblings("textarea:first").length > 0)
				$(this).find("label:first").attr("for", $(this).find("label:first").siblings("textarea:first").attr("id"));
			else if ($(this).find("label:first").siblings("select:first").length > 0)
				$(this).find("label:first").attr("for", $(this).find("label:first").siblings("select:first").attr("id"));
		});
	});
	//]]>
</script>
