﻿<%@ page language="C#" masterpagefile="~/Admin/MasterPage.master" autoeventwireup="true" inherits="Admin_Users, App_Web_1bxv0wc1" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="holdHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holdBody" Runat="Server">
<asp:GridView ID="gridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="True" OnPageIndexChanging="gridView1_PageIndexChanging" PageSize="20">
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#EFF3FB" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <AlternatingRowStyle BackColor="White" />
</asp:GridView>
</asp:Content>

