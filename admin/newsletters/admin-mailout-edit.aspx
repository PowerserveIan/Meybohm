<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-mailout-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_MailoutEdit" Title="Admin - Mailout Add/Edit" %>

<%@ Register TagPrefix="EmailSender" TagName="Progress" Src="~/Controls/Newsletters/NewsletterProgressWindow.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<div class="title">
		<h1>Add New Mailout</h1>
	</div>
	<ul class="breadcrumbs clearfix">
		<li class="firstBreadcrumb">
			<a runat="server" href="~/admin/" title="Home">Dashboard</a></li>
		<li>
			<a runat="server" href="~/admin/newsletters/admin-newsletter.aspx">Newsletter Manager</a></li>
		<li class="currentBreadcrumb">Add New Mailout</li>
	</ul>
	<a id='<%=BaseCode.Helpers.PageView.PageAnchors.center.ToString()%>'></a>
	<asp:PlaceHolder ID="SentSuccessMessage" runat="server" Visible="false">
		<h3 class="success">Your mailout was sent at
			<asp:Literal ID="LastSaveDate" runat="server"></asp:Literal></h3>
	</asp:PlaceHolder>
	<ul class="tabListNav clearfix">
		<li>
			<asp:LinkButton ID="Tab1LinkButton" CommandName="SetTab" CommandArgument="1" CausesValidation="true" runat="server">Select Design</asp:LinkButton>
		</li>
		<li>
			<asp:LinkButton ID="Tab2LinkButton" CommandName="SetTab" CommandArgument="2" CausesValidation="true" runat="server">Approve Layout</asp:LinkButton>
		</li>
		<li>
			<asp:LinkButton ID="Tab3LinkButton" CommandName="SetTab" CommandArgument="3" CausesValidation="true" runat="server">Send</asp:LinkButton>
		</li>
	</ul>
	<div class="tabContent">
		<asp:PlaceHolder ID="Tab1Instructions" runat="server" Visible="false">
			<p class="paddingLeft">
				<strong>Step 1: Select Design - </strong>In this step you select the newsletter's design template and preview the look-and-feel before sending.
			</p>
			<p class="paddingLeft">
				<em>Note: You can
					<asp:HyperLink ID="NewsletterEditLink" runat="server" NavigateUrl="admin-newsletter-edit.aspx"> edit
					the content of the newsletter here</asp:HyperLink>.</em>
			</p>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="Tab2Instructions" runat="server" Visible="false">
			<p class="paddingLeft">
				<strong>Step 2: Approve Layout (optional) -</strong> Optionally send an approval email. This is recommended to confirm the look, feel, and content of the newsletter are just as you intended.
			</p>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="Tab3Instructions" runat="server" Visible="false">
			<p class="paddingLeft">
				<strong>Step 3: Send - </strong>In this step you select the mailing list(s) to send the newsletter to.
			</p>
			<p class="paddingLeft">
				<em>Note: Clicking on the name of the mailing list will allow you to preview the list in a new window. </em>
			</p>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="Tab1" runat="server" Visible="false">
			<!-- required fields -->
			<div class="requiredFields"><span class="asterisk">*</span> required fields</div>
			<div class="formWrapper">
				<div class="formHalf">
					<label for="<%=uxNewsletterDesign.ClientID%>">
						Design Template
					</label>
					<asp:DropDownList CssClass="text" ID="uxNewsletterDesign" runat="server" AutoPostBack="true" />
					<asp:RequiredFieldValidator ID="uxNewsletterDesignReqFVal" runat="server" InitialValue="" ControlToValidate="uxNewsletterDesign" Text="*Design Template is required" ErrorMessage="(HTML Design) Design Template is required." />
				</div>
				<div class="formWhole">
					<p>
						HTML Preview
					</p>
					<div class="formRightColumn borderAllNewsletter">
						<asp:Literal ID="uxHtmlPreview" runat="server" />
						<asp:PlaceHolder ID="NoDesignSelectedText" runat="server"><span>Please select a design from the dropdown above.</span> </asp:PlaceHolder>
					</div>
				</div>
				<asp:PlaceHolder ID="TextPreviewPH" runat="server">
					<div class="formWhole">
						<p>
							Text Preview
						</p>
						<div class="formRightColumn borderAllNewsletter">
							<asp:Literal ID="uxTextPreview" runat="server" />
						</div>
						<!--end formRightColumn-->
						<div class="clear"></div>
					</div>
				</asp:PlaceHolder>
			</div>
			<div class="clear"></div>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="Tab2" runat="server" Visible="false">
			<!-- required fields -->
			<div class="requiredFields"><span class="asterisk">*</span> required fields</div>
			<!-- validation summary -->
			<asp:ValidationSummary ID="Validationsummary1" ValidationGroup="SendApprovalEmailValidationGroup" CssClass="validation" runat="server" DisplayMode="BulletList" HeaderText="<h3 class='failure'>Please correct the following errors:</h3>" ForeColor="#CC0000" />
			<div class="formWrapper">
				<div class="formHalf">
					<label for="<%=uxApprovalEmail.ClientID%>">
						Approval Email Address<br />
						<span>(Comma separated list) </span>
					</label>
					<asp:TextBox CssClass="text" runat="server" ID="uxApprovalEmail" MaxLength="382" />
					<asp:RequiredFieldValidator runat="server" ValidationGroup="SendApprovalEmailValidationGroup" ID="uxEmailReqFVal" ControlToValidate="uxApprovalEmail" ErrorMessage="Email is required." />
					<asp:CustomValidator runat="server" ValidationGroup="SendApprovalEmailValidationGroup" OnServerValidate="uxEmailRegexVal_ServerValidate" ID="uxEmailRegexVal" ControlToValidate="uxApprovalEmail" ErrorMessage="Invalid email format." />
					<asp:Button runat="server" ID="SendEmailButton" Text="Send Email" CssClass="button emailBtn" OnClick="SendApprovalEmails" CausesValidation="true" ValidationGroup="SendApprovalEmailValidationGroup" />
				</div>
				<asp:PlaceHolder ID="approvalEmailSentPH" runat="server" Visible="false">
					<div class="formHalf">
						<div class="formRightColumn">Approval emails sent to <strong>
							<asp:Literal ID="approvalEmailListLiteral" runat="server" />
						</strong>. </div>
					</div>
				</asp:PlaceHolder>
			</div>
			<div class="clear"></div>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="Tab3" runat="server" Visible="false">
			<!-- required fields -->
			<div class="requiredFields"><span class="asterisk">*</span> required fields</div>
			<div class="formWrapper">
				<div class="formHalf">
					<label for="<%=uxMailingLists.ClientID%>">
						Send to Mailing List(s)
						<span>Email format:
							<%=Classes.Newsletters.NewsletterSystem.GetPlainEnglishSendingFormat()%></span>
					</label>
					<asp:CustomValidator runat="server" ID="uxMailingListsRequired" OnServerValidate="uxMailingLists_ServerValidate" ErrorMessage="(Send) You must select at least one mailing list to send to." Text="*You must select at least one mailing list to send to." ValidationGroup="SendNewsletterValidationGroup" />
					<asp:CheckBoxList ID="uxMailingLists" runat="server" CssClass="chbox_list">
					</asp:CheckBoxList>
					<asp:Literal ID="mailingListTextDesign" runat="server" Visible="false"> <a href="admin-mailing-list-edit.aspx?id={0}" target="_blank">{1}</a> </asp:Literal>
					<EmailSender:Progress runat="server" ID="uxProgressWindow" Visible="false" />
					<asp:PlaceHolder ID="noMailingListsMessage" runat="server">No active mailing lists exist (
						<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="admin-mailing-list.aspx">Mailing
							List Manager</asp:HyperLink>
						). </asp:PlaceHolder>
				</div>
			</div>
			<div class="clear"></div>
		</asp:PlaceHolder>
		<div class="clear"></div>
	</div>
	<!--end tabContent-->
	<!-- button container -->
	<div class="btnContainer horizForm">
		<asp:Button ID="uxBack" runat="server" Text="Back" CausesValidation="false" CssClass="button floatLeft backBtn" />
		<asp:Button ID="uxNext" runat="server" Text="Next" CssClass="button floatLeft fwdBtn" />
		<asp:Button ID="uxSendNewsletter" runat="server" Text="Send Newsletter" Visible="false" ValidationGroup="SendNewsletterValidationGroup" CssClass="button floatLeft emailBtn" OnClientClick="$('.floatingBox').show();$('#divContent').html('Preparing emails to send<br />');$(this).hide();" />
		<div class="clear"></div>
	</div>
	<br />
	<asp:Repeater ID="uxBadEmailsRepeater" runat="server" Visible="false" ItemType="Classes.Newsletters.Subscriber">
		<HeaderTemplate>
			<h3 class='errorSummaryHeader'>Sending Error</h3>
			Emails were not sent to the following addresses due to a formatting error
			<br />
		</HeaderTemplate>
		<ItemTemplate>
			<asp:Label runat="server" ID="uxErrorText" ForeColor="#CC0000" Text='<%#Item.Email + " is not a valid email address"%> ' />
			<br />
		</ItemTemplate>
	</asp:Repeater>
	<asp:Label ID="uxNoActionTaken" runat="server" Visible="false" ForeColor="#CC0000" Text="<H3 class='errorSummaryHeader'>Warning!</H3>There are no subscribers in this mailing list, no action was taken" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		$(document).ready(function () {
			$("#<%= uxNewsletterDesign.ClientID %>").change(function () {
				suppressOnBeforeUnload = true;
			});
		});
	</script>
</asp:Content>
