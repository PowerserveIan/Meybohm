<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminBar.ascx.cs" Inherits="Controls_BaseControls_AdminBar" %>
<asp:PlaceHolder runat="server" ID="uxBottomBarPH">
	<div class="adminToolbar">
		<div class="adminToolbarWrapper clearfix">
			<a class="adminToolbarHide" href="#">Hide</a>
			<asp:HyperLink ID="uxLogoutBB" runat="server" CssClass="adminToolbarLogout"><span>Logout</span></asp:HyperLink>
			<asp:LinkButton runat="server" ID="uxClearCaches" CssClass="floatRight" Text="<span>Clear Site's Caches</span>" OnClientClick="return confirm('Are you sure you want to clear the cache for your entire site?');" CausesValidation="false" ToolTip="Clears the Cache on your website to ensure all content a user sees is up to date" />
			<asp:HyperLink ID="uxReturnToAdmin" NavigateUrl="~/admin/" CssClass="adminToolbarFirst" runat="server"><span>Return to Admin</span></asp:HyperLink>
			<asp:HyperLink ID="uxEditPageProperties" runat="server" CssClass="fancybox.iframe"><span>Edit Page Properties</span></asp:HyperLink>
			<asp:PlaceHolder runat="server" ID="uxCMSPH">
				<ul class="popUp editPage clearfix">
					<li>
						<a class="editPage"><span>Edit Page Content</span></a>
						<asp:Repeater runat="server" ID="uxPageRegions">
							<HeaderTemplate>
								<ul>
							</HeaderTemplate>
							<ItemTemplate>
								<li>
									<a class="pageRegionLink" href="#">
										<%# ((KeyValuePair<string,string>)Container.DataItem).Key %></a>
									<asp:HiddenField runat="server" ID="uxContentRegionID" Value='<%# ((KeyValuePair<string,string>)Container.DataItem).Value + "_editableContent" %>' />
								</li>
							</ItemTemplate>
							<FooterTemplate>
								</ul>
							</FooterTemplate>
						</asp:Repeater>
					</li>
				</ul>
				<script type="text/javascript">
					$(document).ready(function () {
						$("a.pageRegionLink").hover(function () {
							$("#" + $(this).siblings().val()).addClass("regionInFocus");
							$("html, body").animate({ scrollTop: $("#" + $(this).siblings().val()).offset().top - $("#" + $(this).siblings().val()).siblings("a.editButton").height() - 55 });
						}, function () {
							$("#" + $(this).siblings().val()).removeClass("regionInFocus");
						});
						$("a.pageRegionLink").click(function () {
							$("#" + $(this).siblings().val()).prev().trigger("click");
							return false;
						});
					});
				</script>
			</asp:PlaceHolder>
			<asp:HyperLink ID="uxComponentAdmin" CssClass="adminToolbarLast" runat="server"><span>Component Manager</span></asp:HyperLink>
		</div>
	</div>
	<script type="text/javascript">
		$(function () {
			$('.adminToolbar .fancybox\\.iframe').fancybox({ 'width': 705, 'height': 600 });
			var $tftBar = $('div.adminToolbar'),
				tftBarHeight = $tftBar.children('div.adminToolbarWrapper').outerHeight(),
				barGoingUp = false,
				barGoingDown = false;
			$('html').addClass('adminBarOn').css('padding-top', '+=' + tftBarHeight);
			$('a.adminToolbarHide').click(function () {
				if (!$tftBar.hasClass('autohide'))
					tftBarUp();
				$tftBar.toggleClass('autohide');
				return false;
			});
			function tftBarUp() {
				barGoingUp = true;
				$('html').animate({ paddingTop: '-=' + tftBarHeight }, 500);
				$tftBar.animate({ top: -tftBarHeight - 5, paddingBottom: '+=12' }, 500, function () { barGoingUp = false; });
			}
			function tftBarDown() {
				barGoingDown = true;
				$('html').animate({ paddingTop: '+=' + tftBarHeight }, 500);
				$tftBar.animate({ top: 0, paddingBottom: 0 }, 500, function () { barGoingDown = false; });
			}
			$tftBar.hover(function () {
				if ($tftBar.hasClass('autohide') && !barGoingDown)
					tftBarDown();
			},
			function () {
				if ($tftBar.hasClass('autohide') && !barGoingUp)
					tftBarUp();
			});
		});
	</script>
</asp:PlaceHolder>
