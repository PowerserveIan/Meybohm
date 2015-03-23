<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-media-collection-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminMediaCollectionEdit" Title="Admin - Media Collection Add/Edit" %>

<%@ Register TagPrefix="Controls" TagName="RichTextEditor" Src="~/Controls/BaseControls/RichTextEditor.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<asp:PlaceHolder runat="server" ID="uxCustomBreadCrumbsPH">
		<li>
			<a runat="server" href="~/admin/showcase/admin-showcase-item.aspx">
				<asp:Literal runat="server" ID="uxShowcaseName"></asp:Literal>
				Property Manager</a></li>
		<li>
			<asp:HyperLink runat="server" ID="uxLinkToMediaCollectionManager"></asp:HyperLink></li>
	</asp:PlaceHolder>
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Is Active" />
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%=uxShowcaseMediaTypeID.ClientID%>">
					Showcase Media Type<span class="asterisk">*</span>
				</label>
				<asp:DropDownList runat="server" ID="uxShowcaseMediaTypeID" />
			</div>
			<div class="formHalf">
				<label for="<%=uxTitle.ClientID%>">
					Title<span class="asterisk">*</span> <span>Must be less than 255 characters</span>
				</label>
				<asp:TextBox runat="server" ID="uxTitle" MaxLength="255" CssClass="text" />
				<asp:RegularExpressionValidator runat="server" ID="uxTitleRegexVal" ControlToValidate="uxTitle" ErrorMessage="Title is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
				<asp:RequiredFieldValidator runat="server" ID="uxTitleReqFVal" ControlToValidate="uxTitle" ErrorMessage="Title is required." />
			</div>
			<div class="formWhole" id="textBlockTR" style="display: none;">
				<label for="<%=uxTextBlock.ClientID%>">
					Text Block<span class="tooltip"> <span>If your text block will contains links, make sure to have those links open in a new window</span></span></label>
				<Controls:RichTextEditor runat="server" ID="uxTextBlock" FieldName="Text Block" />
			</div>
			<asp:PlaceHolder runat="server" ID="uxMediaPlaceHolder" Visible="false">
				<div class="formWhole">
					<asp:HyperLink runat="server" ID="uxEditMedia" CssClass="button edit"></asp:HyperLink>
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
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		//<![CDATA[
		function ToggleTextBlock(mediaType) {
			if (mediaType.indexOf('Text') >= 0)
				$("#textBlockTR").show();
			else
				$("#textBlockTR").hide();
		}

		$(document).ready(function () {
			ToggleTextBlock($("#<%=uxShowcaseMediaTypeID.ClientID%> :selected").text());
			$("#<%=uxShowcaseMediaTypeID.ClientID%>").change(function () {
				ToggleTextBlock($(this).find(':selected').text());
			});
		});
		//]]>
	</script>
</asp:Content>
