﻿<%@ page language="C#" masterpagefile="~/MasterPage2.master" autoeventwireup="true" inherits="Groups_Member, App_Web_lmpxl1fb" title="管理群组成员 - cnOpenBlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<script type="text/javascript" src="/groups/js/member.js"></script>
<style type="text/css">
.mitem { border-bottom:solid 1px #cccccc; padding:4px 0px; margin-top:6px;}
.mitem .col1 { float:left; width:42px; }
.mitem .col1 img { width:42px; border:none; }
.mitem .col2 { float:left; width:400px; margin-left:6px; }
.mitem .col3 { float:right; width:90px; }
.mitem .col3 a { display:block; padding:2px 6px; }
.mitem .col3 a:hover { background-color:#487DA5; color:#ffffff; }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div id="leftdiv" class="left">
    <h2 class="subtitle">管理群组成员</h2>
    <div style="padding-right:40px; padding-bottom:20px;">
    <asp:Literal ID="lblMsgList" runat="server" EnableViewState="false"></asp:Literal>
    </div>
    <div class="pagelist">
    <asp:Literal ID="lblPageList" runat="server" EnableViewState="false"></asp:Literal>
    </div>
</div>
<!--right start-->
<div id="rightdiv" class="right">
    <div class="rightbox">
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
    </script>
    <div class="tablink2">
    <p id="p1"><a href="/baseinfo.aspx">我的资料</a></p>
    <p id="p2"><a href="/write.aspx">发表文章</a></p>
    <p id="p3"><a href="/postlist.aspx">管理文章</a></p>
    <p id="p4"><a href="/inbox.aspx">管理留言</a></p>
    <p id="p5"><a href="/feedback.aspx">管理评论</a></p>
    <p id="p6"><a href="/favorite.aspx">我的网摘</a></p>
    <p id="p7"><a class="curr" href="/group.aspx">我的群组</a></p>
    <p id="p9"><a href="/settings.aspx">主页设置</a></p>
    </div>
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;margin-top:-4px;" /><img src="/images/box/br.gif" alt="" style="float:right;margin-top:-4px;" />');
    </script>
    </div>
    <br />
    <div class="rightbox">
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
    </script>
    <div class="tablink2" style="padding-left:14px;">
    <p><a class="curr" href="/groups.aspx">所有参与的群组</a></p>
    <p><a href="/groups/create.aspx">我创建的群组</a></p>
    <p><a href="/groups/join.aspx">我参加的群组</a></p>
    <p><a href="/groups/post.aspx">我发起的话题</a></p>
    <p><a href="/groups/reply.aspx">我回复的话题</a></p>
    </div>
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;margin-top:-4px;" /><img src="/images/box/br.gif" alt="" style="float:right;margin-top:-4px;" />');
    </script>
    </div>
    <br />
    <div class="rightbox">
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
    </script>
    <div class="tablink2" style="padding-left:14px;">
    <p><a href="/groups/10000.aspx">修改群组资料</a></p>
    <p><a class="curr" href="/groups/10000/member.aspx">管理群组成员</a></p>
    </div>
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;margin-top:-4px;" /><img src="/images/box/br.gif" alt="" style="float:right;margin-top:-4px;" />');
    </script>
    </div>
</div>
<div class="clear"></div>
<script type="text/javascript">gid=<%=groupID %>;</script>
</asp:Content>

