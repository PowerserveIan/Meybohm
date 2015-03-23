<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-news-press-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminNewsPressEdit" Title="Admin - Newspress Add/Edit" %>

<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="DateTimePicker" Src="~/Controls/BaseControls/DateTimePicker.ascx" %>
<%@ Register TagPrefix="Controls" TagName="RichTextEditor" Src="~/Controls/BaseControls/RichTextEditor.ascx" %>
<%@ Register TagPrefix="SEOComponent" TagName="CurrentPageSEO" Src="~/Controls/SEOComponent/SEO_Data_Entry.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<asp:PlaceHolder runat="server" ID="uxCategoriesDontExistPlaceHolder">Please add an enabled category via the
		<a href="admin-news-press-category-edit.aspx?id=0">Category Manager</a>
		before you create a News Press article. </asp:PlaceHolder>
	<asp:PlaceHolder runat="server" ID="uxCategoriesExistPlaceHolder">
		<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
		<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
		<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
			<div class="blue padded optionsList">
				<ul class="inputList checkboxes horizontal">
					<li>
						<asp:CheckBox runat="server" ID="uxFeatured" Text="Is Featured" /></li>
					<li>
						<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Is Active" /></li>
					<asp:PlaceHolder ID="uxArchivePH" runat="server">
						<li>
							<asp:CheckBox runat="server" ID="uxArchived" Text="Is Archived" /></li>
					</asp:PlaceHolder>
				</ul>
				<div class="clear"></div>
			</div>
			<div class="formWrapper">
				<div class="formHalf">
					<label for="<%=uxTitle.ClientID%>">
						Title<span class="asterisk">*</span>
					</label>
					<asp:TextBox CssClass="text" ID="uxTitle" MaxLength="255" runat="server" TextMode="SingleLine" />
					<asp:RegularExpressionValidator runat="server" ID="uxTitleRegexVal" ControlToValidate="uxTitle" ErrorMessage="Title is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
					<asp:RequiredFieldValidator runat="server" ID="uxTitleReqFVal" ControlToValidate="uxTitle" ErrorMessage="Title is required." />
					<asp:CustomValidator ID="uxTitleUniqueValidator" runat="server" ControlToValidate="uxTitle" ErrorMessage="Title is already in use, please choose another." />
				</div>
				<div class="formHalf">
					<label for="<%=uxAuthor.ClientID%>">
						Author
					</label>
					<asp:TextBox CssClass="text" ID="uxAuthor" MaxLength="255" runat="server" TextMode="SingleLine" />
					<asp:RegularExpressionValidator runat="server" ID="uxAuthorRegexVal" ControlToValidate="uxAuthor" ErrorMessage="Author is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
				</div>
				<div class="formWhole">
					<label for="<%=uxStoryHTML.ClientID%>">
						Story HTML<span class="asterisk">*</span>
					</label>
					<Controls:RichTextEditor runat="server" ID="uxStoryHTML" FieldName="Story HTML" Required="true" />
				</div>
				<div class="formWhole">
					<label for="<%=uxSummary.ClientID%>">
						Summary<span class="asterisk">*</span>
					</label>
					<asp:TextBox runat="server" ID="uxSummary" MaxLength="1000" CssClass="text" TextMode="MultiLine" />
					<asp:RegularExpressionValidator runat="server" ID="uxSummaryRegexVal" ControlToValidate="uxSummary" ErrorMessage="Summary is too long.  It must be 1000 characters or less." ValidationExpression="^[\s\S]{0,1000}$" />
					<asp:RequiredFieldValidator runat="server" ID="uxSummaryReqFVal" ControlToValidate="uxSummary" ErrorMessage="Summary is required." />
				</div>
				<div class="formHalf">
					<label for="<%=uxDate.ClientID%>">
						Date Published<span class="asterisk">*</span>
					</label>
					<Controls:DateTimePicker runat="server" ID="uxDate" TextBoxCssClass="text" RequiredErrorMessage="Date Published is required." />
				</div>
			</div>
			<asp:PlaceHolder runat="server" ID="uxCategoryPlaceHolder">
				<div class="rightCol">
					<div class="formHalf"><span class="label">Category<span class="asterisk">*</span> </span>
						<asp:CheckBoxList ID="uxCategory" runat="server" CssClass="inputList checkboxes" RepeatLayout="UnorderedList" />
						<asp:CustomValidator runat="server" ID="uxCategoryRequired" OnServerValidate="uxCategory_ServerValidate" ErrorMessage="You must select at least one category." Text="*You must select at least one category." />
					</div>
				</div>
			</asp:PlaceHolder>
			<div class="clear"></div>
			<SEOComponent:CurrentPageSEO ID="uxSEOData" runat="server" SitePageLinkSetupType="PageFormatter" PageLinkFormatter="~/news-press-details.aspx?id={0}" />
			<!-- button container -->
			<div class="buttons">
				<%--The markup for the buttons is in the BaseEditPage--%>
				<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
			</div>
		</asp:Panel>
	</asp:PlaceHolder>
</asp:Content>