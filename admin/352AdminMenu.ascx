<%@ Control Language="C#" AutoEventWireup="true" CodeFile="352AdminMenu.ascx.cs" Inherits="AdminMenu352Media" %>
<% bool open = Request.Path.ToLower().Contains("admin/default.aspx"); %>
<% if (Page.User.IsInRole("Admin"))
   { %><li<%= open ? " class='current'" : "" %>>
	<a runat="server" href="~/admin/">Dashboard</a></li><% } %>
<asp:PlaceHolder ID="ModuleMenus" runat="server" />