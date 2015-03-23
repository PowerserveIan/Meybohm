var editorLoaded = false;
function InitializeTinyMCE(id, dotsToRoot) {
	editorLoaded = true;
	var dots = '';
	for (var i = 0; i < dotsToRoot; i++) {
		dots += "../";
	}
	$("textarea#" + id).tinymce({
		script_url: dots + 'tft-js/core/tiny_mce/tiny_mce.js',
		relative_urls: false,
		remove_script_host: false,
		theme: "advanced",
		plugins: "safari,pagebreak,style,layer,table,save,advimage,advlink,inlinepopups,preview,media,searchreplace,contextmenu,paste,fullscreen,noneditable,xhtmlxtras,imagemanager,filemanager,cleardiv,spellchecker",
		theme_advanced_buttons1: "bold,italic,strikethrough,|,bullist,numlist,|,outdent,indent,blockquote,sub,sup,|",
		theme_advanced_buttons2: "styleselect,formatselect,cleardiv",
		theme_advanced_buttons3: "pastetext,pasteword,|,search,replace,|,undo,redo,|",
		theme_advanced_buttons4: "link,unlink,anchor,image,insertfile,|",
		theme_advanced_buttons5: "table,visualaid,|,hr,removeformat",
		theme_advanced_buttons6: "spellchecker,fullscreen,cleanup,help,code,|,preview",
		theme_advanced_toolbar_location: "top",
		theme_advanced_toolbar_align: "left",
		theme_advanced_statusbar_location: "bottom",
		theme_advanced_styles: "Required=required;Float Left=floatLeft;Float Right=floatRight",
		theme_advanced_resizing: true,
		forced_root_block: false,
		media_strict: false,
		encoding: "xml",
		valid_children : "+body[style]",
		extended_valid_elements: "div[*],iframe[*],input[*],textarea[*]",
		invalid_elements: "form",
		setup: function (ed) {
			ed.onInit.add(function () {
				$(".mceIframeContainer, .mceIframeContainer iframe").attr('style', 'height: 100%;width:100%;');
				if ($(".mceLayout").parents(".CMBackground").length > 0)
					$(".mceLayout").attr('style', 'width: ' + $(".mceLayout").parents(".CMBackground").find("textarea.tinymce").width() + "px;");
			});
		},
		spellchecker_rpc_url: dots + "TinyMCEHandler.ashx?module=SpellChecker"
	});
}