<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="CQUTMyXueba.background.admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>后台管理</title>
    <link rel="stylesheet" href="css/admin.css" />
    <script src="js/jquery.js" type="text/javascript"></script>
</head>
<body>
    <div id="add" class="main">
        <a href="addperson.aspx">点击这里</a>,添加一个新的候选人。
    </div>
    <div id="view" class="main">
        <table id="usertable">


        </table>
    </div>
    <script>
        function clearTable() {
            $("#usertable").html("");
        }

        function addStudent(id, name, title, intro, type) {
            var ta = (type == 1) ? "学霸候选人" : "未来学霸";
            $("#usertable").html($("#usertable").html() + '<tr><td class="userid">' + id + '</td><td class="name">' + name + '</td><td class="title">' + title + '</td><td class="intro">' + intro + '</td><td class="type">' + ta + '</td><td class="opr"><a onclick="remove(' + id + ')">删除</a></td></tr>');
        }

        function loadTable() {
            $.ajax({
                type: "get",
                url: "/openapi/web.aspx?action=getperson",
                success: function (data) {
                    
                    data = data.replace(" ", "").replace("\n", "");
                    var res = eval("(" + data + ")");
                    if (res.msg == "ok") {
                        clearTable();
                        var count = res.count;
                        for(var i=0;i<count;i++) {
                            var person = res.person[i];
                            var name = person.name;
                            var id = person.id;
                            var intro = person.intro;
                            var title = person.title;
                            var type = person.type;
                            addStudent(id, name, title, intro, type);
                        }
                    } else {
                        alert("加载数据表出现错误，提示内容" + res.msg);
                    }
                }
            });
        }

        function remove(id) {
            if (id == null || id == "") {
                alert("error");
                return;
            }
            $.ajax({
                type: "get",
                url: "/openapi/web.aspx?action=remove&id=" + id,
                success: function (data) {
                    var d = eval("(" + data + ")");
                    if (d.msg == "ok") {
                        alert("成功删除！");
                    } else {
                        aleer("删除失败，错误内容："+d.msg);
                    }
                }
            });
        }

        $(document).ready(function () {
            loadTable();

        });
    </script>
</body>
</html>
