<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-media-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminMediaEdit" Title="Admin - Media Add/Edit" %>

<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<asp:PlaceHolder runat="server" ID="uxCustomBreadCrumbsPH">
		<li>
			<a runat="server" href="~/admin/showcase/admin-showcase-item.aspx">
				<asp:Literal runat="server" ID="uxShowcaseName"></asp:Literal>
				Property Manager
			</a>
		</li>
		<li>
			<asp:HyperLink runat="server" ID="uxLinkToMediaCollectionManager"></asp:HyperLink>
		</li>
		<li>
			<asp:HyperLink runat="server" ID="uxLinkToMediaManager"></asp:HyperLink>
		</li>
	</asp:PlaceHolder>
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Is Active" />
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<asp:PlaceHolder runat="server" ID="uxVideoPlaceHolder">
				<div class="formWhole">
					<label for="<%=uxURL.ClientID%>">
						Link to Youtube video<span class="asterisk">*</span><span class="tooltip"><span style="width: 290px;">You can find the URL on the Youtube page by clicking the "Share" button (shown below)
							<img runat="server" src="~/admin/img/showcaseYoutubeURL.jpg" alt="YouTube URL" width="294" height="100" /></span></span>
					</label>
					<asp:TextBox runat="server" ID="uxURL" MaxLength="1000" CssClass="text" />
					<asp:RegularExpressionValidator runat="server" ID="uxURLRegexVal" ControlToValidate="uxURL" ErrorMessage="Link to Youtube video is too long.  It must be 1000 characters or less." ValidationExpression="^[\s\S]{0,1000}$" />
					<asp:RequiredFieldValidator runat="server" ID="uxURLReqFVal" ControlToValidate="uxURL" ErrorMessage="Link to Youtube video is required." />
				</div>
				<asp:PlaceHolder runat="server" ID="uxPreviewPlaceHolder">
					<div class="formWhole"><span class="label">Preview </span>
						<asp:Literal runat="server" ID="uxPreviewLiteral"></asp:Literal>
					</div>
				</asp:PlaceHolder>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="uxImagePlaceHolder">
				<div class="formWhole"><span class="label">Image<span class="asterisk">*</span><span class="tooltip"><span>Image must be of type: .gif,.jpg,.jpeg,.png.<br />
					<br />
					Suggested resizing tool:
					<a href="http://faststone.org/FSViewerDetail.htm" target="_blank">FastStone</a></span></span>
                        <span id="spnFeaturedDisplay" runat="server" style="display:none;">
                            <span>For a featured fine property, the recommended featured image size is 1900x588</span>
                            <span>Recommended gallery image size is 425x275</span>
                            <span>* The first image in the gallery will be used as the featured image.</span>
                        </span>
                        <span id="spnNormalDisplay" runat="server" style="display:none;">
                            <span>Recommended gallery image size is 425x275</span>
                        </span>
				    </span>
					<Controls:FileUpload runat="server" ID="uxImage" ImageWidth="425" ImageHeight="275" AllowedFileTypes=".gif,.jpg,.jpeg,.png" Required="true" RequiredErrorMessage="Image is required." AllowExternalImageLink="true" IsMultipleEnabled="true" />
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="uxThumbnailPlaceholder">
				<div class="formWhole">
					<label for="<%=uxThumbnailImage.ClientID%>">
						Thumbnail
					</label>
					<asp:Image runat="server" ID="uxThumbnailImage" />
					<div class="clear"></div>
				</div>
			</asp:PlaceHolder>
			<div class="formHalf">
				<label for="<%=uxCaption.ClientID%>">
					Caption <span>Must be less than 50 characters</span>
				</label>
				<asp:TextBox CssClass="text" ID="uxCaption" MaxLength="50" runat="server" />
				<asp:RegularExpressionValidator runat="server" ID="uxCaptionRegexVal" ControlToValidate="uxCaption" ErrorMessage="Caption is too long.  It must be 50 characters or less." ValidationExpression="^[\s\S]{0,50}$" />
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
