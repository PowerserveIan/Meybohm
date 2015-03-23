<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DynamicHeader.ascx.cs" Inherits="Controls_DynamicHeader_DynamicHeader" %>
<%@ Import Namespace="Classes.DynamicHeader" %>
<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/slideshow.css" id="uxCSSFiles" />
<asp:PlaceHolder runat="server" ID="uxSlideShowPH">
	<ul class="images" runat="server" id="uxImages">
		<asp:Repeater runat="server" ID="uxImagesRepeater" ItemType="Classes.DynamicHeader.DynamicImage">
			<ItemTemplate>
				<li<%# Container.ItemIndex > 0 ? " style=\"display:none;\"": "" %>>
					<asp:HyperLink id="uxSlideLink" runat="server" NavigateUrl='<%# Item.IsVideo ? "~/" + BaseCode.Globals.Settings.UploadFolder + "images/" + Item.Name : (String.IsNullOrEmpty(Item.Link) || Item.Link.StartsWith("http") ? Item.Link : BaseCode.Helpers.RootPath + Item.Link) %>' ToolTip="<%# Item.Title %>">
						<img runat="server" src='<%# "~/" + BaseCode.Globals.Settings.UploadFolder + "images/" + Item.Name + "?width=" + ImageWidth + "&height=" + ImageHeight + "&mode=crop&anchor=middlecenter" %>'
					alt="<%# Item.Title %>" width="<%# ImageWidth %>" height="<%# ImageHeight %>" visible="<%# !Item.IsVideo %>"/>
					</asp:HyperLink>
					<asp:HiddenField runat="server" ID="uxDuration" Value="<%# Item.Duration.HasValue ? Item.Duration : Settings.DefaultCycleSpeed %>" />
					<asp:PlaceHolder runat="server" Visible="<%# Settings.EnableCaptions %>"><div class="caption">
						<%# Item.Caption%></div>
					</asp:PlaceHolder>
				</li>
			</ItemTemplate>
		</asp:Repeater>
	</ul>
</asp:PlaceHolder>
<asp:Label runat="server" ID="uxCollectionNotFound" Text="The collection you are looking for has been removed." Visible="false"></asp:Label>
<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/slideshow.js" />
<script type="text/javascript">
	$(document).ready(function () {
		$("#<%= uxImages.ClientID %>").slideShow({
			genericThumbnail: <%= GenericThumbnail.ToString().ToLower() %>,
			hasVideos: <%= m_HasVideos.ToString().ToLower() %>,
			slideHeight: <%= ImageHeight %>,
			slideWidth: <%= ImageWidth %>,
			playByDefault: <%= PlayByDefault.ToString().ToLower() %>,
			showNavigation: <%= ShowNavigation.ToString().ToLower() %>,
			showNumbersAsThumbnails: <%= ShowNumbers.ToString().ToLower() %>,
			showThumbnails: <%= ShowThumbnails.ToString().ToLower() %>,
			thumbnailHeight: <%= ThumbnailHeight %>,
			thumbnailWidth: <%= ThumbnailWidth %>,
			transition: "<%= Transition.ToString() %>"
		});
	});
</script>
