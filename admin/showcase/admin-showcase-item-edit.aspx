<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-showcase-item-edit.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Inherits="Admin_AdminShowcaseItemEdit" Title="Admin - Showcase Item Add/Edit" %>

<%@ Import Namespace="Classes.Showcase" %>
<%@ Register TagPrefix="Controls" TagName="DateTimePicker" Src="~/Controls/BaseControls/DateTimePicker.ascx" %>
<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="PhoneBox" Src="~/Controls/BaseControls/PhoneBoxControl.ascx" %>
<%@ Register TagPrefix="SEOComponent" TagName="CurrentPageSEO" Src="~/Controls/SEOComponent/SEO_Data_Entry.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Address" Src="~/Controls/State_And_Country/Address.ascx" %>
<%@ Register TagPrefix="Showcase" TagName="AdminSellNewHome" Src="~/Controls/Showcase/AdminSellNewHome.ascx" %>
<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
	<%--The markup for the Header (title, breadcrumbs, validation and success) can be found in the BaseEditPage--%>
	<asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
		<div class="blue padded optionsList">
			<ul class="inputList checkboxes horizontal">
				<li>
					<asp:CheckBox runat="server" ID="uxActive" Text="Is Active" Checked="true" /></li>
				<li runat="server" id="uxRentedLI">
					<asp:CheckBox runat="server" ID="uxRented" Text="Is Rented<span class='tooltip'><span>If checked, this property will no longer show up on the frontend</span></span>" /></li>
				<li>
					<asp:CheckBox runat="server" ID="uxFeatured" Text="Is Featured" /></li>
			</ul>
			<div class="clear"></div>
		</div>
		<div class="formWrapper">
			<asp:Literal runat="server" ID="uxMLSData" Visible="false"><h3 class="warning">This information comes from the RETS feed.  Your license agreement prevents you from changing the data.  If you've added custom attributes, you will be able to modify the values below.</h3></asp:Literal>
			<asp:PlaceHolder runat="server" ID="uxMLSIDPH">
				<div class="formHalf">
					<span class="label">MLS ID #</span>
					<asp:Literal runat="server" ID="uxMLSID"></asp:Literal>
				</div>
			</asp:PlaceHolder>
			<div class="formHalf">
				<label for="<%=uxTitle.ClientID%>">
					Title<span class="asterisk">*</span>
				</label>
				<asp:TextBox CssClass="text" runat="server" ID="uxTitle" MaxLength="500" />
				<asp:RegularExpressionValidator runat="server" ID="uxTitleRegexVal" ControlToValidate="uxTitle" ErrorMessage="Title is too long.  It must be 500 characters or less." ValidationExpression="^[\s\S]{0,500}$" />
				<asp:RequiredFieldValidator runat="server" ID="uxTitleReqFVal" ControlToValidate="uxTitle" ErrorMessage="Title is required." />
			</div>
			<asp:PlaceHolder runat="server" ID="uxAvailabilityDatePH">
				<div class="formHalf">
					<label for="<%=uxAvailabilityDate.ClientID%>_uxDate">
						Availability Date
					</label>
					<Controls:DateTimePicker runat="server" ID="uxAvailabilityDate" TextBoxCssClass="text" />
				</div>
			</asp:PlaceHolder>
			<div class="clear"></div>
			<div class="formHalf">
				<label for="<%= uxNeighborhoodID.ClientID %>">
					Neighborhood</label>
				<asp:DropDownList runat="server" ID="uxNeighborhoodID" AppendDataBoundItems="true">
					<asp:ListItem Text="--Select a Neighborhood--" Value=""></asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="formHalf">
				<label for="<%= uxAgentID.ClientID %>">
					Agent</label>
				<asp:DropDownList runat="server" ID="uxAgentID" AppendDataBoundItems="true">
					<asp:ListItem Text="--Select an Agent--" Value=""></asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="formHalf">
				<label for="<%= uxOfficeID.ClientID %>">
					Office</label>
				<asp:DropDownList runat="server" ID="uxOfficeID" AppendDataBoundItems="true">
					<asp:ListItem Text="--Select an Office--" Value=""></asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="formWhole">
				<label for="<%=uxSummary.ClientID%>">
					Summary<span class="asterisk">*</span>
				</label>
				<asp:TextBox runat="server" ID="uxSummary" TextMode="MultiLine" CssClass="text" />
				<asp:RequiredFieldValidator runat="server" ID="uxSummaryReqFVal" ControlToValidate="uxSummary" ErrorMessage="Summary is required." />
			</div>
			<div class="formWhole"><span class="label">Image<span class="asterisk">*</span><span class="tooltip"> <span>This will display on the listing page.<br />
				<br />
				Must be of type .gif, .jpg, .jpeg, .png<br />
				<br />
				Suggested resizing tool:
				<a href="http://faststone.org/FSViewerDetail.htm" target="_blank">FastStone</a></span> </span><span>Optimal image size is 290x167</span></span>
				<Controls:FileUpload runat="server" ID="uxImage" ImageWidth="290" ImageHeight="167" AllowedFileTypes=".gif,.jpg,.jpeg,.png" RequiredErrorMessage="Image is required." Required="true" AllowExternalImageLink="true" />
			</div>
			<div class="formHalf">
				<label for="<%= uxWebsite.ClientID %>">
					Website</label>
				<asp:TextBox runat="server" ID="uxWebsite" MaxLength="1000" CssClass="text" />
				<asp:RegularExpressionValidator runat="server" ID="uxWebsiteRegexVal" ControlToValidate="uxWebsite" ErrorMessage="Website is too long.  It must be 1000 characters or less." ValidationExpression="^[\s\S]{0,1000}$" />
			</div>
			<div class="formHalf">
				<label for="<%= uxVirtualTourURL.ClientID %>">
					Virtual Tour URL</label>
				<asp:TextBox runat="server" ID="uxVirtualTourURL" MaxLength="1000" CssClass="text" />
				<asp:RegularExpressionValidator runat="server" ID="uxVirtualTourURLRegexVal" ControlToValidate="uxVirtualTourURL" ErrorMessage="Virtual Tour URL is too long.  It must be 1000 characters or less." ValidationExpression="^[\s\S]{0,1000}$" />
			</div>
			<div class="formHalf">
				<label for="<%= uxDirections.ClientID %>">
					Directions</label>
				<asp:TextBox runat="server" ID="uxDirections" MaxLength="2000" TextMode="MultiLine" CssClass="text" />
				<asp:RegularExpressionValidator runat="server" ID="uxDirectionsRegexVal" ControlToValidate="uxDirections" ErrorMessage="Directions are too long.  They must be 2000 characters or less." ValidationExpression="^[\s\S]{0,2000}$" />
			</div>
			<div class="formHalf">
				<span class="label">Send Stats To</span>
				<asp:RadioButtonList runat="server" ID="uxSendStatsTo">
					<asp:ListItem Text="Agent and Owner" Value="Both"></asp:ListItem>
					<asp:ListItem Text="Agent Only" Value="Agent"></asp:ListItem>
					<asp:ListItem Text="Owner Only" Value="Owner"></asp:ListItem>
				</asp:RadioButtonList>
			</div>
			<div class="formHalf">
				<label for="<%=uxEmailAddresses.ClientID%>">
					Email Addresses<span>Separate email addresses with a semi-colon</span>
				</label>
				<asp:TextBox CssClass="text" runat="server" ID="uxEmailAddresses" TextMode="MultiLine" />
			</div>
			<asp:PlaceHolder runat="server" ID="uxCollectionPlaceHolder" Visible="false">
				<div class="formWhole">
					<asp:HyperLink runat="server" ID="uxEditCollection" Text="<span>Edit the media collections for this showcase item</span>" CssClass="button edit"></asp:HyperLink>
				</div>
				<div class="formWhole">
					<asp:HyperLink runat="server" ID="uxEditPropertyStatus" Text="<span>Edit the status for this showcase item</span>" CssClass="button edit"></asp:HyperLink>
				</div>
			</asp:PlaceHolder>
		</div>
		<div class="clear"></div>
		<asp:PlaceHolder runat="server" ID="uxLocationPlaceHolder">
			<div class="sectionTitle">
				<div class="bottom">
					<h2>Address Information<span class="asterisk">*</span></h2>
				</div>
			</div>
			<div class="formWrapper">
				<Controls:Address runat="server" ID="uxAddress" Required="true" AutoCalculateCoordinates="true" />
			</div>
			<div class="rightCol indent">
				<div class="helpBubble">
					<div class="top">
						<div class="bottom">
							<h5>This is the address that will show up on the Google Map.</h5>
						</div>
					</div>
				</div>
			</div>
			<div class="clear"></div>
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="uxNewHomeSoldPlaceHolder">
			<div class="sectionTitle">
				<div class="bottom">
					<h2>New Home Sold Information</h2>
				</div>
			</div>
			<div class="blue padded optionsList">
				<asp:CheckBox runat="server" ID="uxSoldHomeIsSold" Text="Is Sold?" />
				<div class="clear"></div>
			</div>
			<div class="formWrapper" id="soldHomeContainer">
				<Showcase:AdminSellNewHome runat="server" ID="uxSoldHomeControl" />
			</div>
			<div class="clear"></div>
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="uxOpenHousePlaceHolder">
			<div class="sectionTitle" id="openHouses">
				<div class="bottom">
					<h2>Open House Information</h2>
				</div>
			</div>
			<div class="blue padded optionsList">
				<asp:CheckBox runat="server" ID="uxRecurring" Text="Is Recurring?" />
				<div class="clear"></div>
			</div>
			<div class="formWrapper fullWidth">
				<div class="formHalf">
					<label for="<%= uxOpenHouseAgentID.ClientID %>">
						Showing Agent</label>
					<asp:DropDownList runat="server" ID="uxOpenHouseAgentID" AppendDataBoundItems="true">
						<asp:ListItem Text="--Select an Agent--" Value=""></asp:ListItem>
					</asp:DropDownList>
				</div>
				<div class="clear"></div>
				<div id="nonRecurring">
					<div class="formHalf">
						<label for="<%= uxBeginDate.ClientID + "_uxDate" %>">
							Begin Date / Time</label>
						<Controls:DateTimePicker runat="server" ID="uxBeginDate" PickerStyle="DateTime" TextBoxCssClass="text" />
					</div>
					<div class="formHalf">
						<label for="<%= uxEndDate.ClientID + "_uxDate" %>">
							End Date / Time</label>
						<Controls:DateTimePicker runat="server" ID="uxEndDate" PickerStyle="DateTime" TextBoxCssClass="text" />
						<asp:CustomValidator runat="server" ID="uxDateCustomVal" OnServerValidate="DateValidate" Display="None" ErrorMessage="End date must be after start date." ClientValidationFunction="ValidateDates" ValidateEmptyText="true" />
					</div>
				</div>
				<div id="recurring">
					<div class="formWhole">
						<asp:ValidationSummary runat="server" ID="uxRecurringValSum" CssClass="validation" ValidationGroup="Recurring" DisplayMode="BulletList" ForeColor="Red" />
						<asp:RequiredFieldValidator runat="server" ID="uxRecurrencePatternRFV" ControlToValidate="uxRecurrencePattern" Display="None" ErrorMessage="You must select either Weekly or Monthly." ValidationGroup="Recurring" />
						<asp:RequiredFieldValidator runat="server" ID="uxNumWeeksRFV" ControlToValidate="uxNumWeeks" Display="None" ErrorMessage="You must specify the weekly recurrence pattern." ValidationGroup="Recurring" />
						<asp:CustomValidator runat="server" ID="uxDaysOfWeekRFV" Display="None" ErrorMessage="You must pick at least one day of the week." ValidationGroup="Recurring" ClientValidationFunction="ValidateDaysOfWeek" />
						<asp:RequiredFieldValidator runat="server" ID="uxNumMonthsRFV" ControlToValidate="uxNumMonths" Display="None" ErrorMessage="You must specify the monthly recurrence pattern." ValidationGroup="Recurring" />
						<asp:RadioButtonList runat="server" ID="uxRecurrencePattern" CssClass="inputList radiobuttons" RepeatLayout="UnorderedList">
							<asp:ListItem Text="Weekly" Selected="True"></asp:ListItem>
							<asp:ListItem Text="Monthly"></asp:ListItem>
						</asp:RadioButtonList>
					</div>
					<div class="formWhole" id="weekly"><span>Recur every
					<asp:TextBox runat="server" ID="uxNumWeeks" CssClass="text small" MaxLength="2" Text="1" />
						week(s) on:</span>
						<asp:CheckBoxList runat="server" ID="uxDaysOfWeek" CssClass="inputList checkboxes" RepeatLayout="UnorderedList">
							<asp:ListItem Text="M" Value="Monday"></asp:ListItem>
							<asp:ListItem Text="T" Value="Tuesday"></asp:ListItem>
							<asp:ListItem Text="W" Value="Wednesday"></asp:ListItem>
							<asp:ListItem Text="Th" Value="Thursday"></asp:ListItem>
							<asp:ListItem Text="F" Value="Friday"></asp:ListItem>
							<asp:ListItem Text="Sa" Value="Saturday"></asp:ListItem>
							<asp:ListItem Text="Su" Value="Sunday"></asp:ListItem>
						</asp:CheckBoxList>
					</div>
					<div class="formWhole" id="monthly" style="display: none;"><span>The
					<asp:DropDownList runat="server" ID="uxWeekNumber" CssClass="dynamic">
						<asp:ListItem Text="First" Value="1"></asp:ListItem>
						<asp:ListItem Text="Second" Value="2"></asp:ListItem>
						<asp:ListItem Text="Third" Value="3"></asp:ListItem>
						<asp:ListItem Text="Fourth" Value="4"></asp:ListItem>
						<asp:ListItem Text="Last" Value="5"></asp:ListItem>
					</asp:DropDownList>
						<asp:DropDownList runat="server" ID="uxMonthlyDayOfWeek" CssClass="dynamic">
							<asp:ListItem Text="Monday"></asp:ListItem>
							<asp:ListItem Text="Tuesday"></asp:ListItem>
							<asp:ListItem Text="Wednesday"></asp:ListItem>
							<asp:ListItem Text="Thursday"></asp:ListItem>
							<asp:ListItem Text="Friday"></asp:ListItem>
							<asp:ListItem Text="Saturday"></asp:ListItem>
							<asp:ListItem Text="Sunday"></asp:ListItem>
						</asp:DropDownList>
						of every
					<asp:TextBox runat="server" ID="uxNumMonths" CssClass="text small" MaxLength="2" Text="1" />
						month(s)</span> </div>
					<hr />
					<asp:PlaceHolder runat="server" ID="uxQuickNavPH" Visible="false">
						<div class="recurringNavigation">
							<label>
								Jump to Date:</label>
							<asp:PlaceHolder runat="server" ID="uxQuickNavYearPH">
								<div class="years">
									<a href="#" class="leftArrow disabled">&laquo;</a>
									<asp:TextBox runat="server" ID="uxQuickNavYear" CssClass="text small" MaxLength="4" />
									<a href="#" class="rightArrow">&raquo;</a>
								</div>
								<div class="clear"></div>
							</asp:PlaceHolder>
							<asp:Repeater runat="server" ID="uxQuickNavMonths">
								<HeaderTemplate>
									<div class="months">
								</HeaderTemplate>
								<ItemTemplate>
									<a href="#<%# ((KeyValuePair<string,int>)Container.DataItem).Value %>" class="year <%# ((KeyValuePair<string,int>)Container.DataItem).Key.Split('-')[1] %>">
										<%# ((KeyValuePair<string,int>)Container.DataItem).Key.Split('-')[0] %></a>
								</ItemTemplate>
								<FooterTemplate>
									</div>
								</FooterTemplate>
							</asp:Repeater>
							<div class="clear"></div>
						</div>
						<script type="text/javascript">
							$(function () {
								$("a.year").click(function () {
									var target = "tr" + $(this).attr('href');
									var $scrollTarget = $(".scroll");
									var targetPosition = $(target).get([0]).offsetTop;
									$scrollTarget.scrollTop(targetPosition);
									return false;
								});
							});
						</script>
					</asp:PlaceHolder>
					<div class="formWhole<%= uxRecurringDates.Items.Count > 12 ? " scroll" : "" %>">
						<table class="listing">
							<thead>
								<tr>
									<th style="width: 150px;">
										Action
									</th>
									<th colspan="2">
										Date / Time Range
									</th>
								</tr>
							</thead>
							<tbody>
								<tr>
									<td>
										<asp:Button runat="server" ID="uxAddRecurrence" Text="Add" CssClass="button add" OnCommand="Recurring_Command" CommandName="Add" ValidationGroup="Recurring" />
									</td>
									<td>
										<Controls:DateTimePicker runat="server" ID="uxRecurringStartDate" PickerStyle="DateTime" TextBoxCssClass="text" Required="true" RequiredErrorMessage="Recurring Start Date is required." ValidationGroup="Recurring" />
									</td>
									<td>
										<Controls:DateTimePicker runat="server" ID="uxRecurringEndDate" PickerStyle="DateTime" TextBoxCssClass="text" Required="true" RequiredErrorMessage="Recurring End Date is required." ValidationGroup="Recurring" />
										<asp:CustomValidator runat="server" ID="uxRecurringDateCustomVal" OnServerValidate="DateValidateRecurring" ErrorMessage="End date must be after start date." ClientValidationFunction="ValidateDatesRecurring" ValidateEmptyText="true" ValidationGroup="Recurring" />
									</td>
								</tr>
								<asp:Repeater runat="server" ID="uxRecurringDates" ItemType="Classes.Showcase.OpenHouse">
									<ItemTemplate>
										<tr<%# " id='" + Item.OpenHouseID + "'" %>>
											<td>
												<%--<asp:LinkButton runat="server" Text="Edit" OnCommand="Recurring_Command" CommandName="Edit" CommandArgument="<%# Item.OpenHouseID %>" CausesValidation="false" CssClass="icon edit noText" OnClientClick="return confirm('Changes made to an occurrence will cause it be created as a new event.  You will not be able to undo this.  Do you wish to continue?');" />--%>
												<asp:LinkButton CssClass="icon delete noText" runat="server" Text="Delete" OnCommand="Recurring_Command" CommandName="Delete" CommandArgument="<%# Item.OpenHouseID %>" CausesValidation="false" />
											</td>
											<td colspan="2">
												<%# Item.BeginDateClientTime.ToString("dddd MM/d/yyyy h:mm tt")%>
												<%# Item.EndDate.HasValue ? " - " + Item.EndDateClientTime.Value.ToString("h:mm tt") : "" %>
											</td>
										</tr>
									</ItemTemplate>
								</asp:Repeater>
							</tbody>
						</table>
					</div>
				</div>
			</div>
			<div class="clear"></div>
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="uxRentalPlaceHolder">
			<div class="sectionTitle">
				<div class="bottom">
					<h2>Rental Information</h2>
				</div>
			</div>
			<div class="formWrapper">
				<div class="formHalf">
					<label for="<%=uxLeaseBegins.ClientID%>_uxDate">
						Lease Begins
					</label>
					<Controls:DateTimePicker runat="server" ID="uxLeaseBegins" TextBoxCssClass="text" />
				</div>
				<div class="formWhole">
					<label for="<%=uxOwnerName.ClientID%>">
						Owner Name<span class="asterisk">*</span>
					</label>
					<asp:TextBox CssClass="text" runat="server" ID="uxOwnerName" MaxLength="500" />
					<asp:RegularExpressionValidator runat="server" ID="uxOwnerNameRegexVal" ControlToValidate="uxOwnerName" ErrorMessage="Owner Name is too long.  It must be 500 characters or less." ValidationExpression="^[\s\S]{0,500}$" />
					<asp:CustomValidator ID="uxOwnerNameCV" runat="server" ClientValidationFunction="ValidateRentalOwner" ErrorMessage="You must enter either an owner name or a company name." />
				</div>
				<div class="formWhole">
					<label for="<%=uxCompanyName.ClientID%>">
						Company Name<span class="asterisk">*</span>
					</label>
					<asp:TextBox CssClass="text" runat="server" ID="uxCompanyName" MaxLength="255" />
					<asp:RegularExpressionValidator runat="server" ID="uxCompanyNameRegexVal" ControlToValidate="uxCompanyName" ErrorMessage="Company Name is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" />
				</div>
				<div class="formHalf">
					<label for="<%=uxContactName.ClientID%>">
						Contact Name
					</label>
					<asp:TextBox CssClass="text" runat="server" ID="uxContactName" MaxLength="500" />
					<asp:RegularExpressionValidator runat="server" ID="uxContactNameRegexVal" ControlToValidate="uxContactName" ErrorMessage="Contact Name is too long.  It must be 500 characters or less." ValidationExpression="^[\s\S]{0,500}$" />
				</div>
				<div class="formHalf">
					<label for="<%=uxContactPhone.ClientID%>_uxPhone">
						Contact Phone
					</label>
					<Controls:PhoneBox runat="server" ID="uxContactPhone" Required="false" TextBoxClass="text" />
				</div>
			</div>
			<div class="clear"></div>
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="uxAttributesPlaceHolder" Visible="false">
			<div class="sectionTitle">
				<div class="bottom">
					<h2>Attributes</h2>
				</div>
			</div>
			<div class="formWrapper">
				<asp:Repeater runat="server" ID="uxAttributeRepeater" ItemType="Classes.Showcase.ShowcaseAttribute">
					<ItemTemplate>
						<asp:HiddenField runat="server" ID="uxAttributeID" Value='<%#Item.ShowcaseAttributeID%>' />
						<div class="formHalf">
							<label id="attributeTitle">
								<%#Item.Title%>
								<asp:Label runat="server" ID="uxNumericLiteral" Text="Input your numeric value for this filter" Visible='<%#Item.Numeric%>' />
							</label>
							<div class="wrapperShort">
								<asp:PlaceHolder runat="server" ID="uxNonRadioButtonGridPH" Visible="<%#Item.ShowcaseFilterID != (int)FilterTypes.RadioButtonGrid && !Item.SingleItemValue%>">
									<asp:TextBox CssClass="text small" runat="server" ID="uxAttributeValueTxt" MaxLength="50" Visible='<%#Item.Numeric%>' />
									<asp:CompareValidator runat="server" ID="uxAttributeValueTxtRngVal" ControlToValidate="uxAttributeValueTxt" Operator="LessThanEqual" ValueToCompare="100000000" Type="Double" ErrorMessage="Value must be a number less than or equal to 100000000." />
									<asp:CheckBoxList runat="server" ID="uxAttributeValues" Visible='<%#!Item.Numeric%>' DataSource='<%#ShowcaseAttributeValue.ShowcaseAttributeValueGetByShowcaseAttributeID(Item.ShowcaseAttributeID).OrderBy(v => v.DisplayOrder)%>'
										DataValueField="ShowcaseAttributeValueID" DataTextField="Value" RepeatLayout="UnorderedList" CssClass="inputList checkboxes">
									</asp:CheckBoxList>
								</asp:PlaceHolder>
								<asp:DropDownList runat="server" ID="uxSingleAttributeValue" Visible="<%# Item.SingleItemValue && !Item.Numeric && Item.ShowcaseFilterID != (int)FilterTypes.RadioButtonGrid %>" DataSource="<%# ShowcaseAttributeValue.ShowcaseAttributeValueGetByShowcaseAttributeID(Item.ShowcaseAttributeID).OrderBy(v => v.DisplayOrder) %>" DataValueField="ShowcaseAttributeValueID"
									DataTextField="Value" AppendDataBoundItems="true">
									<asp:ListItem Text="--Select One--" Value=""></asp:ListItem>
								</asp:DropDownList>
								<asp:PlaceHolder runat="server" ID="uxRadioButtonGridPH" Visible="<%#Item.ShowcaseFilterID == (int)FilterTypes.RadioButtonGrid%>">
									<asp:Repeater runat="server" ID="uxHeaders" DataSource="<%# ShowcaseAttributeHeader.ShowcaseAttributeHeaderGetByShowcaseAttributeID(Item.ShowcaseAttributeID).Where(h=>!h.NoPreferenceColumn) %>" ItemType="Classes.Showcase.ShowcaseAttributeHeader">
										<HeaderTemplate>
											<ul class="filterTitle">
										</HeaderTemplate>
										<ItemTemplate>
											<li>
												<%# Item.Text %></li>
										</ItemTemplate>
										<FooterTemplate>
											</ul>
											<div class="clear"></div>
										</FooterTemplate>
									</asp:Repeater>
									<asp:Repeater runat="server" ID="uxRadioButtonGrid" OnItemDataBound="uxRadioButtonGrid_ItemDataBound" DataSource='<%#ShowcaseAttributeValue.ShowcaseAttributeValueGetByShowcaseAttributeID(Item.ShowcaseAttributeID).OrderBy(v => v.DisplayOrder).ToList()%>' ItemType="Classes.Showcase.ShowcaseAttributeValue">
										<ItemTemplate>
											<span class="attributeHalf">
												<%# Item.Value %></span>
											<asp:RadioButtonList runat="server" ID="uxRadioButtons" CssClass="inputList radiobuttons" RepeatLayout="UnorderedList" DataTextField="Text" DataValueField="Text" DataSource="<%# ShowcaseAttributeHeader.ShowcaseAttributeHeaderGetByShowcaseAttributeID(Item.ShowcaseAttributeID).Where(h=>!h.NoPreferenceColumn) %>">
											</asp:RadioButtonList>
											<asp:HiddenField runat="server" ID="uxValueID" Value="<%# Item.ShowcaseAttributeValueID %>" />
										</ItemTemplate>
									</asp:Repeater>
								</asp:PlaceHolder>
							</div>
						</div>
					</ItemTemplate>
				</asp:Repeater>
			</div>
			<div class="clear"></div>
		</asp:PlaceHolder>
		<SEOComponent:CurrentPageSEO ID="uxSEOData" runat="server" SitePageLinkSetupType="PageFormatter" PageLinkFormatter="~/home-details?id={0}" />
		<!-- button container -->
		<div class="buttons fixedBottom">
			<%--The markup for the buttons is in the BaseEditPage--%>
			<asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
		</div>
	</asp:Panel>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageSpecificJS">
	<script type="text/javascript">
		function ValidateRentalOwner(sender, args) {
			args.IsValid = $("#<%= uxOwnerName.ClientID %>").val() != "" || $("#<%= uxCompanyName.ClientID %>").val() != "";
		}

		<% if (ShowcaseHelpers.IsCurrentShowcaseMLS())
	 { %>
		function ValidateDaysOfWeek(source, args) {
			args.IsValid = $("input[id*=uxDaysOfWeek]:checked").length > 0;
		}

		function ValidateDates(source, args) {
			var startValue = $("#<%= uxBeginDate.ClientID %>_uxDate").val();
			var endValue = $("#<%= uxEndDate.ClientID %>_uxDate").val();
			if (!isNaN(Date.parse(startValue)) && !isNaN(Date.parse(endValue)) && Date.parse(startValue) > Date.parse(endValue))
				args.IsValid = false;
		}

		function ValidateDatesRecurring(source, args) {
			var startValue = $("#<%= uxRecurringStartDate.ClientID %>_uxDate").val();
			var endValue = $("#<%= uxRecurringEndDate.ClientID %>_uxDate").val();
			if (!isNaN(Date.parse(startValue)) && !isNaN(Date.parse(endValue)) && Date.parse(startValue) > Date.parse(endValue))
				args.IsValid = false;
		}

		function ToggleRecurring() {
			if ($("#<%=uxRecurring.ClientID %>").is(":checked")) {
				$("#nonRecurring").hide();
				$("#recurring").show();
			}
			else {
				$("#nonRecurring").show();
				$("#recurring").hide();
			}
		}

		function CustomValidatorEnable(val, enable) {
			val.enabled = enable;
		}

		function ToggleSoldHomes() {
			var isSold = $("#<%=uxSoldHomeIsSold.ClientID %>").is(":checked");
			if (isSold)
				$("#soldHomeContainer").show();
			else
				$("#soldHomeContainer").hide();
			CustomValidatorEnable($("[id$=uxSoldHomeCloseDate_uxDateRFV]")[0], isSold);
			CustomValidatorEnable($("[id$=uxSoldHomeSalePriceRFV]")[0], isSold);
		}

		$(document).ready(function () {
			ToggleRecurring();
			$("#<%=uxRecurring.ClientID %>").click(function () {
				ToggleRecurring();
			});

			ToggleSoldHomes();
			$("#<%=uxSoldHomeIsSold.ClientID %>").click(function () {
				ToggleSoldHomes();
			});

			$("input[id*=uxRecurrencePattern]").click(function () {
				if ($(this).val() == "Weekly") {
					$("#weekly").show();
					$("#monthly").hide();
				} else {
					$("#weekly").hide();
					$("#monthly").show();
				}
				CustomValidatorEnable($("#<%= uxNumWeeksRFV.ClientID %>")[0], $(this).val() == "Weekly");
				CustomValidatorEnable($("#<%= uxDaysOfWeekRFV.ClientID %>")[0], $(this).val() == "Weekly");
				CustomValidatorEnable($("#<%= uxNumMonthsRFV.ClientID %>")[0], $(this).val() != "Weekly");
			});

			$("a.leftArrow").click(function () {
				if ($(this).hasClass("disabled"))
					return false;
				ChangeYear(parseInt($("#<%= uxQuickNavYear.ClientID %>").val()) - 1);
				return false;
			});

			$("a.rightArrow").click(function () {
				if ($(this).hasClass("disabled"))
					return false;
				ChangeYear(parseInt($("#<%= uxQuickNavYear.ClientID %>").val()) + 1);
				return false;
			});

			function ChangeYear(year) {
				$("#<%= uxQuickNavYear.ClientID %>").val(year);
				if ($("a.year." + (year - 1)).length == 0)
					$("a.leftArrow").addClass("disabled");
				else
					$("a.leftArrow").removeClass("disabled");
				if ($("a.year." + (year + 1)).length == 0)
					$("a.rightArrow").addClass("disabled");
				else
					$("a.rightArrow").removeClass("disabled");
				$("a.year." + year).show();
				$("a.year").not("." + year).hide();
			}

			ChangeYear(parseInt($("#<%= uxQuickNavYear.ClientID %>").val()));
		});<% } %>
	</script>
</asp:Content>
