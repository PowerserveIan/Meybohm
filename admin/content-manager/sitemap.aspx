<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="sitemap.aspx.cs" Inherits="Admin_ContentManager_sitemap" Title="Admin - Sitemap" %>

<%@ Import Namespace="Classes.ContentManager" %>
<%@ Register TagName="Toggle" TagPrefix="Language" Src="~/Controls/BaseControls/LanguageToggleAdmin.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<div class="title">
		<h1>Manage Site Map</h1>
	</div>
	<ul class="breadcrumbs clearfix">
		<li class="firstBreadcrumb">
			<a runat="server" href="~/admin" title="Home">Dashboard</a></li>
		<li class="currentBreadcrumb">Manage Site Map</li>
	</ul>
	<asp:Label runat="server" ID="uxMicrositeInactive" Text="The microsite you are managing has been disabled.  Please contact the admin to have it reenabled." Visible="false" />
	<asp:PlaceHolder runat="server" ID="uxUnapprovedPlaceHolder" Visible="false">
		<div class="approvalMenu">
			<div class="notification">
				<asp:Label runat="server" ID="uxMessage" />
				<br />
				<div class="approvalDiv">
					<a runat="server" id="uxShowApprovalDetails" class="approvalLink" href="#" onclick="$('div.editedByDiv').show();return false;">Show Approval Details</a>
					<asp:LinkButton runat="server" ID="uxLiveContent" Text="View Live Content" CausesValidation="false"></asp:LinkButton>
					<asp:LinkButton runat="server" ID="uxUnapprovedContent" Text="View Unapproved Content" CausesValidation="false"></asp:LinkButton>
					<div class="editedByDiv" style="display: none;">
						<a class="close" href="#" onclick="$('div.editedByDiv').hide();return false;">
							<img runat="server" src="~/img/btn_close.gif" alt="close" /></a>
						Edited By:
						<asp:Literal runat="server" ID="uxAllEditors"></asp:Literal>
					</div>
					<div class="approvalRedTxt">
						<asp:Label runat="server" ID="uxAdminMessage" Text="You must approve all changes (even those you make) before the content below will show up on the front end of the site." ForeColor="Red" />
					</div>
				</div>
			</div>
		</div>
		<!--end approvalMenu-->
	</asp:PlaceHolder>
	<asp:PlaceHolder runat="server" ID="uxHideThisIfMSAdmin">
		<div class="paddingLeft paddingBottom paddingTop" runat="server" id="uxMicrositeLanguagePH">
			<asp:PlaceHolder runat="server" ID="uxMicrositePlaceHolder" Visible="false">
				<h1>
					<asp:Literal runat="server" ID="uxMicroSiteName" /></h1>
				<asp:DropDownList CssClass="micrositeList" runat="server" ID="uxMicrositeList" AutoPostBack="true" />
			</asp:PlaceHolder>
			<Language:Toggle ID="uxLanguageToggle" runat="server" />
			<asp:DropDownList runat="server" ID="uxNewHomes" AutoPostBack="true">
				<asp:ListItem Text="New Homes" Value="true"></asp:ListItem>
				<asp:ListItem Text="Existing Homes" Value="false"></asp:ListItem>
			</asp:DropDownList>
		</div>
		<div class="sitemap">
			<table style="width: 500px;">
				<% if (Settings.EnableApprovals)
	   {%>
				<tr>
					<td colspan="2">
						<span class="approval">*Items in <span style="color: Red">red</span> are awaiting approval.</span>
					</td>
				</tr>
				<% }%>
				<tr>
					<td valign="top" runat="server" id="uxPagesPH">
						<div id="uxPages">
							<asp:Literal runat="server" ID="uxPagesList"></asp:Literal>
						</div>
					</td>
					<td valign="top" runat="server" id="uxSitemapPH">
						<div id="uxSiteMap">
							<asp:Literal runat="server" ID="uxSitemapList"></asp:Literal>
						</div>
					</td>
				</tr>
			</table>
		</div>
		<div class="rightCol">
			<asp:Panel runat="server" ID="uxModifySiteMapPlaceholder" DefaultButton="uxAddLink">
				<fieldset>
					<legend>Add Existing Page or External Link</legend>
					<p>
						You may add non-CMS pages to your sitemap by using the form below. Enter the title you want to display on the sitemap and the link to the page in the fields below.
					</p>
					<asp:ValidationSummary runat="server" ID="uxValidationSummary" DisplayMode="BulletList" CssClass="validation" />
					<div class="formWhole">
						<label for="<%= uxPageTitle.ClientID %>">Page Title</label>
						<asp:TextBox CssClass="text" runat="server" ID="uxPageTitle" MaxLength="255" />
						<asp:RequiredFieldValidator runat="server" ID="uxPageTitleReqVal" ControlToValidate="uxPageTitle" ErrorMessage="Page Title is required." />
						<asp:CustomValidator runat="server" ID="uxPageTitleCustomVal" ControlToValidate="uxPageTitle" OnServerValidate="uxPageTitleCustomVal_ServerValidate" ErrorMessage="Page Title already exists, please enter a new title." />
					</div>
					<div class="formWhole">
						<label for="<%= uxLinkToPage.ClientID %>">Link to Page</label>
						<asp:TextBox CssClass="text" runat="server" ID="uxLinkToPage" MaxLength="255" />
						<asp:RequiredFieldValidator runat="server" ID="uxLinkToPageReqVal" ControlToValidate="uxLinkToPage" ErrorMessage="Link to Page is required." />
						<asp:CustomValidator runat="server" ID="uxLinkToPageExistsCustomVal" ControlToValidate="uxLinkToPage" OnServerValidate="uxLinkToPageExistsCustomVal_ServerValidate" ErrorMessage="Page Link already exists." />
						<asp:CustomValidator runat="server" ID="uxLinkToPageCustomVal2" ControlToValidate="uxLinkToPage" OnServerValidate="uxLinkToPageCustomVal2_ServerValidate" ErrorMessage="Page link must be an existing page on your site or a valid web address." />
					</div>
					<asp:Button runat="server" ID="uxAddLink" Text="Add Page" CssClass="button add" />
				</fieldset>
			</asp:Panel>
			<div class="helpBubble">
				<div class="top">
					<div class="bottom">
						<h5>Note:</h5>
						<p>
							The tree on the left represents the pages within the content management system. The tree on the right represents the site map. To get started drag items from the left tree to the right tree. You may drag the folder to copy all pages within a node. Items within
							the sitemap on the right may be moved again by dragging them to a new location. Note that only
							<asp:Literal runat="server" ID="uxDepthLimit"></asp:Literal>
							levels may be created. To sort and delete items in the tree, right click the item of interest.</p>
						<p>
							To show items that you previously hid in the site map (the tree on the right), right click on "Site Map Root" and click "Show All Pages."</p>
						<asp:Literal runat="server" ID="uxOneToOne" Text="<p>A page can only have one corresponding item on the site map tree.</p>"></asp:Literal>
					</div>
				</div>
			</div>
		</div>
		<div class="clear"></div>
	</asp:PlaceHolder>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery.jstree.js"></asp:Literal>
	<script type="text/javascript">
		$(document).ready(function () {
			$("#uxSiteMap,#uxPages").bind("loaded.jstree", function () {
				$(".jstree-closed").each(function () {
					$(this).parents("div").jstree("open_node", this, false, true);
				});
			});
			$("#uxPages").bind("remove.jstree", function (e, data) {
				data.rslt.obj.each(function () {
					$.ajax({
						async: false,
						type: 'POST',
						data: {
							"languageID": <%= LanguageID %>,
							"operation": "remove_page",
							"id": this.id.replace("p", "")
						},
						success: function (r) {
							if (r.status == 'reload') {
								if (<%= String.IsNullOrEmpty(Request.QueryString["r"]).ToString().ToLower() %>)
									window.location = window.location + '?r=' + new Date().getTime();
								return;
							}
							if (!r.status) {
								data.inst.refresh();
							}
						}
					});
				});
			});
			$("#uxSiteMap").delegate("a", "dblclick", function (e) {
				$("#uxSiteMap").jstree("rename");
			});
			$("#uxSiteMap").bind("rename.jstree", function (e, data) {
				$.post(
					"<%= Request.Url %>",
					{
						"languageID": <%= LanguageID %>,
						"operation": "rename_page",
						"id": data.rslt.obj.attr("id").replace("s", ""),
						"title": data.rslt.new_name
					},
					function (r) {
						if (r.status == 'reload') {
								if (<%= String.IsNullOrEmpty(Request.QueryString["r"]).ToString().ToLower() %>)
									window.location = window.location + '?r=' + new Date().getTime();
								return;
							}
						if (!r.status) {
							$.jstree.rollback(data.rlbk);
						}
					}
				);
			});
			$("#uxSiteMap").bind("remove.jstree", function (e, data) {
				data.rslt.obj.each(function () {
					$.ajax({
						async: false,
						type: 'POST',
						data: {
							"languageID": <%= LanguageID %>,
							"operation": "remove_smitem",
							"id": this.id.replace("s", "")
						},
						success: function (r) {
							if (r.status == 'reload') {
								if (<%= String.IsNullOrEmpty(Request.QueryString["r"]).ToString().ToLower() %>)
									window.location = window.location + '?r=' + new Date().getTime();
								return;
							}
							if (!r.status) {
								data.inst.refresh();
							}
						}
					});
				});
			});
			$("#uxPages, #uxSiteMap").bind("move_node.jstree", function (e, data) {
				data.rslt.o.each(function (i) {
					$.ajax({
						async: false,
						type: 'POST',
						data: {
							"languageID": <%= LanguageID %>,
							"operation": "move_node",
							"sourceID": $(this).attr("id"),
							"destinationID": data.rslt.np.attr("id"),
							"position": data.rslt.cp + i,
							"newHome": <%= uxNewHomes.SelectedValue %>
						},
						success: function (r) {
							if (r.status == 'reload') {
								if (<%= String.IsNullOrEmpty(Request.QueryString["r"]).ToString().ToLower() %>)
									window.location = window.location + '?r=' + new Date().getTime();
								return;
							}
							if (!r.status || (r.status != 1 && r.status.toString().indexOf("s") != 0)) {
								$.jstree.rollback(data.rlbk);
								if (r.status.toString().indexOf("s") != 0)
									alert(r.status);
							}
							else if (r.status != 1)
								data.rslt.oc.attr("id", r.status);
						}
					});
				});
			});

			var baseConfig = {
				"crrm": {
					"move": {
						"always_copy": "multitree",
						"check_move": function (m) {
							return $(m.np[0]).parents("div").attr("id") == "uxSiteMap";
						}
					}
				},
				"themes": {
					"url": '<%= ResolveClientUrl("~/admin/css/") %>jquery.tree.css'
				},
				"plugins": ["themes", "html_data", "ui", "dnd", "types", "contextmenu", "crrm"],
				"types": {
					"types": {
						"folder": {
							"icon": {
								"image": '<%= ResolveClientUrl("~/admin/img/TreeIcons/Folder.gif") %>'
							}
						},
						"page": {
							"icon": {
								"image": '<%= ResolveClientUrl("~/admin/img/TreeIcons/aspx.gif") %>'
							}
						},
						"deleted": {
							"icon": {
								"image": '<%= ResolveClientUrl("~/admin/img/TreeIcons/icon-rejected.gif") %>'
							}
						},
						"changed": {
							"icon": {
								"image": '<%= ResolveClientUrl("~/admin/img/TreeIcons/icon-approval.gif") %>'
							}
						}
					}
				},
				"contextmenu": {
					items: function (obj) {
						var tree = $(obj).parents("div").attr("id");
						var type_of_node = obj.attr("rel");
						var shouldDisable = $(obj).hasClass("contextDisabled");
						var menu = {};
						if (shouldDisable)
							return {};
						if (type_of_node == 'folder')
						{
							if (tree =="uxPages")
							{
								return {};
							}
							else
							{
								menu = {
									"ShowHideAll": {
										"label": ($(obj).find("a").hasClass("showall") ? "Hide Non-Menu Pages" : "Show All Pages"),
										"icon": ($(obj).find("a").hasClass("showall") ? '<%= ResolveClientUrl("~/admin/img/TreeIcons/hidden.gif") %>' : '<%= ResolveClientUrl("~/admin/img/TreeIcons/display.gif") %>'),
										"action": function (obj) {
											if ($(obj).find("a").hasClass("showall"))
											{
												location.href = "sitemap.aspx";	
											}
											else
											{
												location.href = "sitemap.aspx?showall=true";
											}
										}
									}
								}

								return menu;
							}
						}
						if (tree == "uxPages") {
							if ($(obj).find("a").hasClass("deleted"))
								menu = {
									"restore": {
										"label": "Restore",
										"icon": '<%= ResolveClientUrl("~/admin/img/TreeIcons/restore.gif") %>',
										"action": function (obj) { 
											$(obj).find("a").removeClass("deleted");
											$(obj).attr("rel", "page");
											$.ajax({
												async: false,
												type: 'POST',
												data: {
													"languageID": <%= LanguageID %>,
													"operation": "restore_page",
													"id": obj.attr("id").replace("p", "")
												},
												success: function (r) {
													if (!r.status) {
														$.jstree.rollback(data.rlbk);
													}
												}
											});
										 }
									}
								}
							else
								menu = {
									"remove": {
										"label": "Delete",
										"icon": '<%= ResolveClientUrl("~/admin/img/TreeIcons/delete.gif") %>',
										"action": function (obj) { this.remove(obj); }
									}
								}
						} else {
							menu = {
								"moveUp": {
									"label": "Move Up",
									"_disabled": $(obj).prev().length == 0,
									"icon": '<%= ResolveClientUrl("~/admin/img/TreeIcons/up.gif") %>',
									"action": function (obj) {
										this.move_node(obj, obj.prev(), "before", false, false, true);
									}
								},
								"moveDown": {
									"label": "Move Down",
									"_disabled": $(obj).next().length == 0,
									"icon": '<%= ResolveClientUrl("~/admin/img/TreeIcons/down.gif") %>',
									"action": function (obj) {
										this.move_node(obj, obj.next(), "after", false, false, true);
									}
								},
								"remove": {
									"label": ($(obj).find("a").hasClass("deleted") ? "Restore" : "Delete"),
									"icon": ($(obj).find("a").hasClass("deleted") ? '<%= ResolveClientUrl("~/admin/img/TreeIcons/restore.gif") %>' : '<%= ResolveClientUrl("~/admin/img/TreeIcons/delete.gif") %>'),
									"action": function (obj) { 
									if ($(obj).find("a").hasClass("deleted"))
									{
										$(obj).find("a").removeClass("deleted");
											$(obj).attr("rel", "page");
											$.ajax({
												async: false,
												type: 'POST',
												data: {
													"languageID": <%= LanguageID %>,
													"operation": "restore_smitem",
													"id": obj.attr("id").replace("s", "")
												},
												success: function (r) {
													if (!r.status) {
														$.jstree.rollback(data.rlbk);
													}
												}
											});
									}
									else
										this.remove(obj); }
								},
								"hide": {
									"label": ($(obj).find("a").hasClass("hidden") ? "Display In Menu" : "Hide From Menu"),
									"icon": ($(obj).find("a").hasClass("hidden") ? '<%= ResolveClientUrl("~/admin/img/TreeIcons/display.gif") %>' : '<%= ResolveClientUrl("~/admin/img/TreeIcons/hidden.gif") %>'),
									"action": function (obj) {
										if ($(obj).find("a").hasClass("hidden"))
										{
											$.ajax({
												async: false,
												type: 'POST',
												data: {
													"languageID": <%= LanguageID %>,
													"operation": "display_smitem",
													"id": obj.attr("id").replace("s", "")
												},
												success: function (r) {
													
														$(obj).find("a").removeClass("hidden");
												}
											});
											
										}
										else
										{
											$.ajax({
												async: false,
												type: 'POST',
												data: {
													"languageID": <%= LanguageID %>,
													"operation": "hide_smitem",
													"id": obj.attr("id").replace("s", "")
												},
												success: function (r) {
														$(obj).find("a").addClass("hidden");
												}
											});
										}
								}
									
								},
							}
						}
						return menu;
					}
				}
			};
			$("#uxPages").jstree(baseConfig);
			$("#uxSiteMap").jstree(baseConfig);
		});
	</script>
</asp:Content>
