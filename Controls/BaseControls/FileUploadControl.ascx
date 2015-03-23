<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FileUploadControl.ascx.cs" Inherits="Controls_BaseControls_FileUploadControl" %>
<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/jquery.fileupload-ui.css" id="uxCSSFiles" />
<style type="text/css">
    .files .template-upload
    {
        cursor:default;
    }
    .cancel > button:hover 
    {
        cursor: pointer;
    }
    .cancel > button
    {
        border: solid 1px black;

        /* IE10 Consumer Preview */ 
        background-image: -ms-linear-gradient(right, rgba(0,0,0,0) 0%, #A30000 100%), 
        -ms-linear-gradient(right, rgba(0,0,0,0) 0%, #800000 100%), 
        -ms-linear-gradient(top, #A30000 0%, #800000 100%);
    
        /* Mozilla Firefox */ 
        background-image: -moz-linear-gradient(right, rgba(0,0,0,0) 0%, #A30000 100%), 
            -moz-linear-gradient(right, rgba(0,0,0,0) 0%, #800000 100%), 
            -moz-linear-gradient(top, #A30000 0%, #800000 100%);
    
        /* Opera */ 
        background-image: -o-linear-gradient(right, rgba(0,0,0,0) 0%, #A30000 100%), 
            -o-linear-gradient(right, rgba(0,0,0,0) 0%, #800000 100%), 
            -o-linear-gradient(top, #A30000 0%, #800000 100%);
    
        /* Webkit (Safari/Chrome 10) */ 
        background-image: -webkit-gradient(linear, right top, left top, color-stop(0, rgba(0,0,0,0)), color-stop(1, #A30000)), 
            -webkit-gradient(linear, right top, left top, color-stop(0, rgba(0,0,0,0)), color-stop(1, #800000)), 
            -webkit-gradient(linear, left top, left bottom, color-stop(0, #A30000), color-stop(1, #800000));
    
        /* Webkit (Chrome 11+) */ 
        background-image: -webkit-linear-gradient(right, rgba(0,0,0,0) 0%, #A30000 100%), 
            -webkit-linear-gradient(right, rgba(0,0,0,0) 0%, #800000 100%), 
            -webkit-linear-gradient(top, #A30000 0%, #800000 100%);
    
        /* W3C Markup, IE10 Release Preview */ 
        background-image: linear-gradient(to left, rgba(0,0,0,0) 0%, #A30000 100%),
            linear-gradient(to left, rgba(0,0,0,0) 0%, #800000 100%), 
            linear-gradient(to bottom, #A30000 0%, #800000 100%);
    
        background-size: 100% 50%, 100% 50%, 100% 100%;
        background-position: 0 0, 0 100%, 0 0;
        background-repeat: no-repeat;
    }
    .cancel > button > .ui-button-text
    {
        padding-left:18px;
        font-weight: normal;
        line-height: 30px;
        font-size: 12px;
    }
    .files th
    {
        font-weight: bold;
        text-align: center;
        display: none;
    }
    .textCaption
    {
        width:150px !important;
    }
</style>
<div class="uploadControl">
	<% if (!ReadOnly){ %>
	<div class="uploader fileupload-buttonbar" runat="server" id="uxUploader">
		<label class="fileinput-button">
			<span>
                <asp:Literal runat="server" ID="uxUploadButtonText"></asp:Literal>
			</span>
			<input type="file" name="files[]" id="uxFileUpload" runat="server" multiple />
		</label>
		<asp:PlaceHolder runat="server" ID="uxExternalImagePH" Visible="false">
			<asp:TextBox CssClass="text externalUrl" runat="server" ID="uxImageExternal" MaxLength="255" Placeholder="Enter external URL" />
			<asp:RegularExpressionValidator runat="server" ID="uxImageExternalREV" ControlToValidate="uxImageExternal" ErrorMessage="External image url is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
		</asp:PlaceHolder>
        <div class="fileupload-content">
			<table class="files">
                <thead>
                    <tr>
                        <th></th>
                        <th>Name</th>
                        <th>Size</th>
                        <th>Caption</th>
                    </tr>
                </thead>
			</table>
		</div>
	</div>
	<asp:CustomValidator runat="server" ID="uxFileUploadRFV" ClientValidationFunction="ValidateRequiredFile" ErrorMessage="You must upload a file." Enabled="false" /><% } %>
	<div id="files_<%= ClientID %>">
		<div runat="server" id="uxDeletePH">
			<%if (IsImage()){%>
			<div class="imageDiv">
				<%}%>
				<asp:HyperLink runat="server" ID="uxFancyLink" Target="_blank" CssClass="fancybox">
					<asp:Image runat="server" ID="uxImage" />
				</asp:HyperLink>
				<% if (IsImage()){%></div>
			<% if (!IsUploadExternal()){ %>
			<a href="#" class="button edit editImageProp floatLeft paddingTop"><span>Edit Image Properties</span></a>
			<%} }%>
			<span style="display: block;">
				<asp:Label runat="server" ID="uxFileNotFound" ForeColor="Red" Visible="false" /></span><% if (!ReadOnly){ %>
			<a href="#" id="markForDeletion" runat="server" title="Mark for Deletion" class="button delete floatLeft paddingTop"><span>Mark for Deletion</span></a><% } %>
		</div>
	</div>
	<span class="deletedMessage" style="display: none;" runat="server" id="uxDeletedMessage">File will be deleted upon Save.</span>
	<div class="clear"></div>
	<!--end uploadControl-->
</div>
<asp:HiddenField runat="server" ID="uxFileUploaded" />
<asp:HiddenField runat="server" ID="uxFileCaption" />
<asp:HiddenField runat="server" ID="uxFileDeleted" />
<script type="text/javascript" src="//ajax.aspnetcdn.com/ajax/jquery.templates/beta1/jquery.tmpl.min.js"></script>
<script type="text/javascript" src='<%= ResolveClientUrl("~/") %>tft-js/core/tiny_mce/plugins/imagemanager/js/mcimagemanager.js'></script>
<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/jquery-ui.custom.min.js,~/tft-js/core/jquery.iframe-transport.js,~/tft-js/core/jquery.fileupload.js,~/tft-js/core/jquery.fileupload-ui.js"></asp:Literal>
<script type="text/javascript">
<% 
    if (Required && !ReadOnly)
	{ 
%>      function ValidateRequiredFile_<%=ClientID%>(source, args) 
        {
            args.IsValid = $("#<%=uxFileUploaded.ClientID %>").val() != '' || (<%= (!String.IsNullOrEmpty(FileName)).ToString().ToLower() %> && $("#<%=uxFileDeleted.ClientID %>").val() == '')
                <% if (AllowExternalImageLink) {%> || $("#<%=uxImageExternal.ClientID %>").val() != ''<% } %>;
        }
<%
    } 
%>
    var filesUploaded = 0;
    $(document).ready(function () {<% if (!ReadOnly){ %>

<% 
        if (String.IsNullOrEmpty(FileName) && IsMultipleEnabled)
        {
%>
        $('.formHalf').hide();

        $('#ctl00_ContentWindow_uxSave, #ctl00_ContentWindow_uxSaveAndAddNew').on('click', function(e)
        {
            var strImageName;
            var strImageCaption;

            $("#<%= uxFileUploaded.ClientID %>").val('');
            $("#<%= uxFileCaption.ClientID %>").val('');

            $('.template-upload').each(function()
            {
                strImageName = $(this).children('.name').text();
                strImageCaption = $(this).children('.caption').children('.textCaption').val();

                $("#<%= uxFileUploaded.ClientID %>").val(strImageName + ',' + $("#<%= uxFileUploaded.ClientID %>").val());
                $('#<%= uxFileCaption.ClientID %>').val(strImageCaption + ',' + $('#<%= uxFileCaption.ClientID %>').val());
            });
        });
<% 
        }
%>

		$("#<%= uxUploader.ClientID %>").fileupload({
			autoUpload: true,
			maxFileSize: <%= MaxFileSize %>,
			acceptFileTypes: new RegExp("\.<%= AllowedFileTypes %>$", "i"),
			namespace: 'file_upload_<%= uxFileUpload.ClientID %>',
			fileInput: $("#<%= uxFileUpload.ClientID %>"),
			dropZone: $("#<%= uxUploader.ClientID %>"),
			done: function (e, data){
				var file = data.files[0];
				
				$("#<%= uxDeletedMessage.ClientID %>").hide();
<% 
                if (String.IsNullOrEmpty(FileName) && IsMultipleEnabled)
                {
%>
			        //$("#<%= uxFileUploaded.ClientID %>").val(file.name + ',' + $("#<%= uxFileUploaded.ClientID %>").val());
			        //$('.progress').after('<td class="caption"><input type="text" class="textCaption" /></td>');

			        filesUploaded++;
			        
			        //alert(filesUploaded + ' of ' + $('.template-upload').length);

			        if(filesUploaded == $('.template-upload').length)
			        {
			            $('.cancel > button > .ui-button-text').html('Remove');

			            $('.template-upload').each(function()
			            {
			                if($(this).children('.caption').children('.textCaption').val() == undefined)
			                {
			                    $(this).children('.progress').after('<td class="caption"><input type="text" class="textCaption text" maxlength="50" /></td>');
			                }
			            });
                    
			            $("#<%= uxFileUploaded.ClientID %>").val('loaded.png');
			            $('.progress').hide();
			            $('.files th').show();
			        }
			    <%
                }
                else
                {
%>
			        $("#<%= uxFileUploaded.ClientID %>").val(file.name);
			        $("#<%= uxDeletePH.ClientID %>").show();
			        $("#<%= markForDeletion.ClientID %>").show();
			        $("#<%= uxFancyLink.ClientID %>").attr("href", '<%= BaseCode.Helpers.RootPath + BaseCode.Globals.Settings.UploadFolder + "temp/" %>' + file.name);
			        
			        if (file.name.toLowerCase().indexOf(".jpg") > 0 || file.name.toLowerCase().indexOf(".jpeg") > 0 || file.name.toLowerCase().indexOf(".gif") > 0 || file.name.toLowerCase().indexOf(".png") > 0 || file.name.toLowerCase().indexOf(".bmp") > 0)
			        {
			            $("#<%= uxFancyLink.ClientID %>").html('<img alt="' + file.name + '" src="<%= BaseCode.Helpers.RootPath + "resizer.aspx?width=" + ImageWidth + "&height=" + ImageHeight + "&trim=1&filename=" + BaseCode.Globals.Settings.UploadFolder + "temp/" %>' + file.name + '" /><span style="display:none"></span>');
				        FancyImage();
				    }
			        else 
			        {
				        $("#<%= uxFancyLink.ClientID %>").html(file.name);
				        $("#<%= uxFancyLink.ClientID %>").unbind('click.fb'); 
				    }
			        
			        $("#<%= uxUploader.ClientID %>").hide();
			        $("#<%= uxUploader.ClientID %> table.files tr").remove();
<%
                }
%>

				return ""; 
			}
		});	

	    $('.files').on('click', '.cancel', function()
	    {
	        if($(this).siblings('.progress').children('.ui-progressbar').attr('aria-valuenow') == '100')
	        {
	            filesUploaded--;
	        }
	        $(this).parent().remove();

	        if($('.template-upload').length == 0)
	        {
	            filesUploaded = 0;
	            $("#<%= uxFileUploaded.ClientID %>").val('');
	        }

	        if(filesUploaded == $('.template-upload').length)
	        {
	            $('.cancel > button > .ui-button-text').html('Remove');

	            $('.template-upload').each(function()
	            {
	                if($(this).children('.caption').children('.textCaption').val() == undefined)
	                {
	                    $(this).children('.progress').after('<td class="caption"><input type="text" class="textCaption text" maxlength="50" /></td>');
	                }
	            });
                    
	            $("#<%= uxFileUploaded.ClientID %>").val('loaded.png');
			    $('.progress').hide();
			    $('.files th').show();
            }
	    });
        
		$("#<%= markForDeletion.ClientID %>").click(function(){
			$("#<%= uxDeletePH.ClientID %>").hide();
			$("#<%= uxFileNotFound.ClientID %>").hide();
			$("#<%= uxDeletedMessage.ClientID %>").show();
			$("#<%= uxUploader.ClientID %>").show();
			$("#<%= uxFileUploaded.ClientID %>").val("");
			$("#<%= uxUploader.ClientID %> table.files tr").remove();
			<% if (!String.IsNullOrEmpty(FileName))
	  { %>$("#<%= uxFileDeleted.ClientID %>").val("true");<%} %>
			return false;
		});<% }%>
		function FancyImage(){
			$("#<%=uxFancyLink.ClientID%>").fancybox({type: "image"});
			$(".imageDiv a").append("<span></span>");
			$(".imageDiv a").hover(function () {
				$(this).children("span").stop().fadeTo(600, '1');
			}, function () {
				$(this).children("span").stop().fadeTo(600, '0');
			});
			if ($("#<%=uxFileNotFound.ClientID %>").length > 0)
				$("#<%= uxDeletePH.ClientID %> .editImageProp").hide();
			else
				$("#<%= uxDeletePH.ClientID %> .editImageProp").show();
			$("#<%= uxDeletePH.ClientID %> .editImageProp").click(function(){
				var editedFilename = $("#<%= uxFancyLink.ClientID %>").attr("href").split("?",1)[0].replace("<%= BaseCode.Helpers.RootPath %>","").replace(/%20/g, " ");
				mcImageManager.edit({
					relative_urls: true,
					path: '{0}' + editedFilename,
					onsave: function(file) {
						window.close();
						$.ajax({
							async: false,
							type: 'POST',
							data: {
								"killCache": "true",
								"filename": editedFilename
							},
							success: function (r) {
								$("#<%= uxFancyLink.ClientID %> img").attr("src", $("#<%= uxFancyLink.ClientID %> img").attr("src") + "&d=" + new Date().getTime());
								$("#<%= uxFancyLink.ClientID %>").attr("href", $("#<%= uxFancyLink.ClientID %>").attr("href") + "?d=" + new Date().getTime());
							}
						});					  
					},
					height: <%= ImageHeight %>,
					width: <%= ImageWidth %>
					});
				return false;
			});
		}
		<% if (IsImage())
	 { %>FancyImage();<%} %>
	});
</script>
<script id="template-upload" type="text/x-jquery-tmpl">
	<tr class="template-upload{{if error}} ui-state-error{{/if}}">
		<td class="preview"></td>
		<td class="name">{{if name}}${name}{{else}}Untitled{{/if}}</td>
		<td class="size">${sizef}</td>
		{{if error}}
            <td class="error" colspan="2">
				Error:
                {{if error === 'maxFileSize'}}Max File Size is <%= MaxFileSize / 1000 %>KB
                {{else error === 'minFileSize'}}File is too small
                {{else error === 'acceptFileTypes'}}Filetype not allowed
                {{else error === 'maxNumberOfFiles'}}Max number of files exceeded
                {{else}}${error}
                {{/if}}
			</td>
		{{else}}
            <td class="progress">
				<div></div>
			</td>
		<td class="start">
			<button>Start</button>
		</td>
		{{/if}}
        <td class="cancel">
			<button>Cancel</button>
		</td>
	</tr>
</script>
