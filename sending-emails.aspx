<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sending-emails.aspx.cs" Inherits="SendingEmails" %>

<!DOCTYPE html>
<html lang="en">
<head>
	<title>Sending Emails</title>
	<%
		if (m_IsSending)
		{%>
	<meta http-equiv="refresh" content="3" />
	<%
		}%>
	<style type="text/css">
		.progressbarcontainer {
			width: 98%;
			height: 15px;
			border: solid 1px black;
			padding: 1px;
		}
		.progressbar {
			width: 0px;
			background-color: Red;
			height: 15px;
		}
		div.floatingBox {
			position: absolute;
			z-index: 5;
			left: 50%;
			margin-left: -200px;
			top: 40%;
		}
		div.progressBox {
			background-color: #999999;
			text-align: left;
			padding: 10px;
			color: #000000;
			position: relative;
			left: -10px;
			bottom: 141px !important;
			bottom: 145px;
			filter: alpha(opacity=95);
			-moz-opacity: .95;
			opacity: .95;
			height: <%=(200 * EmailSender.SendEmailJobs.Count)%>px;
			width: 400px;
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
	<asp:Repeater runat="server" ID="rpJobs" ItemType="EmailSender">
		<ItemTemplate>
			<asp:Panel ID="panProgress" runat="server">
				<div class="sectiontitle">Sending
					<%#Item.Subject%>... </div>
				<p>
				</p>
				<div class="progressbarcontainer">
					<div class="progressbar" style="width: <%#Item.PercentageCompleted%>%"></div>
				</div>
				<br />
				<br />
				<div id="progressdescription<%#Container.ItemIndex%>"><b>
					<%#Math.Round(Convert.ToDecimal(Item.PercentageCompleted), 2)%>% completed</b> -
					<%#Item.SentMails%>
					out of
					<%#Item.TotalMails%>
					emails have been sent. </div>
				<br />
				<br />
				<div id="notSentEmails" runat="server" visible="<%#Item.NotSentMails > 0%>">There was an error sending
					<%#Item.NotSentMails%>
					of your emails, they will be resent after the initial batch has finished.
					<br />
					<br />
				</div>
				<div id="mailServerError" runat="server" visible="<%#Item.MailServerDown%>">Your emails have stopped sending. There appears to be a problem with your mail server. Please contact support or try again later (use the resend button on
					the
					<a runat="server" href="~/admin/newsletters/admin-newsletter-metrics-edit.aspx">statistics page</a>
					to avoid sending to the same person more than once). </div>
			</asp:Panel>
			<asp:Panel ID="panNoNewsletter" runat="server" Visible="false">
				<b>No newsletter is currently being sent.</b>
			</asp:Panel>
		</ItemTemplate>
	</asp:Repeater>
	</form>
</body>
</html>
