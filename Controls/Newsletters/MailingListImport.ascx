<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailingListImport.ascx.cs" Inherits="Admin_Newsletters_MailingListImport" %>
<asp:PlaceHolder ID="uxFileImportPH" runat="server">
	<asp:Panel runat="server" ID="uxPanel" DefaultButton="uxImport" CssClass="blue">
		Location of .csv to import from:
		<asp:CustomValidator ID="uxFileUploadCV" runat="server" ErrorMessage="*File type must be .csv." /><br />
		<asp:FileUpload ID="uxFileUpload" runat="server" />
		<asp:LinkButton ID="uxImport" runat="server" Text="<span>Import</span>" CssClass="button upload"></asp:LinkButton><br />
		<asp:HyperLink ID="uxDownloadExampleCSV" runat="server" Text="Download an example .csv file" NavigateUrl="~/admin/newsletters/Mailing_Lists/ExampleImport.csv"></asp:HyperLink><br />
		<asp:HyperLink ID="uxGetHelpLink" runat="server" Text="How to use the import function" NavigateUrl="~/admin/newsletters/Mailing_Lists/ImportFeature.doc"></asp:HyperLink>
		<div class="clear"></div>
	</asp:Panel>
</asp:PlaceHolder>
<asp:PlaceHolder ID="uxSuccessPH" runat="server" Visible="false">
	<asp:Label ID="uxSuccessLbl" runat="server" />
	<asp:Button ID="uxImportMore" runat="server" Text="Import More" />
	<div class="clear"></div>
</asp:PlaceHolder>
<asp:PlaceHolder ID="uxErrorsPH" runat="server" Visible="false">
	<asp:Label ID="uxNumErrorsLbl" runat="server" />
	<asp:Repeater ID="uxErrorsRepeater" runat="server">
		<HeaderTemplate>
			<table>
				<tr>
					<th style="padding-right: 50px; font-weight: bold;">
						Subscriber Email
					</th>
					<th style="font-weight: bold;">
						Error Type
					</th>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td>
					<%#Eval("SubscriberEmail")%>
				</td>
				<td>
					<%#Eval("Reason")%>
				</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
	<br />
	<asp:Button ID="uxRedo" runat="server" Text="Try again" />
</asp:PlaceHolder>
