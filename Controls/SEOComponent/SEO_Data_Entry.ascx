<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SEO_Data_Entry.ascx.cs" Inherits="Controls_SEOComponent_SEO_Data_Entry" %>
<div class="sectionTitle">
	<div class="bottom">
		<h2>Search Engine Optimization</h2>
	</div>
</div>
<asp:CustomValidator runat="server" ID="uxPageLinkURLCV" ErrorMessage="PageLinkURL is already being used, unable to save SEO data." Display="None" OnServerValidate="uxPageLinkURLCV_ServerValidate" />
<div class="formWrapper">
	<div class="formHalf">
		<label for="<%=uxTitle.ClientID%>">
			Page Title<span class="tooltip"><span>You can see the page title by looking at the very top bar of your browser window when looking at a webpage.<br />
				<br />
				This is extremely important to rankings!<br />
				<br />
				Each and every page should have a unique Page Title and it should contain at least 3-4 keywords. 352 Media Group doesn't even put the company's name in the page title, because as much room as possible should be saved for keywords.<br />
				<br />
				If multiple pages of a website have the same page title, it will hurt the website's rankings.<br />
				<br />
				The page title should be 8-12 words long. Phrases can be separated with ,-+ or |.<br />
				<br />
				NOTE: In the Configuration Settings, you can set a Sitewide Title that will appear at the end of all TITLE tags throughout the website. This is normally set to your company name, so your company name will automatically be added to the end of the TITLE tag
				you specify here. Assuming it is set that way, do not put your company name here in this TITLE.</span></span></label>
		<asp:TextBox runat="server" ID="uxTitle" MaxLength="150" CssClass="text" />
		<asp:RegularExpressionValidator runat="server" ID="uxTitleRegexVal" ControlToValidate="uxTitle" ErrorMessage="SEO Title is too long.  It must be 150 characters or less." ValidationExpression="^[\s\S]{0,150}$" />
	</div>
	<div class="formHalf">
		<label for="<%=uxKeywords.ClientID%>">
			Keywords<span class="tooltip"><span>Blog post about finding good key words:
				<a href="http://www.352media.com/blog/post/2010/05/20/Choosing-the-Best-Keywords-to-Increase-Search-Engine-Rankings.aspx">http://www.352media.com/blog/post/2010/05/20/Choosing-the-Best-Keywords-to-Increase-Search-Engine-Rankings.aspx</a>
				<br />
				<br />
				The best keywords aren’t really words; they’re phrases. Single words are too broad, searched too often, and consumers aren’t committed to what they’re looking for.
				<br />
				<br />
				You have the best chance of ranking under specific keywords and local keywords (with a city or state name in them). Once start building incoming links and getting a higher page rank, have a better shot at ranking under the generic terms.
				<br />
				<br />
				Incorporate those keywords naturally. You can’t just stuff the words onto the page. Google will catch that and ban you from results. Example of keyword stuffing: www.davidnaylor.co.uk/seo-tip-keyword-stuffing.html
				<br />
				<br />
				Only focus on 3-4 keywords per page. Anything more is going to over-saturate the page and you’ll end up not ranking for anything. Anything under you’re missing potential on ranking under some keywords. </span></span>
		</label>
		<asp:TextBox runat="server" ID="uxKeywords" MaxLength="500" CssClass="text" />
		<asp:RegularExpressionValidator runat="server" ID="uxKeywordsREV" ControlToValidate="uxKeywords" ErrorMessage="SEO Keywords is too long.  It must be 500 characters or less." ValidationExpression="^[\s\S]{0,500}$" />
	</div>
	<div class="formWhole">
		<label for="<%=uxDescription.ClientID%>">
			Description<span class="tooltip"><span>Short and concise descriptions are ideal. The first 160 characters are most important. Many search engines stop displaying after this character length in the SERPs (Search Engines Results Pages).<br />
				<br />
				Including a description tag gives a little more control over what searchers will see when your Web Site does come up for search results. So again having a short, attractive and professional description is the goal. The description will not always be displayed
				for a search query. Its display depends on the search engine, content of the description, and the search query itself. (Descriptions should not be duplicated across pages).</span></span></label>
		<asp:TextBox runat="server" ID="uxDescription" TextMode="MultiLine" CssClass="text" />
		<asp:RegularExpressionValidator runat="server" ID="uxDescriptionREV" ControlToValidate="uxDescription" ErrorMessage="SEO Description is too long.  It must be 500 characters or less." ValidationExpression="^[\s\S]{0,500}$" />
	</div>
	<asp:PlaceHolder runat="server" ID="uxFriendlyFilenamePH">
		<div class="formWhole staticError" id="friendlyFileNameContainer">
			<label for="<%=uxFriendlyFilename.ClientID%>">
				Friendly Filename<span class="tooltip"><span>Users will be able to access this item by going to the filename you specify here.</span></span><span>Cannot contain spaces or special characters (hyphens allowed).
					<asp:Literal runat="server" ID="uxAspxAppended">".aspx" will automatically be appended.</asp:Literal></span></label>
			<asp:TextBox runat="server" ID="uxFriendlyFilename" MaxLength="45" CssClass="text" />
			<asp:RegularExpressionValidator runat="server" ID="uxFriendlyFilenameREV" ControlToValidate="uxFriendlyFilename" ErrorMessage="Friendly Filename must be less than 45 characters and only contain alphanumeric characters with hyphens instead of spaces." ValidationExpression="^[a-zA-Z0-9\-\/]{0,45}$" />
			<asp:CustomValidator runat="server" ID="uxFriendlyFilenameUniqueVal" ControlToValidate="uxFriendlyFilename" ErrorMessage="A page already exists with that name in your site.  Your friendly filename must be unique." />
		</div>
	</asp:PlaceHolder>
</div>
<div class="clear"></div>
