<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-builder-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminBuilderEdit" %>

<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="RichTextEditor" Src="~/Controls/BaseControls/RichTextEditor.ascx" %>
<%@ Register TagPrefix="SEOComponent" TagName="CurrentPageSEO" Src="~/Controls/SEOComponent/SEO_Data_Entry.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Active" />
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%= uxName.ClientID %>">
					Name<span class="asterisk">*</span><br />
					<span>Must be less than 255 characters</span></label>
				<asp:TextBox runat="server" ID="uxName" MaxLength="255" CssClass="text" />
				<asp:RequiredFieldValidator runat="server" ID="uxNameReqFVal" ControlToValidate="uxName" ErrorMessage="Name is required." />
				<asp:RegularExpressionValidator runat="server" ID="uxNameRegexVal" ControlToValidate="uxName" ErrorMessage="Name is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
			</div>
			<div class="formHalf">
				<label for="<%= uxOwnerName.ClientID %>">
					Owner Name<br />
					<span>Must be less than 500 characters</span></label>
				<asp:TextBox runat="server" ID="uxOwnerName" MaxLength="255" CssClass="text" />
				<asp:RegularExpressionValidator runat="server" ID="uxOwnerNameRegexVal" ControlToValidate="uxOwnerName" ErrorMessage="Owner Name is too long.  It must be 500 characters or less." ValidationExpression="^[\s\S]{0,500}$" />
			</div>
			<div class="formWhole">
				<label for="<%= uxImage.ClientID %>">
					Image<span>Optimal image size is 400x300</span></label>
				<Controls:FileUpload runat="server" ID="uxImage" Required="False" AllowedFileTypes=".gif,.jpg,.jpeg,.png" AllowExternalImageLink="true" UploadToLocation="~/uploads/builders" ImageHeight="300" ImageWidth="400" />
			</div>
			<div class="formHalf">
				<label for="<%= uxWebsite.ClientID %>">
					Website</label>
				<asp:TextBox runat="server" ID="uxWebsite" MaxLength="1000" CssClass="text" />
				<asp:RegularExpressionValidator runat="server" ID="uxWebsiteRegexVal" ControlToValidate="uxWebsite" ErrorMessage="Website is too long.  It must be 1000 characters or less." ValidationExpression="^[\s\S]{0,1000}$" />
			</div>
			<div class="formWhole">
				<label for="<%= uxInfo.ClientID %>">
					Info<br />
					<span>Must be less than 1000 characters</span></label>
				<Controls:RichTextEditor runat="server" ID="uxInfo" FieldName="Info" MaxLength="1000" HideEditorInitially="true" />
			</div>
			<asp:PlaceHolder runat="server" ID="uxAfterSavePH">
				<table class="listing paddingBottom" id="neighborhoods">
					<thead>
						<tr>
							<th>Action</th>
							<th>Neighborhood Name</th>
						</tr>
					</thead>
					<tbody>
						<asp:Repeater runat="server" ID="uxExistingNeighborhoods" ItemType="Classes.MLS.NeighborhoodBuilder">
							<ItemTemplate>
								<tr>
									<td>
										<asp:LinkButton ID="uxDelete" runat="server" OnCommand="Neighborhood_Command" CommandName="Delete" CommandArgument='<%#Item.NeighborhoodBuilderID%>' OnClientClick="return confirm('Are you sure you want to delete this neighborhood?');" CssClass="button delete"
											Text="<span>Delete</span>" CausesValidation="false" />
									</td>
									<td><%# Item.Neighborhood.Name %></td>
								</tr>
							</ItemTemplate>
						</asp:Repeater>
						<tr>
							<td>
								<asp:LinkButton runat="server" ID="uxAdd" CssClass="button add" Text="<span>Add</span>" OnCommand="Neighborhood_Command" CommandName="Add" ValidationGroup="AddNeighborhood" />
							</td>
							<td>
								<asp:DropDownList runat="server" ID="uxNeighborhoods">
								</asp:DropDownList>
								<asp:RequiredFieldValidator runat="server" ID="uxNeighborhoodsRFV" ControlToValidate="uxNeighborhoods" ErrorMessage="You must select a neighborhood." ValidationGroup="AddNeighborhood" />
							</td>
						</tr>
					</tbody>
				</table>
			</asp:PlaceHolder>
		</div>		
		<div class="rightCol">
			<div class="formHalf"><span class="label">Markets</span>
				<asp:CheckBoxList ID="uxMicrosites" runat="server" CssClass="inputList checkboxes" RepeatLayout="UnorderedList" />
			</div>
		</div>
		<div class="clear"></div>
		<SEOComponent:CurrentPageSEO ID="uxSEOData" runat="server" SitePageLinkSetupType="PageFormatter" PageLinkFormatter="~/builder-details.aspx?id={0}" />
		<!-- button container -->
		<div class="buttons">
			<%--The markup for the buttons is in the BaseEditPage--%>
			<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
		</div>
	</asp:Panel>
</asp:Content>
