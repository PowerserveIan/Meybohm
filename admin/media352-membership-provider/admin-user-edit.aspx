<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-user-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminUserEdit" Title="Admin - User Add/Edit" %>

<%@ Register Src="~/Controls/BaseControls/ProfileControl.ascx" TagPrefix="User" TagName="Profile" %>
<%@ Register Src="~/Controls/Media352_MembershipProvider/StaffProfile.ascx" TagPrefix="Staff" TagName="Profile" %>
<%@ Register Src="~/Controls/Media352_MembershipProvider/UserLoginInformation.ascx" TagPrefix="User" TagName="UserLoginInformation" %>
<%@ Register TagPrefix="SEOComponent" TagName="CurrentPageSEO" Src="~/Controls/SEOComponent/SEO_Data_Entry.ascx" %>

<%@ Reference Control="~/Controls/BaseControls/ProfileControl.ascx" %>
<%@ Import Namespace="Classes.Media352_MembershipProvider" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<asp:CustomValidator ID="uxUserErrorCV" runat="server" Display="None" />
		<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
		<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
		<div class="blue padded optionsList">
			<ul class="inputList checkboxes horizontal">
				<li>
					<asp:CheckBox runat="server" ID="uxIsApproved" Checked="true" Text="Approve User" /></li>
			</ul>
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<User:UserLoginInformation runat="server" ID="uxUserLoginInformation" />
			<asp:PlaceHolder runat="server" ID="uxRolesPlaceHolder">
				<div class="formHalf"><span class="label">Roles </span>
					<asp:CheckBoxList runat="server" ID="uxRoles" CssClass="inputList checkboxes" RepeatLayout="UnorderedList" />
				</div>
				<asp:PlaceHolder runat="server" ID="uxCMSRolesPlaceHolder">
					<div class="formHalf"><span class="label">CMS Roles </span>
						<asp:CheckBoxList runat="server" ID="uxCMSRoles" CssClass="inputList checkboxes" RepeatLayout="UnorderedList" />
					</div>
				</asp:PlaceHolder>
				<div class="clear"></div>
			</asp:PlaceHolder>
		</div>
		<%
			if (Settings.UserManager == UserManagerType.Complex)
			{%>
		<div class="rightCol" style="float:right;">
			<div class="profileWrapper">
				<div class="profileInfoForm">
					<User:Profile ID="uxUserProfile" runat="server" UseProfileStandalone="false" />
				</div>
				<!--end profileInfoForm-->
			</div>
		</div>
		<%
			}%>
		<div class="clear"></div>
		<asp:PlaceHolder runat="server" ID="uxStaffPH">
			<div class="sectionTitle">
				<div class="bottom">
					<h2>Staff Information</h2>
				</div>
			</div>
			<Staff:Profile ID="uxStaffProfile" runat="server" />
			<div class="clear"></div>
			<SEOComponent:CurrentPageSEO ID="uxSEOData" runat="server" SitePageLinkSetupType="PageFormatter" PageLinkFormatter="~/staff-details.aspx?id={0}" />
		</asp:PlaceHolder>
		<!-- button container -->
		<div class="buttons">
			<%--The markup for the buttons is in the BaseEditPage--%>
			<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
		</div>
		<!--end btnContainer-->
	</asp:Panel>
</asp:Content>
