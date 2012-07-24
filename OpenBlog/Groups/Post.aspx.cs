﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using DLL;

public partial class Groups_Post : UserPage
{
    protected string username;

    private int pageIndex = 0, pageSize = 20, pageCount = 1;
    private int pageNumber = 9;//显示页数

    protected void Page_Load(object sender, EventArgs e)
    {
        username = CKUser.Username;
        if (Request.QueryString["q"] != null)
        {
            if (int.TryParse(Request.QueryString["q"], out pageIndex))
                pageIndex -= 1;
            else pageIndex = 0;
        }
        BindTopic();
    }

    private void BindTopic()
    {
        string fields = "[t_id],[t_title],[t_read_cnt],[t_reply_cnt],[t_uptime],[g_id],[g_name]";
        string where = "[g_id]=[t_g_id] and [t_user_name]='" + username + "'";

        string sqlc = "select count(*) from [topic] where [t_user_name]='" + username + "'";
        int cnt = (int)DB.GetValue(sqlc);
        if (cnt == 0) return;

        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        DataTable dt = Data.GetPagingData("[topic],[group]", fields, "[t_id]", "[t_id]", pageIndex, pageSize, where, true);

        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<div class='topicitem'>");
            sb.AppendFormat("<a class='bold' href='/group/topic/{0}{1}'>{2}</a>", row["t_id"], Settings.Ext, Tools.HtmlEncode(row["t_title"].ToString()));
            sb.AppendFormat(" <a class='hui' href='/group/{0}{1}'>[{2}]</a>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
            sb.AppendFormat("<a class='del' href='javascript:void(0);' onclick='javascript:deleteTopic(this,{0});return false;'>删除</a>", row["t_id"]);
            sb.AppendFormat("<a class='edit' href='/group/editpost.aspx?q={0}' target=_blank>修改</a>", row["t_id"]);
            sb.AppendFormat("<p class='em'>浏览({0}) 回复({1}) 发表于 {2}</p>", row["t_read_cnt"], row["t_reply_cnt"], Tools.DateString(row["t_uptime"].ToString()));
            sb.Append("</div>");
        }
        lblMsgList.Text = sb.ToString();

        if (pageCount > 1)
        {
            string url = "/groups/post/{0}" + Settings.Ext;

            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }
    }
}
