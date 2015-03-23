<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FacebookLikeButton.ascx.cs" Inherits="Controls_Publishing_FacebookLikeButton" %>

<fb:like href="<%= UrlToLike %>" class="facebookLike" width="<%= ButtonWidth %>" layout='<%= ShowFriendText ? "standard" : "button_count" %>'></fb:like>