<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MediaCollection.ascx.cs" Inherits="Controls_Showcase_MediaCollection" %>
<%@ Import Namespace="BaseCode" %>
<%@ Import Namespace="Classes.Showcase" %>
<div class="thumbnails">
	<asp:Repeater runat="server" ID="uxSliderItemsRepeater" ItemType="Classes.Showcase.Media">
		<ItemTemplate>
			<a id='thumbnailLink_<%=ClientID%>_<%#MediaType == MediaTypes.Image || MediaType == MediaTypes.ImageAndText ? "image" : "video"%>_<%#Container.ItemIndex%>' class="item" title="<%#Item.Caption%>" rel="imageGallery"
				href='<%#MediaType == MediaTypes.Image || MediaType == MediaTypes.ImageAndText ? (Item.URL.ToLower().StartsWith("http") ? Item.URL : Helpers.RootPath + Globals.Settings.UploadFolder + "images/" + Item.URL) : ""%>'>
				<img runat="server" alt='<%#Item.Caption%>' id="uxThumbnailImage"
					src='<%#MediaType == MediaTypes.Image || MediaType == MediaTypes.ImageAndText ? Helpers.ResizedImageUrl(Item.URL, Globals.Settings.UploadFolder + "images/", 155, 102, true, false) : "http://img.youtube.com/vi/" + Item.Thumbnail + "/2.jpg"%>' /></a>
		</ItemTemplate>
	</asp:Repeater>
</div>
<script type="text/javascript">
	// <![CDATA[
	$(document).ready(function () {
		$(".thumbnails .item").fancybox({
			padding: "0", type: 'image',
			afterShow: function () { if ($(parent.$.find("[id$=uxBackToShowcase]")).length == 0) { $(parent.$.find(".fancybox-next")).hide(); $(parent.$.find(".fancybox-prev")).hide(); $(parent.$.find(".fancybox-close")).hide(); } },
			afterClose: function () { if ($(parent.$.find("[id$=uxBackToShowcase]")).length == 0) { $(parent.$.find(".fancybox-next")).show(); $(parent.$.find(".fancybox-prev")).show(); $(parent.$.find(".fancybox-close")).show(); } }
		});
	});
	// ]]>
</script>
