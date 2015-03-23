<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-dynamic-image-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminDynamicImageEdit" Title="Admin - Dynamic Slide Add/Edit" %>

<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="RichTextEditor" Src="~/Controls/BaseControls/RichTextEditor.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<ul class="inputList checkboxes horizontal">
				<li>
					<asp:CheckBox runat="server" ID="uxIsVideo" Text="Is Video" /></li>
				<li>
					<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Is Active" /></li>
			</ul>
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%=uxTitle.ClientID%>">
					Title<span class="asterisk">*</span></label>
				<asp:TextBox runat="server" ID="uxTitle" Columns="40" MaxLength="255" CssClass="text large" />
				<asp:RequiredFieldValidator runat="server" ID="uxTitleReqFVal" ControlToValidate="uxTitle" Text="* Title is required" ErrorMessage="Title is required." />
				<asp:RegularExpressionValidator runat="server" ID="uxTitleRegexVal" ControlToValidate="uxTitle" Text="* Title is too long" ErrorMessage="Title is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
			</div>
			<div class="formWhole"><span class="label"><span id="imageText" class="spanReset">Image<span class="asterisk">*</span><span class="tooltip"><span>Must be of type .gif, .jpg, .jpeg, .png
				<br />
				<br />
				Suggested resizing tool:
				<a href="http://faststone.org/FSViewerDetail.htm" target="_blank">FastStone</a></span></span> <span>Optimal image size is 600x400</span> </span><span id="videoText" class="spanReset" style="display: none;">Video<span class="asterisk">*</span> <span>Must be FLV
					format and should adhere to a 16:9 aspect ratio</span></span> </span>
				<Controls:FileUpload runat="server" ID="uxName" Required="True" AllowedFileTypes=".gif,.jpg,.jpeg,.png" ImageHeight="400" ImageWidth="600" RequiredErrorMessage="Image is required." />
			</div>
			<asp:PlaceHolder ID="uxThumbnailPlaceHolder" runat="server">
				<div class="formWhole"><span class="label">Thumbnail</span>
					<asp:Image runat="server" ID="uxThumbnail" />
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="uxCaptionPH">
				<div class="formWhole">
					<label for="<%=uxCaption.ClientID%>">
						Caption
					</label>
					<Controls:RichTextEditor runat="server" ID="uxCaption" FieldName="Caption" />
				</div>
			</asp:PlaceHolder>
			<div class="formHalf" id="websiteLink">
				<label for="<%=uxLink.ClientID%>">
					Website Link<span class="tooltip"><span>Where you want to redirect the user to if they click on the image in the header.<br />
						<br />
						If the link is a page outside of your website, it MUST start with http://, e.g. (http://www.352media.com)</span></span>
				</label>
				<asp:TextBox runat="server" ID="uxLink" MaxLength="250" CssClass="text large" />
				<asp:RegularExpressionValidator runat="server" ID="uxLinkRegexVal" ControlToValidate="uxLink" ErrorMessage="Link is too long.  It must be 250 characters or less." ValidationExpression="^[\s\S]{0,250}$" />
				<asp:CustomValidator runat="server" ID="uxLinkToPageCustomVal" ControlToValidate="uxLink" OnServerValidate="uxLinkToPageCustomVal_ServerValidate" ErrorMessage="Link must be an existing page on your site or a valid web address." />
			</div>
			<div class="formHalf">
				<label for="<%=uxDuration.ClientID%>">
					Cycle Interval of Image<span class="tooltip"><span>The amount of time the image will stay on the screen before the next image is displayed.<br />
						<br />
						If no speed is specified, the default duration set in the settings manager will be used.</span></span> <span>(in seconds) </span>
				</label>
				<asp:TextBox runat="server" ID="uxDuration" CssClass="text small" />
				<asp:CompareValidator runat="server" ID="uxDurationCPV" ControlToValidate="uxDuration" ValueToCompare="0" Operator="GreaterThan" Type="Integer" ErrorMessage="Duration must be an integer greater than 0." />
			</div>
		</div>
		<asp:PlaceHolder runat="server" ID="uxCollectionsPH">
			<div class="rightCol">
				<div class="formHalf"><span class="label">Collections<span class="asterisk">*</span> </span>
					<asp:CheckBoxList ID="uxCollections" runat="server" CssClass="inputList checkboxes" />
				</div>
			</div>
		</asp:PlaceHolder>
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
		var originalFileTypes = "<%= uxName.AllowedFileTypes %>";
		$(document).ready(function () {
			$("#<%= uxIsVideo.ClientID %>").click(function () {
				ToggleVideo();
			});

			ToggleVideo();
			function ToggleVideo() {
				if ($("#<%= uxIsVideo.ClientID %>").is(":checked")) {
					$("#imageText").hide();
					$("#websiteLink").hide();
					$("#videoText").show();
					allowedFileTypes_<%= uxName.ClientID %> = "(flv)";
				}
				else {
					$("#imageText").show();
					$("#websiteLink").show();
					$("#videoText").hide();
					allowedFileTypes_<%= uxName.ClientID %> = originalFileTypes;
				}
			}
		});
	</script>
</asp:Content>
