﻿<%@ page language="C#" masterpagefile="~/MasterPage1.master" autoeventwireup="true" inherits="Note, App_Web_i3mdp3xs" title="系统提示 - cnOpenBlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<style type="text/css">
.notebox { margin:40px auto; width:500px; font-size:16px; }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="notebox"><% Response.Write(Resources.Note.ResourceManager.GetString(Request["q"])); %></div>
</asp:Content>

