﻿var arr_cat = new Array(new Array('11','电脑/网络'),new Array('12','生活/时尚'),new Array('13','家庭/婚姻'),new Array('14','电子数码'),new Array('15','商业/理财'),new Array('16','教育/学业'),new Array('17','交通/旅游'),new Array('18','社会/文化'),new Array('19','人文学科'),new Array('20','理工学科'),new Array('21','休闲/娱乐'),new Array('22','忧愁/烦恼'));
function bindCat(k)
{
    var e = el("selCat");
    for(var i in arr_cat){
        e.options[i] = new Option(arr_cat[i][1], arr_cat[i][0]);
        if(arr_cat[i][0]==k.toString()) e.options[i].selected = true;
    }
}
var doing = false;
function uploadImg(e)
{
    if(doing) return;
    doing = true;
    el("notediv").innerHTML = loading;
    var win = window.frames["photo_frm"];
    win.document.forms[0].submit();
}

function loat()
{
    el("notediv").innerHTML = "";
    el("gimg").src = "/upload/group/"+el("hd_group").value+".jpg?"+Math.random();
    el("photo_frm").src = el("photo_frm").src;
}
function save()
{
    if(doing) return;
    doing = true;
    var er = el("notediv");
    er.className = "box erbox";
    el("txtName").className = "put";
    if(!el("txtName").value.Trim()){
        el("txtName").className = "put erput";
        er.innerHTML = "群组名称不能为空。";
        el("txtName").focus();
        return false;
    }
    er.className = "";
    er.innerHTML = loading;
    
    var jj = el("txtJianjie").value;
    var gg = el("txtGonggao").value;
    if(jj.length>200) jj=jj.substr(0,200);
    if(gg.length>200) gg=gg.substr(0,200);
    
    var url = execURL +"?editgroup="+el("hd_group").value;
    var data = "g_name="+ URLencode(el("txtName").value);
    data += "&catid="+ el("selCat").value;
    data += "&tags="+ URLencode(el("txtTags").value);
    data += "&jianjie="+ URLencode(jj);
    data += "&gonggao="+ URLencode(gg);
    
    ajaxPost(url, data);
    
    setTimeout(function(){
        el("notediv").className = "box okbox";
        el("notediv").innerHTML = "更改已保存。";
        doing = false;
    }, 3000);
    setTimeout(function(){
        el("notediv").className = "";
        el("notediv").innerHTML = "";
    }, 8000);
}