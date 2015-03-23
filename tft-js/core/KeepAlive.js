<!--
function sessionKeepAlive(){
	sessionKeepAlive(0);
}
function sessionKeepAlive(dotsToRoot) {
var dots = '';
	for (var i = 0; i < dotsToRoot; i++) {
		dots += "../";
	}
var xmlhttp=false;
/*@cc_on @*/
/*@if (@_jscript_version >= 5)
// JScript gives us Conditional compilation, we can cope with old IE versions.
// and security blocked creation of the objects.
 try {
  xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
 } catch (e) {
  try {
   xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
  } catch (E) {
   xmlhttp = false;
  }
 }
@end @*/
if (!xmlhttp && typeof XMLHttpRequest!='undefined') {
	try {
		xmlhttp = new XMLHttpRequest();
	} catch (e) {
		xmlhttp=false;
	}
}
if (!xmlhttp && window.createRequest) {
	try {
		xmlhttp = window.createRequest();
	} catch (e) {
		xmlhttp=false;
	}
}
 xmlhttp.open("GET", dots + "session-keep-alive.aspx",true);
 xmlhttp.onreadystatechange=function() {
  if (xmlhttp.readyState==4) {
   //alert(xmlhttp.responseText)
  }
 }
 xmlhttp.send(null)

}