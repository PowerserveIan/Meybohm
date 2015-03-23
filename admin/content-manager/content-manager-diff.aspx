<%@ Page Language="c#" Inherits="ContentManagerDiff" CodeFile="content-manager-diff.aspx.cs" %>

<!DOCTYPE html>
<html lang="en">
<head>
	<title>Admin - Page Difference</title>
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
	<meta content="VisualStudio.HTML" name="ProgId">
	<meta content="Microsoft Visual Studio .NET 7.1" name="Originator">
	<style>
		TD.SDeleteSourceL {
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #ff8f8f;
			padding-bottom: 5px;
			border-left: #ccc 1px solid;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.DDeleteSourceL {
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #dedede;
			padding-bottom: 5px;
			border-left: #ccc 1px solid;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.SNoChangeL {
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #fff;
			padding-bottom: 5px;
			border-left: #ccc 1px solid;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.DNoChangeL {
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #fff;
			padding-bottom: 5px;
			border-left: #ccc 1px solid;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.SAddDestinationL {
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #dedede;
			padding-bottom: 5px;
			border-left: #ccc 1px solid;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.DAddDestinationL {
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #7ebf9e;
			padding-bottom: 5px;
			border-left: #ccc 1px solid;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.SReplaceL {
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #ff8f8f;
			padding-bottom: 5px;
			border-left: #ccc 1px solid;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.DReplaceL {
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #d1f0d1;
			padding-bottom: 5px;
			border-left: #ccc 1px solid;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.SDeleteSourceR {
			border-right: #ccc 1px solid;
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #ff8f8f;
			padding-bottom: 5px;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.DDeleteSourceR {
			border-right: #ccc 1px solid;
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #dedede;
			padding-bottom: 5px;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.SNoChangeR {
			border-right: #ccc 1px solid;
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #fff;
			padding-bottom: 5px;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.DNoChangeR {
			border-right: #ccc 1px solid;
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #fff;
			padding-bottom: 5px;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.SAddDestinationR {
			border-right: #ccc 1px solid;
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #dedede;
			padding-bottom: 5px;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.DAddDestinationR {
			border-right: #ccc 1px solid;
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #7ebf9e;
			padding-bottom: 5px;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.SReplaceR {
			border-right: #ccc 1px solid;
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #ff8f8f;
			padding-bottom: 5px;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		TD.DReplaceR {
			border-right: #ccc 1px solid;
			padding-right: 5px;
			border-top: #fff 1px solid;
			padding-left: 5px;
			font-size: 11px;
			background: #d1f0d1;
			padding-bottom: 5px;
			padding-top: 5px;
			border-bottom: #666 1px solid;
		}

		#propertiesDiv {
			border-right: #666 1px solid;
			padding-right: 25px;
			border-top: #ccc 1px solid;
			padding-left: 25px;
			font-size: 12px;
			padding-bottom: 25px;
			border-left: #ccc 1px solid;
			width: 305px;
			padding-top: 25px;
			border-bottom: #666 1px solid;
		}

		.propertiesText {
			padding-right: 0px;
			padding-left: 0px;
			float: left;
			padding-bottom: 5px;
			width: 100px;
			padding-top: 7px;
		}

		.propertiesForm {
			padding-right: 0px;
			padding-left: 0px;
			float: left;
			padding-bottom: 0px;
			padding-top: 5px;
		}

		.form {
			border-right: #666 1px solid;
			border-top: #666 1px solid;
			font-size: 10px;
			background: #ccc;
			border-left: #666 1px solid;
			color: #333333;
			border-bottom: #666 1px solid;
			font-family: Verdana, Arial, Helvetica, sans-serif;
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

		BODY {
			font-family: Verdana, Arial, Helvetica, sans-serif;
		}

		H2 {
			padding-right: 10px;
			padding-left: 0px;
			font-weight: bold;
			font-size: 11px;
			padding-bottom: 0px;
			color: #970001;
			padding-top: 0px;
		}

		#headingTable {
			font-weight: bold;
			font-size: 16px;
			border-bottom: #ccc 1px solid;
			height: 26px;
		}

		#headingTable A {
			font-weight: bold;
			font-size: 11px;
			color: #970001;
			text-decoration: none;
		}

		#headingTable A:hover {
			font-weight: bold;
			font-size: 11px;
			color: #970001;
			text-decoration: underline;
		}

		.diffContainer {
			border-right: #666 1px solid;
			border-top: #ccc 1px solid;
			margin-top: 20px;
			border-left: #ccc 1px solid;
			width: 280px;
			margin-right: 10px;
			padding-top: 15px;
		}

		.diffRowR {
			border-right: #666 1px solid;
		}

		.diffRowL {
			border-left: #ccc 1px solid;
		}

		.diffFooter {
			border-bottom: #ccc 1px solid;
		}

		.diffHeader {
			padding-right: 25px;
			padding-left: 8px;
			font-size: 11px;
			float: left;
			padding-bottom: 5px;
			padding-top: 5px;
		}

		.diffHeaderRegion {
			padding-right: 14px;
			padding-left: 8px;
			font-size: 11px;
			float: left;
			padding-bottom: 5px;
			padding-top: 5px;
		}

		.diffHeaderFile {
			padding-right: 34px;
			padding-left: 8px;
			font-size: 11px;
			float: left;
			padding-bottom: 5px;
			padding-top: 5px;
		}

		.diffDateFile {
			padding-right: 69px;
			padding-left: 8px;
			font-size: 11px;
			float: left;
			padding-bottom: 5px;
			padding-top: 5px;
		}

		.diffText {
			padding-right: 5px;
			padding-left: 8px;
			font-size: 11px;
			float: left;
			padding-bottom: 5px;
			padding-top: 5px;
		}

		BODY {
			font-family: Verdana, Arial, Helvetica, sans-serif;
		}

		H2 {
			padding-right: 10px;
			padding-left: 0px;
			font-weight: bold;
			font-size: 11px;
			padding-bottom: 0px;
			color: #970001;
			padding-top: 0px;
		}

		H3 {
			padding-right: 7px;
			display: inline;
			padding-left: 0px;
			font-weight: bold;
			font-size: 12px;
			padding-bottom: 0px;
			text-transform: uppercase;
			color: #970001;
			padding-top: 2px;
		}

		H4 {
			padding-right: 0px;
			display: inline;
			padding-left: 0px;
			font-weight: bold;
			font-size: 12px;
			padding-bottom: 0px;
			text-transform: uppercase;
			color: #000;
			padding-top: 0px;
		}

		H5 {
			padding-right: 0px;
			display: inline;
			padding-left: 0px;
			font-weight: bold;
			font-size: 11px;
			padding-bottom: 10px;
			color: #970001;
			padding-top: 0px;
		}

		B {
			padding-right: 0px;
			display: inline;
			padding-left: 0px;
			font-weight: bold;
			font-size: 11px;
			padding-bottom: 10px;
			color: #970001;
			padding-top: 0px;
		}

		A.Admin {
			color: #333333;
			text-decoration: none;
			font-size: large;
			font-family: "Arial Narrow", Arial, Courier, monospace;
		}
	</style>
</head>
<body>
	<form runat="server">
	<table width="100%" id="headingTable">
		<tr>
			<td>
				<div style="float: left; padding-top: 12px">Content Manager </div>
			</td>
		</tr>
	</table>
	<div id="mainContainer">
		<br />
		<h2>Difference View</h2>
		<br />
		<img src="../img/treeview/colorKey.gif">
		<br />
		<br />
		<br />
		<asp:Repeater runat="server" ID="rDiffReport" DataSource="<%#ds%>" ItemType="ContentManagerDiff.DifferenceRow">
			<HeaderTemplate>
				<table width="100%" cellspacing="0">
					<tr>
						<td colspan="2" width="50%" style="font-size: 11px;" class="diffContainer">
							<h4>Source</h4>
							<br />
							<br />
							<div style="width: 135px; float: left;"><b>Page:</b></div>
							<%#cmPLeft.Title%>
							<br />
							<br />
							<div style="width: 135px; float: left;"><b>Filename:</b></div>
							<%#cmPLeft.FileName%>
							<br />
							<br />
							<div style="width: 135px; float: left;"><b>Region Name:</b></div>
							<%#cmRLeft.Name%>
							<br />
							<br />
							<div style="width: 135px; float: left;"><b>Date:</b></div>
							<%#cmPRLeft.CreatedClientTime%>
							<br />
							<br />
						</td>
						<td colspan="2" width="50%" style="font-size: 11px;" class="diffContainer">
							<h4>Destination</h4>
							<br />
							<br />
							<div style="width: 135px; float: left;"><b>Page:</b></div>
							<%#cmPRight.Title%>
							<br />
							<br />
							<div style="width: 135px; float: left;"><b>Filename:</b></div>
							<%#cmPRight.FileName%>
							<br />
							<br />
							<div style="width: 135px; float: left;"><b>Region Name:</b></div>
							<%#cmRRight.Name%>
							<br />
							<br />
							<div style="width: 135px; float: left;"><b>Date:</b></div>
							<%#cmPRRight.CreatedClientTime%>
							<br />
							<br />
						</td>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td valign="top" class="S<%#Item.Action%>L" width="1%">
						<%#ds.IndexOf(Item).ToString("00000")%>
					</td>
					<td valign="top" class="S<%#Item.Action%>R">
						<%#:Item.SourceText%>
						&nbsp;
					</td>
					<td valign="top" class="D<%#Item.Action%>L" width="1%">
						<%#ds.IndexOf(Item).ToString("00000")%>:
					</td>
					<td valign="top" class="D<%#Item.Action%>R">
						<%#:Item.DestinationText%>
						&nbsp;
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:Repeater>
	</div>
	<br />
	<a class="Admin" href="#" onclick="javascript:window.close();">
		<img alt="" border="0" src="../img/treeview/icon-closeWindow.gif">Close Window</a>
	</form>
</body>
</html>
