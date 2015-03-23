<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-newsletter-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_NewsletterEdit" Title="Admin - Newsletter Add/Edit" %>

<%@ Register TagPrefix="SEOComponent" TagName="CurrentPageSEO" Src="~/Controls/SEOComponent/SEO_Data_Entry.ascx" %>
<%@ Register TagPrefix="Controls" TagName="DateTimePicker" Src="~/Controls/BaseControls/DateTimePicker.ascx" %>
<%@ Register TagPrefix="Controls" TagName="RichTextEditor" Src="~/Controls/BaseControls/RichTextEditor.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<ul class="tabListNav clearfix">
		<li>
			<asp:LinkButton ID="Tab1LinkButton" CommandName="SetTab" CommandArgument="1" CausesValidation="false" runat="server">Basic Info</asp:LinkButton>
		</li>
		<li>
			<asp:LinkButton ID="Tab2LinkButton" CommandName="SetTab" CommandArgument="2" CausesValidation="false" runat="server">HTML Edit</asp:LinkButton>
		</li>
		<li>
			<asp:LinkButton ID="Tab3LinkButton" CommandName="SetTab" CommandArgument="3" CausesValidation="false" runat="server">Text Edit</asp:LinkButton>
		</li>
		<li>
			<asp:LinkButton ID="Tab4LinkButton" CommandName="SetTab" CommandArgument="4" CausesValidation="false" runat="server">Save &amp; Finish</asp:LinkButton>
		</li>
	</ul>
	<!--end tabListNav-->
	<div class="clear"></div>
	<div class="tabContent">
		<asp:PlaceHolder ID="Tab1Instructions" runat="server" Visible="false">
			<p class="paddingLeft">
				<strong>Step 1: Basic Info -</strong> In this step you define the basic newsletter information. When you are finished, click the &quot;Next&quot; button below or select the corresponding tab of your choice. You will then be able to add the newsletter body's
				HTML and plain text content.
			</p>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="Tab2Instructions" runat="server" Visible="false">
			<p class="paddingLeft">
				<strong>Step 2: HTML Edit -</strong> In this step you create the HTML content for the newsletter. Sending an HTML newsletter allows you to add images and styling to your newsletter. When you are finished select the &quot;Next&quot; button below, or select
				the corresponding tab of your choice.
			</p>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="Tab3Instructions" runat="server" Visible="false">
			<p class="paddingLeft">
				<strong>Step 3: Text Edit -</strong> In this step you create the text-only content for users who cannot receive HTML newsletters. When you are finished select the &quot;Next&quot; button below, or select the corresponding tab of your choice.
			</p>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="Tab4Instructions" runat="server" Visible="false">
			<p class="paddingLeft">
				<strong>Step
					<asp:Literal ID="FinalStepNumber" runat="server">4</asp:Literal>
					: Finish &amp; Save -</strong> In this step you will review the newsletter. You can still go back to the other steps and make changes before saving. Please verify that all information is correct and then click the &quot;Save Changes&quot; button below.
			</p>
			<asp:PlaceHolder ID="SaveWarning" runat="server">
				<p class="paddingLeft">
					<em><strong><span style="color: #ff0000;">WARNING:</span> Your newsletter has not been saved.</strong> You <strong>must</strong> click the &quot;Save Changes&quot; button below before your newsletter is saved.</em>
				</p>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="SuccessMessage" runat="server" Visible="false">
				<!-- success message -->
				<div id="DIV1">
					<div class="successMessage">
						<h4>Success</h4>
						<p>
							Your newsletter was saved at
							<asp:Literal ID="LastSaveDate" runat="server"></asp:Literal>
						</p>
						<div class="clear"></div>
					</div>
					<!--end successMessage-->
				</div>
			</asp:PlaceHolder>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="Tab1" runat="server" Visible="false">
			<div class="blue padded optionsList">
				<ul class="inputList checkboxes horizontal">
					<li>
						<asp:CheckBox runat="server" ID="uxFeatured" Text="Is Featured<span>Add to &quot;Featured Newsletters&quot; listing</span>" /></li>
					<li>
						<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Is Active<span>Display in Newsletter listing</span>" /></li>
				</ul>
				<div class="clear"></div>
			</div>
			<div class="formWrapper">
				<div class="formWhole">
					<label for="<%=uxTitle.ClientID%>">
						Title<span class="asterisk">*</span><span class="tooltip"><span>Newsletter title AND email subject line</span></span></label>
					<asp:TextBox runat="server" ID="uxTitle" CssClass="text" MaxLength="250" />
					<asp:RequiredFieldValidator runat="server" ID="uxTitleReqFVal" ControlToValidate="uxTitle" ErrorMessage="(Basic Info) Title is required." />
					<asp:RegularExpressionValidator runat="server" ID="uxTitleRegexVal" ControlToValidate="uxTitle" ErrorMessage="(Basic Info) Title is too long.  It must be 250 characters or less." ValidationExpression="^[\s\S]{0,250}$" />
					<asp:CustomValidator ID="uxTitleCV" runat="server" ControlToValidate="uxTitle" ErrorMessage="Title is already taken." OnServerValidate="uxTitleCV_ServerValidate" />
				</div>
				<div class="formHalf">
					<label for="<%= uxCMMicrositeID.ClientID %>">
						Market</label>
					<asp:DropDownList runat="server" ID="uxCMMicrositeID" AppendDataBoundItems="true">
						<asp:ListItem Text="--No Specific Market--" Value=""></asp:ListItem>
					</asp:DropDownList>
				</div>
				<div class="formHalf">
					<asp:RadioButtonList runat="server" ID="uxNewHomes">
						<asp:ListItem Text="New Homes" Value="True"></asp:ListItem>
						<asp:ListItem Text="Existing Homes" Value="False"></asp:ListItem>
					</asp:RadioButtonList>
				</div>
				<div class="formWhole">
					<label for="<%=uxDescription.ClientID%>">
						Summary/Description<span class="asterisk">*</span><span class="tooltip"><span>Only displayed on newsletter listings</span></span></label>
					<asp:TextBox runat="server" ID="uxDescription" TextMode="MultiLine" CssClass="text" MaxLength="500" />
					<asp:RegularExpressionValidator runat="server" ID="uxDescriptionRegexVal" ControlToValidate="uxDescription" ErrorMessage="(Basic Info) Summary/Description is too long.  It must be 500 characters or less." ValidationExpression="^[\s\S]{0,500}$" />
				</div>
				<div class="formHalf">
					<label for="<%=uxIssue.ClientID%>">
						Issue<span class="asterisk">*</span><span class="tooltip"><span>Displayed below the title</span></span></label>
					<asp:TextBox runat="server" ID="uxIssue" MaxLength="50" CssClass="text" />
					<asp:RequiredFieldValidator runat="server" ID="uxIssueReqFVal" ControlToValidate="uxIssue" ErrorMessage="(Basic Info) Issue is required." />
					<asp:RegularExpressionValidator runat="server" ID="uxIssueRegexVal" ControlToValidate="uxIssue" ErrorMessage="(Basic Info) Issue is too long.  It must be 50 characters or less." ValidationExpression="^[\s\S]{0,50}$" />
				</div>
				<div class="formHalf"><span class="label">Display Date<span class="asterisk">*</span><span class="tooltip"><span>Date displayed atop the newsletter</span></span></span>
					<Controls:DateTimePicker runat="server" ID="uxDisplayDate" TextBoxCssClass="text" RequiredErrorMessage="Display Date is required." />
				</div>
				<asp:PlaceHolder runat="server" ID="uxCreationDatePH" Visible="false">
					<div class="formHalf"><span class="label">Creation Date<span class="tooltip"><span>For reference only &ndash; will not be displayed</span></span></span>
						<asp:Label ID="uxCreationDate" runat="server" />
					</div>
				</asp:PlaceHolder>
			</div>
			<div class="clear"></div>
			<SEOComponent:CurrentPageSEO ID="uxSEOData" runat="server" SitePageLinkSetupType="PageFormatter" PageLinkFormatter="~/newsletter-details.aspx?id={0}" />
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="Tab2" runat="server" Visible="false">
			<div class="formWrapper">
				<div class="formHalf">
					<label for="<%=uxNewsletterDesign.ClientID%>">
						Design Template<span class="asterisk">*</span></label>
					<asp:DropDownList ID="uxNewsletterDesign" runat="server" CausesValidation="false" AutoPostBack="true" />
					<asp:RequiredFieldValidator ID="uxNewsletterDesignReqFVal" runat="server" InitialValue="" ControlToValidate="uxNewsletterDesign" ErrorMessage="(HTML Edit) Design Template is required." />
					<% if (true)
		{ %>
					&nbsp;<a class="fancybox.iframe textLinkBtn viewLinkBtn" href="#" onclick="createLink(this);">Preview Template</a>
					<% } %>
				</div>
				<div class="formWhole">
					<label for="<%= uxHtmlBody.ClientID %>">
						HTML Body<span class="asterisk">*</span></label>
					<Controls:RichTextEditor runat="server" ID="uxHtmlBody" FieldName="(HTML Edit) HTML Body" />
				</div>
			</div>
			<div class="clear"></div>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="Tab3" runat="server" Visible="false">
			<div class="formWrapper">
				<div class="formWhole">
					<label for="<%=uxTextBody.ClientID%>">
						Text Body<span class="asterisk">*</span></label>
					<asp:TextBox runat="server" ID="uxTextBody" TextMode="MultiLine" CssClass="text" />
					<asp:RequiredFieldValidator runat="server" ID="uxTextBodyReqFVal" ControlToValidate="uxTextBody" ErrorMessage="(Text Edit) Text Body is required." />
				</div>
			</div>
			<div class="clear"></div>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="Tab4" runat="server" Visible="false">
			<div class="formWrapper">
				<div class="formWhole">
					<label for="<%=uxTextBody.ClientID%>">
						Validation</label>
					<asp:PlaceHolder ID="ValidationFailedPlaceHolder" runat="server" Visible="false">
						<!-- error message -->
						<div class="formRightColumn">The following items must be corrected before you can save the newsletter:
							<asp:ValidationSummary ID="ErrorSummary" CssClass="validation" runat="server" DisplayMode="BulletList" />
						</div>
					</asp:PlaceHolder>
					<asp:PlaceHolder ID="ValidationPassedPlaceHolder" runat="server" Visible="false"><strong>Passed.</strong> Your newsletter has been saved. </asp:PlaceHolder>
					<div class="clear"></div>
				</div>
				<asp:PlaceHolder ID="NewsletterSummary" runat="server" Visible="false">
					<asp:PlaceHolder ID="HTMLPreviewPlaceHolder" runat="server">
						<div class="formWhole"><span class="label">HTML Preview</span>
							<div class="borderAllNewsletter">
								<asp:Literal ID="uxHtmlPreview" runat="server" />
							</div>
						</div>
					</asp:PlaceHolder>
					<asp:PlaceHolder ID="TextPreviewPlaceHolder" runat="server" Visible="false">
						<div class="formWhole"><span class="label">Text Preview</span>
							<div class="borderAllNewsletter">
								<asp:Literal ID="uxTextPreview" runat="server" />
							</div>
						</div>
					</asp:PlaceHolder>
				</asp:PlaceHolder>
			</div>
			<div class="clear"></div>
		</asp:PlaceHolder>
		<div class="clear"></div>
	</div>
	<!--end tabContent-->
	<div class="buttons">
		<asp:Button ID="uxBack" runat="server" Text="Previous Step" CausesValidation="false" CssClass="button back" />
		<asp:Button ID="uxSaveAndFinish" runat="server" Text="Save and Finish" CssClass="button save" />
		<asp:Button ID="uxSave" runat="server" Text="Save" CssClass="button save" />
		<asp:Button ID="uxNext" runat="server" Text="Next Step" CssClass="button next" />
		<div class="clear"></div>
		<asp:LinkButton ID="uxCancel" runat="server" CausesValidation="false" Text="Back to Newsletter Listings" CssClass="icon back" />
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="PageSpecificJS" runat="server">
	<script type="text/javascript">
		//<![CDATA[
		$(document).ready(function () {
			$("a.fancybox\\.iframe").fancybox({
				'width': 642,
				'height': 500,
				'closeClick': false,
				'padding': 0
			});

			function ToggleFeatured() {
				if ($("#<%= uxActive.ClientID %>").is(":checked"))
					$("#<%= uxFeatured.ClientID %>").removeAttr("disabled");
				else
					$("#<%= uxFeatured.ClientID %>").attr("disabled", "disabled").removeAttr("checked");
			}

			ToggleFeatured();
			$("#<%= uxActive.ClientID %>").click(function () {
				ToggleFeatured();
			});

			$("#<%= uxNewsletterDesign.ClientID %>").change(function () {
				suppressOnBeforeUnload = true;
			});
		});

		function createLink(hlink) {
			hlink.href = 'admin-template-preview.aspx?templateid=' + $('#<%=uxNewsletterDesign.ClientID%>').find('option:selected').val();
		}
		//]]>
	</script>
</asp:Content>
