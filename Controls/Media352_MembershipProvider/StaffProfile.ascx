<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StaffProfile.ascx.cs" Inherits="Controls_Media352_MembershipProvider_StaffProfile" %>
<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="PhoneBox" Src="~/Controls/BaseControls/PhoneBoxControl.ascx" %>
<asp:PlaceHolder runat="server" ID="uxPermissionsPH">
	<div class="blue padded optionsList">
		<ul class="inputList checkboxes horizontal">
			<li>
				<asp:CheckBox runat="server" ID="uxShowListingLink" Text="Show Agent Listing Link" /></li>
			<li>
				<asp:CheckBox runat="server" ID="uxDisplayInDirectory" Text="Show in Staff/Agent Directory" /></li>
		</ul>
		<div class="clear"></div>
	</div>
</asp:PlaceHolder>
<div class="formWrapper">
	<div class="formHalf">
		<label for="<%=uxStaffType.ClientID%>">
			Category<em>*</em></label>
		<asp:DropDownList runat="server" ID="uxStaffType" AppendDataBoundItems="true">
			<asp:ListItem Text="--Select a Category--" Value=""></asp:ListItem>
		</asp:DropDownList>
		<asp:RequiredFieldValidator runat="server" ID="uxStaffTypeRFV" ControlToValidate="uxStaffType" ErrorMessage="Category is required." InitialValue="" />
	</div>
	<div class="formHalf">
		<label for="<%=uxJobTitle.ClientID%>">
			Job Title</label>
		<asp:DropDownList runat="server" ID="uxJobTitle" AppendDataBoundItems="true">
			<asp:ListItem Text="--Select a Job Title--" Value=""></asp:ListItem>
		</asp:DropDownList>
	</div>
	<div class="clear"></div>
	<div class="formHalf">
		<span class="label">Additional Languages Spoken<br />
			<em>(all Agents speak English)</em></span>
		<asp:CheckBoxList runat="server" ID="uxLanguages" CssClass="inputList checkboxes"></asp:CheckBoxList>
	</div>
	<div class="formHalf">
		<span class="label">Designations</span>
		<asp:CheckBoxList runat="server" ID="uxDesignations" CssClass="inputList checkboxes"></asp:CheckBoxList>
	</div>
	<div class="clear"></div>
	<div class="formHalf">
		<label for="<%= uxWebsite.ClientID %>">
			Personal Website</label>
		<asp:TextBox runat="server" ID="uxWebsite" MaxLength="1000" CssClass="text" />
		<asp:RegularExpressionValidator runat="server" ID="uxWebsiteRegexVal" ControlToValidate="uxWebsite" ErrorMessage="Personal Website is too long.  It must be 1000 characters or less." ValidationExpression="^[\s\S]{0,1000}$" />
	</div>
	<div class="formWhole">
		<label for="<%=uxRating.ClientID%>" class="block">
			Customer Survey Rating (percentage)</label>
		<asp:TextBox ID="uxRating" runat="server" CssClass="text integer" MaxLength="5" /><span>%</span>
		<asp:CheckBox runat="server" ID="uxShowRatingOnSite" Text="Show Rating on Site" />
		<asp:RangeValidator runat="server" ID="uxRatingRangeVal" ControlToValidate="uxRating" Type="Double" MinimumValue="0" MaximumValue="100" ErrorMessage="Customer Survey Rating must be a percentage between 0 and 100." />
	</div>
	<div class="formWhole">
		<label for="<%= uxPhoto.ClientID %>">
			Staff Photo</label>
		<Controls:FileUpload runat="server" ID="uxPhoto" AllowedFileTypes=".gif,.jpg,.jpeg,.png" ImageHeight="134" ImageWidth="102" Required="False" AllowExternalImageLink="true" UploadToLocation="~/uploads/agents" />
	</div>
	<div class="formWhole">
		<label for="<%= uxBiography.ClientID %>">
			Biography</label>
		<asp:TextBox runat="server" ID="uxBiography" MaxLength="500" TextMode="MultiLine" CssClass="text" />
		<asp:RegularExpressionValidator runat="server" ID="uxBiographyREV" ControlToValidate="uxBiography" ErrorMessage="Biography is too long.  It must be 500 characters or less." ValidationExpression="^[\s\S]{0,500}$" />
	</div>
	<div class="formWhole">
		<h3>Offices</h3>
		<table class="listing paddingBottom profileActions" id="offices">
			<thead>
				<tr>
					<th class="action">Action</th>
					<th>Office</th>
					<th>MLS ID</th>
				</tr>
			</thead>
			<tbody>
				<asp:Repeater runat="server" ID="uxUserOffices" ItemType="Classes.Media352_MembershipProvider.UserOffice">
					<ItemTemplate>
						<tr>
							<td>
								<asp:LinkButton ID="uxDelete" runat="server" OnCommand="Office_Command" CommandName="Delete" CommandArgument='<%#Item.UserOfficeID%>' OnClientClick="return confirm('Are you sure you want to remove the user from this office?');" CssClass="button delete"
									Text="<span>Delete</span>" CausesValidation="false" />
							</td>
							<td><%# Item.Office.Name %></td>
							<td><%# Item.MlsID %></td>
						</tr>
					</ItemTemplate>
				</asp:Repeater>
				<tr>
					<td>
						<asp:LinkButton runat="server" ID="uxAdd" CssClass="button add" Text="<span>Add</span>" OnCommand="Office_Command" CommandName="Add" ValidationGroup="AddOffice" />
					</td>
					<td>
						<asp:DropDownList runat="server" ID="uxOffice" AppendDataBoundItems="true">
							<asp:ListItem Text="--Select an Office--" Value=""></asp:ListItem>
						</asp:DropDownList>
						<asp:RequiredFieldValidator runat="server" ID="uxOfficeRFV" ControlToValidate="uxOffice" ErrorMessage="You must select an office." InitialValue="" ValidationGroup="AddOffice" />
					</td>
					<td>
						<asp:TextBox ID="uxMLSID" runat="server" CssClass="text" MaxLength="50" />
						<asp:RequiredFieldValidator runat="server" ID="uxMLSIDRFV" ControlToValidate="uxMLSID" ErrorMessage="MLS ID is required." ValidationGroup="AddOffice" />
					</td>
				</tr>
			</tbody>
		</table>
	</div>
	<div class="formWhole">
		<h3>Teams</h3>
		<table class="listing paddingBottom profileActions" id="teams">
			<thead>
				<tr>
					<th class="action">Action</th>
					<th>Team</th>
				</tr>
			</thead>
			<tbody>
				<asp:Repeater runat="server" ID="uxUserTeams" ItemType="Classes.Media352_MembershipProvider.UserTeam">
					<ItemTemplate>
						<tr>
							<td>
								<asp:LinkButton ID="uxDelete" runat="server" OnCommand="Team_Command" CommandName="Delete" CommandArgument='<%#Item.UserTeamID%>' OnClientClick="return confirm('Are you sure you want to remove the user from this team?');" CssClass="button delete"
									Text="<span>Delete</span>" CausesValidation="false" />
							</td>
							<td><%# Item.Team.Name %></td>
						</tr>
					</ItemTemplate>
				</asp:Repeater>
				<tr>
					<td>
						<asp:LinkButton runat="server" CssClass="button add" Text="<span>Add</span>" OnCommand="Team_Command" CommandName="Add" ValidationGroup="AddTeam" />
					</td>
					<td>
						<asp:DropDownList runat="server" ID="uxTeam" AppendDataBoundItems="true">
							<asp:ListItem Text="--Select a Team--" Value=""></asp:ListItem>
						</asp:DropDownList>
						<asp:RequiredFieldValidator runat="server" ID="uxTeamRFV" ControlToValidate="uxTeam" ErrorMessage="You must select an team." InitialValue="" ValidationGroup="AddTeam" />
					</td>
				</tr>
			</tbody>
		</table>
	</div>
	<div class="formWhole">
		<h3>Testimonials</h3>
		<table class="listing paddingBottom profileActions" id="testomonials">
			<thead>
				<tr>
					<th class="action">Action</th>
					<th class="testimonial">Testimonial</th>
					<th class="giverName">Giver Name and Location</th>
				</tr>
			</thead>
			<tbody>
				<asp:Repeater runat="server" ID="uxUserTestimonial" ItemType="Classes.Media352_MembershipProvider.UserTestimonial">
					<ItemTemplate>
						<tr>
							<td>
								<asp:LinkButton ID="uxDelete" runat="server" OnCommand="Testimonial_Command" CommandName="Delete" CommandArgument='<%#Item.UserTestimonialID%>' OnClientClick="return confirm('Are you sure you want to remove this user testimonial?');" CssClass="button delete"
									Text="<span>Delete</span>" CausesValidation="false" />
							</td>
							<td><%# Item.Testimonial %></td>
							<td><%# Item.GiverNameAndLocation %></td>
						</tr>
					</ItemTemplate>
				</asp:Repeater>
				<tr>
					<td>
						<asp:LinkButton runat="server" CssClass="button add" Text="<span>Add</span>" OnCommand="Testimonial_Command" CommandName="Add" ValidationGroup="AddTestimonial" />
					</td>
					<td>
						<asp:TextBox ID="uxTestimonial" runat="server" CssClass="text" MaxLength="255" TextMode="MultiLine" />
						<asp:RequiredFieldValidator runat="server" ID="uxTestimonialRFV" ControlToValidate="uxTestimonial" ErrorMessage="Testimonial is required." ValidationGroup="AddTestimonial" />
						<asp:RegularExpressionValidator runat="server" ID="uxTestimonialREV" ControlToValidate="uxTestimonial" ErrorMessage="Testimonial is too long.  It must be 255 characters or less." ValidationExpression="^[\s\S]{0,255}$" ValidationGroup="AddTestimonial" />
					</td>
					<td>
						<asp:TextBox ID="uxGiverNameAndLocation" runat="server" CssClass="text" MaxLength="500" />
						<asp:RequiredFieldValidator runat="server" ID="uxGiverNameAndLocationRFV" ControlToValidate="uxGiverNameAndLocation" ErrorMessage="Giver Name and Location is required." ValidationGroup="AddTestimonial" />
						<asp:RegularExpressionValidator runat="server" ID="uxGiverNameAndLocationREV" ControlToValidate="uxGiverNameAndLocation" ErrorMessage="Giver Name and Location is too long.  It must be 500 characters or less." ValidationExpression="^[\s\S]{0,500}$" ValidationGroup="AddTestimonial" />
					</td>
				</tr>
			</tbody>
		</table>
	</div>
