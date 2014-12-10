/***
ad.  cfc love you
Rube love you
**/
flag = 0;
function getXMLHttpRequest() {
    var createXHR;
    if (window.XMLHttpRequest) {
        createXHR = new XMLHttpRequest();
    }
    else {
        createXHR = new ActiveXObject("Microsoft.XMLHTTP");
    }
    return createXHR;
}

function showTips(id) {
    document.getElementById("tipS").style.display = "block";
    var Jsonhttp = getXMLHttpRequest();
    Jsonhttp.onreadystatechange = function () {
        if (Jsonhttp.readyState == 4 && Jsonhttp.status == 200) {
            /* 请修改这里的内容
             */
            result = eval("(" + Jsonhttp.responseText + ")");
            if (result.msg == "ok") {
                flag = 1;
                document.getElementById("tipcontent").innerHTML = "点赞成功";
            }
            else
                document.getElementById("tipcontent").innerHTML = "点赞失败，每天只有一次哦~";
        }
    }
    Jsonhttp.open("GET", "/openapi/web.aspx?action=vote&id="+id, true);
    Jsonhttp.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
    Jsonhttp.send();
}
function closeTips() {
    document.getElementById("tipS").style.display = "none";
    document.getElementById("tipcontent").innerHTML = "正在点赞...";
    if(flag == 1)
                location.href = "/";
}