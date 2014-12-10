function showLeft(id) {
    var left1 = document.getElementById("showInfoLeft");
    left1.className = "showLeft";
    var iframe = document.getElementById("iframetest");
    iframe.src = "people.aspx?ID=" + id;
}
function hiddenLeft(id) {
    var left1 = document.getElementById("showInfoLeft");
    left1.className = "hiddenLeft";
}
function showRight(id) {
    var left1 = document.getElementById("showInfoRight");
    left1.className = "showRight";
    var iframe = document.getElementById("iframetest1");
    iframe.src = "people.aspx?ID=" + id;
}
function hiddenRight(id) {
    var left1 = document.getElementById("showInfoRight");
    left1.className = "hiddenRight";
}
function showL() {
    var left1 = document.getElementById("showInfoLeft");
    left1.className = "showLeft";
}
function hiddenL() {
    var left1 = document.getElementById("showInfoLeft");
    left1.className = "hiddenLeft";
}
function showR() {
    var left1 = document.getElementById("showInfoRight");
    left1.className = "showRight";
}
function hiddenR() {
    var left1 = document.getElementById("showInfoRight");
    left1.className = "hiddenRight";
}