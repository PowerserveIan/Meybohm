<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admin-mailout-metrics.aspx.cs" Inherits="Admin_Newsletters_MailoutMetrics" Title="Admin - Mailout Metrics" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
	<title>Detailed Mailout Statistics</title>
	<link runat="server" id="uxCSSFiles" rel="stylesheet" media="screen, projection" type="text/css" href="~/admin/css/reset.css,~/admin/css/structure.css,~/admin/css/typography.css,~/admin/css/forms.css,~/admin/css/features.css" />
	<style type="text/css">
		body {
			margin: 0;
			background-image: none;
		}
		.actionHeader {
			background-color: #DCEEB9;
			border: 1px solid #9C3;
			padding: 5px;
			font-size: 84.6%;
		}
		.actionHeader label {
			float: none;
			display: inline;
			font-size: 100%;
			font-weight: normal;
		}
		.actionTable {
			width: 100%;
			margin: 0;
			border-top: none;
			font-size: 84.6%;
		}
		.actionFooter {
			background-color: #DCEEB9;
			border: 1px solid #9C3;
			border-top: none;
			padding: 5px;
		}
		div.contentLoadingBackground {
			width: 100%;
			height: 100%;
			position: fixed;
			top: 0;
			left: 0;
			background-color: #000;
			filter: alpha(opacity=40);
			opacity: .40;
		}
		#contentLoading {
			background-color: #FFFFFF;
			border: 1px solid #999999;
			color: #000000;
			left: 50%;
			margin-left: -150px;
			margin-top: -25px;
			padding: 20px 10px;
			position: fixed;
			text-align: center;
			top: 50%;
			width: 300px;
		}
	</style>
	<script type="text/javascript">
		var jqueryPath = "//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js";
		document.write(unescape("%3Cscript src='" + jqueryPath + "' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<script type="text/javascript">		!window.jQuery && document.write('<script type="text/javascript" src="../../tft-js/core/jquery.min.js"><\/script>')</script>
</head>
<body>
	<form id="form1" runat="server">
	<div class="contentLoadingWrapper" style="display: none;">
		<div class="contentLoadingBackground"></div>
		<div id="contentLoading">
			<img runat="server" src="~/img/loading.gif" alt="Loading" style="padding-right: 20px;" />
			Loading, please wait... </div>
	</div>
	<h1>Mailout Statistics</h1>
	<!-- "Sent" action table -->
	<div id="uxSentContainer" style="width: 98%; margin-top: 15px;">
		<div id="uxSentHeader" class="actionHeader"><span style="float: left;"><strong>Sent (<span id="uxSentCount">0</span>)</strong>
			<a href="#"><em>Click here to show+hide</em></a><label>Loading, please wait...</label></span>
			<asp:LinkButton runat="server" ID="uxSentExport" Text="Export" Style="float: right; display: none;" OnCommand="Export_Command" CommandArgument="Sent"></asp:LinkButton><br />
		</div>
		<div id="uxSentTableContainer">
			<table id="uxSentTable" class="listingTable actionTable">
				<thead>
					<tr>
						<th>
							Recipient
						</th>
						<th nowrap="nowrap">
							Date
						</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
			<div id="uxSentFooter" class="actionFooter"><span>
				<a style="cursor: pointer" id="uxSentLoadMore">load more</a><%-- | <a style="cursor:pointer" id="uxSentLoadAll">
					load all</a>--%></span> </div>
		</div>
	</div>
	<!-- "Not Sent" table -->
	<div id="uxNotSentContainer" style="width: 98%; margin-top: 15px;">
		<div id="uxNotSentHeader" class="actionHeader"><span style="float: left;"><strong>Not Sent (<span id="uxNotSentCount">0</span>)</strong>
			<a href="#"><em>Click here to show+hide</em></a><label>Loading, please wait...</label></span>
			<asp:LinkButton runat="server" ID="uxNotSentExport" Text="Export" Style="float: right; display: none;" OnCommand="Export_Command" CommandArgument="NotSent"></asp:LinkButton><br />
		</div>
		<div id="uxNotSentTableContainer">
			<table id="uxNotSentTable" class="listingTable actionTable">
				<thead>
					<tr>
						<th>
							Recipient
						</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
			<div id="uxNotSentFooter" class="actionFooter"><span>
				<a style="cursor: pointer" id="uxNotSentLoadMore">load more</a>
				<%--| <a style="cursor:pointer" id="uxNotSentLoadAll">
					load all</a>--%></span> </div>
		</div>
	</div>
	<!-- "Open" action table -->
	<div id="uxOpenContainer" style="width: 98%; margin-top: 15px;">
		<div id="uxOpenHeader" class="actionHeader"><span style="float: left;"><strong>Opened (<span id="uxOpenCount">0</span>)</strong>
			<a href="#"><em>Click here to show+hide</em></a><label>Loading, please wait...</label></span>
			<asp:LinkButton runat="server" ID="uxOpenedExport" Text="Export" Style="float: right; display: none;" OnCommand="Export_Command" CommandArgument="Opened"></asp:LinkButton><br />
		</div>
		<div id="uxOpenTableContainer">
			<table id="uxOpenTable" class="listingTable actionTable">
				<thead>
					<tr>
						<th>
							Recipient
						</th>
						<th style="width: 130px">
							Date
						</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
			<div id="uxOpenFooter" class="actionFooter"><span>
				<a style="cursor: pointer" id="uxOpenLoadMore">load more</a><%-- | <a style="cursor:pointer" id="uxOpenLoadAll">
					load all</a>--%></span> </div>
		</div>
	</div>
	<!-- "Clicks - By Subscriber" action table -->
	<div id="uxClickContainer" style="width: 98%; margin-top: 15px;">
		<div id="uxClickHeader" class="actionHeader"><span style="float: left;"><strong>Clicks - By User (<span id="uxClickCount">0</span>)</strong>
			<a href="#"><em>Click here to show+hide</em></a><label>Loading, please wait...</label></span>
			<asp:LinkButton runat="server" ID="uxClicksUserExport" Text="Export" Style="float: right; display: none;" OnCommand="Export_Command" CommandArgument="ClicksUser"></asp:LinkButton><br />
		</div>
		<div id="uxClickTableContainer">
			<table id="uxClickTable" class="listingTable actionTable">
				<thead>
					<tr>
						<th>
							Recipient
						</th>
						<th>
							URL
						</th>
						<th style="width: 130px">
							Date
						</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
			<div id="uxClickFooter" class="actionFooter"><span>
				<a style="cursor: pointer" id="uxClickLoadMore">load more</a><%-- | <a style="cursor:pointer" id="uxClickLoadAll">
					load all</a>--%></span> </div>
		</div>
	</div>
	<!-- "Clicks - By Url" action table -->
	<div id="uxClick2Container" style="width: 98%; margin-top: 15px;">
		<div id="uxClick2Header" class="actionHeader"><span style="float: left;"><strong>Clicks - By URL (<span id="uxClick2Count">0</span>)</strong>
			<a href="#"><em>Click here to show+hide</em></a><label>Loading, please wait...</label></span>
			<asp:LinkButton runat="server" ID="uxClicksURLExport" Text="Export" Style="float: right; display: none;" OnCommand="Export_Command" CommandArgument="ClicksURL"></asp:LinkButton><br />
		</div>
		<div id="uxClick2TableContainer">
			<table id="uxClick2Table" class="listingTable actionTable">
				<thead>
					<tr>
						<th>
							URL
						</th>
						<th style="width: 130px">
							Clicks
						</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
			<div id="uxClick2Footer" class="actionFooter"><span>
				<a style="cursor: pointer" id="uxClick2LoadMore">load more</a><%-- | <a style="cursor:pointer" id="uxClick2LoadAll">
					load all</a>--%></span> </div>
		</div>
	</div>
	<!-- "Forward" action table -->
	<div id="uxForwardContainer" style="width: 98%; margin-top: 15px;">
		<div id="uxForwardHeader" class="actionHeader"><span style="float: left;"><strong>Forwards (<span id="uxForwardCount">0</span>)</strong>
			<a href="#"><em>Click here to show+hide</em></a><label>Loading, please wait...</label></span>
			<asp:LinkButton runat="server" ID="uxForwardsExport" Text="Export" Style="float: right; display: none;" OnCommand="Export_Command" CommandArgument="Forward"></asp:LinkButton><br />
		</div>
		<div id="uxForwardTableContainer">
			<table id="uxForwardTable" class="listingTable actionTable">
				<thead>
					<tr>
						<th>
							Recipient
						</th>
						<th>
							Forwarded To
						</th>
						<th>
							Date
						</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
			<div id="uxForwardFooter" class="actionFooter"><span>
				<a style="cursor: pointer" id="uxForwardLoadMore">load more</a><%-- | <a style="cursor:pointer" id="uxForwardLoadAll">
					load all</a>--%></span> </div>
		</div>
	</div>
	<!-- "Unsubscribe" action table -->
	<div id="uxUnsubContainer" style="width: 98%; margin-top: 15px;">
		<div id="uxUnsubHeader" class="actionHeader"><span style="float: left;"><strong>Unsubscriptions (<span id="uxUnsubCount">0</span>)</strong>
			<a href="#"><em>Click here to show+hide</em></a><label>Loading, please wait...</label></span>
			<asp:LinkButton runat="server" ID="uxUnsubscriptionsExport" Text="Export" Style="float: right; display: none;" OnCommand="Export_Command" CommandArgument="Unsubscribe"></asp:LinkButton><br />
		</div>
		<div id="uxUnsubTableContainer">
			<table id="uxUnsubTable" class="listingTable actionTable">
				<thead>
					<tr>
						<th>
							Recipient
						</th>
						<th>
							Date
						</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
			<div id="uxUnsubFooter" class="actionFooter"><span>
				<a style="cursor: pointer" id="uxUnsubLoadMore">load more</a><%-- | <a style="cursor:pointer" id="uxUnsubLoadAll">
					load all</a>--%></span> </div>
		</div>
	</div>
	</form>
	<script type="text/javascript">
		//<![CDATA[
		var mailoutId = <%=MailoutID.ToString()%>;
		
		// Function to load paged actions - used by action-specfic functions below
		// NOTE: All paging and indexing in the metrics system is 1-based
		function loadActions(startIndex, loadType, loadMethod, loadFunction, toggleLink, tableBody, tableFooter, actionCountDiv, loadMoreLink, loadAllLink) {
			loadMoreLink.unbind('click');
			loadAllLink.unbind('click');
			
			$.ajax({		// request page of data asynchronously
				type:		"POST",
				url:		loadMethod,
				data:		'{startIndex:'+startIndex+', mailoutId: '+mailoutId+', loadType:"'+loadType+'"}',
				contentType:"application/json; charset=utf-8",
				dataType:	"json",
				success:	function(msg) {
								toggleLink.siblings("label").hide();
								if( msg.d.TotalCount > 0) // update action count and make hide/show link visible
								{
									actionCountDiv.text(msg.d.TotalCount.toString());
									toggleLink.show();
									toggleLink.siblings("a[id$=uxSentExport]").show();
								}
								
								if( msg.d.PageSize > 0)
									tableBody.append(msg.d.Markup);  // add the new rows to the table display
								else
									tableBody.html(msg.d.Markup);  // replace rows in the table display with complete data
								
								var nextIndex = msg.d.LastIndex + 1;
								if( nextIndex <= msg.d.TotalCount )  // rebind the click handlers to load the next page
								{
									loadMoreLink.click(function() { $(".contentLoadingWrapper").show();loadFunction(nextIndex, "page"); });
									loadAllLink.click(function() { $(".contentLoadingWrapper").show();loadFunction(nextIndex, "all"); });
								}
								else
									tableFooter.hide();
								$(".contentLoadingWrapper").hide();
							},
				error:	function(xml,status,error){
							$(".contentLoadingWrapper").hide();
						}
				});
		}
		
		// Function to load paged "Sent" actions
		function loadSentActions(startIndex, loadType) {
			loadActions( 
				startIndex, 
				loadType, 
				"admin-mailout-metrics.aspx/GetRowPageForSentActions",
				loadSentActions,
				$('#uxSentHeader a'), $('#uxSentTable > tbody'), $('#uxSentFooter'), $('#uxSentCount'), $('#uxSentLoadMore'), $('#uxSentLoadAll'));
		}

		// Function to load paged "Not Sent" addresses
		function loadNotSentActions(startIndex, loadType) {
			loadActions( 
				startIndex, 
				loadType, 
				"admin-mailout-metrics.aspx/GetRowPageForNotSentAddresses",
				loadNotSentActions,
				$('#uxNotSentHeader a'), $('#uxNotSentTable > tbody'), $('#uxNotSentFooter'), $('#uxNotSentCount'), $('#uxNotSentLoadMore'), $('#uxNotSentLoadAll'));
		}

		// Function to load paged "Open" actions
		function loadOpenActions(startIndex, loadType) {
			loadActions( 
				startIndex, 
				loadType, 
				"admin-mailout-metrics.aspx/GetRowPageForOpenActions",
				loadOpenActions,
				$('#uxOpenHeader a'), $('#uxOpenTable > tbody'), $('#uxOpenFooter'), $('#uxOpenCount'), $('#uxOpenLoadMore'), $('#uxOpenLoadAll'));
		}
		
		// Function to load paged "Clicks - By Subscriber" actions
		function loadClickActions(startIndex, loadType) {
			loadActions( 
				startIndex, 
				loadType, 
				"admin-mailout-metrics.aspx/GetRowPageForClickActions",
				loadClickActions,
				$('#uxClickHeader a'), $('#uxClickTable > tbody'), $('#uxClickFooter'), $('#uxClickCount'), $('#uxClickLoadMore'), $('#uxClickLoadAll'));
		}

		// Function to load paged "Clicks - By URL" actions
		function loadClick2Actions(startIndex, loadType) {
			loadActions( 
				startIndex, 
				loadType, 
				"admin-mailout-metrics.aspx/GetRowPageForClicksByUrl",
				loadClick2Actions,
				$('#uxClick2Header a'), $('#uxClick2Table > tbody'), $('#uxClick2Footer'), $('#uxClick2Count'), $('#uxClick2LoadMore'), $('#uxClick2LoadAll'));
		}
		
		// Function to load paged "Forwarded" actions
		function loadForwardActions(startIndex, loadType) {
			loadActions( 
				startIndex, 
				loadType, 
				"admin-mailout-metrics.aspx/GetRowPageForForwardActions",
				loadForwardActions,
				$('#uxForwardHeader a'), $('#uxForwardTable > tbody'), $('#uxForwardFooter'), $('#uxForwardCount'), $('#uxForwardLoadMore'), $('#uxForwardLoadAll'));
		}

		// Function to load paged "Clicks - By URL" actions
		function loadUnsubActions(startIndex, loadType) {
			loadActions( 
				startIndex, 
				loadType, 
				"admin-mailout-metrics.aspx/GetRowPageForUnsubscribeActions",
				loadUnsubActions,
				$('#uxUnsubHeader a'), $('#uxUnsubTable > tbody'), $('#uxUnsubFooter'), $('#uxUnsubCount'), $('#uxUnsubLoadMore'), $('#uxUnsubLoadAll'));
		}

		$(document).ready(function() {
			// Initially hide action containers
			$('#uxSentTableContainer, #uxNotSentTableContainer, #uxOpenTableContainer, #uxClickTableContainer, #uxClick2TableContainer, #uxForwardTableContainer, #uxUnsubTableContainer').hide();

			// Load initial data for action containers
			loadSentActions(1, "page");
			loadNotSentActions(1, "page");
			loadOpenActions(1, "page");
			loadClickActions(1, "page");
			loadClick2Actions(1, "page");
			loadForwardActions(1, "page");
			loadUnsubActions(1, "page");
			// Setup visibility toggling for action containers
			$('#uxSentHeader span a').click(function() { $('#uxSentTableContainer').slideToggle("fast"); }).hide();
			$('#uxNotSentHeader span a').click(function() { $('#uxNotSentTableContainer').slideToggle("fast"); }).hide();
			$('#uxOpenHeader span a').click(function() { $('#uxOpenTableContainer').slideToggle("fast"); }).hide();
			$('#uxClickHeader span a').click(function() { $('#uxClickTableContainer').slideToggle("fast"); }).hide();
			$('#uxClick2Header span a').click(function() { $('#uxClick2TableContainer').slideToggle("fast"); }).hide();
			$('#uxForwardHeader span a').click(function() { $('#uxForwardTableContainer').slideToggle("fast"); }).hide();
			$('#uxUnsubHeader span a').click(function() { $('#uxUnsubTableContainer').slideToggle("fast"); }).hide();
		});
		//]]>
	</script>
</body>
</html>
