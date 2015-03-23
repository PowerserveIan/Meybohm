<%@ Page Language="c#" Inherits="ContentManager2.Admin.ContentManagerLog" MasterPageFile="~/admin/admin.master" CodeFile="content-manager-log.aspx.cs" Title="Admin - Page Log" %>

<%@ Import Namespace="Classes.ContentManager" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<style type="text/css">
		A.Button {
			color: Blue;
			text-decoration: none;
			font-weight: bold;
		}

		.backIcon {
			padding-right: 5px;
			padding-left: 0px;
			float: left;
			padding-bottom: 0px;
			padding-top: 10px;
		}

		.backLink {
			padding-right: 20px;
			padding-left: 0px;
			float: left;
			padding-bottom: 0px;
			padding-top: 15px;
		}

		.backLink A {
			font-size: 11px;
			color: #666;
			text-decoration: underline;
		}

		#logContainer {
			border-top: 1px solid #ccc;
			border-left: 1px solid #ccc;
			border-right: 1px solid #666;
			border-bottom: 1px solid #666;
			font-size: 12px;
		}

		.logRegionDiv {
			padding: 15px 0px 15px 10px;
		}

		.logImage {
			float: left;
			margin: 0px 7px 0px 0px;
		}

		.logHeader {
			background-color: #ABABAB;
			font-size: 11px;
			font-weight: bold;
			color: #666;
		}

		.logDefaultLine {
			background-color: #DEDEDE;
			font-size: 10px;
			font-weight: normal;
			color: #666;
		}

		.logDefaultLine a {
			color: #990007;
		}

		.logCreatedText {
			padding: 5px 50px 5px 10px;
			border-bottom: 1px solid #666;
			border-top: 1px solid #fff;
		}

		.logEditingUserText {
			padding: 5px 50px 5px 0px;
			border-bottom: 1px solid #666;
			border-top: 1px solid #fff;
		}

		.logCurrentText {
			padding: 5px 20px 5px 0px;
			border-bottom: 1px solid #666;
			border-top: 1px solid #fff;
		}

		.logDiffText {
			padding: 5px 5px 5px 0px;
			border-bottom: 1px solid #666;
			border-top: 1px solid #fff;
		}

		#mainContainer {
			padding: 0px;
		}

		h2.CM {
			font-size: 11px;
			font-weight: bold;
			color: #970001;
			padding: 0px 10px 0px 0px;
		}
	</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="PageSpecificJS" runat="server">
	<script type="text/javascript" src="logDiffSelection.js"></script>
	<script type="text/javascript">
		//<![CDATA[
		function hidediv(divid) { document.getElementById(divid).style.display = 'none'; }
		function showdiv(divid) { document.getElementById(divid).style.display = 'block'; }
		function toggle(divid) {
			if (document.getElementById(divid).style.display == 'none') {
				document.getElementById("img" + divid.replace('r', '')).src = "../img/treeview/icon-collapseLog.gif";
				showdiv(divid);
			}
			else {
				document.getElementById("img" + divid.replace('r', '')).src = "../img/treeview/icon-expandLog.gif";
				hidediv(divid);
			}
		}
		//]]>
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<div id="mainContainer">
		<h2 class="CM">File Log </h2>
		<div id="logContainer">
			<asp:Repeater ID="pageLog" runat="server">
				<HeaderTemplate>
					<table border="0" width="100%">
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
						<td>
							<div class="logRegionDiv">
								<a class="Button" href="javascript:toggle('r<%#((KeyValuePair<int, string>)Container.DataItem).Key%>');">
									<img border="0" src="../img/treeview/icon-collapseLog.gif" width="16" height="16" class="logImage" id="img<%#((KeyValuePair<int, string>)Container.DataItem).Key%>" name="img<%#((KeyValuePair<int, string>)Container.DataItem).Key%>" /><b>Region:</b>
									<%#((KeyValuePair<int, string>)Container.DataItem).Value%>
								</a>
							</div>
							<div id='r<%#((KeyValuePair<int, string>)Container.DataItem).Key%>'>
								<asp:Repeater ID="regionLog" runat="server" ItemType="Classes.ContentManager.CMPageRegion">
									<HeaderTemplate>
										<table style="width: 100%;">
											<tr class="logHeader">
												<td class="logCreatedText">
													Date Created
												</td>
												<td class="logEditingUserText">
													Editing User
												</td>
												<td class="logCurrentText">
													Current?
												</td>
												<td class="logDiffText">
													View/Restore
												</td>
												<td class="logDiffText" align="center">
													Diff
												</td>
											</tr>
									</HeaderTemplate>
									<ItemTemplate>
										<tr class="logDefaultLine" onmouseover="mouseover(<%#Item.CMPageRegionID%>);" id="<%#Item.CMPageRegionID%>">
											<td class="logCreatedText" onclick="rowclick(<%#Item.CMPageRegionID%>);">
												<%#Item.CreatedClientTime%>
											</td>
											<td class="logEditingUserText" onclick="rowclick(<%#Item.CMPageRegionID%>);">
												<%#GetUserName(Item)%>
											</td>
											<td class="logCurrentText" onclick="rowclick(<%#Item.CMPageRegionID%>);">
												<%#Item.CurrentVersion ? "Yes" : "No"%>
											</td>
											<td class="logDiffText" onclick="rowclick(<%#Item.CMPageRegionID%>);">
												<a id="rest<%#Item.CMPageRegionID%>" href='<%#((CMPage.GetByID(Item.CMPageID).Deleted == false) ? ("../../" + (CMPage.GetByID(Item.CMPageID).CMMicrositeID.HasValue && CMPage.GetByID(Item.CMPageID).CMMicrositeID.Value > 0 ? CMMicrosite.GetByID(CMPage.GetByID(Item.CMPageID).CMMicrositeID.Value).Name.Replace(" ", "-") + "/" : "") + CMPage.GetByID(Item.CMPageID).FileName + "?viewdate=" + (Item.Created.ToString("yyyyMMddHHmmssfff"))) : "#")%>'>
													View/Restore </a>
											</td>
											<td class="logDiffText" align="center">
												<a onclick="diffClick(<%#Item.CMPageRegionID%>);" id="diff<%#Item.CMPageRegionID%>" style="color: gray;">Diff</a>
											</td>
										</tr>
									</ItemTemplate>
									<FooterTemplate>
										</table>
									</FooterTemplate>
								</asp:Repeater>
							</div>
						</td>
					</tr>
				</ItemTemplate>
				<FooterTemplate>
					</table>
				</FooterTemplate>
			</asp:Repeater>
		</div>
		To use the Diff tool, click inside the row you want to compare, and then click Diff on the line you want to compare it to
		<div class="backIcon">
			<asp:ImageButton ID="Back" Src="../img/treeview/icon-back.gif" runat="server" Text="Back" CausesValidation="false" />
		</div>
		<div class="backLink">
			<asp:LinkButton runat="server" ID="lbBack" Text="back"></asp:LinkButton>
		</div>
	</div>
</asp:Content>
