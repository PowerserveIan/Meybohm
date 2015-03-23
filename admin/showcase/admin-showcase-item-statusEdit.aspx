<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="admin-showcase-item-statusEdit.aspx.cs" Inherits="Admin_AdminShowcaseItemEditStatus" Title="Admin - Showcase Item Edit Status" ValidateRequest="false" %>

<%@ Import Namespace="Classes.Showcase" %>
<%@ Register TagPrefix="Controls" TagName="DateTimePicker" Src="~/Controls/BaseControls/DateTimePicker.ascx" %>
<%@ Register TagPrefix="Controls" TagName="FileUpload" Src="~/Controls/BaseControls/FileUploadControl.ascx" %>
<%@ Register TagPrefix="Controls" TagName="PhoneBox" Src="~/Controls/BaseControls/PhoneBoxControl.ascx" %>
<%@ Register TagPrefix="SEOComponent" TagName="CurrentPageSEO" Src="~/Controls/SEOComponent/SEO_Data_Entry.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Address" Src="~/Controls/State_And_Country/Address.ascx" %>
<%@ Register TagPrefix="Showcase" TagName="AdminSellNewHome" Src="~/Controls/Showcase/AdminSellNewHome.ascx" %>

<asp:Content ContentPlaceHolderID="ContentWindow" runat="server">
    <script type="text/javascript">
        
    </script>
    <asp:PlaceHolder runat="server" ID="uxHeader"></asp:PlaceHolder>
    <asp:Panel runat="server" ID="uxPanel" DefaultButton="uxSave">
        <div class="formWrapper">
            <asp:PlaceHolder runat="server" ID="uxMLSIDPH">
			    <div class="formHalf">
				    <span class="label">MLS ID #</span>
				    <asp:Literal runat="server" ID="uxMLSID"></asp:Literal>
			    </div>
                <div class="formHalf">
				    <span class="label">Title</span>
				    <asp:Literal runat="server" ID="uxTitle"></asp:Literal>
			    </div>

                <div class="clear"></div>

                <div class="formHalf">
				    <span class="label">Is Fine</span>
				    <asp:DropDownList runat="server" ID="uxIsFine" AppendDataBoundItems="true">
					    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
				    </asp:DropDownList>
			    </div>

                <div class="clear"></div>

                <div class="formHalf">
				    <span class="label">Is Fine Featured</span>
				    <asp:DropDownList runat="server" ID="uxIsFineFeatured" AppendDataBoundItems="true">
					    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
				    </asp:DropDownList>
			    </div>

                <div class="clear"></div>

                <div class="formWhole">
			        <span class="label">Description</span>
			        <asp:TextBox runat="server" ID="uxDescription" MaxLength="2000" TextMode="MultiLine" CssClass="text" />
		        </div>

                <div class="formWhole">
			        <span class="label">Features</span>
			        <asp:TextBox runat="server" ID="uxFeatures" MaxLength="2000" TextMode="MultiLine" CssClass="text" />
		        </div>

                <div class="formHalf">
				    <span class="label">Fine Property Tags</span>
				    <asp:CheckBox ID="uxEquestrian" runat="server" Text="Equestrian"/><br />
				    <asp:CheckBox ID="uxEstate" runat="server" Text="Estate"/><br />
				    <asp:CheckBox ID="uxWaterfront" runat="server" Text="Waterfront"/><br />
				    <asp:CheckBox ID="uxGolf" runat="server" Text="Golf"/><br />
				    <asp:CheckBox ID="uxHistoric" runat="server" Text="Historic"/><br />
				    <asp:CheckBox ID="uxAcreage" runat="server" Text="Acreage"/><br />
			    </div>

		    </asp:PlaceHolder>

            <div class="formWhole">
				<asp:HyperLink runat="server" ID="uxEditMediaCollection" Text="<span>Edit the Media Collection for this Fine Property</span>" CssClass="button edit"></asp:HyperLink>
			</div>
        </div>

        <div class="buttons fixedBottom">
		    <%--The markup for the buttons is in the BaseEditPage--%>
		    <asp:PlaceHolder runat="server" ID="uxButtonContainer"></asp:PlaceHolder>
        </div>
    </asp:Panel>
</asp:Content>
