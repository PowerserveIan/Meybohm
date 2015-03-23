<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-video-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminVideoEdit" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<ul class="inputList checkboxes horizontal">
				<li>
					<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Active" /></li>
				<li>
					<asp:CheckBox runat="server" ID="uxFeatured" Text="Featured" /></li>
			</ul>
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formWhole">
				<label for="<%= uxTitle.ClientID %>">
					Title<span class="asterisk">*</span><br />
					<span>Must be less than 255 characters</span></label>
				<asp:TextBox runat="server" ID="uxTitle" MaxLength="255" CssClass="text" />
				<asp:RequiredFieldValidator runat="server" ID="uxTitleReqFVal" ControlToValidate="uxTitle" ErrorMessage="Title is required." />
				<asp:RegularExpressionValidator runat="server" ID="uxTitleRegexVal" ControlToValidate="uxTitle" ErrorMessage="Title is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
			</div>
			<div class="formWhole">
				<label for="<%= uxUrl.ClientID %>">
					Embed Code or URL<span class="asterisk">*</span><span class="tooltip"><span>Dimensions of the video should be 250px wide by 190px tall</span></span><span>Must be less than 2000 characters</span></label>
				<asp:TextBox runat="server" ID="uxUrl" TextMode="MultiLine" CssClass="text" />
				<asp:RequiredFieldValidator runat="server" ID="uxUrlReqFVal" ControlToValidate="uxUrl" ErrorMessage="Embed Code or URL is required." />
				<asp:RegularExpressionValidator runat="server" ID="uxUrlRegexVal" ControlToValidate="uxUrl" ErrorMessage="Embed Code or URL is too long.  It must be 2000 characters or less." ValidationExpression="^[\s\S]{0,2000}$" />
			</div>
			<div class="formWhole">
				<span class="label">Preview</span>
				<div id="previewDiv"></div>
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
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		$(document).ready(function () {
			function UpdatePreview() {
				var iframeTemplate = '<iframe width="250" height="190" src="{0}" frameborder="0" allowfullscreen></iframe>';
				var previewSrc = $("#<%= uxUrl.ClientID %>").val();
				if (previewSrc.indexOf("iframe") == -1)
					previewSrc = iframeTemplate.replace("{0}", previewSrc);
				$("#previewDiv").html(previewSrc);
			}
			UpdatePreview();
			$("#<%= uxUrl.ClientID %>").change(function () {
				UpdatePreview();
			});
		});
	</script>
</asp:Content>
