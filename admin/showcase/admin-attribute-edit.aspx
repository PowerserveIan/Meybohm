<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-attribute-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminShowcaseAttributeEdit" Title="Admin - Attribute Add/Edit" %>

<%@ Import Namespace="Classes.Showcase" %>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificCSS">
	<link runat="server" type="text/css" media="screen, projection" rel="stylesheet" href="~/css/showcase.css" id="uxCSSFiles" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<ul class="inputList checkboxes horizontal">
				<li>
					<asp:CheckBox runat="server" ID="uxNumeric" Text="Is Numeric<span class='tooltip'><span>If you select Numeric, you must select slider or range slider from the filter type dropdown.</span></span>" /></li>
				<li>
					<asp:CheckBox runat="server" ID="uxActive" Checked="true" Text="Is Active" /></li>
				<li id="singleValueLI" style="display: none;">
					<asp:CheckBox runat="server" ID="uxSingleItemValue" Text="Only allow the admin to choose a single value per property<span>(ex. one school, neighborhood, etc.)</span>" /></li>
			</ul>
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<div class="formHalf">
				<label for="<%=uxTitle.ClientID%>">
					Title<span class="asterisk">*</span>
				</label>
				<asp:TextBox CssClass="text" ID="uxTitle" MaxLength="50" runat="server" />
				<asp:RegularExpressionValidator runat="server" ID="uxTitleRegexVal" ControlToValidate="uxTitle" ErrorMessage="Title is too long.  It must be 50 characters or less." ValidationExpression="^[\s\S]{0,50}$" />
				<asp:RequiredFieldValidator runat="server" ID="uxTitleReqFVal" ControlToValidate="uxTitle" ErrorMessage="Title is required." />
				<asp:CustomValidator ID="uxTitleUniqueValidator" runat="server" ControlToValidate="uxTitle" ErrorMessage="Title is already in use, please choose another." />
			</div>
			<asp:PlaceHolder runat="server" ID="uxMLSNamePH" Visible="false">
				<div class="formHalf">
					<span class="label">MLS Name</span>
					<asp:TextBox CssClass="text" ID="uxMLSAttributeName" runat="server" ReadOnly="true" />
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="uxFilterPlaceHolder">
				<div class="formHalf" id="minDiv">
					<label for="<%=uxMinimumValue.ClientID%>">
						Minimum Value Displayed on Filter
					</label>
					<asp:TextBox CssClass="text" ID="uxMinimumValue" MaxLength="11" runat="server" />
					<asp:CompareValidator runat="server" ID="uxMinimumValueCMV" ControlToValidate="uxMinimumValue" Operator="DataTypeCheck" Type="Double" ErrorMessage="Minimum Value Displayed on Filter must be an integer or decimal number." />
				</div>
				<div class="formHalf" id="maxDiv">
					<label for="<%=uxMaximumValue.ClientID%>">
						Maximum Value Displayed on Filter
					</label>
					<asp:TextBox CssClass="text" ID="uxMaximumValue" MaxLength="11" runat="server" />
					<asp:CompareValidator runat="server" ID="uxMaximumValueCMV" ControlToValidate="uxMaximumValue" Operator="DataTypeCheck" Type="Double" ErrorMessage="Maximum Value Displayed on Filter must be an integer or decimal number." />
				</div>
				<div class="formWhole">
					<label for="<%=uxShowcaseFilterID.ClientID%>">
						Filter Type<span class="asterisk">*</span><span class="tooltip"><span>The values shown here are for display purposes only, they are not the actual values of your attribute</span></span>
					</label>
					<asp:DropDownList runat="server" ID="uxShowcaseFilterID" AppendDataBoundItems="true" CssClass="dynamic">
						<asp:ListItem Text="--Select a Filter Type--" Value="0"></asp:ListItem>
					</asp:DropDownList>
					<asp:RequiredFieldValidator runat="server" ID="uxShowcaseFilterIDRFV" ControlToValidate="uxShowcaseFilterID" ErrorMessage="Filter Type is required." InitialValue="0" />
					<asp:CustomValidator runat="server" ID="uxShowcaseFilterNumericVal" ErrorMessage="A numeric filter must be selected if numeric is checked." />
					<asp:CustomValidator runat="server" ID="uxShowcaseDistanceVal" ErrorMessage="The Distance filter may only be used with numeric attributes." />
					<asp:CustomValidator runat="server" ID="uxShowcaseOnlyOneDistanceVal" ErrorMessage="You have already added a distance filter for this Showcase." />
					<asp:CustomValidator runat="server" ID="uxShowcaseRangeSliderVal" ErrorMessage="The Range Slider may only be used with numeric attributes." />
					<div class="clear"></div>
					<br />
					<div id="radioButtonDiv" style="display: none;" class="labelReset">
						<asp:RadioButtonList RepeatLayout="UnorderedList" runat="server" ID="uxRBL">
							<asp:ListItem Text="Value 1"></asp:ListItem>
							<asp:ListItem Text="Value 2"></asp:ListItem>
							<asp:ListItem Text="Value 3"></asp:ListItem>
						</asp:RadioButtonList>
					</div>
					<div id="radioButtonGridDiv" style="display: none;" class="labelReset">
						<h2>Edit Header Columns</h2>
						<div class="wrapperLong">
							<ul class="filterTitle">
								<li>
									<asp:TextBox runat="server" ID="uxHeaderYes" CssClass="text_sm" Text="Yes" ToolTip="Yes" MaxLength="10" />
									<asp:RequiredFieldValidator runat="server" ID="uxHeaderYesRFV" ControlToValidate="uxHeaderYes" ErrorMessage="Header 'Yes' is required." /></li>
								<li>
									<asp:TextBox runat="server" ID="uxHeaderNo" CssClass="text_sm" Text="No" ToolTip="No" MaxLength="10" />
									<asp:RequiredFieldValidator runat="server" ID="uxHeaderNoRFV" ControlToValidate="uxHeaderNo" ErrorMessage="Header 'No' is required." />
								</li>
								<li>
									<asp:TextBox runat="server" ID="uxHeaderNoPreference" Text="No Preference" ToolTip="No Preference" MaxLength="19" />
									<asp:RequiredFieldValidator runat="server" ID="uxHeaderNoPreferenceRFV" ControlToValidate="uxHeaderNoPreference" ErrorMessage="Header 'No Preference' is required." />
								</li>
							</ul>
							<div class="clear"></div>
							<asp:Repeater runat="server" ID="uxRadioButtonGrid">
								<ItemTemplate>
									<span class="attributeHalf">
										<%# Container.DataItem.ToString() %></span>
									<asp:RadioButtonList runat="server" ID="uxRadioButtons" CssClass="filterHalf" RepeatLayout="UnorderedList" DataSource='<%# new [] {"Yes", "No", "No Preference"} %>'>
									</asp:RadioButtonList>
								</ItemTemplate>
							</asp:Repeater>
						</div>
					</div>
					<div id="checkBoxDiv" style="display: none;" class="labelReset">
						<asp:CheckBoxList runat="server" ID="uxCBL">
							<asp:ListItem Text="Value 1"></asp:ListItem>
							<asp:ListItem Text="Value 2"></asp:ListItem>
							<asp:ListItem Text="Value 3"></asp:ListItem>
						</asp:CheckBoxList>
					</div>
					<div id="listBoxDiv" style="display: none;">
						<asp:ListBox runat="server" ID="uxListBox" SelectionMode="Multiple">
							<asp:ListItem Text="Value 1"></asp:ListItem>
							<asp:ListItem Text="Value 2"></asp:ListItem>
							<asp:ListItem Text="Value 3"></asp:ListItem>
						</asp:ListBox>
					</div>
					<div id="dropdownDiv" style="display: none;">
						<asp:DropDownList CssClass="text" runat="server" ID="uxDDL">
							<asp:ListItem Text="Value 1"></asp:ListItem>
							<asp:ListItem Text="Value 2"></asp:ListItem>
							<asp:ListItem Text="Value 3"></asp:ListItem>
						</asp:DropDownList>
					</div>
					<div id="distanceDiv" style="display: none;">
						<asp:TextBox runat="server" ID="uxAddress" Text="Address" CssClass="text" />
					</div>
					<div class="sliderWrapper" id="sliderDiv" style="display: none; width: 350px">
						<select id="slider">
							<option value="None">None</option>
							<option value="Value 1">Value 1</option>
							<option value="Value 2">Value 2</option>
							<option value="Value 3">Value 3</option>
							<option value="Value 4">Value 4</option>
						</select>
					</div>
					<div class="sliderWrapper rangeWrapper" id="rangeSliderDiv" style="display: none; width: 350px">
						<asp:DropDownList runat="server" ID="uxRangeSlider1">
						</asp:DropDownList>
						<asp:DropDownList runat="server" ID="uxRangeSlider2">
						</asp:DropDownList>
						<span class="rangeAmount" id="rangeAmount">0 - 100</span>
					</div>
					<div id="rangeTextDiv" style="display: none;" class="formWhole">
						<asp:TextBox runat="server" CssClass="text floatLeft numbersOnly" Placeholder="Min" Width="40%" MaxLength="11" /><span class="floatLeft"> - </span>
						<asp:TextBox runat="server" CssClass="text floatLeft numbersOnly" Placeholder="Max" Width="40%" MaxLength="11" />
						<div class="clear"></div>
					</div>
				</div>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="uxValuesPlaceHolder" Visible="false">
				<div class="formWhole">
					<asp:HyperLink runat="server" ID="uxEditValues" Text="<span>Edit the values for this attribute</span>" CssClass="button edit"></asp:HyperLink>
				</div>
			</asp:PlaceHolder>
		</div>
		<div class="clear"></div>
		<!-- button container -->
		<div class="buttons">
			<%--The markup for the buttons is in the BaseEditPage--%>
			<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
		</div>
	</asp:Panel>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<asp:Literal runat="server" ID="uxJavaScripts" Text="~/tft-js/core/selectToUISlider.jQuery.js"></asp:Literal>
	<script type="text/javascript">
		//<![CDATA[	
		function showFilter(filterID) {
			$("#minDiv,#maxDiv").hide();
			switch (filterID){
				case "<%= (int)FilterTypes.RadioButtonList %>":
					$("#radioButtonDiv").show();
					$("#singleValueLI").show();
					break;
				case "<%= (int)FilterTypes.RadioButtonGrid %>":
					$("#radioButtonGridDiv").show();
					ValidatorEnable($("#<%= uxHeaderNoPreferenceRFV.ClientID %>")[0], true);
					ValidatorEnable($("#<%= uxHeaderNoRFV.ClientID %>")[0], true);
					ValidatorEnable($("#<%= uxHeaderYesRFV.ClientID %>")[0], true);
					break;
				case "<%= (int)FilterTypes.CheckBoxList %>":
					$("#checkBoxDiv").show();
					$("#singleValueLI").show();
					break;
				case "<%= (int)FilterTypes.ListBox %>":
					$("#listBoxDiv").show();
					$("#singleValueLI").show();
					break;
				case "<%= (int)FilterTypes.DropDown %>":
					$("#dropdownDiv").show();
					$("#singleValueLI").show();
					break;
				case "<%= (int)FilterTypes.Slider %>":
					$("#sliderDiv").show();
					$("#minDiv,#maxDiv").show();
					$("#singleValueLI").show();
					break;
				case "<%= (int)FilterTypes.RangeSlider %>":
					$("#rangeSliderDiv").show();
					$("#minDiv,#maxDiv").show();
					break;
				case "<%= (int)FilterTypes.RangeTextBoxes %>":
					$("#rangeTextDiv").show();
					break;
				case "<%= (int)FilterTypes.Distance %>":
					$("#sliderDiv").show();
					$("#distanceDiv").show();
					break;
				case "<%= (int)FilterTypes.DistanceRange %>":
					$("#rangeSliderDiv").show();
					$("#distanceDiv").show();
					break;
			}
		}

		$(document).ready(function() {
			$("#sliderDiv select").selectToUISlider({labels: 5, tooltip: false}).hide();
			$("#rangeSliderDiv select").selectToUISlider({labels: 5, customSlideFunction: "rangeSliderSlide", tooltip: false}).hide();
			showFilter($("#<%=uxShowcaseFilterID.ClientID%>").val());
			$("#<%=uxShowcaseFilterID.ClientID%>").change(function() {
				$("#radioButtonDiv").hide();
				$("#radioButtonGridDiv").hide();
				$("#checkBoxDiv").hide();
				$("#listBoxDiv").hide();
				$("#dropdownDiv").hide();
				$("#sliderDiv").hide();
				$("#rangeSliderDiv").hide();
				$("#rangeTextDiv").hide();
				$("#distanceDiv").hide();
				$("#singleValueLI").hide();

				ValidatorEnable($("#<%= uxHeaderNoPreferenceRFV.ClientID %>")[0], false);
				ValidatorEnable($("#<%= uxHeaderNoRFV.ClientID %>")[0], false);
				ValidatorEnable($("#<%= uxHeaderYesRFV.ClientID %>")[0], false);

				showFilter($(this).val());
			});

			function showHideFilters(checked) {
				if (checked) {
					$("#<%=uxShowcaseFilterID.ClientID%> option").each(function() {
						if ($(this).val() == "0" || $(this).text() == "Slider" || $(this).text() == "Range Slider" || $(this).text() == "Distance" || $(this).text() == "Distance Range" || $(this).text() == "Range Text Boxes")
							$(this).show();
						else
							$(this).hide();
					});
				}
				else {
					$("#<%=uxShowcaseFilterID.ClientID%> option").each(function() {
						if ($(this).text() == "Range Slider" || $(this).text() == "Distance" || $(this).text() == "Distance Range" || $(this).text() == "Range Text Boxes")
							$(this).hide();
						else
							$(this).show();
					});
					$("#minDiv,#maxDiv").hide();
				}
			}

			$("#<%=uxNumeric.ClientID%>").click(function() {
				showHideFilters($(this).attr('checked'));
				if (<%=(!NewRecord).ToString().ToLower()%> && $(this).attr('checked'))
					alert("Converting this attribute to Numeric will wipe out all existing values");
			});
			showHideFilters($("#<%=uxNumeric.ClientID%>").attr('checked'));
			
			$(".numbersOnly").keyup(function () {
				if ($(this).val() != '')
					$(this).val(parseFloat($(this).val().replace(/,/g, '')).toLocaleString());
			});
		});

		function rangeSliderSlide(e, ui) {
			var firstHandle = $(ui.handle).prev().length > 0 ? $(ui.handle).prev() : $(ui.handle);
			$("#rangeAmount").html(firstHandle.attr("aria-valuetext") + ' - ' + firstHandle.next().attr("aria-valuetext"));
		}
		//]]>
	</script>
</asp:Content>
