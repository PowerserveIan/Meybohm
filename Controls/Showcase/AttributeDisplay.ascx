<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttributeDisplay.ascx.cs" Inherits="Controls_Showcase_AttributeDisplay" %>
<%@ Import Namespace="Classes.Showcase" %>
<table class='attributes <%=Settings.AttributeDisplayStyle == AttributeDisplays.Tabs ? "full" : "featured"%>' border="0">
	<asp:Repeater runat="server" ID="uxAttributesRepeater" DataSource="<%#ShowcaseAttributes%>" ItemType="Classes.Showcase.ShowcaseAttribute">
		<ItemTemplate>
			<tr>
				<td>
					<span>
						<%#Item.Title%>:</span>
				</td>
				<td>
					<%#Item.ShowcaseAttributeValues.Count > 3 ? "<a href='#' class='attributeToggle'>See Features</a><div class='attributeCollapse' style='display: none;'>" : "" %>
					<asp:Repeater runat="server" ID="uxValuesRepeater" DataSource="<%#Item.ShowcaseAttributeValues%>" ItemType="Classes.Showcase.ShowcaseAttributeValue">
						<ItemTemplate>							
							<%# (Item.ShowcaseAttribute.Title.ToLower().Contains("price") ? "$" : "") + Item.Value%>
						</ItemTemplate>
						<SeparatorTemplate>
							<br />
						</SeparatorTemplate>
					</asp:Repeater>
					<%#Item.ShowcaseAttributeValues.Count > 3 ? "</div>" : "" %>
				</td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</table>
<script type="text/javascript">
    $(document).ready(function() {
    	$(".attributeToggle").click(function () {
    		$(this).html($(this).siblings(".attributeCollapse").is(":visible") ? "See Features" : "Collapse Features");
    		$(this).siblings(".attributeCollapse").slideToggle(100);    		
    		return false;
    	});
    });
</script>
