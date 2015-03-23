<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserLoginInformation.ascx.cs" Inherits="Controls_Media352_MembershipProvider_UserLoginInformation" %>
<asp:PlaceHolder runat="server" ID="uxUserNamePlaceHolder">
	<div class="formHalf">
		<label for="<%=uxName.ClientID%>">
			User Name<span class="asterisk">*</span>
		</label>
		<asp:TextBox CssClass="text" runat="server" ID="uxName" MaxLength="50" />
		<asp:RequiredFieldValidator runat="server" ID="uxNameReqFVal" ControlToValidate="uxName" ErrorMessage="Name is required." />
		<asp:RegularExpressionValidator runat="server" ID="uxNameRegexVal" ControlToValidate="uxName" ErrorMessage="Name is too long.  It must be 50 characters or less." ValidationExpression="^[\s\S]{0,50}$" />
		<asp:CustomValidator runat="server" ID="uxNameCV" ErrorMessage="A user already exists with that user name." />
	</div>
</asp:PlaceHolder>
<div class="formHalf">
	<label for="<%=uxEmail.ClientID%>">
		Email<span class="asterisk">*</span>
	</label>
	<asp:TextBox CssClass="text" runat="server" ID="uxEmail" MaxLength="382" />
	<asp:RequiredFieldValidator runat="server" ID="uxEmailReqFVal" ControlToValidate="uxEmail" ErrorMessage="Email is required." />
	<asp:RegularExpressionValidator runat="server" ID="uxEmailRegexVal" ControlToValidate="uxEmail" ErrorMessage="Email is too long.  It must be 382 characters or less." ValidationExpression="^[\s\S]{0,382}$" />
	<asp:RegularExpressionValidator runat="server" ID="uxEmailRegexVal2" ControlToValidate="uxEmail" ErrorMessage="Email is an invalid email address." />
	<asp:CustomValidator runat="server" ID="uxEmailCV" ErrorMessage="A user already exists with that email address." />
</div>
<div class="clear"></div>
<asp:LinkButton ID="uxChangePassword" runat="server" Text="<span>Change Password</span>" OnClick="uxChangePassword_Click" CausesValidation="false" CssClass="button change paddingBottom" />
<hr />
<div style="color: #CC0000;">
	<asp:Literal ID="uxMemberInfoStatus" runat="server" />
</div>
<asp:PlaceHolder runat="server" ID="uxPasswordPH">
	<div class="formHalf">
		<label for="<%=uxPassword.ClientID%>">
			Password<span class="asterisk">*</span> <small>Must be between 6 and 14 characters</small>
		</label>
		<asp:TextBox CssClass="text" runat="server" ID="uxPassword" TextMode="password" MaxLength="14" />
		<asp:RequiredFieldValidator runat="server" ID="uxPasswordReqFVal" ControlToValidate="uxPassword" ErrorMessage="Password is required." />
		<asp:RegularExpressionValidator runat="server" ID="uxPasswordRegexVal" ControlToValidate="uxPassword" ErrorMessage="Password must be between 6 and 14 characters." />
	</div>
	<div class="formHalf">
		<label for="<%=uxConfirmPassword.ClientID%>">
			Confirm Password<span class="asterisk">*</span>
		</label>
		<asp:TextBox CssClass="text" runat="server" ID="uxConfirmPassword" TextMode="password" MaxLength="14" />
		<asp:RequiredFieldValidator runat="server" ID="uxConfirmPasswordReqFVal" ControlToValidate="uxConfirmPassword" ErrorMessage="Confirm Password is required." />
		<asp:RegularExpressionValidator runat="server" ID="uxConfirmPasswordREV" ControlToValidate="uxPassword" ErrorMessage="Confirm Password must be between 6 and 14 characters." />
		<asp:CompareValidator ID="uxConfirmPasswordCV" runat="server" ErrorMessage="Passwords don't match." ControlToValidate="uxConfirmPassword" ControlToCompare="uxPassword" />
	</div>
	<asp:PlaceHolder runat="server" ID="uxSecurityQuestionPlaceHolder">
		<div class="clear"></div>
		<div class="formHalf">
			<label for="<%=uxPasswordQuestion.ClientID%>">
				Security Question<span class="asterisk">*</span>
			</label>
			<asp:DropDownList CssClass="dynamic" runat="server" ID="uxPasswordQuestion" AppendDataBoundItems="true">
				<asp:ListItem Text="" Value=""></asp:ListItem>
			</asp:DropDownList>
			<asp:RequiredFieldValidator runat="server" ID="uxPasswordQuestionReqFVal" ControlToValidate="uxPasswordQuestion" ErrorMessage="Security Question is required." InitialValue="" />
		</div>
		<div class="formHalf">
			<label for="<%=uxPasswordAnswer.ClientID%>">
				Security Answer<span class="asterisk">*</span>
			</label>
			<asp:TextBox CssClass="text" runat="server" ID="uxPasswordAnswer" MaxLength="50" />
			<asp:RequiredFieldValidator runat="server" ID="uxPasswordAnswerReqFVal" ControlToValidate="uxPasswordAnswer" ErrorMessage="Security Answer is required." />
			<asp:RegularExpressionValidator runat="server" ID="uxPasswordAnswerRegexVal" ControlToValidate="uxPasswordAnswer" ErrorMessage="Security Answer is too long.  It must be 50 characters or less." ValidationExpression="^[\s\S]{0,50}$" />
		</div>
	</asp:PlaceHolder>
</asp:PlaceHolder>
<div class="clear"></div>
