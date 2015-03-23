<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContentRegion.ascx.cs" Inherits="Controls_ContentManager_ContentRegion" %>
<asp:Literal runat="server" ID="uxScripts" Text="~/tft-js/core/tiny_mce/jquery.tinymce.js,~/tft-js/core/tinymceinit.js"></asp:Literal>
<asp:PlaceHolder runat="server" ID="uxHoverOffsetOpenPH">
	<div class="CMWrapper">
		<asp:Literal runat="server" ID="uxHoverDivStyle"></asp:Literal>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="uxTablePlaceHolder">
	<table id="tblHolder" class="CMTop" cellspacing="0" cellpadding="2" border="0" runat="server" style="display: none;">
		<tr>
			<td>
				<asp:CheckBox ID="uxGlobalChangeCheckBox" runat="server" Text="This is a Global Region (uncheck this box to change this page only)."></asp:CheckBox>
			</td>
		</tr>
	</table>
</asp:PlaceHolder>
<asp:HyperLink runat="server" ID="uxToggleRadEditor" Text="Edit this Content" CssClass="editButton" NavigateUrl="#"></asp:HyperLink>
<asp:PlaceHolder runat="server" ID="uxUnapprovedPlaceHolder" Visible="false">
	<div class="approvalMenu">
		<div class="notification">
			<asp:Label runat="server" ID="uxMessage" />
			<asp:LinkButton runat="server" ID="uxLiveContent" Text="View Live Content &raquo;"></asp:LinkButton>
			<asp:LinkButton runat="server" ID="uxUnapprovedContent" Text="View Unapproved Content &raquo;"></asp:LinkButton>
			<br />
			<a runat="server" class="showApprovalDetails" id="uxShowApprovalDetails" href="#">Approval Details</a>
			<div class="editedByDiv" style="display: none;">Last Edited: <span class="editTime">
				<asp:Literal runat="server" ID="uxLastEditedDate"></asp:Literal></span>
				<br />
				Edited By:
				<asp:Literal runat="server" ID="uxAllEditors"></asp:Literal>
			</div>
		</div>
	</div>
	<!--end approvalMenu-->
	<script type="text/javascript">
		//<![CDATA[
		$(document).ready(function () {
			$("a#<%=uxShowApprovalDetails.ClientID%>").click(function () {
				$(this).toggleClass("down")
				$(this).siblings("div.editedByDiv").slideToggle();
				return false;
			});
		});
		//]]>
	</script>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="uxDraftPH" Visible="false">
	<span>You are viewing your most recently saved draft</span>
</asp:PlaceHolder>
<div runat="server" id="editableContent">
	<asp:Literal runat="server" ID="uxContent"></asp:Literal>
</div>
<asp:TextBox runat="server" ID="uxEditor" CssClass="tinymce hidden" TextMode="MultiLine" />
<asp:Button runat="server" ID="uxCancel" Text="Cancel" CssClass="cancel" Style="display: none;" CausesValidation="false" />
<asp:Button runat="server" ID="uxSubmit" Text="Submit" CssClass="cancel" Style="display: none;" CausesValidation="false" />
<asp:Button runat="server" ID="uxSaveAsDraft" Text="Save as Draft" CssClass="cancel" Style="display: none;" CausesValidation="false" />
<asp:PlaceHolder runat="server" ID="uxHoverOffsetClosePH"></div> </div> </asp:PlaceHolder>
