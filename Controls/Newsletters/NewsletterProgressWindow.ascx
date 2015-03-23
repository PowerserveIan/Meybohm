<%@ Control Language="c#" AutoEventWireup="false" %>
<style type="text/css">
	.progressbarcontainer
	{
		width: 98%;
		height: 15px;
		border: solid 1px black;
		padding: 1px;
	}
	.progressbar
	{
		width: 0px;
		background-color: Blue;
		height: 15px;
	}
	div.floatingBox
	{
		position: absolute;
		z-index: 5;
		left: 50%;
		margin-left: -200px;
		top: 40%;
	}
	div.progressBox
	{
		background-color: #999999;
		text-align: left;
		padding: 10px;
		color: #000000;
		position: relative;
		left: -10px;
		bottom: 141px !important;
		bottom: 145px;
		filter: alpha(opacity=95);
		-moz-opacity: .95;
		opacity: .95;
		width: 400px;
	}
</style>
<div id="divFloating" class="floatingBox" style="display: none;">
	<div id="divProgress" class="progressBox">
		<div id="divContent">
			Preparing emails to send<br />
		</div>
		<a onclick="CloseWindow()" href="#" style="color: #000000; font-weight: bold;">Click
			here to close this window</a><br />
		<span style="font-size: small">NOTE: This is just a progress indicator. Closing the
			window will not stop the emails from sending.</span>
	</div>
</div>

<script type="text/javascript">
	//<![CDATA[
	function createRequestObject() {
		var ro;
		var browser = navigator.appName;
		if (browser == "Microsoft Internet Explorer") {
			ro = new ActiveXObject("Microsoft.XMLHTTP");
		} else {
			ro = new XMLHttpRequest();
		}
		return ro;
	}

	var http = createRequestObject();
	var complete = false;

	function sndReq(action) {
		var d = new Date();
		http.open('get', '<%=HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "/sending-emails.aspx"%>?t=' + d.getMilliseconds() + '&action=' + action);
		http.onreadystatechange = handleResponse;
		http.send(null);
	}

	function handleResponse() {
		if (http.readyState == 4) {
			var response = http.responseText;
			//alert(response);
			if (response == '')
				CloseWindow();
			else if (response != 'sent') {
				setTimeout("sndReq('updateprogress')", 1000);
				$('#divContent').html(response);

			}
			else if (!complete) {
				sndReq('updatesent');
				complete = true;
			}
		}
	}

	function CloseWindow() {
		$('#divFloating').hide();
	}

	$(document).ready(function() {
		sndReq('updateprogress');
	});
	//]]>
</script>

