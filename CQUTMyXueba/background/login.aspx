<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="CQUTMyXueba.background._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>后台管理-登陆</title>
    <link rel="stylesheet" href="css/login.css" />
    <script src="js/jquery.js" type="text/javascript"></script>
</head>
<body>
    <table>
        <tr><td class="info">用户名</td><td class="val"><input id="name" type="text" maxlength="20"/></td></tr>
        <tr><td class="info">密码</td><td class="val"><input id="password" type="password" maxlength="32" /></td></tr>
        <tr><td class="info">验证码</td><td class="val"><input type="text" id="check" maxlength="5" /></td><td><img src="/checkcode.aspx" /></td></tr>
        <tr><td><button id="login">登陆</button></td></tr>
    </table>
    <script>
        $(document).ready(function () {
            function clear(data){
                return data.replace(" ","").replace("*","");
            }

            function validate() {
                var name =clear( $("#name").val());
                var password = clear($("#password").val());
                var check = clear($("#check").val());

                return name.length >= 3 && password.length >= 6 && check.length >= 4;
            }

            $("#login").click(function () {
                if (validate()) {
                    var name = clear($("#name").val());
                    var password = clear($("#password").val());
                    var check = clear($("#check").val());

                    $.ajax({
                        type: "post",
                        url: "/openapi/web.aspx?action=login",
                        data: "name=" + name + "&password=" + password + "&check=" + check,
                        success: function (data) {
                            var res = eval("(" + data + ")");
                            switch (res.msg) {
                                case "ok": location.href = res.page; break;
                                case "error-login": alert("登陆失败：验证错误"); break;
                                case "error-nopermission": alert("管理员设置了暂时禁止登陆"); break;
                                case "error-code": alert("登陆失败：验证码错误"); break;
                                default: alert("操作出现错误，错误内容：" + res.msg);
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(errorThrown);
                        }
                    });
                }
            });
        });
    </script>
</body>
</html>
