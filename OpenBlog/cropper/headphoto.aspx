﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="headphoto.aspx.cs" Inherits="cropper_headphoto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link type="text/css" rel="stylesheet" href="css/common.css" />
    <script type="text/javascript" src="script/prototype.js"></script>
    <script type="text/javascript" src="script/cropper/scriptaculous.js?load=builder,dragdrop"></script>
    <script type="text/javascript" src="script/cropper/cropper.js"></script>
    <script type="text/javascript">
    var position=new Array();
    function onEndCrop(opic){
	    $('x1').value=position[0]=opic.x1;
	    $('y1').value=position[1]=opic.y1;
	    $('x2').value=position[2]=opic.x2;
	    $('y2').value=position[3]=opic.y2;
	    $("w_b").innerHTML = opic.x2-opic.x1;
	    $("h_b").innerHTML = opic.y2-opic.y1;
    }
    function setFrmSize(){
        var win = top || parent;
        if(!win) return;
        var frm = win.document.getElementById("head_frm");
        if(!frm) return;
        var div = $("opic").parentNode;
        var w = div.offsetWidth+4;
        var h = div.offsetHeight+20;
        if(w>660) w = 674;
        frm.style.width = w+"px";
        frm.style.height = h+"px";
    }
    function setDiv(){
        var e = $("opic");
        var w = e.offsetWidth+2;
        var h = e.offsetHeight+2;
        if(w>660){ w = 670; }
        if(h>600) { h = 610; }
        e.parentNode.style.width = w+'px';
        e.parentNode.style.height = h+'px';
        
        setFrmSize();
    }
    Event.observe(window,'load',function(){
            new Cropper.ImgWithPreview('opic',{minWidth:120,minHeight:90,ratioDim:{x:10,y:10},displayOnInit:true,onEndCrop:onEndCrop,previewWrap:'preview'});
	    });		
    </script>
</head>
<body>
    <div id="preview" style="display:none;"></div>
    <div style="margin-bottom:4px; padding-left:6px;">裁剪宽度：<b id="w_b"></b> &nbsp; 高度：<b id="h_b"></b></div>
    <div style="overflow:auto;border:solid 1px #ccc;"><img src="/upload/photo/<%=DLL.CKUser.Username %>-b.jpg?<%=new Random().Next(1,99).ToString() %>" id="opic" onload="setDiv()" /></div>
    <form action="/ajax/uploadfile.aspx" method="post">
    <input type="hidden" id="cutimg" name="cutimg" value="1" />
    <input type="hidden" id="x1" name="x1" />
    <input type="hidden" id="y1" name="y1" />
    <input type="hidden" id="x2" name="x2" />
    <input type="hidden" id="y2" name="y2" />
    </form>
</body>
</html>
