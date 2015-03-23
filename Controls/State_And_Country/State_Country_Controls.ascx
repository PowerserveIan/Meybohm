<%@ Control Language="C#" AutoEventWireup="true" CodeFile="State_Country_Controls.ascx.cs" Inherits="Controls_State_And_Country_State_Country_Controls" ViewStateMode="Enabled" %>
<div class="formHalf" runat="server" id="countrydropdowndiv">
	<label for='<%=uxCountry.ClientID%>'>
		Country<asp:Literal ID="uxCountyRequired" runat="server" Text="<span class='asterisk'>*</span>" />
	</label>
	<asp:DropDownList runat="server" ID="uxCountry" AppendDataBoundItems="true" CssClass="country">
		<asp:ListItem Text="--Select One--" Value=""></asp:ListItem>
	</asp:DropDownList>
	<asp:CustomValidator runat="server" ID="uxCountryCV" ErrorMessage="Country is required." OnServerValidate="cvCountry_ServerValidate" ControlToValidate="uxCountry" ValidateEmptyText="true" />
</div>
<div class="formHalf" id="statedropdowndiv" runat="server">
	<label for='<%=uxState.ClientID%>'>
		State<asp:Literal ID="uxStateRequired" runat="server" Text="<span class='asterisk'>*</span>" /></label>
	<select runat="server" id="uxState">
		<option value="">--Select One--</option>
	</select>
	<asp:CustomValidator runat="server" ID="uxStateCV" ErrorMessage="State is required." OnServerValidate="cvState_ServerValidate" ControlToValidate="uxState" ValidateEmptyText="true" />
</div>
<div class="formHalf" id="stateotherdiv" runat="server">
	<label for='<%=uxStateOther.ClientID%>'>
		State/Province (other):</label>
	<asp:TextBox runat="server" ID="uxStateOther" CssClass="text" MaxLength="50" />
</div>
<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/State_Country.js" />
<script type="text/javascript">
	// <![CDATA[
	var scworking = false;
	$(document).ready(function() {
		RunAtStartup_<%= ClientID %>();

		$("#<%= uxCountry.ClientID %>").change(function(){
			Country_Change(this, "<%= uxState.ClientID %>", "<%= statedropdowndiv.ClientID %>", "<%= uxStateOther.ClientID %>", "<%= stateotherdiv.ClientID %>", "<%= UseShipTo.ToString().ToLower() %>");
			//Toggle International Number checkboxes
			var internationalNumbers = $(this).parents("div.grouping").length > 0 ? $(this).parents("div.grouping").find("[id$=uxInternationalNumber]") : $("[id$=uxInternationalNumber]");
			internationalNumbers.each(function(){
				var checked = $(this).is(":checked");
				if (checked != parseInt($("#<%= uxCountry.ClientID %>").val()) > 1)
				{
					$(this).attr("checked", !checked);
					eval("ToggleMask_" + $(this).attr("id").replace("_uxInternationalNumber", "") + "()");
				}
			});
		});

		if ($("#<%= uxCountry.ClientID %>").val() != "" && $("#<%= uxState.ClientID %>").val() == "")
			$("#<%= uxCountry.ClientID %>").trigger("change");
	});

	function RunAtStartup_<%= ClientID %>()
	{
		if ($("#<%=uxCountry.ClientID%>").find("option:selected").text().toLowerCase() == "united states" || $("#<%=uxCountry.ClientID%>").find("option:selected").text().toLowerCase() == "canada") {
			$("#<%=stateotherdiv.ClientID%>").hide();
			$("#<%=statedropdowndiv.ClientID%>").show();
		}
		else if ($("#<%=uxCountry.ClientID%>").val() == "") {
			$("#<%=stateotherdiv.ClientID%>").hide();
			$("#<%=statedropdowndiv.ClientID%>").show();
		}
		else {
			$("#<%=stateotherdiv.ClientID%>").show();
			$("#<%=statedropdowndiv.ClientID%>").hide();
		}
}

function State<%= ClientID %>_ClientValidate(sender, args){
		args.IsValid = ValidateStateSelection($('#<%= uxState.ClientID%>'), $('#<%=uxStateOther.ClientID %>'), $('#<%=uxCountry.ClientID %>'));
	}

	function Country<%= ClientID %>_ClientValidate(sender, args){
		args.IsValid = ValidateCountrySelection($('#<%=uxCountry.ClientID %>'));
	}
	// ]]>
</script>