</div>
<div class="rightCol" style="float:right;">
	<div class="profileWrapper">
		<div class="profileInfoForm">
			<h3>Phone Numbers</h3>
			<div class="formHalf">
				<label for='<%=uxHomePhone.ClientID + "_uxPhoneBox_text"%>'>
					Home Phone</label>
				<Controls:PhoneBox runat="server" ID="uxHomePhone" Required="false" TextBoxClass="text" ShowExtension="false" />
			</div>
			<div class="formHalf">
				<label for='<%=uxCellPhone.ClientID + "_uxPhoneBox_text"%>'>
					Cell Phone</label>
				<Controls:PhoneBox runat="server" ID="uxCellPhone" Required="false" TextBoxClass="text" ShowExtension="false" />
			</div>
			<div class="formHalf">
				<label for='<%=uxOfficePhone.ClientID + "_uxPhoneBox_text"%>'>
					Office Phone</label>
				<Controls:PhoneBox runat="server" ID="uxOfficePhone" Required="false" TextBoxClass="text" />
			</div>
			<div class="formHalf">
				<label for='<%=uxFax.ClientID + "_uxPhoneBox_text"%>'>
					Fax</label>
				<Controls:PhoneBox runat="server" ID="uxFax" Required="false" TextBoxClass="text" ShowExtension="false" />
			</div>
			<div class="formHalf">
				<label for='<%=uxPrimaryPhone.ClientID %>'>
					Primary Phone</label>
				<asp:DropDownList runat="server" ID="uxPrimaryPhone">
					<asp:ListItem Text="Home Phone" Value="Home Phone"></asp:ListItem>
					<asp:ListItem Text="Cell Phone" Value="Cell Phone"></asp:ListItem>
					<asp:ListItem Text="Office Phone" Value="Office Phone"></asp:ListItem>
					<asp:ListItem Text="Fax" Value="Fax"></asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="clear"></div>
		</div>
		<!--end profileInfoForm-->
	</div>
</div>
<script type="text/javascript">
	$(document).ready(function () {
		$("#<%= uxRating.ClientID %>").keydown(function (e) {
			var key = e.charCode || e.keyCode || 0;
			return key == 8 || key == 9 || key == 46 || (key >= 37 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == 110 || key == 190;
		});
	});
</script>
