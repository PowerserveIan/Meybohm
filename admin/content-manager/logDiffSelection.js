
var highlightedRegion; var selectedRegion;;
function mouseover(cmPageRegionId){
	if(highlightedRegion!=null&&highlightedRegion!=selectedRegion) {
		document.getElementById(highlightedRegion).style.backgroundColor="";
		document.getElementById(highlightedRegion).style.color="";
		document.getElementById('rest'+highlightedRegion).style.color="#970001";
		document.getElementById('diff'+highlightedRegion).style.color="gray";
		document.getElementById('diff'+highlightedRegion).href="#";
	} else if (highlightedRegion!=null) {
		document.getElementById('diff'+highlightedRegion).style.color="gray";
		document.getElementById('diff'+highlightedRegion).href="#";
	}
	if (selectedRegion!=cmPageRegionId)
	{
		document.getElementById(cmPageRegionId).style.backgroundColor="#FFBF5E";
	}
	document.getElementById(cmPageRegionId).style.color="gray";
	document.getElementById('rest'+cmPageRegionId).style.color="gray";
	if(selectedRegion!=null&&cmPageRegionId!=selectedRegion) {
		document.getElementById('diff'+cmPageRegionId).style.color="#970001";
		document.getElementById('diff'+cmPageRegionId).href="#";
	}
	highlightedRegion=cmPageRegionId
	return null;
}
function rowclick(cmPageRegionId) {
	if(selectedRegion!=null){
		document.getElementById(selectedRegion).style.backgroundColor="";
		document.getElementById(selectedRegion).style.color="";
		document.getElementById('rest'+selectedRegion).style.color="#970001";
	}
	if(highlightedRegion==cmPageRegionId) {
		document.getElementById('diff'+cmPageRegionId).style.color="gray";
		document.getElementById('diff'+cmPageRegionId).href="#";
	}
	document.getElementById(cmPageRegionId).style.backgroundColor="#FFFFCC";
	document.getElementById(cmPageRegionId).style.color="gray";
	selectedRegion = cmPageRegionId;
}
function diffClick(cmPageRegionId) {
	if(selectedRegion!=cmPageRegionId&&selectedRegion!=undefined)
		window.open("content-manager-diff.aspx?leftid="+selectedRegion+"&rightid="+cmPageRegionId,"","","");
}