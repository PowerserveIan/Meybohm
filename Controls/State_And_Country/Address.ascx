<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Address.ascx.cs" Inherits="Controls_State_And_Country_Address" %>
<%@ Register TagPrefix="Controls" TagName="Zipcode" Src="~/Controls/BaseControls/ZipcodeControl.ascx" %>
<div class="formWhole">
	<label for="<%= uxAddress.ClientID %>">
		<asp:Literal runat="server" ID="uxAddressLabel" Text="Address"></asp:Literal><asp:Literal runat="server" ID="uxAddressReqAst"><span class="asterisk">*</span></asp:Literal></label>
	<asp:TextBox runat="server" ID="uxAddress" MaxLength="255" CssClass="text" />
	<asp:RequiredFieldValidator runat="server" ID="uxAddressReqFVal" ControlToValidate="uxAddress" ErrorMessage="Address is required." />
	<asp:RegularExpressionValidator runat="server" ID="uxAddressRegexVal" ControlToValidate="uxAddress" ErrorMessage="Address is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
</div>
<asp:PlaceHolder runat="server" ID="uxAddress2PH">
	<div class="formWhole">
		<label for="<%= uxAddress2.ClientID %>">
			<asp:Literal runat="server" ID="uxAddress2Label" Text="Address 2"></asp:Literal></label>
		<asp:TextBox runat="server" ID="uxAddress2" MaxLength="255" CssClass="text" />
		<asp:RegularExpressionValidator runat="server" ID="uxAddress2RegexVal" ControlToValidate="uxAddress2" ErrorMessage="Address 2 is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
	</div>
</asp:PlaceHolder>
<div class="formHalf">
	<label for="<%= uxCity.ClientID %>">
		City<asp:Literal runat="server" ID="uxCityReqAst"><span class="asterisk">*</span></asp:Literal></label>
	<asp:TextBox runat="server" ID="uxCity" MaxLength="50" CssClass="text" />
	<asp:RequiredFieldValidator runat="server" ID="uxCityReqFVal" ControlToValidate="uxCity" ErrorMessage="City is required." />
	<asp:RegularExpressionValidator runat="server" ID="uxCityRegexVal" ControlToValidate="uxCity" ErrorMessage="City is too long.  It must be 50 characters or less." ValidationExpression="^[\s\S]{0,50}$" />
</div>
<div class="formHalf">
	<label for="<%= uxStateID.ClientID %>">
		State<asp:Literal runat="server" ID="uxStateReqAst"><span class="asterisk">*</span></asp:Literal></label>
	<asp:DropDownList runat="server" ID="uxStateID" AppendDataBoundItems="true">
		<asp:ListItem Text="--Select a State--" Value=""></asp:ListItem>
	</asp:DropDownList>
	<asp:RequiredFieldValidator runat="server" ID="uxStateIDReqFVal" ControlToValidate="uxStateID" ErrorMessage="State is required." InitialValue="" />
</div>
<div class="clear"></div>
<div class="formHalf">
	<label for="<%=uxZip.ClientID%>_uxZipcode">
		<asp:Literal runat="server" ID="uxZipcodeLabel" Text="Postal Code"></asp:Literal><asp:Literal runat="server" ID="uxZipReqAst"><span class="asterisk">*</span></asp:Literal></label>
	<Controls:Zipcode runat="server" ID="uxZip" TextBoxClass="text" />
</div>
<asp:PlaceHolder runat="server" ID="uxLatLongPH">
	<div class="formHalf">
		<label for="<%= uxLatitude.ClientID %>">
			Latitude<asp:Literal runat="server" ID="uxLatitudeReqAst"><span class="asterisk">*</span></asp:Literal></label>
		<asp:TextBox runat="server" ID="uxLatitude" CssClass="text" />
		<asp:RequiredFieldValidator runat="server" ID="uxLatitudeReqFVal" ControlToValidate="uxLatitude" ErrorMessage="Latitude is required." />
		<asp:RangeValidator runat="server" ID="uxLatitudeRangeVal" ControlToValidate="uxLatitude" Type="Double" MinimumValue="-90" MaximumValue="90" ErrorMessage="Latitude must be a decimal between -90 and 90." />
	</div>
	<div class="formHalf">
		<label for="<%= uxLongitude.ClientID %>">
			Longitude<asp:Literal runat="server" ID="uxLongitudeReqAst"><span class="asterisk">*</span></asp:Literal></label>
		<asp:TextBox runat="server" ID="uxLongitude" CssClass="text" />
		<asp:RequiredFieldValidator runat="server" ID="uxLongitudeReqFVal" ControlToValidate="uxLongitude" ErrorMessage="Longitude is required." />
		<asp:RangeValidator runat="server" ID="uxLongitudeRangeVal" ControlToValidate="uxLongitude" Type="Double" MinimumValue="-180" MaximumValue="180" ErrorMessage="Longitude must be a decimal between -180 and 180." />
	</div>
</asp:PlaceHolder>
<% if (!ReadOnly && AutoCalculateCoordinates && ShowLatAndLong)
   { %>
<script type="text/javascript" src="//maps.google.com/maps/api/js?sensor=false"></script>
<script type="text/javascript">
	//<![CDATA[
	var geocoder = new google.maps.Geocoder();
	function GeocodeAddress() {
		var address;
		address = $.trim(($("#<%=uxAddress.ClientID%>").val() + " " + $("#<%=uxCity.ClientID%>").val() + " " + $("#<%= uxStateID.ClientID %> option:selected").text() + " " + $("#<%=uxZip.ClientID%>_uxZipcode").val()).replace(/null/g, ""));
		if (address != "") {
			geocoder.geocode({ 'address': address }, function (results, status) {
				if (status == google.maps.GeocoderStatus.OK) {
					$("#<%=uxLatitude.ClientID%>").val(results[0].geometry.location.lat());
					$("#<%=uxLongitude.ClientID%>").val(results[0].geometry.location.lng());
				}
			});
		}
	}

	$(document).ready(function () {
		$("#<%=uxAddress.ClientID%>, #<%=uxCity.ClientID%>, #<%= uxStateID.ClientID %>, #<%=uxZip.ClientID%>_uxZipcode").change(function () {
			GeocodeAddress();
		});
	});
	//]]>
</script>
<%} %>