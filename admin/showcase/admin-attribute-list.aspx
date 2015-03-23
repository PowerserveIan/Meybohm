<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-attribute-list.aspx.cs" Inherits="Admin_AttributeList" Title="Admin - MLS Attribute List" %>

<%@ Import Namespace="Classes.Showcase" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<div class="title">
		<h1><%= Showcases.GetByID(ShowcaseHelpers.GetCurrentShowcaseID().Value).Title %> MLS Attribute List</h1>
	</div>
	<ul class="breadcrumbs clearfix">
		<li class="firstBreadcrumb">
			<a title="Home" href="../">Dashboard</a></li>
		<li class="currentBreadcrumb"><%= Showcases.GetByID(ShowcaseHelpers.GetCurrentShowcaseID().Value).Title %> MLS Attribute List</li>
	</ul>
	<div class="blue">
		<h4>All of the attributes listed below are potential attributes and filters that you may import into your listing.  The items you checkoff will appear in the
			<a href="admin-attribute.aspx">Attribute Manager</a>
			where you can decide whether you want to make a filter out of them or just display the data in the popup for the home.</h4>
		<div class="clear"></div>
	</div>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="formWrapper" style="width: 100%;">
			<a href="#" id="checkAll">Check All</a>
			/ 
		<a href="#" id="uncheckAll">Uncheck All</a>
			<asp:CheckBoxList runat="server" ID="uxAttributes" CssClass="inputList checkboxes" RepeatColumns="5"></asp:CheckBoxList>
		</div>
		<div class="clear"></div>
		<!-- button container -->
		<div class="buttons fixedBottom">
			<asp:Button runat="server" ID="uxSave" Text="Save" CssClass="button save" />
		</div>
	</asp:Panel>
	<asp:PlaceHolder runat="server" ID="uxAfterSavePH" Visible="false">
		<h3 class="success">The Attribute list has been successfully <u>updated</u>.</h3>
		<div class="buttons" id="afterSaveButtons">
			<h4>What would you like to do next?</h4>
			<a href="admin-attribute.aspx" class="button view"><span>View Attributes</span></a><a id="editToggle" href="#" class="button edit"><span>Edit Attribute List</span></a>
		</div>
		<script type="text/javascript">
			document.getElementById('ctl00_ContentWindow_uxPanel').style.display = 'none';
			$(document).ready(function () {
				$('#afterSaveButtons a.edit').click(function () {
					$('#sectionTitleContainer,#ctl00_ContentWindow_uxPanel, h3.success').slideToggle();
					$('#afterSaveButtons').remove();
					return false;
				});
			});
		</script>
	</asp:PlaceHolder>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		$(document).ready(function () {
			$("#checkAll").click(function () {
				if (confirm('Are you sure you want to do this?  This will cause more data to be imported and can slow down the display of results for your frontend users.'))
					$("input[id*=uxAttributes]").attr("checked", "checked");
				return false;
			});
			$("#uncheckAll").click(function () {
				$("input[id*=uxAttributes]").removeAttr("checked");
				return false;
			});
		});
	</script>
</asp:Content>
