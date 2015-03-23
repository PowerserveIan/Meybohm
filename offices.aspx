<%@ Page Title="Offices" Language="C#" MasterPageFile="~/frontend.master" AutoEventWireup="true" CodeFile="offices.aspx.cs" Inherits="offices" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="Server">
    <script>
        $(function () {
            $('.hoods.offices li.temp-block .officeManager .officeManagerName').hide();

            $('.hoods.offices li.temp-block .officeManager').hover(
                function () {
                    var name = $(this).find('.officeManagerName');
                    $(this).stop(true, false).animate({ height: "170px", width: "140px", margin: "30px 0 0 -60px" }, 300, function () {
                        name.fadeIn(300);
                    });
                },
                function () {
                    var name = $(this).find('.officeManagerName');
                    name.hide();

                    $(this).stop(true, false).animate({ height: "50px", width: "50px", margin: "85px 0 0 10px" }, 300, function () {
                        name.hide(300);
                    });
                });
        });
    </script>
	<div class="hoods offices">
		<div class="headerBG">
			<h1>Meybohm Offices</h1>
		</div>
		<div class="hoodDirect">
			<h3>Office Directory</h3>
			<asp:Repeater ID="uxOffices" runat="server" ItemType="Powerserve.Meybohm.Model.OfficeInfo">
				<HeaderTemplate>
					<ul>
				</HeaderTemplate>
				<ItemTemplate>
					<li class="temp-block">
                        <div class="officeManager">
                            <div class="officeManagerImage">
                                <img alt="" src='<%# ResolveUrl(BaseCode.Globals.Settings.UploadFolder + "agents/" + Item.ManagerImage) %>' />
                            </div>
                            <div class="officeManagerName">
                                Office Manager<br /><%# Item.ManagerFirstName %> <%# Item.ManagerLastName %>
                            </div>
                        </div>
                        <div class="officeImage">
						    <img alt="" src='<%# ResolveUrl(BaseCode.Globals.Settings.UploadFolder + "offices/" + Item.OfficeImage) %>' />
                        </div>
                        <div class="officeInfo">
						    <a href="#" class="officeName"><%# Item.OfficeName %></a>
						    <span><%# Item.OfficeAddress %></span>
						    <span><%# string.Format("{0}, {1} {2}", Item.OfficeCity, Item.OfficeState, Item.OfficeZip)%></span>
                            <span class="officeManagerName">Office Manager: <%# Item.ManagerFirstName %> <%# Item.ManagerLastName %></span>
						    <span>Fax: <%# Item.OfficeFax %></span>
						    <span>Phone: <%# Item.OfficePhone %></span>
                        </div>
                        <div class="clear"></div>
					</li>
				</ItemTemplate>
				<FooterTemplate>
					</ul>
				</FooterTemplate>
			</asp:Repeater>
			<div class="clear"></div>
		</div>
		<div class="mapContainer"></div>
	</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentSideCol"></asp:Content>
<asp:Content ContentPlaceHolderID="PageSpecificJS" runat="Server">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/google-maps.js"></asp:Literal>
	<script type="text/javascript">
		var markerList = [<%= m_OfficesJS %>];

		$(document).ready(function () {
			$(".content").addClass("map").addClass("full").addClass("officesHome");

			$("a.officeName").click(function () {
				google.maps.event.trigger(markers[$("li.temp-block").index($(this).parents("li.temp-block"))], 'click');
				window.scrollTo(0, $('.mapContainer').offset().top - 50);
				return false;
			});
		});
	</script>
</asp:Content>

