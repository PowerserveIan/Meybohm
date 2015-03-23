<%@ Page MasterPageFile="~/admin/admin.master" Language="c#" Inherits="ContentManager2.Admin.ContentManagerPage" CodeFile="content-manager-page.aspx.cs" Title="Admin - Page Properties" %>

<%@ Import Namespace="Classes.ContentManager" %>
<%@ Register TagPrefix="SEOComponent" TagName="CurrentPageSEO" Src="~/Controls/SEOComponent/SEO_Data_Entry.ascx" %>
<%@ Register TagName="Toggle" TagPrefix="Language" Src="~/Controls/BaseControls/LanguageToggleAdmin.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<asp:PlaceHolder runat="server" ID="uxUnapprovedPlaceHolder" Visible="false">
			<asp:Label runat="server" ID="uxMessage" />
			<div class="approvalDiv">
				<a runat="server" id="uxShowApprovalDetails" class="approvalLink" href="#" onclick="$('div.editedByDiv').show();return false;">Show Approval Details</a>
				<asp:LinkButton runat="server" ID="uxLiveContent" Text="View Live Page" CausesValidation="false"></asp:LinkButton>
				<asp:LinkButton runat="server" ID="uxUnapprovedContent" Text="View Unapproved Page" CausesValidation="false"></asp:LinkButton>
				<asp:Label runat="server" ID="uxFlaggedForDeletion" Font-Bold="true" Visible="false" />
				<div class="editedByDiv" style="display: none;">
					<a class="close" href="#" onclick="$('div.editedByDiv').hide();return false;">
						<img runat="server" src="~/img/btn_close.gif" alt="close" /></a>Last Edited:
					<asp:Literal runat="server" ID="uxLastEditedDate"></asp:Literal>
					<br />
					Edited By:
					<asp:Literal runat="server" ID="uxAllEditors"></asp:Literal>
				</div>
			</div>
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="uxHideForNonDefaultLanguageTop">
			<div class="blue padded optionsList">
				<asp:CheckBox runat="server" ID="uxFeaturedPage" Checked="<%#ds.Page.FeaturedPage%>" CssClass="chbox horiz" Text="Featured Page<span>This page will appear in areas of your site that highlight CMS pages</span>" />
				<div class="clear"></div>
			</div>
		</asp:PlaceHolder>
		<div class="formWrapper">
			<asp:PlaceHolder runat="server" ID="uxLanguageTogglePH">
				<div class="formHalf">
					<label for="<%=uxLanguageToggle.ClientID%>">
						Language <span class="notice">Changing the Language will take you to another page, and you will lose any unsaved content.</span>
					</label>
					<Language:Toggle ID="uxLanguageToggle" runat="server" CssClass="text" />
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="uxMicrositeNamePH">
				<div class="formHalf"><span class="label">Microsite Name</span>
					<asp:Literal runat="server" ID="uxMicrositeName" Text="N/A" />
				</div>
			</asp:PlaceHolder>
			<div class="formWhole">
				<label for="<%=uxTitle.ClientID%>">
					Page Header Title<span class="asterisk">*</span> <span>This is used for display only, if you want to change the actual title tag of the page, do so below in the SEO section </span>
				</label>
				<asp:TextBox runat="server" ID="uxTitle" TextMode="SingleLine" MaxLength="150" Text='<%#Settings.EnableMultipleLanguages ? ds.PageTitle.Title : ds.Page.Title%>' CssClass="text" />
				<asp:RegularExpressionValidator runat="server" ID="uxTitleRegexVal" ControlToValidate="uxTitle" ErrorMessage="Title is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
				<asp:RequiredFieldValidator runat="server" ID="TitleValidator" ControlToValidate="uxTitle" ErrorMessage="Title is required." />
				<asp:CustomValidator ID="uxTitleUniqueValidator" runat="server" ControlToValidate="uxTitle" ErrorMessage="Page title is already in use, please choose another." />
			</div>
			<asp:PlaceHolder runat="server" ID="uxHideForNonDefaultLanguage">
				<asp:PlaceHolder runat="server" ID="uxRolesEditPlaceHolder" Visible="false">
					<div class="formHalf">
						<label for="<%=uxRolesEditList.ClientID%>">
							Allow these roles to <strong>edit</strong> this page
						</label>
						<asp:CheckBoxList ID="uxRolesEditList" runat="server" CssClass="chbox_list" />
						<div class="clear"></div>
					</div>
				</asp:PlaceHolder>
				<asp:PlaceHolder runat="server" ID="uxRolesPlaceHolder" Visible="false">
					<div class="formHalf">
						<label for="<%=uxRolesList.ClientID%>">
							Allow these roles to <strong>view</strong> this page <span class="tooltip"><span>If no roles are checked, all visitors to your site will be able to access this page
								<%
									if (!CMSHelpers.HasFullCMSPermission() && Settings.EnableApprovals)
									{%>
								<br />
								If a role is greyed out, then that role is set as an Editor and has to be able to view the page.
								<%
									}%>
							</span></span>
						</label>
						<asp:CheckBoxList ID="uxRolesList" runat="server" CssClass="chbox_list" />
						<div class="clear"></div>
					</div>
				</asp:PlaceHolder>
				<asp:PlaceHolder runat="server" ID="uxTemplatePlaceHolder">
					<div class="formHalf"><span class="label">Template </span>
						<asp:Label ID="lblTemplate" runat="server"><%#ds.Templates.Where(t => t.CMTemplateID == ds.Page.CMTemplateID).FirstOrDefault() == null ? "" : ds.Templates.Where(t => t.CMTemplateID == ds.Page.CMTemplateID).FirstOrDefault().Name%></asp:Label>
					</div>
				</asp:PlaceHolder>
				<asp:PlaceHolder runat="server" ID="uxDynamicCollectionPH" Visible="false">
					<div class="formHalf">
						<label for="<%=uxDynamicCollection.ClientID%>">
							Dynamic Header Collection
						</label>
						<asp:DropDownList runat="server" ID="uxDynamicCollection" AppendDataBoundItems="true">
							<asp:ListItem Text="--None--" Value=""></asp:ListItem>
						</asp:DropDownList>
					</div>
				</asp:PlaceHolder>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="uxEditPH">
				<div class="formHalf">
					<asp:Literal runat="server" ID="uxNewRecordPageName" Text="<label for=''>Page Name<span class='asterisk'>*</span><span>May only contain alphanumeric characters and hyphens.</span></label>"></asp:Literal>
					<asp:Literal runat="server" ID="uxEditPageLit" Text="<span class='label'>Edit This Page</span>"></asp:Literal>
					<asp:TextBox ID="Filename" CssClass="form text" runat="server" Visible="<%#PageId == 0%>" Text='<%#ds.Page.FileName%>' Enabled="<%#PageId == 0%>" />
					<% if (PageId != 0)
		{%><a href='../../<%#(ds.Page.CMMicrositeID.HasValue && ds.Page.CMMicrositeID.Value > 0 ? CMMicrosite.GetByID(ds.Page.CMMicrositeID.Value).Name.ToLower().Replace(" ", "-") + "/" + ds.Page.FileName : ds.Page.FileName)%>'>
		   <%#ds.Page.FileName%></a>
					<%	}%>
					<asp:CustomValidator ID="FilenameCustomValidator" runat="server" ControlToValidate="Filename" ErrorMessage="Page name already in use, use another." Enabled="<%#FilenameCustomValidator.Enabled && PageId == 0%>" />
					<asp:RegularExpressionValidator ID="FilenameNoExtensionREV" runat="server" ControlToValidate="Filename" Enabled="<%#FilenameNoExtensionREV.Enabled && PageId == 0%>" ErrorMessage="Please enter a valid Page name."
						ValidationExpression="^([\w+\-]){1,}" />
					<asp:RegularExpressionValidator ID="FilenameASPXExtensionREV" runat="server" ControlToValidate="Filename" Enabled="<%#FilenameASPXExtensionREV.Enabled && PageId == 0%>" ErrorMessage="Please enter a valid Page name ending in .aspx."
						 ValidationExpression="^([\w+\-]){1,}\.aspx" />
					<asp:RequiredFieldValidator ID="FilenameValidator" runat="server" ControlToValidate="Filename" Enabled="<%#FilenameValidator.Enabled && PageId == 0%>" ErrorMessage="The page requires a filename." />
				</div>
			</asp:PlaceHolder>
		</div>
		<div class="clear"></div>
		<asp:PlaceHolder runat="server" ID="uxHideForNonDefaultLanguage2">
			<div class="sectionTitle">
				<div class="bottom">
					<h2>Form</h2>
					<h4>This section is only applicable if you insert a form into your page</h4>
				</div>
			</div>
			<div class="formWrapper">
				<div class="formHalf">
					<label for="<%=FormRecipient.ClientID%>">
						Form Recipient<span class="tooltip"><span>This is where you will put the email address of recipients of the form data, if this is left blank your form will not submit data anywhere. If you want to input multiple recipients please separate the email addresses
							with a comma.</span></span>
					</label>
					<asp:TextBox ID="FormRecipient" CssClass="form text" runat="server" Text='<%#ds.Page.FormRecipient%>'/>
					<asp:RegularExpressionValidator ID="FormRecipientREV" runat="server" ControlToValidate="FormRecipient" ErrorMessage="Invalid email address(s). Please separate addresses with a comma and a space." ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"
						 />
					<div class="clear"></div>
				</div>
				<div class="formHalf">
					<label for="<%=ResponsePage.ClientID%>">
						Response Page<span class="tooltip"><span>This is where the user will be redirected to after submitting form data</span></span>
					</label>
					<asp:DropDownList ID="ResponsePage" CssClass="form text" runat="server" DataSource="<%#ds.Pages.OrderBy(p => p.Title).ToList()%>" DataValueField="CMPageID" DataTextField="Title">
					</asp:DropDownList>
					<div class="clear"></div>
				</div>
			</div>
			<div class="clear"></div>
		</asp:PlaceHolder>
		<SEOComponent:CurrentPageSEO ID="uxSEOData" runat="server" SitePageLinkSetupType="PageUrl" ShowFriendlyFilename="false" />
		<!-- button container -->
		<div class="buttons">
			<%--The markup for the buttons is in the BaseEditPage--%>
			<asp:Label runat="server" ID="uxSaveDisabled" Text="You should make your changes to the unapproved page" ForeColor="Red" Visible="false" CssClass="floatRight" />
			<asp:Button ID="uxNext" runat="server" Text="Next Page" CssClass="button floatRight next" CausesValidation="false" />
			<asp:Button ID="uxPrevious" runat="server" Text="Previous Page" CssClass="button floatRight prev" CausesValidation="false" />
			<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
		</div>
	</asp:Panel>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		//<![CDATA[
		var dynamicfile=<%#Convert.ToString(PageId == 0 && IsPostBack == false).ToLower()%>;
		var templatepath='';
		if(dynamicfile)
			if($('#<%=Filename.ClientID%>').val() != '' && $('#<%=Filename.ClientID%>').val().indexOf('/')==0) 
				dynamicfile=false;

		$(document).ready(function(){
			$("#<%= uxTitle.ClientID %>").keyup(function(){
				if (dynamicfile)
					$('#<%= Filename.ClientID %>').val(templatepath + $(this).val().replace(/ /g, '-').replace(/[^a-z0-9\-]/ig,'').toLowerCase()<% if (BaseCode.Globals.Settings.RequireASPXExtensions){ %>+ '.aspx'<% } %>);
			});
		});
		//]]>
	</script>
</asp:Content>
