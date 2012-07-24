﻿var topmonth, toptotal;
var topic_new, topic_hot;

var _init = function(){

    topweek = el("top20box").innerHTML;
    topic_new = el("huati_div").innerHTML;
    
    startTopMarquee();
    startRightMarquee();
    setLeftOver();
    setRightOver();
};
window.onload = _init;

function startTopMarquee()
{
    var speed = 5;
    var sp = el("sp_marq");

    if(sp.scrollWidth<sp.offsetWidth+2) return;
    sp.innerHTML += sp.innerHTML;
    var Marquee = function(){
        if(sp.offsetWidth+sp.scrollLeft>=sp.scrollWidth){
            sp.scrollLeft = sp.scrollWidth/2-sp.offsetWidth;
        }
        sp.scrollLeft += 1;
        if(i++>400) clearInterval(inter);
    };
    var i;
    var inter;
    setInterval(function(){i=0;inter=setInterval(Marquee, speed);}, 12000);
}
function startRightMarquee()
{
    var speed = 5;
    var sp = el("join_div");

    if(sp.scrollHeight<sp.offsetHeight+2) return;
    sp.innerHTML += sp.innerHTML;
    var Marquee = function(){
        if(sp.offsetHeight+sp.scrollTop>=sp.scrollHeight){
            sp.scrollTop = sp.scrollHeight/2-sp.offsetHeight;
        }
        sp.scrollTop += 1;
        if(i++>190) clearInterval(inter);
    };
    var i;
    var inter;
    setInterval(function(){i=0;inter=setInterval(Marquee, speed);}, 10000);
}

function setLeftOver()
{
    var divs = els("leftdiv", "div");
    for(var i in divs){
        if(divs[i].className=="groupbox"){
            divs[i].onmouseover = function(){
                this.style.backgroundColor = "#f0f0f0";
            }
            divs[i].onmouseout = function(){
                this.style.backgroundColor = "#ffffff";
            }
        }
    }
}

var node;
function setRightOver()
{
    var div = el("top20box").firstChild;
    while(true){
        div.onmouseover = function(){
            rightOver(this);
        };
        div = div.nextSibling;
        if(div.className=="clear") break;
    }
    
    rightOver(el("top20box").firstChild);
    el("top20box").firstChild.style.backgroundColor = "#cc6323";
    node = null;
    rightOver(el("top20box").childNodes[2]);
    
}
function rightOver(e)
{
    if(node){
        if(node.nextSibling) node.nextSibling.style.display = "";
        el("top20box").removeChild(node);
    }
    node = document.createElement("div");
    node.className = "top20itembig";
    node.innerHTML = "<img src='/upload/group/"+e.lang+"-s.jpg' onerror='this.src=\"/upload/group/nophoto-s.jpg\";' />"+e.childNodes[1].innerHTML+"<br />热度："+e.childNodes[2].innerHTML+"<b>"+e.childNodes[0].innerHTML+"</b><div class='clear'></div>";
    el("top20box").insertBefore(node, e);
    
    e.style.display = "none";
}

function showtop(e, i, catid)
{
    if(e.className=="gtoptab curr") return;
    e.className = "gtoptab curr";
    if(node){
        el("top20box").removeChild(node);
        node = null;
    }
    el("top20box").removeChild(el("top20box").firstChild);
    if(i==1){
        e.nextSibling.className = "gtoptab";
        if(topmonth){
            el("top20box").innerHTML = topmonth;
            setRightOver();
        }else{
            getTopGroup("month", catid);
        }
    }else{
        e.previousSibling.className = "gtoptab";
        if(toptotal){
            el("top20box").innerHTML = toptotal;
            setRightOver();
        }else{
            getTopGroup("total", catid);
        }
    }
}
function getTopGroup(k, catid)
{
    var url = dataURL +"?gettopgroup="+k;
    if(catid) url += "&cat="+ catid;
    var req = getAjax();
    req.open("GET", url, true);
    req.onreadystatechange = function(){
        if(req.readyState==4){
            var re = req.responseText;
            if(k=="total") toptotal = re;
            else topmonth = re;
            el("top20box").innerHTML = re;
            setRightOver();
        }
    };
    req.send(null);
}

function showhuati(e, i, catid)
{
    e.blur();
    if(i==1){
        if(e.parentNode.className == "huatitop") return;
        e.parentNode.className = "huatitop";
        if(topic_new) el("huati_div").innerHTML = topic_new;
        else getHuati("new", catid);
    }else{
        if(e.parentNode.className == "huatitop huatitop2") return;
        e.parentNode.className = "huatitop huatitop2";
        if(topic_hot) el("huati_div").innerHTML = topic_hot;
        else getHuati("hot", catid);
    }
}
function getHuati(k, catid)
{
    var url = dataURL +"?gethuati="+k;
    if(catid) url += "&cat="+ catid;
    var req = getAjax();
    req.open("GET", url, true);
    req.onreadystatechange = function(){
        if(req.readyState==4){
            var re = req.responseText;
            if(k=="new") topic_new = re;
            else topic_hot = re;
            el("huati_div").innerHTML = re;
        }
    };
    req.send(null);
}

function joinGroup(e,id)
{
}