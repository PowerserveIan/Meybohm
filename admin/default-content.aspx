<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="default-content.aspx.cs" Inherits="admin_DefaultContent" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
	<div class="title">
		<h1>Modify Site Content</h1>
	</div>
	<ul class="breadcrumbs clearfix">
		<li class="firstBreadcrumb">
			<a runat="server" href="~/admin/" title="Home">Dashboard</a></li>
		<li class="currentBreadcrumb">Modify Site Content</li>
	</ul>
	<div class="blue">
		<h2 class="inline">How to use this page<span class="tooltip"><span>Don't forget to open this page's code-behind and uncomment out all the components your site is using!</span></span></h2>
		<h5>Pressing the "Delete Existing Content" for a component will delete all database content editable in the managers. If you are going to use the "Insert Default Content" button, it is recommended that you first click the Delete button so that you don't get any
			conflicts in content. Some components may take up to a minute to finish inserting the default content, so please wait until the page has fully finished reloading. You can then check the component's management areas to see the default content that was inserted.</h5>
		<asp:Repeater runat="server" ID="uxComponentRepeater">
			<ItemTemplate>
				<div class="componentContainer">
					<h2>
						<%# Container.DataItem.ToString() %></h2>
					<asp:Button ID="uxDeleteExistingContent" runat="server" OnCommand="uxComponent_Command" CssClass="button delete" CommandName="Delete" CommandArgument="<%# Container.DataItem.ToString() %>" Text="Delete Existing Content" />
					<asp:Button ID="uxInsertDefaultContent" runat="server" OnCommand="uxComponent_Command" CssClass="button save" CommandName="Insert" CommandArgument="<%# Container.DataItem.ToString() %>" Text="Insert Default Content" />
				</div>
				<div class="clear"></div>
				<br />
			</ItemTemplate>
		</asp:Repeater>
		<div class="clear"></div>
	</div>
</asp:Content>
