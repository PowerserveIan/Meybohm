(function () { tinymce.create('tinymce.plugins.ClearDiv', { init: function (ed, url) { ed.addCommand('mceClearDiv', function () { ed.execCommand('mceInsertContent', false, '<div class="clear"></div>', null) }); ed.addButton('cleardiv', { title: 'cleardiv.cleardiv_title', cmd: 'mceClearDiv', image: url.replace("plugins/cleardiv", "") + 'themes/advanced/img/clearDiv.gif' }) }, getInfo: function () { return { longname: 'Insert Clear Div', author: 'Zach Floyd', authorurl: 'http://352media.com', version: tinymce.majorVersion + "." + tinymce.minorVersion} } }); tinymce.PluginManager.add('cleardiv', tinymce.plugins.ClearDiv) })();