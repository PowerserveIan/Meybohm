/**
 * editor_plugin_src.js
 *
 * Copyright 2009, Moxiecode Systems AB
 * Released under LGPL License.
 *
 * License: http://tinymce.moxiecode.com/license
 * Contributing: http://tinymce.moxiecode.com/contributing
 */

(function() {
	tinymce.create('tinymce.plugins.ClearDiv', {
		init : function(ed, url) {
			// Register commands
			ed.addCommand('mceClearDiv', function() {
				ed.execCommand('mceInsertContent', false, '<div class="clear"></div>', null);
			});

			// Register buttons
			ed.addButton('cleardiv', {
				title: 'cleardiv.cleardiv_title',
				cmd: 'mceClearDiv',
				image: url.replace("plugins/cleardiv", "") + 'themes/advanced/img/clearDiv.gif'
			});
		},

		getInfo : function() {
			return {
				longname : 'Insert Clear Div',
				author : 'Zach Floyd',
				authorurl : 'http://352media.com',
				version : tinymce.majorVersion + "." + tinymce.minorVersion
			};
		}
	});

	// Register plugin
	tinymce.PluginManager.add('cleardiv', tinymce.plugins.ClearDiv);
})();