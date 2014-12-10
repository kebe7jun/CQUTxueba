/***
ad. cfc love you
**/
function showLOAD() {
    var t = setTimeout(" fadeOut(iBase.Id('LOADING'), 80, 0)", 2000);
}
var start;
function updateImg() {

    if (document.readyState == "complete") {
        try {
            showLOAD();
            clearInterval(start);//执行成功，清除监听
        } catch (err) {
            return true;
        }
    }
}

window.onload = function () {
    if (document.all) {//判断是否是IE
        start = setInterval('updateImg()', 1000);
    } else {
        showLOAD();
    }

}
